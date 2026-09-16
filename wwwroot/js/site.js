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
    document.getElementById('charCount' ).innerText = el.value.length;
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

document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("movieSearch");
    const carousel = document.getElementById("movieCarousel");
    const suggestions = document.getElementById("movieSuggestions");
    const noMoviesMessage = document.getElementById("noMoviesMessage");

    if (!searchInput || !carousel || !suggestions) {
        return;
    }

    const carouselInner = carousel.querySelector(".carousel-inner");
    const originalMovieCards = Array.from(
        carousel.querySelectorAll(".movie-container")
    );

    const titles = originalMovieCards
        .map(function (movieCard) {
            return movieCard.dataset.title;
        })
        .filter(function (title, index, allTitles) {
            return title && allTitles.indexOf(title) === index;
        })
        .sort(function (firstTitle, secondTitle) {
            return firstTitle.localeCompare(secondTitle, "pl-PL");
        });

    function renderMovies(movieCards) {
        carouselInner.innerHTML = "";

        const hasMovies = movieCards.length > 0;

        carousel.style.display = hasMovies ? "" : "none";

        const carouselButtons = carousel.querySelectorAll(".carouselBtn");

        carouselButtons.forEach(function (button) {
            button.hidden = !hasMovies;
        });

        if (noMoviesMessage) {
            noMoviesMessage.style.display = hasMovies ? "none" : "block";
        }

        const itemsPerSlide = 3;

        for (let index = 0; index < movieCards.length; index += itemsPerSlide) {
            const carouselItem = document.createElement("div");
            carouselItem.className = "carousel-item movie-filter-slide";

            if (index === 0) {
                carouselItem.classList.add("active");
            }

            const row = document.createElement("div");
            row.className = "row row-carousel justify-content-center";

            movieCards
                .slice(index, index + itemsPerSlide)
                .forEach(function (movieCard, cardIndex) {
                    movieCard.classList.remove("movie-filter-card");
                    movieCard.style.animationDelay = `${cardIndex * 0.08}s`;

                    row.appendChild(movieCard);

                    void movieCard.offsetWidth;

                    movieCard.classList.add("movie-filter-card");
                });

            carouselItem.appendChild(row);
            carouselInner.appendChild(carouselItem);
        }
    }

    function filterMovies(value) {
        const query = value.trim().toLocaleLowerCase("pl-PL");

        if (!query) {
            renderMovies(originalMovieCards);
            return;
        }

        const matchingCards = originalMovieCards.filter(function (movieCard) {
            const title = movieCard.dataset.title.toLocaleLowerCase("pl-PL");

            return title.includes(query);
        });

        renderMovies(matchingCards);
    }

    function showSuggestions(value) {
        const query = value.trim().toLocaleLowerCase("pl-PL");

        suggestions.innerHTML = "";

        if (!query) {
            suggestions.classList.remove("visible");
            return;
        }

        const matchingTitles = titles.filter(function (title) {
            return title.toLocaleLowerCase("pl-PL").includes(query);
        });

        matchingTitles.forEach(function (title) {
            const suggestion = document.createElement("button");

            suggestion.type = "button";
            suggestion.className = "movie-suggestion";
            suggestion.textContent = title;

            suggestion.addEventListener("click", function () {
                searchInput.value = title;
                filterMovies(title);
                suggestions.classList.remove("visible");
            });

            suggestions.appendChild(suggestion);
        });

        suggestions.classList.toggle(
            "visible",
            matchingTitles.length > 0
        );
    }

    searchInput.addEventListener("input", function () {
        filterMovies(searchInput.value);
        showSuggestions(searchInput.value);
    });

    document.addEventListener("click", function (event) {
        if (!event.target.closest(".search-wrapper")) {
            suggestions.classList.remove("visible");
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const toggle = document.getElementById("aiChatToggle");
    const close = document.getElementById("aiChatClose");
    const windowElement = document.getElementById("aiChatWindow");
    const form = document.getElementById("aiChatForm");
    const input = document.getElementById("aiChatInput");
    const messages = document.getElementById("aiChatMessages");

    if (!toggle || !close || !windowElement || !form || !input || !messages) {
        return;
    }

    function addMessage(text, type) {
        const message = document.createElement("div");

        message.className = `ai-chat-message ai-chat-message-${type}`;
        message.textContent = text;

        messages.appendChild(message);
        messages.scrollTop = messages.scrollHeight;
    }

    toggle.addEventListener("click", function () {
        windowElement.hidden = !windowElement.hidden;

        if (!windowElement.hidden) {
            input.focus();
        }
    });

    close.addEventListener("click", function () {
        windowElement.hidden = true;
    });

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

        const message = input.value.trim();

        if (!message) {
            return;
        }

        addMessage(message, "user");
        input.value = "";

        try {
            const response = await fetch('@Url.Action("AskAsync", "Movies")', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    message: message
                })
            });

            if (!response.ok) {
                throw new Error("Server response error.");
            }

            const data = await response.json();
            addMessage(data.reply, "bot");
        } catch {
            addMessage(
                "Failed to connect to the AI assistant.",
                "bot"
            );
        }
    });
});