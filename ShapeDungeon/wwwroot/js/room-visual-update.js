// TODO (MAYBE LOL): Reduce the repeated code.
// If it's not done like this, an error about already declared variable is thrown.
if ((typeof roomEl).toString() == "undefined") {
    const roomEl = document.getElementById("room");

    let leftBorder = "border-left:solid 1rem white";
    let rightBorder = "border-right:solid 1rem white";
    let topBorder = "border-top:solid 1rem white";
    let bottomBorder = "border-bottom:solid 1rem white";

    let roomStyle;

    function removeLeftBorder() {
        leftBorder = leftBorder.replace("white", "dimgray");
    }

    function removeRightBorder() {
        rightBorder = rightBorder.replace("white", "dimgray");
    }

    function removeTopBorder() {
        topBorder = topBorder.replace("white", "dimgray");
    }

    function removeBottomBorder() {
        bottomBorder = bottomBorder.replace("white", "dimgray");
    }

    function applyStartBackground() {
        roomEl.style.background = "#2a9fd669";
    }

    function applySafeBackground() {
        roomEl.style.background = "#77b30069";
    }

    function applyEnemyBackground() {
        roomEl.style.background = "#cc000069";
    }

    function applyEndBackground() {
        roomEl.style.background = "#9933cc69";
    }

    function updateRoomStyle() {
        roomStyle = `height:50vh;width:50vh;${leftBorder};${rightBorder};${topBorder};${bottomBorder}`;
    }

    function applyRoomStyle() {
        updateRoomStyle();
        room.setAttribute("style", roomStyle);
    }
} else {
    function removeLeftBorder() {
        leftBorder = leftBorder.replace("white", "dimgray");
    }

    function removeRightBorder() {
        rightBorder = rightBorder.replace("white", "dimgray");
    }

    function removeTopBorder() {
        topBorder = topBorder.replace("white", "dimgray");
    }

    function removeBottomBorder() {
        bottomBorder = bottomBorder.replace("white", "dimgray");
    }

    function applyStartBackground() {
        roomEl.style.background = "#2a9fd669";
    }

    function applySafeBackground() {
        roomEl.style.background = "#77b30069";
    }

    function applyEnemyBackground() {
        roomEl.style.background = "#cc000069";
    }

    function applyEndBackground() {
        roomEl.style.background = "#9933cc69";
    }

    function updateRoomStyle() {
        roomStyle = `height:50vh;width:50vh;${leftBorder};${rightBorder};${topBorder};${bottomBorder}`;
    }

    function applyRoomStyle() {
        updateRoomStyle();
        room.setAttribute("style", roomStyle);
    }
}
