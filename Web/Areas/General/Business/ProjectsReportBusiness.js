var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjects", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectsReport/_Report', $('#frm-ReportParameterProjects').serialize(), '#Projects-Report', 'ProjectsReportCallback();');
    });
}
