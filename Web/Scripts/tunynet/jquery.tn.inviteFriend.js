(function ($) {
    $(document).ready(function () {
        //已经注册过的用户点击时
        $("li[id^='follow-user']").live("click", function () {
            $inputCheckbox = $(this).find("input[type='checkbox']");
            if ($inputCheckbox.is(":checked")) {
                $(this).removeClass("tn-bg-deep tn-border-deep tn-corner-all");
                $(this).addClass("tn-border-gray");
                $inputCheckbox.removeAttr("checked");
            } else {
                $(this).addClass("tn-bg-deep tn-border-deep tn-corner-all");
                $(this).removeClass("tn-border-gray");
                $inputCheckbox.attr("checked", "checked");
            }
            GetSelectNum();
        });

        //点击全选按钮
        $("input#sendEmails-checkbox-checkall").live("click", function () {
            $inputSendEmails = $("input[type='checkbox'][name='SendEmails']");
            $inputFollowUsers = $("input[type='checkbox'][name='followUsers']");
            var $followUsers = $("li[id^='follow-user-']");
            $li = $("li[id^='follow-user']");
            if (this.checked) {
                $li.addClass("tn-bg-deep tn-border-deep tn-corner-all");
                $li.removeClass("tn-border-gray");
                $inputSendEmails.attr("checked", "checked");
                $inputFollowUsers.attr("checked", "checked");
            } else {
                $li.removeClass("tn-bg-deep tn-border-deep tn-corner-all");
                $li.addClass("tn-border-gray");
                $inputSendEmails.removeAttr("checked");
                $inputFollowUsers.removeAttr("checked");
            }
            GetSelectNum();
        });
        //点击复选框时
        $("input").live("click", function () {
            GetSelectNum();
        });

        //控制csv模块和Email模块之间的隐藏与显示
        $("a[id^='show-tnc-invite']").click(function (e) {
            e.preventDefault();
            $this = $(this);
            $("div[id='" + $this.attr("name") + "']").show();
            $this.parents("div[id^='tnc-invite']").hide();
        });
        //控制点击图片时，自动更改下拉菜单中的内容
        $("img[id^='click-mail']").click(function () {
            $("img[id^='click-']").removeClass("tn-border-gray");
            $thisImg = $(this);
            $thisImg.addClass("tn-border-gray");
            $("select[id='dropdown-emals'] option").removeAttr("selected");
            $("select[id='dropdown-emals'] option").each(function () {
                if ($thisImg.attr("alt") == $(this).html()) {
                    $(this).attr("selected", "selected");
                }
            });
        });
        //验证用户输入的邮箱是否符合标准
        $("a#button-csv-submit").click(function () {
            //验证用户输入是否正确
            var arr = $(document.getElementById('emails-user-input')).val().split(',');
            $(arr).each(function (i) {
                if (this == "") {
                    arr.splice(i, i + 1);
                }
            });
            if (arr.length > 20) {
                //超过长度，
                alert("一次最多支持20个邮箱，现在您已经超过" + (arr.length - 20) + "个");
            } else {
                $(this).parents("form:first").submit();
            }
        });
        //触发标签为A的submit按钮
        $("a[id^='button-submit-']").live('click', function (e) {
            e.preventDefault();
            $(this).parents("form:first").submit();
        });
    });

    //获取选择的按钮
    function GetSelectNum() {
        $inputSendEmails = $("input[name='SendEmails']:checked");
        $inputFollowUsers = $("input[name='followUsers']:checked");
        $allCount = $inputFollowUsers.length + $inputSendEmails.length;
        $("em[id='em-SelectNum']").html($allCount);
        if ($allCount <= 0) {
            $("button#attentionButton").addClass("tn-button-disabled");
            $("button#attentionButton").attr("disabled", "disabled");
        } else {
            $("button#attentionButton").removeClass("tn-button-disabled");
            $("button#attentionButton").removeAttr("disabled");
        }
    }

    function errorCallBack(data) {
        data = $.parseJSON(data);
        //        art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
        alert(data.MessageContent);
    }
})(jQuery);