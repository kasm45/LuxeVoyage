import { Link, useParams } from 'react-router-dom';
import BookingWidget from '../components/BookingWidget.jsx';
import { getExperienceById } from '../data/experiences.js';

export default function ExperienceDetail() {
  const { id } = useParams();
  const e = getExperienceById(id);

  if (!e) {
    return (
      <div className="mx-auto max-w-container px-gutter py-24 text-center">
        <h1 className="font-serif text-h2">Experience not found</h1>
        <Link to="/experiences" className="mt-6 inline-block font-sans font-semibold text-primary">
          ← Back to experiences
        </Link>
      </div>
    );
  }

  return (
    <article className="pb-20">
      <div className="relative h-[380px] w-full overflow-hidden md:h-[460px]">
        <img src={e.image} alt={e.title} className="h-full w-full object-cover" />
        <div className="absolute inset-0 bg-gradient-to-t from-background via-black/20 to-transparent" />
        <div className="absolute bottom-0 left-0 w-full px-gutter pb-10">
          <div className="mx-auto max-w-container">
            <span className="inline-block rounded-full bg-white/90 px-3 py-1 font-sans text-label-caps uppercase text-on-secondary-container">
              {e.category} · {e.region}
            </span>
            <h1 className="mt-4 font-serif text-4xl font-bold text-white drop-shadow md:text-h1">{e.title}</h1>
            <p className="mt-3 max-w-2xl font-sans text-body-lg text-white/90">{e.excerpt}</p>
          </div>
        </div>
      </div>

      <div className="mx-auto grid max-w-container gap-12 px-gutter py-12 lg:grid-cols-3">
        <div className="lg:col-span-2">
          <h2 className="font-serif text-h2 text-on-surface">The experience</h2>
          <p className="mt-4 font-sans text-body-lg text-on-surface-variant">{e.description}</p>
          <p className="mt-6 font-sans text-sm font-semibold text-on-surface">
            Typical duration: <span className="font-normal text-on-surface-variant">{e.duration}</span>
          </p>
        </div>
        <aside>
          <BookingWidget title="Request this experience" pricePerNight={e.priceFrom} variant="experience" />
        </aside>
      </div>
    </article>
  );
}
