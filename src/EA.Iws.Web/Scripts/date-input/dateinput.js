$(document).ready(function () {

    //Notification received start date
    $("#NotificationReceivedStart_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedStart_Month").focus();
        }
    });

    $("#NotificationReceivedStart_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedStart_Year").focus();
        }
    });

    //Notification received end date
    $("#NotificationReceivedEnd_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedEnd_Month").focus();
        }
    });

    $("#NotificationReceivedEnd_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedEnd_Year").focus();
        }
    });

    //Consent "valid from" start date
    $("#ConsentValidFromStart_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromStart_Month").focus();
        }
    });

    $("#ConsentValidFromStart_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromStart_Year").focus();
        }
    });

    //Consent "valid from" end date
    $("#ConsentValidFromEnd_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromEnd_Month").focus();
        }
    });

    $("#ConsentValidFromEnd_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromEnd_Year").focus();
        }
    });

    //Consent "valid to" start date 
    $("#ConsentValidToStart_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToStart_Month").focus();
        }
    });

    $("#ConsentValidToStart_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToStart_Year").focus();
        }
    });

    //Consent "valid to" end date 
    $("#ConsentValidToEnd_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToEnd_Month").focus();
        }
    });

    $("#ConsentValidToEnd_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToEnd_Year").focus();
        }
    });

    //Start - Update Key Dates
    //Notification received
    $("#NotificationReceivedDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedDate_Month").focus();
        }
    });

    $("#NotificationReceivedDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NotificationReceivedDate_Year").focus();
        }
    });

    //Assessment started
    $("#CommencementDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#CommencementDate_Month").focus();
        }
    });

    $("#CommencementDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#CommencementDate_Year").focus();
        }
    });

    //Notification complete
    $("#CompleteDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#CompleteDate_Month").focus();
        }
    });

    $("#CompleteDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#CompleteDate_Year").focus();
        }
    });

    //Transmitted on
    $("#TransmittedDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#TransmittedDate_Month").focus();
        }
    });

    $("#TransmittedDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#TransmittedDate_Year").focus();
        }
    });

    //Acknowledged on
    $("#AcknowledgedDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#AcknowledgedDate_Month").focus();
        }
    });

    $("#AcknowledgedDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#AcknowledgedDate_Year").focus();
        }
    });

    //Decision required by
    $("#DecisionRequiredByDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#DecisionRequiredByDate_Month").focus();
        }
    });

    $("#DecisionRequiredByDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#DecisionRequiredByDate_Year").focus();
        }
    });

    //Consent date
    $("#ConsentedDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentedDate_Month").focus();
        }
    });

    $("#ConsentedDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentedDate_Year").focus();
        }
    });

    //Consent valid from
    $("#ConsentValidFromDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromDate_Month").focus();
        }
    });

    $("#ConsentValidFromDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidFromDate_Year").focus();
        }
    });

    //Consent valid to
    $("#ConsentValidToDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToDate_Month").focus();
        }
    });

    $("#ConsentValidToDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentValidToDate_Year").focus();
        }
    });
    // End - Update Key Dates

    //Key Date
    $("#NewDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NewDate_Month").focus();
        }
    });

    $("#NewDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#NewDate_Year").focus();
        }
    });

    //PaymentDate_Day
    $("#PaymentDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#PaymentDate_Month").focus();
        }
    });

    $("#PaymentDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#PaymentDate_Year").focus();
        }
    });

    //External User Journey
    //StartDay
    $("#StartDay").keyup(function () {
        if ($(this).val().length == 2) {
            $("#StartMonth").focus();
        }
    });

    $("#StartMonth").keyup(function () {
        if ($(this).val().length== 2) {
            $("#StartYear").focus();
        }
    });

    //EndDay
    $("#EndDay").keyup(function () {
        if ($(this).val().length == 2) {
            $("#EndMonth").focus();
        }
    });

    $("#EndMonth").keyup(function () {
        if ($(this).val().length == 2) {
            $("#EndYear").focus();
        }
    });

    //Reports
    //From_Day
    $("#From_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#From_Month").focus();
        }
    });

    $("#From_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#From_Year").focus();
        }
    });

    //From_Day
    $("#To_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#To_Month").focus();
        }
    });

    $("#To_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#To_Year").focus();
        }
    });

    //InputParameters_FromDate_Day
    $("#InputParameters_FromDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#InputParameters_FromDate_Month").focus();
        }
    });

    $("#InputParameters_FromDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#InputParameters_FromDate_Year").focus();
        }
    });

    //InputParameters_ToDate_Day
    $("#InputParameters_ToDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#InputParameters_ToDate_Month").focus();
        }
    });

    $("#InputParameters_ToDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#InputParameters_ToDate_Year").focus();
        }
    });

    //ConsentWithdrawnDate_Day
    $("#ConsentWithdrawnDate_Day").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentWithdrawnDate_Month").focus();
        }
    });

    $("#ConsentWithdrawnDate_Month").keyup(function () {
        if ($(this).val().length == 2) {
            $("#ConsentWithdrawnDate_Year").focus();
        }
    });

});


