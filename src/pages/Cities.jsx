import { useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { cities } from '../data/cities.js';

export default function Cities() {
  const regions = useMemo(() => ['All', ...new Set(cities.map((c) => c.region))], []);
  const [region, setRegion] = useState('All');

  const filtered = useMemo(() => {
    return cities.filter((c) => {
      if (region !== 'All' && c.region !== region) return false;
      return true;
    });
  }, [region]);

  return (
    <div className="mx-auto max-w-container px-gutter py-12">
      <h1 className="font-serif text-h1 text-on-surface">Cities</h1>
      <p className="mt-4 max-w-3xl font-sans text-body-lg text-on-surface-variant">
        Signature urban and coastal hubs we return to again and again — each with on-the-ground hosts.
      </p>

      <div className="mt-10 max-w-xs">
        <label className="flex flex-col gap-1 font-sans text-label-caps uppercase text-on-surface-variant">
          Region
          <select
            value={region}
            onChange={(e) => setRegion(e.target.value)}
            className="rounded-xl border border-surface-variant bg-surface-container-low px-3 py-2.5 font-sans text-sm text-on-surface outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
          >
            {regions.map((r) => (
              <option key={r} value={r}>
                {r}
              </option>
            ))}
          </select>
        </label>
      </div>

      <div className="mt-12 grid grid-cols-1 gap-8 md:grid-cols-2">
        {filtered.map((city) => (
          <Link
            key={city.id}
            to="/destinations"
            className="group overflow-hidden rounded-2xl border border-surface-variant bg-white shadow-card transition-shadow hover:shadow-cardHover"
          >
            <div className="relative h-56 overflow-hidden">
              <img
                src={city.image}
                alt={city.name}
                className="h-full w-full object-cover transition-transform duration-700 group-hover:scale-105"
              />
              <div className="absolute inset-0 bg-gradient-to-t from-black/70 to-transparent" />
              <div className="absolute bottom-0 left-0 p-6 text-white">
                <p className="font-sans text-label-caps uppercase text-white/80">{city.country}</p>
                <h2 className="font-serif text-2xl font-semibold">{city.name}</h2>
                <p className="mt-2 font-sans text-sm text-white/85">{city.tagline}</p>
              </div>
            </div>
            <div className="p-5 font-sans text-sm font-semibold text-primary">Explore related destinations →</div>
          </Link>
        ))}
      </div>
    </div>
  );
}
