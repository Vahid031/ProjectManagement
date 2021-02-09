var ProgramPlanPermissions;
var Sets = [];


$(function () {
    ProgramPlanPermissions = $('#permission-ProgramPlan').val().split(',');
    
    if ($.inArray("/ProjectDefine/ProgramPlan/_List", ProgramPlanPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProgramPlan/_List', '', '#FormList-ProgramPlan', 'ListProgramPlanCallback();');
    }

    if ($.inArray("/ProjectDefine/ProgramPlan/_Create", ProgramPlanPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProgramPlan/_Create', '', '#FormContainer-ProgramPlan', 'CreateProgramPlanCallback();');
    }

    EventHandlerProgramPlan();
});

function CreateProgramPlanCallback() {
    CheckValue();
    HandleValidation();

    GetDropDown("ExecutionSetTitle", "DropDownPanel", "/ProjectDefine/ProgramPlan/GetSets", "", "", "FormContainer-ProgramPlan", "DropDownSelected", Sets);

    GetDropDown("BeneficiarySetTitle", "BeneficiarySetDropDownPanel", "/ProjectDefine/ProgramPlan/GetSets", "", "", "FormContainer-ProgramPlan", "BeneficiarySetDropDownSelected", Sets);

    DatePic('#ProgramPlan_AgreementDate');
    DatePic('#ProgramPlan_FirstProjectDate');
    DatePic('#ProgramPlan_ForecastLastProjectDate');
}

function UpdateProgramPlanCallback() {
    CreateProgramPlanCallback();
}

function ListProgramPlanCallback() {
    Pager(1, 5, "ProgramPlan", DataRefreshProgramPlan(1, 5, $("#sort-ProgramPlan").val()));
    
    HandleValidation();

    SortArrow();
}

function DropDownSelected(id, code, title) {
    
    if (id != null) {
        $('#ProgramPlan_ExecutionSetId').val(id);
        $('#ExecutionSetTitle').val(title + ' (' + code + ')');
    }
    else {
        $('#ProgramPlan_ExecutionSetId').val('');
        $('#ExecutionSetTitle').val('انتخاب کنید...');
    }
    
    $('#ExecutionSetTitle').removeData('previousValue');
    $('#ExecutionSetTitle').valid();
}


function BeneficiarySetDropDownSelected(id, code, title) {
    if (id != null) {
        $('#ProgramPlan_BeneficiarySetId').val(id);
        $('#BeneficiarySetTitle').val(title + ' (' + code + ')');
    }
    else {
        $('#ProgramPlan_BeneficiarySetId').val('');
        $('#BeneficiarySetTitle').val('انتخاب کنید...');
    }

    $('#BeneficiarySetTitle').removeData('previousValue');
    $('#BeneficiarySetTitle').valid();
}

function EventHandlerProgramPlan() {
    $("#FormContainer-ProgramPlan").on("submit", "#frm-ProgramPlan", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDefine/ProgramPlan/_Create', $('#frm-ProgramPlan').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProgramPlan();

            if ($('#tbl-ProgramPlan .page-record').val() == null)
                LoadDataProgramPlan(1);
            else
                LoadDataProgramPlan($('#tbl-ProgramPlan .page-record').val());

            if ($.inArray("/ProjectDefine/ProgramPlan/_Create", ProgramPlanPermissions) == -1) {
                $('#FormContainer-ProgramPlan').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProgramPlan").on("keypress", "#ExecutionSetTitle", function () {
        $('#ProgramPlan_ExecutionSetId').val('');

        $('#ExecutionSetTitle').removeData('previousValue');
        $('#ExecutionSetTitle').valid();
    });

    $("#FormContainer-ProgramPlan").on("keypress", "#BeneficiarySetTitle", function () {
        $('#ProgramPlan_BeneficiarySetId').val('');

        $('#BeneficiarySetTitle').removeData('previousValue');
        $('#BeneficiarySetTitle').valid();
    });

    $("#FormContainer-ProgramPlan").on("click", "#frm-ProgramPlan .btnNew", function () {
        ClearFormProgramPlan();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProgramPlan").on("keypress", "#tbl-ProgramPlan tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProgramPlan(1);
            return false;
        }
    });

    $("#FormList-ProgramPlan").on("change keyup", "#tbl-ProgramPlan tbody tr:first select", function (e) {
        LoadDataProgramPlan(1);
    });


}

function DataRefreshProgramPlan(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProgramPlan').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProgramPlan/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProgramPlan tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان برنامه'>" + json[i].ProgramTitle + "</td>");
            tr.append("<td data-th='عنوان طرح'>" + json[i].PlanTitle + "</td>");
            tr.append("<td data-th='کد طرح'>" + json[i].PlanCode + "</td>");
            tr.append("<td data-th='نوع طرح'>" + json[i].PlanTypeTitle + "</td>");
            tr.append("<td data-th='تعداد کل پروژه ها'>" + json[i].TotalProjects + "</td>");
            tr.append("<td data-th='تاریخ شروع اولین پروژه'>" + json[i].FirstProjectFinantialYearTitle + "</td>");
            tr.append("<td data-th='پیش بینی تاریخ خاتمه آخرین پروژه'>" + json[i].ForecastLastProjectFinantialYearTitle + "</td>");
            tr.append("<td data-th='دستگاه اجرایی'>" + json[i].ExecutionSetTitle + "</td>");
            tr.append("<td data-th='دستگاه بهره برداری'>" + json[i].BeneficiarySetTitle + "</td>");

            if ($.inArray("/ProjectDefine/ProgramPlan/_Update", ProgramPlanPermissions) > -1 && $.inArray("/ProjectDefine/ProgramPlan/_Delete", ProgramPlanPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProgramPlan(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProgramPlan'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProgramPlan/_Update", ProgramPlanPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProgramPlan(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProgramPlan/_Delete", ProgramPlanPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProgramPlan'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProgramPlan tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProgramPlan/_Update", ProgramPlanPermissions) == -1 && $.inArray("/ProjectDefine/ProgramPlan/_Delete", ProgramPlanPermissions) == -1) {
            $('#tbl-ProgramPlan th:last').remove();
            $('#tbl-ProgramPlan tbody tr:first td:last').remove();
            $('#tbl-ProgramPlan tfoot td').attr('colspan', $('#tbl-ProgramPlan tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}


function LoadDataProgramPlan(pageRecord) {
    if ($.inArray("/ProjectDefine/ProgramPlan/_List", ProgramPlanPermissions) > -1) {
        var totalRecords = DataRefreshProgramPlan(pageRecord, $('#tbl-ProgramPlan .page-size').val(), $('#sort-ProgramPlan').val());

        Pager(pageRecord, $('#tbl-ProgramPlan .page-size').val(), "ProgramPlan", totalRecords);
    }
}

function ClearFormProgramPlan() {
    
    $('#frm-ProgramPlan input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDefine/ProgramPlan/_Create", ProgramPlanPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProgramPlan').validate();
    $('#frm-ProgramPlan').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProgramPlan(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProgramPlan/_Update', { Id: id }, '#FormContainer-ProgramPlan', 'UpdateProgramPlanCallback();');
}



function DeleteProgramPlan(id) {
    Ajax('Post', '/ProjectDefine/ProgramPlan/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProgramPlan tbody tr').length != 2) {
            pageRecord = $('#tbl-ProgramPlan .page-record').val();
        }
        else {
            if ($('#tbl-ProgramPlan .page-record').val() != 1)
                pageRecord = $('#tbl-ProgramPlan .page-record').val() - 1;
        }

        LoadDataProgramPlan(pageRecord);
    }, 'json');

    
}