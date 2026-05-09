import React from 'react';
import illustration from '../assets/noprompt.svg';
import logo from '../assets/file.svg';

interface AuthLayoutProps {
  children: React.ReactNode;
}

export const AuthLayout = ({ children }: AuthLayoutProps) => {
  return (
    // 1. h-screen remplace min-h-screen pour verrouiller la hauteur. overflow-hidden empêche le scroll global.
    <div className="flex h-screen w-full font-sans bg-gray-50 overflow-hidden">

      {/* Colonne Gauche */}
      <div className="hidden lg:flex w-1/2 bg-[#021124] text-white p-8 xl:p-12 flex-col h-full">

        {/* 1. Header (Logo centré) */}
        {/* flex justify-center centre le contenu horizontalement */}
        <div className="w-20 h-20 rounded-full bg-white flex item-center justify-center p-3 shadow-xl">
          <img 
            src={logo} 
            alt="Logo Unprompted" 
            // h-16 définit la hauteur (tu peux tester h-12, h-20, etc. selon la taille de ton SVG)
            // w-auto permet de garder les bonnes proportions sans déformer l'image
            className="max-h-full w-auto object-contain" 
          />
        </div>

        {/* 2. Centre (Texte par-dessus l'Image) */}
        {/* On utilise 'relative' ici pour que l'image absolute reste bloquée dans ce conteneur */}
        <div className="flex-1 relative flex flex-col justify-center min-h-0 mt-8 mb-8">

          {/* 🖼️ L'IMAGE EN ARRIÈRE-PLAN (Sous l'écriture) */}
          <div className="absolute inset-0 z-0 flex items-center justify-center">
            <img 
              src={illustration} 
              alt="Illustration" 
              // object-cover force l'image à remplir TOUT l'espace
              // opacity-40 (ou 50, 60...) permet de baisser la luminosité de l'image pour pouvoir lire le texte
              className="w-full h-full object-cover object-center opacity-50 scale-110"
            />
          </div>

          {/* ✍️ LE TEXTE PAR DESSUS (Au premier plan) */}
          {/* relative et z-10 forcent ce texte à s'afficher au-dessus de l'image */}
          <div className="relative z-10 pointer-events-none">
            <h1 className="text-4xl xl:text-5xl font-bold mb-4 leading-tight drop-shadow-lg">
              Bienvenue sur <br />
              <span className="text-[#a6c1ee]">Unprompted</span>
            </h1>
            <p className="text-base text-gray-200 max-w-md leading-relaxed drop-shadow-md">
              Gérez vos projets académiques avec efficacité. Une plateforme sécurisée pour l'excellence dans la recherche et la gouvernance IA.
            </p>
          </div>

        </div>

        {/* 3. Footer (Statistiques) */}
        <div className="flex-none flex gap-8 xl:gap-12">
          <div>
            <p className="text-2xl xl:text-3xl font-bold">99.9%</p>
            <p className="text-xs text-gray-400 mt-1">Disponibilité</p>
          </div>
          <div>
            <p className="text-2xl xl:text-3xl font-bold">SEC-2</p>
            <p className="text-xs text-gray-400 mt-1">Conformité</p>
          </div>
          <div>
            <p className="text-2xl xl:text-3xl font-bold">AI-Ready</p>
            <p className="text-xs text-gray-400 mt-1">Infrastructure</p>
          </div>
        </div>

      </div>

      {/* Colonne Droite - Centrage parfait du formulaire */}
      {/* overflow-y-auto ajouté ici au cas où le formulaire devrait scroller sur de très petits écrans */}
      <div className="w-full lg:w-1/2 flex flex-col justify-center items-center p-4 sm:p-8 h-full overflow-y-auto">
        {children}
      </div>

    </div>
  );
};