
document.addEventListener("mousemove", (event) => {
    document.body.style.transition = 0.3 + "s";
    document.body.style.backgroundPositionX = Math.round(- event.pageX / 100) + "px";
    document.body.style.backgroundPositionY = Math.round(-event.pageY / 100) + "px";
});

function Diff(left, right)
{
   return left - right;
}

function onSearch() {
    let form = document.getElementById("srchform");
    let value = document.getElementById("srchitem").value;
    form.action += "?src=" + value;
    form.submit();
}

function onSelectRadio(name)
{
    let form = document.getElementById("form-radio");
    form.action += "?type=" + name.slice(1, name.length);
    form.submit();
}


function onRem(id)
{
    let form = document.getElementById("rm-action");
    let item = document.getElementById("rm-item-" + id).value;
    form.action += "?guid=" + item;
    form.submit();
}