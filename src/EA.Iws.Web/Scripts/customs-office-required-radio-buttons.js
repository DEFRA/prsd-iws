$(function () {

    $(".panel").removeClass("hide-for-initial-load");

    var customOfficeSelected = $(".customs-office-required-radio-button").filter(":checked");
    if (customOfficeSelected.length) {
        // Fires the click event for a radio button if the page loads with one being selected already (e.g. an edit page).
        customOfficeSelected.triggerHandler("click");
    }
});


$(".customs-office-required-radio-button").click(function () {
    var addressBlock = $('#other-description');

    addressBlock.removeClass('js-hidden');
});

$(".customs-office-not-required-radio-button").click(function () {
    var addressBlock = $('#other-description');

    addressBlock.addClass('js-hidden');
});