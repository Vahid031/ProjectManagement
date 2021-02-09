var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionContractAmendmentDateReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionContractAmendmentDateReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}
function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionContractAmendmentDate", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionContractAmendmentDateReport/_Report', $('#frm-ReportParameterProjectSectionContractAmendmentDate').serialize(), '#ProjectSectionContractAmendmentDate-Report', 'ProjectsReportCallback();');
    });
}
