var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ProjectSectionFinalParticipantsReport').val().split(',');
    LoadPartialView('GET', '/General/ProjectSectionFinalParticipantsReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ProjectSectionFinalParticipantsReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterProjectSectionFinalParticipants", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ProjectSectionFinalParticipantsReport/_Report', $('#frm-ReportParameterProjectSectionFinalParticipants').serialize(), '#ProjectSectionFinalParticipants-Report', 'ProjectSectionFinalParticipantsReportCallback();');
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