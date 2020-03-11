
$( "#Gamil" ).click(function() {
    copy("pawel.romanowski000@gmail.com");
});

$( "#Phone" ).click(function() {
    copy("Jeszcze niedostępny");
});

function copy(element){
    try
    {
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val(element).select();
        document.execCommand("copy");
        $temp.remove();
        myFunction();
    }
    catch(err){
        $("#contact-fucking-explorer").text(element);
    }
}

function myFunction () 
{

    // Get the snackbar DIV
    var x = document.getElementById("snackbar");

    // Add the "show" class to DIV
    x.className = "show";

    // After 3 seconds, remove the show class from DIV
    setTimeout(function(){ x.className = x.className.replace("show", ""); }, 2000);
}
