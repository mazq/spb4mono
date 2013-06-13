/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />

(function ($) {
    $(document).ready(function () {
        $("form[plugin!='ajaxForm']").live("submit", function (e) {
            var _form = $(this);
            var _button = $("button[type='submit']", _form);

            if (_form.valid()) {
                _button.attr("disabled", true).removeClass("tn-button-primary").addClass("tn-button-disabled");
            } else {
                _button.attr('disabled', false).removeClass("tn-button-disabled").addClass("tn-button-primary");
                return false;
            }
        });

        $("button[url]").live("click", function () {
            var url = $(this).attr("url");
            if (url)
                window.location.href = url;
        });
        $("form").livequery(function () { $.validator.unobtrusive.parse(document); });


        //处理火狐下刷新后单选框和单选钮仍然选中问题
        if ($.browser.mozilla) {
            $("input[type='radio']").attr("autocomplete", "off");
            $("input[type='checkbox']").attr("autocomplete", "off");
        }

        /*Begin表单内容保存提示插件
        *表单有录入内容但未保存时，用户离开页面提示
        *调用示例：('form').enable_changed_form_confirm("您确定不保存就离开页面吗?");
        */
        //解决IE下 javascript:void(0)会触发window.onbeforeunload事件的问题
        $("a[href='javascript:void(0)']").live("click", function (e) {
            e.preventDefault();
        });
        $("a[href='javascript:void(0);']").live("click", function (e) {
            e.preventDefault();
        });
        $("a[href='javascript:;']").live("click", function (e) {
            e.preventDefault();
        });
        $.fn.enable_changed_form_confirm = function (prompt) {
            var _f = this;
            $('input:text,input:password, textarea', this).each(function () {
                $(this).attr('_value', $(this).val());
            });

            $('input:checkbox,input:radio', this).each(function () {
                var _v = this.checked ? 'on' : 'off';
                $(this).attr('_value', _v);
            });

            $('select', this).each(function () {
                $(this).attr('_value', this.options[this.selectedIndex].value);
            });

            $(this).submit(function () {
                window.onbeforeunload = null;
            });

            window.onbeforeunload = function () {
                if (is_form_changed(_f)) {
                    return prompt;
                }
            }
        }

        function is_form_changed(f) {
            var changed = false;
            $('input:text,input:password,textarea', f).each(function () {
                var _v = $(this).attr('_value');
                if (typeof (_v) == 'undefined') _v = '';
                if (_v != $(this).val()) changed = true;
            });

            $('input:checkbox,input:radio', f).each(function () {
                var _v = this.checked ? 'on' : 'off';
                if (_v != $(this).attr('_value')) changed = true;
            });

            $('select', f).each(function () {
                var _v = $(this).attr('_value');
                if (typeof (_v) == 'undefined') _v = '';
                if (_v != this.options[this.selectedIndex].value) changed = true;
            });
            return changed;
        }

        /*End表单内容保存提示插件*/
    });
})(jQuery);