const mainNavbarEl = document.getElementById("main-navbar");

function createNavOverlay() {
    const overlayEl = document.createElement("div");

    overlayEl.style.position = "absolute";
    overlayEl.style.width = "100%";
    overlayEl.style.height = "100%";
    overlayEl.style.background = "#000000";
    overlayEl.style.opacity = "50%";

    return overlayEl;
}

export function disableNavbar() {
    const childEl = createNavOverlay();
    mainNavbarEl.appendChild(childEl);
}