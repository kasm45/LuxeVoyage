import { useState } from 'react';
import { NavLink, Link } from 'react-router-dom';

const linkClass = ({ isActive }) =>
  [
    'font-medium transition-colors duration-200 text-sm md:text-base',
    isActive
      ? 'text-orange-600 font-semibold relative after:block after:w-1 after:h-1 after:bg-orange-600 after:rounded-full after:mx-auto after:mt-1'
      : 'text-slate-700 dark:text-slate-300 hover:text-orange-500',
  ].join(' ');

const navItems = [
  { to: '/destinations', label: 'Destinations' },
  { to: '/cities', label: 'Cities' },
  { to: '/experiences', label: 'Experiences' },
  { to: '/tours', label: 'Tours' },
  { to: '/hotels', label: 'Stays' },
  { to: '/about', label: 'About' },
  { to: '/contact', label: 'Contact' },
];

export default function Navbar() {
  const [open, setOpen] = useState(false);

  return (
    <nav className="fixed top-0 z-50 w-full border-b border-white/20 bg-white/70 shadow-sm backdrop-blur-xl dark:border-slate-800/50 dark:bg-slate-900/70">
      <div className="mx-auto flex max-w-container items-center justify-between gap-4 px-4 py-4 md:px-12">
        <Link to="/" className="font-serif text-xl font-bold tracking-tight text-slate-900 dark:text-white md:text-2xl">
          LuxeVoyage
        </Link>

        <div className="hidden items-center gap-6 lg:flex xl:gap-8">
          {navItems.map(({ to, label }) => (
            <NavLink key={to} to={to} className={linkClass}>
              {label}
            </NavLink>
          ))}
        </div>

        <div className="flex items-center gap-2 md:gap-4">
          <Link
            to="/login"
            className="hidden rounded-full p-2 text-slate-700 hover:text-orange-500 sm:block"
            aria-label="Account"
          >
            <span className="material-symbols-outlined text-[26px]">account_circle</span>
          </Link>
          <Link
            to="/contact"
            className="hidden scale-95 rounded-2xl bg-gradient-to-r from-primary to-surface-tint px-5 py-2.5 text-sm font-semibold text-on-primary shadow-card transition-transform active:scale-100 md:inline-flex md:px-6 md:py-3"
          >
            Book Now
          </Link>
          <button
            type="button"
            className="inline-flex rounded-lg p-2 text-slate-800 lg:hidden"
            onClick={() => setOpen((v) => !v)}
            aria-expanded={open}
            aria-label="Menu"
          >
            <span className="material-symbols-outlined">{open ? 'close' : 'menu'}</span>
          </button>
        </div>
      </div>

      {open && (
        <div className="border-t border-surface-variant bg-white/95 px-4 py-4 lg:hidden dark:bg-slate-900/95">
          <div className="flex flex-col gap-3">
            {navItems.map(({ to, label }) => (
              <NavLink
                key={to}
                to={to}
                className={linkClass}
                onClick={() => setOpen(false)}
              >
                {label}
              </NavLink>
            ))}
            <Link to="/login" className="pt-2 font-medium text-slate-700" onClick={() => setOpen(false)}>
              Login
            </Link>
            <Link
              to="/contact"
              className="rounded-2xl bg-gradient-to-r from-primary to-surface-tint px-4 py-3 text-center font-semibold text-on-primary"
              onClick={() => setOpen(false)}
            >
              Book Now
            </Link>
          </div>
        </div>
      )}
    </nav>
  );
}
