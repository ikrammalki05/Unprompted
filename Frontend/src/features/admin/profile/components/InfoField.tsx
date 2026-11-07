interface InfoFieldProps {
  label: string;
  value: string;
  editable: boolean;
  editValue: string;
  onChange: (value: string) => void;
}

export function InfoField({ label, value, editable, editValue, onChange }: InfoFieldProps) {
  return (
    <div className="flex flex-col gap-2">
      <span className="text-[11px] font-bold text-slate-400 uppercase tracking-[0.08em]">{label}</span>
      {editable ? (
        <input
          className="w-full px-4 py-3 border border-slate-200 rounded-xl text-sm font-semibold text-slate-900 outline-none transition-all focus:border-blue-400 focus:ring-4 focus:ring-blue-50"
          value={editValue}
          onChange={(e) => onChange(e.target.value)}
        />
      ) : (
        <span className="text-sm font-semibold text-slate-900 min-h-[46px] flex items-center">
          {value}
        </span>
      )}
    </div>
  );
}
