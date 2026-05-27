import { useState } from 'react';

export const LoginForm = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Tentative de connexion avec :', { email, password });
  };

  return (
    // C'EST ICI LA MAGIE DU BLOC BLANC : bg-white, shadow-xl, rounded-2xl
    <div className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8 sm:p-10">
      
      <h2 className="text-3xl font-semibold mb-2 text-gray-900">Connexion</h2>
      <p className="text-gray-500 mb-8 text-sm">Accédez à votre espace de travail académique.</p>

      <form onSubmit={handleSubmit} className="space-y-5">
        
        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-1.5">Email</label>
          <div className="relative">
            <input 
              type="email" 
              placeholder="nom@universite.fr" 
              className="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-[#a6c1ee] focus:border-[#a6c1ee] outline-none transition-all bg-gray-50"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <span className="absolute left-3 top-3.5 text-gray-400">✉️</span> 
          </div>
        </div>

        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-1.5">Mot de passe</label>
          <div className="relative">
            <input 
              type="password" 
              placeholder="••••••••" 
              className="w-full pl-10 pr-10 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-[#a6c1ee] focus:border-[#a6c1ee] outline-none transition-all bg-gray-50"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <span className="absolute left-3 top-3.5 text-gray-400">🔒</span>
            <span className="absolute right-3 top-3.5 text-gray-400 cursor-pointer">👁️</span>
          </div>
        </div>

        <div className="flex items-center justify-between mt-2">
          <div className="flex items-center">
            <input type="checkbox" id="remember" className="h-4 w-4 text-blue-600 rounded border-gray-300" />
            <label htmlFor="remember" className="ml-2 block text-sm text-gray-600">Se souvenir de moi</label>
          </div>
          <a href="#" className="text-sm font-semibold text-[#0066cc] hover:underline">Mot de passe oublié ?</a>
        </div>

        <button 
          type="submit" 
          className="w-full flex justify-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-bold text-white bg-[#021124] hover:bg-gray-800 transition-colors mt-6"
        >
          Se connecter
        </button>
      </form>

      <div className="mt-10 flex flex-col items-center gap-3 text-[10px] font-bold text-gray-400 tracking-widest text-center">
        <a href="#" className="hover:text-gray-600 transition-colors">POLITIQUE DE CONFIDENTIALITÉ</a>
        <a href="#" className="hover:text-gray-600 transition-colors">CONDITIONS D'UTILISATION</a>
        <a href="#" className="hover:text-gray-600 transition-colors">SUPPORT ACADÉMIQUE</a>
      </div>
    </div>
  );
};