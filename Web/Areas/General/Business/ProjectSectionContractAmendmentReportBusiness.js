var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionContractAmendmentReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionContractAmendmentReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});
function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}
function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionContractAmendment", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionContractAmendmentReport/_Report', $('#frm-ReportParameterProjectSectionContractAmendment').serialize(), '#ProjectSectionContractAmendment-Report', 'ProjectsReportCallback();');
    });
}
