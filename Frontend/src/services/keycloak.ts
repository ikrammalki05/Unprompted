import Keycloak from 'keycloak-js';

const keycloakConfig = {
  url: 'http://localhost:8080',
  realm: 'unprompted',
  clientId: 'react-frontend',
};

export const keycloak = new Keycloak(keycloakConfig);
