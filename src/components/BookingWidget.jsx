import { useMemo, useState } from 'react';

/**
 * Dynamic pricing:
 * - stay: nightly rate × nights, adjustment for extra guests
 * - experience: flat starting price scaled by guests
 */
export default function BookingWidget({
  title = 'Reserve',
  pricePerNight,
  variant = 'stay',
  currency = '$',
  minGuests = 1,
  maxGuests = 8,
  maxNights = 21,
}) {
  const [guests, setGuests] = useState(2);
  const [nights, setNights] = useState(3);

  const { subtotal, fees, total } = useMemo(() => {
    if (variant === 'experience') {
      const base = pricePerNight;
      const guestMult = 1 + Math.max(0, guests - 1) * 0.15;
      const sub = base * guestMult;
      const service = sub * 0.06;
      return {
        subtotal: Math.round(sub),
        fees: Math.round(service),
        total: Math.round(sub + service),
      };
    }
    const base = pricePerNight * nights;
    const extraGuests = Math.max(0, guests - 2);
    const guestAdj = base * extraGuests * 0.12;
    const service = base * 0.08;
    const sub = base + guestAdj;
    const tot = sub + service;
    return {
      subtotal: Math.round(sub),
      fees: Math.round(service),
      total: Math.round(tot),
    };
  }, [pricePerNight, nights, guests, variant]);

  return (
    <div className="rounded-2xl border border-surface-variant bg-white p-6 shadow-card">
      <h3 className="font-serif text-h3 text-on-surface">{title}</h3>
      <p className="mt-1 font-sans text-sm text-on-surface-variant">
        {variant === 'experience' ? (
          <>
            From {currency}
            {pricePerNight} · adjusts by party size
          </>
        ) : (
          <>
            {currency}
            {pricePerNight} / night · estimated total updates live
          </>
        )}
      </p>

      <div className="mt-6 space-y-4">
        <label className="block">
          <span className="font-sans text-label-caps uppercase text-on-surface-variant">Guests</span>
          <input
            type="range"
            min={minGuests}
            max={maxGuests}
            value={guests}
            onChange={(e) => setGuests(Number(e.target.value))}
            className="mt-2 w-full accent-primary"
          />
          <div className="mt-1 font-sans text-sm text-on-surface">{guests} guests</div>
        </label>

        {variant === 'stay' && (
          <label className="block">
            <span className="font-sans text-label-caps uppercase text-on-surface-variant">Nights</span>
            <input
              type="range"
              min={1}
              max={maxNights}
              value={nights}
              onChange={(e) => setNights(Number(e.target.value))}
              className="mt-2 w-full accent-primary"
            />
            <div className="mt-1 font-sans text-sm text-on-surface">{nights} nights</div>
          </label>
        )}
      </div>

      <dl className="mt-6 space-y-2 border-t border-surface-variant pt-6 font-sans text-sm">
        <div className="flex justify-between text-on-surface-variant">
          <dt>Stay subtotal</dt>
          <dd>
            {currency}
            {subtotal.toLocaleString()}
          </dd>
        </div>
        <div className="flex justify-between text-on-surface-variant">
          <dt>Service &amp; planning</dt>
          <dd>
            {currency}
            {fees.toLocaleString()}
          </dd>
        </div>
        <div className="flex justify-between border-t border-surface-variant pt-3 font-semibold text-on-surface">
          <dt>Estimated total</dt>
          <dd>
            {currency}
            {total.toLocaleString()}
          </dd>
        </div>
      </dl>

      <button
        type="button"
        className="mt-6 w-full rounded-2xl bg-gradient-to-r from-primary to-surface-tint py-3.5 font-sans text-sm font-semibold text-on-primary shadow-card transition-opacity hover:opacity-90"
      >
        Continue
      </button>
    </div>
  );
}
