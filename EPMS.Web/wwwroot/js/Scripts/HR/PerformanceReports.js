$(document).ready(function () {
    let managerId = localStorage.getItem("Id");
    let defaultCategory = "Finance"; 

    loadPerformanceReports(defaultCategory);

    $.get("http://localhost:5207/api/assignmanager/categories", function (data) {
        let options = data.map(cat => `<option value="${cat}">${cat}</option>`).join("");
        $("#categorySelect").html(options);
        $("#categorySelect").val(defaultCategory);
    });

    $("#categorySelect").change(function () {
        loadPerformanceReports($(this).val());
    });

    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };

    $("#conductEvaluation").click(function () {
        let category = $("#categorySelect").val();
        let feedbackText = `Excellent work in ${category}`;

        console.log(category);

        $.ajax({
            url: "http://localhost:5207/api/assignmanager/SubmitEvaluation",
            type: "POST",
            contentType: "application/json", 
            data: JSON.stringify({
                managerId: managerId,
                category: category,
                feedbackText: feedbackText
            }),
            success: function (response) {
                toastr.success(response);
                loadPerformanceReports(category);
              
            },
            error: function (xhr) {
                toastr.error("Error submitting evaluation: " + xhr.responseText);
            }
        });
    });


    $("#generateReport").click(function () {
        let category = $("#categorySelect").val();
        window.location.href = `http://localhost:5207/api/assignmanager/DownloadPerformanceReportPdf/${category}`;
        toastr.success("Report Downloaded Successfully");
    });

    $("#generatepdfReport").click(function () {
        let category = $("#categorySelect").val();
        try {
            window.location.href = `http://localhost:5207/api/assignmanager/DownloadPerformanceReportExcel/${category}`;
            toastr.success("Report Downloaded Successfully");
        } catch (er) {
            console.log(er);
        }
    });

    function loadPerformanceReports(category = "") {
        console.log("Loading reports for category:", category);

        $.ajax({
            url: `http://localhost:5207/api/assignmanager/Performances?category=${category}`,
            type: "GET",
            success: function (data) {
                console.log("Data received:", data);

                let tableBody = $("#performanceTableBody");
                tableBody.empty();

                
                let filteredData = category
                    ? data.filter(report => report.category?.toLowerCase() === category.toLowerCase())
                    : data;

                if (filteredData.length === 0) {
                    tableBody.append(`<tr><td colspan="4" class="text-center text-danger">No reports found.</td></tr>`);
                    return;
                }

                filteredData.forEach(report => {
                    tableBody.append(`
                <tr>
                    <td>${report.userName}</td>  
                    <td>${report.category}</td>  
                    <td>${report.star}</td>  
                    <td>${report.score}</td>
                </tr>
                `);
                });
            },
            error: function (xhr) {
                console.error("Error fetching performance reports:", xhr.responseText);
                toastr.error("Error loading performance reports");
            }
        });
    }


    $("#downloadExcel").click(() => window.location.href = "http://localhost:5207/api/assignmanager/DownloadEmployeeData/excel");

    $("#downloadPdf").click(() => window.location.href = "http://localhost:5207/api/assignmanager/DownloadEmployeeData/pdf");
});
