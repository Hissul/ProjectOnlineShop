

//const phoneInput = document.getElementById("Phone");
//const addressInput = document.getElementById("Address");
//const checkoutButton = document.getElementById("checkoutButton");

//document.addEventListener("DOMContentLoaded", function () {
    

//    // Проверяем, что все элементы существуют перед добавлением событий
//    if (phoneInput && addressInput && checkoutButton) {
//        // Функция для проверки состояния полей
//        function toggleCheckoutButton() {
//            // Активируем кнопку, если оба поля заполнены
//            const phoneIsValid = phoneInput.value.trim() !== "";
//            const addressIsValid = addressInput.value.trim() !== "";

//            if (phoneIsValid && addressIsValid) {
//                checkoutButton.removeAttribute("disabled");
//            } else {
//                checkoutButton.setAttribute("disabled", "true");
//            }
//        }

//        // Добавляем обработчики событий для полей
//        phoneInput.addEventListener("input", toggleCheckoutButton);
//        addressInput.addEventListener("input", toggleCheckoutButton);

//        // Проверяем состояние кнопки при первоначальной загрузке
//        toggleCheckoutButton();
//    }
//});
