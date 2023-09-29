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
    $('#startDate').datetimepicker({
        uiLibrary: 'bootstrap5',
        iconsLibrary: 'fontawesome',
        footer: true, modal: true
    });

    $('#endDate').datetimepicker({
        uiLibrary: 'bootstrap5',
        iconsLibrary: 'fontawesome',
        footer: true, modal: true
    });



    $("#filter").submit(function (e)
    {
        e.preventDefault();

        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        $("tbody tr").each(function ()
        {
            var pubDate = $(this).find("td:eq(1)").text(); 
            if (isDateInRange(pubDate, startDate, endDate))
            {
                $(this).show();
            }
            else
            {
                $(this).hide();
            }
        });
    });


    $('#search').on('submit', function (e)
    {
        e.preventDefault();

        var searchTerm = $('#title').val() || $('#name').val();
        if (searchTerm == undefined) return;

        searchTerm = searchTerm.toLowerCase();

        $('tbody tr').each(function ()
        {
            var title = $(this).find('td:first').text().toLowerCase();
            if (title.includes(searchTerm))
            {
                $(this).show();
            }
            else
            {
                $(this).hide();
            }
        });
    });


});

function isDateInRange(dateString, startDate, endDate)
{
    var pubDate = new Date(dateString);
    var start = new Date(startDate);
    var end = new Date(endDate);

    return pubDate >= start && pubDate <= end;
}