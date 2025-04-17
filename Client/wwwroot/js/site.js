

window.addEventListener('DOMContentLoaded', () => {
    const notification = document.getElementById('notification');
    console.log(notification); // Проверьте, существует ли элемент
    if (notification) {
        setTimeout(() => {
            notification.style.transition = 'opacity 0.5s ease, visibility 0s 0.5s';
            notification.style.opacity = '0';
            notification.style.visibility = 'hidden';
        }, 2000); // Уведомление исчезает через 2 секунды
    }
});
