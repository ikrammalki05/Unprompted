# Setup Keycloak — Unprompted

## 1. Lancer Keycloak
```bash
docker run -d --name keycloak -p 8080:8080 \
  -e KC_BOOTSTRAP_ADMIN_USERNAME=admin \
  -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin \
  quay.io/keycloak/keycloak:26.2.0 start-dev
```

## 2. Importer le realm
- Ouvrir http://localhost:8080 (admin / admin)
- **Manage realms** → **Create realm** → importer `keycloak/unprompted-realm.json`

## 3. Créer un utilisateur de test
- **Users** → **Add user** → username : `test_user`
- Onglet **Credentials** → mot de passe : `test123` → Temporary : **Off**
- Onglet **Role mapping** → assigner le rôle `Etudiant`

## 4. Configurer le client_secret
Dans `Backend/API/appsettings.Development.json` :
```json
{
  "Keycloak": {
    "ClientSecret": "DEMANDER_LE_SECRET_EN_PRIVE"
  }
}
```

## 5. Lancer l'API
```bash
cd Backend && dotnet run --project API
```

## 6. Tester
```bash
# Obtenir un token
curl -X POST "http://localhost:8080/realms/unprompted/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&client_id=unprompted-api&client_secret=SECRET&username=test_user&password=test123"

# Appeler l'API
curl http://localhost:5066/api/test/etudiant \
  -H "Authorization: Bearer VOTRE_TOKEN"
```