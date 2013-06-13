/// <reference path="jquery-1.7.1.js" />
/// <reference path="jquery-ui-1.8.7.js" />
///zhengw:用于解决IE9下关闭不了上传图片对话框问题，
///问题描述：jqueryUI1.8.7有个很变态的地方，它会重写cleanData，把对的改成了错的，
///用本文件再次替换为jquery类库中的方法。
(function ($) {


    $.cleanData = function (elems) {
        var data, id,
			cache = jQuery.cache,
			special = jQuery.event.special,
			deleteExpando = jQuery.support.deleteExpando;

        for (var i = 0, elem; (elem = elems[i]) != null; i++) {
            if (elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) {
                continue;
            }

            id = elem[jQuery.expando];

            if (id) {
                data = cache[id];

                if (data && data.events) {
                    for (var type in data.events) {
                        if (special[type]) {
                            jQuery.event.remove(elem, type);

                            // This is a shortcut to avoid jQuery.event.remove's overhead
                        } else {
                            jQuery.removeEvent(elem, type, data.handle);
                        }
                    }

                    // Null the DOM reference to avoid IE6/7/8 leak (#7054)
                    if (data.handle) {
                        data.handle.elem = null;
                    }
                }

                if (deleteExpando) {
                    delete elem[jQuery.expando];

                } else if (elem.removeAttribute) {
                    elem.removeAttribute(jQuery.expando);
                }

                delete cache[id];
            }
        }
    }


} (jQuery));