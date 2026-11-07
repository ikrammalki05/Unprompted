from groq import Groq
from app.core.config import settings

client = Groq(api_key=settings.GROQ_API_KEY) if settings.GROQ_API_KEY else None

CANDIDATE_MODELS = [
    settings.MODEL_NAME,
    "openai/gpt-oss-20b",
    "openai/gpt-oss-120b",
    "qwen/qwen3.8-27b",
    "qwen/qwen3.6-27b",
]


class LLMChain:
    @staticmethod
    def generate_response(system_prompt: str, user_message: str, temperature: float):
        """Envoie la requête à Groq; fallback extractif si le LLM est indisponible."""
        last_error = None

        if client is not None:
            tried = []
            for model in CANDIDATE_MODELS:
                if not model or model in tried:
                    continue
                tried.append(model)
                try:
                    chat_completion = client.chat.completions.create(
                        messages=[
                            {"role": "system", "content": system_prompt},
                            {"role": "user", "content": user_message},
                        ],
                        model=model,
                        temperature=temperature,
                    )
                    response_text = chat_completion.choices[0].message.content
                    tokens = chat_completion.usage.total_tokens if chat_completion.usage else 0
                    return response_text, tokens
                except Exception as e:
                    last_error = e
                    print(f"⚠️ Modèle {model} indisponible: {e}")

        # Fallback : réponse basée sur le contexte déjà injecté dans system_prompt
        context = _extract_context(system_prompt)
        if context:
            excerpt = context[:1800].strip()
            answer = (
                "Voici une synthèse basée sur le cahier des charges indexé "
                "(mode secours sans LLM Groq) :\n\n"
                f"{excerpt}\n\n"
                f"Votre question : {user_message}"
            )
            return answer, 0

        msg = (
            "Impossible de générer une réponse IA pour le moment "
            f"(Groq: {last_error}). Vérifiez GROQ_API_KEY / modèle, "
            "et que le cahier des charges PDF a bien été indexé."
        )
        return msg, 0


def _extract_context(system_prompt: str) -> str:
    markers = ["CONTEXTE :", "Voici les informations tirées", "cahier des charges"]
    text = system_prompt or ""
    for marker in markers:
        idx = text.find(marker)
        if idx >= 0:
            return text[idx + len(marker) :].strip()
    return text.strip()
