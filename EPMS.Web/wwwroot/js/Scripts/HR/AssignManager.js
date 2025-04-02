$(document).ready(function () {

    
        loadUsers();
        loadEmployees();
        loadManagers();
    loadDropdowns();
    });
    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };
    function loadEmployees() {
        $.get("http://localhost:5207/api/assignmanager/employees", function (data) {
            console.log(data)
            let employeeDropdown = $("#employeeSelect");
            employeeDropdown.empty();
            data.forEach(emp => {
                employeeDropdown.append(`<option value="${emp.id}">${emp.name}</option>`);
            });
        });
    }

    function loadManagers() {
        $.get("http://localhost:5207/api/assignmanager/managers", function (data) {
            console.log(data)
            let managerDropdown = $("#managerSelect");
            managerDropdown.empty();
            data.forEach(manager => {
                managerDropdown.append(`<option value="${manager.id}">${manager.name}</option>`);
            });
        });
    }

    function assignManager() {
        let employeeId = $("#employeeSelect").val();
        let managerId = $("#managerSelect").val();

        $.ajax({
            url: "http://localhost:5207/api/assignmanager/assign",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ EmployeeId: employeeId, ManagerId: managerId }),
            success: function (response) {
                
                toastr.success("Assigned Successfully");
                close();
                loadUsers();
               
            },
            error: function () {
                toastr.error("Error Assigning Goal")
            }
        });
}


function close() {
    $('#assignManagerModal').modal('hide'); 
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove(); 
}

let currentPage = 1;
const pageSize = 3; 
let usersData = []; 

function loadUsers() {
    $.ajax({
        url: "http://localhost:5207/api/assignmanager",
        type: "GET",
        success: function (data) {
            console.log(data);

            if (!data || data.length === 0) {
                toastr.warning("No employees found.");
                usersData = [];
            } else {
           
                usersData = data.filter(user => user.Role !== "Manager");
            }

            currentPage = 1; 
            renderUsers();
        },
        error: function (xhr) {
            toastr.error("Error loading users: " + xhr.responseText);
        }
    });
}
function renderUsers() {
    let tableBody = $("#userTableBody").empty();

    let employees = usersData.filter(user => user.Role !== "Manager");

    let startIndex = (currentPage - 1) * pageSize;
    let endIndex = Math.min(startIndex + pageSize, employees.length);

    let paginatedUsers = employees.slice(startIndex, endIndex);

    paginatedUsers.forEach(user => {
        tableBody.append(`
            <tr>
                <td>${user.name}</td>
                <td>${user.managerName ? user.managerName : "Not Assigned"}</td> 
            </tr>
        `);
    });

    renderPagination(employees.length); 
}

function renderPagination(totalUsers) {
    let totalPages = Math.ceil(totalUsers / pageSize);
    let paginationContainer = $("#pagination").empty();

    if (totalPages > 1) {
        paginationContainer.append(`
            <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="changePage(${currentPage - 1})">Previous</a>
            </li>
        `);

        for (let i = 1; i <= totalPages; i++) {
            paginationContainer.append(`
                <li class="page-item ${i === currentPage ? 'active' : ''}">
                    <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
                </li>
            `);
        }

        paginationContainer.append(`
            <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="changePage(${currentPage + 1})">Next</a>
            </li>
        `);
    }
}


function changePage(page) {
    let totalPages = Math.ceil(usersData.length / pageSize);

    if (page < 1 || page > totalPages) return;
    currentPage = page;
    renderUsers();
}
