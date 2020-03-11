$(window).on('load',function() {
    galleryTest();
   // $(".footer-text-div").css("width",($(".wrapper").width()-328 ));
});

$( window ).resize(function() {
    galleryTest();
    //Todo move this
    //$(".footer-text-div").css("width",($(".wrapper").width()-328 ));
});

function galleryTest(){
    if( $( window ).width() <= 1000){
        $(".gallery-main").css({"display":"none"});
        $("#phone-gallery").css({"display":"block"});
    }
    else{
         $(".gallery-main").css({"display":"table"});
         $("#phone-gallery").css({"display":"none"});
    }
}

