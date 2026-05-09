import Keycloak from 'keycloak-js';

// Remplace ces valeurs par celles de ton serveur Keycloak backend
const keycloakConfig = {
  url: 'http://localhost:8080', // L'URL de ton Keycloak
  realm: 'unprompted-realm',         // Le nom de ton Realm
  clientId: 'react-frontend',        // Le nom de ton Client ID
};

export const keycloak = new Keycloak(keycloakConfig);