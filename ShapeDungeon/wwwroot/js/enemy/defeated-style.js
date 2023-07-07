const enemyShapeEl = document.getElementById("enemy-shape");
const enemyLevelEl = document.getElementById("enemy-level");

enemyLevelEl.style.color = "rgb(139 0 0 / 68%)";

function defeatedEnemyTriangle() {
    enemyShapeEl.style.borderLeftColor = "#8b000040";
}

function defeatedEnemy() {
    enemyShapeEl.style.backgroundColor = "#8b000040";
}