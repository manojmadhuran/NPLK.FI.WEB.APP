$(document).ready(function () {
   
    window.onload = loadcomments();
    var tblcomment = $("#tblcomments").DataTable();
   

    function loadcomments() {
        

        const params = new URLSearchParams(window.location.search);
        var requestID = params.get("reqID");
        
        var Lvllist = document.getElementById("revertTo");

        $.ajax({
            type: "GET",
            url: '/Home/LevelList',
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                var size = result.length;
                var i = 0;
                while (i < size) {
                    var val = result[i].split('|')[0];
                    var txt = result[i].split('|')[1];

                    var option = document.createElement("option");
                    option.text = txt;
                    option.value = val;
                    Lvllist.add(option);
                    i++;
                }
            }, error: function () {
               
            }
        });


        $.ajax({
            type: "GET",
            data: {
                'reqid': requestID
            },
            url: '/Home/LoadHistory',
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result != null) {
                    if (result.length > 0) {
                        document.getElementById("lblcusName").innerHTML = result[0];
                        document.getElementById("lblarea").innerHTML = result[2];
                        document.getElementById("lblextlimit").innerHTML = ((result[1] != "" || result[1] != null) ? financial(result[1]) : "0.00");
                        document.getElementById("lblexposure").innerHTML = ((result[3] != "" || result[3] != null) ? financial(result[3]) : "0.00");
                        var action = result[4];
                        document.getElementById("lblcuscode").innerHTML = result[5];

                        var actionlbl = document.getElementById("lblstatus");
                        if (action == "APPROVE") {
                            //actionlbl.style.color = "#ffc93c";
                            $("#btnforward").hide();
                            $("#btnrevert").hide();
                            document.getElementById("lblcommHeader").innerHTML = "SAP Updated Credit Limit (Rs)";
                        } else if (action == "HOLD") {
                            actionlbl.style.color = "#ffc93c";
                        } else if (action == "COMPLETED") {
                            actionlbl.style.color = "#799351";
                        } else if (action == "NOT-REQUIRED") {
                            actionlbl.style.color = "#440047";
                        } else if (action == "FORWARD") {
                            actionlbl.style.color = "#0f4c75";
                        } else if (action == "REVERSE") {
                            $("#btnapprove").hide();
                            actionlbl.style.color = "#d54062";
                        }
                        actionlbl.innerHTML = action;
                    }
                }
            },
            error: function () {

            }
        });




        //$("#modal-running").css("display", "block");
        //$.ajax({
        //    type: "GET",  
        //    data: {
        //        'reqID': requestID
        //    },          
        //    url: '/Home/viewComments_',            
        //    contentType: "application/json; charset=utf-8",
        //    success: function (result) {
        //        var size = result.length;
        //        $("#commentlist").empty();

        //        result.forEach(element => {
        //            var role = element.Role;
        //            var user = element.UserName;
        //            var comment = element.Comment;
        //            var createon = element.CreatedDate;
        //            var limit = element.Limit;
        //            var uaction = element.Action;
        //            var rtype = element.RType;

        //            if(limit > 0){
        //                limit = financial(limit);
        //            }

        //            if (rtype == "LE") {

        //                var dom = "<tr>\
        //                            <td>"+ createon + "</td>\
        //                            <td>"+ role +"</td>\
        //                            <td>"+ user + "</td>\
        //                            <td>"+ comment +"</td>\
        //                            <td>"+ uaction + "</td>\
        //                            <td>"+ limit + "</td>\
        //                            <td> <img src='../Content/dist/img/image.png'  style='width: 30px; height: 30px;cursor: pointer;' /> </td>\
        //                        </tr>";

        //            } else if (rtype == "OR") {
                                               
        //            }
            
        //            $("#commentlist").append(dom);
        //        });
        //        $("#modal-running").css("display", "none");
        //    }, error: function () {
        //        $("#modal-running").css("display", "none");
        //    }
        //});




        //////////// load comments /////////////
        var table = $('#tblcomments');
        table.DataTable({
            "serverSide": "true",
            "autoWidth": true,
            "processing": "true",
            "paging": false,
            "searching": false,
            "language": {
                "processing": "Processing Data..."
            },
            "ajax": {
                "type": "POST",
                "url": "/Home/viewComments_",
                "datatype": "json",
                "data": function (d) {
                    d.reqID = requestID
                }
            },
            "columns": [
                {
                    "data": "CreatedDate", "autoWidth": true, "orderable": false
                }
                ,
                {

                    "data": "Role", "autoWidth": true, "orderable": false
                },
                {

                    "data": "UserName", "autoWidth": true, "orderable": false
                },
                {

                    "data": "Comment", "autoWidth": true, "orderable": false
                },
                {

                    "data": "Action", "autoWidth": true, "orderable": false
                },
                {

                    "data": "Limit", "autoWidth": true, "orderable": false, "render": $.fn.dataTable.render.number(',', '.', 2)

                },
                {
                    "targets": -1,
                    "data": null,
                    "orderable": false,
                    "defaultContent": "<img src='../Content/dist/img/image.png' style='width: 30px; height: 30px; cursor: pointer;' />"
                }
            ],
        });


    }

  

    $('#tblcomments tbody').on('click', 'img', function (x) {
        var data = $('#tblcomments').DataTable().row($(this).parents('tr')).data(); 
        var file = data.CommID;
        newwindow = window.open("../../ImageView/images.aspx?comment=" + file + "", "windowName", 'height=500,width=900');
        if (window.focus) { newwindow.focus() }
        return false;

    });

    $("#btnforward").click(function () {

      

        const params = new URLSearchParams(window.location.search);
        var requestID = params.get("reqID");
        
        var crdlimit = $("#txtreclimit").val();
        var comment = $("#txtcomment").val();
        var Uaction = 'FORWARD';

        if (crdlimit != "" && comment != "") {

            $("#modal-running").css("display", "block");
            $.ajax({
                type: "GET",
                data: {
                    'ReqID': requestID, 'allowlimit': crdlimit, 'comment': comment, 'Uaction': Uaction, 'Level': 0
                },
                url: '/Home/AddRequestComment',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result > 0) {
                        var id = result;
                        //clearfields();
                        //loadcomments();

                        toastr.success("Credit enhancement request FORWARD.\n REF: "+id);
                        window.location.replace('/Home/ViewRequests');
                    }

                    $("#modal-running").css("display", "none");
                }, error: function () {
                    $("#modal-running").css("display", "none");
                }
            });
        } else {
            toastr.error("All fields are required.");
        }
    });

    $("#btnmodalRevert").click(function(){
        const params = new URLSearchParams(window.location.search);
        var requestID = params.get("reqID");

        var e = document.getElementById("revertTo");
        var revertTo = e.options[e.selectedIndex].value;
        var comment = $("#txtrevertcomment").val();
        var Uaction = 'REVERSE';

        if (comment != "") {

            $("#modal-running").css("display", "block");
            $.ajax({
                type: "GET",
                data: {
                    'ReqID': requestID, 'allowlimit': 0, 'comment': comment, 'Uaction': Uaction, 'Level':revertTo
                },
                url: '/Home/AddRequestComment',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result > 0) {
                        var id = result;
                        //clearfields();
                        //loadcomments();
                       
                        toastr.success("Credit enhancement request REVERT.\n REF: "+id);
                        window.location.replace('/Home/ViewRequests');
                    }

                    $("#modal-running").css("display", "none");
                }, error: function () {
                    $("#modal-running").css("display", "none");
                }
            });
        } else {
            toastr.error("All fields are required.");
        }
    });

    $("#btnapprove").click(function(){
        const params = new URLSearchParams(window.location.search);
        var requestID = params.get("reqID");

        var crdlimit = $("#txtreclimit").val();
        var comment = $("#txtcomment").val();
        var Uaction = 'APPROVE';

        if (crdlimit != "" && comment != "") {

            $("#modal-running").css("display", "block");
            $.ajax({
                type: "GET",
                data: {
                    'ReqID': requestID, 'allowlimit': crdlimit, 'comment': comment, 'Uaction': Uaction, 'Level':0
                },
                url: '/Home/AddRequestComment',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result > 0) {
                        var id = result;
                        //clearfields();
                        //loadcomments();

                        toastr.success("Credit enhancement request APPROVED.\n REF: "+id);
                        window.location.replace('/Home/ViewRequests');
                    }

                    $("#modal-running").css("display", "none");
                }, error: function () {
                    $("#modal-running").css("display", "none");
                }
            });
        } else {
            toastr.error("All fields are required.");
        }
    });


    $("#btndone").click(function () {
               
        const params = new URLSearchParams(window.location.search);
        var requestID = params.get("reqID");
        var crdlimit = $("#txtreclimit").val();
        var comment = $("#txtcomment").val();
        var selectedOption = $("input:radio[name=radioAction]:checked").val();
        if (crdlimit != "" && comment != "") {
            $("#modal-running").css("display", "block");

            $.ajax({
                type: "GET",
                data: {
                    'ReqID': requestID, 'allowlimit': crdlimit, 'comment': comment, 'Uaction': selectedOption, 'Level': 0
                },
                url: '/Home/AddRequestComment',
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result > 0) {
                        var id = result;

                        $("#modal-confirm").css("display", "block");
                        document.getElementById("lblmodalreqid").innerHTML = "Credit enhancement process COMPLETED.\n REF: " + id;
                        $("#btnclose").click(function () {
                            //window.close();
                            location.replace("/Home/ViewRequests", true);
                        });
                    }

                    $("#modal-running").css("display", "none");

                }, error: function () {
                    $("#modal-running").css("display", "none");
                }
            });


        } else {
            toastr.error("All fields are required.");
        }
    });

    function clearfields(){
        $("#txtreclimit").val("");
        $("#txtcomment").val("");
    }

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    function financial(x) {
        return numberWithCommas(Number.parseFloat(x).toFixed(2));
    }
});