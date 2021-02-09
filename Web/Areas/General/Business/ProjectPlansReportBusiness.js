var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectPlansReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectPlansReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectPlansReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectPlans", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectPlansReport/_Report', $('#frm-ReportParameterProjectPlans').serialize(), '#ProjectPlans-Report', 'ProjectPlansReportCallback();');
    });
}
