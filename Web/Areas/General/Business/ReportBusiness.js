var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ReportReport').val().split(',');
    LoadPartialView('GET', '/General/Report/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ReportReportCallback() {

}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterReport", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/Report/_Report', $('#frm-ReportParameterReport').serialize(), '#Report-Report', 'ReportReportCallback();');
    });
}
