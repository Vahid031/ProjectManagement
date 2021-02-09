var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionRecoupmentsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionRecoupmentsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionRecoupmentsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionRecoupments", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionRecoupmentsReport/_Report', $('#frm-ReportParameterProjectSectionRecoupments').serialize(), '#ProjectSectionRecoupments-Report', 'ProjectSectionRecoupmentsReportCallback();');
    });
}