var regFNameOk = false;
var regLNameOk = false;
var regEmailOk = false;
var regPassOk = false;
var regDobOk = false;
var regCardNoOk = false;
var regExpiryOk = false;
var newPassOk = false;

if (location.pathname == "/Register") {
    var dateToday = new Date().toISOString().substring(0, 10);
    var thisMonth = new Date();
    thisMonth.setMonth(thisMonth.getMonth() + 1)
    document.querySelector("#MainContent_tbDateOfBirth").setAttribute("max", dateToday);
    document.querySelector("#MainContent_tbExpiry").setAttribute("min", thisMonth.toISOString().substring(0, 7));
    if (document.querySelector("#MainContent_tbFirstName").value != "") {
        checkFName();
        checkLName();
        checkEmail();
        checkDob();
        cardNumberValidate(1);
        cardNumberValidate(2);
        cardNumberValidate(3);
        cardNumberValidate(4);
        checkExpiry();
    }
    document.querySelector("form").setAttribute("onsubmit", "event.preventDefault();checkRegistrationInputs();");
    document.querySelector("#MainContent_tbFirstName").addEventListener("input", function () {
        checkFName();
    })

    function checkFName() {
        if (/^[A-Za-z0-9\s]+$/.test(document.querySelector("#MainContent_tbFirstName").value)) {
            document.querySelector("#MainContent_tbFirstName").style.border = "1px solid #42dc00";
            regFNameOk = true;
        } else {
            document.querySelector("#MainContent_tbFirstName").style.border = "1px solid #e50000";
            regFNameOk = false;
        }
    }

    document.querySelector("#MainContent_tbLastName").addEventListener("input", function () {
        checkLName();
    })

    function checkLName() {
        if (/^[A-Za-z0-9\s]+$/.test(document.querySelector("#MainContent_tbLastName").value)) {
            document.querySelector("#MainContent_tbLastName").style.border = "1px solid #42dc00";
            regLNameOk = true;
        } else {
            document.querySelector("#MainContent_tbLastName").style.border = "1px solid #e50000";
            regLNameOk = false;
        }
    }

    document.querySelector("#MainContent_tbEmail").addEventListener("input", function () {
        checkEmail();
    })

    function checkEmail() {
        if (/(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])/.test(document.querySelector("#MainContent_tbEmail").value)) {
            document.querySelector("#MainContent_tbEmail").style.border = "1px solid #42dc00";
            regEmailOk = true;
        } else {
            document.querySelector("#MainContent_tbEmail").style.border = "1px solid #e50000";
            regEmailOk = false;
        }
    }

    var registerPasswordField = document.getElementById("MainContent_tbPassword");

    registerPasswordField.addEventListener("focusin", function () {
        document.querySelector(".passComplexCheck").style.display = "block";
    })

    registerPasswordField.addEventListener("focusout", function () {
        document.querySelector(".passComplexCheck").style.display = "none";
    })

    registerPasswordField.addEventListener("input", function () {
        if (registerPasswordField.value.length > 11) {
            document.querySelector(".passwordLength").classList.remove("fa-times");
            document.querySelector(".passwordLength").classList.add("fa-check");
            document.querySelector(".passwordLength").classList.remove("passwordNotMet");
            document.querySelector(".passwordLength").classList.add("passwordMet");
        } else if (registerPasswordField.value.length < 12) {
            document.querySelector(".passwordLength").classList.add("fa-times");
            document.querySelector(".passwordLength").classList.remove("fa-check");
            document.querySelector(".passwordLength").classList.add("passwordNotMet");
            document.querySelector(".passwordLength").classList.remove("passwordMet");
        }
        if (/[A-Z]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordUppercase").classList.remove("fa-times");
            document.querySelector(".passwordUppercase").classList.add("fa-check");
            document.querySelector(".passwordUppercase").classList.remove("passwordNotMet");
            document.querySelector(".passwordUppercase").classList.add("passwordMet");
        } else if (!/[A-Z]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordUppercase").classList.add("fa-times");
            document.querySelector(".passwordUppercase").classList.remove("fa-check");
            document.querySelector(".passwordUppercase").classList.add("passwordNotMet");
            document.querySelector(".passwordUppercase").classList.remove("passwordMet");
        }
        if (/[a-z]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordLowercase").classList.remove("fa-times");
            document.querySelector(".passwordLowercase").classList.add("fa-check");
            document.querySelector(".passwordLowercase").classList.remove("passwordNotMet");
            document.querySelector(".passwordLowercase").classList.add("passwordMet");
        } else if (!/[a-z]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordLowercase").classList.add("fa-times");
            document.querySelector(".passwordLowercase").classList.remove("fa-check");
            document.querySelector(".passwordLowercase").classList.add("passwordNotMet");
            document.querySelector(".passwordLowercase").classList.remove("passwordMet");
        }
        if (/\d/.test(registerPasswordField.value)) {
            document.querySelector(".passwordDigit").classList.remove("fa-times");
            document.querySelector(".passwordDigit").classList.add("fa-check");
            document.querySelector(".passwordDigit").classList.remove("passwordNotMet");
            document.querySelector(".passwordDigit").classList.add("passwordMet");
        } else if (!/\d/.test(registerPasswordField.value)) {
            document.querySelector(".passwordDigit").classList.add("fa-times");
            document.querySelector(".passwordDigit").classList.remove("fa-check");
            document.querySelector(".passwordDigit").classList.add("passwordNotMet");
            document.querySelector(".passwordDigit").classList.remove("passwordMet");
        }
        if (/[^A-Za-z\d]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordSpecial").classList.remove("fa-times");
            document.querySelector(".passwordSpecial").classList.add("fa-check");
            document.querySelector(".passwordSpecial").classList.remove("passwordNotMet");
            document.querySelector(".passwordSpecial").classList.add("passwordMet");
        } else if (!/[^A-Za-z\d]/.test(registerPasswordField.value)) {
            document.querySelector(".passwordSpecial").classList.add("fa-times");
            document.querySelector(".passwordSpecial").classList.remove("fa-check");
            document.querySelector(".passwordSpecial").classList.add("passwordNotMet");
            document.querySelector(".passwordSpecial").classList.remove("passwordMet");
        }
        if (registerPasswordField.value.length > 11 && /[A-Z]/.test(registerPasswordField.value) && /[a-z]/.test(registerPasswordField.value) && /\d/.test(registerPasswordField.value) && /[^A-Za-z\d]/.test(registerPasswordField.value)) {
            regPassOk = true;
            registerPasswordField.style.border = "1px solid #42dc00";
        } else {
            regPassOk = false;
            registerPasswordField.style.border = "1px solid #e50000";
        }
    })
    document.querySelector("#MainContent_tbDateOfBirth").addEventListener("change", function () {
        checkDob();
    })

    function checkDob() {
        if (document.querySelector("#MainContent_tbDateOfBirth").value == "") {
            document.querySelector("#MainContent_tbDateOfBirth").style.border = "1px solid #e50000";
            regDobOk = false;
        } else {
            document.querySelector("#MainContent_tbDateOfBirth").style.border = "1px solid #42dc00";
            regDobOk = true;
        }
    }

    document.querySelector("#MainContent_tbExpiry").addEventListener("change", function () {
        checkExpiry();
    })

    function checkExpiry() {
        if (document.querySelector("#MainContent_tbExpiry").value == "") {
            document.querySelector("#MainContent_tbExpiry").style.border = "1px solid #e50000";
            regExpiryOk = false;
        } else {
            document.querySelector("#MainContent_tbExpiry").style.border = "1px solid #42dc00";
            regExpiryOk = true;
        }
    }

    function checkCardNo() {
        if ((document.getElementById("MainContent_tbCardNumber1").value + document.getElementById("MainContent_tbCardNumber2").value + document.getElementById("MainContent_tbCardNumber3").value + document.getElementById("MainContent_tbCardNumber4").value).toString().length == 16) {
            regCardNoOk = true;
        } else {
            regCardNoOk = false;
        }
    }
} else if (location.pathname == "/Reset" || location.pathname == "/Profile") {
    var newPasswordField = document.getElementById("MainContent_tbNewPassword");

    newPasswordField.addEventListener("focusin", function () {
        document.querySelector(".passComplexCheck").style.display = "block";
    })

    newPasswordField.addEventListener("focusout", function () {
        document.querySelector(".passComplexCheck").style.display = "none";
    })

    newPasswordField.addEventListener("input", function () {
        if (newPasswordField.value.length > 11) {
            document.querySelector(".passwordLength").classList.remove("fa-times");
            document.querySelector(".passwordLength").classList.add("fa-check");
            document.querySelector(".passwordLength").classList.remove("passwordNotMet");
            document.querySelector(".passwordLength").classList.add("passwordMet");
        } else if (newPasswordField.value.length < 12) {
            document.querySelector(".passwordLength").classList.add("fa-times");
            document.querySelector(".passwordLength").classList.remove("fa-check");
            document.querySelector(".passwordLength").classList.add("passwordNotMet");
            document.querySelector(".passwordLength").classList.remove("passwordMet");
        }
        if (/[A-Z]/.test(newPasswordField.value)) {
            document.querySelector(".passwordUppercase").classList.remove("fa-times");
            document.querySelector(".passwordUppercase").classList.add("fa-check");
            document.querySelector(".passwordUppercase").classList.remove("passwordNotMet");
            document.querySelector(".passwordUppercase").classList.add("passwordMet");
        } else if (!/[A-Z]/.test(newPasswordField.value)) {
            document.querySelector(".passwordUppercase").classList.add("fa-times");
            document.querySelector(".passwordUppercase").classList.remove("fa-check");
            document.querySelector(".passwordUppercase").classList.add("passwordNotMet");
            document.querySelector(".passwordUppercase").classList.remove("passwordMet");
        }
        if (/[a-z]/.test(newPasswordField.value)) {
            document.querySelector(".passwordLowercase").classList.remove("fa-times");
            document.querySelector(".passwordLowercase").classList.add("fa-check");
            document.querySelector(".passwordLowercase").classList.remove("passwordNotMet");
            document.querySelector(".passwordLowercase").classList.add("passwordMet");
        } else if (!/[a-z]/.test(newPasswordField.value)) {
            document.querySelector(".passwordLowercase").classList.add("fa-times");
            document.querySelector(".passwordLowercase").classList.remove("fa-check");
            document.querySelector(".passwordLowercase").classList.add("passwordNotMet");
            document.querySelector(".passwordLowercase").classList.remove("passwordMet");
        }
        if (/\d/.test(newPasswordField.value)) {
            document.querySelector(".passwordDigit").classList.remove("fa-times");
            document.querySelector(".passwordDigit").classList.add("fa-check");
            document.querySelector(".passwordDigit").classList.remove("passwordNotMet");
            document.querySelector(".passwordDigit").classList.add("passwordMet");
        } else if (!/\d/.test(newPasswordField.value)) {
            document.querySelector(".passwordDigit").classList.add("fa-times");
            document.querySelector(".passwordDigit").classList.remove("fa-check");
            document.querySelector(".passwordDigit").classList.add("passwordNotMet");
            document.querySelector(".passwordDigit").classList.remove("passwordMet");
        }
        if (/[^A-Za-z\d]/.test(newPasswordField.value)) {
            document.querySelector(".passwordSpecial").classList.remove("fa-times");
            document.querySelector(".passwordSpecial").classList.add("fa-check");
            document.querySelector(".passwordSpecial").classList.remove("passwordNotMet");
            document.querySelector(".passwordSpecial").classList.add("passwordMet");
        } else if (!/[^A-Za-z\d]/.test(newPasswordField.value)) {
            document.querySelector(".passwordSpecial").classList.add("fa-times");
            document.querySelector(".passwordSpecial").classList.remove("fa-check");
            document.querySelector(".passwordSpecial").classList.add("passwordNotMet");
            document.querySelector(".passwordSpecial").classList.remove("passwordMet");
        }
        if (newPasswordField.value.length > 11 && /[A-Z]/.test(newPasswordField.value) && /[a-z]/.test(newPasswordField.value) && /\d/.test(newPasswordField.value) && /[^A-Za-z\d]/.test(newPasswordField.value)) {
            newPassOk = true;
            newPasswordField.style.border = "1px solid #42dc00";
        } else {
            newPassOk = false;
            newPasswordField.style.border = "1px solid #e50000";
        }
    })
}

function cardNumberValidate(no) {
    if (no != 4) {
        if (/^[0-9]{4}(?!=-)$/.test(document.getElementById("MainContent_tbCardNumber" + no).value)) {
            document.getElementById("MainContent_tbCardNumber" + (no + 1)).focus();
            document.getElementById("MainContent_tbCardNumber" + no).style.border = "1px solid #42dc00";
            document.getElementById("MainContent_tbCardNumber" + no).value = document.getElementById("MainContent_tbCardNumber" + no).value.substring(0, 4);
            regCardNoOk = true;
        } else {
            document.getElementById("MainContent_tbCardNumber" + no).style.border = "1px solid #e50000";
            regCardNoOk = false;
        }
    } else {
        if (/^[0-9]{4}(?!=-)$/.test(document.getElementById("MainContent_tbCardNumber4").value)) {
            document.getElementById("MainContent_tbExpiry").focus();
            document.getElementById("MainContent_tbCardNumber4").style.border = "1px solid #42dc00";
            document.getElementById("MainContent_tbCardNumber4").value = document.getElementById("MainContent_tbCardNumber4").value.substring(0, 4);
            regCardNoOk = true;
        } else {
            document.getElementById("MainContent_tbCardNumber4").style.border = "1px solid #e50000";
            regCardNoOk = false;
        }
    }
}

function checkRegistrationInputs() {
    if (!document.getElementById("clientRegistrationError")) {
        var registrationErrorSpan = document.createElement("span");
        registrationErrorSpan.setAttribute("id", "clientRegistrationError");
        document.getElementById("MainContent_lbMemberRegistration").parentNode.insertBefore(registrationErrorSpan, document.getElementById("MainContent_lbMemberRegistration").nextSibling);
        registrationErrorSpan.setAttribute("style", "color:#e50000;margin-bottom:20px;");
    }
    var registrationError = "";
    if (!regFNameOk) {
        registrationError += "First Name is empty or invalid\n";
    }
    if (!regLNameOk) {
        registrationError += "Last Name is empty or invalid\n";
    }
    if (!regEmailOk) {
        registrationError += "Email does not meet requirements\n";
    }
    if (!regPassOk) {
        registrationError += "Password does not meet requirements\n";
    }
    if (!regDobOk) {
        registrationError += "Date of Birth is empty\n";
    }
    if (!regCardNoOk) {
        registrationError += "Card Number must be 16 digits\n";
    }
    if (!regExpiryOk) {
        registrationError += "Card Expiry is empty\n";
    }
    document.getElementById("clientRegistrationError").innerText = "";
    document.getElementById("clientRegistrationError").innerText = registrationError;
    if (regFNameOk && regLNameOk && regEmailOk && regPassOk && regDobOk && regCardNoOk && regExpiryOk) {
        document.querySelector("#MainContent_btnRegister").setAttribute("disabled", "true");
        document.querySelector("form").submit();
    }
}

function checkResetPassInputs() {
    if (!document.getElementById("clientResetError")) {
        var resetErrorSpan = document.createElement("span");
        resetErrorSpan.setAttribute("id", "clientResetError");
        document.getElementById("MainContent_lbLogin").parentNode.insertBefore(resetErrorSpan, document.getElementById("MainContent_lbLogin").nextSibling);
        resetErrorSpan.setAttribute("style", "color:#e50000;margin-bottom:20px;");
    }
    var resetError = "";
    if (!newPassOk) {
        resetError += "New Password does not meet requirements\n";
    }
    document.getElementById("clientResetError").innerText = "";
    document.getElementById("clientResetError").innerText = resetError;
    if (newPassOk) {
        document.querySelector("#MainContent_btnReset").setAttribute("disabled", "true");
        document.querySelector("form").submit();
    }
}

function showChangePasswordModal() {
    document.querySelector("#changePasswordBackground").style.display = "flex";
}

function closeChangePasswordModal() {
    document.querySelector("#changePasswordBackground").style.display = "none";
}