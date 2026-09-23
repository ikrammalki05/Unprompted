interface InfoFieldProps {
  label: string;
  value: string;
  editable: boolean;
  editValue: string;
  onChange: (value: string) => void;
}

export function InfoField({ label, value, editable, editValue, onChange }: InfoFieldProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <span className="text-[10px] font-bold text-[#94a3b8] uppercase tracking-[0.05em]">{label}</span>
      {editable ? (
        <input
          className="w-full p-[8px_12px] border border-[#e2e8f0] rounded-lg text-sm text-[#1e293b] outline-none transition-all focus:border-[#2563eb] focus:ring-1 focus:ring-[#2563eb]"
          value={editValue}
          onChange={(e) => onChange(e.target.value)}
        />
      ) : (
        <span className="text-sm font-medium text-[#1e293b]">{value}</span>
      )}
    </div>
  );
}