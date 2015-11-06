function toggleJsHidden(target) {
    if (target.hasClass("js-hidden")) {
        target.removeClass("js-hidden");
    } else {
        target.addClass("js-hidden");
    }
    return false;
}

function postAjax(url, event, formSelector, successFunction) {
    $.post(url, $(formSelector).serialize()).done(function (data) {
        successFunction(data);
    }).fail(function () {
        return true;
    });

    event.preventDefault ? event.preventDefault() : event.returnValue = false;
    event.stopPropagation ? event.stopPropagation() : event.cancelBubble = true;
    return false;
}

function getAjax(url, data, successFunction) {
    $.get(url, data).done(function(successData) {
        successFunction(successData);
    }).fail(function() {
        console.log("error on get. Url: " + url);
        console.log(data);
    });

    event.preventDefault ? event.preventDefault() : event.returnValue = false;
    event.stopPropagation ? event.stopPropagation() : event.cancelBubble = true;
    return false;
}