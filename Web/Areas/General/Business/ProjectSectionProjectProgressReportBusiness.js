var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionProjectProgressReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionProjectProgressReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}
function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ProjectSectionProjectProgress", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionProjectProgressReport/_Report', $('#frm-ProjectSectionProjectProgress').serialize(), '#ProjectSectionProjectProgressReport-Report', 'ProjectsReportCallback();');
    });
}