var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionMadeh46sReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionMadeh46sReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionMadeh46sReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionMadeh46s", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionMadeh46sReport/_Report', $('#frm-ReportParameterProjectSectionMadeh46s').serialize(), '#ProjectSectionMadeh46s-Report', 'ProjectSectionMadeh46sReportCallback();');
    });
}