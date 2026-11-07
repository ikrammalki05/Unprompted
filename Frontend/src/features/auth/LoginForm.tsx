import { useState } from 'react';
import { keycloak } from '../../services/keycloak';

/**
 * Écran React de secours / pré-auth.
 * La saisie email/mot de passe réelle se fait sur le thème Keycloak (login.ftl),
 * identique à la maquette merge — on ne simule pas d'auth locale.
 */
export const LoginForm = () => {
  const [busy, setBusy] = useState(false);

  const goLogin = () => {
    if (busy) return;
    setBusy(true);
    keycloak.login({ redirectUri: `${window.location.origin}/` });
  };

  return (
    <div className="w-full max-w-[400px]">
      <div className="mb-10 lg:hidden">
        <div className="bg-[#05162b] text-white px-4 py-2 inline-block rounded-sm mb-8">
          <span className="font-black tracking-tighter">UNPROMPTED</span>
        </div>
      </div>

      <div className="mb-10">
        <h2 className="text-4xl font-extrabold mb-3 text-slate-900 tracking-tight">Connexion</h2>
        <p className="text-slate-500 font-medium">Accédez à votre espace de travail académique.</p>
      </div>

      <button
        type="button"
        onClick={goLogin}
        disabled={busy}
        className="w-full py-4 px-6 bg-black text-white rounded-xl font-black text-sm hover:bg-slate-800 active:scale-[0.98] transition-all shadow-xl shadow-black/10 disabled:opacity-70"
      >
        {busy ? 'Redirection…' : 'Se connecter'}
      </button>

      <div className="mt-16 flex flex-col items-center gap-4 text-[10px] font-black text-slate-400 tracking-[0.15em] text-center">
        <span>POLITIQUE DE CONFIDENTIALITÉ</span>
        <span>CONDITIONS D&apos;UTILISATION</span>
        <span>SUPPORT ACADÉMIQUE</span>
      </div>
    </div>
  );
};
