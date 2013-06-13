/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />
/// <reference path="jquery.metadata.js" />

(function ($) {

    /*
    * 字符串格式化方法扩展，用于模板字符串替换
    */
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/\{(\d+)\}/g, function (m, i) {
            return args[i];
        });
    }



    /*
    *快捷回复：响应Ctrl+Enter快捷键
    */
    $.fn.ShortcutSubmit = function () {
        this.keyup(function (e) {
            var stat = false;
            if (e.keyCode == 17) {
                stat = true;
                //取消等待
                setTimeout(function () {
                    stat = false;
                }, 500);
            }
            if (((e.keyCode || e.which) == 13) && (e.ctrlKey || stat)) {
                $(this).parents("form:first").submit();
            }
        })
    }

    //焦点改变的时候
    $.fn.OnFocusChange = function (focusClass, outFocusClass, exclude) {
        var comments = this;
        this.each(function () {
            $(this).die();
            $comment = $(this);
            $focusClass = focusClass || $(this).data("focus");
            $outFocusClass = outFocusClass || $(this).data("outfocus");
            if ($outFocusClass)
                $comment.addClass($outFocusClass);
            if ($focusClass)
                $comment.removeClass($focusClass);
            $(this).click(function () {
                $comment = $(this);
                $focusClass = $comment.data("focus");
                $outFocusClass = $comment.data("outfocus");
                if ($outFocusClass)
                    $comment.removeClass($outFocusClass);
                if ($focusClass)
                    $comment.addClass($focusClass);
                $exclude = exclude || $comment.data("exclude");
                $(document).bind("click", function (evt) {
                    if ($outFocusClass)
                        comments.addClass($outFocusClass);
                    if ($focusClass)
                        comments.removeClass($focusClass);
                    if ($(evt.target).closest($comment).length > 0 || ($exclude && $(evt.target).is($exclude))) {
                        if ($outFocusClass)
                            $comment.removeClass($outFocusClass);
                        if ($focusClass)
                            $comment.addClass($focusClass);
                    } else {
                        $(document).unbind("click", arguments.callee);
                    }
                });
            });
        });
    }

    //全选/全不选

    $(document).ready(function () {
        $.ajaxSetup({ cache: false });

        //响应ctrl+enter按键
        $("[plugin='ShortcutSubmit']").livequery(function () {
            $(this).ShortcutSubmit();
        });

        //处理照片的自动缩放        
        $("img").livequery(function () {
            var $this = $(this);

            var objectWidth = $this.attr("width");      //img控件的定义宽度
            var objectHeight = $this.attr("height");    //img控件的定义高度

            var img = $("<img/>");
            img.load(function () {
                var imgWidth = this.width;               //图片实际宽度
                var imgHeight = this.height;             //图片实际高度

                if (objectWidth && objectHeight) {          //同时设置了img控件的宽度和高度
                    if (objectWidth >= imgWidth && objectHeight >= imgHeight) {
                        $this.removeAttr("width");
                        $this.removeAttr("height");
                    } else if (objectWidth >= imgWidth && objectHeight < imgHeight) {
                        $this.removeAttr("width");
                    } else if (objectWidth < imgWidth && objectHeight >= imgHeight) {
                        $this.removeAttr("height");
                    } else if (objectWidth < imgWidth && objectHeight < imgHeight) {
                        if (imgWidth >= imgHeight) {
                            $this.removeAttr("height");
                        } else {
                            $this.removeAttr("width");
                        }
                    }
                } else if (objectWidth) {                   //只设置了img控件的宽度
                    if (objectWidth >= imgWidth) {
                        $this.removeAttr("width");
                    }
                } else if (objectHeight) {                  //只设置了img控件的高度
                    if (objectHeight >= imgHeight) {
                        $this.removeAttr("height");
                    }
                }
            });
            img.attr("src", $this.attr("src"));
        });

        //根据焦点添加和删除样式。
        $('div[plugin="OnFocus"]').livequery(function () {
            $comment = $(this);
            $focusClass = $(this).data("focus");
            $outFocusClass = $(this).data("outfocus");
            if ($outFocusClass)
                $comment.addClass($outFocusClass);
            if ($focusClass)
                $comment.removeClass($focusClass);
            $(this).click(function () {
                $comment = $(this);
                $focusClass = $comment.data("focus");
                $outFocusClass = $comment.data("outfocus");
                if ($outFocusClass)
                    $comment.removeClass($outFocusClass);
                if ($focusClass)
                    $comment.addClass($focusClass);
                $exclude = $comment.data("exclude");
                $(document).bind("click", function (evt) {
                    if ($outFocusClass)
                        $('div[plugin="OnFocus"]').addClass($outFocusClass);
                    if ($focusClass)
                        $('div[plugin="OnFocus"]').removeClass($focusClass);
                    if ($(evt.target).closest($comment).length > 0 || ($exclude && $(evt.target).is($exclude))) {
                        if ($outFocusClass)
                            $comment.removeClass($outFocusClass);
                        if ($focusClass)
                            $comment.addClass($focusClass);
                    } else {
                        $(document).unbind("click", arguments.callee);
                    }
                });
            });
        });

        $('[plugin="pageSize"]').livequery(function () {
            var $select = $(this).find("select");
            $select.change(function () {
                $.cookie('PageSize', $(this).val(), { path: '/' });
                refresh();
            });
            if ($.cookie('PageSize') != null) {
                $select.val($.cookie('PageSize'), { path: '/' });
            }
        });

        //获取更多控件的脚本
        $('[plugin="GetMore"]').livequery(function () {
            $(this).click(function (e) {
                e.preventDefault();
                $this = $(this);
                $this.hide();
                $this.after("<div class='tn-loading tn-border-gray tn-corner-all'></div>");
                $link = $(this).find("a");
                $.get($link.attr("href"), function (data) {
                    var $next = $this.next();
                    if ($next.is(".tn-loading")) {
                        $next.remove();
                    }
                    $this.replaceWith(data);
                });
            });
        });

        //后台右侧的菜单显示控制
        $('[plugin="ShortcutMenu"]').livequery(function () {
            $parent = $(this).parent();
            $parent.removeClass("tn-open");
            $parent.removeClass("tn-close");
            if ($.cookie('ShortcutMenuIsOpen') == 'false' || $.cookie('ShortcutMenuIsOpen') == false) {
                $parent.addClass("tn-close");
            } else {
                $parent.addClass("tn-open");
            }
            $(this).click(function () {
                $parent = $(this).parent();
                if ($parent.is(".tn-open")) {
                    $parent.removeClass("tn-open");
                    $parent.addClass("tn-close");
                    $.cookie('ShortcutMenuIsOpen', null); // 删除
                    $.cookie('ShortcutMenuIsOpen', false, { path: '/', expires: 7 }); //设置带时间的cookie  7天
                } else {
                    $.cookie('ShortcutMenuIsOpen', null); // 删除
                    $.cookie('ShortcutMenuIsOpen', true, { path: '/', expires: 7 }); //设置带时间的cookie  7天
                    $parent.removeClass("tn-close");
                    $parent.addClass("tn-open");
                };
            });
        });

        //动态操作脚本
        $('a[plugin="activityOperation"]').live("click", function () {
            $li = $('input[name="' + $(this).attr("name") + '"]').closest("li.tn-list-item");
            $.post($(this).attr("href"), function (data) {
                if (data.MessageType >= 0) {
                    $li.slideUp("fast");
                }
                else {
                    alert(data.MessageContent);
                }
            });
            return false;
        });

        //tabs 插件
        $('[plugin="tabs"]').livequery(function () {
            var data = parseObject($(this).attr("data"));
            if (data.select)
                data.select = eval(data.select);
            if (data.isSample)
                $(this).spbtabs(data);
            else
                $(this).tabs(data);
            if ($("a[tabtarget]", $(this)).length)
                $(this).bind("tabsselect", function (event, ui) {
                    var url = $.data(ui.tab, 'load.tabs');
                    if (data.isSample)
                        url = $.data(ui.tab, 'load.spbtabs');
                    if (url) {
                        if ($(ui.tab).is("a[tabtarget='self']")) {
                            location.href = url;
                            return false;
                        }
                        else if ($(ui.tab).is("a[tabtarget='blank']")) {
                            window.open($.data(ui.tab, 'load.tabs'));
                            if (data.isSample)
                                window.open($.data(ui.tab, 'load.spbtabs'));

                            return false;
                        }
                    }
                    return true;
                });
        });
        //Upoload

        $('[plugin="uploadify"]').livequery(function () {
            $(this).each(function () {
                var datas = parseObject($(this).attr("data"));
                $(this).uploadify(datas);
            });
        });
        //ztree
        $('[plugin="Tree"]').livequery(function () {
            var treeid = $(this).attr("id");
            //展开树之前回调函数
            var curExpandNode = null;

            //单一路径函数
            function singlePath(newNode) {
                if (newNode === curExpandNode) return;
                if (curExpandNode && curExpandNode.open == true) {
                    var zTree = $.fn.zTree.getZTreeObj(treeid);
                    if (newNode.parentTId === curExpandNode.parentTId) {
                        zTree.expandNode(curExpandNode, false);
                    } else {
                        var newParents = [];
                        while (newNode) {
                            newNode = newNode.getParentNode();
                            if (newNode === curExpandNode) {
                                newParents = null;
                                break;
                            } else if (newNode) {
                                newParents.push(newNode);
                            }
                        }
                        if (newParents != null) {
                            var oldNode = curExpandNode;
                            var oldParents = [];
                            while (oldNode) {
                                oldNode = oldNode.getParentNode();
                                if (oldNode) {
                                    oldParents.push(oldNode);
                                }
                            }
                            if (newParents.length > 0) {
                                for (var i = Math.min(newParents.length, oldParents.length) - 1; i >= 0; i--) {
                                    if (newParents[i] !== oldParents[i]) {
                                        zTree.expandNode(oldParents[i], false);
                                        break;
                                    }
                                }
                            } else {
                                zTree.expandNode(oldParents[oldParents.length - 1], false);
                            }
                        }
                    }
                }
                curExpandNode = newNode;
            }
            //被展开之后回调函数
            function onExpand(event, treeId, treeNode) {
                //如果存在额外方法则执行
                var addOnExpand = data.Settings.callback.addOnExpand;
                if (addOnExpand) {
                    addOnExpand(event, treeId, treeNode);
                }
                curExpandNode = treeNode;
            }
            function beforeExpand(treeId, treeNode) {
                //如果存在额外方法则执行
                var addBeforeExpand = data.Settings.callback.addBeforeExpand;
                if (addBeforeExpand) {
                    addBeforeExpand(treeId, treeNode);
                }
                var pNode = curExpandNode ? curExpandNode.getParentNode() : null;
                var treeNodeP = treeNode.parentTId ? treeNode.getParentNode() : null;
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                for (var i = 0, l = !treeNodeP ? 0 : treeNodeP.children.length; i < l; i++) {
                    if (treeNode !== treeNodeP.children[i]) {
                        zTree.expandNode(treeNodeP.children[i], false);
                    }
                }
                while (pNode) {
                    if (pNode === treeNode) {
                        break;
                    }
                    pNode = pNode.getParentNode();
                }
                if (!pNode) {
                    singlePath(treeNode);
                }
            }
            var data = eval("(" + ($(this).attr("data")) + ")");
            if (data.TreeNodes) {
                data.TreeNodes = eval(data.TreeNodes);
            }
            if (data.Settings) {
                data.Settings = eval(data.Settings);
            }
            //初始化
            $.fn.zTree.init($("#" + $(this).attr("id")), data.Settings, data.TreeNodes);
        });

        //日期选择器
        $('[plugin="datetimepicker"]').livequery(function () {
            var data = parseObject($(this).attr("data"));
            var minDate = $(this).attr("data-val-rangedate-min");
            var maxDate = $(this).attr("data-val-rangedate-max");
            if (minDate)
                $.extend(data, { minDate: minDate });
            if (maxDate)
                $.extend(data, { maxDate: maxDate });
            if (data.onSelect)
                data.onSelect = eval(data.onSelect);
            if (data.onClose)
                data.onClose = eval(data.onClose);
            if (data.beforeShow)
                data.beforeShow = eval(data.beforeShow);
            if (Boolean(data.showTime))
                $(this).datetimepicker(data);
            else
                $(this).datepicker(data);
        });

        // 联动下拉列表
        $('[plugin="linkageDropDownList"]').livequery(function () {
            //$(this).removeAttr("name");
            $("select", $(this)).change(linkageDropDownListChange);
            //初始化
            $("select:last", $(this)).change();
        });
        //Ajax分页ajaxPagingButton
        $('[plugin="ajaxPagingButton"] a').live("click", function () {
            var data = parseObject($(this).parent().attr("data"));
            $.get(this.href, function (html) {
                $("#" + data.updateTargetId).replaceWith(html);
            });
            return false;
        });

        // AjaxAction请求@html.ajaxAction
        $('[plugin="ajaxAction"] ').livequery(function () {
            var $this = $(this);
            var data = parseObject($(this).attr("data"));
            $.get(data.url, function (html) {
                $this.replaceWith(html);
            });
        });

        //弹出提示消息
        $('[plugin="tipsy"]').livequery(function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoNS });
        });
        //AJAX删除按钮
        $("a[plugin='AjaxDeleteButton']").livequery(function () {
            $(this).unbind("click").click(function () {
                var $this = $(this);
                var postHref = $this.attr("href");
                var datainfo = parseObject($(this).attr("data"));

                art.dialog.confirm(datainfo.confirm, function () {
                    $.post(postHref, function (data) {
                        if (data.MessageType >= 0) {
                            $(datainfo.deleteTarget).slideUp("fast", function () {
                                $this.remove();
                                var fnsucess = eval(datainfo.SuccessFn);
                                if (fnsucess)
                                    fnsucess(data);
                            });
                        }
                        else {
                            var fnerror = eval(datainfo.ErrorFn);
                            if (fnerror)
                                fnerror(data);
                            else
                                alert(data.MessageContent);
                        }
                    });
                }, function () {
                });

                return false;
            });
        });

        //模式框
        $('a[plugin="dialog"]').live("click", function () {
            var id = this.id || "tunynet_dialog";
            var data = parseObject($(this).attr("data"));
            data = $.extend({ id: id, title: false }, data || {});
            data.close = eval(data.close);
            data.init = eval(data.init);
            data.show = eval(data.show);
            var dialog = art.dialog(data);
            $.get(this.href, function (html) {
                if (typeof html == "object" && html.MessageContent) {
                    dialog.close();
                    art.dialog.tips(html.MessageContent, 1.5, html.MessageType);
                }
                else
                    dialog.content(html);
            });
            return false;
        });

        //加关注点模式框"X"时回调函数
        function DialogCloseFn() {
            //            var id = this.config.id;
            //            var itemId = id.split('-')[1];
            //            $("#" + id).remove();
            //            $("span[id='hasFollowed-" + itemId + "']").show();
            //            $("a[id='hasFollowed-" + itemId + "']").show(); //处理Brisk皮肤下的按钮样式
        }

        $('[dialogOperation = "closeAll"]').live("click", function () {
            var list = art.dialog.list;
            for (var i in list) {
                list[i].close();
            };
            return false;
        });
        $('[dialogOperation = "close"]').live("click", function () {
            var dialog = artDialog.focus;
            if (dialog)
                dialog.close();
            return false;
        });
        $('[dialogOperation = "hide"]').live("click", function () {
            var dialog = artDialog.focus;
            if (dialog)
                dialog.hide();
            return false;
        });

        //添加水印
        $.watermark.options.hideBeforeUnload = false;
        $('input[type="text"][watermark],textarea[watermark]').livequery(function () {
            $(this).watermark($(this).attr("watermark"));
        });

        $('button[menu],a[menu]').livequery(function () {
            var ops = new Object();
            ops.disabled = $(this).attr("data_menu_disabled");
            ops.clickTrigger = $(this).attr("data_menu_clickTrigger");
            ops.url = $(this).attr("url");
            $(this).menuButton(ops);
            $("a.tn-item-link", $($(this).attr("menu"))).hover(
		    function () {
		        $(this).addClass("tn-bg-gray");
		    },
		    function () {
		        $(this).removeClass("tn-bg-gray");
		    }
        );

        });

        //ajaxForm
        $('form[plugin="ajaxForm"]').livequery(function () {
            var data = parseObject($(this).attr("data"));

            data.beforeSubmit = function (arr, $form, options) {
                if ($.watermark) {
                    $form.find("input:text,textarea").each(function () {
                        $.watermark._hide($(this));
                    });
                }
                if (!$form.valid()) {
                    return false;
                }
                $form.block({
                    message: '处理中'
                });
                var beforeSubmitFn = eval(data.beforeSubmitFn);
                if (beforeSubmitFn)
                    beforeSubmitFn(arr, $form, options);
            };

            data.error = function (response, statusText, xhr, $form) {
                $form.unblock();
                var errorFn = eval(data.errorFn);
                if (errorFn) {
                    errorFn(response.responseText, statusText, xhr, $form);
                }
            };

            data.success = function (response, statusText, xhr, $form) {
                $form.unblock();
                var successFn = eval(data.successFn);
                if (successFn) {
                    $.ajaxSetup({ cache: false });
                    successFn(response, statusText, xhr, $form);
                }
                if (data.closeDialog) {
                    var list = art.dialog.list;
                    for (var i in list) {
                        if ($(list[i].content()).find($form).length)
                            list[i].close();
                    };
                }
            };
            $(this).ajaxForm(data);
        });

        //用户卡片气泡样式显示
        $("[plugin='tipsyHoverCard']").livequery(function () {
            $(this).tipsyHoverCard();
        });

        //拉黑
        $("a[id^='StopUser_']").live("click", function (e) {
            e.preventDefault();
            $this = $(this);

            art.dialog.confirm('加入黑名单会解除关系，确定要加入黑名单吗？&nbsp;&nbsp;', function () {
                $.ajax({
                    type: "Post",
                    url: $this.attr("href"),
                    success: function () {
                        $this.remove();
                        art.dialog.tips("加入黑名单成功", 1.5, 1);
                    },
                    error: function () {
                        art.dialog.tips("加入黑名单失败", 1.5, -1);
                    }
                });

            });
        });
    });
    //转为日期格式
    function parseDate(value) {
        if (value == null)
            return null;
        return $.datepicker.parseDate($.datepicker.W3C, value);
    }
    //转为对象类型
    function parseObject(value) {
        if (value == null)
            return null;
        return eval("(" + value + ")");
    }
    //联动下拉列表change事件
    function linkageDropDownListChange() {
        var $currentDropDownList = $(this);
        var data = parseObject($(this).parent().attr("data"));
        var index = eval(this.id.substr(data.ControlName.length + 1));
        //如果选中有效值，则将当前下拉列表设置为表单项；否则将前一个下拉列表设置为表单项
        $currentDropDownList.nextAll("select").remove();
        var value = $currentDropDownList.val();

        if (value && value.length > 0 && index < data.Level - 1) { //加载下一级
            $.getJSON(data.GetChildSelectDataUrl, { id: $currentDropDownList.val() }, function (response) {
                if (response == null)
                    return;
                if (!response.length)
                    return;
                //更新当前下拉列表后面的下拉列表
                $currentDropDownList.after("\n<select id='" + data.ControlName + "_" + eval(parseInt(index) + 1)
                                                     + "' class='tn-dropdownlist'><option value=''>请选择</option></select>");
                $(response).each(function () {
                    $("#" + data.ControlName + "_" + eval(parseInt(index) + 1))
                          .append("<option value='" + this.id + "'>" + this.name + "</option>");
                });
                $currentDropDownList.next().change(linkageDropDownListChange);
            });
        }
        if (index > 0 && value == data.DefaultValue)
            value = $("#" + data.ControlName + "_" + eval(parseInt(index) - 1)).val();
        $('input[type="hidden"][name="' + data.ControlName + '"]').val(value);
    }    //向文本框的光标处插入内容

    //标签式导航
    $('.spb-nav1-area li.tn-list-item-position:not(.tn-navigation-active)').hover(function () {
        $(this).addClass('tn-navigation-hover');
    }, function () {
        $(this).removeClass('tn-navigation-hover');
    });


    //交互状态 
    $('.tn-state-default,.tn-menu-item').live("mouseover", function () {
        $(this).addClass('tn-state-hover');
    }).live("mouseout", function () {
        $(this).removeClass('tn-state-hover');
    });

    //表格 
    $('.tn-table-grid-row').hover(function () {
        $(this).addClass('tn-bg-gray');
    }, function () {
        $(this).removeClass('tn-bg-gray');
    });

    //按钮
    $('.tn-button-default').live("mouseover", function () {
        $(this).addClass('tn-button-default-hover');
    }).live("mouseout", function () {
        $(this).removeClass('tn-button-default-hover');
    });

    $('.tn-button-primary').live("mouseover", function () {
        $(this).addClass('tn-button-primary-hover');
    }).live("mouseout", function () {
        $(this).removeClass('tn-button-primary-hover')
    });

    $('.tn-button-secondary').live("mouseover", function () {
        $(this).addClass('tn-button-secondary-hover');
    }).live("mouseout", function () {
        $(this).removeClass('tn-button-secondary-hover');
    });

    $('.tn-button-lite').live("mouseover", function () {
        $(this).addClass('tn-button-default');
    }).live("mouseout", function () {
        $(this).removeClass('tn-button-default');
    });

    $('a[id$=_videobtn],a[ntype=video]').live("click", function (event) {

        event.preventDefault();
        var $mediaContainer = $('#videoContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {

            var href = $clickObj.attr('href');
            if (!href || href == 'javascript:;') {

                if ($('textarea[custombuttons]').length) {
                    var data = $.parseJSON($('textarea[custombuttons]').attr('custombuttons'));
                    href = data.videoButton + '?textAreaId=' + $('textarea[custombuttons]').attr('id');
                }
            }

            $.get(href, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));

            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    $('a[ntype=topic]').live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#topicContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {

            var href = $clickObj.attr('href');

            $.get(href, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));

            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    $('a[ntype=image]').live("click", function (event) {
        event.preventDefault();
        var $mediaContainer = $('#imageContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {

            var href = $clickObj.attr('href');

            $.get(href, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));
            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });


    $('a[id$=_musicbtn],a[ntype=music]').live("click", function (event) {

        event.preventDefault();
        var $mediaContainer = $('#musicContainer');
        $clickObj = $(this);

        if ($mediaContainer.length == 0) {
            var href = $clickObj.attr('href');
            if (!href || href == 'javascript:;') {

                if ($('textarea[custombuttons]').length) {
                    var data = $.parseJSON($('textarea[custombuttons]').attr('custombuttons'));
                    href = data.musicButton + '?textAreaId=' + $('textarea[custombuttons]').attr('id');
                }
            }

            $.get(href, function (data) {
                $('body').append(toggleMediaContainerStatus($(data), $clickObj));

            });
        }
        else {
            toggleMediaContainerStatus($mediaContainer, $clickObj);
        }
    });

    function toggleMediaContainerStatus(obj, clickObj) {

        if (obj.is(":hidden")) {
            var $parentNode = clickObj.parent();
            var position, top, left;

            if ($parentNode.is('span') && $parentNode.parents('table.aui_dialog').length == 0) {
                position = $parentNode.offset();
                top = (position.top + 15);
                left = position.left;
            }
            else {
                position = clickObj.offset();
                top = position.top + 15;
                left = position.left - 17;
            }
            obj.attr("style", "display:block;top:" + top + "px; left:" + left + "px;");

            $(document).bind("click", function (e) {
                $(document).unbind("click", arguments.callee);
                if ($(e.target).is($('*:not(.tn-smallicon-cross)', obj))) {
                    return;
                }

                obj.hide();
            });
        }
        else {
            obj.hide();
        }

        return obj;
    }

    var loadingMusic = false;
    $('a[ntype="mediaPlay"]').live("click", function (event) {
        event.preventDefault();
        if (loadingMusic) {
            return false;
        }

        var _this = $(this);
        if (_this.siblings('dl').length == 0) {
            loadingMusic = true;
            $.get(_this.attr('href'), function (data) {
                _this.parent().append(data);
                loadingMusic = false;
                _this.siblings('a[ntype="mediaPlay"],br').andSelf().hide();
            });
        }
    });

    $('a[ntype="closeMedia"]').live("click", function (event) {
        event.preventDefault();

        var parent = $(this).parents('dl');
        parent.siblings('a[ntype=mediaPlay],br').show();
        parent.remove();
    });

    var $emotion;
    //表情选择器
    $('a[id$=_smileybtn],span[ntype=emotion]').live("click", function () {

        var $emotionSelector = $('#emotionSelector');
        $emotion = $(this);

        if ($emotionSelector.length == 0) {
            var emotionUrl = $("input[name='list-emotions']").val();
            if (emotionUrl == undefined) {
                return false;
            }

            $.get(emotionUrl, function (data) {
                $('body').append(toggleEmotionSelectorStatus($(data)));
            });
        }
        else {
            $("input[name='list-emotions']").empty();
        }

        if ($emotionSelector.is(":hidden")) {
            toggleEmotionSelectorStatus($emotionSelector);
        } else {
            toggleEmotionSelectorStatus($emotionSelector);
            toggleEmotionSelectorStatus($emotionSelector);
        }

        return false;
    });

    function toggleEmotionSelectorStatus(obj) {
        if (obj.is(":hidden")) {
            var $parentNode = $emotion.parent();
            var position, top, left;

            if ($parentNode.is('span') && $parentNode.parents('table.aui_dialog').length == 0) {
                position = $emotion.parent().offset();
                top = (position.top + 15);
                left = position.left;
            }
            else {
                position = $emotion.offset();
                top = position.top + 15;
                left = position.left - 17;
            }

            obj.attr("style", "display:block;top:" + top + "px; left:" + left + "px;");
            if ($('div[id=emotion-container]', obj).children('div[id=listEmotions-]').length <= 0) {
                $("#emotion-tabs a:first", obj).one('click', function () {
                    bindEmotionTabClickEvent($(this));
                    return false;
                }).click();
            }

            $(document).bind("click", function (e) {
                $(document).unbind("click", arguments.callee);
                var $emotionTabs = $("#emotion-tabs *", $(this));
                if ($(e.target).is($emotionTabs)) {
                    return;
                }

                obj.hide();
            });
        }
        else {
            obj.hide();
        }

        return obj;
    }

    $("#emotion-tabs a").live('click', function () {
        bindEmotionTabClickEvent($(this));
        return false;
    });

    function bindEmotionTabClickEvent(obj) {

        var $emotionTabs = obj.parents("#emotion-tabs");
        $("li", $emotionTabs).removeClass("tn-tabs-selected");
        obj.parent().addClass("tn-tabs-selected");

        var value = obj.attr("value");
        var $listEmotions = $("#listEmotions-" + value, $emotionTabs.siblings('#emotion-container'));


        if ($listEmotions.length > 0) {
            $listEmotions.show();
        }
        else {
            LoadEmotions($emotionTabs.siblings('#emotion-container'), value, obj.attr("ohref"));
        }

        $("div[id^='listEmotions-']:not(#listEmotions-" + value + ")", $("#emotion-container")).hide();
    }

    function LoadEmotions(obj, value, ohref) {
        if (obj.children('#listEmotions-' + value).length > 0)
            return;

        $.getJSON(ohref, { directoryName: value }, function (data) {

            var $listEmotions = $("<div id='listEmotions-" + value + "' class='tn-emotion-list tn-helper-clearfix' style='height:120px;'></div>");
            $(data.Emotions).each(function () {

                $listEmotions.append("<span class=\"tn-border-gray\"><img src='" + this.ImgUrl + "' alt='" + this.Description +
                                     "' title='" + this.Description + "' value=\"" + this.Code + "\"/></span>");
            });

            $("#emotion-container").prepend($listEmotions);

            //鼠标移至表情时，显示预览图片
            $("span", obj).mouseover(function (e) {
                e.preventDefault();

                var imgHtml = "<span class='tn-widget-content tn-border-gray'><img src='" + $(this).children("img").attr("src") + "' alt='" + $(this).children("img").attr("alt") +
                     "' title='" + $(this).children("img").attr("title") + "' value='" + $(this).children("img").attr("value") + "'style=\"max-height:40px;max-width:40px;\"/></span>";

                if ($(this).index() % 12 > 5)
                    $("#leftOriginal").html(imgHtml);
                else
                    $("#rightOriginal").html(imgHtml);
            }).mouseout(function () { $("#emotion-container div.tn-emotion-original").html(""); }); // end img mouseover

            $("img", obj).parent().off('click').click(function (e) {

                e.preventDefault();

                $addEmotion = $('#addEmotions-');
                $img = $(this).find('img');

                $textArea = $emotion.parents('form').find('textarea');
                if ($textArea.tinymce && $textArea.attr('plugin') == 'tinymce') {
                    var imgHtml = "<span><img src='" + $img.attr("src") + "' alt='" + $img.attr("alt") +
                    "' title='" + $img.attr("title") + "' value=\"" + $img.attr("value") + "\"/></span>";

                    $textArea.insertContentToEditor(imgHtml);
                }
                else {
                    if ($.watermark)
                        $.watermark.hide($textArea);
                    insertAtCaret($textArea[0], $img.attr("value"));
                    $('#emotionSelector').hide();
                }

                return false;
            });  // end img click
        });   // end $.get
    }
})(jQuery);

function checkAll(allCheckBox, itemName) {
    var items = document.getElementsByName(itemName);
    for (var i = 0; i < items.length; i++) {
        if (items[i].type == 'checkbox') {
            items[i].checked = allCheckBox.checked;
        }
    }
}
//刷新当前页面
function refresh() {
    window.location.reload();
}

//向文本域中插入文本
function insertAtCaret(textObj, textFeildValue) {
    if (document.all && textObj.createTextRange && textObj.caretPos) {
        var caretPos = textObj.caretPos;
        caretPos.text = caretPos.text.charAt(caretPos.text.length - 1) == '' ? textFeildValue + '' : textFeildValue;
    } else if (textObj.setSelectionRange) {
        var rangeStart = textObj.selectionStart;
        var rangeEnd = textObj.selectionEnd;
        var tempStr1 = textObj.value.substring(0, rangeStart);
        var tempStr2 = textObj.value.substring(rangeEnd);
        textObj.value = tempStr1 + textFeildValue + tempStr2;
        textObj.focus();
        var len = textFeildValue.length;
        textObj.setSelectionRange(rangeStart + len, rangeStart + len);
        //textObj.blur();
    } else {
        textObj.value += textFeildValue;
    }
}
