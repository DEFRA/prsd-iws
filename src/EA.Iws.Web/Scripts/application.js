function ShowHideContent() {
    var self = this;


    self.escapeElementName = function (str) {
        result = str.replace('[', '\\[').replace(']', '\\]')
        return (result);
    };

    self.showHideRadioToggledContent = function () {
        $(".block-label input[type='radio']").each(function () {

            var $radio = $(this);
            var $radioGroupName = $radio.attr('name');
            var $radioLabel = $radio.parent('label');

            var dataTarget = $radioLabel.attr('data-target');

            // Add ARIA attributes

            // If the data-target attribute is defined
            if (dataTarget) {

                // Set aria-controls
                $radio.attr('aria-controls', dataTarget);

                $radio.on('click', function () {

                    // Select radio buttons in the same group
                    $radio.closest('form').find(".block-label input[name='" + self.escapeElementName($radioGroupName) + "']").each(function () {
                        var $this = $(this);

                        var groupDataTarget = $this.parent('label').attr('data-target');
                        var $groupDataTarget = $('#' + groupDataTarget);

                        // Hide toggled content
                        $groupDataTarget.hide();
                        // Set aria-expanded and aria-hidden for hidden content
                        $this.attr('aria-expanded', 'false');
                        $groupDataTarget.attr('aria-hidden', 'true');
                    });

                    var $dataTarget = $('#' + dataTarget);
                    $dataTarget.show();
                    // Set aria-expanded and aria-hidden for clicked radio
                    $radio.attr('aria-expanded', 'true');
                    $dataTarget.attr('aria-hidden', 'false');

                });

            } else {
                // If the data-target attribute is undefined for a radio button,
                // hide visible data-target content for radio buttons in the same group

                $radio.on('click', function () {

                    // Select radio buttons in the same group
                    $(".block-label input[name='" + self.escapeElementName($radioGroupName) + "']").each(function () {

                        var groupDataTarget = $(this).parent('label').attr('data-target');
                        var $groupDataTarget = $('#' + groupDataTarget);

                        // Hide toggled content
                        $groupDataTarget.hide();
                        // Set aria-expanded and aria-hidden for hidden content
                        $(this).attr('aria-expanded', 'false');
                        $groupDataTarget.attr('aria-hidden', 'true');
                    });

                });
            }

        });
    }
    self.showHideCheckboxToggledContent = function () {

        $(".block-label input[type='checkbox']").each(function () {

            var $checkbox = $(this);
            var $checkboxLabel = $(this).parent();

            var $dataTarget = $checkboxLabel.attr('data-target');

            // Add ARIA attributes

            // If the data-target attribute is defined
            if (typeof $dataTarget !== 'undefined' && $dataTarget !== false) {

                // Set aria-controls
                $checkbox.attr('aria-controls', $dataTarget);

                // Set aria-expanded and aria-hidden
                $checkbox.attr('aria-expanded', 'false');
                $('#' + $dataTarget).attr('aria-hidden', 'true');

                // For checkboxes revealing hidden content
                $checkbox.on('click', function () {

                    var state = $(this).attr('aria-expanded') === 'false' ? true : false;

                    // Toggle hidden content
                    $('#' + $dataTarget).toggle();

                    // Update aria-expanded and aria-hidden attributes
                    $(this).attr('aria-expanded', state);
                    $('#' + $dataTarget).attr('aria-hidden', !state);

                });
            }

        });
    }
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
    }).each(function() {
        var selectBox = $(this);
        $("a[href='#" + selectBox.attr("id") + "']").click(function () {
            selectBox.select2("focus");
        });
    });

    $("select[data-select-allow-clear='true']").select2({
        allowClear: true
    });

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