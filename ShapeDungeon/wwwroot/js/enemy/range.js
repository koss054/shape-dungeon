﻿const minLevelEl = document.getElementById("min-level");
const maxLevelEl = document.getElementById("max-level");

minLevelEl.addEventListener("change", updateCookies);
maxLevelEl.addEventListener("change", updateCookies);

const minCookieExists = document.cookie.includes("min");
const maxCookieExists = document.cookie.includes("max")

if (!minCookieExists)
    document.cookie = "min=0";

if (!maxCookieExists)
    document.cookie = "max=100";

minLevelEl.value = getCookie("min");
maxLevelEl.value = getCookie("max");

function updateCookies() {
    document.cookie = `min=${minLevelEl.value}`;
    document.cookie = `max=${maxLevelEl.value}`;
}

function getCookie(cookieName) {
    let name = cookieName + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let cookies = decodedCookie.split(';');

    for (let i = 0; i < cookies.length; i++) {
        let c = cookies[i];

        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }

        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }

    return "";
}