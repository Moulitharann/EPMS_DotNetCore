$(document).ready(function () {
    var managerId = localStorage.getItem("Id");
    var managerName = localStorage.getItem("userName") || "Manager";
    var token = localStorage.getItem("token"); 

    

    $("#managerName").text(managerName);
    loadEmployees(managerId);

    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };

    $("#redirect").click(function () {
        window.location.href = "/Manager/AssignGoals";
    });

    $("#redirectto").click(function () {
        window.location.href = "/Manager/PerformanceReview";
    });

    function loadEmployees(managerId) {
        $.ajax({
            url: `http://localhost:5207/api/Manager/employees/${managerId}`,
            type: "GET",
            headers: {
                "Authorization": "Bearer " + token // Include authentication token
            },
            success: function (employees) {
                var employeeTable = $("#employeeTable");
                employeeTable.empty();

                if (!Array.isArray(employees) || employees.length === 0) {
                    toastr.warning("No employees found.");
                    employeeTable.append('<tr><td colspan="3" class="text-center">No employees found</td></tr>');
                    return;
                }

                var tempid = 1;
                employees.forEach(employee => {
                    var row = `<tr>
                        <td>${tempid++}</td>
                        <td>${employee.name}</td> <!-- Fixed field -->
                        <td>
                            <button class="btn btn-success btn-sm provide-feedback" data-id="${employee.id}">
                                Provide Feedback
                            </button>
                        </td>
                    </tr>`;
                    employeeTable.append(row);
                });

                $("#employeeTable").on("click", ".provide-feedback", function () {
                    var employeeId = $(this).data("id");
                    if (!employeeId) {
                        toastr.warning("Invalid employee ID.");
                        return;
                    }
                    window.location.href = `Feedback?employeeId=${employeeId}`;
                });
            },
            error: function (xhr) {
                toastr.error("Error loading employees: " + (xhr.responseText || "Unknown error."));
                $("#employeeTable").html('<tr><td colspan="3" class="text-center">Error loading employees</td></tr>');
            }
        });
    }

    $("#viewNotifications").click(function () {
        $.ajax({
            url: `http://localhost:5207/api/Manager/Notifications/${managerId}`,
            type: "GET",
            headers: {
                "Authorization": "Bearer " + token // Include authentication token
            },
            success: function (notifications) {
                var notificationTable = $("#notificationTable");
                notificationTable.empty();

                if (!Array.isArray(notifications) || notifications.length === 0) {
                    toastr.warning("No review requests found.");
                    notificationTable.append('<tr><td colspan="3" class="text-center">No review requests found.</td></tr>');
                    return;
                }

                notifications.forEach(notification => {
                    var row = `<tr>
                        <td>${notification.employeeName}</td> <!-- Fixed field -->
                        <td>${notification.message}</td>
                        
                    </tr>`;
                    notificationTable.append(row);
                });

                $("#notificationTable").on("click", ".mark-read", function () {
                    var notificationId = $(this).data("id");

                    if (!notificationId) {
                        toastr.warning("Invalid notification ID.");
                        return;
                    }

                    $.ajax({
                        url: `http://localhost:5207/api/Manager/MarkNotificationRead/${notificationId}`,
                        type: "POST",
                        headers: {
                            "Authorization": "Bearer " + token
                        },
                        success: function () {
                            toastr.success("Notification marked as read.");
                            $("#viewNotifications").click(); // Refresh notifications
                        },
                        error: function () {
                            toastr.error("Error marking notification as read.");
                        }
                    });
                });

                $("#notificationsModal").modal("show");
            },
            error: function (xhr) {
                toastr.error("Error loading notifications: " + (xhr.responseText || "Unknown error."));
                $("#notificationTable").html('<tr><td colspan="3" class="text-center">Error loading notifications</td></tr>');
            }
        });
    });
});
