alert("auth.js loaded");

async function login() {
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const response = await fetch("http://localhost:5085/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem("jwtToken", data.token);
        window.location.href = "employees.html";
    } else {
        document.getElementById("loginError").textContent = "Invalid email or password.";
    }
}
