(function ($) {
    $(document).ready(function () {
        //邮箱自动完成的部分脚本
        var strs = new Array();
        var strinfo = "163.com,126.com,qq.com,gmail.com,yahoo.com.cn,yahoo.cn,live.cn,hotmail.com,sina.com,sina.cn,vip.sina.com,my3ia.sina.com,139.com,21cn.com,sogou.com,189.cn,yeah.net,sohu.com,foxmail.com";

        if ($('[plugin="EmailAutoComplete"]').length) {

            //邮箱自动完成的脚本
            $('[plugin="EmailAutoComplete"]').autocomplete({
                minLength: 1,
                source: function (request, response) {
                    if (strs.length == 0)
                        strs = strinfo.split(",");

                    $this = $($(this)[0].element);

                    var userinput = $this.val();
                    if ($this.val().indexOf("@") > 0)
                        userinput = $this.val().substr(0, $this.val().indexOf("@"));

                    var strEmail = new Array();

                    if ($this.val().indexOf("@") > 0)
                        var emailSuffix = $this.val().substr($this.val().indexOf("@") + 1, $this.val().length - $this.val().indexOf("@") + 1);

                    var i = 0;
                    $.each(strs, function (index, item) {
                        if (emailSuffix && emailSuffix != "" && item.indexOf(emailSuffix) >= 0) {
                            strEmail[i] = {
                                "value": userinput + "@" + item,
                                "label": userinput + "@" + item
                            };
                            i++;
                        } else if (!emailSuffix || emailSuffix == "") {
                            strEmail[i] = {
                                "value": userinput + "@" + item,
                                "label": userinput + "@" + item
                            };
                            i++;
                        }
                    });

                    response(strEmail);
                },

                search: function (event, ui) { },
                open: function (event, ui) { },
                focus: function (event, ui) {
                    $this.val(ui.item.label);
                    return false;
                },
                select: function (event, ui) {
                    $this.val(ui.item.label);
                    return false;
                }
            }).data("autocomplete")._renderItem = function (ul, item) {
                return $("<li class='ui-corner-project'></li>").data("item.autocomplete", item).append("<a>" + item.value + "</a>").appendTo(ul);
            };
        }
    });
})(jQuery);
