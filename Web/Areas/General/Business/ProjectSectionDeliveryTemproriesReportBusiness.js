var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionDeliveryTemproriesReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionDeliveryTemproriesReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionDeliveryTemproriesReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionDeliveryTemprories", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionDeliveryTemproriesReport/_Report', $('#frm-ReportParameterProjectSectionDeliveryTemprories').serialize(), '#ProjectSectionDeliveryTemprories-Report', 'ProjectSectionDeliveryTemproriesReportCallback();');
    });
}
