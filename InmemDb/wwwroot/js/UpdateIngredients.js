
$(function () {
    $("#successSpan").hide();
    $("#failSpan").hide();
    $("#updateButton").click(function () {

        $.ajax({
            type: "POST",
            url: "Cart/EditDishIngredients",
            data: $("#UpdateIngredients").serialize(),
            success: function (data) {
                $("#successSpan").show();
                $("#successSpan").fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);
            },
            error: function () {
                $("#failSpan").show();
                $("#failSpan").fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);
            }
        });
    });
});

