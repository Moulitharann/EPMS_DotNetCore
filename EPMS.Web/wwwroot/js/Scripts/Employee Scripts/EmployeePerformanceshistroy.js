$(document).ready(function () {
    var id = window.localStorage.getItem("Id");
    if (!id) {
        $("#performanceHistory").html('<div class="alert alert-warning text-center">No Employee ID found.</div>');
        toastr.warning("No Employee ID found.");
        return;
    }

    $.ajax({
        url: `http://localhost:5207/api/EmployeeDashboard/GetEmployeeEvaluations/${id}`, 
        type: "GET",
        success: function (data) {
            console.log(data)
            $("#performanceHistory").empty();
            if (!data || data.length === 0) {
                $("#performanceHistory").html('<div class="alert alert-info text-center">No evaluations found.</div>');
            } else {
                data.forEach(function (item) {
                    $("#performanceHistory").append(
                        `<div class="card mb-3">
                                        <div class="card-body">
                                            <h5 class="card-title"><i class="icon fas fa-user-tie"></i> Manager: ${item.managerName}</h5>
                                            <p class="card-text"><i class="icon fas fa-comments"></i> Comments: ${item.comments}</p>
                                            <p class="card-text"><i class="icon fas fa-star"></i> Score: <strong>${item.score}</strong></p>
                                        </div>
                                    </div>`
                    );
                });
            }
        },
        error: function () {
            $("#performanceHistory").html('<div class="alert alert-danger text-center">Failed to load data.</div>');
        }
    });
});