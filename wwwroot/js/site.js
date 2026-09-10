// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('.star');
    const ratingInput = document.getElementById('rating-value');
    const submitBtn = document.querySelector('input[type="submit"]');

    function updateSubmitButton() {
        const rating = parseInt(ratingInput.value);
        submitBtn.disabled = (rating < 1 || rating > 5);
    }

    stars.forEach(star => {
        star.addEventListener('click', function () {
            const value = parseInt(this.dataset.value);
            ratingInput.value = value;
            updateStars(value);
            updateSubmitButton();
        });

        star.addEventListener('mouseenter', function () {
            const value = parseInt(this.dataset.value);
            updateStars(value);
        });

        star.addEventListener('mouseleave', function () {
            const current = parseInt(ratingInput.value) || 0;
            updateStars(current);
        });
    });

    function updateStars(count) {
        stars.forEach((star, index) => {
            if (index < count) {
                star.innerHTML = '&#9733;';
            } else {
                star.innerHTML = '&#9734;';
            }
        });
    }

    const initialRating = parseInt(ratingInput.value) || 0;
    updateStars(initialRating);
    updateSubmitButton();

});