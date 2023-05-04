const radioBtns = document.getElementsByName("shape");
const shape = document.getElementById("selected-shape");
const strengthEl = document.getElementById("strength");
const vigorEl = document.getElementById("vigor");
const agilityEl = document.getElementById("agility");

for (const radioBtn of radioBtns) {
    radioBtn.addEventListener("click", () => {
        switch (radioBtn.id) {
            case "square":
                shape.setAttribute("style", "height:10rem;width:10rem;background-color:limegreen");
                setPlayerAttributes("square");
                break;
            case "triangle":
                shape.setAttribute("style", "width:0;height:0;border-top: 5rem solid transparent;border-left: 10rem solid limegreen;border-bottom: 5rem solid transparent");
                setPlayerAttributes("triangle");
                break;
            case "circle":
                shape.setAttribute("style", "height:10rem;width:10rem;background-color:limegreen;border-radius:50%");
                setPlayerAttributes("circle");
                break;
            default:
                console.log("bruh, there's an error...");
                break;
        }
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
    vigorEl.value = newVigor
    agilityEl.value = newAgility;
}