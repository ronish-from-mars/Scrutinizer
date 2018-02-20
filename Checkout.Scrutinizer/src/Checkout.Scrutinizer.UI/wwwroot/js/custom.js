

$(document).on("click", "#show-errors-only", function () {
    var isChecked = $(this).is(':checked');
    if (isChecked) {
        $('.no-error').hide();
        $('.has-errors').show();
    } else {
        $('.no-error').show();
        $('.has-errors').show();
    }
});

