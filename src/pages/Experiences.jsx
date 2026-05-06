import { useMemo, useState } from 'react';
import FilterBar from '../components/FilterBar.jsx';
import ExperienceCard from '../components/ExperienceCard.jsx';
import { experiences } from '../data/experiences.js';
import { useFilteredItems } from '../hooks/useFilteredItems.js';

export default function Experiences() {
  const categories = useMemo(() => {
    const s = new Set(experiences.map((e) => e.category));
    return ['All', ...Array.from(s)];
  }, []);
  const regions = useMemo(() => {
    const s = new Set(experiences.map((e) => e.region));
    return ['All', ...Array.from(s)];
  }, []);

  const [category, setCategory] = useState('All');
  const [region, setRegion] = useState('All');
  const [maxPrice, setMaxPrice] = useState(100000);

  const filtered = useFilteredItems(experiences, { category, region, maxPrice });

  return (
    <div className="mx-auto max-w-container px-gutter py-12">
      <h1 className="font-serif text-h1 text-on-surface">Experiences</h1>
      <p className="mt-4 max-w-3xl font-sans text-body-lg text-on-surface-variant">
        Hands-on immersions — helicopters at dawn, cellar doors at dusk — engineered for memory-rich travel.
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

      <div className="mt-12 grid grid-cols-1 gap-10 md:grid-cols-2 lg:grid-cols-3">
        {filtered.map((e) => (
          <ExperienceCard key={e.id} experience={e} variant="grid" />
        ))}
      </div>

      {filtered.length === 0 && (
        <p className="mt-16 text-center font-sans text-on-surface-variant">No experiences match those filters.</p>
      )}
    </div>
  );
}
