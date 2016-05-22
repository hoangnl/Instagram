var itemToPlay;
$(document).on('click', '.screen', function () {
    itemToPlay = $(this).attr('id');
    console.log(itemToPlay);
    id = itemToPlay.substring(6, itemToPlay.length);
    console.log(id);
    PlayVideo(id);
});
$(document).on('click', '.play-button', function () {
    itemToPlay = $(this).attr('id');
    id = itemToPlay.substring(11, itemToPlay.length);
    PlayVideo(id);
});

function PlayVideo(id) {
    $('video').each(function () {
        $(this)[0].pause();
    });
    newFeedsList = document.getElementById('newFeedsList');
    video = document.getElementById('video' + id);
    pauseScreen = document.getElementById('screen' + id);
    screenButton = document.getElementById('screen-button' + id);

    // Progress Bar Container
    pbarContainer = document.getElementById('pbar-container' + id);
    pbar = document.getElementById('pbar' + id);

    // Buttons Container
    playButton = document.getElementById('play-button' + id);
    timeField = document.getElementById('time-field' + id);
    soundButton = document.getElementById('sound-button' + id);
    sbarContainer = document.getElementById('sbar-container' + id);
    sbar = document.getElementById('sbar' + id);
    fullscreenButton = document.getElementById('fullscreen-button' + id);

    video.load();
    if (video.paused) {

        video.play();
        playButton.src = '/Images/Player/pause.png';
        update = setInterval(updatePlayer, 30);
        pauseScreen.style.display = 'none';
        screenButton.src = '/Images/Player/play.png';

    } else {
        video.pause();
        playButton.src = '/Images/Player/play.png';
        window.clearInterval(update);

        pauseScreen.style.display = 'block';
        screenButton.src = '/Images/Player/play.png';
    }
    video.addEventListener('canplay', function () {

        //playButton.addEventListener('click', playOrPause, false);
        pbarContainer.addEventListener('click', skip, false);
        updatePlayer();
        soundButton.addEventListener('click', muteOrUnmute, false);
        sbarContainer.addEventListener('click', changeVolume, false);
        fullscreenButton.addEventListener('click', fullscreen, false);
        //screenButton.addEventListener('click', playOrPause, false);

    }, false);
};

function playOrPause() {
    if (video.paused) {
        video.play();
        playButton.src = '/Images/Player/pause.png';
        update = setInterval(updatePlayer, 30);

        pauseScreen.style.display = 'none';
        screenButton.src = '/Images/Player/play.png';
    } else {
        video.pause();
        playButton.src = '/Images/Player/play.png';
        window.clearInterval(update);

        pauseScreen.style.display = 'block';
        screenButton.src = '/Images/Player/play.png';
    }
}

function updatePlayer() {
    var percentage = (video.currentTime / video.duration) * 100;
    pbar.style.width = percentage + '%';
    timeField.innerHTML = getFormattedTime();
    if (video.ended) {
        window.clearInterval(update);
        playButton.src = '/Images/Player/replay.png';

        pauseScreen.style.display = 'block';
        screenButton.src = '/Images/Player/replay.png';
    } else if (video.paused) {
        playButton.src = '/Images/Player/play.png';
        screenButton.src = '/Images/Player/play.png';
    }
}

function skip(ev) {
    var mouseX = ev.pageX - pbarContainer.offsetLeft - newFeedsList.offsetLeft;
    var width = window.getComputedStyle(pbarContainer).getPropertyValue('width');
    width = parseFloat(width.substr(0, width.length - 2));
    video.currentTime = (mouseX / width) * video.duration;
    updatePlayer();
}

function getFormattedTime() {
    var seconds = Math.round(video.currentTime);
    var minutes = Math.floor(seconds / 60);
    if (minutes > 0) seconds -= minutes * 60;
    if (seconds.toString().length === 1) seconds = '0' + seconds;

    var totalSeconds = Math.round(video.duration);
    var totalMinutes = Math.floor(totalSeconds / 60);
    if (totalMinutes > 0) totalSeconds -= totalMinutes * 60;
    if (totalSeconds.toString().length === 1) totalSeconds = '0' + totalSeconds;

    return minutes + ':' + seconds + ' / ' + totalMinutes + ':' + totalSeconds;
}

function muteOrUnmute() {
    if (!video.muted) {
        video.muted = true;
        soundButton.src = '/Images/Player/mute.png';
        sbar.style.display = 'none';
    } else {
        video.muted = false;
        soundButton.src = '/Images/Player/sound.png';
        sbar.style.display = 'block';
    }
}

function changeVolume(ev) {
    var mouseX = ev.pageX - sbarContainer.offsetLeft - newFeedsList.offsetLeft;
    var width = window.getComputedStyle(sbarContainer).getPropertyValue('width');
    width = parseFloat(width.substr(0, width.length - 2));

    video.volume = (mouseX / width);
    sbar.style.width = (mouseX / width) * 100 + '%';
    video.muted = false;
    soundButton.src = '/Images/Player/sound.png';
    sbar.style.display = 'block';
}

function fullscreen() {
    if (video.requestFullscreen) {
        video.requestFullscreen();
    } else if (video.webkitRequestFullscreen) {
        video.webkitRequestFullscreen();
    } else if (video.mozRequestFullscreen) {
        video.mozRequestFullscreen();
    } else if (video.msRequestFullscreen) {
        video.msRequestFullscreen();
    }
}