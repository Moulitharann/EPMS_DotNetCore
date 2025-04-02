$(document).ready(function () {
    $("#loginForm").submit(function (event) {
        event.preventDefault();

        $("#loadingSpinner").removeClass("d-none");
        $("#loginForm").addClass("d-none");

        var loginData = {
            Email: $("#email").val(),
            Password: $("#password").val()
        };

        $.ajax({
            url: "http://localhost:5207/api/auth/login",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(loginData),
            xhrFields: {
                withCredentials: true
            },
            success: function (response) {
                console.log("Response received:", response);

                if (response && response.user) {
                    localStorage.setItem("Id", response.user.id);
                    localStorage.setItem("userRole", response.user.role);
                    localStorage.setItem("userName", response.user.name);
                    localStorage.setItem("userEmail", response.user.email);
                    localStorage.setItem("ManagerId", response.user.managerId);

                    sessionStorage.setItem("managerId", response.user.managerId);
                    $("#loginMessage").removeClass("d-none alert-danger").addClass("alert-success")
                        .text("Login successful! Redirecting...");

                    setTimeout(() => {
                        let rolePath = {
                            "HRAdmin": "/HRAdmin/Dashboard",
                            "Manager": "/Manager/Dashboard",
                            "Employee": "/Employee/Dashboard"
                        };
                        window.location.href = rolePath[response.user.role] || "/Home/Dashboard";
                    }, 1500);
                } else {
                    $("#loginMessage").removeClass("d-none alert-success").addClass("alert-danger")
                        .text("Login failed. Invalid response from server.");
                }

                $("#loadingSpinner").addClass("d-none");
                $("#loginForm").removeClass("d-none");
            },

            error: function (xhr) {
                console.log("Error:", xhr.responseText);
                $("#loginMessage").removeClass("d-none alert-success").addClass("alert-danger")
                    .text("Invalid email or password.");
                $("#loadingSpinner").addClass("d-none");
                $("#loginForm").removeClass("d-none");
            }
        });

    });
});