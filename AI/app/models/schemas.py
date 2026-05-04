from pydantic import BaseModel
from typing import Optional

# Ce que le Backend envoie pour poser une question
class ChatRequest(BaseModel):
    student_id: str
    project_id: str
    message: str
    # Le niveau d'accès défini par le prof (unlimited, limited, restricted)
    access_level: str 

# La réponse que ton service IA renvoie
class ChatResponse(BaseModel):
    response: str
    tokens_used: int
    dependency_score: float
    status: str