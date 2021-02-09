var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionStatementPayReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionStatementPayReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionStatementPay", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionStatementPayReport/_Report', $('#frm-ReportParameterProjectSectionStatementPay').serialize(), '#ProjectSectionStatementPay-Report', 'ProjectsReportCallback();');
    });
}
