import React from 'react';

interface ProfileFormFieldProps {
  label: string;
  name?: string;
  value?: string;
  defaultValue?: string;
  placeholder?: string;
  type?: string;
  icon?: React.ElementType;
  readOnly?: boolean;
  onChange?: (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => void;
  as?: 'input' | 'select';
  children?: React.ReactNode;
}

export const ProfileFormField: React.FC<ProfileFormFieldProps> = ({
  label,
  name,
  value,
  defaultValue,
  placeholder,
  type = 'text',
  icon: Icon,
  readOnly,
  onChange,
  as = 'input',
  children,
}) => {
  const fieldClass = `w-full ${Icon ? 'pl-11' : 'pl-4'} pr-4 py-3.5 rounded-xl text-sm font-semibold text-slate-900 outline-none transition-all placeholder:text-slate-400 ${
    readOnly
      ? 'bg-slate-50 border border-slate-100 text-slate-500 cursor-not-allowed'
      : 'bg-white border border-slate-200 focus:border-blue-400 focus:ring-4 focus:ring-blue-50'
  }`;

  return (
    <div className="space-y-2">
      <label className="block text-[11px] font-bold text-slate-400 uppercase tracking-[0.08em]">
        {label}
      </label>
      <div className="relative group">
        {Icon && (
          <div className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-blue-500 transition-colors">
            <Icon className="w-4 h-4" />
          </div>
        )}
        {as === 'select' ? (
          <select
            name={name}
            value={value}
            defaultValue={defaultValue}
            onChange={onChange}
            disabled={readOnly}
            className={`${fieldClass} appearance-none cursor-pointer`}
          >
            {children}
          </select>
        ) : (
          <input
            name={name}
            type={type}
            value={value}
            defaultValue={defaultValue}
            onChange={onChange as (e: React.ChangeEvent<HTMLInputElement>) => void}
            readOnly={readOnly}
            placeholder={placeholder}
            className={fieldClass}
          />
        )}
      </div>
    </div>
  );
};

export default ProfileFormField;
