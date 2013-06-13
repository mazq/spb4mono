$(function () {
    /*
    * 快捷搜索
    */

    var QuickSearcher = function () {
        this.init();
    };

    QuickSearcher.fn = QuickSearcher.prototype = {
        init: function () {
            var self = this;

            this.searchForm = $('#quickSearchForm');
            this.keyword = $('#keyword', this.searchForm);
            //先存储下全局搜索链接
            var globalSearchHref = $("#allResult").data("searchUrl");
            //快捷搜索智能提示
            this.keyword.keyup(function (e) {

                //搜索提示
                if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 13) {
                    return;
                }

                var quickSearchDisplay = $("#quickSearchDisplay");
                var quickSearchList = $(".tnui-quickSearchList");

                if (quickSearchList.length > 0) {
                    if ($(this).val() == "") {
                        quickSearchDisplay.hide();
                    } else {
                        var keyword = $(this).val();
                        quickSearchDisplay.show();

                        //定位
                        var left = $(this).offset().left;
                        var top = $(this).offset().top;
                        quickSearchDisplay.css("z-index", "999").css("position", "absolute").offset({ left: left, top: top + 19 });

                        $("#all-search-span").text(keyword);
                        $("#all-search-a").attr("href", "");
                        $("#all-search-a").attr("href", globalSearchHref + "?keyword=" + keyword);
                        $("#allResult").data("searchUrl", "");
                        $("#allResult").data("searchUrl", globalSearchHref + "?keyword=" + keyword);
                        //如果把这句放到下面注释的地方(在循环里面哦)，那么就只会编码第一次循环，调了好久才发现，不知为何-_-!!
                        keyword = encodeURIComponent(keyword);
                        //加载各应用搜索结果
                        quickSearchList.each(function () {
                            //keyword = encodeURIComponent(keyword);
                            $(this).load($(this).data("url") + keyword + "&topNumber=3");
                        })
                    }
                }

            })

            //当前项号
            var termNum = 0;
            //鼠标按键及Enter事件响应
            this.keyword.keydown(function (e) {
                if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 13) {
                    //获取项的总数目
                    var termSum = $(".tnui-option").length - 1;
                    var keyType;
                    switch (e.keyCode) {
                        case 38: // up
                            e.preventDefault();
                            keyType = "up";
                            break;
                        case 40: // down
                            e.preventDefault();
                            keyType = "down";
                            break;
                        case 13: // enter
                            e.preventDefault();
                            keyType = "enter";
                            break;
                    }

                    var selfOption = $(".tnui-option[iskeyon=1]");
                    //按回车跳到相应的url
                    if (keyType == "enter") {
                        //当前选中的项
                        if (selfOption.length == 0) {
                            self.searchForm.submit();
                        } else {
                            var goUrl = selfOption.data("searchUrl");
                            if (goUrl) {
                                window.location.href = goUrl;
                            } else {
                                e.preventDefault();
                            }
                        }
                    } else if (keyType == "down") {
                        $(".tnui-option").removeClass("tn-bg-gray");
                        //如果没有一个项选中，就选中第一个项，否则就向下移一项
                        if (selfOption.length == 0) {
                            $(".tnui-option:first").attr("iskeyon", "1").addClass("tn-bg-gray");
                        } else {
                            termNum++;
                            //如果到了最后一个，就依然选中第一个项
                            if (termNum > termSum) {
                                termNum = 0;
                            }
                            selfOption.attr("iskeyon", "0").removeClass("tn-bg-gray");
                            $(".tnui-option").eq(termNum).attr("iskeyon", "1").addClass("tn-bg-gray");
                        }
                    } else if (keyType == "up") {
                        $(".tnui-option").removeClass("tn-bg-gray");
                        //如果没有一个项选中，就选中最后一个项，否则就向上移一项
                        if (selfOption.length == 0) {
                            $(".tnui-option:last").attr("iskeyon", "1").addClass("tn-bg-gray");
                            termNum = termSum;
                        } else {
                            //如果到了第一个，就选中最后一个项
                            if (termNum == 0) {
                                termNum = termSum + 1;
                            }
                            termNum--;
                            selfOption.attr("iskeyon", "0").removeClass("tn-bg-gray");
                            $(".tnui-option").eq(termNum).attr("iskeyon", "1").addClass("tn-bg-gray");
                        }
                    }
                }
            })

            //快捷搜索鼠标划过事件
            $(".tnui-quickSearchList").delegate(".tnui-option", "mouseenter", function () {
                $(".tnui-option").removeClass("tn-bg-gray");
                $(this).addClass("tn-bg-gray");
            });

            $(".tnui-quickSearchList").delegate(".tnui-option", "mouseleave", function () {
                $(this).removeClass("tn-bg-gray");
            });

            $("#allResult").mouseover(function () {
                $(".tnui-option").removeClass("tn-bg-gray");
                $(this).addClass("tn-bg-gray");
            });

            $("#allResult").mouseout(function () {
                $(this).removeClass("tn-bg-gray");
            });

            //快捷搜索鼠标点击事件
            $(".tnui-quickSearchList").delegate("li", "click", function () {
                var searchUrl = $(this).data("searchUrl");
                if (searchUrl) {
                    window.location.href = searchUrl;
                }
            });

            //点击页面隐藏智能提示框
            $(document).click(function (e) {
                e = e || window.event;

                if ($(e.target).attr("id") == "keyword" || $(e.target).hasClass("tnui-option") || $(e.target).hasClass("tn-list-item-row")) {
                    return false
                };
                $("#quickSearchDisplay").hide();
            });

            //点击文本框显示智能提示框
            this.keyword.click(function (e) {
                e = e || window.event;
                var element = $(e.target);
                if (element.attr("id") == "keyword" && element.val() == "") { return false };
                $("#quickSearchDisplay").show();
            });

            //关键字输入框水印
            var watermark = $("#quickSearchDisplay").data("watermark");
            this.keyword.watermark(watermark);

            //注册表单提交事件
            this.searchForm.submit(function (e) {
                if ($.trim(self.keyword.val()) == "") {
                    
                }
                //是否跳到相应搜索页面
                var goRelevantApp = $("#quickSearchDisplay").data("gorelevantapp");
                var currentAppName = $("#quickSearchDisplay").data("appname");
                var appOption = $(".tnui-option").eq(0);
                if ((goRelevantApp == true || goRelevantApp == "True") && appOption.data("appname") == currentAppName) {
                    e.preventDefault();
                    window.location.href = appOption.data("searchUrl");
                }
            });

            //注册搜索按钮点击事件
            $('#button-search-header').click(function (e) {
                e.preventDefault();
                self.searchForm.submit();
            });

        }
    }

    QuickSearcher = new QuickSearcher();

})