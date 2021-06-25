$(document).ready(function () {
    bsCustomFileInput.init();
    $('#tblordhistory').DataTable();

    //window.onload = pageload();

   

    //function pageload() {
    //    var reasonlist = document.getElementById("reasonlist");
    //    $.ajax({
    //        type: "GET",
    //        url: '/Home/reasonList',
    //        contentType: "application/json; charset=utf-8",
    //        success: function (result) {
    //            var size = result.length;
    //            var i = 0;
    //            while (i < size) {
    //                var val = result[i].split('|')[0];
    //                var txt = result[i].split('|')[1];

    //                var option = document.createElement("option");
    //                option.text = txt;
    //                option.value = val;
    //                reasonlist.add(option);
    //                i++;
    //            }
    //        }, error: function () {
               
    //        }
    //    });
    //}

    toastr.options = {
        "positionClass": "toast-top-full-width",
        "closeButton": true,
        "timeOut": "4000",
        "preventDuplicates": true
    }

    $("#txtcuscode").keyup(function (event) {
        if (event.keyCode === 13) {
            clearForm();
            var cuscode = $("#txtcuscode").val();
            $("#modal-running").css("display", "block");
            $.ajax({
                type: "GET",
                data: {
                    'cusCode': cuscode
                },
                url: '/Home/LoadCusDetails',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $("#txtcusname").val(result.Cusname);
                    $("#txtsalescode").val(result.SalesCode);
                    $("#txtsalesArea").val(result.SalesArea);
                    var limit = result.Crdlimit;
                    if(limit > 0){
                        limit = financial(limit);
                    }
                    $("#txtcrdlimit").val(limit);
                    $("#modal-running").css("display", "none");
                }, error: function () {
                    toastr.error("Invalid customer code.");
                    $("#modal-running").css("display", "none");
                }
            });
        }
    });



    $("#btninitiate").click(function () {
        var cuscode = $("#txtcuscode").val();
        var salecode = $("#txtsalescode").val();
        var allowlimit = $("#txtEntitlecrdlimit").val(); //entitled
        var existlimit = $("#txtcrdlimit").val();
        var comment = $("#txtcomment").val();
        var filename = $("#exampleInputFile").val();
        var Rtype = $("#drptype option:selected").val();
        var crdExpo = $("#txtcrdExposure").val();//Credit Exposure

        if (Rtype == "OR") {
            allowlimit = "0.00";
        }

       
        

        if (cuscode != "" && salecode != "" && allowlimit != "" && comment != "") {

            $("#modal-running").css("display", "block");
            var formData = new FormData($('#excel-upload-form')[0]);

            $.ajax({
                type: "GET",
                data: {
                    'cusCode': cuscode, 'filename': filename, 'salescode': salecode, 'allowlimit': allowlimit, 'comment': comment, 'Rtype': Rtype, 'crdExpo': crdExpo, 'existing': existlimit
                },
                url: '/Home/InitiateRequest',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var reqID = result;
                    if (reqID > 0) {
                        toastr.success("Credit enhancement request created.\n REF: " + reqID);
                    }
                    $("#lblreqid").val(reqID);
                    document.getElementById("divheader").style.visibility = "hidden";
                    var div = document.getElementById("divupload");
                    div.style.visibility = "visible";
                    div.style.marginTop = "-400px";
                   
                    $("#txtcuscode").val("");                    
                    clearForm();    
                    
                    $("#modal-running").css("display", "none");

                }, error: function () {
                    $("#modal-running").css("display", "none");
                }
            });

        } else {
            toastr.error("All fields are required.");
        }

    });

    //$("#btnsearch").click(function () {
    //    var cuscode = $("#txtcuscode").val();
    //    if(cuscode == ""){
    //        cuscode = 0;
    //    }
    //    var year = $("#year").val();
    //    var e = document.getElementById("month");
    //    var month = e.options[e.selectedIndex].value;

    //    var ee = document.getElementById("status");
    //    var status = e.options[ee.selectedIndex].value;
       
    //    if (year != "" && month > 0) {
    //        loadRequests(cuscode, year, month,status);
    //    } else {
    //        toastr.error("All fields mandotary.");
    //    }

    //});

    function loadRequests(cusCode, Year, Month, Status) {
        $("#modal-running").css("display", "block");

        $.ajax({
            type: "POST",
            data: {
                'cuscode': cusCode, 'year': Year, 'month': Month, 'status': Status
            },
            url: '/Home/ViewRequests_',
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                var size = result.length;

                $("#secposts").empty();

                result.forEach(element => {
                    var reqid = element.Reqid;
                    var cuscode = element.Cuscode;
                    var cusname = element.Cusname;
                    var limit = element.Crdlimit;
                    if (limit > 0) {
                        limit = financial(limit);
                    } else {
                        limit = financial(0);
                    }
                    var comment = element.Comments;
                    var statuslvl = element.Curstatus;
                    var statusID = element. StatusID;
                    var count_ = element.Count_;
                    var reqStatus = element.Reqstatus;
                    var date = element.Date;
                    var file = element.FileName;
                    var RType = element.RType;

                    if (RType == "LE") {
                        var dom =
                            "<div class='user-block'>\
                                <img class='img-circle img-bordered-sm' src='/Content/dist/img/mart.png' alt='user image'>\
                                        <span class='username'>\
                                        <a href='#'>"+ cusname + "</a>\
                                            <span class='float-right text-danger font-weight-bold'>\
                                                "+ statuslvl + " " + reqStatus + "\
                                                <br/><a href='/Content/uploaded/"+ file + "' class='float-right' target='_blank'><img src='/Content/dist/img/attach.png' /></a>\
                                            </span>\
                                        </span>\
                                        <span class='description'>Request created on "+ date + "\
                                            \
                                        </span>\
                                        <span class='description'>\
                                            <span class='float-left text-success font-weight-bolder'>\
                                                Rs: "+ limit + "\
                                            </span>\
                                        </span>\
                                        <br />\
                                        <p class='description'>\
                                            <a href='/Home/ViewComments?reqID="+ reqid + "&SID=" + statusID + "' id='btnview' class='btn btn-primary btn-sm' >\
                                                <i class='far fa-comments'></i>\
                                            Comments ("+ count_ + ")\
                                            </a>\
                                        </p>\
                                    <hr style='margin-bottom:5px;margin-top:-5px;' />\
                                    </div>";
                    } else if (RType == "OR") {
                        var dom =
                            "<div class='user-block'>\
                                <img class='img-circle img-bordered-sm' src='/Content/dist/img/mart.png' alt='user image'>\
                                        <span class='username'>\
                                        <a href='#'>"+ cusname + "</a>\
                                            <span class='float-right text-danger font-weight-bold'>\
                                                "+ statuslvl + " " + reqStatus + "\
                                                <br/><a href='/Content/uploaded/"+ file + "' class='float-right' target='_blank'><img src='/Content/dist/img/attach.png' /></a>\
                                            </span>\
                                        </span>\
                                        <span class='description'>Request created on "+ date + "\
                                            \
                                        </span>\
                                        <span class='description'>\
                                            <span class='float-left text-success font-weight-bolder'>\
                                                Order Releasing Request\
                                            </span>\
                                        </span>\
                                        <br />\
                                        <p class='description'>\
                                            <a href='/Home/ViewComments?reqID="+ reqid + "&SID=" + statusID + "' id='btnview' class='btn btn-primary btn-sm' >\
                                                <i class='far fa-comments'></i>\
                                            Comments ("+ count_ + ")\
                                            </a>\
                                        </p>\
                                    <hr style='margin-bottom:5px;margin-top:-5px;' />\
                                    </div>";
                    }
                    $("#secposts").append(dom);
                });
                $("#modal-running").css("display", "none");
            }, error: function () {
                    $("#modal-running").css("display", "none");
                }
        }); 
    }


    function isNum(event) {
        var key = event.keyCode;
        if (key >= 48 && key < 58) {
            return true;
        } else {
            return false;
        }
    }

    function clearForm(){
        
        $("#txtcusname").val("");
        $("#txtsalescode").val("");
        $("#txtcrdlimit").val("");
        $("#txtallowcrdlimit").val("");
        $("#txtcomment").val("");
        $("#txtsalesArea").val("");
    }
    //format number as currency
    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    function financial(x) {
        return numberWithCommas(Number.parseFloat(x).toFixed(2));
    }


    $("#txtallowcrdlimit").keypress(function(e){
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });


    $("#txtreclimit").keypress(function (e) {
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });

    $("#txtcuscode").keypress(function (e) {
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace

        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    });

    $("#drptype").change(function (e) {
        var val = $("#drptype option:selected").val();
        if (val == "OR") {
            $("#txtallowcrdlimit").attr('disabled', 'disabled');
        } else {
            $("#txtallowcrdlimit").removeAttr('disabled');
        }
    });

});