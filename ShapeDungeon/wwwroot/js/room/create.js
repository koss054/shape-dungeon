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

const roomEl = document.getElementById("room");
const roomCreateBtn = document.getElementById("create-room-btn");

const enemyForm = document.getElementById("enemy-form");
const enemyLevel = document.getElementById("enemy-level");
const enemySelect = document.getElementById("enemy-select");
const enemyShape = document.getElementById("selected-enemy-shape");
const enemyLevelNum = document.getElementById("enemy-level-number");

const roomDirectionBtnArr = [leftRadioBtn, rightRadioBtn, upRadioBtn, downRadioBtn];
const roomTypeBtnArr = [startRadioBtn, safeRadioBtn, enemyRadioBtn, endRadioBtn];

// Doing this becuase bootstrap changes the checked property to true.
// When the script loads this.checked is already true and the button never gets the right checked value.
let isLeftChecked = false;
let isRightChecked = false;
let isUpChecked = false;
let isDownChecked = false;

leftRadioBtn.addEventListener("click", toggleRadioBtn);
rightRadioBtn.addEventListener("click", toggleRadioBtn);
upRadioBtn.addEventListener("click", toggleRadioBtn);
downRadioBtn.addEventListener("click", toggleRadioBtn);

startBtnLabel.addEventListener("click", toggleRoomOptionBackground);
safeBtnLabel.addEventListener("click", toggleRoomOptionBackground);
enemyBtnLabel.addEventListener("click", toggleRoomOptionBackground);
endBtnLabel.addEventListener("click", toggleRoomOptionBackground);

updateRoomOnLoad();

function toggleRadioBtn() {
    let direction = this.id;
    direction = direction.replace("-dir", "");

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
        case "start-label": roomEl.style.background = "#2a9fd669"; hideEnemyForm(); hideEnemyShape(); break;
        case "safe-label": roomEl.style.background = "#77b30069"; hideEnemyForm(); hideEnemyShape(); break;
        case "enemy-label": roomEl.style.background = "#cc000069"; displayEnemyForm(); enemyRoomSelected(); break;
        case "end-label": roomEl.style.background = "#9933cc69"; hideEnemyForm(); hideEnemyShape(); break;
        default: console.log("bruh, room option error..."); break;
    }

    for (const radioBtn of roomTypeBtnArr) {
        if (radioBtn.id != currRoomOption) radioBtn.checked = false;
    }

    defaultRoomTypeChecked();
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

function updateRoomOnLoad() {
    let leftBorder = "border-left:solid 1rem white";
    let rightBorder = "border-right:solid 1rem white";
    let topBorder = "border-top:solid 1rem white";
    let bottomBorder = "border-bottom:solid 1rem white";

    if (leftRadioBtn.checked) {
        isLeftChecked = true;
        leftBorder = leftBorder.replace("white", "dimgray");
    }

    if (rightRadioBtn.checked) {
        isRightChecked = true;
        rightBorder = rightBorder.replace("white", "dimgray");
    }

    if (upRadioBtn.checked) {
        isUpChecked = true;
        topBorder = topBorder.replace("white", "dimgray");
    }

    if (downRadioBtn.checked) {
        isDownChecked = true;
        bottomBorder = bottomBorder.replace("white", "dimgray");
    }

    room.setAttribute("style", `height:50vh;width:50vh;${leftBorder};${rightBorder};${topBorder};${bottomBorder}`);

    if (startRadioBtn.checked) roomEl.style.background = "#2a9fd669";
    else if (safeRadioBtn.checked) roomEl.style.background = "#77b30069";
    else if (endRadioBtn.checked) roomEl.style.background = "#9933cc69";
    else if (enemyRadioBtn.checked) roomEl.style.background = "#cc000069";
}

function displayEnemyForm() {
    enemyForm.style.display = "block";
}

function hideEnemyForm() {
    enemyForm.style.display = "none";
}

function disableCreationBtns() {
    for (const btn of roomDirectionBtnArr) btn.setAttribute("disabled", "disabled");
    for (const btn of roomTypeBtnArr) btn.setAttribute("disabled", "disabled");
    roomCreateBtn.setAttribute("disabled", "disabled");
}

function disableLeftRadioBtn() {
    isLeftChecked = true;
    leftRadioBtn.checked = true;
    leftRadioBtn.removeEventListener("click", toggleRadioBtn);
}

function disableRightRadioBtn() {
    isRightChecked = true;
    rightRadioBtn.checked = true;
    rightRadioBtn.removeEventListener("click", toggleRadioBtn);
}

function disableUpRadioBtn() {
    isUpChecked = true;
    upRadioBtn.checked = true;
    upRadioBtn.removeEventListener("click", toggleRadioBtn);
}

function disableBottomRadioBtn() {
    isDownChecked = true;
    downRadioBtn.checked = true;
    downRadioBtn.removeEventListener("click", toggleRadioBtn);
}

function defaultRoomTypeChecked() {
    roomCreateBtn.setAttribute("disabled", "disabled");
    for (const btn of roomTypeBtnArr) {
        btn.addEventListener("click", function () { roomCreateBtn.removeAttribute("disabled") });
    }
}

function disableRadioBtnOnDirectionalCreate() {
    if (leftRadioBtn.checked) disableLeftRadioBtn();
    else if (rightRadioBtn.checked) disableRightRadioBtn();
    else if (upRadioBtn.checked) disableUpRadioBtn();
    else if (downRadioBtn.checked) disableBottomRadioBtn();
}

function enemyRoomSelected() {
    enemyShape.style.display = "block";
    enemyLevel.style.display = "block";
    enemySelect.addEventListener("change", updateShape);

    let currentShape, shapeLetter, level;
    updateShape();

    function updateShape() {
        currentShape = enemySelect.options[enemySelect.selectedIndex].text;
        shapeLetter = currentShape.charAt(currentShape.length - 1);
        level = currentShape.split("Lvl.")[1].split(" ")[0];

        enemyLevelNum.innerText = level;

        // TODO: Reduce code in this file, in order to import updateShape function and use it here
        // Unable to use type="module", since the Create.cshtml file stops recognizing functions when used in <script></script>.
        switch (shapeLetter) {
            case "S":
                enemyShape.setAttribute("style", "height:10rem;width:10rem;background-color:red");
                break;
            case "T":
                enemyShape.setAttribute("style", "width:0;height:0;border-top: 5rem solid transparent;border-left: 10rem solid red;border-bottom: 5rem solid transparent");
                break;
            case "C":
                enemyShape.setAttribute("style", "height:10rem;width:10rem;background-color:red;border-radius:50%");
                break;
            default: console.log("bruuuh, enemy shape doesn't exist... how?? lol");
        }
    }
}

// Functions used in Create.cshtml that become unrecognizable.
function enableEnemyShapeDisplay() {
    enemyShape.style.display = "block";
    enemyLevel.style.display = "block";
}

function displayEnemySquare(level) {
    enemyShape.setAttribute("style", "height:10rem;width:10rem;background-color:red");
    enemyLevelNum.innerText = level;
}

function displayEnemyTriangle(level) {
    enemyShape.setAttribute("style", "width:0;height:0;border-top: 5rem solid transparent;border-left: 10rem solid red;border-bottom: 5rem solid transparent");
    enemyLevelNum.innerText = level;
}

function displayEnemyCricle(level) {
    enemyShape.setAttribute("style", "height:10rem;width:10rem;background-color:red;border-radius:50%");
    enemyLevelNum.innerText = level;
}

function hideEnemyShape() {
    enemyShape.style.display = "none";
    enemyLevel.style.display = "none";
}