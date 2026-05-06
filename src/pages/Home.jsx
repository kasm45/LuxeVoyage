import { Link } from 'react-router-dom';
import DestinationCard from '../components/DestinationCard.jsx';
import ExperienceCard from '../components/ExperienceCard.jsx';
import { destinations } from '../data/destinations.js';
import { experiences } from '../data/experiences.js';

export default function Home() {
  const heroImg =
    'https://lh3.googleusercontent.com/aida-public/AB6AXuBmuxz78CkUOTf5-2i-K-RA3SRKhZP0sQOPSNKZzrmyEwu8oLqJpQG6x6Mfym3dnnHxXx2g9TQkXA1Clvv-XI2lvoJXEdh8jaqGFhm5LuSC2Mb3gQxMxhCTyvj_Grfw6cxxBHdHV3YvV8Kln4kiivHHvHzcClnjwLBPWhaV5kFpGjhyjS6ULK8Wzpo6hqkTcvre-RpRnza8Xuh5q0n9Df6vgUzkdbo6U8ZSEOkurVGq5WPTb7Cps2vuto6xjjoWAf2OAfhfu254U-k';

  const featured = destinations.slice(0, 3);
  const exp = experiences.slice(0, 3);

  return (
    <>
      <header className="relative flex min-h-[600px] w-full items-center justify-center overflow-hidden md:h-[85vh]">
        <div className="absolute inset-0 bg-slate-900">
          <img
            alt="Coastal landscape"
            src={heroImg}
            className="h-full w-full object-cover opacity-80 mix-blend-overlay"
          />
          <div className="absolute inset-0 bg-gradient-to-t from-background/90 via-transparent to-transparent" />
        </div>
        <div className="relative z-10 mx-auto flex w-full max-w-container flex-col items-center px-gutter text-center">
          <h1 className="font-serif text-4xl font-bold leading-tight tracking-tight text-white text-glass-shadow md:text-h1 md:leading-[1.2]">
            Refined Exploration Awaits.
          </h1>
          <p className="mt-6 max-w-2xl font-sans text-body-lg text-white/90 text-glass-shadow">
            Discover curated destinations and seamless high-end hospitality tailored for the discerning traveler.
          </p>
          <div className="glass-panel relative z-20 mt-10 flex w-full max-w-4xl flex-col items-center gap-4 rounded-xl p-4 shadow-[0px_4px_20px_rgba(15,23,42,0.15)] md:flex-row md:p-4">
            <div className="relative w-full flex-1">
              <span className="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-secondary">location_on</span>
              <input
                readOnly
                placeholder="Where to?"
                className="w-full rounded-lg border-transparent bg-white/80 py-4 pl-12 pr-4 font-sans text-on-surface outline-none ring-primary/20 transition-all focus:border-primary focus:ring-2"
              />
            </div>
            <div className="relative w-full flex-1">
              <span className="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-secondary">calendar_month</span>
              <input
                readOnly
                placeholder="Dates"
                className="w-full rounded-lg border-transparent bg-white/80 py-4 pl-12 pr-4 font-sans text-on-surface outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              />
            </div>
            <Link
              to="/destinations"
              className="flex w-full items-center justify-center gap-2 whitespace-nowrap rounded-lg bg-gradient-to-r from-primary to-surface-tint px-8 py-4 font-sans text-sm font-semibold text-on-primary shadow-md transition-opacity hover:opacity-90 md:w-auto"
            >
              <span className="material-symbols-outlined text-xl">search</span>
              Explore
            </Link>
          </div>
        </div>
      </header>

      <div className="mx-auto flex w-full max-w-container flex-col gap-20 px-gutter py-16 md:py-xxl">
        <section>
          <div className="mb-10 flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
            <div>
              <h2 className="font-serif text-h2 text-on-surface">Iconic Destinations</h2>
              <p className="mt-2 font-sans text-body-md text-on-surface-variant">Handpicked locales for your next escape.</p>
            </div>
            <Link
              to="/destinations"
              className="flex items-center gap-1 font-sans text-sm font-semibold text-primary transition-colors hover:text-primary-container"
            >
              View all <span className="material-symbols-outlined text-base">arrow_forward</span>
            </Link>
          </div>
          <div className="grid auto-rows-[280px] grid-cols-1 gap-6 md:grid-cols-3 md:grid-rows-[repeat(2,220px)]">
            <DestinationCard destination={featured[0]} featured className="md:col-span-2 md:row-span-2 min-h-[300px]" />
            <DestinationCard destination={featured[1]} className="min-h-[220px]" />
            <DestinationCard destination={featured[2]} className="min-h-[220px]" />
          </div>
        </section>

        <section className="overflow-hidden">
          <div className="mb-10">
            <h2 className="font-serif text-h2 text-on-surface">Curated Experiences</h2>
            <p className="mt-2 font-sans text-body-md text-on-surface-variant">
              Elevate your journey with exclusive activities.
            </p>
          </div>
          <div className="-mx-gutter flex gap-6 overflow-x-auto px-gutter pb-4 scrollbar-hide snap-x snap-mandatory md:mx-0 md:px-0">
            {exp.map((e) => (
              <ExperienceCard key={e.id} experience={e} />
            ))}
          </div>
          <div className="mt-8 text-center">
            <Link to="/experiences" className="font-sans text-sm font-semibold text-primary hover:text-primary-container">
              Browse all experiences →
            </Link>
          </div>
        </section>

        <section className="rounded-2xl bg-surface-container-low p-10 text-center md:p-16">
          <h2 className="font-serif text-h2 text-on-surface">The LuxeVoyage Standard</h2>
          <div className="mx-auto mt-12 grid max-w-5xl grid-cols-1 gap-12 md:grid-cols-3">
            {[
              { icon: 'verified_user', title: 'Vetted Excellence', text: 'Every property and partner is meticulously inspected.' },
              { icon: 'support_agent', title: '24/7 Concierge', text: 'Dedicated support anticipating needs before they arise.' },
              { icon: 'diamond', title: 'Exclusive Access', text: 'Private experiences unavailable to the general public.' },
            ].map((x) => (
              <div key={x.title} className="flex flex-col items-center">
                <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-secondary-container text-primary">
                  <span className="material-symbols-outlined text-3xl">{x.icon}</span>
                </div>
                <h3 className="font-serif text-xl font-semibold text-on-surface">{x.title}</h3>
                <p className="mt-2 max-w-xs font-sans text-body-md text-on-surface-variant">{x.text}</p>
              </div>
            ))}
          </div>
        </section>
      </div>
    </>
  );
}
