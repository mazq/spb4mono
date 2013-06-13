
$(document).ready(function () {

    $("a[name^='deleteCurrentMblog-']").live('click', function (e) {
        e.preventDefault();
        $this = $(this);
        var microblogId = $this.data("id");
        art.dialog.confirm("删除该条微博？", function () {
            $.ajax({
                type: "Post",
                url: $this.attr("href"),
                success: function () {
                    $("#microblog-" + microblogId).fadeOut();
                },
                error: function () {
                    art.dialog.tips("删除失败", 1.5, -1);
                }
            });
        });
    });

    $("a[id^='favoriteMicroblog-']").live('click', function (e) {

        e.preventDefault();
        var $this = $(this);

        if ($this.attr('status') == '1') {
            $.dialog.confirm("确认取消收藏这条微博？", function () {
                $.ajax({
                    type: "Post",
                    url: $this.attr("href"),
                    success: function (data) {
                        $this.text('收藏').attr('status', '0');
                        $.dialog.tips("取消成功", 1.5, 1);
                    },
                    error: function () {
                        $.dialog.tips("取消失败", 1.5, -1);
                    }
                });
            });
        }
        else {
            $.ajax({
                type: "Post",
                url: $this.attr("href"),
                success: function (data) {
                    $this.text('取消收藏').attr('status', '1');
                    $.dialog.tips("收藏成功", 1.5, 1);
                },
                error: function () {
                    $.dialog.tips("收藏失败", 1.5, -1);
                }
            });
        }


    });

    //home end

    //_MicroblogInList_Attachments.cshtml  begin
    $("a[id^='attachmentsListLiImage-']").live('click', function (e) {
        e.preventDefault();

        $this = $(this);
        var microblogId = $this.attr("data-microblogId");
        //在图片加载完成之前，显示等待的样式
        $("<span class=\"tn-loading\" id=\"attachmentsListLiImageWatingSpan-" + microblogId + "\"></span>").prependTo("#attachmentsListLiImage-" + microblogId);
        $("#microblogAttachmentContainer-" + microblogId).load($this.attr("href"), function () {
            $("#attachmentsListLiImageWatingSpan-" + microblogId).remove();
            $("#attachmentsListUl-" + microblogId).hide();
        });
    });


    $("a[id^='attachmentsListLiVideo-']").live('click', function (e) {

        $this = $(this);
        var $parent = $this.parents('.tn-waterfall-list');
        e.preventDefault();

        if ($parent.length == 0) {

            var microblogId = $this.attr("data-microblogId");
            //在加载视频之前，显示等待样式
            $("<span class=\"tn-loading\" id=\"attachmentsListLiVideoWatingSpan-" + microblogId + "\"></span>").prependTo("#attachmentsListLiVideo-" + microblogId);
            $("#microblogAttachmentContainer-" + microblogId).load($this.attr("href"), function () {
                $("#attachmentsListLiVideoWatingSpan-" + microblogId).remove();
                $("#attachmentsListUl-" + microblogId).hide();
            });
        }
        else {
            document.location.href = $parent.find('a[name="ShowMicroblog"]').attr('href');
        }

    });

    $("a[id^='attachmentsListLiMusic-']").live('click', function (e) {

        $this = $(this);
        var $parent = $this.parents('.tn-waterfall-list');
        e.preventDefault();

        if ($parent.length == 0) {

            var microblogId = $this.attr("data-microblogId");
            //在加载视频之前，显示等待样式
            $("<span class=\"tn-loading\" id=\"attachmentsListLiMusicWatingSpan-" + microblogId + "\"></span>").prependTo("#attachmentsListLiMusic-" + microblogId);
            $("#microblogAttachmentContainer-" + microblogId).load($this.attr("href"), function () {
                $("#attachmentsListLiMusicWatingSpan-" + microblogId).remove();
                $("#attachmentsListUl-" + microblogId).hide();
            });
        }
        else {
            document.location.href = $parent.find('a[name="ShowMicroblog"]').attr('href');
        }

    });
    //_MicroblogInList_Attachments.cshtml  end

    //_MicroblogInList_Attachments_Images.cshtml 
    $("a[id^='attachmentsListLiImageUnfold-']").live("click", function (e) {
        e.preventDefault();
        $this = $(this);
        var microblogId = $this.attr("data-microblogId");
        $("#attachmentsListUl-" + microblogId).show();
        $("#microblogAttachmentContainer-" + microblogId).html("");
    });


    //_MicroblogInList_Attachments_Music.cshtml
    $("a[id^='attachmentsListLiMusicUnfold-']").live("click", function (e) {
        e.preventDefault();
        $this = $(this);
        var microblogId = $this.attr("data-microblogId");
        $("#attachmentsListUl-" + microblogId).show();
        $("#microblogAttachmentContainer-" + microblogId).html("");
    });

    //_MicroblogInList_Attachments_Video.cshtml
    $("a[id^='attachmentsListLiVideoUnfold-']").live("click", function (e) {
        e.preventDefault();
        $this = $(this);
        var microblogId = $this.attr("data-microblogId");
        $("#attachmentsListUl-" + microblogId).show();
        $("#microblogAttachmentContainer-" + microblogId).html("");
    });

});

//_MicroblogInList_Attachments_Images.cshtml 
//左右旋转 触发的事件
//参数说明：angle-旋转的角度（顺时针），picID-要旋转的图片标签的ID
var turn90Angle = function (angle, picID) {
    //首先创建 一个img标签
    var turnAngleCookie = "spb_MicroblogPic_turnAngle_" + picID;
    var copyPicID = picID + "_Copy";
    var currentPic = $("#" + picID);
    var copyCurrentPic = $("#" + copyPicID);

    if (copyCurrentPic.length <= 0) {
        currentPic.css("display", "none");
        currentPic.after('<img id="' + copyPicID + '" src="' + currentPic.attr("src") + '"  />');
    }

    //只有一种情况需要调整图片的高度：满足条件如下
    //1 原图宽大于高（如果显示框就是宽大于高，这条忽略）
    //2 原图宽大大于显示框的高
    //3 当下一个旋转的是：90或者270度的时候
    var viewHeight = 520;
    var currentAngle = $.cookie(turnAngleCookie);
    if (currentPic.width() > viewHeight) {
        if (currentAngle == 0 || currentAngle == 180 || currentAngle == -180)
            $("#" + copyPicID).attr("width", viewHeight);
        else
            $("#" + copyPicID).removeAttr("width");
    }


    currentAngle = currentAngle * 1 + angle * 1;
    currentPic.css("display", "none");
    copyCurrentPic = $("#" + copyPicID);

    copyCurrentPic.rotate({ angle: currentAngle });

    if (currentAngle > 270)
        currentAngle = currentAngle - 360;
    else if (currentAngle < -180)
        currentAngle = currentAngle + 360;

    $.cookie(turnAngleCookie, currentAngle);


    var containerHeight = currentPic.height();
    if (currentAngle % 180 != 0)
        containerHeight = currentPic.width();
    currentPic.parent().parent().attr("style", "height:" + containerHeight + "; overflow:hidden");
    copyCurrentPic.parent().attr("onclick", "$('#" + picID + "').css('display', 'inline');$('#" + copyPicID + "').remove();$.cookie(turnAngleCookie, 0);");

}