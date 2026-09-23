import { keycloak } from '../../services/keycloak'; // Ajuste le chemin

export const UnauthorizedPage = () => {

    // Fonction pour détruire la session et revenir à l'accueil
    const handleLogout = () => {
        keycloak.logout({
            redirectUri: window.location.origin // Le ramène à la racine après déconnexion
        });
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-[#f8fafc] p-4">
            <div className="bg-white p-10 rounded-2xl shadow-sm border border-[#f1f5f9] max-w-md w-full text-center flex flex-col items-center gap-4">

                {/* Icône d'alerte */}
                <div className="w-16 h-16 bg-[#fee2e2] text-[#ef4444] rounded-full flex items-center justify-center mb-2">
                    <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z" />
                        <line x1="12" y1="9" x2="12" y2="13" />
                        <line x1="12" y1="17" x2="12.01" y2="17" />
                    </svg>
                </div>

                <h1 className="text-2xl font-extrabold text-[#1e293b]">Accès Refusé</h1>

                <p className="text-[14.5px] text-[#64748b] leading-relaxed mb-4">
                    Votre compte ne dispose d'aucun rôle actif (Administrateur, Enseignant ou Étudiant) pour accéder à cette application. Veuillez contacter le support technique.
                </p>

                {/* Le bouton salvateur */}
                <button
                    onClick={handleLogout}
                    className="w-full bg-[#1e293b] text-white p-3 rounded-xl font-bold shadow-sm hover:bg-[#0f172a] transition-all"
                >
                    Se déconnecter et changer de compte
                </button>

            </div>
        </div>
    );
};