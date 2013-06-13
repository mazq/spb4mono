
(function ($) {
    $(document).ready(function () {
        $("a[id^='attitude-'][id$='-action']").livequery("click", function (e) {
            e.preventDefault();

            var self = $(this);
            var attitude = $(this).data("attitude");

            //未登录用户不能操作
            if (attitude.disabled === "NotLogin") {
                var dialog = art.dialog();
                $.ajax({
                    type: "GET",
                    url: attitude.loginUrl,
                    dataType: "html",
                    cache: false,
                    success: function (data) {
                        dialog.content(data);
                    }
                });
                return false;
            }

            //不能顶踩自己
            if (attitude.disabled === "CurrentUser") {
                art.dialog.tips("不能对自己的内容进行操作！", 1.5, -1);
                return false;
            }

            //判断是否允许操作
            if (attitude.mode === "Bidirection") {
                //1.正常操作，即已经顶踩过，又点击了一次
                //2.处于禁用状态（已反向操作且不允许修改）
                if (self.hasClass("tn-icon-active")) {
                    art.dialog.alert("不能重复进行此操作！");
                    return false;
                }
                if (self.hasClass("tn-bg-deep")) {
                    return false;
                }
            } else {
                //单向操作，已经操作过并且不允许取消，则不做任何操作
                if (self.hasClass("tn-icon-active") && !attitude.enableCancel) {
                    art.dialog.alert("不能重复进行此操作！");
                    return false;
                }
            }

            var url = $(this).attr("href");

            $.post(url, function (data) {
                if (data.MessageType === 1) {
                    if (attitude.mode === "Bidirection") {
                        //反向操作的链接对象
                        var other = self.siblings("a");

                        //根据反向操作的当前状态，判断之前是否已经顶过或踩过
                        //如果已经顶过或踩过
                        if (other.hasClass("tn-icon-active") || other.hasClass(" tn-bg-deep")) {
                            //不管是否可取消，反向操作的顶踩数-1
                            var otherCount = $("#" + other.attr("id") + "-count");
                            otherCount.html(parseInt(otherCount.html()) - 1);
                            if (other.hasClass("tn-icon-active")) {
                                other.removeClass("tn-icon-active");
                            } else if (other.hasClass(" tn-bg-deep")) {
                                other.removeClass(" tn-bg-deep");
                                other.children("span:first").removeClass("tn-border-light").addClass("tn-border-deep");
                            }

                            //如果不可取消，当前操作方向的顶踩数+1
                            if (!attitude.enableCancel) {
                                var selfCount = $("#" + self.attr("id") + "-count");
                                selfCount.html(parseInt(selfCount.html()) + 1);
                                //区分小手还是方块
                                if (attitude.AttitudeStyle == "SupportOppose") {
                                    self.addClass("tn-icon-active");
                                } else if (attitude.AttitudeStyle == "UpDown") {
                                    self.addClass(" tn-bg-deep");
                                    self.children("span:first").removeClass("tn-border-deep").addClass("tn-border-light");
                                }
                            }
                            //如果没有顶过或踩过
                        } else {
                            //首先当前操作方向顶踩数+1
                            var selfCount = $("#" + self.attr("id") + "-count");
                            selfCount.html(parseInt(selfCount.html()) + 1);
                            if (attitude.AttitudeStyle == "SupportOppose") {
                                self.addClass("tn-icon-active");
                            } else if (attitude.AttitudeStyle == "UpDown") {
                                self.addClass(" tn-bg-deep");
                                self.children("span:first").removeClass("tn-border-deep").addClass("tn-border-light");
                            }

                            //如果不可修改，反向操作的样式变为disabled
                            if (!attitude.isModify) {
                                other.addClass("tn-icon-disabled");
                            }
                        }

                        var callback = eval(attitude.onSuccessCallBack);
                        if (callback) {
                            callback(data);
                        }
                    } else {
                        var selfCount = $("#" + self.attr("id") + "-count");

                        //原先喜欢过
                        if (self.hasClass("tn-icon-active")) {
                            selfCount.html(parseInt(selfCount.html()) - 1);
                            self.removeClass("tn-icon-active")
                        } else {
                            selfCount.html(parseInt(selfCount.html()) + 1);
                            self.addClass("tn-icon-active")
                        }
                    }
                } else {
                    art.dialog.tips(data.MessageContent, 1.5, data.MessageType);
                }
            });
        });
    });
})(jQuery);