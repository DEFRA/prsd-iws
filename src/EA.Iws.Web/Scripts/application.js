function select2Init() {
    $("select[data-select-box='true']").removeClass("form-control").select2({
        placeholder: "Please select...",
        sortResults: function (results, container, query) {
            return results.sort(function (a, b) {
                a = a.text.toLowerCase();
                b = b.text.toLowerCase();

                var ax = [], bx = [];

                a.replace(/(\d+)|(\D+)/g, function (_, $1, $2) { ax.push([$1 || Infinity, $2 || ""]) });
                b.replace(/(\d+)|(\D+)/g, function (_, $1, $2) { bx.push([$1 || Infinity, $2 || ""]) });

                while (ax.length && bx.length) {
                    var an = ax.shift();
                    var bn = bx.shift();
                    var nn = (an[0] - bn[0]) || an[1].localeCompare(bn[1]);
                    if (nn) return nn;
                }

                return ax.length - bx.length;
            });
        }
    }).each(function () {
        var selectBox = $(this);
        $("a[href='#" + selectBox.attr("id") + "']").click(function () {
            selectBox.select2("focus");
        });
    });

    $("select[data-select-allow-clear='true']").select2({
        allowClear: true
    });
}

$(document).ready(function () {

    // Turn off jQuery animation
    jQuery.fx.off = true;

    // Details/summary polyfill
    // See /javascripts/vendor/details.polyfill.js

    // Where .multiple-choice uses the data-target attribute
    // to toggle hidden content
    var showHideContent = new GOVUK.ShowHideContent();
    showHideContent.init();

    //Unhide if javascript is enabled
    $('.no-js-hidden').css('display', 'block');

    // Prevent double form submissions
    $(':submit').preventDoubleClick();

    $("input[data-type='date']").on('keyup', function (event) {
        if ($(this).val().length >= 2
           && ((event.keyCode >= 48 && event.keyCode <= 57)
          || (event.keyCode >= 96 && event.keyCode <= 105))) {
            $(this).parent(".form-group").next().find("input").focus();
        }
    });

    // Select lists
    select2Init();

    // Menu
    $("#accordion h3").click(function() {
        if ($(this).hasClass("current")) {
            $(".current").removeClass("current");
            $("#accordion ul ul").slideUp();
            return;
        }
        
        $(".current").removeClass("current");
        $(this).addClass('current');

        $("#accordion ul ul").slideUp();
        $(".current").siblings("ul").slideDown();
    });

    // Datable
    $("input[data-datable]").datable();
});

$(window).load(function () {
    // If there is an error summary, set focus to the summary
    if ($(".error-summary").length) {
        $(".error-summary").focus();
        $(".error-summary a").click(function (e) {
            e.preventDefault();
            var href = $(this).attr("href");
            $(href).focus();
        });
    }
    // Otherwise, set focus to the field with the error
    else {
        $(".error input:first").focus();
    }
});