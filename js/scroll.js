
$('a[data-href]').on('click', function() {

    var offset = 40;
    var duration = 1500;
    var link = this;

    $('html, body').animate({
        scrollTop: $($(link).data('href')).offset().top - offset 
    }, duration);
    
    return false;
});
