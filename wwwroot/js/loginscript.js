(function ($) {
    "use strict";

    // Function to toggle password visibility
    function togglePasswordVisibility() {
        let passFields = document.querySelectorAll('input[type="password"][data-eye]');
        passFields.forEach(field => {
            let wrapper = document.createElement('div');
            wrapper.classList.add('pass-wrapper');

            field.parentNode.insertBefore(wrapper, field);
            wrapper.appendChild(field);

            let toggleBtn = document.createElement('i');
            toggleBtn.classList.add('zmdi', 'zmdi-eye');
            wrapper.appendChild(toggleBtn);

            toggleBtn.addEventListener('click', function () {
                if (field.type === 'password') {
                    field.type = 'text';
                    toggleBtn.classList.remove('zmdi-eye');
                    toggleBtn.classList.add('zmdi-eye-off');
                } else {
                    field.type = 'password';
                    toggleBtn.classList.remove('zmdi-eye-off');
                    toggleBtn.classList.add('zmdi-eye');
                }
            });
        });
    }

    // Initialize the toggle password functionality
    document.addEventListener('DOMContentLoaded', function () {
        togglePasswordVisibility();
    });

})(jQuery);

// Form validation
function validateForm() {
    var pass = document.getElementById("pass").value;
    var confirmPass = document.getElementById("confirmpass").value;
    if (pass !== confirmPass) {
        alert("Passwords do not match!");
        return false;
    }
    return true;
}
