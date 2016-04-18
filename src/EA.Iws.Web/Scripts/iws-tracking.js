$(function () {

    // Send ga event when a user opens a progressive disclosure
    $('[data-track="disclosure"]').each(function () {
        $(this).click(function () {

            var state = $(this).parent().next().attr('aria-hidden') === "false" ? true : false;

            if (state) {
                ga("send", "event", category($(this)), action($(this)), label($(this)), value($(this)));
            }
        });
    });


    // Send ga event when a user clicks an element - for example, a checkbox
    // or a link that goes to an external site
    $('[data-track="element"]').each(function() {
        $(this).click(function () {
            ga("send", "event", category($(this)), action($(this)), label($(this)), value($(this)));
        });
    });

    // Send ga event when a user clicks a link to another internal page
    $('[data-track="link"]').each(function () {
        $(this).click(function (event) {
            event.preventDefault();

            var href = $(this).attr("href");

            ga("send", "event", category($(this)), action($(this)), label($(this)), value($(this)), {
                hitCallback: createFunctionWithTimeout(function () {
                    window.location.href = href;
                })
            });
        });
    });
});

function createFunctionWithTimeout(callback, optTimeout) {
    var called = false;
    setTimeout(callback, optTimeout || 1000);
    return function () {
        if (!called) {
            called = true;
            callback();
        }
    }
}

function category(element) {
    return element.data("category") ? element.data("category") : "";
}

function action(element) {
    return element.data("action") ? element.data("action") : "";
}

function label(element) {
    return element.data("label") ? element.data("label") : document.title;
}

function value(element) {
    return element.data("value") ? element.data("value") : "1";
}
