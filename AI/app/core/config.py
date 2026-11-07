import os
from pathlib import Path

# Charge le .env à la racine du repo si présent
try:
    from dotenv import load_dotenv
    root_env = Path(__file__).resolve().parents[3] / ".env"
    local_env = Path(__file__).resolve().parents[2] / ".env"
    if root_env.exists():
        load_dotenv(root_env)
    elif local_env.exists():
        load_dotenv(local_env)
except Exception:
    pass


class Settings:
    GROQ_API_KEY = os.getenv("GROQ_API_KEY")
    INTERNAL_API_KEY = os.getenv("INTERNAL_API_KEY", "Kassah1234")
    MODEL_NAME = os.getenv("MODEL_NAME", "openai/gpt-oss-20b")
    CHROMA_HOST = os.getenv("CHROMA_SERVER_HOST", "localhost")
    CHROMA_PORT = os.getenv("CHROMA_SERVER_HTTP_PORT", "8000")


settings = Settings()

if not settings.GROQ_API_KEY:
    print("⚠️ GROQ_API_KEY manquante — le LLM ne pourra pas répondre.")
