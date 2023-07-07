const enemyShapeEl = document.getElementById("enemy-shape");
const enemyLevelEl = document.getElementById("enemy-level");

enemyLevelEl.style.color = "darkred";

function defeatedEnemyTriangle() {
    enemyShapeEl.style.borderLeftColor = "darkred";
}

function defeatedEnemy() {
    enemyShapeEl.style.backgroundColor = "darkred";
}