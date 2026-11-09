import os
from fastapi import FastAPI
from app.api.endpoints import router

# Préchargement modèle Chroma optionnel (désactivé par défaut en local Windows)
if os.getenv("CHROMA_PRELOAD", "0") == "1":
    try:
        import urllib.request
        from pathlib import Path

        print("⏳ Vérification du modèle d'IA ChromaDB...")
        url = "https://chroma-onnx-models.s3.amazonaws.com/all-MiniLM-L6-v2/onnx.tar.gz"
        dest_dir = Path.home() / ".cache" / "chroma" / "onnx_models" / "all-MiniLM-L6-v2"
        dest_dir.mkdir(parents=True, exist_ok=True)
        dest_file = dest_dir / "onnx.tar.gz"
        if not dest_file.exists() or dest_file.stat().st_size < 79_000_000:
            print("📥 Téléchargement du modèle Chroma...")
            urllib.request.urlretrieve(url, dest_file)
            print("✅ Téléchargement terminé")
        else:
            print("✅ Modèle déjà présent")
    except Exception as e:
        print(f"⚠️ Préchargement Chroma ignoré: {e}")

app = FastAPI(title="Unprompted AI Governance Service")
app.include_router(router)


@app.get("/")
def root():
    return {"service": "unprompted-ai", "status": "up"}
