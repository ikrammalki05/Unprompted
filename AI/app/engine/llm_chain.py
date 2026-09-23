from groq import Groq
from app.core.config import settings

# Initialisation du client avec la configuration
client = Groq(api_key=settings.GROQ_API_KEY)

class LLMChain:
    @staticmethod
    def generate_response(system_prompt: str, user_message: str, temperature: float):
        """Envoie la requête à Groq et retourne la réponse et les tokens utilisés."""
        chat_completion = client.chat.completions.create(
            messages=[
                {"role": "system", "content": system_prompt},
                {"role": "user", "content": user_message}
            ],
            model=settings.MODEL_NAME,
            temperature=temperature,
        )
        
        response_text = chat_completion.choices[0].message.content
        tokens = chat_completion.usage.total_tokens if chat_completion.usage else 0
        
        return response_text, tokens