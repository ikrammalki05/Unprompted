import os
import urllib.request
from fastapi import FastAPI, UploadFile, File, Form
from app.api.endpoints import router

# --- LE HACK POUR BYPASSER LE TIMEOUT DE CHROMADB ---
def preload_chroma_model():
    print("⏳ Vérification du modèle d'IA ChromaDB...")
    url = "https://chroma-onnx-models.s3.amazonaws.com/all-MiniLM-L6-v2/onnx.tar.gz"
    dest_dir = "/root/.cache/chroma/onnx_models/all-MiniLM-L6-v2"
    os.makedirs(dest_dir, exist_ok=True)
    dest_file = os.path.join(dest_dir, "onnx.tar.gz")
    
    # Si le fichier n'existe pas ou s'il est incomplet (moins de 79 Mo)
    if not os.path.exists(dest_file) or os.path.getsize(dest_file) < 79000000:
        print("📥 Téléchargement manuel du modèle en cours (sans limite de temps)... Patience !")
        try:
            # urlretrieve attendra aussi longtemps que nécessaire, sans faire de Timeout !
            urllib.request.urlretrieve(url, dest_file)
            print("✅ Téléchargement terminé avec succès ! 🚀")
        except Exception as e:
            print(f"❌ Erreur lors du téléchargement: {e}")
    else:
        print("✅ Le modèle est déjà présent, tout est prêt !")

# On lance notre fonction de secours AVANT de démarrer le serveur
preload_chroma_model()
# ---------------------------------------------------------

app = FastAPI(title="Unprompted AI Governance Service")

# On inclut toutes les routes définies dans endpoints.py
app.include_router(router)