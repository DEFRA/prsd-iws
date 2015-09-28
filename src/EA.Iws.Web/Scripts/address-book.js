/*
* Takes a prefix and json object, loops through the key-values and populates the inputs with the id:
* "Prefix_JsonKey"
* The input is populated with the value from the Json key-value pair.
* If the "Prefix_Key" input cannot be found it will search by id of "Value".
* This is used by Enum radio-buttons.
*/
$.deserializeIntoNamedInputs = function (prefix, json) {
    $.each(json, function (key, value) {
        var prefixedWithPropertyName = $("#" + prefix + "_" + key);
        if (prefixedWithPropertyName.length) {
            prefixedWithPropertyName.val(value);
        }
        else if ($("#" + value).length && $("#" + value).is(":radio")) {
            $("#" + value).attr('checked', true);
            $("#" + value).triggerHandler("click");
        }
    });
}

function getSearchUrlForAddressBook(url, term, type) {
    return url + "?term=" + term + "&type=" + type;
}

function autocompleteListEntriesForData(data) {
    return $.map(data, autocompleteHtmlForAddressBookRecord);
}

/*
* Sets the drop-down entry displayed on auto-complete to show the following:
* Business Name (bold)
* Address line 1, Postcode
* Hidden Json string
*/
function autocompleteHtmlForAddressBookRecord(addressBookRecord) {
    return "<span class='autocomplete-title'>" + addressBookRecord.BusinessData.Name + "</span>"
        + "<span class='autocomplete-line'>" + addressBookRecord.AddressData.StreetOrSuburb + ", "
        + addressBookRecord.AddressData.PostalCode + "</span>"
        + "<span class='autocomplete-data'>" + JSON.stringify(addressBookRecord) + "</span>";
}

/*
* Responds to the select event of the auto-complete list.
* Retrieves the content of the last item in the auto-complete box. This should be a span
* containing the serialized object. e.g.:
* <span class="autocomplete-data">{ myJsonData }</span>
* Then binds this data to input fields on the page.
*/
function selectAutocompleteData(event, ui) {
    try {
        var data = JSON.parse($(ui.item.value).last().html());

        // Do not put the html content in the box.
        $("#Business_Name").val(data.BusinessData.Name);

        $.deserializeIntoNamedInputs("Address", data.AddressData);
        $.deserializeIntoNamedInputs("Business", data.BusinessData);
        $.deserializeIntoNamedInputs("Contact", data.ContactData);
    } catch (e) {
        console.log("An error occurred obtaining the JSON from string");
    } finally {
        event.preventDefault ? event.preventDefault() : event.returnValue = false;
    }
}

$(function() {
    $("*[data-addressbook-url]").each(function() {
        var url = $(this).data("addressbook-url");
        var type = $(this).data("addressbook-type");
        $(this).autocomplete({
            html: true,
            source: function(request, response) {
                $.getJSON(getSearchUrlForAddressBook(url,
                        request.term,
                        type),
                    function() {}).done(function(data) {
                    response(autocompleteListEntriesForData(data));
                }).error(function() {
                    console.log("An error occurred retrieving address book entries");
                });
            },
            select: selectAutocompleteData
        });
    });
});
