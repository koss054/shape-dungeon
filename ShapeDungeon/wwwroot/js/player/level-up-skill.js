import { getCookie } from "../shared/cookie-get.js";

const fastSwitchEl = document.getElementById("fast-level-up-switch");
const confirmationWindowEl = document.getElementById("stat-confirmation");
const statNameEl = document.getElementById("stat-name");
const strengthConfirmEl = document.getElementById("strength-yes-btn");
const vigorConfirmEl = document.getElementById("vigor-yes-btn");
const agilityConfirmEl = document.getElementById("agility-yes-btn");
const noBtnEl = document.getElementById("no-btn");

const yesBtnCollection = [strengthConfirmEl, vigorConfirmEl, agilityConfirmEl];

fastSwitchEl.addEventListener("click", toggleSwitch);

strengthPlusEl.addEventListener("click", showConfirmationWindow);
vigorPlusEl.addEventListener("click", showConfirmationWindow);
agilityPlusEl.addEventListener("click", showConfirmationWindow);

noBtnEl.addEventListener("click", closeConfirmationWindow);

const switchCookieExists = document.cookie.includes("f-switch");
if (!switchCookieExists)
    document.cookie = "f-switch=no";

const switchCookieOnLoad = getCookie("f-switch");
switch (switchCookieOnLoad) {
    case "yes": fastSwitchEl.checked = true; break;
    case "no": fastSwitchEl.checked = false; break;
    default: console.log("bruh, cookie for fast switch is buggy");
}

function toggleSwitch() {
    const switchCookieBeforeToggle = getCookie("f-switch");
    switch (switchCookieBeforeToggle) {
        case "yes": document.cookie = "f-switch=no"; break;
        case "no": document.cookie = "f-switch=yes"; break;
    }
}

function showConfirmationWindow() {
    const fSwitchCookie = getCookie("f-switch");
    let statName = this.id.replace("-plus", "");
    statName = statName.charAt(0).toUpperCase() + statName.slice(1);
    statNameEl.innerText = statName;

    if (fSwitchCookie === "yes") {
        fastStatUpgrade(statName);
    } else if (fSwitchCookie === "no") {
        confirmationWindowEl.classList.remove("hidden");
        toggleStatConfirmYesButton(statName);
    }
}

function fastStatUpgrade(statToUpgrade) {
    switch (statToUpgrade) {
        case "Strength": strengthConfirmEl.click(); break;
        case "Vigor": vigorConfirmEl.click(); break;
        case "Agility": agilityConfirmEl.click(); break;
    }
}

function closeConfirmationWindow() {
    confirmationWindowEl.classList.add("hidden");
    toggleStatConfirmYesButton();
}

function toggleStatConfirmYesButton(btnToDisplay) {
    for (const btn of yesBtnCollection)
        btn.classList.add("hidden");

    switch (btnToDisplay) {
        case "Strength": strengthConfirmEl.classList.remove("hidden"); break;
        case "Vigor": vigorConfirmEl.classList.remove("hidden"); break;
        case "Agility": agilityConfirmEl.classList.remove("hidden"); break;
    }
}