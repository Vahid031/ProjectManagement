var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionMadeh48sReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionMadeh48sReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionMadeh48sReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionMadeh48s", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionMadeh48sReport/_Report', $('#frm-ReportParameterProjectSectionMadeh48s').serialize(), '#ProjectSectionMadeh48s-Report', 'ProjectSectionMadeh48sReportCallback();');
    });
}

function ClearFormReport() {    
    $('#frm-Report input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    var $validator = $('#frm-Report').validate();
    $('#frm-Report').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}