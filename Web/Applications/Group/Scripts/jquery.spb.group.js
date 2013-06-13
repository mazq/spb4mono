$(function () {
    $("a[id^='applyJoin-']").live('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        var joinway = $this.data("joinway");
        var url = $this.attr("href");
        var isApplied = $this.data("isapplied");
        $.ajaxSetup({ cache: false });


        //done:zhaok,by zhengw:为什么要判断url来确定是直接加入方式？这种方法太不好了，可以在链接上加个标识属性data-joinway="@((int)JoinWay.Direct)"来实现；
        if (joinway == 'Direct') {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.MessageType == '1') //成功                    
                    {
                        $this.hide().next().show();
                        var sucess = eval($this.data("success"));
                        if (sucess)
                            sucess();
                    }
                    else
                        art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
                }
            });
            return false;
        }

        if (joinway == 'ByApply' && isApplied == "True") {
            art.dialog.tips("您已申请过该群组！", 1.5, 0);
            return false;
        }


    });
    $("a[id^='quitGroup-']").live('click', function () {
        $.post($(this).attr("href"), function (data) {
            art.dialog.tips(data.MessageContent, 1.5, data.MessageType, function () { window.location.reload(); });
        });
        return false;
    });

    $("#editAnnouncement").toggle(function () {
        var $span = $(this).children("span.tn-icon");
        $span.removeClass("tn-smallicon-collapse-open").addClass("tn-smallicon-collapse-close");
        $(this).children("span.tn-action-text").html("折叠");
        $("#shortAnnouncement").hide();
        $("#longAnnouncement").show();
    }, function () {
        var $span = $(this).children("span.tn-icon");
        $span.removeClass("tn-smallicon-collapse-close").addClass("tn-smallicon-collapse-open");
        $(this).children("span.tn-action-text").html("展开");
        $("#shortAnnouncement").show();
        $("#longAnnouncement").hide();
    });

    $("li[name='application-all']").addClass("tn-tabs-selected");
    $("#microblogType").hide();

    $("li[name^='application-']").live('click',function () {
        if ($(this).attr("name") == 'application-Microblog') {
            $("#microblogType").show();
        }
        else {
            $("#microblogType").hide();
        }
        var $this = $(this);
        var url = $(this).children("a").attr("href");
        $("#activitiesForLoadDiv").load(url, function () {
            $("li[name^='application-']").removeClass("tn-tabs-selected");
            $this.addClass("tn-tabs-selected");
        });
        return false;
    });

    $("span[name^='microblogType-']").live('click',function () {
        var $this = $(this);
        var url = $(this).children("a").attr("href");
        $("#activitiesForLoadDiv").load(url, function () {
            $("span[name^='microblogType-']").removeClass("tn-selected");
            $this.addClass("tn-selected");
        });
        return false;
    });

});
