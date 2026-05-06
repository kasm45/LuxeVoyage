import { useState } from 'react';
import { destinations } from '../data/destinations.js';

export default function AttractionManagement() {
  const [rows, setRows] = useState(destinations);

  return (
    <div>
      <h1 className="font-serif text-h1 text-on-surface">Attraction management</h1>
      <p className="mt-2 max-w-2xl font-sans text-body-md text-on-surface-variant">
        Lightweight mock table — edits stay in session memory only.
      </p>

      <div className="mt-10 overflow-x-auto rounded-2xl border border-surface-variant bg-white shadow-card">
        <table className="min-w-full divide-y divide-surface-variant text-left font-sans text-sm">
          <thead className="bg-surface-container-low">
            <tr>
              <th className="px-6 py-4 font-semibold text-on-surface">Destination</th>
              <th className="px-6 py-4 font-semibold text-on-surface">Region</th>
              <th className="px-6 py-4 font-semibold text-on-surface">Category</th>
              <th className="px-6 py-4 font-semibold text-on-surface">From ($)</th>
              <th className="px-6 py-4 font-semibold text-on-surface">Published</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-surface-variant">
            {rows.map((d, idx) => (
              <tr key={d.id}>
                <td className="px-6 py-4 font-medium text-on-surface">{d.name}</td>
                <td className="px-6 py-4 text-on-surface-variant">{d.region}</td>
                <td className="px-6 py-4 text-on-surface-variant">{d.category}</td>
                <td className="px-6 py-4">
                  <input
                    type="number"
                    className="w-24 rounded-lg border border-surface-variant px-2 py-1"
                    value={d.priceFrom}
                    onChange={(e) => {
                      const v = Number(e.target.value);
                      setRows((prev) => prev.map((r, i) => (i === idx ? { ...r, priceFrom: v } : r)));
                    }}
                  />
                </td>
                <td className="px-6 py-4">
                  <input type="checkbox" defaultChecked className="accent-primary" />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
