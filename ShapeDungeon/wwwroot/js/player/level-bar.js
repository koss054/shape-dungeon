const levelBarEl = document.getElementById("level-bar");

function updateBar(currentXp, xpRequired) {
    const barPercentage = (currentXp * 100) / xpRequired;
    levelBarEl.style.width = `${barPercentage}%`;
}