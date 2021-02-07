$(document).ready(function () {
    $(".link_playlist").click(function () {
        $(this)[0].children[0].click();
    })

    //Remove Playlist
    $('.PlaylistRemove').click(function () {
        $(this)[0].children[1].submit();
    })
})