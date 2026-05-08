<!DOCTYPE html>
<html lang="fr" class="h-full">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>${msg("loginTitle",(realm.displayName!''))}</title>
    
    <script src="https://cdn.tailwindcss.com"></script>
</head>

<body class="flex h-screen w-full font-sans bg-gray-50 overflow-hidden">
    
    <div class="hidden lg:flex w-1/2 bg-[#021124] text-white p-8 xl:p-12 flex-col h-full">
        
        <div class="flex-none flex justify-center w-full">
            <div class="w-20 h-20 rounded-full bg-white flex items-center justify-center p-3 shadow-xl text-[#021124] font-bold">
                LOGO
            </div>
        </div>
        
        <div class="flex-1 flex flex-col justify-center min-h-0 mt-8 mb-8 relative">
            <div class="relative z-10">
                <h1 class="text-4xl xl:text-5xl font-bold mb-4 leading-tight drop-shadow-lg">
                    Bienvenue sur <br />
                    <span class="text-[#a6c1ee]">Unprompted</span>
                </h1>
                <p class="text-base text-gray-200 max-w-md leading-relaxed drop-shadow-md">
                    Gérez vos projets académiques avec efficacité. Une plateforme sécurisée pour l'excellence dans la recherche et la gouvernance IA.
                </p>
            </div>
        </div>

        <div class="flex-none flex gap-8 xl:gap-12">
            <div>
                <p class="text-2xl xl:text-3xl font-bold">99.9%</p>
                <p class="text-xs text-gray-400 mt-1">Disponibilité</p>
            </div>
            <div>
                <p class="text-2xl xl:text-3xl font-bold">SEC-2</p>
                <p class="text-xs text-gray-400 mt-1">Conformité</p>
            </div>
            <div>
                <p class="text-2xl xl:text-3xl font-bold">AI-Ready</p>
                <p class="text-xs text-gray-400 mt-1">Infrastructure</p>
            </div>
        </div>
    </div>

    <div class="w-full lg:w-1/2 flex flex-col justify-center items-center p-4 sm:p-8 h-full overflow-y-auto">
        <div class="w-full max-w-md bg-white rounded-2xl shadow-xl p-8 sm:p-10">
            
            <h2 class="text-3xl font-semibold mb-2 text-gray-900">Connexion</h2>
            <p class="text-gray-500 mb-6 text-sm">Accédez à votre espace de travail académique.</p>

            <#if message?has_content && (message.type != 'warning' || !isAppInitiatedAction??)>
                <div class="mb-6 p-4 rounded-lg <#if message.type = 'success'>bg-green-50 text-green-700 border border-green-200<#elseif message.type = 'warning'>bg-yellow-50 text-yellow-700 border border-yellow-200<#elseif message.type = 'error'>bg-red-50 text-red-700 border border-red-200<#else>bg-blue-50 text-blue-700 border border-blue-200</#if>">
                    <p class="text-sm font-semibold">${kcSanitize(message.summary)?no_esc}</p>
                </div>
            </#if>

            <#if realm.password>
                <form id="kc-form-login" onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post" class="space-y-5">
                    
                    <div>
                        <label for="username" class="block text-sm font-semibold text-gray-700 mb-1.5">Email</label>
                        <div class="relative">
                            <input tabindex="1" id="username" name="username" value="${(login.username!'')}" type="text" autofocus autocomplete="off" placeholder="nom@universite.fr" class="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-[#a6c1ee] focus:border-[#a6c1ee] outline-none transition-all bg-gray-50" />
                            <span class="absolute left-3 top-3.5 text-gray-400">✉️</span> 
                        </div>
                    </div>

                    <div>
                        <label for="password" class="block text-sm font-semibold text-gray-700 mb-1.5">Mot de passe</label>
                        <div class="relative">
                            <input tabindex="2" id="password" name="password" type="password" autocomplete="off" placeholder="••••••••" class="w-full pl-10 pr-4 py-3 border border-gray-200 rounded-lg focus:ring-2 focus:ring-[#a6c1ee] focus:border-[#a6c1ee] outline-none transition-all bg-gray-50" />
                            <span class="absolute left-3 top-3.5 text-gray-400">🔒</span>
                        </div>
                    </div>

                    <div class="flex items-center justify-between mt-2">
                        <#if realm.rememberMe && !usernameHidden??>
                            <div class="flex items-center">
                                <input tabindex="3" id="rememberMe" name="rememberMe" type="checkbox" class="h-4 w-4 text-blue-600 rounded border-gray-300" <#if login.rememberMe??>checked</#if>>
                                <label for="rememberMe" class="ml-2 block text-sm text-gray-600">Se souvenir de moi</label>
                            </div>
                        </#if>
                        
                        <#if realm.resetPasswordAllowed>
                            <a tabindex="5" href="${url.loginResetCredentialsUrl}" class="text-sm font-semibold text-[#0066cc] hover:underline">Mot de passe oublié ?</a>
                        </#if>
                    </div>

                    <button tabindex="4" name="login" id="kc-login" type="submit" class="w-full flex justify-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-bold text-white bg-[#021124] hover:bg-gray-800 transition-colors mt-6">
                        Se connecter
                    </button>
                </form>
            </#if>

            <div class="mt-10 flex flex-col items-center gap-3 text-[10px] font-bold text-gray-400 tracking-widest text-center">
                <a href="#" class="hover:text-gray-600 transition-colors">POLITIQUE DE CONFIDENTIALITÉ</a>
                <a href="#" class="hover:text-gray-600 transition-colors">CONDITIONS D'UTILISATION</a>
            </div>

        </div>
    </div>
</body>
</html>