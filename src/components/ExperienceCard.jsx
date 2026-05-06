import { Link } from 'react-router-dom';

export default function ExperienceCard({ experience, variant = 'rail' }) {
  const { id, title, excerpt, priceFrom, image, icon } = experience;
  const rail =
    variant === 'rail'
      ? 'min-w-[280px] max-w-[320px] shrink-0 snap-start sm:min-w-[320px]'
      : 'w-full max-w-none';

  return (
    <Link
      to={`/experiences/${id}`}
      className={`group flex flex-col overflow-hidden rounded-2xl border border-surface-variant bg-surface shadow-card transition-shadow hover:shadow-cardHover ${rail}`}
    >
      <div className="relative h-[200px] w-full overflow-hidden">
        <img
          src={image}
          alt={title}
          className="h-full w-full object-cover transition-transform duration-500 group-hover:scale-105"
          style={{ border: '0.5px solid rgba(255,255,255,0.1)' }}
        />
      </div>
      <div className="flex flex-1 flex-col p-6">
        <div className="mb-2 flex items-start justify-between gap-2">
          <h3 className="font-serif text-lg font-semibold leading-tight text-on-surface md:text-xl">{title}</h3>
          {icon && (
            <span className="material-symbols-outlined shrink-0 text-primary" aria-hidden>
              {icon}
            </span>
          )}
        </div>
        <p className="mb-4 font-sans text-sm text-on-surface-variant">{excerpt}</p>
        <div className="mt-auto flex items-center justify-between border-t border-surface-variant pt-4">
          <span className="font-sans font-semibold text-on-surface">From ${priceFrom}</span>
          <span className="font-sans text-sm font-semibold text-primary group-hover:text-primary-container">Details →</span>
        </div>
      </div>
    </Link>
  );
}
