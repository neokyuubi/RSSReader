// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//function deleteFeed(button) {
//    event.preventDefault();
//    let parentForm = button.closest('form');
//    parentForm.submit();
//}


$(document).ready(function () {
    $("#selectAll").click(function () {
        $("input[type=checkbox]").prop("checked", $("#selectAll").is(":checked"));
    });


    let today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
    $('#startDate').datepicker({
        uiLibrary: 'bootstrap5',
        iconsLibrary: 'fontawesome',
    });

    $('#endDate').datepicker({
        uiLibrary: 'bootstrap5',
        iconsLibrary: 'fontawesome',
    
    });

});