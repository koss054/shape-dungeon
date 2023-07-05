const attackBtn = document.getElementById("attack-btn");
const enemyHpEl = document.getElementById("enemy-hp");
const playerStrengthEl = document.getElementById("player-strength");

attackBtn.addEventListener("click", attackEnemy);

function attackEnemy() {
    fetch("/Combat/Test", {
        method: "PATCH",
        body: JSON.stringify({ hp: 54 }),
        headers: { "Content-type": "application/json;" }
    })
        .then(response => response.json())
        .then(() => updateEnemyHp())

}

function updateEnemyHp() {
    fetch("/Combat/Test2")
        .then(response => response.json())
        .then(enemyInfo => enemyHpEl.innerText = enemyInfo.hp);
}