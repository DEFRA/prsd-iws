$(function () {
    attachBlurAndShowValidations();
});

function expandSection(event) {
    var childRow = $(event.target).parent().next(".grid-row");
    toggleJsHidden(childRow);
}

function onSuccess(data) {
    var openSections = getOpenSectionIds();
    $('.ajax-target').html(data);
    attachBlurAndShowValidations();
    $(openSections).each(function () {
        $(this.toString()).next(".grid-row").removeClass("js-hidden");
    });
}

function onAdd(data) {
    onSuccess(data);
    $(".facility-header").last().next(".grid-row").removeClass("js-hidden");
}

function getOpenSectionIds() {
    var result = [];
    $(".form .grid-row")
        .not(".js-hidden")
        .prev("div")
        .each(function (i) { result[i] = "#" + $(this).attr("id") });
    return result;
}

function attachBlurAndShowValidations() {
    $(".business-name").on("blur", function () {
        var textToSet = "Unnamed facility";
        if ($(this).val().length) {
            textToSet = $(this).val();
        }
        $(this).parents(".grid-row")
            .first()
            .prev(".facility-header")
            .find("a")
            .text(textToSet);
    });

    $(".error").parents(".grid-row").first().removeClass(".js-hidden");
}

function setSite(event) {
    $(".hidden-actual-site").val(false);
    var id = "#is-actual-" + $(event.target).val();
    $(id).val(true);
}