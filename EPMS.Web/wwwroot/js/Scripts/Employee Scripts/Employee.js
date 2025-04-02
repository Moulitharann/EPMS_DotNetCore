
    $(document).ready(function () {
      
        toastr.options = {
            "positionClass": "toast-top-right",
            "closeButton": true,
            "progressBar": true,
            "timeOut": "3000"
        };

    loadGoals();

    $("#openGoalModal").click(function () {
        $("#goalId").val("");
    $("#goalTypeText").val("");
    $("#goalDeadline").val("");
    $("#taskStatus").val("Not Started");
    $("#goalModal").data("isPersonalGoal", 1); 
    $("#goalModal").modal("show");
            });

    $("#saveGoal").click(function () {
                var employeeId = window.localStorage.getItem("Id");
    var goalData = {
        EmployeeId: employeeId,
    GoalText: $("#goalTypeText").val(),
    Deadline: $("#goalDeadline").val(),
    GoalStatus: $("#taskStatus").val(),
    IsPersonalGoal: $("#goalModal").data("isPersonalGoal")
                };

    var editId = $("#goalId").val();
    if (editId) {
        updateGoal(editId, goalData);
                } else {
        addGoal(goalData);
                }
            });

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

    function addGoal(goalData) {
        $.ajax({
            url: "http://localhost:5207/api/EmployeeDashboard/SetGoal",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(goalData),
            success: function () {
                toastr.success("Goal added successfully!");
                $("#goalModal").modal("hide");
                loadGoals();
            },
            error: function (xhr) {
                toastr.error("Failed to add goal: " + xhr.responseText);
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
                $("#goalModal").modal("hide");
                loadGoals();
            },
            error: function (xhr) {
                toastr.error("Failed to update goal: " + xhr.responseText);
            }
        });
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

    function paginateGoals(goals, listId, paginationId, itemsPerPage = 3) {
        $(paginationId).empty();
    let totalPages = Math.ceil(goals.length / itemsPerPage);

    function renderPage(page) {
        $(listId).empty();
    let start = (page - 1) * itemsPerPage;
    let paginatedGoals = goals.slice(start, start + itemsPerPage);

                    paginatedGoals.forEach(goal => {
        let deadline = new Date(goal.Deadline).toISOString().split("T")[0];

    let goalItem = `<li class="list-group-item">
        <span><strong>${goal.GoalText}</strong> (Deadline: ${deadline}, Status: ${goal.GoalStatus})</span>
        <button class="btn btn-sm btn-warning edit-goal" data-id="${goal.Id}" data-text="${goal.GoalText}" data-deadline="${goal.Deadline}" data-status="${goal.GoalStatus}" data-personal="${goal.IsPersonalGoal}">Edit</button>
    </li>`;
    $(listId).append(goalItem);
                    });
                }

    renderPage(1);
            }
        });
