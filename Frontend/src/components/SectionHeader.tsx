import { IconPlus } from './Icons';

interface SectionHeaderProps {
  title: string;
  onAdd?: () => void;
  addLabel?: string;
  accentColor?: string;
}

export function SectionHeader({ title, onAdd, addLabel, accentColor = 'bg-[#1e293b]' }: SectionHeaderProps) {
  return (
    <div className="flex items-center justify-between flex-1 min-w-0 mb-4">
      <h2 className="flex items-center gap-3 text-[17px] font-bold text-[#1e293b]">
        <span className={`w-[4px] h-5 ${accentColor} rounded-full shrink-0`} />
        {title}
      </h2>
      {onAdd && (
        <button className="inline-flex items-center gap-2 p-[7px_16px] rounded-lg bg-[#1e293b] text-white text-[13px] font-bold shadow-sm hover:opacity-90 transition-all" onClick={onAdd}>
          <IconPlus /> {addLabel}
        </button>
      )}
    </div>
  );
}