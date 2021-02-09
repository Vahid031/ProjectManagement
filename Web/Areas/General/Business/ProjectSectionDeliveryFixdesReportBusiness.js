var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionDeliveryFixdesReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionDeliveryFixdesReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionDeliveryFixdesReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionDeliveryFixdes", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionDeliveryFixdesReport/_Report', $('#frm-ReportParameterProjectSectionDeliveryFixdes').serialize(), '#ProjectSectionDeliveryFixdes-Report', 'ProjectSectionDeliveryFixdesReportCallback();');
    });
}