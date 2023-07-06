import { updateEnemyHpBar, updatePlayerHpBar } from "../combat/combat-hp-bars.js";
import { shake } from "../animations/shake.js";

// Buttons.
const attackBtn = document.getElementById("attack-btn");

// Visual elements.
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
    fetch("/Combat/Test", {
        method: "PATCH",
        body: JSON.stringify({ hp: Number(currentHpEnemyEl.innerText) - player.strength }),
        headers: { "Content-type": "application/json;" }
    })
        .then(response => response.json())
        .then(() => {
            updateEnemyHp()
            shake(enemyHealthBarContainerEl);
        });
}

function updateEnemyHp() {
    fetch("/Combat/Test2")
        .then(response => response.json())
        .then(enemyInfo => {
            currentHpEnemyEl.innerText = enemyInfo.hp;
            updateEnemyHpBar(enemyInfo.hp, totalHpEnemyEl.innerText);
        });
}

function onCombatPageLoad() {
    // Assign character stats.
    populatePlayerStats();
    populateEnemyStats();

    // Update hp bars.
    updateEnemyHpBar(currentHpEnemyEl.innerText, totalHpEnemyEl.innerText);
    updatePlayerHpBar(currentHpPlayerEl.innerText, totalHpPlayerEl.innerText);
}