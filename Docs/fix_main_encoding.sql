UPDATE Fichier
SET contenu = N'# Projet 1 - Fondations Unprompted
def welcome(name: str) -> str:
    return f"Bienvenue {name} sur Unprompted"

if __name__ == "__main__":
    print(welcome("Ali"))
    print("Workspace operationnel.")
',
    language = N'python',
    last_modified = GETDATE()
WHERE id_projet = 1 AND nom = N'main';
