const strengthPlusEl = document.getElementById("strength-plus");
const vigorPlusEl = document.getElementById("vigor-plus");
const agilityPlusEl = document.getElementById("agility-plus");

function toggleSkillpointBtns(skillpointCount) {
    if (skillpointCount <= 0) {
        strengthPlusEl.classList.remove("btn-outline-success");
        vigorPlusEl.classList.remove("btn-outline-success");
        agilityPlusEl.classList.remove("btn-outline-success");
        strengthPlusEl.classList.add("disabled", "btn-outline-dark");
        vigorPlusEl.classList.add("disabled", "btn-outline-dark");
        agilityPlusEl.classList.add("disabled", "btn-outline-dark");
    } else {
        strengthPlusEl.classList.remove("disabled", "btn-outline-dark");
        vigorPlusEl.classList.remove("disabled", "btn-outline-dark");
        agilityPlusEl.classList.remove("disabled", "btn-outline-dark");
        strengthPlusEl.classList.add("btn-outline-success");
        vigorPlusEl.classList.add("btn-outline-success");
        agilityPlusEl.classList.add("btn-outline-success");
    }
}