$(document).ready(function () {
    $('.input-uppercase').on('input', function (evt) {
        $(this).val(function (_, val) {
            return val.toUpperCase();
        });
    });
});