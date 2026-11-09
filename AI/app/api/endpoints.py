from fastapi import APIRouter, UploadFile, File, Form,Depends 
from app.models.schemas import ChatRequest, ChatResponse
from app.core.security import verify_service_key
from app.engine.vectordb import VectorDBManager
from app.services.processor import DocumentProcessor
from app.engine.llm_chain import LLMChain
from app.services.policy_rules import GovernancePolicy
import os

# Création du routeur
router = APIRouter()

db_manager = VectorDBManager()
processor = DocumentProcessor()

@router.get("/health")
async def health_check():
    return {"status": "healthy", "service": "ai-governance-service"}

@router.post("/upload", dependencies=[Depends(verify_service_key)])
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

@router.post("/ask", response_model=ChatResponse, dependencies=[Depends(verify_service_key)])
async def ask_ai(request: ChatRequest):
    
    # 1. Recherche du contexte
    context = db_manager.search_context(project_id=request.project_id, query=request.message)
    
    # 2. Application de la Gouvernance stricte
    if request.access_level == "restreint" and not context:
        return ChatResponse(
            response="🔒 [Accès Restreint] Je suis désolé, mais je ne trouve pas la réponse strictement dans le cahier des charges.",
            tokens_used=0,
            dependency_score=0.0,
            status="success"
        )
        
    # 3. Récupération des règles via policy_rules.py
    system_prompt, temperature = GovernancePolicy.get_prompt_and_temperature(request.access_level, context)
    
    if not system_prompt:
        return ChatResponse(response="❌ Erreur : Niveau d'accès inconnu.", tokens_used=0, dependency_score=0.0, status="error")

    # 4. Génération de la réponse via llm_chain.py
    response_text, tokens = LLMChain.generate_response(system_prompt, request.message, temperature)

    return ChatResponse(
        response=response_text,
        tokens_used=tokens, 
        dependency_score=1.0 if context else 0.0,
        status="success"
    )