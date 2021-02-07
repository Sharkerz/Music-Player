$(document).ready(function () {

    var wavesurfer = WaveSurfer.create({
        container: '#music_player'
    });

    wavesurfer.on('ready', function () {
        wavesurfer.play();
    });

    var volume = 0.5;
    wavesurfer.setVolume(volume);

    $('#songTable tbody').on('click', '.SongItemPlay', function () {
        var path = $(this).attr('data-path');
        var title = $(this).attr('data-title');
        var artist = $(this).attr('data-artist');


        $("#player").removeAttr('hidden');
        $("#playerNoHide").removeAttr('hidden');

        $('#play_button').text('pause');
        wavesurfer.play();

        $('#mute_button').text('volume_off');
        wavesurfer.setMute(false)


        $("#title_player").text(title + " - " + artist)
        var origin = window.location.origin;
        wavesurfer.load(origin + "/songs/" + path);
    });

    // Button Play / Pause
    $('#play_button').click(function () {
        if ($(this).text() == 'pause') {
            $(this).text('play_arrow')
        }
        else {
            $(this).text('pause')
        }
        wavesurfer.playPause();
    })

    // Button Mute / Unmute
    $('#mute_button').click(function () {
        if ($(this).text() == 'volume_off') {
            $(this).text('volume_up')
            wavesurfer.setMute(true)
        }
        else {
            $(this).text('volume_off')
            wavesurfer.setMute(false)
        }
    })

    // Volume Up / Down
    $('#more_volume').click(function () {
        if (volume < 0.9) {
            volume += 0.1;
        }
        wavesurfer.setVolume(volume);
    })
    $('#less_volume').click(function () {
        if (volume > 0.1) {
            volume -= 0.1;
        }
        wavesurfer.setVolume(volume);
    })

    //Remove Song from playlist
    $('.SongItemRemove').click(function () {
        $(this)[0].children[1].submit();
    })


    // == Song list ==
    // Delete
    $('#songTable tbody').on('click', '.btnSongDelete', function () {
            $(this).parent()[0].submit();
        })

    $('#songTable tbody').on('click', '.btnSong', function () {
        $(this).prev()[0].click()
    })
});