$(function () {


    $("#ShowCart").click(function () {

        $.ajax({
            type: "POST",
            url: "ShoppingCart/Cart",

            success: function (data) {
                $('#CartPartial').html(data);

            },
            error: function () {

            }
        });
    });

});