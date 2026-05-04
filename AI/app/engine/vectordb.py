import chromadb
import os

class VectorDBManager:
    def __init__(self):
        # On récupère les infos de connexion définies dans docker-compose
        self.host = os.getenv("CHROMA_SERVER_HOST", "chroma-db")
        self.port = os.getenv("CHROMA_SERVER_HTTP_PORT", "8000")
        
        # On se connecte au serveur ChromaDB
        self.client = chromadb.HttpClient(host=self.host, port=self.port)
        
        # On crée ou récupère la collection (comme une table en SQL)
        self.collection = self.client.get_or_create_collection(name="cahiers_charges")

    # ---- NOUVELLE FONCTION À AJOUTER ICI ----
    def add_documents(self, project_id: str, chunks):
        """
        Stocke les morceaux de texte dans ChromaDB avec le project_id
        """
        # On prépare les données pour ChromaDB
        ids = [f"{project_id}_{i}" for i in range(len(chunks))]
        documents = [chunk.page_content for chunk in chunks]
        metadatas = [{"project_id": project_id} for _ in chunks]
        
        # Ajout effectif dans la base vectorielle
        self.collection.add(
            ids=ids,
            documents=documents,
            metadatas=metadatas
        )
        print(f"✅ {len(chunks)} morceaux ajoutés pour le projet {project_id}")
    # ------------------------------------------

    def search_context(self, project_id: str, query: str, n_results: int = 10):
        """
        Cherche les extraits les plus pertinents pour un projet précis.
        """
        results = self.collection.query(
            query_texts=[query],
            n_results=n_results,
            # C'est ici qu'on filtre par project_id pour ne pas mélanger les projets
            where={"project_id": project_id} 
        )
        
        # On retourne juste les textes trouvés (le contexte)
        if results['documents']:
            return " ".join(results['documents'][0])
        return ""