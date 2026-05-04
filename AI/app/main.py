from fastapi import FastAPI, UploadFile, File, Form
from app.models.schemas import ChatRequest, ChatResponse
from app.engine.vectordb import VectorDBManager
from app.services.processor import DocumentProcessor
import os
from groq import Groq # <-- La nouveauté est ici
app = FastAPI(title="Unprompted AI Governance Service")

db_manager = VectorDBManager()
processor = DocumentProcessor()

# ---------------------------------------------------------
# CONFIGURATION SÉCURISÉE DE L'IA
# ---------------------------------------------------------
# On récupère la clé depuis l'environnement Docker
GROQ_API_KEY = os.getenv("GROQ_API_KEY")

# Bonne pratique : faire planter l'application au démarrage si la clé manque
if not GROQ_API_KEY:
    raise ValueError("⚠️ ERREUR FATALE : La variable d'environnement GROQ_API_KEY est introuvable !")

client = Groq(api_key=GROQ_API_KEY)
MODEL_NAME = "llama-3.1-8b-instant"


@app.get("/health")
async def health_check():
    return {"status": "healthy", "service": "ai-governance-service"}

@app.post("/upload")
async def upload_pdf(project_id: str = Form(...), file: UploadFile = File(...)):
    temp_file_path = f"temp_{file.filename}"
    with open(temp_file_path, "wb") as buffer:
        buffer.write(await file.read())
        
    chunks = processor.process_pdf(temp_file_path)
    
    if chunks:
        db_manager.add_documents(project_id, chunks)
        os.remove(temp_file_path)
        return {"status": "success", "message": f"✅ {len(chunks)} morceaux ajoutés au projet {project_id} !"}
    
    os.remove(temp_file_path)
    return {"status": "error", "message": "Impossible de lire le document."}


# ---------------------------------------------------------
# LE COEUR DU RAG : La nouvelle route /ask
# ---------------------------------------------------------
@app.post("/ask", response_model=ChatResponse)
async def ask_ai(request: ChatRequest):
    
    # 1. La Recherche (Les Oreilles)
    context = db_manager.search_context(
        project_id=request.project_id, 
        query=request.message
    )
    
    # Si on ne trouve rien dans le PDF, on refuse de répondre ! (Gouvernance)
    if not context:
        return ChatResponse(
            response="Je suis désolé, mais je ne trouve pas la réponse à cette question dans le cahier des charges de votre projet.",
            tokens_used=0,
            dependency_score=0.0,
            status="success"
        )

    # 2. Le Prompt Système (Les Règles du jeu)
    system_prompt = f"""
    Tu es un assistant IA strict et professionnel pour des étudiants ingénieurs.
    Tu dois répondre à la question de l'étudiant en utilisant UNIQUEMENT le contexte fourni ci-dessous.
    Si la réponse n'est pas dans le contexte, dis que tu ne sais pas.
    Ne donne pas d'informations extérieures.
    
    CONTEXTE EXTRAIT DU CAHIER DES CHARGES :
    {context}
    """

    # 3. L'appel à l'IA (Le Cerveau)
    chat_completion = client.chat.completions.create(
        messages=[
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": request.message}
        ],
        model=MODEL_NAME,
        temperature=0.2, # Température basse = réponses précises et moins créatives
    )

    # 4. On récupère la réponse formulée par Llama 3
    ai_final_response = chat_completion.choices[0].message.content

    return ChatResponse(
        response=ai_final_response,
        tokens_used=0, # On pourra calculer les tokens plus tard
        dependency_score=1.0, 
        status="success"
    )