/*
* AutoSuggest
* Copyright 2009-2010 Drew Wilson
* www.drewwilson.com
* code.drewwilson.com/entry/autosuggest-jquery-plugin
*
* Version 1.4   -   Updated: Mar. 23, 2010
*
* This Plug-In will auto-complete or auto-suggest completed search queries
* for you as you type. You can add multiple selections and remove them on
* the fly. It supports keybord navigation (UP + DOWN + RETURN), as well
* as multiple AutoSuggest fields on the same page.
*
* Inspied by the Autocomplete plugin by: J歳n Zaefferer
* and the Facelist plugin by: Ian Tearle (iantearle.com)
*
* This AutoSuggest jQuery plug-in is dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/

(function ($) {
    $.fn.autoSuggest = function (data, options) {
        var defaults = {
            asHtmlID: false,
            startText: "请在这里输入",
            emptyText: "未搜索到结果",
            preFill: "",
            selectedItemProp: "value", //name of object property
            selectedValuesProp: "value", //name of object property
            searchObjProps: "value", //comma separated list of object property names
            queryParam: "q",
            retrieveLimit: false, //number for 'limit' param on ajax request
            extraParams: {},
            matchCase: false,
            minChars: 1,
            keyDelay: 400,
            resultsHighlight: true,
            neverSubmit: false,
            selectionLimit: false,
            showResultList: true,
            start: function () { },
            selectionClick: function (elem) { },
            selectionAdded: function (elem) { },
            selectionRemoved: function (elem) { elem.remove(); },
            formatList: false, //callback function
            formatExpandList: false, //callback function
            beforeRetrieve: function (string) { return string; },
            retrieveComplete: function (data) { return data; },
            resultClick: function (data) { },
            resultsComplete: function () { },
            enableManualAdd: false, //是否允许手动录入数据
            allSelectData: {},
            categoryData: {},
            defaultCategory: "所有类别",
            limitTextPattern: "最多只能添加{0}个",
            remainTextPattern: "您还可以选择{0}个",
            widthClass: "tn-longer"
        };

        var opts = $.extend(defaults, options || {});

        var d_type = "object";
        var d_count = 0;
        if (typeof data == "string") {
            d_type = "string";
            var req_string = data;
        } else {
            var org_data = data;
            for (k in data) if (data.hasOwnProperty(k)) d_count++;
        }
        if ((d_type == "object" && d_count > 0) || d_type == "string") {
            return this.each(function (x) {
                if (!opts.asHtmlID) {
                    x = x + "" + Math.floor(Math.random() * 100); //this ensures there will be unique IDs on the page if autoSuggest() is called multiple times
                    var x_id = "as-input-" + x;
                } else {
                    x = opts.asHtmlID;
                    var x_id = x;
                }
                opts.start.call(this);
                var input = $(this);
                input.attr("autocomplete", "off").addClass("as-input tn-input-medium").attr("id", x_id).val(opts.startText);
                input.parents('form:first').submit(function () {
                    if (input.val() == opts.startText)
                        input.val('');
                });
                if (opts.enableManualAdd)
                    input.attr("name", x);
                var input_focus = false;

                // Setup basic elements and render them to the DOM
                input.wrap('<ul class="as-selections " id="as-selections-' + x + '"></ul>').wrap('<li class="as-original" id="as-original-' + x + '"></li>');
                input.parents("ul#as-selections-" + x).wrap("<div class=\"tn-expand-select tn-widget-content tn-border-gray " + opts.widthClass + "\"></div>");
                var selections_holder = $("#as-selections-" + x);
                var org_li = $("#as-original-" + x);
                var results_holder = $('<div class="as-results" id="as-results-' + x + '"></div>').hide();
                var results_ul = $('<ul class="as-list ui-autocomplete ui-menu"></ul>');
                var values_input = $('<input type="hidden" class="as-values" name="' + x + '" id="as-values-' + x + '" />');
                var prefill_value = "";

                if (typeof opts.preFill == "string") {
                    var vals = opts.preFill.split(",");
                    for (var i = 0; i < vals.length; i++) {
                        var v_data = {};
                        v_data[opts.selectedValuesProp] = vals[i];
                        if (vals[i] != "") {
                            add_selected_item(v_data, "000" + i);
                        }
                    }
                    prefill_value = opts.preFill;
                } else {
                    prefill_value = "";
                    var prefill_count = 0;
                    for (k in opts.preFill) if (opts.preFill.hasOwnProperty(k)) prefill_count++;
                    if (prefill_count > 0) {
                        for (var i = 0; i < prefill_count; i++) {
                            var new_v = opts.preFill[i][opts.selectedValuesProp];
                            if (new_v == undefined) { new_v = ""; }
                            prefill_value = prefill_value + new_v + ",";
                            if (new_v != "") {
                                add_selected_item(opts.preFill[i], "000" + i);
                            }
                        }
                    }
                }
                if (prefill_value != "") {
                    input.val("");
                    var lastChar = prefill_value.substring(prefill_value.length - 1);
                    if (lastChar != ",") { prefill_value = prefill_value + ","; }
                    values_input.val("," + prefill_value);
                    $("li.as-selection-item", selections_holder).addClass("blur tn-bg-gray tn-border-gray").removeClass("selected tn-bg-deep tn-border-deep");
                }
                input.after(values_input);
                selections_holder.click(function () {
                    input_focus = true;
                    input.focus();
                }).mousedown(function () { input_focus = false; })
                .parents(".tn-expand-select").after(results_holder);

                if (opts.allSelectData && opts.allSelectData.length > 0) {
                    //add select list
                    selections_holder
                    // .wrap("<div class=\"tn-expand-select tn-widget tn-widget-content tn-border-gray tn-bg-gray\"></div>")
                    .after("<a class=\"tn-icon tn-smallicon-collapse-open\" href=\"javascript:void(0)\"></a>")
                    .parents(".tn-expand-select").after("<div class=\"tn-expand-select-box tn-widget\" id=\"as-allselect-" + x + "\" ></div>");
                    var allselect_holder = $("#as-allselect-" + x);
                    var switchButton = selections_holder.next("a.tn-icon");
                    switchButton.click(function () {
                        if (allselect_holder.is(":hidden")) {
                            switchButton.removeClass("tn-smallicon-collapse-open").addClass("tn-smallicon-collapse-close");
                            allselect_holder.show();
                            //重置剩余个数
                            if (opts.selectionLimit)
                                $("#as-remainCount-" + x).text(eval(opts.selectionLimit - $("li.as-selection-item", selections_holder).length));
                            $(document).bind("click", function (e) {
                                if ($(e.target).is("#as-allselect-" + x + " *")) {
                                    return;
                                }
                                $(document).unbind("click", arguments.callee);
                                switchButton.removeClass("tn-smallicon-collapse-close").addClass("tn-smallicon-collapse-open");
                                allselect_holder.hide();
                            });
                        }
                        else {
                            switchButton.removeClass("tn-smallicon-collapse-close").addClass("tn-smallicon-collapse-open");
                            allselect_holder.hide();
                        }
                        return false;
                    });
                    //添加头部
                    if (opts.selectionLimit || opts.categoryData && opts.categoryData.length > 0) {
                        allselect_holder.append("<div class=\"tn-expand-select-head tn-widget-content tn-bg-gray tn-border-gray tn-border-rbl\"></div>");

                        if (opts.categoryData && opts.categoryData.length > 0) {
                            if (opts.selectionLimit && opts.widthClass != "tn-short" && opts.widthClass != "tn-medium")
                                allselect_holder.find("div.tn-expand-select-head").append("<span class=\"tn-helper-left\">"
                            + $.validator.format(opts.remainTextPattern, "<strong id=\"as-remainCount-" + x + "\">" + opts.selectionLimit + "</strong>")
                            + "</span>");
                            allselect_holder.find("div.tn-expand-select-head").append("<select id=\"as-category-" + x + "\"></select>");
                        }
                        else {
                            if (opts.selectionLimit)
                                allselect_holder.find("div.tn-expand-select-head").append(
                            $.validator.format(opts.remainTextPattern, "<strong id=\"as-remainCount-" + x + "\">" + opts.selectionLimit.toString() + "</strong>"));
                        }
                    }
                    //添加内容主体
                    allselect_holder.append("<div class=\"tn-expand-select-list tn-widget-content tn-border-gray tn-border-lr\">"
                          + "<ul class=\"tn-helper-reset tn-helper-clearfix\" id=\"as-selectlist-" + x + "\">"
                            + "</ul>"
                        + "</div>");
                    //添加尾部
                    allselect_holder.append("<div class=\"tn-expand-select-foot tn-helper-clearfix tn-widget-content tn-border-gray\">"
                    + "<a href=\"javascript:void(0)\" class=\"tn-button tn-corner-all tn-button-text-only tn-button-secondary\"><span class=\"tn-button-text\">关闭</span></a>").hide();

                    //处理类别数据
                    if (opts.categoryData && opts.categoryData.length > 0) {
                        var $category = $("select#as-category-" + x);
                        function processCategoryData(data) {
                            $category.html("");
                            if (opts.defaultCategory)
                                $category.append("<option>" + opts.defaultCategory + "</option>");
                            $(data).each(function () {
                                $category.append("<option value=\"" + this.value + "\">" + this.name + "</option>");
                            });
                        }
                        //异步加载类别
                        if (typeof opts.categoryData == "string") {
                            $category.html("<option>loading...</option>");
                            $.getJSON(opts.categoryData, function (data) {
                                processCategoryData(data);
                            });
                        }
                        else {
                            processCategoryData(opts.categoryData);
                        }

                    }
                    //处理列表数据
                    var $selectlist = $("ul#as-selectlist-" + x);
                    function processAllSelectData(data) {
                        $selectlist.html("");
                        if (data.length == 0)
                            $selectlist.html("<div class=\"tn-no-data\">暂无数据</div>");
                        $(data).each(function (i) {
                            //  if (!opts.formatExpandList) {
                            //         formatted = formatted.html(this_data[opts.selectedItemProp]);
                            //     } else {
                            //         formatted = opts.formatExpandList.call(this, this_data, formatted);
                            //     }                         
                            var avatarHtml = "";
                            if (this.userAvatarUrl && this.userAvatarUrl.length > 0)
                                avatarHtml = "<div class=\"tn-avatar-mini\"><img src=\"" + this.userAvatarUrl + "\"></div>";
                            var text = "";
                            if (this[opts.selectedItemProp])
                                text = this[opts.selectedItemProp];
                            else
                                text = this;
                            var $li = $("<li class=\"tn-expand-select-item tn-widget-content tn-widget-same\" plugin=\"tipsy\" original-title=\"" + text + "\">"
                             + avatarHtml
                             + "<div class=\"tn-user-name-info\">" + text + "</div>"
                            + "</li>")
                        .data("data", { attributes: this, num: i });
                            $selectlist.append($li);
                        });



                        //菜单项鼠标效果
                        allselect_holder.find("div.tn-expand-select-list").find(".tn-expand-select-item").hover(
		                function () {
		                    $(this).addClass("tn-bg-gray tn-border-gray");
		                },
		                function () {
		                    $(this).removeClass("tn-bg-gray tn-border-gray");
		                }
	                    );
                    }
                    if (typeof opts.allSelectData == "object") {
                        allselectoptionValues = processAllSelectData(opts.allSelectData);
                    }
                    if (typeof opts.allSelectData == "string") {
                        $selectlist.html("<div class=\"tn-loading\"></div>");
                        $.getJSON(opts.allSelectData, function (data) {
                            processAllSelectData(data);
                        });
                        var $category = $("select#as-category-" + x);
                        if ($category.length)
                            $category.change(function () {
                                $selectlist.html("<div class=\"tn-loading\"></div>");
                                $.getJSON(opts.allSelectData, { categoryId: $(this).val() }, function (data) {
                                    processAllSelectData(data);
                                });
                            });
                    }
                    else {
                        processAllSelectData(opts.allSelectData);
                    }
                    //菜单项点击事件
                    allselect_holder.find("div.tn-expand-select-list").find("li.tn-expand-select-item").live("click",
                    function () {
                        //检查是否超出限制                       
                        if (opts.selectionLimit && $("li.as-selection-item", selections_holder).length >= opts.selectionLimit) {
                            alert($.validator.format(opts.limitTextPattern, opts.selectionLimit));
                            return false;
                        }

                        var raw_data = $(this).data("data");
                        var number = "0" + raw_data.num;
                        var data = raw_data.attributes;
                        add_selected_item(data, number);
                        // if (values_input.val().indexOf(data[opts.selectedValuesProp]) >= 0)
                        //     $(this).addClass("tn-selected tn-bg-deep tn-border-deep");
                        // else
                        //     $(this).removeClass("tn-selected tn-bg-deep tn-border-deep");
                        input.val("");
                        //重置剩余个数
                        if (opts.selectionLimit)
                            $("#as-remainCount-" + x).text(eval(opts.selectionLimit - $("li.as-selection-item", selections_holder).length));

                    });
                    //关闭按钮
                    allselect_holder.find("div.tn-expand-select-foot").find("a.tn-button").click(function () {
                        input.val("").focus();
                        switchButton.click();
                    });
                }
                var timeout = null;
                var prev = "";
                var totalSelections = 0;
                var tab_press = false;

                // Handle input field events
                input.focus(function () {
                    if ($(this).val() == opts.startText && values_input.val() == "") {
                        $(this).val("");
                    } else if (input_focus) {
                        $("li.as-selection-item", selections_holder).removeClass("blur tn-bg-gray tn-border-gray");
                        if ($(this).val() != "") {
                            results_ul.css("width", selections_holder.outerWidth());
                            results_holder.show();
                        }
                    }
                    input_focus = true;
                    return true;
                }).blur(function () {
                    if ($(this).val() == "" && values_input.val() == "" && prefill_value == "") {
                        $(this).val(opts.startText);
                    } else if (input_focus) {
                        $("li.as-selection-item", selections_holder).addClass("blur tn-bg-gray tn-border-gray").removeClass("selected tn-bg-deep tn-border-deep");
                        results_holder.hide();
                    }
                }).keydown(function (e) {
                    // track last key pressed
                    lastKeyPressCode = e.keyCode;
                    first_focus = false;
                    switch (e.keyCode) {
                        case 38: // up
                            e.preventDefault();
                            moveSelection("up");
                            break;
                        case 40: // down
                            e.preventDefault();
                            moveSelection("down");
                            break;
                        case 8:  // delete
                            if (input.val() == "") {
                                var last = values_input.val().split(",");
                                last = last[last.length - 2];
                                selections_holder.children().not(org_li.prev()).removeClass("selected tn-bg-deep tn-border-deep");
                                if (org_li.prev().hasClass("selected")) {
                                    values_input.val(values_input.val().replace(last + ",", ""));
                                    opts.selectionRemoved.call(this, org_li.prev());
                                } else {
                                    opts.selectionClick.call(this, org_li.prev());
                                    org_li.prev().addClass("selected tn-bg-deep tn-border-deep");
                                }
                                //重置剩余个数
                                if (opts.selectionLimit)
                                    $("#as-remainCount-" + x).text(eval(opts.selectionLimit - $("li.as-selection-item", selections_holder).length));
                            }
                            if (input.val().length == 1) {
                                results_holder.hide();
                                prev = "";
                            }
                            if ($(":visible", results_holder).length > 0) {
                                if (timeout) { clearTimeout(timeout); }
                                timeout = setTimeout(function () { keyChange(); }, opts.keyDelay);
                            }
                            break;
                        case 9: case 188: case 32:  // tab or comma 、space                                                        
                            //检查是否超出限制                            
                            if (opts.selectionLimit && $("li.as-selection-item", selections_holder).length >= opts.selectionLimit) {
                                break;
                            }

                            tab_press = true;
                            var i_input = $.trim(input.val()).replace(/(,)/g, "");
                            if (opts.enableManualAdd && i_input != "" && values_input.val().search("," + i_input + ",") < 0 && i_input.length >= opts.minChars) {
                                e.preventDefault();
                                var n_data = {};
                                n_data[opts.selectedItemProp] = i_input;
                                n_data[opts.selectedValuesProp] = i_input;
                                var lis = $("li", selections_holder).length;

                                add_selected_item(n_data, "00" + (lis + 1));
                                input.val("");
                            }
                        case 13: // return
                            tab_press = false;
                            var active = $("li.active:first", results_holder);
                            if (active.length > 0) {
                                active.click();
                                results_holder.hide();
                            }
                            if (opts.neverSubmit || active.length > 0) {
                                e.preventDefault();
                            }
                            break;
                        default:
                            if (opts.showResultList) {
                                if (opts.selectionLimit && $("li.as-selection-item", selections_holder).length >= opts.selectionLimit) {
                                    results_ul.html('<li class="as-message tn-widget tn-widget-content tn-border-gray tn-border-rbl">' + $.validator.format(opts.limitTextPattern, opts.selectionLimit) + '</li>');
                                    results_holder.show();
                                } else {
                                    if (timeout) { clearTimeout(timeout); }
                                    timeout = setTimeout(function () { keyChange(); }, opts.keyDelay);
                                }
                            }
                            break;
                    }
                }).bind("input", function () {
                    if (timeout) { clearTimeout(timeout); }
                    timeout = setTimeout(function () { keyChange(); }, opts.keyDelay);
                });

                function keyChange() {
                    // ignore if the following keys are pressed: [del] [shift] [capslock]
                    if (lastKeyPressCode == 46 || (lastKeyPressCode > 8 && lastKeyPressCode < 32)) { return results_holder.hide(); }
                    var string = input.val().replace(/[\\]+|[\/]+/g, "");
                    if (string == prev) return;
                    prev = string;
                    if (string.length >= opts.minChars) {

                        if (opts.selectionLimit && $("li.as-selection-item", selections_holder).length >= opts.selectionLimit) {
                            return;
                        }

                        selections_holder.addClass("loading");
                        if (d_type == "string") {
                            var req_data = {};
                            if (opts.retrieveLimit) {
                                req_data["limit"] = encodeURIComponent(opts.retrieveLimit);
                            }
                            if (opts.beforeRetrieve) {
                                string = opts.beforeRetrieve.call(this, string);
                            }
                            req_data[opts.queryParam] = encodeURIComponent(string);
                            $.extend(req_data, opts.extraParams);
                            $.getJSON(req_string, req_data, function (data) {
                                d_count = 0;
                                var new_data = opts.retrieveComplete.call(this, data);
                                for (k in new_data) if (new_data.hasOwnProperty(k)) d_count++;
                                processData(new_data, string);
                            });
                        } else {
                            if (opts.beforeRetrieve) {
                                string = opts.beforeRetrieve.call(this, string);
                            }
                            processData(org_data, string);
                        }
                    } else {
                        selections_holder.removeClass("loading");
                        results_holder.hide();
                    }
                }
                var num_count = 0;
                function processData(data, query) {
                    if (!opts.matchCase) { query = query.toLowerCase(); }
                    var matchCount = 0;
                    results_holder.html(results_ul.html("")).hide();
                    for (var i = 0; i < d_count; i++) {
                        var num = i;
                        num_count++;
                        var forward = false;
                        if (opts.searchObjProps == "value") {
                            var str = data[num].value;
                        } else {
                            var str = "";
                            var names = opts.searchObjProps.split(",");
                            for (var y = 0; y < names.length; y++) {
                                var name = $.trim(names[y]);
                                str = str + data[num][name] + " ";
                            }
                        }
                        if (str) {
                            if (!opts.matchCase) { str = str.toLowerCase(); }
                            if (str.search(query) != -1 && values_input.val().search("," + data[num][opts.selectedValuesProp] + ",") == -1) {
                                forward = true;
                            }
                        }
                        if (forward) {
                            var formatted = $('<li class="as-result-item tn-widget tn-widget-content tn-border-gray tn-border-rbl" id="as-result-item-' + num + '"></li>').click(function () {
                                var raw_data = $(this).data("data");
                                var number = raw_data.num;
                                if ($("#as-selection-" + number, selections_holder).length <= 0 && !tab_press) {
                                    var data = raw_data.attributes;
                                    input.val("").focus();
                                    prev = "";
                                    add_selected_item(data, number);
                                    opts.resultClick.call(this, raw_data);
                                    results_holder.hide();
                                }
                                tab_press = false;
                            }).mousedown(function () { input_focus = false; }).mouseover(function () {
                                $("li", results_ul).removeClass("active tn-bg-deep");
                                $(this).addClass("active tn-bg-deep");
                            }).data("data", { attributes: data[num], num: num_count });
                            var this_data = $.extend({}, data[num]);
                            if (!opts.matchCase) {
                                var regx = new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + query + ")(?![^<>]*>)(?![^&;]+;)", "gi");
                            } else {
                                var regx = new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + query + ")(?![^<>]*>)(?![^&;]+;)", "g");
                            }

                            if (opts.resultsHighlight) {
                                this_data[opts.selectedItemProp] = this_data[opts.selectedItemProp].replace(regx, "<em>$1</em>");
                            }
                            if (!opts.formatList) {
                                formatted = formatted.html(this_data[opts.selectedItemProp]);
                            } else {
                                formatted = opts.formatList.call(this, this_data, formatted);
                            }
                            results_ul.append(formatted);
                            delete this_data;
                            matchCount++;
                            if (opts.retrieveLimit && opts.retrieveLimit == matchCount) { break; }
                        }
                    }
                    selections_holder.removeClass("loading");
                    if (matchCount <= 0) {
                        results_ul.html('<li class="as-message tn-widget tn-widget-content tn-border-gray tn-border-rbl">' + opts.emptyText + '</li>');
                    }
                    results_ul.css("width", selections_holder.outerWidth());
                    results_holder.show();
                    opts.resultsComplete.call(this);
                }

                function add_selected_item(data, num) {
                    var value = data[opts.selectedValuesProp];
                    if (!value)
                        value = data;

                    if (values_input.val().indexOf(value) >= 0)
                        return;

                    values_input.val(values_input.val() + value + ",");
                    var item = $('<li class="as-selection-item tn-bg-light tn-border-light" id="as-selection-' + num + '"></li>').click(function () {
                        opts.selectionClick.call(this, $(this));
                        selections_holder.children().removeClass("selected tn-bg-deep tn-border-deep");
                        $(this).addClass("selected tn-bg-deep tn-border-deep");
                    }).mousedown(function () { input_focus = false; });
                    var close = $('<a class="as-close tn-text-note">&times;</a>').click(function () {
                        values_input.val(values_input.val().replace(value + ",", ""));
                        opts.selectionRemoved.call(this, item);
                        if (opts.selectionLimit == 1)
                            input.show();
                        input_focus = true;
                        input.focus();
                        //重置剩余个数
                        if (opts.selectionLimit)
                            $("#as-remainCount-" + x).text(eval(opts.selectionLimit - $("li.as-selection-item", selections_holder).length));

                        return false;
                    });
                    var html = data[opts.selectedItemProp];
                    if (!html)
                        html = data;
                    org_li.before(item.html(html.toString()).append(close));
                    opts.selectionAdded.call(this, org_li.prev());
                    if (opts.selectionLimit == 1)
                        input.hide();
                }

                function moveSelection(direction) {
                    if ($(":visible", results_holder).length > 0) {
                        var lis = $("li", results_holder);
                        if (direction == "down") {
                            var start = lis.eq(0);
                        } else {
                            var start = lis.filter(":last");
                        }
                        var active = $("li.active:first", results_holder);
                        if (active.length > 0) {
                            if (direction == "down") {
                                start = active.next();
                            } else {
                                start = active.prev();
                            }
                        }
                        lis.removeClass("active tn-bg-deep");
                        start.addClass("active tn-bg-deep");
                    }
                }

            });
        }
    }
})(jQuery);  	