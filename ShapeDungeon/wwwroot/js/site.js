// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const goToActiveRoomBtn = document.getElementById("active-room-nav-btn");
document.addEventListener("keypress", (e) => {
    console.log(e.key.toLowerCase())
    if (e.key.toLowerCase() === "a") {
        goToActiveRoomBtn.click();
    }
});