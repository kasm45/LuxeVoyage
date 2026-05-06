import { Link, useParams } from 'react-router-dom';
import BookingWidget from '../components/BookingWidget.jsx';
import { getDestinationById } from '../data/destinations.js';

export default function DestinationDetail() {
  const { id } = useParams();
  const d = getDestinationById(id);

  if (!d) {
    return (
      <div className="mx-auto max-w-container px-gutter py-24 text-center">
        <h1 className="font-serif text-h2">Destination not found</h1>
        <Link to="/destinations" className="mt-6 inline-block font-sans font-semibold text-primary">
          ← Back to destinations
        </Link>
      </div>
    );
  }

  return (
    <article className="pb-20">
      <div className="relative h-[420px] w-full overflow-hidden md:h-[520px]">
        <img src={d.image} alt={d.name} className="h-full w-full object-cover" />
        <div className="absolute inset-0 bg-gradient-to-t from-background via-transparent to-transparent" />
        <div className="absolute bottom-0 left-0 w-full px-gutter pb-10">
          <div className="mx-auto flex max-w-container flex-wrap gap-2">
            {d.tags?.map((t) => (
              <span
                key={t}
                className="rounded-[40px] bg-white/90 px-3 py-1 font-sans text-label-caps uppercase text-on-secondary-container backdrop-blur"
              >
                {t}
              </span>
            ))}
          </div>
          <div className="mx-auto mt-4 max-w-container">
            <h1 className="font-serif text-4xl font-bold text-white drop-shadow md:text-h1">{d.name}</h1>
            <p className="mt-3 max-w-2xl font-sans text-body-lg text-white/90">{d.excerpt}</p>
          </div>
        </div>
      </div>

      <div className="mx-auto grid max-w-container gap-12 px-gutter py-12 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <h2 className="font-serif text-h2 text-on-surface">Overview</h2>
          <p className="mt-4 font-sans text-body-lg text-on-surface-variant">{d.description}</p>
          <h3 className="mt-10 font-serif text-xl font-semibold text-on-surface">Signature moments</h3>
          <ul className="mt-4 list-inside list-disc space-y-2 font-sans text-body-md text-on-surface-variant">
            {d.highlights.map((h) => (
              <li key={h}>{h}</li>
            ))}
          </ul>
        </div>
        <aside className="lg:col-span-1">
          <BookingWidget title="Plan your stay" pricePerNight={d.priceFrom} />
          <Link
            to="/contact"
            className="mt-6 block text-center font-sans text-sm font-semibold text-primary hover:text-primary-container"
          >
            Talk to a concierge →
          </Link>
        </aside>
      </div>
    </article>
  );
}
