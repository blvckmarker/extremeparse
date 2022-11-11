
document.addEventListener("mousemove", (event) => {
    document.body.style.backgroundPositionX = -event.pageX / 100 + "px";
    document.body.style.backgroundPositionY = -event.pageY / 100 + "px";
});

document.onloadstart = (event) => {
    if (window.screen.width < 1200)
        document.body.style.background = "rbg(13,17,23)";
}