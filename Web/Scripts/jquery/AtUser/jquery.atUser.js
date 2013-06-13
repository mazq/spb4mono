/**
* 功能介绍：主要实现微博@用户
* 使用说明：触发对象.atUser(data, textareaId, randomNum, selector)
* data:数据
* textareaId:显示数据的文本框
* randomNum:随机数，用于区分不同的弹出框
* selector:触发对象
* 创建者fanyc 20121114
*/

(function () {
    (function ($) {
        AtUser = function (data, textareaId, randomNum, selector) {
            this.data = data;
            this.textareaId = textareaId;
            this.randomNum = randomNum;
            this.selector = selector;
            this.init();

        };

        AtUser.prototype = {
            constructor: AtUser,
            init: function () {
                var self = this;
                self.selector = this.selector;
                self.atUserView = $("#atUserView-" + self.randomNum);
                self.userList = $("#userList", self.atUserView);
                self.searchUser = $("#searchUser", self.atUserView);
                self.close = $("#close", self.atUserView);
                self.keyword = "";
                //判断是否是HTML编辑器
                if (self.selector.tinymce) {
                    if (!self.atUserView.is(":visible")) {
                        self.searchUser.val("");
                        self._display(self.data.slice(0, 8));
                        self.searchUser.focus();
                    }

                }

                //单击按钮事件，显示弹出框
                self.selector.live("click", function (e) {
                    if (!self.atUserView.is(":visible")) {
                        self.searchUser.val("");
                        self._display(self.data.slice(0, 8));
                        self.searchUser.focus();
                    }
                    return false;
                });
                //单击其他位置，关闭弹出框
                $(document).click(function (e) {
                    e.stopPropagation();
                    if (self.atUserView.is(":visible")) {
                        self.atUserView.hide();
                    }

                });
                self.atUserView.click(function (e) {
                    e.stopPropagation();
                });

                //搜索框上下enter键盘事件
                self.searchUser.on("keydown", function (e) {
                    self._onkeydown(e);

                });
                //绑定搜索事件
                self.searchUser.on("keyup", function (e) {
                    if (e.keyCode != 38 && e.keyCode != 40) {
                        self.keyword = $.trim($(this).val());
                        var searchedData = self._search(self.keyword, self.data);
                        self._display(searchedData.slice(0, 8));
                    }

                });
                //关闭按钮
                self.close.on("click", function (e) {
                    self.atUserView.hide();
                });

                $(document).keypress(function (e) {
                    if (e.keyCode == 27) {
                        self.atUserView.hide();
                    }
                });
                //鼠标划过或点击事件
                self.userList.on('mouseenter', 'li', function (e) {
                    self.userList.find('.tn-bg-gray').removeClass('tn-bg-gray');
                    $(e.currentTarget).addClass('tn-bg-gray');
                }).on('click', function (e) {
                    self._choose();
                });

            },

            //搜索
            _search: function (keyword, data) {
                var items;
                if ($.isArray(data) && data.length > 0) {
                    items = $.map(data, function (item, i) {
                        var match, name, regexp;
                        try {
                            name = $.isPlainObject(item) ? item["name"] : item;
                            regexp = new RegExp(keyword.replace("+", "\\+"), 'i');
                            match = name.match(regexp);
                        } catch (e) {
                            return null;
                        }

                        if (match) {
                            return item;
                        } else {
                            return null;
                        }
                    });
                }
                return items;
            },

            //展示弹出层
            _display: function (data) {
                var self = this;
                var tpl = "<li data-value='${name}'>${name}</li>";
                var $ul = self.userList;
                self._clear();
                $.each(data, function (i, item) {
                    var li = self._evalTpl(tpl, item);
                    return $ul.append(self._highlight(li, self.keyword));
                });
                var position = self.selector.parent().position();
                self.atUserView.css({ top: position.top + 15, left: position.left }).show();
                $ul.find("li:first").addClass("tn-bg-gray");
            },
            //清空userList内容
            _clear: function () {
                var self = this;
                return self.userList.empty();
            },
            //选中li
            _choose: function () {
                var self = this;
                var $li = self.userList.find(".tn-bg-gray");
                if ($li.attr("data-value")) {
                    var str = $li.attr("data-value") + " ";
                    self._replaceStr(str);
                }
                else {
                    var $inputor = $(self.textareaId);
                    $inputor.focus();
                    self.atUserView.hide();
                }
            },
            //把选中的值显示在文本框中
            _replaceStr: function (str) {
                var self = this;
                var caret_pos = 0;
                var $inputor = $(self.textareaId);
                var arr = str.split('(');

                str = arr[0].toString() + " ";

                //获取光标，区分IE还是火狐
                if (document.selection) {
                    var end, endRange, len, normalizedValue, pos, range, start, textInputRange;
                    range = document.selection.createRange();
                    pos = 0;
                    if (range && range.parentElement() === $inputor) {
                        normalizedValue = $inputor.value.replace(/\r\n/g, "\n");
                        len = normalizedValue.length;
                        textInputRange = $inputor.createTextRange();
                        textInputRange.moveToBookmark(range.getBookmark());
                        endRange = $inputor.createTextRange();
                        endRange.collapse(false);

                        if (textInputRange.compareEndPoints("StartToEnd", endRange) > -1) {
                            start = end = len;
                        } else {

                            start = -textInputRange.moveStart("character", -len);
                            end = -textInputRange.moveEnd("character", -len);
                        }
                    }

                    caret_pos = start;

                } else {
                    caret_pos = $inputor.caretPos();
                }

                //区分html编辑器还是微博发表弹出框

                if ($inputor.tinymce) {

                    tinyMCE.activeEditor.selection.moveToBookmark(tinyMCE.activeEditor.windowManager.bookmark);
                    tinyMCE.activeEditor.execCommand('mceInsertContent', false, "<span>@" + str + "<span>");
                    tinyMCE.activeEditor.windowManager.bookmark = tinyMCE.activeEditor.selection.getBookmark(1);

                } else {
                    var source = $inputor.val();
                    var start_str = source.slice(0, caret_pos);
                    //区分IE还是火狐
                    if (document.selection) {
                        var start_strIE = source.slice(0, caret_pos);
                        var text = start_strIE + "@" + str;
                        $inputor.val(text);
                    } else {
                        var text = start_str + "@" + str + source.substr(caret_pos, source.length);
                        $inputor.val(text);
                        $inputor.caretPos(start_str.length + str.length + 1);
                    }
                }

                $inputor.change();
                self.atUserView.hide();
            },
            //模版替换
            _evalTpl: function (tpl, map) {
                var el;
                try {
                    return el = tpl.replace(/\$\{([^\}]*)\}/g, function (tag, key, pos) {
                        return map[key];
                    });
                } catch (error) {
                    return "";
                }
            },
            //搜索框绑定上移下移回车键盘事件
            _onkeydown: function (e) {
                var self = this;
                switch (e.keyCode) {
                    case 38:
                        e.preventDefault();
                        self.prev();
                        break;
                    case 40:
                        e.preventDefault();
                        self.next();
                        break;
                    case 13:
                        e.preventDefault();
                        self._choose();
                        break;

                }
                return false;
            },
            //上移
            prev: function () {
                var self = this;
                var cur = self.userList.find('.tn-bg-gray').removeClass('tn-bg-gray');
                var prev = cur.prev();

                if (!prev.length) {
                    prev = self.userList.find('li:last');
                }
                prev.addClass('tn-bg-gray');
            },
            //下移
            next: function () {
                var self = this;
                var cur = self.userList.find('.tn-bg-gray').removeClass('tn-bg-gray');
                var next = cur.next();

                if (!next.length) {
                    next = self.userList.find('li:first');
                }
                next.addClass('tn-bg-gray');
            },
            //搜索高亮
            _highlight: function (li, query) {
                if (query == "") {
                    return li;
                }
                return li.replace(new RegExp(">\\s*(\\w*)(" + query.replace("+", "\\+") + ")(\\w*)\\s*<", 'ig'), function (str, $1, $2, $3) {
                    return '> ' + $1 + '<strong>' + $2 + '</strong>' + $3 + ' <';
                });
            }
        }
        //插件入口
        $.fn.atUser = function (data, textareaId, randomNum) {
            var selector = this;
            var atUser = new AtUser(data, textareaId, randomNum, selector);

        };

    })(jQuery)
}).call(this);