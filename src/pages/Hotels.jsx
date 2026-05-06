import { useEffect, useMemo, useState } from 'react';
import { useLocation } from 'react-router-dom';
import FilterBar from '../components/FilterBar.jsx';
import HotelCard from '../components/HotelCard.jsx';
import { hotels } from '../data/hotels.js';
import { useFilteredItems } from '../hooks/useFilteredItems.js';

export default function Hotels() {
  const location = useLocation();
  const categories = useMemo(() => {
    const s = new Set(hotels.map((h) => h.category));
    return ['All', ...Array.from(s)];
  }, []);
  const regions = useMemo(() => {
    const s = new Set(hotels.map((h) => h.region));
    return ['All', ...Array.from(s)];
  }, []);

  const [category, setCategory] = useState('All');
  const [region, setRegion] = useState('All');
  const [maxPrice, setMaxPrice] = useState(100000);

  const filtered = useFilteredItems(hotels, { category, region, maxPrice });

  useEffect(() => {
    if (!location.hash) return;
    const id = location.hash.replace('#', '');
    const el = document.getElementById(id);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }, [location.hash, filtered]);

  return (
    <div className="mx-auto max-w-container px-gutter py-12">
      <h1 className="font-serif text-h1 text-on-surface">Stays</h1>
      <p className="mt-4 max-w-3xl font-sans text-body-lg text-on-surface-variant">
        Villas, design towers, and alpine lodges — inspected personally, priced transparently.
      </p>

      <div className="mt-10">
        <FilterBar
          category={category}
          onCategoryChange={setCategory}
          categories={categories}
          region={region}
          onRegionChange={setRegion}
          regions={regions}
          maxPrice={maxPrice}
          onMaxPriceChange={setMaxPrice}
        />
      </div>

      <div className="mt-12 grid grid-cols-1 gap-8 md:grid-cols-2">
        {filtered.map((h) => (
          <HotelCard key={h.id} hotel={h} />
        ))}
      </div>

      {filtered.length === 0 && (
        <p className="mt-16 text-center font-sans text-on-surface-variant">No stays match those filters.</p>
      )}
    </div>
  );
}
