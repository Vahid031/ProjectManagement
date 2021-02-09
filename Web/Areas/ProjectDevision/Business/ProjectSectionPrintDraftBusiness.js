var Permissions;

$(function () {
    Permissions = $('#permission-ProjectSectionPrintDraft').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionPrintDraft/_Report", Permissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionPrintDraft/_Report', '', '#FormContainer-ProjectSectionPrintDraft', 'ReportProjectSectionPrintDraftCallback();');
    } 

});


function ReportProjectSectionPrintDraftCallback() {
        
}