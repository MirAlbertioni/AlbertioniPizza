$(function () {

 
    $("#ShowCart").click(function () {

        $.ajax({
            type: "POST",
            url: "Cart/Index",
            
            success: function (data) {
                $('#Cart').html(data);

            },
            error: function () {
                
            }
        });
    });

});

                       