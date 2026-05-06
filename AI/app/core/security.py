from fastapi import Security, HTTPException, status
from fastapi.security.api_key import APIKeyHeader
from app.core.config import settings

# On définit le nom du "Badge" qu'on va chercher dans la requête
API_KEY_NAME = "X-Service-API-Key"

# FastAPI va chercher ce header automatiquement
api_key_header = APIKeyHeader(name=API_KEY_NAME, auto_error=False)

async def verify_service_key(api_key_header: str = Security(api_key_header)):
    """
    Vérifie que la requête possède la bonne clé API secrète.
    """
    if api_key_header == settings.INTERNAL_API_KEY:
        return api_key_header
    else:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="⛔ Accès refusé : Vous n'êtes pas le Backend C# autorisé."
        )