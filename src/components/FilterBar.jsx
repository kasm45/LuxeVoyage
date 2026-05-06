export default function FilterBar({
  category,
  onCategoryChange,
  categories,
  region,
  onRegionChange,
  regions,
  maxPrice,
  onMaxPriceChange,
  pricePresets = [500, 1000, 2500, 5000, 100000],
}) {
  return (
    <div className="flex flex-col gap-4 rounded-2xl border border-surface-variant bg-white p-4 shadow-card md:flex-row md:flex-wrap md:items-end">
      <label className="flex flex-1 flex-col gap-1 font-sans text-label-caps uppercase text-on-surface-variant">
        Category
        <select
          value={category}
          onChange={(e) => onCategoryChange(e.target.value)}
          className="rounded-xl border border-surface-variant bg-surface-container-low px-3 py-2.5 font-sans text-sm font-normal capitalize text-on-surface outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
        >
          {categories.map((c) => (
            <option key={c} value={c}>
              {c}
            </option>
          ))}
        </select>
      </label>
      <label className="flex flex-1 flex-col gap-1 font-sans text-label-caps uppercase text-on-surface-variant">
        Region
        <select
          value={region}
          onChange={(e) => onRegionChange(e.target.value)}
          className="rounded-xl border border-surface-variant bg-surface-container-low px-3 py-2.5 font-sans text-sm font-normal text-on-surface outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
        >
          {regions.map((r) => (
            <option key={r} value={r}>
              {r}
            </option>
          ))}
        </select>
      </label>
      <label className="flex min-w-[200px] flex-1 flex-col gap-1 font-sans text-label-caps uppercase text-on-surface-variant">
        Max budget (from)
        <select
          value={maxPrice}
          onChange={(e) => onMaxPriceChange(Number(e.target.value))}
          className="rounded-xl border border-surface-variant bg-surface-container-low px-3 py-2.5 font-sans text-sm font-normal text-on-surface outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
        >
          <option value={100000}>Any</option>
          {pricePresets.map((p) => (
            <option key={p} value={p}>
              Up to ${p}
            </option>
          ))}
        </select>
      </label>
    </div>
  );
}
