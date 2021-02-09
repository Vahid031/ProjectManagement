var PlanTypePermissions;

$(function () {
    PlanTypePermissions = $('#permission-PlanType').val().split(',');
    
    if ($.inArray("/BaseInformation/PlanType/_List", PlanTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/PlanType/_List', '', '#FormList-PlanType', 'ListPlanTypeCallback();');
    }

    if ($.inArray("/BaseInformation/PlanType/_Create", PlanTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/PlanType/_Create', '', '#FormContainer-PlanType', 'CreatePlanTypeCallback();');
    }

    EventHandlerPlanType();
});

function CreatePlanTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdatePlanTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListPlanTypeCallback() {
    Pager(1, 5, "PlanType", DataRefreshPlanType(1, 5, $("#sort-PlanType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerPlanType() {
    $("#FormContainer-PlanType").on("submit", "#frm-PlanType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/PlanType/_Create', $('#frm-PlanType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormPlanType();

            if ($('#tbl-PlanType .page-record').val() == null)
                LoadDataPlanType(1);
            else
                LoadDataPlanType($('#tbl-PlanType .page-record').val());

            if ($.inArray("/BaseInformation/PlanType/_Create", PlanTypePermissions) == -1) {
                $('#FormContainer-PlanType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-PlanType").on("click", "#frm-PlanType .btnNew", function () {
        ClearFormPlanType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-PlanType").on("keypress", "#tbl-PlanType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataPlanType(1);
            return false;
        }
    });

    $("#FormList-PlanType").on("change keyup", "#tbl-PlanType tbody tr:first select", function (e) {
        LoadDataPlanType(1);
    });

}

function DataRefreshPlanType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-PlanType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/PlanType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-PlanType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان طرح'>" + json[i].PlanType.Title + "</td>");

            if ($.inArray("/BaseInformation/PlanType/_Update", PlanTypePermissions) > -1 && $.inArray("/BaseInformation/PlanType/_Delete", PlanTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdatePlanType(" + json[i].PlanType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeletePlanType'," + json[i].PlanType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/PlanType/_Update", PlanTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdatePlanType(" + json[i].PlanType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/PlanType/_Delete", PlanTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeletePlanType'," + json[i].PlanType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-PlanType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/PlanType/_Update", PlanTypePermissions) == -1 && $.inArray("/BaseInformation/PlanType/_Delete", PlanTypePermissions) == -1) {
            $('#tbl-PlanType th:last').remove();
            $('#tbl-PlanType tbody tr:first td:last').remove();
            $('#tbl-PlanType tfoot td').attr('colspan', $('#tbl-PlanType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataPlanType(pageRecord) {
    if ($.inArray("/BaseInformation/PlanType/_List", PlanTypePermissions) > -1) {
        var totalRecords = DataRefreshPlanType(pageRecord, $('#tbl-PlanType .page-size').val(), $('#sort-PlanType').val());

        Pager(pageRecord, $('#tbl-PlanType .page-size').val(), "PlanType", totalRecords);
    }
}

function ClearFormPlanType() {
    
    $('#frm-PlanType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/PlanType/_Create", PlanTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-PlanType').validate();
    $('#frm-PlanType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdatePlanType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/PlanType/_Update', { Id: id }, '#FormContainer-PlanType', 'UpdatePlanTypeCallback();');
}



function DeletePlanType(id) {
    Ajax('Post', '/BaseInformation/PlanType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-PlanType tbody tr').length != 2) {
            pageRecord = $('#tbl-PlanType .page-record').val();
        }
        else {
            if ($('#tbl-PlanType .page-record').val() != 1)
                pageRecord = $('#tbl-PlanType .page-record').val() - 1;
        }

        LoadDataPlanType(pageRecord);
    }, 'json');
}