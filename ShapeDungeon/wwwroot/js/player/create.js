import { updateShape } from "../shared/apply-shape.js";
import { playerColor } from "../shared/colors.js";

const radioBtns = document.getElementsByName("shape");
const shape = document.getElementById("selected-shape");
const strengthEl = document.getElementById("strength");
const vigorEl = document.getElementById("vigor");
const agilityEl = document.getElementById("agility");

for (const radioBtn of radioBtns) {
    radioBtn.addEventListener("click", () => {
        updateShape(shape, radioBtn.id, playerColor);
        setPlayerAttributes(radioBtn.id);
    })
}

function setPlayerAttributes(shape) {
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