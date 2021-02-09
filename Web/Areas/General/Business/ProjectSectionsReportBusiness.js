var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSections", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionsReport/_Report', $('#frm-ReportParameterProjectSections').serialize(), '#ProjectSections-Report', 'ProjectSectionsReportCallback();');
    });
}
