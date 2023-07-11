import { updateEnemyHpBar, updatePlayerHpBar } from "../combat/combat-hp-bars.js";
import { shake } from "../animations/shake.js";

// Buttons.
const attackBtn = document.getElementById("attack-btn");

// Visual elements.
const winScreenEl = document.getElementById("win-screen");
const enemyShapeEl = document.getElementById("enemy-shape");
const enemyActionEl = document.getElementById("enemy-action");
const totalHpEnemyEl = document.getElementById("enemy-total-hp");
const totalHpPlayerEl = document.getElementById("player-total-hp");
const currentHpEnemyEl = document.getElementById("enemy-current-hp");
const currentHpPlayerEl = document.getElementById("player-current-hp");
const enemyHealthBarContainerEl = document.getElementById("enemy-health-bar-container");
const playerHealthBarContainerEl = document.getElementById("player-health-bar-container");

// Variables used in script.
const player = { strength: 0, vigor: 0, agility: 0 };
const enemy = { strength: 0, vigor: 0, agility: 0 };

attackBtn.addEventListener("click", attackEnemy);
onCombatPageLoad();

function populateStats() {
    fetch("/Response/Player/Stats")
        .then(response => response.json())
        .then(stats => {
            player.strength = stats.strength;
            player.vigor = stats.vigor;
            player.agility = stats.agility;
            return fetch("/Response/Enemy/Stats");
        })
        .then(response => response.json())
        .then(stats => {
            enemy.strength = stats.strength;
            enemy.vigor = stats.vigor;
            enemy.agility = stats.agility;
            if (player.agility < stats.agility)
                enemyAttacksFirst();
        });
}

function attackEnemy() {
    fetch("/Response/Enemy/Defend", {
        method: "PATCH",
        body: JSON.stringify({ hpToReduce: Number(player.strength) }),
        headers: { "Content-type": "application/json;" }
    })
        .then(response => response.json())
        .then(x => {
            shake(enemyHealthBarContainerEl);
            updateEnemyActionBar(x.isPlayerAttacking);
            currentHpEnemyEl.innerText = x.updatedCharacterHp;
            updateEnemyHpBar(x.updatedCharacterHp, totalHpEnemyEl.innerText);
            if (x.updatedCharacterHp <= 0) playerWinCombat();
            else attackPlayer();
        });
}

function attackPlayer() {
    fetch("/Response/Enemy/Attack", {
        method: "PATCH",
        body: JSON.stringify({ hpToReduce: Number(enemy.strength) }),
        headers: { "Content-type": "application/json" }
    })
        .then(response => response.json())
        .then(x => {
            setTimeout(function () {
                updateEnemyActionBar(x.isPlayerAttacking);
                shake(playerHealthBarContainerEl);
                currentHpPlayerEl.innerText = x.updatedCharacterHp;
                updatePlayerHpBar(x.updatedCharacterHp, totalHpPlayerEl.innerText);
                if (x.updatedCharacterHp <= 0) console.log("Player lost.... bruuuh");
            }, 1000)
        })
}

function enemyAttacksFirst() {
    attackBtn.classList.add("disable-click");
    attackPlayer();
}

function updateEnemyActionBar(isPlayerAttacking) {
    if (isPlayerAttacking) {
        enemyActionEl.classList.remove("btn-outline-danger");
        enemyActionEl.classList.add("btn-outline-info");
        enemyActionEl.innerText = "Defending....";
        attackBtn.classList.remove("disable-click");
    } else {
        enemyActionEl.classList.remove("btn-outline-info");
        enemyActionEl.classList.add("btn-outline-danger");
        enemyActionEl.innerText = "Attacking....";
        attackBtn.classList.add("disable-click");
    }
}

function playerWinCombat() {
    fetch("/Response/Combat/Win")
        .then(response => response.json())
        .then(hasWon => {
            if (hasWon) {
                updateScreenOnPlayerWin();
            }
        });
}

function updateScreenOnPlayerWin() {
    attackBtn.classList.add("disable-click");
    winScreenEl.style.zIndex = "100";
    winScreenEl.style.opacity = "100";
    enemyShapeEl.style.transform = "translateY(1000%)";
    enemyActionEl.classList.remove("btn-outline-danger");
    enemyActionEl.classList.remove("btn-outline-info");
    enemyActionEl.classList.add("gold");
    enemyActionEl.innerText = "Homie deaded 😢";
}

function onCombatPageLoad() {
    // Assign character stats.
    populateStats();

    // Update hp bars.
    updateEnemyHpBar(currentHpEnemyEl.innerText, totalHpEnemyEl.innerText);
    updatePlayerHpBar(currentHpPlayerEl.innerText, totalHpPlayerEl.innerText);
}