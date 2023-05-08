const leftRadioBtn = document.getElementById("left-dir");
const rightRadioBtn = document.getElementById("right-dir");
const upRadioBtn = document.getElementById("up-dir");
const downRadioBtn = document.getElementById("down-dir");
const roomEl = document.getElementById("room");

// Doing this becuase bootstrap changes the checked property to true.
// When the script loads this.checked is already true and the button never gets the right checked value.
let isLeftChecked = false;
let isRightChecked = false;
let isUpChecked = false;
let isDownChecked = false;

leftRadioBtn.addEventListener("click", function() { toggleRadioBtn("left") }, false);
rightRadioBtn.addEventListener("click", function() { toggleRadioBtn("right") }, false);
upRadioBtn.addEventListener("click", function () { toggleRadioBtn("up") }, false);
downRadioBtn.addEventListener("click", function () { toggleRadioBtn("down") }, false);

updateRoom();

function toggleRadioBtn(direction) {
    toggleCheckedBool(direction);
    toggleRadioHighlight(direction);
    updateRoom();
}

function toggleCheckedBool(direction) {
    switch (direction) {
        case "left": isLeftChecked = !isLeftChecked; break;
        case "right": isRightChecked = !isRightChecked; break;
        case "up": isUpChecked = !isUpChecked; break;
        case "down": isDownChecked = !isDownChecked; break;
        default: console.log("bruh, there's an error...");
    }
}

function toggleRadioHighlight(direction) {
    switch (direction) {
        case "left": leftRadioBtn.checked = isLeftChecked; break;
        case "right": rightRadioBtn.checked = isRightChecked; break;
        case "up": upRadioBtn.checked = isUpChecked; break;
        case "down": downRadioBtn.checked = isDownChecked; break;
        default: console.log("bruh, error...");
    }
}

function updateRoom() {
    let leftBorder = "border-left:solid 1rem white";
    let rightBorder = "border-right:solid 1rem white";
    let topBorder = "border-top:solid 1rem white";
    let bottomBorder = "border-bottom:solid 1rem white";

    if (isLeftChecked) leftBorder = leftBorder.replace("white", "dimgray");
    if (isRightChecked) rightBorder = rightBorder.replace("white", "dimgray");
    if (isUpChecked) topBorder = topBorder.replace("white", "dimgray");
    if (isDownChecked) bottomBorder = bottomBorder.replace("white", "dimgray");

    room.setAttribute("style", `height:50vh;width:50vh;${leftBorder};${rightBorder};${topBorder};${bottomBorder}`);
}