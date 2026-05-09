

export const Terminal = () => {
  return (
    <div className="flex flex-col h-full p-2">
      {/* Menu du terminal */}
      <div className="flex gap-4 border-b border-[#333333] px-2 text-[11px] tracking-wide uppercase">
        <button className="pb-1 text-[#cccccc] hover:text-white">Problèmes <span className="bg-[#007acc] text-white rounded-full px-1.5 text-[9px] ml-1">1</span></button>
        <button className="pb-1 text-white border-b border-[#007acc]">Terminal</button>
      </div>
      
      {/* Contenu du terminal */}
      <div className="flex-1 overflow-y-auto p-2 font-mono text-[13px]">
        <div className="text-white">
          <span className="text-[#4daf4a]">kaddoura@unprompted</span> <span className="text-[#98c379]">➜</span> <span className="text-[#61afef]">/workspaces/unprompted</span> <span className="text-[#e5c07b]">(main)</span> $ npm start
        </div>
        <div className="mt-2 text-[#98c379]">➜ Local: http://localhost:3000/</div>
      </div>
    </div>
  );
};