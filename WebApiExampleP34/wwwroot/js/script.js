
let token = null


document.addEventListener("DOMContentLoaded", function () {

    if (localStorage.getItem("token")) {
        token = localStorage.getItem("token")
        console.log("Token from local storage: ", token)
    }


    document.querySelector("form#login-form").addEventListener("submit", function (event) {

        event.preventDefault();

        const formData = new FormData(event.target);

        const data = Object.fromEntries(formData.entries());

        console.log(data);

        fetch("/api/v1/auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }).then(response => response.json())
            .then(result => {
                console.log("Success:", result);

                localStorage.setItem("token", result.token)

                // Видалити форму
                // зробити запит на профіль
                // вивести інформацію про профіль


            })
            .catch(error => console.error("Error:", error));



    })

})
