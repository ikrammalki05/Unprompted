import React from 'react';

interface ControlledSliderProps {
  value: number;
  min?: number;
  max?: number;
  step?: number;
  onChange: (value: number) => void;
  label: string;
  description?: string;
}

const ControlledSlider: React.FC<ControlledSliderProps> = ({ 
  value, 
  min = 0, 
  max = 10, 
  step = 0.5, 
  onChange,
  label,
  description
}) => {
  return (
    <div className="w-full">
      <label className="block text-sm font-black text-slate-800 mb-6">{label}</label>
      <div className="flex items-center gap-6">
        <input 
          type="range" 
          min={min} 
          max={max} 
          step={step} 
          value={value}
          onChange={(e) => onChange(parseFloat(e.target.value))}
          className="flex-1 h-1.5 bg-slate-100 rounded-full appearance-none cursor-pointer accent-blue-500 transition-all hover:bg-slate-200"
        />
        <div className="px-5 py-3 bg-blue-50 rounded-xl text-blue-600 font-black text-lg min-w-[70px] text-center shadow-inner">
          {value.toFixed(1)}
        </div>
      </div>
      {description && (
        <p className="mt-4 text-xs text-slate-400 font-medium">{description}</p>
      )}
    </div>
  );
};

export default ControlledSlider;
