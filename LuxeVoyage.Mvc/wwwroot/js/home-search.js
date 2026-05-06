/**
 * Homepage hero: destination suggestions (all DB destinations, English filter) +
 * custom English calendar (no native date picker).
 */
(function () {
  var MONTHS_EN = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];
  var MONTHS_SHORT_EN = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  var WEEKDAYS_EN = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

  var dataEl = document.getElementById('lv-home-destinations-data');
  var form = document.getElementById('lv-home-search-form');
  if (!form || !dataEl) return;

  var destinations = [];
  try {
    destinations = JSON.parse(dataEl.textContent || '[]');
  } catch (e) {
    destinations = [];
  }

  var queryInput = document.getElementById('lv-home-query');
  var destIdInput = document.getElementById('lv-home-destination-id');
  var suggestionsEl = document.getElementById('lv-home-suggestions');
  var whereWrap = document.getElementById('lv-home-where-wrap');

  var datesDisplay = document.getElementById('lv-home-dates-display');
  var dateHidden = document.getElementById('lv-home-date');
  var dateEndHidden = document.getElementById('lv-home-date-end');
  var datePanel = document.getElementById('lv-home-date-panel');
  var datesWrap = document.getElementById('lv-home-dates-wrap');
  var calendarMount = document.getElementById('lv-calendar-mount');

  /** --- Destination suggestions --- */
  function norm(s) {
    return (s || '').toLowerCase();
  }

  function haystack(d) {
    if (d.searchText) return norm(d.searchText);
    return [
      norm(d.title),
      norm(d.locationLabel),
      norm(d.regionLabel),
      norm(d.city),
      norm(d.country)
    ].join(' ');
  }

  function filterDestinations(q) {
    var n = norm(q).trim();
    if (!n) return destinations.slice();
    return destinations.filter(function (d) {
      return haystack(d).indexOf(n) !== -1;
    });
  }

  function renderSuggestions(list, queryText) {
    if (!suggestionsEl) return;
    suggestionsEl.innerHTML = '';
    var qTrim = (queryText || '').trim();
    if (!list.length) {
      if (qTrim.length > 0) {
        var empty = document.createElement('div');
        empty.className = 'px-4 py-6 text-sm text-on-surface-variant text-center';
        empty.textContent = 'No matching destinations. Try another spelling or browse all destinations.';
        suggestionsEl.appendChild(empty);
        suggestionsEl.classList.remove('hidden');
      } else {
        suggestionsEl.classList.add('hidden');
      }
      return;
    }
    var heading = document.createElement('div');
    heading.className = 'lv-home-suggestions-header';
    heading.textContent = 'Popular destinations — search or pick from the list';
    suggestionsEl.appendChild(heading);

    list.forEach(function (d) {
      var btn = document.createElement('button');
      btn.type = 'button';
      btn.className =
        'lv-home-suggestion-item flex w-full items-start gap-3 border-b border-outline-variant/30 px-4 py-3 text-left text-on-surface hover:bg-primary/5 last:border-0';
      var sub = [d.city, d.country, d.regionLabel].filter(Boolean).join(' · ');
      if (!sub) sub = [d.locationLabel, d.regionLabel].filter(Boolean).join(' · ');
      btn.innerHTML =
        '<span class="material-symbols-outlined mt-0.5 shrink-0 text-primary text-[22px]">travel_explore</span>' +
        '<span class="min-w-0"><span class="block font-medium leading-snug">' +
        escapeHtml(d.title) +
        '</span><span class="block text-sm text-on-surface-variant">' +
        escapeHtml(sub) +
        '</span></span>';
      btn.addEventListener('click', function () {
        if (queryInput) queryInput.value = d.title;
        if (destIdInput) destIdInput.value = String(d.id);
        suggestionsEl.classList.add('hidden');
      });
      suggestionsEl.appendChild(btn);
    });
    suggestionsEl.classList.remove('hidden');
    adjustPopoverFlip(suggestionsEl);
  }

  function escapeHtml(str) {
    if (!str) return '';
    return String(str)
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;');
  }

  function openSuggestions() {
    var q = queryInput ? queryInput.value : '';
    renderSuggestions(filterDestinations(q), q);
  }

  function closeSuggestions() {
    if (suggestionsEl) suggestionsEl.classList.add('hidden');
  }

  /** Move popover above anchor if it would extend past the viewport bottom */
  function adjustPopoverFlip(panel) {
    if (!panel || panel.classList.contains('hidden')) return;
    panel.classList.remove('lv-popover-flip');
    requestAnimationFrame(function () {
      var margin = 12;
      var r = panel.getBoundingClientRect();
      if (r.bottom <= window.innerHeight - margin) return;
      panel.classList.add('lv-popover-flip');
      var r2 = panel.getBoundingClientRect();
      if (r2.top < margin) panel.classList.remove('lv-popover-flip');
    });
  }

  window.addEventListener('resize', function () {
    adjustPopoverFlip(suggestionsEl);
    adjustPopoverFlip(datePanel);
  });

  /** --- Calendar (English, custom DOM only) --- */
  var viewYear;
  var viewMonth;
  var selStart;
  var selEnd;

  function pad2(n) {
    return (n < 10 ? '0' : '') + n;
  }

  function parseIsoLocal(iso) {
    if (!iso || typeof iso !== 'string') return null;
    var p = iso.split('-');
    if (p.length !== 3) return null;
    var y = parseInt(p[0], 10);
    var m = parseInt(p[1], 10) - 1;
    var day = parseInt(p[2], 10);
    if (isNaN(y) || isNaN(m) || isNaN(day)) return null;
    return new Date(y, m, day);
  }

  function toIsoLocal(d) {
    if (!d || isNaN(d.getTime())) return '';
    return d.getFullYear() + '-' + pad2(d.getMonth() + 1) + '-' + pad2(d.getDate());
  }

  function sameDay(a, b) {
    if (!a || !b) return false;
    return (
      a.getFullYear() === b.getFullYear() &&
      a.getMonth() === b.getMonth() &&
      a.getDate() === b.getDate()
    );
  }

  function dayLTE(a, b) {
    var ta = a.getFullYear() * 10000 + (a.getMonth() + 1) * 100 + a.getDate();
    var tb = b.getFullYear() * 10000 + (b.getMonth() + 1) * 100 + b.getDate();
    return ta <= tb;
  }

  function formatEnglish(d) {
    if (!d || isNaN(d.getTime())) return '';
    return MONTHS_SHORT_EN[d.getMonth()] + ' ' + d.getDate() + ', ' + d.getFullYear();
  }

  function syncDisplayFromHidden() {
    if (!datesDisplay) return;
    var s = dateHidden && dateHidden.value ? parseIsoLocal(dateHidden.value) : null;
    var e = dateEndHidden && dateEndHidden.value ? parseIsoLocal(dateEndHidden.value) : null;
    if (!s) {
      datesDisplay.value = '';
      return;
    }
    if (e && dayLTE(s, e) && !sameDay(s, e)) {
      datesDisplay.value = formatEnglish(s) + ' – ' + formatEnglish(e);
    } else {
      datesDisplay.value = formatEnglish(s);
    }
  }

  function loadSelectionFromHidden() {
    selStart = dateHidden && dateHidden.value ? parseIsoLocal(dateHidden.value) : null;
    selEnd =
      dateEndHidden && dateEndHidden.value ? parseIsoLocal(dateEndHidden.value) : null;
    if (selStart) {
      viewYear = selStart.getFullYear();
      viewMonth = selStart.getMonth();
    } else {
      var t = new Date();
      viewYear = t.getFullYear();
      viewMonth = t.getMonth();
    }
  }

  function renderCalendar() {
    if (!calendarMount) return;

    var header = document.createElement('div');
    header.className = 'lv-cal-header';
    var prev = document.createElement('button');
    prev.type = 'button';
    prev.className = 'lv-cal-nav';
    prev.setAttribute('aria-label', 'Previous month');
    prev.textContent = '‹';
    prev.addEventListener('click', function (ev) {
      ev.stopPropagation();
      viewMonth--;
      if (viewMonth < 0) {
        viewMonth = 11;
        viewYear--;
      }
      renderCalendar();
    });

    var title = document.createElement('span');
    title.className = 'lv-cal-title';
    title.textContent = MONTHS_EN[viewMonth] + ' ' + viewYear;

    var next = document.createElement('button');
    next.type = 'button';
    next.className = 'lv-cal-nav';
    next.setAttribute('aria-label', 'Next month');
    next.textContent = '›';
    next.addEventListener('click', function (ev) {
      ev.stopPropagation();
      viewMonth++;
      if (viewMonth > 11) {
        viewMonth = 0;
        viewYear++;
      }
      renderCalendar();
    });

    header.appendChild(prev);
    header.appendChild(title);
    header.appendChild(next);

    var hint = document.createElement('p');
    hint.className = 'lv-cal-hint';
    hint.textContent = 'Select start date, then optional end date. English calendar.';

    var weekRow = document.createElement('div');
    weekRow.className = 'lv-cal-weekdays';
    WEEKDAYS_EN.forEach(function (w) {
      var span = document.createElement('span');
      span.textContent = w;
      weekRow.appendChild(span);
    });

    var grid = document.createElement('div');
    grid.className = 'lv-cal-grid';

    var first = new Date(viewYear, viewMonth, 1);
    var startPad = (first.getDay() + 6) % 7;
    var daysInMonth = new Date(viewYear, viewMonth + 1, 0).getDate();
    var totalCells = Math.ceil((startPad + daysInMonth) / 7) * 7;

    var today = new Date();

    for (var i = 0; i < totalCells; i++) {
      var dayNum = i - startPad + 1;
      var cellDate;
      var muted = false;
      if (dayNum < 1 || dayNum > daysInMonth) {
        cellDate = new Date(viewYear, viewMonth, dayNum);
        muted = true;
      } else {
        cellDate = new Date(viewYear, viewMonth, dayNum);
      }

      var btn = document.createElement('button');
      btn.type = 'button';
      btn.className = 'lv-cal-day';
      if (muted) btn.classList.add('lv-cal-day--muted');
      btn.textContent = String(cellDate.getDate());

      if (!muted && sameDay(cellDate, today)) btn.classList.add('lv-cal-day--today');

      var inRange = false;
      var isStart = false;
      var isEnd = false;
      if (!muted && selStart) {
        if (selEnd && dayLTE(selStart, selEnd)) {
          var lo = selStart;
          var hi = selEnd;
          if (!dayLTE(lo, hi)) {
            lo = selEnd;
            hi = selStart;
          }
          inRange = dayLTE(lo, cellDate) && dayLTE(cellDate, hi);
          isStart = sameDay(cellDate, selStart);
          isEnd = sameDay(cellDate, selEnd);
        } else {
          isStart = sameDay(cellDate, selStart);
        }
      }

      if (inRange && !isStart && !isEnd) btn.classList.add('lv-cal-day--in-range');
      if (isStart) btn.classList.add('lv-cal-day--range-start');
      if (isEnd) btn.classList.add('lv-cal-day--range-end');

      (function (cd, mutedFlag) {
        if (mutedFlag) {
          btn.addEventListener('click', function (ev) {
            ev.stopPropagation();
            viewYear = cd.getFullYear();
            viewMonth = cd.getMonth();
            renderCalendar();
          });
        } else {
          btn.addEventListener('click', function (ev) {
            ev.stopPropagation();
            if (!selStart || (selStart && selEnd)) {
              selStart = new Date(cd.getFullYear(), cd.getMonth(), cd.getDate());
              selEnd = null;
            } else {
              var end = new Date(cd.getFullYear(), cd.getMonth(), cd.getDate());
              if (sameDay(end, selStart)) {
                selEnd = null;
              } else if (dayLTE(end, selStart)) {
                selEnd = selStart;
                selStart = end;
              } else {
                selEnd = end;
              }
            }
            renderCalendar();
          });
        }
      })(cellDate, muted);

      grid.appendChild(btn);
    }

    var actions = document.createElement('div');
    actions.className = 'lv-cal-actions';

    function btnSecondary(label) {
      var b = document.createElement('button');
      b.type = 'button';
      b.className = 'lv-cal-btn lv-cal-btn-secondary';
      b.textContent = label;
      return b;
    }

    var btnToday = btnSecondary('Today');
    btnToday.addEventListener('click', function (ev) {
      ev.stopPropagation();
      var n = new Date();
      viewYear = n.getFullYear();
      viewMonth = n.getMonth();
      selStart = new Date(n.getFullYear(), n.getMonth(), n.getDate());
      selEnd = null;
      renderCalendar();
    });

    var btnClear = btnSecondary('Clear');
    btnClear.addEventListener('click', function (ev) {
      ev.stopPropagation();
      selStart = null;
      selEnd = null;
      renderCalendar();
    });

    var btnApply = document.createElement('button');
    btnApply.type = 'button';
    btnApply.className = 'lv-cal-btn lv-cal-btn-primary';
    btnApply.textContent = 'Apply';
    btnApply.addEventListener('click', function (ev) {
      ev.stopPropagation();
      if (dateHidden) dateHidden.value = selStart ? toIsoLocal(selStart) : '';
      if (dateEndHidden) {
        if (selStart && selEnd && dayLTE(selStart, selEnd) && !sameDay(selStart, selEnd))
          dateEndHidden.value = toIsoLocal(selEnd);
        else dateEndHidden.value = '';
      }
      syncDisplayFromHidden();
      if (datePanel) datePanel.classList.add('hidden');
    });

    actions.appendChild(btnToday);
    actions.appendChild(btnClear);
    actions.appendChild(btnApply);

    var bodyScroll = document.createElement('div');
    bodyScroll.className = 'lv-cal-body';
    bodyScroll.appendChild(hint);
    bodyScroll.appendChild(weekRow);
    bodyScroll.appendChild(grid);

    var root = document.createElement('div');
    root.className = 'lv-cal';
    root.appendChild(header);
    root.appendChild(bodyScroll);
    root.appendChild(actions);

    calendarMount.innerHTML = '';
    calendarMount.appendChild(root);
    adjustPopoverFlip(datePanel);
  }

  function closeDatePanel() {
    if (datePanel) datePanel.classList.add('hidden');
  }

  function openDatePanel() {
    closeSuggestions();
    loadSelectionFromHidden();
    if (datePanel) datePanel.classList.remove('hidden');
    renderCalendar();
  }

  if (queryInput) {
    queryInput.addEventListener('focus', openSuggestions);
    queryInput.addEventListener('click', openSuggestions);
    queryInput.addEventListener('input', function () {
      if (!queryInput.value.trim() && destIdInput) destIdInput.value = '';
      renderSuggestions(filterDestinations(queryInput.value), queryInput.value);
    });
  }

  if (datesDisplay) {
    datesDisplay.addEventListener('click', function (ev) {
      ev.preventDefault();
      if (datePanel && datePanel.classList.contains('hidden')) openDatePanel();
      else closeDatePanel();
    });
    datesDisplay.addEventListener('focus', function () {
      openDatePanel();
    });
  }

  document.addEventListener('click', function (ev) {
    var t = ev.target;
    if (!form.contains(t)) {
      closeSuggestions();
      closeDatePanel();
      return;
    }
    if (whereWrap && whereWrap.contains(t) && suggestionsEl && suggestionsEl.contains(t)) return;
    if (whereWrap && whereWrap.contains(t) && queryInput && (t === queryInput || queryInput.contains(t)))
      return;
    if (whereWrap && !whereWrap.contains(t)) closeSuggestions();
    if (datesWrap && !datesWrap.contains(t)) closeDatePanel();
  });

  document.addEventListener('keydown', function (ev) {
    if (ev.key === 'Escape') {
      closeSuggestions();
      closeDatePanel();
    }
  });

  syncDisplayFromHidden();
})();
