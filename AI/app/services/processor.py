from langchain_community.document_loaders import PyPDFLoader
from langchain_text_splitters import RecursiveCharacterTextSplitter

class DocumentProcessor:
    def __init__(self):
        # On définit comment on découpe le texte
        # chunk_size: 1000 caractères par morceau
        # chunk_overlap: 100 caractères de répétition pour ne pas couper au milieu d'une info importante
        self.text_splitter = RecursiveCharacterTextSplitter(
            chunk_size=1000, 
            chunk_overlap=100
        )

    def process_pdf(self, file_path: str):
        """Prend un chemin de fichier et retourne une liste de chunks"""
        try:
            loader = PyPDFLoader(file_path)
            documents = loader.load()
            
            # Découpage en petits morceaux
            chunks = self.text_splitter.split_documents(documents)
            return chunks
        except Exception as e:
            print(f"Erreur lors du traitement du PDF: {e}")
            return []