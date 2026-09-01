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

function previewImage(event) {
    const input = event.target;
    const preview = document.getElementById('previewImage');

    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        preview.src = '/Img/Placeholder.jpg';
    }
}
function clearPosterSelection() {
    const input = document.getElementById('PosterFileInput');
    const preview = document.getElementById('previewImage');

    try {
        if (preview && preview.src && preview.src.startsWith('blob:')) {
            URL.revokeObjectURL(preview.src);
        }
    } catch (e) {
    }

    if (input) {
        try {
            input.value = '';
        } catch (e) {
            const newInput = input.cloneNode(true);
            input.parentNode.replaceChild(newInput, input);
        }
    }

    if (preview) {
        preview.src = '/Img/Placeholder.jpg';
    }
}
function updateCount(el) {
    document.getElementById('charCount').innerText = el.value.length;
    }