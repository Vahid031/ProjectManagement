var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectStatementReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionStatementReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectStatement", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionStatementReport/_Report', $('#frm-ReportParameterProjectStatement').serialize(), '#ProjectStatement-Report', 'ProjectsReportCallback();');
    });
}
