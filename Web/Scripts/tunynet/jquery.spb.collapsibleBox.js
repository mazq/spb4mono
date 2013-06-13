/*
* Version: Beta 2
* Release: 2010-5-29
*/
(function ($) {
    var settings;
    $.fn.collapsibleBox = function (callerSettings) {
        settings = $.extend({
            autoOpen: false,
            switchPosition: "left"
        }, callerSettings || {});
        return this.each(
			function () {
			    var $this = $(this);
			    if (settings.switchPosition == "right" || $this.is(".tn-widget"))
			        $this.children(".tn-collapsible-header").addClass("tn-switch-right").append("<span class=\"tn-icon tn-switch\"></span>");
			    else if (settings.switchPosition == "left")
			        $this.children(".tn-collapsible-header").addClass("tn-switch-left").append("<span class=\"tn-icon tn-switch\"></span>");

			    function openBox() {
			        $this.removeClass('tn-collapsible-closed').addClass("tn-collapsible-opened");
			        $this.children(".tn-collapsible-header").find('.tn-icon').removeClass('tn-smallicon-collapse-open').addClass('tn-smallicon-collapse-close');
			        $this.children(".tn-collapsible-content").show();
			    }
			    function closeBox() {
			        $this.removeClass('tn-collapsible-opened').addClass("tn-collapsible-closed");
			        $this.children(".tn-collapsible-header").find('.tn-icon').removeClass('tn-smallicon-collapse-close').addClass('tn-smallicon-collapse-open');
			        $this.children(".tn-collapsible-content").hide();
			    }
			    $(this).children(".tn-collapsible-header").click(function () {
			        if ($this.is('.tn-collapsible-closed')) {
			            openBox();
			        }
			        else {
			            closeBox();
			        }
			    });
			    //实现切换效果
			    if ($(this).is('.tn-collapsible-closed') || settings.autoOpen) {
			        closeBox();
			    }
			    else {
			        openBox();
			    }
			}
		);
    };
    $(function () { $(".tn-collapsible").collapsibleBox(); });
})(jQuery);