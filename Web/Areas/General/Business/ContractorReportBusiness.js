var ReportPermissions;

$(function () {
    ReportPermissions = $('#permission-ContractorReport').val().split(',');
    LoadPartialView('GET', '/General/ContractorReport/_Search', '', '#FormSearch-Report', 'SearchReportCallback();');

    EventHandlerReport();
});

function SearchReportCallback() {
    HandleValidation();
}

function ContractorReportCallback() {
    
}

function EventHandlerReport() {
    $("#FormSearch-Report").on("submit", "#frm-ReportParameterContractor", function (e) {
        e.preventDefault();
        LoadPartialView('GET', '/General/ContractorReport/_Report', $('#frm-ReportParameterContractor').serialize(), '#Contractor-Report', 'ContractorReportCallback();');
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