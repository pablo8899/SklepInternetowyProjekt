$(function () {
    const API_AUTH_URL = "/api/auth";
    const RegisterError = document.getElementById("signup-errors");
    const LoginError = document.getElementById("signin-errors");

    $("#showModal").on("click", () => {
        $("#loginForm").show();
    });

    $("#closeModal").on("click", () => {
        $("#loginForm").hide();
    });

    $("#SignIn").on("submit", async function (e) {

        e.preventDefault();
        LoginError.style.display = "none";
        LoginError.innerText = "";
        const response = await fetch(API_AUTH_URL + "/login", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: getFormData($(this))
        });

        HandleError(await response.json())
        

    });

    $("#SignUp").on("submit", async function (e) {

        e.preventDefault();
        RegisterError.style.display = "none";
        RegisterError.innerText = "";
        const response = await fetch(API_AUTH_URL + "/register", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: getFormData($(this))
        });

        HandleError(await response.json())
    })

    $("#logout").on("click", async function (e) {

        e.preventDefault();

        const response = await fetch(API_AUTH_URL + "/logout", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: getFormData($(this))
        });

        HandleError(await response.json())
    })
})

function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return JSON.stringify(indexed_array);
}

function HandleError(res,reload = true)
{
    let errMessage = document.querySelector("[class='errorMessage']");
    errMessage.innerText = "";
    errMessage.parentElement.style.display = "none";
    let errorClass = document.querySelectorAll("[class~='error']");

    for (var i = 0; i < errorClass.length; i++) {
        errorClass[i].innerText = ""
    }

    if (res.errors) {

        for (const [key, value] of Object.entries(res.errors)) {
            let input = document.querySelector("[name='" + key.toLowerCase() + "']");
            let errorDiv = input.parentElement.children[1];
            let errors = "";
            for (var i = 0; i < value.length; i++) {
                errors = value[i]+"<br>";
            }

            errorDiv.innerHTML = errors;
        }
        return;
    }


    if (!res.success) {
        errMessage.innerText = res.message;
        errMessage.parentElement.style.display = "";
    } else {
        window.location.reload(reload);
    }

}