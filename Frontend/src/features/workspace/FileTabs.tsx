
export const FileTabs = () => {
  return (
    <div className="flex bg-[#2d2d2d] border-b border-[#1e1e1e] overflow-x-auto no-scrollbar">
      {/* Onglet actif */}
      <div className="px-4 py-2 bg-[#1e1e1e] text-[#ffffff] text-sm border-t-2 border-[#007acc] cursor-pointer flex items-center gap-2 min-w-fit">
        <span className="text-[#e3c75f]">JS</span> App.jsx
        <button className="ml-2 hover:bg-[#333] rounded p-0.5 text-[#999] hover:text-white">✕</button>
      </div>
      
      {/* Onglet inactif */}
      <div className="px-4 py-2 text-[#969696] text-sm border-t-2 border-transparent border-r border-[#2d2d2d] bg-[#2d2d2d] hover:bg-[#2b2b2b] cursor-pointer flex items-center gap-2 min-w-fit">
        <span className="text-[#519aba]">#</span> index.css
      </div>
    </div>
  );
};