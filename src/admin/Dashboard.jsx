const metrics = [
  { label: 'Active itineraries', value: '128', delta: '+12%', icon: 'map' },
  { label: 'Concierge SLA', value: '14m', delta: '-3m', icon: 'schedule' },
  { label: 'Partner score', value: '4.9', delta: '+0.1', icon: 'star' },
  { label: 'Carbon offsets', value: '842t', delta: '+42t', icon: 'forest' },
];

export default function Dashboard() {
  return (
    <div>
      <h1 className="font-serif text-h1 text-on-surface">Dashboard</h1>
      <p className="mt-2 max-w-2xl font-sans text-body-md text-on-surface-variant">
        Operational pulse for LuxeVoyage — mock metrics until backend wiring.
      </p>

      <div className="mt-10 grid gap-6 sm:grid-cols-2 xl:grid-cols-4">
        {metrics.map((m) => (
          <div
            key={m.label}
            className="rounded-2xl border border-surface-variant bg-white p-6 shadow-card"
          >
            <div className="flex items-center justify-between">
              <span className="font-sans text-sm text-on-surface-variant">{m.label}</span>
              <span className="material-symbols-outlined text-primary">{m.icon}</span>
            </div>
            <p className="mt-4 font-serif text-3xl font-semibold text-on-surface">{m.value}</p>
            <p className="mt-2 font-sans text-xs font-semibold text-tertiary">{m.delta}</p>
          </div>
        ))}
      </div>

      <div className="mt-10 rounded-2xl border border-dashed border-outline-variant bg-surface-container-low/50 p-10 text-center font-sans text-sm text-on-surface-variant">
        Charts and exports plug in here — placeholder keeps layout faithful to the LuxeVoyage admin shell.
      </div>
    </div>
  );
}
