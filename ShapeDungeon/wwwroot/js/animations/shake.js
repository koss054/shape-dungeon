export function shake(element, interval = 100) {
    element.classList.add("shaking");
    element.style.transition = "all " + (interval / 100).toString() + "s";
    setTimeout(() => {
        element.style.transform = "translateX(-25%)";
    }, interval * 0);
    setTimeout(() => {
        element.style.transform = "translateX(25%)";
    }, interval * 1);
    setTimeout(() => {
        element.style.transform = "translateX(-10%)";
    }, interval * 2);
    setTimeout(() => {
        element.style.transform = "translateX(10%)";
    }, interval * 3);
    setTimeout(() => {
        element.style.transform = "translateX(-5%)";
    }, interval * 4);
    setTimeout(() => {
        element.style.transform = "translateX(0%)";
    }, interval * 5);
    element.classList.remove("shaking");
}