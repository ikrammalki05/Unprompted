const statsData = [
  {
    id: 1,
    title: "Total étudiants",
    value: "1,284",
    badge: "+4.2%",
    badgeColor: "bg-green-100 text-green-700",
    icon: "🎓", // Remplace par ton SVG
    iconBg: "bg-blue-50",
  },
  {
    id: 2,
    title: "Total enseignants",
    value: "86",
    badge: "Stable",
    badgeColor: "bg-gray-200 text-gray-700",
    icon: "👩‍🏫",
    iconBg: "bg-gray-100",
  },
  {
    id: 3,
    title: "Total classes",
    value: "42",
    badge: "+12",
    badgeColor: "bg-green-100 text-green-700",
    icon: "🚪",
    iconBg: "bg-blue-50",
  },
];

export const StatCards = () => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      {statsData.map((stat) => (
        <div key={stat.id} className="bg-white rounded-2xl p-6 shadow-sm border border-gray-50">
          <div className="flex justify-between items-start mb-6">
            <div className={`w-12 h-12 rounded-xl flex items-center justify-center text-xl ${stat.iconBg}`}>
              {stat.icon}
            </div>
            <span className={`px-3 py-1 text-xs font-bold rounded-full ${stat.badgeColor}`}>
              {stat.badge}
            </span>
          </div>
          <div>
            <p className="text-sm font-semibold text-gray-500 mb-1">{stat.title}</p>
            <p className="text-3xl font-bold text-gray-900">{stat.value}</p>
          </div>
        </div>
      ))}
    </div>
  );
};