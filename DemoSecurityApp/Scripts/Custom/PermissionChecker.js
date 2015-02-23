var allowedFeatureList = new Array();
var enablePermission = false;
/*Register ajax complete callback to apply 'Userpermissions' on ajax requests*/
/*Ajax OnComplete START*/


//For - Authorizing after ajax response
$(document).ajaxComplete(function (event, request, settings) {

    var allDataFeatureElements = $('[data-feature]');

    if (enablePermission == true) {
        $(allDataFeatureElements).each(function () {
            var currentfid = $(this).data("feature");
            //Check If the current data-feature element in Page lies in allowed Features list
            var result = $.inArray(currentfid, allowedFeatureList);
            if (result != -1) {
            }
            else {
                //$(this).addClass("noDisplay");

                //Remove element from DOM if not authorized to access it
                $(this).remove();
            }
        });
    }
});
/*Ajax OnComplete END*/


//For - Authorizing after page load
$(function () {
    //To disable caching of ajax requests in IE browsers
    $.ajaxSetup({
        cache: false
    });

    var allDataFeatureElements = $('[data-feature]');

    //Get allowed Features List for login user
    $.getJSON("/home/getAllAccessibleFeatureControlIDJSON", function (data) {
        enablePermission = data.enablePerm;
        if (data.enablePerm == true) {
            for (var i = 0; i < data.controlIDS.length; i++) {
                allowedFeatureList.push(data.controlIDS[i]);
            }
            $(allDataFeatureElements).each(function () {
                var currentfid = $(this).data("feature");
                var result = $.inArray(currentfid, allowedFeatureList);
                var $ele = $(this);

                if (result != -1) {
                    //if ($ele.is('span')) {
                    //$(this).removeClass("noDisplay").addClass("DisplayInline");
                    //}
                    //else
                    //{
                    //$(this).removeClass("noDisplay").addClass("Display");
                    //}
                }
                else {
                    //$(this).addClass("noDisplay");
                    $(this).remove();
                }
            });
        }
    });
});