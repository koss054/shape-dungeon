const leftRadioBtn = document.getElementById("left-dir");
const rightRadioBtn = document.getElementById("right-dir");
const upRadioBtn = document.getElementById("up-dir");
const downRadioBtn = document.getElementById("down-dir");

const startBtnLabel = document.getElementById("start-label");
const safeBtnLabel = document.getElementById("safe-label");
const enemyBtnLabel = document.getElementById("enemy-label");
const endBtnLabel = document.getElementById("end-label");
const startRadioBtn = document.getElementById("start-room-option");
const safeRadioBtn = document.getElementById("safe-room-option");
const enemyRadioBtn = document.getElementById("enemy-room-option");
const endRadioBtn = document.getElementById("end-room-option");

const radioBtnArr = [startRadioBtn, safeRadioBtn, enemyRadioBtn, endRadioBtn];

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

startBtnLabel.addEventListener("click", toggleRoomOptionBackground);
safeBtnLabel.addEventListener("click", toggleRoomOptionBackground);
enemyBtnLabel.addEventListener("click", toggleRoomOptionBackground);
endBtnLabel.addEventListener("click", toggleRoomOptionBackground);

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

function toggleRoomOptionBackground() {
    const currRoomOption = this.id;
    switch (currRoomOption) {
        case "start-label": roomEl.style.background = "#2a9fd669"; break;
        case "safe-label": roomEl.style.background = "#77b30069"; break;
        case "enemy-label": roomEl.style.background = "#cc000069"; break;
        case "end-label": roomEl.style.background = "#9933cc69"; break;
        default: console.log("bruh, room option error..."); break;
    }

    for (const radioBtn of radioBtnArr) {
        if (radioBtn.id != currRoomOption) radioBtn.checked = false;
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

    if (startRadioBtn.checked) roomEl.style.background = "#2a9fd669";
    else if (safeRadioBtn.checked) roomEl.style.background = "#77b30069";
    else if (enemyRadioBtn.checked) roomEl.style.background = "#cc000069";
    else if (endRadioBtn.checked) roomEl.style.background = "#9933cc69";
}