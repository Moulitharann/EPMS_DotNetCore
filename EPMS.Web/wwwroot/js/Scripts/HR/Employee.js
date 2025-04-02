$(document).ready(function () {
    loadUsers();

    // Show/Hide category field based on role selection
    $("#userRole").change(function () {
        toggleCategory();
    });

    // Search event listener
    $("#searchInput").on("input", searchUsers);

    // Category filter event listener
    $("#categoryFilter").change(filterByCategory);
});

var usersData = []; // Store all user data
var currentPage = 1;
var rowsPerPage = 5;

function showLoading(show) {
    $("#loading").toggle(show);
}

function toggleCategory() {
    if ($("#userRole").val() === "Employee") {
        $("#categoryGroup").show();
    } else {
        $("#categoryGroup").hide();
    }
}

function loadUsers() {
    showLoading(true);
    $.ajax({
        url: "http://localhost:5207/api/assignmanager",
        type: "GET",
        success: function (data) {
            console.log(data);
            usersData = data;
            renderTable();
            setupPagination();
        },
        error: function (xhr) {
            toastr.error("Error loading users:", xhr.responseText);
        },
        complete: function () {
            showLoading(false);
        }
    });
}

function renderTable() {
    var tableBody = $("#userTableBody").empty();
    var start = (currentPage - 1) * rowsPerPage;
    var end = start + rowsPerPage;
    var paginatedUsers = usersData.slice(start, end);

    paginatedUsers.forEach(user => {
        tableBody.append(`
            <tr>
                <td>${user.name}</td>
                <td>${user.email}</td>
                <td>${user.role}</td>
                <td>${user.role === "Employee" ? (user.managerName ?? "Not Assigned") : "-"}</td>
                <td>${user.category || "-"}</td>
                <td>
                    <button class="btn btn-warning btn-sm" onclick="showEditUserModal(${user.id}, '${user.name}', '${user.email}', '${user.role}', '${user.category || ""}', '${user.managerId || ""}')">Edit</button>
                    <button class="btn btn-danger btn-sm" onclick="deleteUser(${user.id})">Delete</button>
                </td>
            </tr>
        `);
    });
}

function setupPagination() {
    var totalPages = Math.ceil(usersData.length / rowsPerPage);
    var pagination = $("#pagination").empty();

    if (totalPages > 1) {
        pagination.append(`
            <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="changePage(${currentPage - 1})">Previous</a>
            </li>
        `);

        for (var i = 1; i <= totalPages; i++) {
            pagination.append(`
                <li class="page-item ${i === currentPage ? 'active' : ''}">
                    <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
                </li>
            `);
        }

        pagination.append(`
            <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="changePage(${currentPage + 1})">Next</a>
            </li>
        `);
    }
}

function changePage(page) {
    if (page < 1 || page > Math.ceil(usersData.length / rowsPerPage)) return;
    currentPage = page;
    renderTable();
    setupPagination();
}

function searchUsers() {
    let searchText = $("#searchInput").val().toLowerCase();
    $("#userTableBody tr").each(function () {
        let rowText = $(this).text().toLowerCase();
        $(this).toggle(rowText.includes(searchText));
    });
}

function filterByCategory() {
    let selectedCategory = $("#categoryFilter").val().toLowerCase();
    $("#userTableBody tr").each(function () {
        let category = $(this).find("td:nth-child(5)").text().toLowerCase();
        $(this).toggle(selectedCategory === "" || category === selectedCategory);
    });
}

function sortTable(columnIndex) {
    let table = document.querySelector(".table tbody");
    let rows = Array.from(table.rows);
    let ascending = table.getAttribute("data-sort-order") !== "asc";
    table.setAttribute("data-sort-order", ascending ? "asc" : "desc");

    rows.sort((a, b) => {
        let aText = a.cells[columnIndex].textContent.trim().toLowerCase();
        let bText = b.cells[columnIndex].textContent.trim().toLowerCase();
        return ascending ? aText.localeCompare(bText) : bText.localeCompare(aText);
    });

    table.innerHTML = "";
    rows.forEach(row => table.appendChild(row));
}

function showAddUserModal() {
    $("#userId").val("");
    $("#userName").val("");
    $("#userEmail").val("");
    $("#userRole").val("Manager");
    $("#userCategory").val("HR");
    toggleCategory();
    $("#userModal").modal("show");
    $("#saveUserBtn").text("Add User").off("click").on("click", addUser);
}

function showEditUserModal(id, name, email, role, category) {
    $("#userId").val(id);
    $("#userName").val(name);
    $("#userEmail").val(email);
    $("#userRole").val(role);
    $("#userCategory").val(category || "HR");
    toggleCategory();
    $("#userModal").modal("show");
    $("#saveUserBtn").text("Update User").off("click").on("click", function () {
        updateUser(id);
    });
}

function addUser() {
    var userRole = $("#userRole").val();

    var user = {
        Name: $("#userName").val(),
        Email: $("#userEmail").val(),
        Role: userRole,
        Category: userRole === "Employee" ? $("#userCategory").val() : "UnCategorized",
        Manager: userRole === "Employee" ? $("#managerSelect").val() : null, 
        PasswordHash: "Default@123",
        
    };

    if (!user.Name || !user.Email || !user.Role) {
        toastr.error("Please fill in all required fields.");
        return;
    }

    showLoading(true);
    $.ajax({
        url: "http://localhost:5207/api/assignmanager",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(user),
        success: function () {
            toastr.success("User added successfully.");
            $("#userModal").modal("hide");
            loadUsers();
        },
        error: function (xhr) {
            toastr.error("Error adding user: " + xhr.responseText);
        },
        complete: function () {
            showLoading(false);
        }
    });
}

async function getPassword(id) {
    try {
        let response = await $.ajax({
            url: `http://localhost:5207/api/assignmanager/GetPassword/${id}`,
            type: "GET",
            contentType: "application/json",
        });

        return response;
    } catch (error) {
        console.error("Error fetching password:", error);
    }
}

async function updateUser(id) {
    var existingPassword = await getPassword(id);

    var user = {
        Id: id,
        Name: $("#userName").val(),
        Email: $("#userEmail").val(),
        Role: $("#userRole").val(),
        Category: $("#userRole").val() === "Employee" ? $("#userCategory").val() : null,
        PasswordHash: existingPassword.toString(),
    };

    showLoading(true);
    $.ajax({
        url: `http://localhost:5207/api/assignmanager/${id}`,
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(user),
        success: function () {
            toastr.success("User updated successfully.");
            $("#userModal").modal("hide");
            loadUsers();
        },
        error: function (xhr) {
            toastr.error("Error updating user:", xhr.responseText);
        },
        complete: function () {
            showLoading(false);
        }
    });
}

function deleteUser(id) {
    if (confirm("Are you sure you want to delete this user?")) {
        showLoading(true);
        $.ajax({
            url: `http://localhost:5207/api/assignmanager/${id}`,
            type: "DELETE",
            success: function () {
                toastr.success("User deleted successfully.");
                loadUsers();
            },
            error: function (xhr) {
                toastr.error("Error deleting user:", xhr.responseText);
            },
            complete: function () {
                showLoading(false);
            }
        });
    }
}
