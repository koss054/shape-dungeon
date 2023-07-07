import { updateEnemyHpBar, updatePlayerHpBar } from "../combat/combat-hp-bars.js";
import { shake } from "../animations/shake.js";

// Buttons.
const attackBtn = document.getElementById("attack-btn");

// Visual elements.
const winScreenEl = document.getElementById("win-screen");
const enemyShapeEl = document.getElementById("enemy-shape");
const totalHpEnemyEl = document.getElementById("enemy-total-hp");
const totalHpPlayerEl = document.getElementById("player-total-hp");
const currentHpEnemyEl = document.getElementById("enemy-current-hp");
const currentHpPlayerEl = document.getElementById("player-current-hp");
const enemyHealthBarContainerEl = document.getElementById("enemy-health-bar-container");

// Variables used in script.
const player = { strength: 0, vigor: 0, agility: 0 };
const enemy = { strength: 0, vigor: 0, agility: 0 };

attackBtn.addEventListener("click", attackEnemy);
onCombatPageLoad();

function populatePlayerStats() {
    fetch("/Response/Player/Stats")
        .then(response => response.json())
        .then(stats => {
            player.strength = stats.strength;
            player.vigor = stats.vigor;
            player.agility = stats.agility;
        });
}

function populateEnemyStats() {
    fetch("/Response/Enemy/Stats")
        .then(response => response.json())
        .then(stats => {
            enemy.strength = stats.strength;
            enemy.vigor = stats.vigor;
            enemy.agility = stats.agility;
        });
}

function attackEnemy() {
    fetch("/Response/Enemy/Reduce-Health", {
        method: "PATCH",
        body: JSON.stringify({ hpToReduce: Number(player.strength) }),
        headers: { "Content-type": "application/json;" }
    })
        .then(response => response.json())
        .then(updatedEnemyHp => {
            currentHpEnemyEl.innerText = updatedEnemyHp;
            updateEnemyHpBar(updatedEnemyHp, totalHpEnemyEl.innerText);
            shake(enemyHealthBarContainerEl);
            if (updatedEnemyHp <= 0) playerWinCombat();
        });
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
    attackBtn.disabled = true;
    winScreenEl.style.zIndex = "100";
    winScreenEl.style.opacity = "100";
    enemyShapeEl.style.transform = "translateY(1000%)";
}

function onCombatPageLoad() {
    // Assign character stats.
    populatePlayerStats();
    populateEnemyStats();

    // Update hp bars.
    updateEnemyHpBar(currentHpEnemyEl.innerText, totalHpEnemyEl.innerText);
    updatePlayerHpBar(currentHpPlayerEl.innerText, totalHpPlayerEl.innerText);
}