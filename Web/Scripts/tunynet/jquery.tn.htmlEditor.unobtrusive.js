
(function ($) {
    //简单模式，若需为该模式增删插件或调整展示按钮，则可以修改该选项，下面的标准模式及高级模式类似
    var simpleOptions = {
        plugins: "paste,fullscreen,media,contextmenu,inlinepopups,changemode,syntaxhl,table",
        theme_advanced_buttons1: 'bold,italic,|,bullist,numlist|,forecolor,backcolor,|,link,unlink',
        theme_advanced_buttons2: '',
        theme_advanced_buttons3: ''
    };
    //标准模式
    var standardOptions = {
        plugins: 'paste,fullscreen,media,contextmenu,inlinepopups,syntaxhl,table',
        theme_advanced_buttons1: 'fontselect,fontsizeselect,|,forecolor,backcolor,|,undo,redo,|,pasteword',
        theme_advanced_buttons2: 'bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,indent,outdent,|,link,unlink,anchor,|,bullist,numlist,|,sub,sup,|,removeformat,table,media,syntaxhl,|,code,fullscreen',
        theme_advanced_buttons3: ''
    };
    //高级模式
    //    var enhancedOptions = {
    //        plugins: 'paste,fullscreen,style,media,contextmenu,inlinepopups,changemode,syntaxhl,table',
    //        theme_advanced_buttons1: 'fontselect,fontsizeselect,|,forecolor,backcolor,|,styleprops,removeformat,cleanup,|,table,|,undo,redo,|,cut,copy,pastetext,pasteword,|,code,|,fullscreen,changemode',
    //        theme_advanced_buttons2: 'bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,indent,outdent,|,link,unlink,anchor,|,bullist,numlist,|,sub,sup,|,hr,charmap,image,media,|,syntaxhl',
    //        theme_advanced_buttons3: ''
    //    };
    var enhancedOptions = {
        plugins: 'paste,fullscreen,media,contextmenu,inlinepopups,changemode,syntaxhl,table',
        theme_advanced_buttons1: 'fontselect,fontsizeselect,|,forecolor,backcolor,|,undo,redo,|,pasteword',
        theme_advanced_buttons2: 'bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,indent,outdent,|,link,unlink,anchor,|,bullist,numlist,|,sub,sup,|,removeformat,table,media,syntaxhl,|,code,fullscreen,|,changemode',
        theme_advanced_buttons3: ''
    };


    //扩展插入编辑器内容的快捷方法
    //example: $("#textareaId").insertContentToEditor("<p>Hello,world!</p>");
    $.fn.insertContentToEditor = function (content) {
        var tinymceObj = $(this).tinymce();
        tinyMCE.activeEditor.selection.moveToBookmark(tinyMCE.activeEditor.windowManager.bookmark);
        tinyMCE.activeEditor.windowManager.bookmark = 0;
        tinyMCE.activeEditor.execCommand('mceInsertContent', false, content);
        tinyMCE.activeEditor.focus();
        return this;
    };

    $(document).ready(function () {
        var commonOptions = {
            script_url: '../scripts/tinymce/tiny_mce.js',
            language: "zh-cn",
            // General options
            theme: "advanced",
            elements: 'nourlconvert',
            relative_urls: false,
            convert_urls: false,
            theme_advanced_toolbar_location: 'top',
            theme_advanced_toolbar_align: 'left',
            theme_advanced_statusbar_location: 'bottom',
            theme_advanced_resizing: true,
            theme_advanced_resize_horizontal: false
        };

        //绑定tinymce插件
        $('textarea[plugin="tinymce"]').livequery(function () {
            var data = $.parseJSON($(this).attr("data"));
            if (data == null)
                return;
            var options = {};
            var customButtons = $.parseJSON($(this).attr("customButtons"));
            if (customButtons == null)
                customButtons = {};
            var themeButtons1_add = "";

            if (customButtons.smileyButton)
                themeButtons1_add += ",|,smileybtn";
            if (customButtons.musicButton)
                themeButtons1_add += ",|,musicbtn";
            if (customButtons.videoButton)
                themeButtons1_add += ",|,videobtn";
            if (customButtons.photoButton)
                themeButtons1_add += ",|,photobtn";
            if (customButtons.fileButton)
                themeButtons1_add += ",|,filebtn";
            if (customButtons.atuserButton)
                themeButtons1_add += ",|,atuserbtn";

            $.extend(data, {
                theme_advanced_buttons1_add: themeButtons1_add,
                setup: function (ed) {
                    transparentImageUrl = ed.baseURI.toAbsolute("css/transparent.png");
                    if (customButtons.smileyButton) {
                        // Add a custom button
                        ed.addButton('smileybtn', {
                            title: '表情',
                            onclick: function () {
                                tinyMCE.activeEditor.focus();
                                tinyMCE.activeEditor.windowManager.bookmark = tinyMCE.activeEditor.selection.getBookmark(1);

                                if ($('#emotionSelector').length == 0) {
                                    $.get(customButtons.smileyButton, function (data) {
                                        $('body').append(data);
                                        $('a[id$=_smileybtn]').click();
                                    });
                                }
                            }
                        });
                    }
                    if (customButtons.videoButton)
                        ed.addButton('videobtn', {
                            title: '视频',
                            image: transparentImageUrl,
                            onclick: function () {
                                tinyMCE.activeEditor.focus();
                                tinyMCE.activeEditor.windowManager.bookmark = tinyMCE.activeEditor.selection.getBookmark(1);

                                return false;
                            }
                        });
                    if (customButtons.musicButton)
                        ed.addButton('musicbtn', {
                            title: '音乐',
                            image: transparentImageUrl,
                            onclick: function () {
                                tinyMCE.activeEditor.focus();
                                tinyMCE.activeEditor.windowManager.bookmark = tinyMCE.activeEditor.selection.getBookmark(1);
                                return false;
                            }

                        });
                    if (customButtons.photoButton)
                        ed.addButton('photobtn', {
                            title: '图片',
                            image: transparentImageUrl,
                            onclick: function () {
                                ed.focus();
                                var dialog = art.dialog({ id: ed.id + "_uploadPhotoDialog", title: false });

                                $.get(customButtons.photoButton, function (html) {
                                    dialog.content(html);
                                });
                            }
                        });
                    if (customButtons.fileButton)
                        ed.addButton('filebtn', {
                            title: '文件',
                            image: transparentImageUrl,
                            onclick: function () {
                                ed.focus();
                                var dialog = art.dialog({ id: ed.id + "_uploadFileDialog", title: false });
                                $.get(customButtons.fileButton, function (html) {
                                    dialog.content(html);
                                });
                            }
                        });
                    if (customButtons.atuserButton)
                        ed.addButton('atuserbtn', {
                            title: '@朋友',
                            image: transparentImageUrl,
                            onclick: function () {
                                tinyMCE.activeEditor.focus();
                                tinyMCE.activeEditor.windowManager.bookmark = tinyMCE.activeEditor.selection.getBookmark(1);

                                $.get(customButtons.atuserButton, { textareaId: ed.id, seletorId: ed.id + "_atuserbtn" }, function (html) {
                                    if ($("div[id^=atUserView]").length > 0) {
                                        return;
                                    } else {
                                        $("body").append(html);
                                    }

                                });
                            }
                        });
                }
            });
            $.extend(options, commonOptions);

            $.extend(options, data);
            if (data.editorMode == "Enhanced") {
                $.extend(options, enhancedOptions);
                $(this).data("ChangeMode_IsFull", true);
            }
            else if (data.editorMode == "Standard") {
                $.extend(options, standardOptions);
                $(this).data("ChangeMode_IsFull", false);
            }
            else
                $.extend(options, simpleOptions);

            //延迟加载，textArea获取焦点时，再初始化编辑器
            var tinymceLazyloadCookieName = "tn_tinymceLazyload_" + location.pathname;
            if (data.lazyload && !$.cookie(tinymceLazyloadCookieName))
                $(this).focus(function () {
                    $(this).tinymce(options);
                    $.cookie(tinymceLazyloadCookieName, true);
                });
            else
                $(this).tinymce(options);

            //切换简洁/高级功能
            function tinymceChangeMode(ed) {
                tinyMCE.execCommand('mceRemoveControl', false, ed.id);
                var $tinyMCE = $("#" + ed.id);
                if ($tinyMCE.data("ChangeMode_IsFull")) {
                    $tinyMCE.data("ChangeMode_IsFull", false);
                    $tinyMCE.tinymce($.extend(commonOptions, simpleOptions, data, { theme_advanced_buttons1_add: data.theme_advanced_buttons1_add + ",|,changemode" }));
                } else {
                    $tinyMCE.data("ChangeMode_IsFull", true);
                    $tinyMCE.tinymce($.extend(commonOptions, enhancedOptions, data));
                }
                tinyMCE.execCommand('mceAddControl', false, ed.id);
            };
            $(this).data("ChangeModeFunctionScript", tinymceChangeMode);
            $('form:has(textarea[plugin="tinymce"])').find(":submit,:button").click(function () {
                tinyMCE.triggerSave();
            });
        });
    });
})(jQuery);