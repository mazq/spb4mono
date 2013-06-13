

//检测 微博输入框内容的长度是否在范围内部
var checkTextLength = function (objId, countSpan, publishButton) {

    var val = $("#" + objId).val();
    var parten = /^\s*$/;
    if (parten.test(val) && val.length > 0) {
        return false;
    }
    if (!countSpan)
        countSpan = "countSpan";
    if (!publishButton)
        publishButton = "publishButton";
    //计算 还剩余多少字可以填写
    var length = 140 - countTextLength(val);

    var $ps = $('#' + publishButton);
    var $count = $("#" + countSpan);
    if (length < 0) {
        length = -length;
        $count.html('已超出<strong class="tn-text-bright">' + length + '</strong>字');
        $ps.attr('class', 'tn-button tn-corner-all  tn-button-large tn-button-secondary');
        $ps.attr('disabled', 'disabled');
    }
    else {
        $count.html('还可以输入<strong>' + length + '</strong>字');
        if (length > 139) {
            $ps.attr('class', 'tn-button tn-corner-all  tn-button-large tn-button-secondary');
            $ps.attr('disabled', 'disabled');
        }
        else {
            $ps.attr('class', 'tn-button tn-corner-all tn-button-text-only tn-button-large tn-button-primary');
            $ps.removeAttr('disabled');
        }
    }
}

//计算 微博输入框内容的字数
var countTextLength = function (value) {
    if (!value)
        return 0;
    return parseInt((value.replace(/[^\x00-\xff]/g, "**").length + 1) / 2);
}

//显示 弹出的窗口
var showTipBlock = function (obj, loadUrl) {

    $('#tipContainerDiv').html('<br/><br/><div class="tn-loading"></div>');
    $('#tipContainerDiv').load(loadUrl);
    var position;
    //如果页面中有模式框则采用position定位，否则采用ofset定位
    if (obj.parents('td.aui_main').length > 0) {
        position = obj.parent().position();
    }
    else {
        position = obj.parent().offset();
    }

    $("#tipBlockDiv").attr("style", "display:inline; top:" + (position.top + 15) + "px; left:" + position.left + "px;");

    //绑定事件：点击弹出窗口以外任何地方，窗口关闭
    $(document).bind("click", function (e) {
        containerControl("tipBlockDiv", e);
        //若 窗口已经关闭，则移除事件绑定
        if ($("#tipBlockDiv").is(":hidden")) {
            $(document).unbind("click", arguments.callee);
        }
    });
}


//关闭 弹出的窗口
var closeTipBlock = function () {
    $('#tipBlockDiv').hide();
    clearContainer();
}

//清除 弹出框中的内容
var clearContainer = function () {
    $("#tipContainerDiv").html("");
}

//发布微博 表单提交成功 调用函数
var microblogCreateSuccess = function () {
    $('#msg-sussess').show();
    setTimeout(function () { $('#msg-sussess').hide(); }, 700);
    $("#microblogBody").val("");
}

//发布微博 表单提交失败 调用函数
var microblogCreateError = function () {
    alert("微博发布失败！");
}


//控制点击事件时 弹出模块 是否显示
//controlId：点击控制显示的元素的ID
//containerId：控制显示与否的容器
//e：触发事件
var containerControl = function (containerId, e) {

    debugger
    if (e.target.attributes.id == null) {

        //获取div左上角的Top,Left,Width,Height
        var microblogBody = document.getElementById(containerId);
        var divTop = microblogBody.offsetTop; //获取该元素对应父容器的上边距
        var divLeft = microblogBody.offsetLeft; //对应父容器的上边距
        var divWidth = $(microblogBody).width();
        var divHeight = $(microblogBody).height();

        //获取鼠标点击的：Top,Left
        e = e || window.event;
        var clickLeft = e.pageX || e.clientX + document.body.scroolLeft;
        var clickTop = e.pageY || e.clientY + document.body.scrollTop;

        //判读鼠标点击的区域，触发不同的方法
        if (!((clickLeft > divLeft && clickLeft < divLeft + divWidth) && (clickTop > divTop && clickTop < divTop + divHeight))) {
            closeTipBlock();
        }
    }
}


//topic插入操作调用方法
//microblogBody : textArea元素jquery对象
//topicName ： 要选中的topic的名称例如：#伦敦奥运会#
var newTopicClickEvent = function (microblogBodyId, topicName) {
    var microblogBodyVal = $("#" + microblogBodyId).val();

    if (microblogBodyVal.indexOf(topicName, 0) < 0) {
        //获取光标位置
        var mousePosition = $("#mousePositionInMicroblogBody").val();
        //在光标位置插入主题
        createNewTopic(microblogBodyId, topicName, mousePosition);


    }

    selectTextContent(microblogBodyId, topicName);
}

//将传入的topic 加到微博内容中去
var createNewTopic = function (microblogBodyId, topicName, mousePosition) {
    var microblogBody = $("#" + microblogBodyId).val();
    var frontString = microblogBody.substring(0, mousePosition);
    var backString = microblogBody.substring(mousePosition, microblogBody.length);

    $("#" + microblogBodyId).val(frontString + topicName + backString);
    $("#mousePositionInMicroblogBody").val(parseInt($("#mousePositionInMicroblogBody").val()) + topicName.length);
}

//将内容设置为选中状态
var selectTextContent = function (microblogBodyId, topicName) {

    var microblogBody = $("#" + microblogBodyId);

    var topicLength = topicName.length;
    var startIndex = microblogBody.val().indexOf(topicName, 0) + 1;
    var microblogBodyLength = microblogBody.val().length;

    if (document.createRange) {
        var textObj = microblogBody.get(0);
        textObj.focus();
        textObj.selectionStart = startIndex;
        textObj.selectionEnd = startIndex + topicLength - 2;
    }
    else {
        var microblogTextArea = document.getElementsByName("microblogBody")[0];
        var range = microblogTextArea.createTextRange();
        range.moveStart("character", startIndex);
        range.moveEnd("character", startIndex + topicLength - microblogBodyLength - 2);
        range.select();
    }
}

//获取鼠标在文本中的位置
var getMousePosition = function (microblogBodyId) {

    var microblogBody = document.getElementById(microblogBodyId)
    var result = 0;

    if (microblogBody.setSelectionRange) { //IE以外 
        result = microblogBody.selectionStart
    }
    else { //IE
        if (microblogBody.value != "") {
            var rng = event.srcElement.createTextRange();
            try {
                rng.moveToPoint(event.x, event.y);
            }
            catch (e) { }
            rng.moveStart("character", -event.srcElement.value.length);
            result = rng.text.length;
        }
        else {
            result = 0;
        }
    }

    if (result < 2) {
        result = $("#" + microblogBodyId).val().length;
    }

    //alert(result);
    $("#mousePositionInMicroblogBody").val(result.toString());
    //return result;
}






