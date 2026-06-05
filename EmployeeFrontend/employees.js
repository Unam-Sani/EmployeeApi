const API_BASE = "https://localhost:5001/api"; // same as in auth.js

function getToken() {
    return localStorage.getItem("jwtToken");
}

function ensureAuthenticated() {
    const token = getToken();
    if (!token) {
        window.location.href = "index.html";
    }
}

window.onload = () => {
    ensureAuthenticated();
    loadEmployees();
};

async function loadEmployees() {
    const token = getToken();
    const response = await fetch(`${API_BASE}/employees`, {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    const employees = await response.json();

    const list = document.getElementById("employeeList");
    list.innerHTML = "";

    employees.forEach(emp => {
        const card = document.createElement("div");
        card.className = "employee-card";

        card.innerHTML = `
            <label>ID</label>
            <input type="text" value="${emp.id}" disabled>

            <label>Name</label>
            <input type="text" value="${emp.name}" disabled>

            <label>Email</label>
            <input type="text" value="${emp.email}" disabled>

            <label>Department</label>
            <input type="text" value="${emp.department}" disabled>

            <button class="action-btn edit-btn" onclick="editEmployee(${emp.id})">Edit</button>
            <button class="action-btn delete-btn" onclick="deleteEmployee(${emp.id})">Delete</button>
        `;

        list.appendChild(card);
    });
}

async function saveEmployee() {
    const token = getToken();
    const id = document.getElementById("employeeId").value;
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const department = document.getElementById("department").value;

    const employee = { name, email, department };

    const url = id ? `${API_BASE}/employees/${id}` : `${API_BASE}/employees`;
    const method = id ? "PUT" : "POST";

    const response = await fetch(url, {
        method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(employee)
    });

    if (response.status === 401) {
        logout();
        return;
    }

    clearForm();
    loadEmployees();
}

async function editEmployee(id) {
    const token = getToken();

    const response = await fetch(`${API_BASE}/employees/${id}`, {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    const emp = await response.json();

    document.getElementById("employeeId").value = emp.id;
    document.getElementById("name").value = emp.name;
    document.getElementById("email").value = emp.email;
    document.getElementById("department").value = emp.department;

    window.scrollTo({ top: 0, behavior: "smooth" });
}

async function deleteEmployee(id) {
    const token = getToken();

    const response = await fetch(`${API_BASE}/employees/${id}`, {
        method: "DELETE",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (response.status === 401) {
        logout();
        return;
    }

    loadEmployees();
}

function clearForm() {
    document.getElementById("employeeId").value = "";
    document.getElementById("name").value = "";
    document.getElementById("email").value = "";
    document.getElementById("department").value = "";
}

function logout() {
    localStorage.removeItem("jwtToken");
    window.location.href = "index.html";
}
