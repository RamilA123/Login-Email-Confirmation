$(document).ready(function () {
    $('#checkeye').click(function () {
        var previousType = $(this).prev().attr("type");

        if (previousType == "password") {
            $(this).prev().attr("type", "text");
        } else {
            $(this).prev().attr("type", "password");
        }
    });
});

