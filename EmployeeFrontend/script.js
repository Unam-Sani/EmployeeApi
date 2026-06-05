const API_BASE = "https://localhost:5001/api"; // adjust port if needed

async function login() {
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const errorEl = document.getElementById("loginError");
    errorEl.textContent = "";

    try {
        const response = await fetch(`${API_BASE}/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            errorEl.textContent = "Invalid credentials or server error.";
            return;
        }

        const data = await response.json();

        // Adjust this if your property name is different (e.g. data.jwtToken)
        const token = data.token;

        if (!token) {
            errorEl.textContent = "No token returned from server.";
            return;
        }

        localStorage.setItem("jwtToken", token);

        window.location.href = "employees.html";
    } catch (err) {
        console.error(err);
        errorEl.textContent = "Could not connect to server.";
    }
}
