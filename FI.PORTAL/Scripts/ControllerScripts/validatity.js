$(document).ready(function () {
    $('.select2').select2();

   
    $("input[data-bootstrap-switch]").each(function () {
        $(this).bootstrapSwitch('state', false);
    });
   
    
    $('#tblPainterRequest').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
    });

    $('#tblPainters').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": true,
    });

    $('#tblCategories').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
    });


  
    
});



function isMobNumber(evt) {
    evt = (evt) ? evt : window.event;
    var len = $("#txtreclimit").val().length;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) || len > 9) {
        return false;
    }
    return true;
}
function isConNumber(evt) {
    evt = (evt) ? evt : window.event;
    var len = $("#txtContactNumber").val().length;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) || len > 9) {
        return false;
    }
    return true;
}
function isPointNumber(evt) {
    evt = (evt) ? evt : window.event;
    var len = $("#txtPackPoints").val().length;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) || len > 2) {
        return false;
    }
    return true;
}

function ValidateEmail(inputText) {
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (inputText.value.match(mailformat)) {           
        return true;
    }
    else {
        alert("You have entered a invalid email address!");  
        return false;
    }
}