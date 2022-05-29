$(document).ready(function () {

    // button create Customer on click function
    $("#btnCrtCustomer").click(function () {
        var cuscode = $("#txtCustomerCode").val();
        var cusName = $("#txtcusname").val();
        var crdlimit = $("#txtcrdLimit").val();
        var crdPeriod = $("#txtcrdPeriod").val();


        if (cuscode != "" && cusName != "" && crdlimit != "" && crdPeriod != "") {

        //    $("#modal-running").css("display", "block");
        //    var formData = new FormData($('#excel-upload-form')[0]);

            $.ajax({
                type: "GET",
                data: {
                    'cusCode': cuscode, 'cusName': cusName, 'crdlimit': crdlimit, 'crdPeriod': crdPeriod
                },
                url: '/Home/CreateCustomer',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var reqID = result;
                    if (reqID > 0) {
                        toastr.success("Customer Added Successfully.\n Customer Code: " + reqID);
                        clearForm();
                    } else {
                        toastr.error("Something went Wrong .. Error Creating Customer");
                    }
                                    
                }, error: function () {
                
                }
            });

        } else {
            toastr.error("All fields are required.");
        }

    });

    function clearForm() {

        $("#txtCustomerCode").val("");
        $("#txtcusname").val("");
        $("#txtcrdLimit").val("");
        $("#txtcrdPeriod").val("");
      
    }


});