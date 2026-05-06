import { Link } from 'react-router-dom';

export default function Footer() {
  return (
    <footer className="mt-20 w-full border-t border-slate-800 bg-slate-900 dark:bg-black">
      <div className="mx-auto grid max-w-7xl grid-cols-1 gap-12 px-6 py-16 md:grid-cols-4 md:px-12 md:py-20">
        <div>
          <div className="mb-4 font-serif text-xl font-bold tracking-tight text-white">LuxeVoyage</div>
          <p className="font-serif text-sm text-slate-400">
            © {new Date().getFullYear()} LuxeVoyage Exploration. All rights reserved.
          </p>
        </div>
        <div className="col-span-1 flex flex-col flex-wrap gap-x-12 gap-y-4 text-sm md:col-span-3 md:flex-row md:justify-end">
          <Link className="text-slate-400 transition-colors hover:text-white hover:underline hover:underline-offset-4" to="/about">
            About
          </Link>
          <Link className="text-slate-400 transition-colors hover:text-white hover:underline hover:underline-offset-4" to="/contact">
            Contact Us
          </Link>
          <a className="text-slate-400 transition-colors hover:text-white hover:underline hover:underline-offset-4" href="#">
            Privacy Policy
          </a>
          <a className="text-slate-400 transition-colors hover:text-white hover:underline hover:underline-offset-4" href="#">
            Terms of Service
          </a>
          <a className="text-slate-400 transition-colors hover:text-white hover:underline hover:underline-offset-4" href="#">
            Sustainability
          </a>
        </div>
      </div>
    </footer>
  );
}
