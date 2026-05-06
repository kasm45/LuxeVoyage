import { useMemo, useState } from 'react';
import FilterBar from '../components/FilterBar.jsx';
import DestinationCard from '../components/DestinationCard.jsx';
import { destinations } from '../data/destinations.js';
import { useFilteredItems } from '../hooks/useFilteredItems.js';

export default function Destinations() {
  const categories = useMemo(() => {
    const s = new Set(destinations.map((d) => d.category));
    return ['All', ...Array.from(s)];
  }, []);
  const regions = useMemo(() => {
    const s = new Set(destinations.map((d) => d.region));
    return ['All', ...Array.from(s)];
  }, []);

  const [category, setCategory] = useState('All');
  const [region, setRegion] = useState('All');
  const [maxPrice, setMaxPrice] = useState(100000);

  const filtered = useFilteredItems(destinations, { category, region, maxPrice });

  return (
    <div className="mx-auto max-w-container px-gutter py-12">
      <div className="max-w-3xl">
        <h1 className="font-serif text-h1 text-on-surface">Destinations</h1>
        <p className="mt-4 font-sans text-body-lg text-on-surface-variant">
          Explore coastal escapes, alpine retreats, and vibrant cities — curated with the LuxeVoyage lens.
        </p>
      </div>

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
        {filtered.map((d) => (
          <DestinationCard key={d.id} destination={d} featured className="min-h-[320px]" />
        ))}
      </div>

      {filtered.length === 0 && (
        <p className="mt-16 text-center font-sans text-on-surface-variant">No destinations match those filters.</p>
      )}
    </div>
  );
}
