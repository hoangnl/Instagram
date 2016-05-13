
$("#open_btn").click(function () {
    $.FileDialog({ multiple: true }).on('files.bs.filedialog', function (ev) {
        var files = ev.files;
        var text = "";
        files.forEach(function (f) {
            text += f.name + "<br/>";
        });
        $("#output").html(text);
    }).on('cancel.bs.filedialog', function (ev) {
        $("#output").html("Cancelled!");
    });
});

