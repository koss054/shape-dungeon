const radioBtns = document.getElementsByName("shape");
const shape = document.getElementById("selected-shape");
const strengthEl = document.getElementById("strength");
const vigorEl = document.getElementById("vigor");
const agilityEl = document.getElementById("agility");

strengthEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");
vigorEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");
agilityEl.setAttribute("style", "background:lightgray;color:#cc0000;text-align:center;border-radius:0.5rem");

for (const radioBtn of radioBtns) {
    radioBtn.addEventListener("click", () => {
        switch (radioBtn.id) {
            case "square":
                shape.setAttribute("style", "height:10rem;width:10rem;background-color:#cc0000");
                setEnemyAttributes("square");
                break;
            case "triangle":
                shape.setAttribute("style", "width:0;height:0;border-top: 5rem solid transparent;border-left: 10rem solid #cc0000;border-bottom: 5rem solid transparent");
                setEnemyAttributes("triangle");
                break;
            case "circle":
                shape.setAttribute("style", "height:10rem;width:10rem;background-color:#cc0000;border-radius:50%");
                setEnemyAttributes("circle");
                break;
            default:
                console.log("bruh, there's an error...");
                break;
        }
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