export default function Contact() {
  return (
    <div className="mx-auto max-w-container px-gutter py-16">
      <div className="grid gap-12 lg:grid-cols-2">
        <div>
          <h1 className="font-serif text-h1 text-on-surface">Contact</h1>
          <p className="mt-4 font-sans text-body-lg text-on-surface-variant">
            Tell us where you are dreaming — we will reply with a curated direction within one business day.
          </p>
          <ul className="mt-10 space-y-4 font-sans text-body-md text-on-surface">
            <li>
              <span className="text-on-surface-variant">Concierge:</span>{' '}
              <a className="font-semibold text-primary hover:text-primary-container" href="mailto:hello@luxevoyage.example">
                hello@luxevoyage.example
              </a>
            </li>
            <li>
              <span className="text-on-surface-variant">Studio hours:</span> Mon–Sat, 9a–7p CET
            </li>
          </ul>
        </div>
        <form
          className="glass-panel rounded-2xl p-8 shadow-card"
          onSubmit={(e) => {
            e.preventDefault();
          }}
        >
          <label className="block font-sans text-label-caps uppercase text-on-surface-variant">
            Name
            <input
              className="mt-2 w-full rounded-xl border border-surface-variant bg-white/90 px-4 py-3 font-sans text-body-md outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              placeholder="Your name"
            />
          </label>
          <label className="mt-6 block font-sans text-label-caps uppercase text-on-surface-variant">
            Email
            <input
              type="email"
              className="mt-2 w-full rounded-xl border border-surface-variant bg-white/90 px-4 py-3 font-sans text-body-md outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              placeholder="you@example.com"
            />
          </label>
          <label className="mt-6 block font-sans text-label-caps uppercase text-on-surface-variant">
            Message
            <textarea
              rows={4}
              className="mt-2 w-full rounded-xl border border-surface-variant bg-white/90 px-4 py-3 font-sans text-body-md outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              placeholder="Destinations, dates, celebration?"
            />
          </label>
          <button
            type="submit"
            className="mt-8 w-full rounded-2xl bg-gradient-to-r from-primary to-surface-tint py-3.5 font-sans text-sm font-semibold text-on-primary shadow-card"
          >
            Send message
          </button>
        </form>
      </div>
    </div>
  );
}
