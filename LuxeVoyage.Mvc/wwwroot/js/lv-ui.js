/** Client helpers + shared toast + image fallback */
(function () {
  var fallbackImage = '/images/placeholders/lv-placeholder.svg';

  window.lvPlaceholder = function (e) {
    if (e && typeof e.preventDefault === 'function') {
      e.preventDefault();
    }
    return false;
  };

  var leaveMsDefault = 260;

  function dismissFlashEl(el) {
    if (!el || !el.parentNode) return;
    el.classList.add('lv-toast--leave');
    setTimeout(function () {
      if (el.parentNode) el.parentNode.removeChild(el);
    }, leaveMsDefault);
  }

  /**
   * Programmatic toast (e.g. favorites). Uses same styling as server flash messages.
   * @param {string} text
   * @param {boolean} isError
   * @param {{ durationMs?: number }} [opts]
   */
  window.lvShowToast = function (text, isError, opts) {
    opts = opts || {};
    var ms = opts.durationMs != null ? opts.durationMs : 3200;
    var stack = document.querySelector('.lv-toast-stack');
    if (!stack) {
      stack = document.createElement('div');
      stack.className = 'lv-toast-stack';
      stack.setAttribute('aria-live', 'polite');
      stack.setAttribute('aria-atomic', 'true');
      document.body.appendChild(stack);
    }
    var el = document.createElement('div');
    el.className =
      'lv-toast ' + (isError ? 'lv-toast-error' : 'lv-toast-success');
    el.setAttribute('role', isError ? 'alert' : 'status');
    el.innerHTML =
      '<span class="material-symbols-outlined ' +
      (isError ? 'text-error' : 'text-primary') +
      '" aria-hidden="true">' +
      (isError ? 'error' : 'check_circle') +
      '</span><p class="m-0">' + text + '</p>';
    stack.appendChild(el);
    setTimeout(function () {
      dismissFlashEl(el);
    }, ms);
  };

  function initServerFlash() {
    document.querySelectorAll('.lv-toast[data-lv-auto-dismiss]').forEach(function (el) {
      var ms = parseInt(el.getAttribute('data-lv-auto-dismiss'), 10);
      if (isNaN(ms)) ms = 3200;
      setTimeout(function () {
        dismissFlashEl(el);
      }, ms);
    });
  }

  function applyImageFallback(img) {
    if (!img) return;
    if (!img.getAttribute('src') || !img.getAttribute('src').trim()) {
      img.setAttribute('src', fallbackImage);
    }
    img.onerror = function () {
      if (img.getAttribute('src') === fallbackImage) return;
      img.onerror = null;
      img.setAttribute('src', fallbackImage);
    };
  }

  function initImageFallbacks() {
    document.querySelectorAll('img').forEach(applyImageFallback);
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function () {
      initServerFlash();
      initImageFallbacks();
    });
  } else {
    initServerFlash();
    initImageFallbacks();
  }
})();
