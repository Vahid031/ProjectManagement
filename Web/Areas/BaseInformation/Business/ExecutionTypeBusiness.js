var ExecutionTypePermissions;

$(function () {
    ExecutionTypePermissions = $('#permission-ExecutionType').val().split(',');
    
    if ($.inArray("/BaseInformation/ExecutionType/_List", ExecutionTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ExecutionType/_List', '', '#FormList-ExecutionType', 'ListExecutionTypeCallback();');
    }

    if ($.inArray("/BaseInformation/ExecutionType/_Create", ExecutionTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ExecutionType/_Create', '', '#FormContainer-ExecutionType', 'CreateExecutionTypeCallback();');
    }

    EventHandlerExecutionType();
});

function CreateExecutionTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateExecutionTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListExecutionTypeCallback() {
    Pager(1, 5, "ExecutionType", DataRefreshExecutionType(1, 5, $("#sort-ExecutionType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerExecutionType() {
    $("#FormContainer-ExecutionType").on("submit", "#frm-ExecutionType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/ExecutionType/_Create', $('#frm-ExecutionType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormExecutionType();

            if ($('#tbl-ExecutionType .page-record').val() == null)
                LoadDataExecutionType(1);
            else
                LoadDataExecutionType($('#tbl-ExecutionType .page-record').val());

            if ($.inArray("/BaseInformation/ExecutionType/_Create", ExecutionTypePermissions) == -1) {
                $('#FormContainer-ExecutionType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-ExecutionType").on("click", "#frm-ExecutionType .btnNew", function () {
        ClearFormExecutionType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ExecutionType").on("keypress", "#tbl-ExecutionType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataExecutionType(1);
            return false;
        }
    });

    $("#FormList-ExecutionType").on("change keyup", "#tbl-ExecutionType tbody tr:first select", function (e) {
        LoadDataExecutionType(1);
    });

}

function DataRefreshExecutionType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ExecutionType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/ExecutionType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ExecutionType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].ExecutionType.Title + "</td>");

            if ($.inArray("/BaseInformation/ExecutionType/_Update", ExecutionTypePermissions) > -1 && $.inArray("/BaseInformation/ExecutionType/_Delete", ExecutionTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateExecutionType(" + json[i].ExecutionType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteExecutionType'," + json[i].ExecutionType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ExecutionType/_Update", ExecutionTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateExecutionType(" + json[i].ExecutionType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ExecutionType/_Delete", ExecutionTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteExecutionType'," + json[i].ExecutionType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ExecutionType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/ExecutionType/_Update", ExecutionTypePermissions) == -1 && $.inArray("/BaseInformation/ExecutionType/_Delete", ExecutionTypePermissions) == -1) {
            $('#tbl-ExecutionType th:last').remove();
            $('#tbl-ExecutionType tbody tr:first td:last').remove();
            $('#tbl-ExecutionType tfoot td').attr('colspan', $('#tbl-ExecutionType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataExecutionType(pageRecord) {
    if ($.inArray("/BaseInformation/ExecutionType/_List", ExecutionTypePermissions) > -1) {
        var totalRecords = DataRefreshExecutionType(pageRecord, $('#tbl-ExecutionType .page-size').val(), $('#sort-ExecutionType').val());

        Pager(pageRecord, $('#tbl-ExecutionType .page-size').val(), "ExecutionType", totalRecords);
    }
}

function ClearFormExecutionType() {
    
    $('#frm-ExecutionType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/ExecutionType/_Create", ExecutionTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ExecutionType').validate();
    $('#frm-ExecutionType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateExecutionType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/ExecutionType/_Update', { Id: id }, '#FormContainer-ExecutionType', 'UpdateExecutionTypeCallback();');
}



function DeleteExecutionType(id) {
    Ajax('Post', '/BaseInformation/ExecutionType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ExecutionType tbody tr').length != 2) {
            pageRecord = $('#tbl-ExecutionType .page-record').val();
        }
        else {
            if ($('#tbl-ExecutionType .page-record').val() != 1)
                pageRecord = $('#tbl-ExecutionType .page-record').val() - 1;
        }

        LoadDataExecutionType(pageRecord);
    }, 'json');
}