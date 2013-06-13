$(document).ready(function () {
    //关注日志/取消关注
    $("a[id^='blog-subscribe']").live('click', function (e) {
        var self = $(this);
        var threadId = self.attr("threadId");
        var isSubscribePage = self.attr("isSubscribePage").toLowerCase();
        //如果是在 关注的日志 页面
        if (isSubscribePage=="true") {
            art.dialog.confirm('您确认要取消关注吗？', function () {
                $.post(self.attr("href"), function (data) {
                    if (data.MessageType == '1') {
                        self.closest("li").remove();
                    } else {
                        art.dialog.tips(data.MessageContent, 2, data.MessageType);
                    }
                });
            })
        }else{
            $.post(self.attr("href"), function (data) {
                if (data.MessageType == '1') {                
                    $("a[id^='blog-subscribe'][id$='" + threadId + "']").show();
                    self.hide();
                } else {
                    art.dialog.tips(data.MessageContent, 2, data.MessageType);
                }
            });   
        }

        return false;
    });

    //日志列表中的删除操作
    $("a[id^='blog-delete-from-list-']").live('click', function (e) {
        var self = $(this);

        art.dialog.confirm('您确认要删除吗？', function () {
            $.post(self.attr("href"), function (data) {
                art.dialog.tips(data.MessageContent, 1.5, data.MessageType, function () {
                    if (data.MessageType == '1') {
                        self.closest("li").remove();
                    } else {
                        art.dialog.tips(data.MessageContent, 2, data.MessageType);
                    }
                });
            });
        });

        return false;
    });
});