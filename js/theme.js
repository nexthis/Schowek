//$(':root').css('--themeColor',


function dark(){
    $('#style-text').text("Biały Styl");
    $(':root').css('--accent-color',"#343a40");
    $(':root').css('--color-jumbotron',"#545d67");
    $(':root').css('--color-of-background',"#828a8a");
    $(':root').css('--color-list-group',"#343a40");
    $(':root').css('--color-muted',"#c8ced4");
    $(':root').css('--color-table',"#343a40");
    $(':root').css('--color-text',"whitesmoke");
    $('body').css('color',"whitesmoke");
    $(".modal-content").css('background-color',"var(--accent-color)");
    $("meta[name='theme-color']").attr('content', "#343a40");
}

function bright(){
    $('#style-text').text("Czarny Styl");
    $(':root').css('--accent-color',"rgb(86, 186, 229)"); //#343a40  // bac rgb(76, 76, 76);
    $(':root').css('--color-jumbotron',"#e9ecef");
    $(':root').css('--color-of-background',"whitesmoke");
    $(':root').css('--color-list-group',"#fff");
    $(':root').css('--color-muted',"#6c757d");
    $(':root').css('--color-table',"#dee2e6");
    $(':root').css('--color-text',"black");
    $('body').css('color',"black");
    $(".modal-content").css('background-color',"#FFF");
    $("meta[name='theme-color']").attr('content', "rgb(86, 186, 229)");
}

$('#switch-sm, #switch-mb').change(function() {
    $('#switch-sm, #switch-mb').prop("checked", this.checked);
    if (this.checked) {
        dark();
    } else {
        bright();
    }

});




var checkboxValues = JSON.parse(localStorage.getItem('checkboxValues')) || {},
    $checkboxes = $(".switch :checkbox");

$checkboxes.on("change", function(){
  $checkboxes.each(function(){
    checkboxValues[this.id] = this.checked;
  });
  
  localStorage.setItem("checkboxValues", JSON.stringify(checkboxValues));
});

// On page load
$.each(checkboxValues, function(key, value) {
  $("#" + key).prop('checked', value);
  if(value ==true)dark();
});