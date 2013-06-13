/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery-ui.js" />
(function ($) {
    $.validator.setDefaults({
        ignore: ".ignore,:hidden:not(input[type='hidden'],textarea[plugin='tinymce'])",
        errorClass: "field-validation-error",
        errorElement: "span",
        errorPlacement: function (label, element) {
            // position error label after generated textarea
            if (element.is("textarea")) {
                label.insertAfter(element.next());
            } else {
                label.insertAfter(element)
            }
        },
        focusInvalid: function () {
            // put focus on tinymce on submit validation
            if (this.settings.focusInvalid) {
                try {
                    var toFocus = $(this.findLastActive() || this.errorList.length && this.errorList[0].element || []);
                    if (toFocus.is("textarea")) {
                        tinyMCE.get(toFocus.attr("id")).focus();
                    } else {
                        toFocus.filter(":visible").focus();
                    }
                } catch (e) {
                    // ignore IE throwing errors when focusing hidden elements
                }
            }
        }
    });

    // The validator function
    $.validator.addMethod('rangeDate', function (value, element, param) {
        if (!value) {
            return true; // not testing 'is required' here!
        }
        try {
            var dateValue = $.datepicker.parseDate($.datepicker.W3C, value); // hard-coding uk date format, but could embed this as an attribute server-side (based on the current culture)
        }
        catch (e) {
            return false;
        }
        return param.min <= dateValue && dateValue <= param.max;
    });

    // The adapter to support ASP.NET MVC unobtrusive validation
    $.validator.unobtrusive.adapters.add('rangedate', ['min', 'max'], function (options) {
        var params = {
            min: $.datepicker.parseDate($.datepicker.W3C, options.params.min),
            max: $.datepicker.parseDate($.datepicker.W3C, options.params.max)
        };

        options.rules['rangeDate'] = params;
        if (options.message) {
            options.messages['rangeDate'] = options.message;
        }
    });

    // The validator function
    $.validator.addMethod('isTrue', function (value, element) {
        return $(element).is(":checked");
    });

    // The adapter to support ASP.NET MVC unobtrusive validation
    $.validator.unobtrusive.adapters.add('istrue', [], function (options) {
        options.rules['isTrue'] = {};
        if (options.message) {
            options.messages['isTrue'] = options.message;
        }
    });

    jQuery.validator.addMethod("minBlength", function (value, element, param) { return this.optional(element) || value.replace(/[^\x00-\xff]/g, "**").length >= param * 2; });
    jQuery.validator.addMethod("maxBlength", function (value, element, param) { return this.optional(element) || value.replace(/[^\x00-\xff]/g, "**").length <= param * 2; });

    jQuery.validator.addMethod("regularExpression", function (value, element, param) {
        if (element) {
            var reg = new RegExp(param);
            return reg.test(value);
        }
        else {
            return false;
        }
    }, "");
    //重写获取长度方法，解决单双字节问题、忽略Html标记
    //    $.extend($.validator.prototype,
    //    {
    //        getLength: function (value, element) {
    //            switch (element.nodeName.toLowerCase()) {
    //                case 'select':
    //                    return $("option:selected", element).length;
    //                case 'input':
    //                    if (this.checkable(element)) {
    //                        return this.findByName(element.name).filter(':checked').length;
    //                    }
    //                    if ((/password/i).test(element.type)) {
    //                        return value.length;
    //                    }
    //            }
    //            var length = value.replace(/<[^>]+>/g, "").length;    
    //            return length;
    //        }
    //    });
} (jQuery));