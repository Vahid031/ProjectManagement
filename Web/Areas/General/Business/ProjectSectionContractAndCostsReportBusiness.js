var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionContractAndCostsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionContractAndCostsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectsReportCallback() {

}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionContractAndCosts", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionContractAndCostsReport/_Report', $('#frm-ReportParameterProjectSectionContractAndCosts').serialize(), '#ProjectSectionContractAndCosts-Report', 'ProjectsReportCallback();');
    });
}
