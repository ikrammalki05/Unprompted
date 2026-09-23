import os

class Settings:
    GROQ_API_KEY = os.getenv("GROQ_API_KEY")
    INTERNAL_API_KEY = os.getenv("INTERNAL_API_KEY", "un_mot_de_passe_super_secret_pfe_2026")
    # Paramètres du modèle
    MODEL_NAME = "llama-3.1-8b-instant"
    
    # Paramètres de la base vectorielle (pour vectordb.py plus tard)
    CHROMA_HOST = os.getenv("CHROMA_SERVER_HOST", "chroma-db")
    CHROMA_PORT = os.getenv("CHROMA_SERVER_HTTP_PORT", "8000")

settings = Settings()

# Sécurité au démarrage
if not settings.GROQ_API_KEY:
    raise ValueError("⚠️ ERREUR FATALE : La variable d'environnement GROQ_API_KEY est introuvable !")