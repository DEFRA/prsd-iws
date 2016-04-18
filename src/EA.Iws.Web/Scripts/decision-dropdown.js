setDropdownRevealSections = function (dropdownId, panelClass) {
    if (dropdownId && dropdownId[0] !== "#") {
        dropdownId = "#" + dropdownId;
    }
    var selectedDropdownSelector = dropdownId + " :selected";
    var preLoad = true;
    $(dropdownId).change(function () {
        var selected = $(selectedDropdownSelector).val();

        $("." + panelClass).addClass("js-hidden");

        if (!preLoad) {
            $(".form-group").removeClass("error");
            $("span.field-validation-error").empty();
            var errorsSummary = $(".error-summary");
            errorsSummary.empty();
            errorsSummary.removeClass("error-summary");
            errorsSummary.addClass("error-summary-valid");
        }

        $("#status" + selected).removeClass("js-hidden");

        if (selected === "" || selected === undefined || selected === null) {
            $("#status").addClass("js-hidden");
        } else {
            $("#status").removeClass("js-hidden");
        }

        preLoad = false;
    });

    var currentlySelected = $(selectedDropdownSelector).text();

    if (currentlySelected !== "") {
        $(dropdownId).trigger("change");
    }
};