import { Link } from 'react-router-dom';

export default function HotelCard({ hotel }) {
  const { id, name, city, excerpt, priceFrom, rating, image, amenities } = hotel;

  return (
    <div
      id={`hotel-${id}`}
      className="flex flex-col overflow-hidden rounded-2xl border border-surface-variant bg-white shadow-card transition-shadow hover:shadow-cardHover scroll-mt-28"
    >
      <div className="relative h-52 w-full">
        <img src={image} alt={name} className="h-full w-full object-cover" />
        <span className="absolute right-4 top-4 rounded-full bg-white/90 px-3 py-1 font-sans text-sm font-semibold text-on-surface shadow">
          ★ {rating}
        </span>
      </div>
      <div className="flex flex-1 flex-col p-6">
        <p className="font-sans text-label-caps uppercase text-on-surface-variant">{city}</p>
        <h3 className="mt-1 font-serif text-xl font-semibold text-on-surface">{name}</h3>
        <p className="mt-2 line-clamp-2 font-sans text-sm text-on-surface-variant">{excerpt}</p>
        {amenities?.length > 0 && (
          <ul className="mt-3 flex flex-wrap gap-2">
            {amenities.slice(0, 3).map((a) => (
              <li key={a} className="rounded-full bg-surface-container-low px-2 py-0.5 font-sans text-xs text-on-surface-variant">
                {a}
              </li>
            ))}
          </ul>
        )}
        <div className="mt-6 flex items-center justify-between gap-4">
          <span className="font-serif text-lg font-semibold">From ${priceFrom}/night</span>
          <Link
            to={{ pathname: '/hotels', hash: `hotel-${id}` }}
            className="font-sans text-sm font-semibold text-primary hover:text-primary-container"
          >
            Jump to stay
          </Link>
        </div>
      </div>
    </div>
  );
}
