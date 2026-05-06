/**

 * AJAX favorite toggle: intercepts .lv-favorite-form submits when fetch is available.

 * Falls back to normal POST when JS disabled or fetch missing.

 * UI updates only after a successful JSON response; buttons disabled while in-flight.

 */

(function () {

  function lvFavoriteToast(text, isError) {
    if (typeof window.lvShowToast === 'function') {
      window.lvShowToast(text, isError, { durationMs: 2800 });
      return;
    }
    var el = document.createElement('div');
    el.className =
      'fixed top-20 left-1/2 -translate-x-1/2 z-[70] max-w-lg px-4 py-3 rounded-lg shadow-lg font-body-md ' +
      (isError
        ? 'bg-error-container text-on-error-container'
        : 'bg-surface-container-lowest border border-outline-variant text-on-surface');
    el.setAttribute('role', 'status');
    el.textContent = text;
    document.body.appendChild(el);
    setTimeout(function () {
      el.remove();
    }, 2800);
  }



  function lvSetFavoriteButtonState(button, isFavorite) {

    var span = button.querySelector('.lv-favorite-icon');

    if (!span) return;

    span.textContent = isFavorite ? 'favorite' : 'favorite_border';

    span.style.fontVariationSettings = "'FILL' " + (isFavorite ? '1' : '0');

    button.setAttribute('aria-pressed', isFavorite ? 'true' : 'false');

    button.setAttribute('title', isFavorite ? 'Remove favorite' : 'Save favorite');
    button.classList.toggle('is-active', !!isFavorite);

  }



  function lvSetFavoriteLoading(form, loading) {

    form.classList.toggle('lv-favorite-form--loading', loading);

    var btn = form.querySelector('button[type="submit"]');

    if (btn) {

      btn.disabled = loading;

      btn.setAttribute('aria-busy', loading ? 'true' : 'false');

    }

  }



  /** Parse JSON body when present (handles 200/400/401 JSON from MVC). */

  function parseResponseBody(response) {

    return response.json().catch(function () {

      return null;

    });

  }



  document.addEventListener(

    'submit',

    function (e) {

      var form = e.target;

      if (!form || !form.classList || !form.classList.contains('lv-favorite-form')) return;

      if (typeof window.fetch !== 'function') return;



      e.preventDefault();



      if (form.classList.contains('lv-favorite-form--loading')) return;



      var fd = new FormData(form);

      var action = form.getAttribute('action') || form.action;

      if (!action) {

        lvFavoriteToast('Could not submit favorite.', true);

        return;

      }



      lvSetFavoriteLoading(form, true);



      fetch(action, {

        method: 'POST',

        body: fd,

        credentials: 'same-origin',

        headers: { 'X-Requested-With': 'Fetch' }

      })

        .then(function (response) {

          return parseResponseBody(response).then(function (data) {

            return { response: response, data: data };

          });

        })

        .then(function (result) {

          var response = result.response;

          var data = result.data;



          if (response.status === 401 && data && data.loginUrl) {

            window.location.assign(data.loginUrl);

            return;

          }



          if (data && data.loginUrl) {

            window.location.assign(data.loginUrl);

            return;

          }



          if (!response.ok) {

            if (data && data.error) lvFavoriteToast(data.error, true);

            else lvFavoriteToast('Request could not be completed.', true);

            return;

          }



          if (!data) {

            lvFavoriteToast('Unexpected response from server.', true);

            return;

          }



          if (data.ok === false && data.error) {

            lvFavoriteToast(data.error, true);

            return;

          }



          if (data.ok && typeof data.isFavorite === 'boolean') {

            var btn = form.querySelector('button[type="submit"]');

            if (btn) lvSetFavoriteButtonState(btn, data.isFavorite);

            if (data.message) lvFavoriteToast(data.message, false);

            return;

          }



          lvFavoriteToast('Unexpected response from server.', true);

        })

        .catch(function () {

          lvFavoriteToast('Network error. Please try again.', true);

        })

        .finally(function () {

          lvSetFavoriteLoading(form, false);

        });

    },

    true

  );

})();


