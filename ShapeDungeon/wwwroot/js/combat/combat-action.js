import { updateEnemyHpBar, updatePlayerHpBar } from "../combat/combat-hp-bars.js";

const attackBtn = document.getElementById("attack-btn");
const playerStrengthEl = document.getElementById("player-strength");

const currentHpEnemyEl = document.getElementById("enemy-current-hp");
const totalHpEnemyEl = document.getElementById("enemy-total-hp");
const currentHpPlayerEl = document.getElementById("player-current-hp");
const totalHpPlayerEl = document.getElementById("player-total-hp");

attackBtn.addEventListener("click", attackEnemy);

onCombatPageLoad();

const player = {
    strength: 0,
    vigor: 0,
    agility: 0,
};

function populatePlayerStats() {
    fetch("/Response/Player/Stats")
        .then(response => response.json())
        .then(stats => {
            player.strength = stats.strength;
            player.vigor = stats.vigor;
            player.agility = stats.agility;
        });
}

function attackEnemy() {

    fetch("/Combat/Test", {
        method: "PATCH",
        body: JSON.stringify({ hp: Number(currentHpEnemyEl.innerText) - player.strength }),
        headers: { "Content-type": "application/json;" }
    })
        .then(response => response.json())
        .then(() => updateEnemyHp())

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

    // Update hp bars.
    updateEnemyHpBar(currentHpEnemyEl.innerText, totalHpEnemyEl.innerText);
    updatePlayerHpBar(currentHpPlayerEl.innerText, totalHpPlayerEl.innerText);
}