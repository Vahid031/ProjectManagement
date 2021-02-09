var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionSupervisorVisitsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionSupervisorVisitsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionSupervisorVisitsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionSupervisorVisits", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionSupervisorVisitsReport/_Report', $('#frm-ReportParameterProjectSectionSupervisorVisits').serialize(), '#ProjectSectionSupervisorVisits-Report', 'ProjectSectionSupervisorVisitsReportCallback();');
    });
}
