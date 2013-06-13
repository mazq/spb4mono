
$(document).ready(function () {

    //关注帖吧按钮
    $("a[id^='SubscribeSectionButton_']").live('click', function (e) {
        var $this = $(this);
        var sectionId = $(this).attr("sectionId");
        var buttonUrl = $(this).attr("buttonUrl");
        $.ajaxSetup({ cache: false });
        $.ajax({
            type: "POST",
            url: $this.attr("href"),
            success: function (data) {
                if (data.MessageType == '1') //成功                    
                    $this.replaceWith('<span class="tn-explain-icon"><span class="tn-icon tn-smallicon-accept"></span><span class="tn-icon-text">已关注</span></span>');
                else
                    art.dialog.tips(data.MessageContent, 2, data.MessageType);
            }
        });
        return false;
    });
});