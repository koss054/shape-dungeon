const partialViewEl = document.getElementById("room-with-nav-partial");
const moveUpBtn = document.getElementById("move-up-btn");


if (moveUpBtn != undefined) {
    moveUpBtn.addEventListener("click", () => {
        fetch("/Response/Room/Move/Up")
            .then(() => {
                return fetch("/Response/Room/Partial/Active");
            })
            .then(data => {
                data.text().then(html => {
                    $('#room-with-nav-partial').html(html);
                })
                return fetch("/Response/Room/Partial/Style");
            })
            .then(data => {
                data.text().then(styledHtml => {
                    $('#room-style-partial').html(styledHtml);
                })
            })
    });
};