$(document).ready(function () {
    $("a.tn-smallicon-upload").first().hide();
    $("a.tn-smallicon-download").last().hide();

    $(".tn-smallicon-upload").click(function (e) {
        e.preventDefault();
        $this = $(this);
        var tr = $(this).parents("tr:first");
        var id = tr.data("id");
        var referenceId = tr.prev().data("id");

        $.ajax({
            type: "POST",
            url: $this.attr("href"),
            data: { id: id, referenceId: referenceId },
            success: function (data) {
                if (data.MessageType == -1) {
                    art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
                } else {
                    var trBefore = tr.prev();
                    tr.insertBefore(trBefore);
                    $("a.tn-smallicon-upload,a.tn-smallicon-download").show();
                    $("a.tn-smallicon-upload").first().hide();
                    $("a.tn-smallicon-download").last().hide();
                }
            }
        });
    });

    $("a.tn-smallicon-download").click(function (e) {
        e.preventDefault();

        $this = $(this);
        var tr = $(this).parents("tr:first");
        var id = tr.data("id");
        var referenceId = tr.next().data("id");

        $.ajax({
            type: "POST",
            url: $this.attr("href"),
            data: { id: id, referenceId: referenceId },
            success: function (data) {
                if (data.MessageType == -1) {
                    art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
                } else {
                    var trAfter = tr.next();
                    tr.insertAfter(trAfter);
                    $("a.tn-smallicon-upload,a.tn-smallicon-download").show();
                    $("a.tn-smallicon-upload").first().hide();
                    $("a.tn-smallicon-download").last().hide();
                }
            }
        });
    });
});
