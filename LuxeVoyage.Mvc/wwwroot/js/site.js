// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
  const form = document.querySelector('[data-booking-capacity-form]');
  if (!form) return;

  const guestsInput = form.querySelector('input[name="Guests"]');
  const submitBtn = form.querySelector('[data-booking-submit]');
  const inlineMessage = form.querySelector('[data-capacity-inline-message]');
  if (!guestsInput || !submitBtn || !inlineMessage) return;

  const minRaw = guestsInput.getAttribute('data-min-guests');
  const maxRaw = guestsInput.getAttribute('data-max-guests');
  const kind = (guestsInput.getAttribute('data-kind') || '').toLowerCase();
  const minGuests = minRaw ? parseInt(minRaw, 10) : NaN;
  const maxGuests = maxRaw ? parseInt(maxRaw, 10) : NaN;

  if (Number.isNaN(minGuests) || Number.isNaN(maxGuests)) return;

  function capacityLabel() {
    const range = minGuests === maxGuests ? `${minGuests}` : `${minGuests}-${maxGuests}`;
    if (kind === 'experience') return `This experience supports ${range} guests.`;
    if (kind === 'tour') return `This tour supports ${range} guests.`;
    if (kind === 'stay') return `This stay supports ${range} guests.`;
    return `This option supports ${range} guests.`;
  }

  function validate() {
    const value = parseInt(guestsInput.value || '', 10);
    const invalid = Number.isNaN(value) || value < minGuests || value > maxGuests;
    submitBtn.disabled = invalid;
    if (invalid) {
      inlineMessage.textContent = capacityLabel();
      inlineMessage.classList.remove('hidden');
      guestsInput.setAttribute('aria-invalid', 'true');
    } else {
      inlineMessage.textContent = '';
      inlineMessage.classList.add('hidden');
      guestsInput.removeAttribute('aria-invalid');
    }
  }

  guestsInput.addEventListener('input', validate);
  guestsInput.addEventListener('change', validate);
  validate();
})();
