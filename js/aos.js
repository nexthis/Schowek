AOS.init({
    duration: 1200,
    delay:100,
    esing: 'ease-in'
})

$(document).ready(function(){
    $('[data-toggle="popover"]').popover(); 
});

function isScrolledIntoView(elem)
{
    var docViewTop = $(window).scrollTop();
    var docViewBottom = docViewTop + $(window).height();

    var elemTop = $(elem).offset().top;
    var elemBottom = elemTop + $(elem).height();

    return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
}

function Utils() {

}

Utils.prototype = {
    constructor: Utils,
    isElementInView: function (element, fullyInView) {
        var pageTop = $(window).scrollTop();
        var pageBottom = pageTop + $(window).height();
        var elementTop = $(element).offset().top;
        var elementBottom = elementTop + $(element).height();

        if (fullyInView === true) {
            return ((pageTop < elementTop) && (pageBottom > elementBottom));
        } else {
            return ((elementTop <= pageBottom) && (elementBottom >= pageTop));
        }
    }
};

var Utils = new Utils();

$(window).ready(function(){

    var Isadmission = Utils.isElementInView($('#admission'), false);
    var Isphotos = Utils.isElementInView($('#photos'), false);
    var Isrequirements = Utils.isElementInView($('#requirements'), false);
    var Ischange = Utils.isElementInView($('#change'), false);
    if (Isphotos)
        $('#photos').find(">:first-child").removeAttr( "data-aos" );
    if (Isrequirements && Isadmission)
        $('#requirements').find(">:first-child").removeAttr( "data-aos" );
    if (Ischange && Isadmission)
        $('#change').find(">:first-child").removeAttr( "data-aos" );
});