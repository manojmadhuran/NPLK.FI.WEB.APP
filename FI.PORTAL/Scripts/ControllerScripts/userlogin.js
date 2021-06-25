$(document).ready(function () {

    

});

$("#btnlogin").click(function () {
    var username = $("#txtusername").val();
    var password = $("#txtpassword").val();
    
    if (username != "" && password != "") {
        document.getElementById("lblLogstatus").innerHTML = "Validating user account ...";
        userloginValidation(username,password);
    }else{
         toastr.error("All fields are required.");
    }
 });

function userloginValidation(uname, pwd){
    $.ajax({
        type: "GET",
        data: {
            'uname': uname, 'password': pwd
        },
        url: '/Login/UserLoginValidation',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var role = result;
            if(role > 0){
                window.location.replace("/Home/Index");
            }else{
                toastr.error("Invalid username or password.");
                document.getElementById("lblLogstatus").innerHTML = "";
            }
        },
        error: function () {
            $("#modal-running").css("display", "none");
        }
    });
}