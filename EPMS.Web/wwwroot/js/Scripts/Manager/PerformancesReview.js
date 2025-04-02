document.addEventListener("DOMContentLoaded", function () {
    let managerId = localStorage.getItem("Id");


   
    toastr.options = {
        "positionClass": "toast-top-right",
        "closeButton": true,
        "progressBar": true,
        "timeOut": "3000"
    };
    fetch(`http://localhost:5207/api/manager/employees/${managerId}`)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            let select = document.getElementById("employeeSelect");
            select.innerHTML = data.map(emp => `<option value="${emp.id}">${emp.name}</option>`).join("");
        });

    document.getElementById("conductEvaluation").addEventListener("click", function () {
        let employeeId = document.getElementById("employeeSelect").value;
        let feedback = {
            EmployeeId: employeeId,
            ManagerId: managerId,
            FeedbackText: "Great work!",
            Rating: 4,
            Date: new Date().toISOString()
        };
        console.log(feedback);

        fetch(`http://localhost:5207/api/manager/SubmitEvaluation`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(feedback)
        }).then(response => response.text())
            .then(result => {
                toastr.success(result)
                document.getElementById("generateReport").disabled = false;
            });
    });

    //get all Employee records
   
       



    document.getElementById("generateReport").addEventListener("click", function () {
        let employeeId = document.getElementById("employeeSelect").value;
        window.location.href = `http://localhost:5207/api/manager/GenerateReport/${employeeId}`;
    });
});