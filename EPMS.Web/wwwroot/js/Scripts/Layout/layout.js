$(document).ready(function () {
    updateNavbar();
});

function updateNavbar() {
    var userRole = localStorage.getItem("userRole");
    var userName = localStorage.getItem("userName");
    var currentPath = window.location.pathname; // Get the current URL path
    var roleFeatures = "";

    if (userRole === "HRAdmin") {
        roleFeatures = `
                        <li class="nav-item"><a href="/HRAdmin/Dashboard" class="nav-link">HR Dashboard</a></li>
                        <li class="nav-item"><a href="/HRAdmin/employees" class="nav-link">Manage Employees</a></li>
                        <li class="nav-item"><a href="/HRAdmin/AssignManager" class="nav-link">Assign Manager</a></li>
                        <li class="nav-item"><a href="/HRAdmin/EvaluationCriteria" class="nav-link">Evaluation Criteria</a></li>
                        <li class="nav-item"><a href="/HRAdmin/PerformanceReports" class="nav-link">Generate Report</a></li>
                    `;
    } else if (userRole === "Manager") {
        roleFeatures = `
                        <li class="nav-item"><a href="/Manager/Dashboard" class="nav-link">Manager Dashboard</a></li>
                        <li class="nav-item"><a href="/Manager/AssignGoals" class="nav-link">Manage Goals</a></li>
                        <li class="nav-item"><a href="/Manager/PerformanceReview" class="nav-link">Performance Review</a></li>
                    `;
    } else if (userRole === "Employee") {
        roleFeatures = `
                        <li class="nav-item"><a href="/Employee/Dashboard" class="nav-link">Dashboard</a></li>
                        <li class="nav-item"><a href="/Employee/Feedback" class="nav-link">Request Feedback</a></li>
                        <li class="nav-item"><a href="/Employee/PerformanceHistory" class="nav-link">Performance History</a></li>
                    `;
    }

    $("#roleFeatures").html(roleFeatures);

    // Highlight the active menu item
    $("#roleFeatures .nav-link").each(function () {
        if ($(this).attr("href") === currentPath) {
            $(this).addClass("active"); // Bootstrap 'active' class
        }
    });

    if (userRole && userName) {
        $("#authLinks").html(`
                        <li class="nav-item">
<a href="#" class="nav-link btn custom-logout-btn" id="logoutBtn">Logout</a>
                        </li>
                    `);

        $("#logoutBtn").click(function (e) {
            e.preventDefault();
            localStorage.clear();
            window.location.href = "/Auth/Login";
        });
    } else {
        $("#authLinks").html(`
                        <div class="d-flex align-items-center">
                            <a href="/Auth/Login" class="nav-link btn btn-outline-light mx-2 d-inline-block">Login</a>
                            <a href="/Auth/Register" class="nav-link btn btn-outline-light d-inline-block">Register</a>
                        </div>
                    `);
    }
}