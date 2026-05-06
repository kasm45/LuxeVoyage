import { NavLink, Outlet } from 'react-router-dom';

const itemClass = ({ isActive }) =>
  [
    'flex items-center gap-3 rounded-xl px-4 py-3 font-sans text-sm font-medium transition-colors',
    isActive ? 'bg-white text-primary shadow-sm' : 'text-slate-600 hover:bg-white/60',
  ].join(' ');

export default function AdminLayout() {
  return (
    <div className="flex min-h-screen bg-background">
      <aside className="hidden w-64 shrink-0 border-r border-surface-variant bg-surface-container-low p-6 md:flex md:flex-col">
        <div className="font-serif text-xl font-bold text-on-surface">LuxeVoyage</div>
        <p className="mt-1 font-sans text-xs uppercase tracking-wide text-on-surface-variant">Admin</p>
        <nav className="mt-10 flex flex-col gap-1">
          <NavLink to="/admin" end className={itemClass}>
            <span className="material-symbols-outlined text-xl">dashboard</span>
            Dashboard
          </NavLink>
          <NavLink to="/admin/attractions" className={itemClass}>
            <span className="material-symbols-outlined text-xl">travel_explore</span>
            Attractions
          </NavLink>
          <NavLink to="/" className="mt-8 text-sm font-medium text-primary hover:underline">
            ← Back to site
          </NavLink>
        </nav>
      </aside>
      <div className="flex-1 overflow-auto">
        <div className="border-b border-surface-variant bg-white/80 px-4 py-4 backdrop-blur md:hidden">
          <div className="font-serif font-semibold">Admin</div>
          <div className="mt-3 flex gap-2">
            <NavLink
              to="/admin"
              end
              className="rounded-lg bg-surface-container-low px-3 py-2 text-sm font-medium text-on-surface"
            >
              Dashboard
            </NavLink>
            <NavLink
              to="/admin/attractions"
              className="rounded-lg bg-surface-container-low px-3 py-2 text-sm font-medium text-on-surface"
            >
              Attractions
            </NavLink>
          </div>
        </div>
        <div className="p-6 md:p-10">
          <Outlet />
        </div>
      </div>
    </div>
  );
}
