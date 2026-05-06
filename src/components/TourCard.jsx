import { Link } from 'react-router-dom';

export default function TourCard({ tour }) {
  const { id, title, excerpt, days, priceFrom, image } = tour;

  return (
    <div
      id={`tour-${id}`}
      className="scroll-mt-28 overflow-hidden rounded-2xl border border-surface-variant bg-white shadow-card transition-shadow hover:shadow-cardHover"
    >
      <div className="grid gap-0 md:grid-cols-5">
        <div className="relative md:col-span-2">
          <img src={image} alt={title} className="h-56 w-full object-cover md:h-full md:min-h-[220px]" />
        </div>
        <div className="flex flex-col justify-center p-6 md:col-span-3">
          <div className="mb-2 flex flex-wrap items-center gap-2">
            <span className="rounded-full bg-secondary-container/80 px-3 py-1 font-sans text-label-caps uppercase text-on-secondary-container">
              {tour.region}
            </span>
            <span className="rounded-full bg-surface-container-low px-3 py-1 font-sans text-xs font-semibold text-on-surface-variant">
              {days} days
            </span>
          </div>
          <h3 className="font-serif text-h3 text-on-surface">{title}</h3>
          <p className="mt-2 font-sans text-body-md text-on-surface-variant">{excerpt}</p>
          <div className="mt-6 flex flex-wrap items-center justify-between gap-4">
            <p className="font-serif text-lg font-semibold text-on-surface">From ${priceFrom.toLocaleString()}</p>
            <Link
              to={{ pathname: '/tours', hash: `tour-${id}` }}
              className="rounded-2xl bg-gradient-to-r from-primary to-surface-tint px-6 py-3 font-sans text-sm font-semibold text-on-primary shadow-card"
            >
              Jump to tour
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}
