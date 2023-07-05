import { updateEnemyHpBar } from "../combat/combat-hp-bars.js";

const attackBtn = document.getElementById("attack-btn");
const playerStrengthEl = document.getElementById("player-strength");

const currentHpEnemyEl = document.getElementById("enemy-current-hp");
const totalHpEnemyEl = document.getElementById("enemy-total-hp");

attackBtn.addEventListener("click", attackEnemy);

onCombatPageLoad();

function attackEnemy() {
    fetch("/Combat/Test", {
        method: "PATCH",
        body: JSON.stringify({ hp: 1 }),
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
    // Update hp bars.
    updateEnemyHpBar(currentHpEnemyEl.innerText, totalHpEnemyEl.innerText);
}