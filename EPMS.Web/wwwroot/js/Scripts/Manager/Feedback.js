$(document).ready(function () {
    var managerId = window.localStorage.getItem("Id");  
    var urlParams = new URLSearchParams(window.location.search);
    var employeeId = urlParams.get("employeeId");  

    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };

    if (!managerId || !employeeId) {
        toastr.warning("Missing Manager ID or Employee ID.");
        return;
    }

    $.ajax({
        url: `http://localhost:5207/api/Manager/getEmployeeGoals`,
        method: "GET",
        data: { managerId: managerId, employeeId: employeeId },
        dataType: "json",
        success: function (goals) {
            let taskDropdown = $("#taskSelect");
            taskDropdown.empty().append('<option value="">-- Select Task --</option>');

            if (!Array.isArray(goals) || goals.length === 0) {
                toastr.warning("No goals found for the selected employee.");
                return;
            }

            goals.forEach(goal => {
                taskDropdown.append(`<option value="${goal.id}">${goal.goalText}</option>`);
            });
        },
        error: function (err) {
            toastr.error("Error fetching goals: " + (err.responseText || "Unknown error."));
        }
    });

    $("#feedbackForm").on("submit", function (e) {
        e.preventDefault();

        let taskId = $("#taskSelect").val();
        let starRating = $("#starRating").val();
        let comments = $("#comments").val();

        if (!taskId) {
            toastr.warning("Please select a task before submitting feedback.");
            return;
        }

        if (!starRating || isNaN(parseInt(starRating))) {
            toastr.warning("Invalid star rating. Please provide a valid rating.");
            return;
        }

        let feedbackData = {
            EmployeeId: parseInt(employeeId),
            ReviewerId: parseInt(managerId),
            TaskId: taskId,
            TaskName_forFeedback: $("#taskSelect option:selected").text(),
            Star: parseInt(starRating),
            Comment: comments,
            FeedbackDate: new Date().toISOString()
        };

        $.ajax({
            url: "http://localhost:5207/api/Manager/submitFeedback",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(feedbackData),
            success: function () {
                toastr.success("Feedback submitted successfully!");
                $("#feedbackForm")[0].reset();
            },
            error: function (xhr) {
                toastr.error("Error submitting feedback: " + (xhr.responseText || "Unknown error."));
            }
        });
    });
});
