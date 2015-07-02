/*
Purpose: Moves registration and additional registration number inputs to underneath
a selected radio button. This is so different hint text can be shown depending on the
radio button selected without changing visible text.

The inputs should be moved relative to their hint text which may or may not be present
in a panel (.panel-indent) div underneath the radio button input.

Optionally a <details> element may be present which is also moved to the panel identified
by its data-target attribute.

All hint elements will be moved back into the hidden panel when another radio button is
selected and the inputs will move to that radio buttons' panel instead.

The additional hint text is differentiated from the normal hint text by use of the
data-name attribute.

A non-Javascript hint should be provided for the registration number block which
has the class js-hidden.

Known Issues: 
    + If there is an additional registration number hint but no hint for the other input
      the hint will appear in the wrong place (under registration number).
*/

$(function () {

    $(".panel-indent").removeClass(".hide-for-initial-load");

    placeDetailsElementsInCorrectLocation();

    var businessTypeSelected = $(".business-type-radio-button").filter(":checked");
    if (businessTypeSelected.length) {
        // Fires the click event for a radio button if the page loads with one being selected already (e.g. an edit page).
        businessTypeSelected.triggerHandler("click");
    }
});

function placeDetailsElementsInCorrectLocation() {
    // The details elements are placed by the input for non-JavaScript users.
    // Once the page loads for JavaScript users we need to move the details to the correct location.
    var registrationBlock = $("#registration-block");
    var detailsElements = registrationBlock.find("details");
    detailsElements.appendTo($("#" + detailsElements.attr("data-target")));
}

$(".business-type-radio-button").click(function () {

    var panelIndentClass = ".panel-indent";
    var formGroupClass = ".form-group";

    // Find the div containing the registration number input and the form group inputs.
    var registrationBlock = $('#registration-block');

    if (!registrationBlock.length) {
        return;
    }

    var registrationblockRegistrationNumber = registrationBlock.children(formGroupClass).first();
    var registrationblockAdditionalRegistrationNumber = registrationBlock.children(formGroupClass).last();

    // Find the panel underneath the radio button label.
    var hiddenPanel = $(this).parent().next(panelIndentClass);

    // Work out if the registration block has previously been moved.
    var isRegistrationBlockCurrentlyNested = registrationBlock.parent(panelIndentClass).length;
    if (isRegistrationBlockCurrentlyNested) {
        registrationblockAdditionalRegistrationNumber.removeClass('margin-bottom-30');

        // Move the hint texts currently in the registration block back to their proper position.
        // This moves both of the form hints back to the parent panel since find returns an array of elements.
        var hintSpans = registrationBlock.find('.form-hint');
        hintSpans.prependTo(registrationBlock.parent(panelIndentClass));

        // Move any details sections back to the parent.
        registrationBlock.find("details").appendTo(registrationBlock.parent(panelIndentClass));
    }

    // Move the currently hidden registration number input to the correct location.
    $(registrationBlock).appendTo(hiddenPanel);

    // Move the span to the correct location.
    $(hiddenPanel).children('.form-hint').first()
        .insertAfter(registrationblockRegistrationNumber.children('.form-label').first());

    var detailsSection = hiddenPanel.children("details");
    if (detailsSection.length) {
        detailsSection.appendTo(registrationblockRegistrationNumber);
    }

    // Move any additional registration input hint text.
    var additionalInputHint = hiddenPanel.children(".form-hint").filter("span[data-name='additional-hint']");
    if (additionalInputHint.length) {
        additionalInputHint.insertAfter(registrationblockAdditionalRegistrationNumber.children('.form-label').first());
    }

    // If the form group isn't going to be the only element in the panel add a margin class.
    var registrationInputHasNeighbourFormGroup = registrationBlock.next().length;
    if (registrationInputHasNeighbourFormGroup) {
        registrationblockAdditionalRegistrationNumber.addClass('margin-bottom-30');
    }

    // Show the registration number input.
    // This comes last so users don't get any screen flicker.
    registrationBlock.removeClass('js-hidden');
});