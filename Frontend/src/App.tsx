import { RouterProvider } from 'react-router-dom';
import { router } from './app/routes';

function App() {
  return (
    // RouterProvider va écouter l'URL du navigateur et afficher la bonne page
    <RouterProvider router={router} />
  );
}

export default App;