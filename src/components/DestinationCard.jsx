import { Link } from 'react-router-dom';

export default function DestinationCard({ destination, featured = false, className = '' }) {
  const { id, name, image, excerpt, tags } = destination;

  return (
    <Link
      to={`/destinations/${id}`}
      className={`group relative cursor-pointer overflow-hidden rounded-2xl shadow-card transition-shadow hover:shadow-cardHover ${className}`}
    >
      <img
        src={image}
        alt={name}
        className="h-full min-h-[220px] w-full object-cover transition-transform duration-700 group-hover:scale-105"
        style={{ border: '0.5px solid rgba(255,255,255,0.1)' }}
      />
      <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent" />
      <div className="absolute bottom-0 left-0 w-full p-6">
        {tags?.length > 0 && (
          <div className="mb-2 flex flex-wrap gap-2">
            {tags.map((t) => (
              <span
                key={t}
                className="rounded-[40px] bg-secondary-container/90 px-3 py-1 font-sans text-label-caps uppercase text-on-secondary-container backdrop-blur-sm"
              >
                {t}
              </span>
            ))}
          </div>
        )}
        <h3 className="font-serif text-xl font-semibold text-white md:text-h3">{name}</h3>
        <p className={`mt-1 font-sans text-sm text-white/80 ${featured ? 'line-clamp-2' : 'line-clamp-1'}`}>{excerpt}</p>
      </div>
    </Link>
  );
}
