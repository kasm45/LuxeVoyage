import { useEffect, useMemo, useState } from 'react';
import { useLocation } from 'react-router-dom';
import FilterBar from '../components/FilterBar.jsx';
import TourCard from '../components/TourCard.jsx';
import { tours } from '../data/tours.js';
import { useFilteredItems } from '../hooks/useFilteredItems.js';

export default function Tours() {
  const location = useLocation();
  const categories = useMemo(() => {
    const s = new Set(tours.map((t) => t.category));
    return ['All', ...Array.from(s)];
  }, []);
  const regions = useMemo(() => {
    const s = new Set(tours.map((t) => t.region));
    return ['All', ...Array.from(s)];
  }, []);

  const [category, setCategory] = useState('All');
  const [region, setRegion] = useState('All');
  const [maxPrice, setMaxPrice] = useState(100000);

  const filtered = useFilteredItems(tours, { category, region, maxPrice });

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
      <h1 className="font-serif text-h1 text-on-surface">Tours</h1>
      <p className="mt-4 max-w-3xl font-sans text-body-lg text-on-surface-variant">
        Multi-day itineraries with private guides, seamless transfers, and room to breathe between highlights.
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

      <div className="mt-12 flex flex-col gap-8">
        {filtered.map((t) => (
          <TourCard key={t.id} tour={t} />
        ))}
      </div>

      {filtered.length === 0 && (
        <p className="mt-16 text-center font-sans text-on-surface-variant">No tours match those filters.</p>
      )}
    </div>
  );
}
