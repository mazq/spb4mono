/*
* Version: Beta 2
* Release: 2010-5-29
*/
(function($) {
    var settings;
    $.fn.sideMenu = function(callerSettings) {
        settings = $.extend({
    }, callerSettings || {});
    return this.each(
			function() {
			    $(this).children("li:has(ul)").children("a.tn-menu-text").click(function() {
			        if ($(this).siblings("ul").is(":hidden")) {
			            $(this).siblings("ul").show();
			            $(this).siblings("span").removeClass("tn-icon-expand").addClass("tn-icon-fold");
			        }
			        else {
			            $(this).siblings("ul").hide();
			            $(this).siblings("span").removeClass("tn-icon-fold").addClass("tn-icon-expand");
			        }
			    });
			});
};
})(jQuery);