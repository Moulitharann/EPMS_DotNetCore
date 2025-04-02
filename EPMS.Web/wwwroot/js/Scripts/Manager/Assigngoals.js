$(document).ready(function () {
    var managerId = localStorage.getItem("Id");

    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };

    if (!managerId) {
        toastr.warning("Manager ID is missing! Please log in again.");
        return;
    }

    function loadEmployees() {
        $.ajax({
            url: `http://localhost:5207/api/Manager/employees/${managerId}`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                let employeeDropdown = $("#employeeSelect").empty().append('<option value="">-- Select Employee --</option>');
                data.forEach(employee => {
                    employeeDropdown.append(`<option value="${employee.id}">${employee.name}</option>`);
                });
            },
            error: function (error) {
                toastr.error("Error fetching employees: " + error.responseText);
            }
        });
    }

    function loadGoals() {
        $.ajax({
            url: `http://localhost:5207/api/Manager/goals/${managerId}`,
            method: "GET",
            dataType: "json",
            success: function (data) {
                console.log("Goals fetched:", data); 

                let tableBody = $("#goalTableBody").empty();

                if (data.length === 0) {
                    tableBody.append('<tr><td colspan="6" class="text-center">No goals available</td></tr>');
                    return;
                }

                data.forEach(goal => {
                    let formattedDate = new Date(goal.Deadline).toLocaleDateString("en-GB"); // Format date as dd/mm/yyyy

                    tableBody.append(
                        `<tr data-id="${goal.Id}">
                        <td>${goal.employeeName || "N/A"}</td>
                        <td>${goal.goalText}</td>
                        <td>${goal.weightage}%</td>
                        <td>${goal.deadline}</td>
                        <td>${goal.goalStatus || "Not Started"}</td>
                        <td>
                            <button class="btn btn-warning btn-sm editGoal">Edit</button>   
                            <button class="btn btn-danger btn-sm deleteGoal" data-id="${goal.id}">Delete</button>
                        </td>
                    </tr>`
                    );
                });
            },
            error: function (xhr) {
                console.error("Error fetching goals:", xhr.responseText);
                toastr.error("Error fetching goals: " + (xhr.responseText || "Unexpected error."));
            }
        });
    }
    

    $("#goalForm").on("submit", function (e) {
        e.preventDefault();  // Prevent default form submission

        var goalData = {
            EmployeeId: $("#employeeSelect").val(),
            GoalText: $("#goalTitle").val(),
            Weightage: $("#goalWeightage").val(),
            Deadline: $("#goalDeadline").val(),
            GoalStatus: $("#goalProgress").val(),
            ManagerId: localStorage.getItem("Id")
        };

        console.log("Submitting goal:", goalData);  // Debugging log

        $.ajax({
            url: "http://localhost:5207/api/Manager/assignGoal",  // Ensure this is correct
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(goalData),
            success: function (response) {
                console.log("Goal saved successfully:", response);  // Debugging log

                toastr.success("Goal assigned successfully!");

                // ✅ Reset form
                $("#goalForm")[0].reset();

                // ✅ Forcefully close modal
                $("#goalModal").modal("hide");
                $(".modal-backdrop").remove(); // Fixes background overlay issue
                $("body").removeClass("modal-open"); // Fixes scroll issue

                // ✅ Reload goal list
                loadGoals();
            },
            error: function (xhr) {
                console.error("Error saving goal:", xhr.responseText || xhr);
                toastr.error("Error saving goal: " + (xhr.responseText || "Unexpected error."));
            }
        });
    });


    $(document).on("click", ".editGoal", function () {
        var row = $(this).closest("tr");

        $("#goalId").val(row.data("id"));
        $("#goalTitle").val(row.children().eq(1).text());
        $("#goalWeightage").val(parseInt(row.children().eq(2).text()));
        $("#goalDeadline").val(row.children().eq(3).text());
        $("#goalProgress").val(row.children().eq(4).text());

        var employeeName = row.children().eq(0).text();

        // Find the matching employee ID based on the name
        $("#employeeSelect option").each(function () {
            if ($(this).text() === employeeName) {
                $(this).prop("selected", true);
            }
        });

        $('#goalModal').modal('show');
    });

    $(document).on("click", ".deleteGoal", function () {
        var goalId = $(this).data("id"); // Get goal ID from the button

        if (!goalId) {
            toastr.error("Invalid goal ID.");
            return;
        }

        $.ajax({
            url: `http://localhost:5207/api/Manager/deleteGoal/${goalId}`,
            method: "DELETE",
            success: function () {
                toastr.success("Goal deleted successfully!");
                loadGoals(); // Reload the goals list after deletion
            },
            error: function (xhr) {
                toastr.error("Error deleting goal: " + (xhr.responseText || "Unexpected error."));
            }
        });

    });

    loadEmployees();
    loadGoals();
});