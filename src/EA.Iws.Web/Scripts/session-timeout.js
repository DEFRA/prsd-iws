(() => {
    let defaultTimeOutInMinutes;
    let warningTimeInMinutes;

    let sessionTimeoutInSeconds;
    let warningTimeInSeconds;

    let sessionWarningTimer;
    let sessionLogoutTimer;

    const $sessionDialog = $("#govuk-timeout-dialog, .govuk-timeout-overlay");

    function startSessionTimeout() {
        clearTimeout(sessionWarningTimer);
        clearTimeout(sessionLogoutTimer);

        sessionWarningTimer = setTimeout(() => showSessionWarning(), (sessionTimeoutInSeconds - warningTimeInSeconds) * 1000);

        sessionLogoutTimer = setTimeout(() => logout(), sessionTimeoutInSeconds * 1000);
    }

    function showSessionWarning() {
        setTimeValue(warningTimeInSeconds - 1);

        $sessionDialog.show();
    }

    function setTimeValue(val) {
        let setVal = () => $("#govuk-timeout-countdown").html(formatTime(val));

        setVal();

        if (val < 1) return;

        setTimeout(() => {
            setTimeValue(--val);

            setVal();
        }, 1000);
    }

    function closeSessionWarning() {
        $sessionDialog.hide();
    }

    async function post(url) {
        let tokenName = "__RequestVerificationToken";

        let token = $(`input[name='${tokenName}']`).val()

        let formData = new URLSearchParams();
        formData.append(tokenName, token)

        await $.ajax({
            url: url,
            type: "POST",
            data: formData.toString(),
            contentType: 'application/x-www-form-urlencoded;charset=UTF-8'
        });
    }

    async function logout() {
        await post('/Account/LogOff')

        let signOutUrl = `${location.protocol}//${location.host}/Account/SessionSignedOut`

        document.location.href = signOutUrl;
    }

    function setTimeWith(ltTimeoutInMinutes, lWarningTimeInMinutes) {
        defaultTimeOutInMinutes = ltTimeoutInMinutes;
        warningTimeInMinutes = lWarningTimeInMinutes;

        sessionTimeoutInSeconds = defaultTimeOutInMinutes * 60;
        warningTimeInSeconds = warningTimeInMinutes * 60;
    }

    function formatTime(seconds) {
        let mins = Math.floor(seconds / 60);
        let secs = seconds % 60;

        let minutesResult = mins > 0 ? `${mins} minute(s) and ` : "";

        return `${minutesResult}${secs} seconds`;
    }

    function start(timeoutInMinutes, warningBeforeInMinutes, authenticated) {
        if (authenticated == "False") return;

        $(document).ready(() => {
            setTimeWith(timeoutInMinutes, warningBeforeInMinutes);
            startSessionTimeout();

            $("#govuk-timeout-keep-signin-btn").click(async () => {
                await post('/Account/ExtendSession');

                setTimeWith(timeoutInMinutes, warningBeforeInMinutes);

                startSessionTimeout();

                closeSessionWarning();
            });
        });
    }

    window.GOVUK = window.GOVUK || {};
    window.GOVUK.sessionTimeout = window.GOVUK.sessionTimeout || {};
    window.GOVUK.sessionTimeout.start = start;
})();