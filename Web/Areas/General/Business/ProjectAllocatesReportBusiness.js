var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectAllocatesReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectAllocatesReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectAllocatesReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectAllocates", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectAllocatesReport/_Report', $('#frm-ReportParameterProjectAllocates').serialize(), '#ProjectAllocates-Report', 'ProjectAllocatesReportCallback();');
    });
}
