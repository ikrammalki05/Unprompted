import chromadb
import os
from pathlib import Path


class VectorDBManager:
    def __init__(self):
        # Mode local (sans Docker) : PersistentClient
        # Mode Docker : HttpClient vers chroma-db
        use_local = os.getenv("CHROMA_LOCAL", "1") == "1"
        host = os.getenv("CHROMA_SERVER_HOST", "chroma-db")

        if use_local or host in ("localhost", "127.0.0.1"):
            persist_dir = os.getenv(
                "CHROMA_PERSIST_DIR",
                str(Path(__file__).resolve().parents[2] / "chroma_data"),
            )
            Path(persist_dir).mkdir(parents=True, exist_ok=True)
            self.client = chromadb.PersistentClient(path=persist_dir)
        else:
            port = int(os.getenv("CHROMA_SERVER_HTTP_PORT", "8000"))
            self.client = chromadb.HttpClient(host=host, port=port)

        self.collection = self.client.get_or_create_collection(name="cahiers_charges")

    def add_documents(self, project_id: str, chunks):
        ids = [f"{project_id}_{i}" for i in range(len(chunks))]
        documents = [chunk.page_content for chunk in chunks]
        metadatas = [{"project_id": str(project_id)} for _ in chunks]

        # Upsert-friendly: delete old chunks for this project first
        try:
            existing = self.collection.get(where={"project_id": str(project_id)})
            if existing and existing.get("ids"):
                self.collection.delete(ids=existing["ids"])
        except Exception:
            pass

        self.collection.add(
            ids=ids,
            documents=documents,
            metadatas=metadatas,
        )
        print(f"✅ {len(chunks)} morceaux ajoutés pour le projet {project_id}")

    def search_context(self, project_id: str, query: str, n_results: int = 10):
        try:
            results = self.collection.query(
                query_texts=[query],
                n_results=n_results,
                where={"project_id": str(project_id)},
            )
            if results.get("documents") and results["documents"][0]:
                return " ".join(results["documents"][0])
        except Exception as e:
            print(f"Erreur recherche Chroma: {e}")
        return ""
