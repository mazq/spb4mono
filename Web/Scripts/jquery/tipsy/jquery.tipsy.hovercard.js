// tipsy.hovercard, twitter style hovercards for tipsy
// version 0.1.1
// (c) 2010 René Föhring rf@bamaru.de
// released under the MIT license

(function ($) {
    $.fn.tipsyHoverCard = function (options) {

        var opts = $.extend({}, $.fn.tipsyHoverCard.defaults, options);
        this.tipsy(opts);

        function clearHideTimeout(ele) {
            if (ele.data('timeoutId')) clearTimeout(ele.data('timeoutId'));
            ele.data('timeoutId', null);
        }
        function setHideTimeout(ele) {
            clearHideTimeout(ele);
            var options = ele.tipsy(true).options;
            var timeoutId = setTimeout(function () { $(ele).tipsy('hide'); }, options.hideDelay);
            ele.data('timeoutId', timeoutId);
        }

        function show(ele) {
            clearHideTimeout(ele);

            var tip = ele.tipsy('tip');
            ele.tipsy('show');
            tip.addClass('tipsy-hovercard');
            tip.addClass(ele.attr('outerclass'));

            var sizeObj = ele;
            if (ele.children().length > 0) {
                sizeObj = ele.children().first();
            }

            var pos = sizeObj.offset();
            var top = pos.top, left = pos.left;

            var outerwidth = sizeObj.outerWidth(), outerheight = sizeObj.outerHeight();

            var $arrowRefer = tip.find("div[class*='tn-bubble-arrow-']");
            if ($arrowRefer.length == 1) {

                var regDirection = /tipsy-[a-z]/;
                var direction = regDirection.exec(tip.attr('class'));

                var regArrow = /tn-bubble-arrow-[a-zA-Z]*/g;
                var arrowDirection = regArrow.exec($arrowRefer.attr("class"));

                var newArrowDirection = 'tn-bubble-arrow-';
                switch (direction.toString().charAt(6)) {
                    case 's':
                        top -= ($arrowRefer.outerHeight() + 8);
                        left = outerwidth < 35 ? (left - 21 + (outerwidth / 2 - 5)) : left;
                        newArrowDirection += 'bottom';
                        break;
                    case 'n':
                        top += outerheight;
                        left = outerwidth < 35 ? (left - 21 + (outerwidth / 2 - 5)) : left;
                        newArrowDirection += 'top';
                        break;
                    case 'w':
                        newArrowDirection += 'left';
                        break;
                    case 'e':
                        newArrowDirection += 'right';
                        break;
                }
                ele.data('content', ele.data('content').replace(arrowDirection.toString(), newArrowDirection.toString()));
            }

            if (ele.data('content')) {
                $('div.tipsy-inner', tip).html(ele.data('content'));
            }

            tip.css({ top: top, left: left });
            tip.data('tipsyAnchor', ele);
            tip.hover(tipEnter, tipLeave);
            ele.data('visible', true);

        }

        function hide(ele) {
            setHideTimeout(ele);
            ele.data('visible', false);
        }

        function enter() {
            var a = $(this);
            var url = a.data('userCardUrl');
            if (url && !a.data('content')) {
                $.ajax({
                    url: url,
                    dataType: "html",
                    success: function (data) {
                        a.data("content", data);
                        show(a);
                    },
                    error: function () {
                        a.attr('title', 'Error loading ' + url);
                        if (a.data('visible')) show(a);
                    },
                    failure: function () {
                        a.attr('title', 'Failed to load ' + url);
                        if (a.data('visible')) show(a);
                    }
                });
            }
            else
                show(a);
        }

        function leave() {
            hide($(this));
        }

        function tipEnter() {
            var a = $(this).data('tipsyAnchor');
            clearHideTimeout(a);
        }
        function tipLeave() {
            var a = $(this).data('tipsyAnchor');
            setHideTimeout(a);
        }

        if ($.fn.hoverIntent && opts.hoverIntent) {
            // 'out' is called with a latency, even if 'timeout' is set to 0
            // therefore, we're using good ol' mouseleave for out-handling
            var config = $.extend({ over: enter, out: function () { } }, opts.hoverIntentConfig);
            this.hoverIntent(config).mouseleave(leave);
        } else {
            this.unbind('hover').bind(function () { enter }, function () { leave });
        }
        return this;
    }

    $.fn.tipsyHoverCard.defaults = {
        gravity: $.fn.tipsy.autoNS,
        trigger: 'manual',
        fallback: '',
        html: true,
        title: function () {
            var content = $(this).data("content");
            if (content)
                return content;
            return $(this).attr("title") || $(this).attr("original-title") || "";
        },
        hideDelay: 300,
        opacity: 1,
        hoverIntent: true,
        hoverIntentConfig: {
            sensitivity: 3,
            interval: 300,
            timeout: 0
        }
    };
})(jQuery);
