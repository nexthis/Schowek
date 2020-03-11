
var one = false;
$(window).on('load',function() {
    //if($("#progresEvent").css('opacity') != 0)
    loadProgress();
    one = true;
});



var scroll = $(window).scroll(function(){

    if($("#progresEvent").css('opacity') != 0 && one ==false ){ //$("#progresEvent").css('opacity') != 0 && one ==false 
        one = true
        loadProgress();
    }
    if($("#progresEvent").css('opacity') == 0 ){ //$("#progresEvent").css('opacity') == 0 
        one = false;
        $('#progress1').css("width",0);
        $('#progress2').css("width",0);
        $('#progress3').css("width",0);
        $('#progress4').css("width",0);
        $('#progress5').css("width",0);
        $('#progress6').css("width",0);
        $('#progress7').css("width",0);
    }


//if($("#progresEvent").is(":visible") == true) alert("asdasd");
//else alert(isHidden(document.getElementById("progresEvent") ));

});



function loadProgress(){
    $('#progress1').animate({width:70+"%"},3000);
    $('#progress2').animate({width:35+"%"},1470);
    $('#progress3').animate({width:60+"%"},2700);
    $('#progress4').animate({width:50+"%"},2100);
    $('#progress5').animate({width:40+"%"},1680);
    $('#progress6').animate({width:40+"%"},1680);
    $('#progress7').animate({width:55+"%"},2310);

    var t = 0;
    var inter = setInterval(function () {
        if(t<=70)
        $('#progress1').text(t+"%");
        if(t<=35)
        $('#progress2').text(t+"%");
        if(t<=60)
        $('#progress3').text(t+"%");
        if(t<=50)
        $('#progress4').text(t+"%");
        if(t<=40)
        $('#progress5').text(t+"%");
        if(t<=40)
        $('#progress6').text(t+"%");
        if(t<=55)
        $('#progress7').text(t+"%");

        if(t == 70) clearInterval(inter);
        t += 1;
    },42);
    t = 0;
}

