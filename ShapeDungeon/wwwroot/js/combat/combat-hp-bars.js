const enemyHpBarEl = document.getElementById("enemy-health-bar");

export function updateEnemyHpBar(currentHp, totalHp) {
    const barPercentage = (Number(currentHp) * 100) / Number(totalHp);
    enemyHpBarEl.style.width = `${barPercentage}%`;
}