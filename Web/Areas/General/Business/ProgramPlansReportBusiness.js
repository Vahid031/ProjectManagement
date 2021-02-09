var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProgramPlansReport').val().split(',');
    LoadPartialView('GET', '/General/ProgramPlansReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProgramPlansReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProgramPlans", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProgramPlansReport/_Report', $('#frm-ReportParameterProgramPlans').serialize(), '#ProgramPlans-Report', 'ProgramPlansReportCallback();');
    });
}