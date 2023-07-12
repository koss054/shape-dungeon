// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const keyPressCombo = 2;
const btnArray = new Array(keyPressCombo);
const goToActiveRoomBtn = document.getElementById("active-room-nav-btn");

document.addEventListener("keypress", (e) => {
    updateArray(e.key);
    if (btnArray[0] === 'a' && btnArray[1] === 'q') {
        goToActiveRoomBtn.click();
    }
});

function updateArray(keyPressed) {
    if (btnArray.length == 2) btnArray.shift()
    btnArray.push(keyPressed.toLowerCase());
}

setInterval(function () {
    btnArray.shift();
}, 200);