
document.addEventListener("mousemove", (event) => {
    document.body.style.transition = 0.3 + "s";
    document.body.style.backgroundPositionX = Math.round(- event.pageX / 100) + "px";
    document.body.style.backgroundPositionY = Math.round(-event.pageY / 100) + "px";
});

function Diff(left, right)
{
   return left - right;
}

