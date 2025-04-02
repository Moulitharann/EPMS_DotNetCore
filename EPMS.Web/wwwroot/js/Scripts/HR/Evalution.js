function addCriteria() {
    var data = {
        CriteriaName: $("#criteriaName").val(),
        Weightage: parseInt($("#criteriaWeight").val()), 
        EmployeeId: parseInt($("#employeeId").val())  
    };

    $.ajax({
        url: "http://localhost:5207/api/assignmanager/DefineCriteria",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function () {  
            toastr.success("Criteria Added Successfully");
            fetchCriteria();
        },
        error: function (xhr) {
            if (xhr.status === 404) {
                toastr.error("Employee Not Found");
            } else {
                toastr.error("Error adding criteria: " + (xhr.responseText || "Unknown error"));
            }
        }
    });
}

toastr.options = {
    "positionClass": "toast-top-right",
    "closeButton": true,
    "progressBar": true,
    "timeOut": "3000"
};
let currentPage = 1;
const pageSize = 5; 

function fetchCriteria(pageNumber = 1) {
    $.ajax({
        url: `http://localhost:5207/api/assignmanager/GetCriteria?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        type: "GET",
        success: function (response) {
            console.log(response)
            if (response.items.length > 0) {
                $("#criteriaTableContainer").show(); 
                populateTable(response.items);
                updatePaginationControls(response.totalRecords, pageNumber);
            } else {
                $("#criteriaTableContainer").hide(); 
            }
        },
        error: function () {
            toastr.error("Error fetching criteria.");
        }
    });
}

function populateTable(data) {
    let tableBody = $("#criteriaBody");
    tableBody.empty(); 

    data.forEach(item => {
        tableBody.append(`
            <tr>
                <td>${item.criteriaName}</td>
                <td>${item.weightage}</td>
                <td>${item.employeeName}</td>
                <td>${item.managerName}</td>
                <td>${item.score}</td>
            </tr>
        `);
    });
}

function updatePaginationControls(totalRecords, pageNumber) {
    let totalPages = Math.ceil(totalRecords / pageSize);
    $("#pageInfo").text(`Page ${pageNumber} of ${totalPages}`);

    $("#prevPage").prop("disabled", pageNumber === 1);
    $("#nextPage").prop("disabled", pageNumber === totalPages || totalPages === 0);

    $("#prevPage").off("click").on("click", function () {
        if (pageNumber > 1) fetchCriteria(--currentPage);
    });

    $("#nextPage").off("click").on("click", function () {
        if (pageNumber < totalPages) fetchCriteria(++currentPage);
    });
}



$(document).ready(function () {
    fetchCriteria();
});