// jQuery plugin to prevent double click
jQuery.fn.preventDoubleClick = function () {
    $(this).on('click', function (e) {
        var $el = $(this);
        if ($el.data('clicked')) {
            // Previously clicked, stop actions
            e.preventDefault();
            e.stopPropagation();
        } else {
            // Mark to ignore next click
            $el.data('clicked', true);
            // Unmark after 1 second
            window.setTimeout(function() {
                $el.removeData('clicked');
            }, 1000);
        }
    });
    return this;
};