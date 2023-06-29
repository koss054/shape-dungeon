import { updateShape } from "../shared/apply-shape.js";
import { enemyColor } from "../shared/colors.js";

const radioBtns = document.getElementsByName("shape");
const shape = document.getElementById("selected-shape");
const strengthEl = document.getElementById("strength");
const vigorEl = document.getElementById("vigor");
const agilityEl = document.getElementById("agility");
const levelEl = document.getElementById("level");

strengthEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");
vigorEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");
agilityEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");

strengthEl.addEventListener("change", updateEnemyLevel);
vigorEl.addEventListener("change", updateEnemyLevel);
agilityEl.addEventListener("change", updateEnemyLevel);

for (const radioBtn of radioBtns) {
    radioBtn.addEventListener("click", () => {
        updateShape(shape, radioBtn.id, enemyColor);
        setEnemyAttributes(radioBtn.id);
        updateEnemyLevel();
    })
}

function setEnemyAttributes(shape) {
    let newStrength, newVigor, newAgility;

    switch (shape) {
        case "square":
            newStrength = 2;
            newVigor = 5;
            newAgility = 1;
            break;
        case "triangle":
            newStrength = 5;
            newVigor = 3;
            newAgility = 2;
            break;
        case "circle":
            newStrength = 2;
            newVigor = 2;
            newAgility = 5;
            break;
        default:
            console.log("bruh, there's an error...");
            break;
    }

    strengthEl.value = newStrength;
    vigorEl.value = newVigor;
    agilityEl.value = newAgility;
}

function updateEnemyLevel() {
    const currStrength = Number(strengthEl.value);
    const currVigor = Number(vigorEl.value);
    const currAgility = Number(agilityEl.value);

    levelEl.value = currStrength + currVigor + currAgility;
}