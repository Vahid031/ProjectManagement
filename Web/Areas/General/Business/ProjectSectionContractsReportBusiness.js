var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionContractsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionContractsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionContractsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionContracts", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionContractsReport/_Report', $('#frm-ReportParameterProjectSectionContracts').serialize(), '#ProjectSectionContracts-Report', 'ProjectSectionContractsReportCallback();');
    });
}