var ContractorPermissions;
var ContractorLevels;

$(function () {
    ContractorPermissions = $('#permission-Contractor').val().split(',');

    if ($.inArray("/BaseInformation/Contractor/_List", ContractorPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Contractor/_List', '', '#FormList-Contractor', 'ListContractorCallback();');
    }

    if ($.inArray("/BaseInformation/Contractor/_Create", ContractorPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Contractor/_Create', '', '#FormContainer-Contractor', 'CreateContractorCallback();');
    }

    EventHandlerContractor();

    ContractorLevels = '';
});





function CreateContractorCallback() {
    CheckValue();

    HandleValidation();

    DatePic('#Contractor_RegistrationDate_');

    // این کد برای نمایش مقادیر حقیقی / حقوقی می باشد
    $('#frm-Member').find('#Contractor_LicenseValueAddedNumber,#Contractor_EconomicCode,#Contractor_NationalID,#Contractor_RegistrationDate_,#Contractor_RegistrationNumber').prop('disabled', false);
    $('#RegistrationNumber,#RegistrationDate_,#NationalID,#EconomicCode,#LicenseValueAddedNumber,#contractorLevel').css({ 'display': 'none' });
}

function UpdateContractorCallback() {
    CheckValue();

    HandleValidation();

    DatePic('#Contractor_RegistrationDate_');

    $('#frm-Member').find('#Contractor_LicenseValueAddedNumber,#Contractor_EconomicCode,#Contractor_NationalID,#Contractor_RegistrationDate_,#Contractor_RegistrationNumber').prop('disabled', false);
    $('#RegistrationNumber,#RegistrationDate_,#NationalID,#EconomicCode,#LicenseValueAddedNumber,#contractorLevel').css({ 'display': 'none' });

    if ($('#Contractor_ContractorType').val() == 'حقوقی') {
        $('#frm-Member').find('#Contractor_LicenseValueAddedNumber,#Contractor_EconomicCode,#Contractor_NationalID,#Contractor_RegistrationDate_,#Contractor_RegistrationNumber').prop('disabled', true);
        $('#RegistrationNumber,#RegistrationDate_,#NationalID,#EconomicCode,#LicenseValueAddedNumber,#contractorLevel').fadeIn(1000);
    }


    //Get ContractorLevels That Exists
    ContractorLevels = '';

    Ajax('Post', '/BaseInformation/Contractor/GetContractorLevelsByContractorId', 'contractorId=' + $('#Id').val(), function (data, textStatus, xhr) {
        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            ContractorLevels += 'MajorTypeId:' + json[i].MajorTypeId + ',MajorTypeTitle:' + json[i].MajorTypeTitle + ',LevelTypeId:' + json[i].LevelTypeId + ',LevelTypeTitle:' + json[i].LevelTypeTitle + ',FinishCreditDate:' + json[i].FinishCreditDate + '|';
        }

    }, 'json');
}

function ListContractorCallback() {
    Pager(1, 5, "Contractor", DataRefreshContractor(1, 5, $("#sort-Contractor").val()));

    HandleValidation();
    SortArrow();
}

function EventHandlerContractor() {
    $("#FormContainer-Contractor").on("submit", "#frm-Contractor", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Contractor/_Create', $('#frm-Contractor').serialize() + "&ContractorLevels=" + ContractorLevels, function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormContractor();

            if ($('#tbl-Contractor .page-record').val() == null)
                LoadDataContractor(1);
            else
                LoadDataContractor($('#tbl-Contractor .page-record').val());

            if ($.inArray("/BaseInformation/Contractor/_Create", ContractorPermissions) == -1) {
                $('#FormContainer-Contractor').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Contractor").on("click", "#frm-Contractor .btnNew", function () {
        ClearFormContractor();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Contractor").on("keypress", "#tbl-Contractor tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataContractor(1);
            return false;
        }
    });

    $("#FormList-Contractor").on("change keyup", "#tbl-Contractor tbody tr:first select", function (e) {
        LoadDataContractor(1);
    });

    $('#FormContainer-Contractor').on('change keyup', '#frm-Contractor #Contractor_ContractorType', function () {
        if ($('#Contractor_ContractorType').val() == 'حقوقی') {
            $('#frm-Member').find('#Contractor_LicenseValueAddedNumber,#Contractor_EconomicCode,#Contractor_NationalID,#Contractor_RegistrationDate_,#Contractor_RegistrationNumber').prop('disabled', true);
            $('#RegistrationNumber,#RegistrationDate_,#NationalID,#EconomicCode,#LicenseValueAddedNumber,#contractorLevel').fadeIn(1000);
        }
        else {
            $('#frm-Member').find('#Contractor_LicenseValueAddedNumber,#Contractor_EconomicCode,#Contractor_NationalID,#Contractor_RegistrationDate_,#Contractor_RegistrationNumber').prop('disabled', false);
            $('#RegistrationNumber,#RegistrationDate_,#NationalID,#EconomicCode,#LicenseValueAddedNumber,#contractorLevel').fadeOut(1000);
        }
    });

    $("#FormContainer-Contractor").on("click", "#btnAddLevel", function () {
        PopupFormHtml("انتخاب رشته و رتبه پیمانکار", "/BaseInformation/Contractor/_Level", "LevelCallBack()", true, "YesPopupClick();")
    });
}

function LevelCallBack() {
    DatePic('#txtFinishCreditDate');

    $("#frm-ContractorLevel").on("click", "#btnAddContractorLevel", function (e) {
        e.preventDefault();
        if ($('#ddlMajorTypeId').val() == '' || $('#ddlLevelTypeId').val() == '' || $('#txtFinishCreditDate').val() == '') {
            Messages('danger', 'مقادیر را به درستی پر کنید');
        }
        else {
            var tr = $('<tr/>');
            var rowId = $('#tbl-ContractorLevel tbody tr').length + 1;

            tr.append("<td data-th='ردیف'>" + rowId + "</td>");
            tr.append("<td style='display:none' data-th='نوع رشته پیمانکار'>" + $('#ddlMajorTypeId').val() + "</td>");
            tr.append("<td data-th='نوع رشته پیمانکار'>" + $('#ddlMajorTypeId option:selected').text() + "</td>");
            tr.append("<td style='display:none' data-th='رتبه پیمانکار'>" + $('#ddlLevelTypeId').val() + "</td>");
            tr.append("<td data-th='رتبه پیمانکار'>" + $('#ddlLevelTypeId option:selected').text() + "</td>");
            tr.append("<td data-th='تاریخ اتمام اعتبار پیمانکار'>" + $('#txtFinishCreditDate').val() + "</td>");
            tr.append("<td data-th='حذف'><a onmousedown = \"DeleteContractorLevel(" + rowId + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");


            $('#tbl-ContractorLevel tbody').append(tr);
        }
    });

    //if (ContractorLevels == '')
    //    ContractorLevels = 'MajorTypeId:1,MajorTypeTitle:sssss,LevelTypeId:2,LevelTypeTitle:ddddd,FinishCreditDate:1391/01/01|MajorTypeId:1,MajorTypeTitle:v  c c c ,LevelTypeId:2,LevelTypeTitle:wqwewewqe,FinishCreditDate:1391/01/01|MajorTypeId:1,MajorTypeTitle:aaasd,LevelTypeId:2,LevelTypeTitle:fff,FinishCreditDate:1392/01/01|'

    $(ContractorLevels.split('|')).each(function (i, row) {
        if (row != '') {
            var tr = $('<tr/>');
            var rowId = $('#tbl-ContractorLevel tbody tr').length + 1;

            tr.append("<td data-th='ردیف'>" + rowId + "</td>");
            tr.append("<td style='display:none' data-th='نوع رشته پیمانکار'>" + row.split(',')[0].split(':')[1] + "</td>");
            tr.append("<td data-th='نوع رشته پیمانکار'>" + row.split(',')[1].split(':')[1] + "</td>");
            tr.append("<td style='display:none' data-th='رتبه پیمانکار'>" + row.split(',')[2].split(':')[1] + "</td>");
            tr.append("<td data-th='رتبه پیمانکار'>" + row.split(',')[3].split(':')[1] + "</td>");
            tr.append("<td data-th='تاریخ اتمام اعتبار پیمانکار'>" + row.split(',')[4].split(':')[1] + "</td>");
            tr.append("<td data-th='حذف'><a onmousedown = \"DeleteContractorLevel(" + rowId + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");

            $('#tbl-ContractorLevel tbody').append(tr);
        }
    });
}

function YesPopupClick() {
    ContractorLevels = '';

    $('#tbl-ContractorLevel tbody tr').each(function (i, row) {
        var MajorTypeId = $(row).find('td').eq(1).html();
        var MajorTypeTitle = $(row).find('td').eq(2).html();
        var LevelTypeId = $(row).find('td').eq(3).html();
        var LevelTypeTitle = $(row).find('td').eq(4).html();
        var FinishCreditDate = $(row).find('td').eq(5).html();

        ContractorLevels += 'MajorTypeId:' + MajorTypeId + ',MajorTypeTitle:' + MajorTypeTitle + ',LevelTypeId:' + LevelTypeId + ',LevelTypeTitle:' + LevelTypeTitle + ',FinishCreditDate:' + FinishCreditDate + '|';
    });

    if (ContractorLevels == '')
        ContractorLevels = '|';
}

function DeleteContractorLevel(rowId) {
    $('#tbl-ContractorLevel tbody tr').each(function (i, row) {
        if ($(row).find('td').eq(0).html() == rowId) {
            $(row).remove();
        }
    });

    $('#tbl-ContractorLevel tbody tr').each(function (i, row) {
        $(row).find('td').eq(0).html(i + 1);
        $(row).find('td').eq(6).html("<a onmousedown = \"DeleteContractorLevel(" + (i + 1) + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a>");
    });
}

function DataRefreshContractor(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Contractor').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/BaseInformation/Contractor/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Contractor tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام شرکت'>" + json[i].Contractor.CompanyName + "</td>");
            tr.append("<td data-th='نام مدیر عامل'>" + json[i].Contractor.ManagerFirstName + "</td>");
            tr.append("<td data-th='نام خانوادگی مدیر عامل'>" + json[i].Contractor.ManagerLastName + "</td>");
            tr.append("<td data-th='موبایل مدیر عامل'>" + json[i].Contractor.ManagerMobileNumber + "</td>");
            tr.append("<td data-th='کد ملی مدیر عامل'>" + json[i].Contractor.ManagerNationalCode + "</td>");
            tr.append("<td data-th='شماره تلفن'>" + json[i].Contractor.PhoneNumber + "</td>");



            if (json[i].Contractor.IsActive == true)
                tr.append("<td style='text-align:center' data-th='وضعیت'><input type='checkbox' checked disabled></td>");
            else
                tr.append("<td style='text-align:center' data-th='وضعیت'><input type='checkbox' disabled></td>");


            if ($.inArray("/BaseInformation/Contractor/_Update", ContractorPermissions) > -1 && $.inArray("/BaseInformation/Contractor/_Delete", ContractorPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateContractor(" + json[i].Contractor.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteContractor'," + json[i].Contractor.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Contractor/_Update", ContractorPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateContractor(" + json[i].Contractor.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Contractor/_Delete", ContractorPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteContractor'," + json[i].Contractor.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Contractor tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Contractor/_Update", ContractorPermissions) == -1 && $.inArray("/BaseInformation/Contractor/_Delete", ContractorPermissions) == -1) {
            $('#tbl-Contractor th:last').remove();
            $('#tbl-Contractor tbody tr:first td:last').remove();
            $('#tbl-Contractor tfoot td').attr('colspan', $('#tbl-Contractor tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataContractor(pageRecord) {
    if ($.inArray("/BaseInformation/Contractor/_List", ContractorPermissions) > -1) {
        var totalRecords = DataRefreshContractor(pageRecord, $('#tbl-Contractor .page-size').val(), $('#sort-Contractor').val());

        Pager(pageRecord, $('#tbl-Contractor .page-size').val(), "Contractor", totalRecords);
    }
}

function ClearFormContractor() {

    $('#frm-Contractor input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Contractor/_Create", ContractorPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-Contractor').validate();
    $('#frm-Contractor').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();

    ContractorLevels = '';
}


function UpdateContractor(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Contractor/_Update', { Id: id }, '#FormContainer-Contractor', 'UpdateContractorCallback();');
}



function DeleteContractor(id) {
    Ajax('Post', '/BaseInformation/Contractor/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Contractor tbody tr').length != 2) {
            pageRecord = $('#tbl-Contractor .page-record').val();
        }
        else {
            if ($('#tbl-Contractor .page-record').val() != 1)
                pageRecord = $('#tbl-Contractor .page-record').val() - 1;
        }

        LoadDataContractor(pageRecord);
    }, 'json');
}