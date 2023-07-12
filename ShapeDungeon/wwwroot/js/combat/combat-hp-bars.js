const enemyHpBarEl = document.getElementById("enemy-health-bar");
const playerHpBarEl = document.getElementById("player-health-bar");

// Possible TO DO: Combine functions - DRY
// Easier to use in other files this way as the element doesn't need to be specified.
export function updateEnemyHpBar(currentHp, totalHp) {
    const barPercentage = (Number(currentHp) * 100) / Number(totalHp);
    enemyHpBarEl.style.width = `${barPercentage}%`;
}

export function updatePlayerHpBar(currentHp, totalHp) {
    const barPercentage = (Number(currentHp) * 100) / Number(totalHp);
    playerHpBarEl.style.width = `${barPercentage}%`;
}