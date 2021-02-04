var Nightly = new Nightly();

var theme = localStorage.getItem('theme');
if (theme == "dark") {
    Nightly.darkify()
    $('#btn_theme').text('wb_sunny');
}
else {
    Nightly.lightify()
    $('#btn_theme').text('brightness_3');
}


document.getElementById("btn_theme").addEventListener("click", function () {
    theme = localStorage.getItem('theme');
    if (theme == "dark") {
        localStorage.setItem('theme', 'light');
        $(this).text('brightness_3');
    }
    else {
        localStorage.setItem('theme', 'dark');
        $(this).text('wb_sunny');
    }
    Nightly.toggle()
});