
$('.carousel.carousel-slider').carousel({ fullWidth: true });
$(".button-collapse").sideNav({
    menuWidth: 350
});
$('.modal').modal();




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


