/*
* Version: Beta 2
* Release: 2010-7-31
*/

(function ($) {
    var allUIMenus = [];
    function bindEvent(element) {
        var options = $.data(element, "menuButton").options;
        var $this = $(element);
        var ismouseEnterMenu = false;
        if (options.buttonHoverClass)
            $this.removeClass(options.buttonHoverClass);
        var buttonTime = null;
        if (options.disabled == false && options.menu) {
            if (options.clickTrigger) {
                $this.bind("click", function () {
                    killAllUIMenus();
                    if ($(options.menu).is(":hidden"))
                        showmenu();
                    return false;
                });
            }
            else {
                var showMenuTimer;
                $this.bind("mouseenter", function () {
                    showMenuTimer = setTimeout(function () { killAllUIMenus(); showmenu(); }, options.duration);
                    return false;
                }).bind("mouseleave", function () {
                    clearTimeout(showMenuTimer);
                    buttonTime = setTimeout(function () {
                        killAllUIMenus();
                    }, 150);
                });
            }
        }
        //在什么位置显示
        function fixPosotion() {

            var $menuContainer = $(options.menu);
            var left = $this.offset().left;

            var win_width = $(window).width() / 2 + 400;
            if (left + $menuContainer.outerWidth() > win_width) {
                left = left - ($menuContainer.outerWidth() - $this.outerWidth());
            }

            var top = $this.offset().top;
            var scrollTop = $(document).scrollTop();
            if (top + $this.height() + $menuContainer.outerHeight() > scrollTop + $(window).height()) {
                top = top - $menuContainer.outerHeight();
            }
            else {
                top = top + $this.outerHeight();
            }

            if ($menuContainer.hasClass('tnc-prompt')) {
                left = left + 18;
                top = top - 16;
            }
            else {
                top = top - 1;
            }

            if (options.position == "right") {
                top = top - ($this.height() + 8);
                left = left + 10;
            }

            $menuContainer.css({ position: 'absolute', left: left, top: top, "z-index": 9999 }).show();
        }

        function showmenu() {
            $(options.menu).detach().appendTo(document.body);
            fixPosotion();
            //加个方法，获取异步内容，加载到$(options.menu)中。可以在options中增加url参数
            var hasLoadElement = $(options.menu).find('div.tn-loading').length > 0;
            if (options.url && $(options.menu).children().length == 0 || hasLoadElement) {
                if (!hasLoadElement) {
                    $(options.menu).html("<div class=\"tn-loading\"style=\"width:90px\"></div>");
                }
                $.get(options.url, function (data) {
                    $(options.menu).html(data);
                    fixPosotion();
                })
            }
            if (options.buttonHoverClass)
                $this.addClass(options.buttonHoverClass);
            menuBlur();
            $this.blur();
        };
        function killAllUIMenus() {
            $(allUIMenus).each(function (i) {
                if ($(allUIMenus[i]).length) { $(allUIMenus[i]).hide(); };
            });
            if (options.buttonHoverClass)
                $this.removeClass(options.buttonHoverClass);
        }
        function menuBlur() {
            if (options.clickTrigger) {
                $(document).bind("click", function (e) {
                    if ($(e.target).is(options.menu + " *")) {
                        return;
                    }
                    $(document).unbind("click", arguments.callee);
                    killAllUIMenus();
                });
                $(options.menu).find("a").click(function () {
                    var url = $(this).attr("href");
                    if (url) {
                        location.href = url;
                    }
                });
            }
            else {
                var t = null;
                $(options.menu).bind("mouseenter", function () {
                    if (t) {
                        clearTimeout(t);
                        t = null;
                    }
                    if (buttonTime) {
                        clearTimeout(buttonTime);
                        buttonTime = null;
                    }
                }).bind("mouseleave", function () {
                    t = setTimeout(function () {
                        killAllUIMenus();
                    }, options.duration);
                });
            }
        }
    };

    $.fn.menuButton = function (options) {
        options = options || {};
        return this.each(function () {
            var menuButton = $.data(this, "menuButton");
            if (menuButton) {
                $.extend(menuButton.options, options);
            } else {
                var t = $(this);

                $.data(this, "menuButton", { options: $.extend({}, $.fn.menuButton.defaults, { disabled: (t.attr("disabled") ? t.attr("disabled") == "true" : undefined), menu: t.attr("menu"), duration: t.attr("duration") }, options) });
                $(this).removeAttr("disabled");
                allUIMenus.push(options.menu ? options.menu : t.attr("menu"));
                //$(this).parent().append("<span class=\"tn-icon tn-icon-triangle-down\">&nbsp;</span>");
            }
            bindEvent(this);
        });
    };
    $.fn.menuButton.defaults = { disabled: false, url: "", buttonHoverClass: "tn-hover", clickTrigger: false, position: "bottom", menu: null, duration: 500 };

})(jQuery);

$(function () {
    $('[menu]').each(function () {
        var ops = new Object();
        ops.disabled = $(this).attr("data_menu_disabled");
        ops.clickTrigger = $(this).attr("data_menu_clickTrigger");
        ops.url = $(this).attr("url");
        ops.position = $(this).attr("data_menu_position");
        $(this).menuButton(ops);
    });
});