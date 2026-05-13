<!DOCTYPE html>
<html lang="fr" class="h-full">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>${msg("loginTitle",(realm.displayName!''))}</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800;900&display=swap" rel="stylesheet">
    <style>
        body { font-family: 'Inter', sans-serif; }
    </style>
</head>

<body class="flex h-screen w-full bg-white overflow-hidden">
    
    <!-- Left Side: Branding -->
    <div class="hidden lg:flex w-[55%] bg-[#05162b] text-white p-12 xl:p-20 flex-col justify-between h-full relative overflow-hidden">
        
        <!-- Background decorative elements (optional) -->
        <div class="absolute inset-0 opacity-10 pointer-events-none">
            <div class="absolute top-[-10%] right-[-10%] w-96 h-96 bg-blue-500 rounded-full blur-[120px]"></div>
        </div>

        <div class="relative z-10">
            <!-- Logo -->
            <div class="mb-24">
                <div class="bg-white p-4 inline-block rounded-sm shadow-xl">
                   <span class="text-[#05162b] font-black text-xl tracking-tighter italic">UNPROMPTED</span>
                </div>
            </div>
            
            <div class="max-w-xl">
                <h1 class="text-6xl xl:text-7xl font-black mb-8 leading-[1.1] tracking-tight">
                    Bienvenue sur <br />
                    <span class="text-[#7eaefd]">Unprompted</span>
                </h1>
                <p class="text-lg text-slate-300 max-w-md leading-relaxed font-medium">
                    Gérez vos projets académiques avec efficacité. Une plateforme sécurisée pour l'excellence dans la recherche et la gouvernance IA.
                </p>
            </div>
        </div>

        <!-- Stats Footer -->
        <div class="relative z-10 flex gap-12 xl:gap-16">
            <div>
                <p class="text-3xl xl:text-4xl font-black tracking-tight">99.9%</p>
                <p class="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">Disponibilité</p>
            </div>
            <div>
                <p class="text-3xl xl:text-4xl font-black tracking-tight">SEC-2</p>
                <p class="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">Conformité</p>
            </div>
            <div>
                <p class="text-3xl xl:text-4xl font-black tracking-tight">AI-Ready</p>
                <p class="text-[11px] text-slate-500 font-bold uppercase tracking-widest mt-2">Infrastructure</p>
            </div>
        </div>
    </div>

    <!-- Right Side: Login Form -->
    <div class="w-full lg:w-[45%] flex flex-col justify-center items-center p-8 h-full overflow-y-auto">
        <div class="w-full max-w-[400px]">
            
            <div class="mb-10">
                <h2 class="text-4xl font-extrabold mb-3 text-slate-900 tracking-tight">Connexion</h2>
                <p class="text-slate-500 font-medium">Accédez à votre espace de travail académique.</p>
            </div>

            <#if message?has_content && (message.type != 'warning' || !isAppInitiatedAction??)>
                <div class="mb-6 p-4 rounded-xl <#if message.type = 'success'>bg-green-50 text-green-700<#elseif message.type = 'warning'>bg-yellow-50 text-yellow-700<#elseif message.type = 'error'>bg-red-50 text-red-700<#else>bg-blue-50 text-blue-700</#if>">
                    <p class="text-sm font-bold">${kcSanitize(message.summary)?no_esc}</p>
                </div>
            </#if>

            <#if realm.password>
                <form id="kc-form-login" onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post" class="space-y-6">
                    
                    <div class="space-y-2">
                        <label for="username" class="text-sm font-bold text-slate-700">Email</label>
                        <div class="relative group">
                            <div class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-blue-500 transition-colors">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                                </svg>
                            </div>
                            <input tabindex="1" id="username" name="username" value="${(login.username!'')}" type="text" autofocus autocomplete="off" placeholder="nom@universite.fr" 
                                   class="w-full pl-12 pr-4 py-4 bg-slate-50 border border-slate-100 rounded-xl focus:bg-white focus:ring-4 focus:ring-blue-50 focus:border-blue-200 outline-none transition-all font-semibold text-slate-800" />
                        </div>
                    </div>

                    <div class="space-y-2">
                        <label for="password" class="text-sm font-bold text-slate-700">Mot de passe</label>
                        <div class="relative group">
                            <div class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-blue-500 transition-colors">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                                </svg>
                            </div>
                            <input tabindex="2" id="password" name="password" type="password" autocomplete="off" placeholder="••••••••" 
                                   class="w-full pl-12 pr-12 py-4 bg-slate-50 border border-slate-100 rounded-xl focus:bg-white focus:ring-4 focus:ring-blue-50 focus:border-blue-200 outline-none transition-all font-semibold text-slate-800" />
                            <div class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-300 cursor-pointer hover:text-slate-500">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                                </svg>
                            </div>
                        </div>
                    </div>

                    <div class="flex items-center justify-between">
                        <#if realm.rememberMe && !usernameHidden??>
                            <label class="flex items-center cursor-pointer group">
                                <input tabindex="3" id="rememberMe" name="rememberMe" type="checkbox" class="w-5 h-5 rounded-md border-slate-200 text-blue-600 focus:ring-blue-500 transition-all" <#if login.rememberMe??>checked</#if>>
                                <span class="ml-3 text-[13px] font-bold text-slate-600 group-hover:text-slate-900 transition-colors">Se souvenir de moi</span>
                            </label>
                        </#if>
                        
                        <#if realm.resetPasswordAllowed>
                            <a tabindex="5" href="${url.loginResetCredentialsUrl}" class="text-[13px] font-bold text-blue-600 hover:text-blue-800 transition-colors">Mot de passe oublié ?</a>
                        </#if>
                    </div>

                    <button tabindex="4" name="login" id="kc-login" type="submit" class="w-full py-4 px-6 bg-black text-white rounded-xl font-black text-sm hover:bg-slate-800 active:scale-[0.98] transition-all shadow-xl shadow-black/10 mt-4">
                        Se connecter
                    </button>
                </form>
            </#if>

            <!-- Footer Links -->
            <div class="mt-16 flex flex-col items-center gap-4 text-[10px] font-black text-slate-400 tracking-[0.15em] text-center">
                <a href="#" class="hover:text-slate-900 transition-colors">POLITIQUE DE CONFIDENTIALITÉ</a>
                <a href="#" class="hover:text-slate-900 transition-colors">CONDITIONS D'UTILISATION</a>
                <a href="#" class="hover:text-slate-900 transition-colors">SUPPORT ACADÉMIQUE</a>
            </div>

        </div>
    </div>
</body>
</html>