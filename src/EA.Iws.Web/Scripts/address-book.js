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
            prefixedWithPropertyName.val(decodeEntities(value));
        }
        else if ($("#" + value).length && $("#" + value).is(":radio")) {
            $("#" + value).prop('checked', true);
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

function decodeEntities(encodedString) {
    if (encodedString === null) {
        return "";
    }
    var textArea = document.createElement('textarea');
    textArea.innerHTML = encodedString;
    return textArea.value;
}

/*
* Sets the drop-down entry displayed on auto-complete to show the following:
* Business Name (bold)
* Address line 1, Postcode (or Town/City if postcode is null)
* Hidden Json string
*/
function autocompleteHtmlForAddressBookRecord(addressBookRecord) {
    return "<span class='autocomplete-title'>" + addressBookRecord.BusinessData.Name + "</span>"
        + "<span class='autocomplete-line'>" + addressBookRecord.AddressData.StreetOrSuburb + ", "
        + ((addressBookRecord.AddressData.PostalCode) ? addressBookRecord.AddressData.PostalCode : addressBookRecord.AddressData.TownOrCity) + "</span>"
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
        $("#Business_Name").val(decodeEntities(data.BusinessData.Name));

        // Select2 changes the way we can access the values of the country.
        var countryInput = $("#Address_CountryId");
        if (countryInput.length !== 0) {
            countryInput.select2().val(data.AddressData.CountryId).trigger("change");
        }

        $.deserializeIntoNamedInputs("Address", data.AddressData);
        $.deserializeIntoNamedInputs("Business", data.BusinessData);
        $.deserializeIntoNamedInputs("Contact", data.ContactData);
    } catch (e) {
        console.log("An error occurred obtaining the JSON from string");
    } finally {
        event.preventDefault ? event.preventDefault() : event.returnValue = false;
    }
}

$(function () {
    $("*[data-addressbook-url]").each(function () {
        var url = $(this).data("addressbook-url");
        var type = $(this).data("addressbook-type");
        $(this).autocomplete({
            html: true,
            source: function (request, response) {
                // Does not use .getJSON because caching needs to be removed for this request.
                $.ajax({
                    url: getSearchUrlForAddressBook(url,
                        request.term,
                        type),
                    success: function () { },
                    dataType: "json",
                    cache: false
                }).done(function (data) {
                    response(autocompleteListEntriesForData(data));
                }).error(function () {
                    console.log("An error occurred retrieving address book entries");
                });
    },
        select: selectAutocompleteData,
    focus: function (event, ui) {
        var data = JSON.parse($(ui.item.value).last().html());

        $("#Business_Name").val(decodeEntities(data.BusinessData.Name));

        event.preventDefault ? event.preventDefault() : event.returnValue = false;
    }
});
});
});
