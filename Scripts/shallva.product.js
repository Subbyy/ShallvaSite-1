var prodRowClicks = 0;
var bodyClicks = 0;

$(document).ready(function () {
    $('#prod-table input').each(function () {
        if ($(this).val() == '0') {
            $(this).val('');
        }
    });

    $('#prod-table input').change(function () {
        if ($(this).val() == '0') {
            $(this).val('');
        }
    });

    $('#prod-table tbody > tr').click(function () {
        $('#prod-table tbody > tr').removeClass('slct-prod');
        $(this).addClass('slct-prod');
        prodRowClicks++;
    });

    $('html').click(function () {
        bodyClicks++;

        if (bodyClicks > prodRowClicks) {
            bodyClicks = prodRowClicks = 0;
            $('#prod-table tbody > tr.slct-prod').removeClass('slct-prod');
        }
    });

    $('#addToCart').click(function () {
        $('#prod-table').submit();
    });

});

function afterAddToCart(data) {

}