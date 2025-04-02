$(document).ready(function () {
    $("#viewEmployees").click(function () {
        window.location.href = "/HRAdmin/employees";
    });
    $("#viewAssign").click(function () {
        window.location.href = "/HRAdmin/AssignManager";
    })
    $("#defineCriteria").click(function () {
        window.location.href = "/HRAdmin/EvaluationCriteria";
    });

    $("#generateReports").click(function () {
        window.location.href = "/HRAdmin/PerformanceReports";
    });
});

toastr.options = {
    "positionClass": "toast-top-right",
    "closeButton": true,
    "progressBar": true,
    "timeOut": "3000"
};