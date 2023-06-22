const placeholderColor = "C0L0R";

const type = {
    square: `height:10rem;width:10rem;background-color:${placeholderColor}`,
    triangle: `width:0;height:0;border-top: 5rem solid transparent;border-left: 10rem solid ${placeholderColor};border-bottom: 5rem solid transparent`,
    circle: `height:10rem;width:10rem;background-color:${placeholderColor};border-radius:50%`,
    missing: `height:10rem;width:10rem;background-color:magenta;color:black;text-align:center`
}

export function updateShape(shapeElement, shapeType, shapeColor) {
    const nodeName = shapeElement.nodeName;
    let shapeStyle = type[shapeType];

    if (shapeElement === undefined || nodeName === undefined) {
        window.alert("Check the code, g, incorrect shape element passed to function.");
        return;
    }

    if (shapeStyle === undefined) {
        shapeStyle = type["missing"];
        shapeElement.innerText = "Unrecognized shape type!";
    } else {
        shapeElement.innerText = "";
    }

    if (shapeColor === undefined)
        shapeColor = "magenta"

    shapeStyle = shapeStyle.replace(placeholderColor, shapeColor);
    shapeElement.setAttribute("style", shapeStyle);
}