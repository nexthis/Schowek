$( document ).ready(function() {
    timeOfPublication ();
});

function timeOfPublication ()
{
    var i = 1;
    while ($("#data"+i.toString()).length) {
    var dataFist=  $("#data"+i.toString()).attr("data");
    var dataLeft = new Date(dataFist);
    var data = new Date;

    var score=Math.ceil((data - dataLeft) / 8.64e7);

    if(score == 1){ $("#data"+i.toString()).text(score+" dzień temu");}
    else
    $("#data"+i.toString()).text(score+" dni temu");

    if(score >= 30 ){ 
        var month = score / 30;
        month = Math.round(month);
        if(month == 1){ $("#data"+i.toString()).text(month+" miesiąc temu");}
        else
        $("#data"+i.toString()).text(month+" miesiące temu");
    }

    i++;
    }
}