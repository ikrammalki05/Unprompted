/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
<<<<<<< HEAD
    "./src/**/*.{js,ts,tsx,jsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "#3b82f6", // Blue for "Retour"
        success: "#65a30d", // Green for "Enregistrer"
        background: "#f9fafb",
        card: "#ffffff",
      },
      borderRadius: {
        '3xl': '1.5rem',
      },
      boxShadow: {
        'custom': '0 10px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1)',
      }
    },
=======
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
>>>>>>> feature/etudiants-projets-cahier-charge
  },
  plugins: [],
}
