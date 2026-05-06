export default function About() {
  return (
    <div className="mx-auto max-w-container px-gutter py-16">
      <h1 className="font-serif text-h1 text-on-surface">About LuxeVoyage</h1>
      <p className="mt-6 max-w-3xl font-sans text-body-lg text-on-surface-variant">
        We are a collective of travel architects obsessed with the small details: the second pour of wine, the quieter trailhead,
        the room that actually faces the volcano. LuxeVoyage pairs editorial inspiration with operational rigor so your trip feels
        effortless on the surface and seamless underneath.
      </p>
      <div className="mt-12 grid gap-10 md:grid-cols-2">
        <section className="rounded-2xl border border-surface-variant bg-white p-8 shadow-card">
          <h2 className="font-serif text-h3 text-on-surface">Our promise</h2>
          <p className="mt-4 font-sans text-body-md text-on-surface-variant">
            Transparent pricing, vetted partners, and humans on call when plans evolve.
          </p>
        </section>
        <section className="rounded-2xl border border-surface-variant bg-white p-8 shadow-card">
          <h2 className="font-serif text-h3 text-on-surface">How we work</h2>
          <p className="mt-4 font-sans text-body-md text-on-surface-variant">
            We listen first, design second, and stay connected through your journey — no anonymous call centers.
          </p>
        </section>
      </div>
    </div>
  );
}
