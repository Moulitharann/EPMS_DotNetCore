$(document).ready(function () {
    // Show category dropdown only when role is "Employee"
    $("#role").change(function () {
        if ($(this).val() === "Employee") {
            $("#categoryContainer").show();
        } else {
            $("#categoryContainer").hide();
            $("#category").val(""); // Clear category if not Employee
        }
    });

    $("#registerForm").submit(function (event) {
        event.preventDefault();

        // Show loading spinner and hide the form
        $("#loadingSpinner").removeClass("d-none");
        $("#registerForm").addClass("d-none");

        // Retrieve input values safely
        var roleValue = $("#role").val() || "";
        var categoryValue = (roleValue === "Employee") ? ($("#category").val() || "") : "";

        var registerData = {
            Name: $("#name").val().trim(),
            Email: $("#email").val().trim(),
            Password: $("#password").val(),
            Role: roleValue,
            Category: categoryValue
        };

        console.log("Sending Data:", registerData);

        $.ajax({
            url: "http://localhost:5207/api/auth/register", // Ensure correct API URL
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(registerData),
            xhrFields: {
                withCredentials: true
            },
            success: function (response) {
                console.log("Response:", response);
                $("#registerMessage")
                    .removeClass("d-none alert-danger")
                    .addClass("alert-success")
                    .text("Registration successful! Redirecting to login...");

                setTimeout(() => window.location.href = "/Auth/Login", 2000);
            },
            error: function (xhr) {
                console.log("Error Response:", xhr.responseText);

                $("#registerMessage")
                    .removeClass("d-none alert-success")
                    .addClass("alert-danger")
                    .text("Registration failed. " + (xhr.responseText || "Please try again."));

                // Hide loading spinner and show form again
                $("#loadingSpinner").addClass("d-none");
                $("#registerForm").removeClass("d-none");
            }
        });
    });
});
