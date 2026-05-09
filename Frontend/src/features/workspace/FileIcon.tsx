import React from 'react';
// Importe les icônes nécessaires de lucide-react
import { 
  FolderIcon, 
  FolderOpenIcon, 
  FileTextIcon, 
  FileJsonIcon, 
  SquareDotIcon, // Pour représenter C#
  AtSignIcon // Pour représenter .tsx/.jsx
} from 'lucide-react';

interface FileIconProps {
  name: string;
  isFolder?: boolean;
  isOpen?: boolean;
}

// Fonction utilitaire pour obtenir l'extension
const getExtension = (name: string) => {
  return name.split('.').pop()?.toLowerCase();
};

export const FileIcon: React.FC<FileIconProps> = ({ name, isFolder, isOpen }) => {
  // 1. Gestion des dossiers
  if (isFolder) {
    if (isOpen) {
      return <FolderOpenIcon className="w-4 h-4 text-[#e3c75f]" />; // Jaune VS Code
    }
    return <FolderIcon className="w-4 h-4 text-[#cccccc]" />; // Gris VS Code
  }

  // 2. Gestion des fichiers spécifiques (mapping par nom exact ou extension)
  const extension = getExtension(name);

  switch (extension) {
    case 'tsx':
    case 'jsx':
      return <AtSignIcon className="w-4 h-4 text-[#519aba]" />; // Bleu React
    case 'cs':
      return <SquareDotIcon className="w-4 h-4 text-[#ffffff]" />; // Blanc (style C#)
    case 'json':
      return <FileJsonIcon className="w-4 h-4 text-[#d8c360]" />; // Jaune JSON
    case 'css':
      return <FileTextIcon className="w-4 h-4 text-[#519aba]" />; // Bleu CSS
    case 'readme.md':
      return <FileTextIcon className="w-4 h-4 text-[#ffffff]" />;
    default:
      // Fichier par défaut
      return <FileTextIcon className="w-4 h-4 text-[#999999]" />;
  }
};