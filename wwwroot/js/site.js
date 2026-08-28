// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {

    const key = 'mv-theme';
    const toggle = document.getElementById('theme-toggle');

    function apply(theme) {
        if (theme === 'dark') document.body.classList.add('dark-theme');
        else document.body.classList.remove('dark-theme');
        if (toggle) toggle.textContent = theme === 'dark' ? '☀️' : '🌙';
    }

    const stored = localStorage.getItem(key);
    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    const initial = stored || (prefersDark ? 'dark' : 'light');
    apply(initial);

    if (toggle) {
        toggle.addEventListener('click', function () {
            const nowDark = document.body.classList.contains('dark-theme');
            const next = nowDark ? 'light' : 'dark';
            apply(next);
            localStorage.setItem(key, next);
        });
    }
})();