import React from 'react';

interface ProfileLayoutProps {
  title: string;
  subtitle: string;
  actions?: React.ReactNode;
  aside: React.ReactNode;
  children: React.ReactNode;
  message?: { type: 'success' | 'error'; text: string } | null;
}

export const ProfileLayout: React.FC<ProfileLayoutProps> = ({
  title,
  subtitle,
  actions,
  aside,
  children,
  message,
}) => {
  return (
    <div className="p-6 md:p-8 pb-16 max-w-6xl mx-auto">
      <div className="mb-8 flex flex-col sm:flex-row sm:items-end sm:justify-between gap-4">
        <div>
          <h1 className="text-3xl md:text-4xl font-black text-slate-900 tracking-tight">{title}</h1>
          <p className="mt-2 text-slate-500 font-medium">{subtitle}</p>
        </div>
        {actions && <div className="flex flex-wrap items-center gap-3">{actions}</div>}
      </div>

      {message && (
        <div
          className={`mb-6 px-4 py-3 rounded-2xl text-sm font-semibold border ${
            message.type === 'success'
              ? 'bg-emerald-50 text-emerald-700 border-emerald-100'
              : 'bg-red-50 text-red-700 border-red-100'
          }`}
        >
          {message.text}
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-[1.6fr_0.9fr] gap-6 lg:gap-8">
        <section className="bg-white rounded-3xl border border-slate-100 shadow-sm p-6 md:p-8">
          {children}
        </section>
        <aside className="space-y-6">{aside}</aside>
      </div>
    </div>
  );
};

export default ProfileLayout;
