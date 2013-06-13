/*
* Version: Beta 2
* Release: 2010-5-29
*/
(function ($) {
    var photoViewer = {};
    var settings = {
        open: function () { },
        close: function () { }
    }

    //初始化
    photoViewer.Init = function (options) {
        options = $.extend(settings, options || {});
        return this.each(function (nr) {
            photoViewer.Interface(this, options);
        });
    };

    photoViewer.Interface = function (el, options) {
        $(el).click(function () {
            var dialog = art.dialog({
                title: false,
                width: '100%',
                height: '100%',
                left: '0%',
                top: '0%',
                fixed: false,
                resize: false,
                drag: false
            });
            // jQuery ajax
            var currentData = $(el).data("dialog");
            if (currentData) {
                dialog.content(currentData);
            }
            else {
                $.ajax({
                    url: el.href,
                    success: function (data) {
                        dialog.content(data);
                        $(el).data("dialog", data);
                    },
                    cache: true
                });
            }

            return false;
        });
    }
    $.fn.photoViewer = photoViewer.Init;

})(jQuery);