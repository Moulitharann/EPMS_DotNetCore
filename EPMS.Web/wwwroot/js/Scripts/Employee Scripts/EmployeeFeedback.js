

$(document).ready(function () {
    loadGoals();

    toastr.options = {
        closeButton: true,
        progressBar: true,
        positionClass: "toast-top-right",
        timeOut: "3000"
    };
    
    $("#addGoal").click(function () {
        var taskStatusValue = $("#taskStatus").val();
        var employeeId = window.localStorage.getItem("Id");
        var isPersonal = $(this).data("personal") ?? 1;

        var goalData = {
            EmployeeId: employeeId,
            GoalText: $("#goalType").val(),
            Deadline: $("#goalDeadline").val(),
            GoalStatus: taskStatusValue,
            IsPersonalGoal: isPersonal
        };

        var editId = $(this).data("edit-id");
        if (editId) {
            updateGoal(editId, goalData);
        } else {
            addGoal(goalData);
        }
    });

    $(document).on("click", ".complete-goal", function () {
        var goalId = $(this).data("id");
        var goalItem = $(this).closest("li");

        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/UpdateGoal/" + goalId,
            type: "PUT",
            contentType: "application/json",
            data: JSON.stringify({ GoalStatus: "Completed" }),
            success: function () {
                goalItem.fadeOut(300, function () { $(this).remove(); });
            },
            error: function () {
                toastr.error("Error completing goal.");
            }
        });
    });

    $(document).on("click", ".edit-goal", function () {
        var goalId = $(this).data("id");
        var goalText = $(this).data("text");
        var goalDeadline = $(this).data("deadline");
        var taskStatus = $(this).data("status");
        var isPersonalGoal = $(this).data("personal");

        $("#goalType").val(goalText);
        $("#goalDeadline").val(goalDeadline);
        $("#taskStatus").val(taskStatus);
        $("#addGoal").text("Update Goal").data("edit-id", goalId).data("personal", isPersonalGoal);
    });

    function addGoal(goalData) {
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/SetGoal",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(goalData),
            success: function () {
                toastr.success("Goal added successfully!");
                resetForm();
                loadGoals();
            }
        });
    }

    function updateGoal(goalId, goalData) {
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/UpdateGoal/" + goalId,
            type: "PUT",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(goalData),
            success: function () {
                toastr.success("Goal updated successfully!");
                resetForm();
                loadGoals();
            }
        });
    }

    function resetForm() {
        $("#goalDeadline").val("");
        $("#taskStatus").val("");
        $("#addGoal").text("Add Goal").removeData("edit-id").removeData("personal");
    }

    function loadGoals() {
        var id = window.localStorage.getItem("Id");
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/GetGoals",
            type: "GET",
            success: function (goals) {
                $("#personalGoals, #managerGoals").empty();
                let personalGoals = goals.filter(g => g.IsPersonalGoal && id == g.EmployeeId);
                let managerGoals = goals.filter(g => !g.IsPersonalGoal);

                paginateGoals(personalGoals, "#personalGoals", "#personalGoalsPagination");
                paginateGoals(managerGoals, "#managerGoals", "#managerGoalsPagination");
            }
        });
    }
    loadManager();

    function loadManager() {
        var employeeId = window.localStorage.getItem("Id");

        $.ajax({
            url: `http://localhost:5207/api/EmployeeDashboard/GetManager/${employeeId}`,
            type: "GET",
            success: function (response) {
                console.log(response)
                $("#feedbackType").empty();
                if (response && response.managerName) {
                    $("#feedbackType").append(`<option value="${response.managerName}">${response.managerName}</option>`);
                    localStorage.setItem("ManagerId", response.managerId || ""); 
                } else {
                    $("#feedbackType").append(`<option value="">No Manager Found</option>`);
                    localStorage.setItem("ManagerId", ""); 
                }
            },
            error: function (xhr) {
                $("#feedbackType").empty();
                $("#feedbackType").append(`<option value="">Error fetching manager</option>`);
                console.error("❌ Error loading manager:", xhr.responseText);
            }
        });
    }
    function paginateGoals(goals, listId, paginationId, itemsPerPage = 2) {
        $(paginationId).empty();
        let totalPages = Math.ceil(goals.length / itemsPerPage);

        function renderPage(page) {
            $(listId).empty();
            let start = (page - 1) * itemsPerPage;
            let paginatedGoals = goals.slice(start, start + itemsPerPage);

            paginatedGoals.forEach(goal => {
                if (goal.GoalStatus === "Completed") return;

                let goalItem = `<li class="list-group-item d-flex justify-content-between align-items-center">
                                     <span><strong>${goal.GoalText}</strong> (Deadline: ${goal.Deadline}, Status: ${goal.GoalStatus})</span>
                                     <div>
                                         <button class="btn btn-sm btn-warning edit-goal"
                                             data-id="${goal.Id}"
                                             data-text="${goal.GoalText}"
                                             data-deadline="${goal.Deadline}"
                                             data-status="${goal.GoalStatus}"
                                             data-personal="${goal.IsPersonalGoal}">
                                             Edit
                                         </button>
                                              </div>
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

    $("#requestFeedback").click(function () {
        let employeeId = localStorage.getItem("Id");
        let managerId = sessionStorage.getItem("managerId");
        let message = $("#feedbackRequest").val();
           
        if (!employeeId || !managerId || !message) {
            toastr.error("All fields are required.");
            return;
        }

        let feedbackData = {
            Employee: parseInt(employeeId),
            ManagerId: parseInt(managerId), 
            Message: message
        };

        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/SubmitFeedback",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(feedbackData),
            success: function (response) {
                toastr.success("Feedback submitted successfully!");
                $("#feedbackRequest").val(""); // Clear input field
            },
            error: function (xhr) {
                toastr.error("Error submitting feedback: " + xhr.responseText);
            }
        });
    });

});