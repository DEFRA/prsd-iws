$(function () {

    // Send ga event when a user opens a progressive disclosure
    $('[data-track="disclosure"]').each(function () {
        $(this).click(function () {

            var state = $(this).parent().next().attr('aria-hidden') === "false" ? true : false;

            if (state) {
                ga("send", "event", Category($(this)), Action($(this)), Label($(this)), Value($(this)));
            }
        });
    });


    // Send ga event when a user clicks an element - for example, a checkbox
    $('[data-track="element"]').each(function() {
        $(this).click(function () {
            ga("send", "event", Category($(this)), Action($(this)), Label($(this)), Value($(this)));
        });
    });

    // Send ga event when a user submits a form
    $('[data-track="submit"]').each(function () {
        $(this).submit(function (event) {
            event.preventDefault();

            var form = this;
            ga("send", "event", Category($(this)), Action($(this)), Label($(this)), Value($(this)), {
                hitCallback: createFunctionWithTimeout(function () {
                    $(form).unbind("submit").submit();
                })
            });
        });
    });

    // Send ga event when a user clicks a link to another page
    $('[data-track="link"]').each(function () {
        $(this).click(function (event) {
            event.preventDefault();

            var href = $(this).attr("href");

            ga("send", "event", Category($(this)), Action($(this)), Label($(this)), Value($(this)), {
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

function Category(element) {
    return element.data("category") ? element.data("category") : "";
}

function Action(element) {
    return element.data("action") ? element.data("action") : "";
}

function Label(element) {
    return element.data("label") ? element.data("label") : document.title;
}

function Value(element) {
    return element.data("value") ? element.data("value") : "";
}
