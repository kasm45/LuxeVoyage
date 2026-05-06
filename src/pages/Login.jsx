import { Link } from 'react-router-dom';

export default function Login() {
  return (
    <div className="mx-auto flex min-h-[70vh] max-w-container flex-col items-center justify-center px-gutter py-16">
      <div className="w-full max-w-md rounded-2xl border border-surface-variant bg-white p-10 shadow-card">
        <h1 className="text-center font-serif text-h2 text-on-surface">Welcome back</h1>
        <p className="mt-2 text-center font-sans text-sm text-on-surface-variant">Sign in to manage journeys and saved stays.</p>
        <form
          className="mt-8 space-y-4"
          onSubmit={(e) => {
            e.preventDefault();
          }}
        >
          <label className="block font-sans text-label-caps uppercase text-on-surface-variant">
            Email
            <input
              type="email"
              className="mt-2 w-full rounded-xl border border-surface-variant bg-surface-container-low px-4 py-3 font-sans text-body-md outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              placeholder="you@example.com"
            />
          </label>
          <label className="block font-sans text-label-caps uppercase text-on-surface-variant">
            Password
            <input
              type="password"
              className="mt-2 w-full rounded-xl border border-surface-variant bg-surface-container-low px-4 py-3 font-sans text-body-md outline-none focus:border-primary focus:ring-2 focus:ring-primary/20"
              placeholder="••••••••"
            />
          </label>
          <button
            type="submit"
            className="w-full rounded-2xl bg-gradient-to-r from-primary to-surface-tint py-3.5 font-sans text-sm font-semibold text-on-primary shadow-card"
          >
            Continue
          </button>
        </form>
        <p className="mt-6 text-center font-sans text-sm text-on-surface-variant">
          Planning tools only —{' '}
          <Link to="/admin" className="font-semibold text-primary hover:text-primary-container">
            staff portal
          </Link>
        </p>
      </div>
    </div>
  );
}
