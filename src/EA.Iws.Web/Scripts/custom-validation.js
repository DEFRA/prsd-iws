﻿jQuery.validator.setDefaults({
    onfocusout: false,
    onkeyup: false,
    onclick: false,
    focusInvalid: false,
    // Create a custom error summary that will appear before the form
    showErrors: function (errorMap, errorList) {
        if (errorList.length !== 0) {

            var headerErrorCount = "";

            if (errorList.length === 1) {
                headerErrorCount = " 1 error";
            }
            else
            {
                headerErrorCount = errorList.length + " errors";
            }

            summary = "<h2 class='heading-medium error-summary-heading'>You have " + headerErrorCount + " on this page</h2>";

            $(".form-group").removeClass("error");

            //Generate our error summary list
            for (error in errorList) {
                summary += '<li><a href="#' + errorList[error].element.id + '">' + errorList[error].message + '</a></li>';

                // Allow form groups to opt-out of their parent being the one highlighted.
                if ($(errorList[error].element).parents(".form-group").filter("div [data-validation-behaviour='ignore-parent']").length) {
                    $(errorList[error].element).parents(".form-group").first().addClass("error");
                } else {
                    $(errorList[error].element).parents(".form-group").last().addClass("error");
                }
            }

            // Toggle classes on validation summary
            $('.error-summary-valid').addClass('error-summary-errors').removeClass('error-summary-valid');
                
            $('.error-summary-errors').addClass('error-summary');

            // Output our error summary and place it in the error container
            $('.error-summary-errors ul').html(summary);

            // Focus on first error link in the error container 
            $('.error-summary-errors a').first().focus();

            // Move the focus to the associated input when error message link is triggered.
            // A simple href anchor link doesnt seem to place focus inside the input
            $('.error-summary-errors a').click(function () {
                $($(this).attr('href')).focus();
                return false;
            });
        }
        this.defaultShowErrors();
    }
});