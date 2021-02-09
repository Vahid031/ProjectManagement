var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionDepositeLiberalizationsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionDepositeLiberalizationsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionDepositeLiberalizationsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionDepositeLiberalizations", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionDepositeLiberalizationsReport/_Report', $('#frm-ReportParameterProjectSectionDepositeLiberalizations').serialize(), '#ProjectSectionDepositeLiberalizations-Report', 'ProjectSectionDepositeLiberalizationsReportCallback();');
    });
}