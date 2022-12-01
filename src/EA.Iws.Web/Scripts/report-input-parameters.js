$(function () {
    $("#InputParameters_SelectedTextField").change(function () {
        var panel = $('.operator-text-panel');

        if (this.value === "-1") {
            panel.addClass('js-hidden');
        } else {
            panel.removeClass('js-hidden');
        }
    });
});