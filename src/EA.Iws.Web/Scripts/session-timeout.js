(function () {
    "use strict";

    let defaultTimeOutInMinutes;
    let warningTimeInMinutes;

    let sessionTimeoutInSeconds;
    let warningTimeInSeconds;

    let sessionWarningTimer;
    let sessionLogoutTimer;
    let countdownTimer;

    let eventBound = false;

    const $sessionDialog = $("#govuk-timeout-dialog, #govuk-timeout-overlay");

    function startSessionTimeout() {
        clearTimeout(sessionWarningTimer);
        clearTimeout(sessionLogoutTimer);

        sessionWarningTimer = setTimeout(() => showSessionWarning(), (sessionTimeoutInSeconds - warningTimeInSeconds) * 1000);
        sessionLogoutTimer = setTimeout(() => logout(), sessionTimeoutInSeconds * 1000);
    }

    function showSessionWarning() {
        clearTimeout(countdownTimer); // Stop any previous countdown
        setTimeValue(warningTimeInSeconds - 1);
        $sessionDialog.show();
    }

    function setTimeValue(val) {
        let setVal = () => $("#govuk-timeout-countdown").html(formatTime(val));
        setVal();

        if (val < 1) return;

        countdownTimer = setTimeout(() => {
            setTimeValue(--val);
            setVal();
        }, 1000);
    }

    function closeSessionWarning() {
        clearTimeout(countdownTimer); // Stop countdown when modal is closed
        $sessionDialog.hide();
    }

    async function post(url) {
        let tokenName = "__RequestVerificationToken";
        let token = $('input[name=' + tokenName + ']').val();

        let formData = new URLSearchParams();
        formData.append(tokenName, token);

        await $.ajax({
            url: url,
            type: "post",
            data: formData.toString(),
            contentType: 'application/x-www-form-urlencoded;charset=UTF-8'
        });
    }

    async function logout() {
        let signOutUrl = null;
        let logOffUrl = null;
        if (location.host.includes('uat')) {
            logOffUrl = location.protocol + '//' + location.host + '/' + location.pathname.split('/')[1] + '/Account/LogOff';
            signOutUrl = location.protocol + '//' + location.host + '/' + location.pathname.split('/')[1] + '/Account/SessionSignedOut';
        }
        else {
            logOffUrl = '/Account/LogOff';
            signOutUrl = location.protocol + '//' + location.host + '/Account/SessionSignedOut';
        }

        await post(logOffUrl);
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
        let minutesResult = mins > 0 ? mins + ' minute(s) and ' + secs + ' seconds' : + secs + ' seconds';

        return minutesResult;
    }

    function start(timeoutInMinutes, warningBeforeInMinutes, authenticated) {
        if (authenticated === "False")
            return;

        setTimeWith(timeoutInMinutes, warningBeforeInMinutes);
        startSessionTimeout();

        if (!eventBound) {
            $("#govuk-timeout-keep-signin-btn").on("click", async () => {
                let extendSessionUrl = null;
                if (location.host.includes('uat')) {
                    extendSessionUrl = location.protocol + '//' + location.host + '/' + location.pathname.split('/')[1] + '/Account/ExtendSession';
                }
                else {
                    extendSessionUrl = location.protocol + '//' + location.host + '/Account/ExtendSession';
                }
                await post(extendSessionUrl);

                clearTimeout(sessionWarningTimer);
                clearTimeout(sessionLogoutTimer);
                clearTimeout(countdownTimer);

                setTimeWith(timeoutInMinutes, warningBeforeInMinutes);
                startSessionTimeout();
                closeSessionWarning();
            });

            eventBound = true;
        }
    }

    window.GOVUK = window.GOVUK || {};
    window.GOVUK.sessionTimeout = window.GOVUK.sessionTimeout || {};
    window.GOVUK.sessionTimeout.start = start;
}).call(this);