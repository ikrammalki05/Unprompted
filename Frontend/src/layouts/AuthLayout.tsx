import React from 'react';

interface AuthLayoutProps {
  children: React.ReactNode;
}

export const AuthLayout = ({ children }: AuthLayoutProps) => {
  return (
    <div className="flex h-screen w-full font-sans bg-white overflow-hidden">
      {/* Left branding — aligned with Keycloak theme / merge demo */}
      <div className="hidden lg:flex w-[55%] bg-[#05162b] text-white p-12 xl:p-20 flex-col justify-between h-full relative overflow-hidden">
        <div className="absolute inset-0 opacity-20 pointer-events-none">
          <div className="absolute top-[-10%] right-[-10%] w-96 h-96 bg-blue-500 rounded-full blur-[120px]" />
          <div className="absolute bottom-[-20%] left-[-10%] w-80 h-80 bg-indigo-600 rounded-full blur-[100px]" />
        </div>

        <div className="relative z-10">
          <div className="mb-24">
            <div className="bg-white px-5 py-3 inline-block rounded-sm shadow-xl">
              <span className="text-[#05162b] font-black text-xl tracking-tighter">UNPROMPTED</span>
            </div>
          </div>

          <div className="max-w-xl">
            <h1 className="text-6xl xl:text-7xl font-black mb-8 leading-[1.1] tracking-tight">
              Bienvenue sur <br />
              <span className="text-[#7eaefd]">Unprompted</span>
            </h1>
            <p className="text-lg text-slate-300 max-w-md leading-relaxed font-medium">
              Gérez vos projets académiques avec efficacité. Une plateforme sécurisée pour
              l&apos;excellence dans la recherche et la gouvernance IA.
            </p>
          </div>
        </div>

        <div className="relative z-10 flex gap-12 xl:gap-16">
          <div>
            <p className="text-3xl xl:text-4xl font-black tracking-tight">99.9%</p>
            <p className="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">
              Disponibilité
            </p>
          </div>
          <div>
            <p className="text-3xl xl:text-4xl font-black tracking-tight">SEC-2</p>
            <p className="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">
              Conformité
            </p>
          </div>
          <div>
            <p className="text-3xl xl:text-4xl font-black tracking-tight">AI-Ready</p>
            <p className="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">
              Infrastructure
            </p>
          </div>
        </div>
      </div>

      <div className="w-full lg:w-[45%] flex flex-col justify-center items-center p-8 h-full overflow-y-auto bg-white">
        {children}
      </div>
    </div>
  );
};
