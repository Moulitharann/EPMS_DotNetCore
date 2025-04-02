$(document).ready(function () {

    // Open modal to add a new goal
    $("#openGoalModal").click(function () {
        $("#goalId").val("");
        $("#goalTypeText").val("");
        $("#goalDeadline").val("");
        $("#taskStatus").val("Not Started");
        $("#goalModal").data("isPersonalGoal", 1);
        $("#goalModal").modal("show");
    });

    loadGoals();

    // Save or update goal when the button is clicked
    $("#saveGoal").click(function () {
        var employeeId = parseInt(window.localStorage.getItem("Id")); // Convert to number
        var goalData = {
            EmployeeId: employeeId,
            GoalText: $("#goalTypeText").val(),
            Deadline: $("#goalDeadline").val(),
            GoalStatus: $("#taskStatus").val(),
            IsPersonalGoal: Number($("#goalModal").data("isPersonalGoal")), // Ensure number
            //managerId: null
        };

        console.log("Submitting Goal Data:", goalData); // Debugging

        var editId = $("#goalId").val();
        if (editId) {
            updateGoal(editId, goalData);
        } else {
            addGoal(goalData);
        }
    });

    // Open modal for editing a goal
    $(document).on("click", ".edit-goal", function () {
        var goalId = $(this).data("id");
        var goalText = $(this).data("text");
        var goalDeadline = $(this).data("deadline");
        var goalStatus = $(this).data("status");
        var isPersonalGoal = $(this).data("personal");

        $("#goalId").val(goalId);
        $("#goalTypeText").val(goalText);
        $("#goalDeadline").val(goalDeadline);
        $("#taskStatus").val(goalStatus);
        $("#goalModal").data("isPersonalGoal", isPersonalGoal ? 1 : 0);
        $("#goalModal").modal("show");
    });

    // Function to add a new goal
    // Function to add a new goal
    function addGoal(goalData) {
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/SetGoal",
            type: "POST",
            contentType: "application/json",
            dataType: "text",  // <-- Change from "json" to "text" to handle string responses
            data: JSON.stringify(goalData),
            success: function (response) {
                console.log("API Response:", response); // Debugging
                if (response.includes("Goal set successfully")) {
                    toastr.success("Goal added successfully!");
                    $("#goalModal").modal("hide");
                    loadGoals();
                } else {
                    toastr.warning("Unexpected response: " + response);
                }
            },
            error: function (xhr) {
                console.error("Error adding goal:", xhr.responseText);
                toastr.error("Failed to add goal.");
            }
        });
    }


    // Function to update an existing goal
    function updateGoal(goalId, goalData) {
        console.log("Updating goal:", goalId, goalData);
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/UpdateGoal/" + goalId,
            type: "PUT",
            contentType: "application/json",
            dataType: "text",
            data: JSON.stringify(goalData),
            success: function () {
                toastr.success("Goal updated successfully!");
                $("#goalModal").modal("hide");
                loadGoals();
            },
            error: function (xhr) {
                console.error("Error updating goal:", xhr.responseText);
                toastr.error("Failed to update goal.");
            }
        });
    }

    // Function to load goals and display them
    function loadGoals() {
        var id = parseInt(window.localStorage.getItem("Id")); // Convert to number
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/GetGoals",
            type: "GET",
            success: function (goals) {
                console.log("Goals fetched:", goals); // Debugging
                $("#personalGoals, #managerGoals").empty();

                let personalGoals = goals.filter(g => g.isPersonalGoal && id == g.employeeId);
                let managerGoals = goals.filter(g => !g.isPersonalGoal);

                personalGoals.sort((a, b) => b.id - a.id);
                managerGoals.sort((a, b) => b.id - a.id);

                paginateGoals(personalGoals, "#personalGoals", "#personalGoalsPagination");
                paginateGoals(managerGoals, "#managerGoals", "#managerGoalsPagination");
            },
            error: function (xhr) {
                console.error("Error loading goals:", xhr.responseText);
                toastr.error("Failed to load goals.");
            }
        });
    }

    // Function for paginating goals
    function paginateGoals(goals, listId, paginationId, itemsPerPage = 3) {
        $(paginationId).empty();
        let totalPages = Math.ceil(goals.length / itemsPerPage);

        function renderPage(page) {
            $(listId).empty();
            let start = (page - 1) * itemsPerPage;
            let paginatedGoals = goals.slice(start, start + itemsPerPage);

            paginatedGoals.forEach(goal => {
                let deadline = new Date(goal.deadline).toISOString().split("T")[0];

                let goalItem = `<li class="list-group-item d-flex justify-content-between align-items-center">
                <span><strong>${goal.goalText}</strong> (Deadline: ${deadline}, Status: ${goal.goalStatus})</span>
                <button class="btn btn-sm btn-warning edit-goal"
                    data-id="${goal.id}"
                    data-text="${goal.goalText}"
                    data-deadline="${goal.deadline}"
                    data-status="${goal.goalStatus}"    
                    data-personal="${goal.isPersonalGoal}">
                    Edit
                </button>
            </li>`;
                $(listId).append(goalItem);
            });
        }

        for (let i = 1; i <= totalPages; i++) {
            $(paginationId).append(`<li class="page-item"><a class="page-link" href="#">${i}</a></li>`);
        }

        $(paginationId + " a").click(function () {
            renderPage(parseInt($(this).text()));
        });

        renderPage(1);
    }
});
