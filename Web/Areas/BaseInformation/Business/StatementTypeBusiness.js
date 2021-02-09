var StatementTypePermissions;

$(function () {
    StatementTypePermissions = $('#permission-StatementType').val().split(',');
    
    if ($.inArray("/BaseInformation/StatementType/_List", StatementTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/StatementType/_List', '', '#FormList-StatementType', 'ListStatementTypeCallback();');
    }

    if ($.inArray("/BaseInformation/StatementType/_Create", StatementTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/StatementType/_Create', '', '#FormContainer-StatementType', 'CreateStatementTypeCallback();');
    }

    EventHandlerStatementType();
});

function CreateStatementTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateStatementTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListStatementTypeCallback() {
    Pager(1, 5, "StatementType", DataRefreshStatementType(1, 5, $("#sort-StatementType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerStatementType() {
    $("#FormContainer-StatementType").on("submit", "#frm-StatementType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/StatementType/_Create', $('#frm-StatementType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormStatementType();

            if ($('#tbl-StatementType .page-record').val() == null)
                LoadDataStatementType(1);
            else
                LoadDataStatementType($('#tbl-StatementType .page-record').val());

            if ($.inArray("/BaseInformation/StatementType/_Create", StatementTypePermissions) == -1) {
                $('#FormContainer-StatementType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-StatementType").on("click", "#frm-StatementType .btnNew", function () {
        ClearFormStatementType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-StatementType").on("keypress", "#tbl-StatementType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataStatementType(1);
            return false;
        }
    });

    $("#FormList-StatementType").on("change keyup", "#tbl-StatementType tbody tr:first select", function (e) {
        LoadDataStatementType(1);
    });

}

function DataRefreshStatementType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-StatementType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/StatementType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-StatementType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نوع صورت وضعیت'>" + json[i].StatementType.Title + "</td>");

            if ($.inArray("/BaseInformation/StatementType/_Update", StatementTypePermissions) > -1 && $.inArray("/BaseInformation/StatementType/_Delete", StatementTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateStatementType(" + json[i].StatementType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteStatementType'," + json[i].StatementType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/StatementType/_Update", StatementTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateStatementType(" + json[i].StatementType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/StatementType/_Delete", StatementTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteStatementType'," + json[i].StatementType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-StatementType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/StatementType/_Update", StatementTypePermissions) == -1 && $.inArray("/BaseInformation/StatementType/_Delete", StatementTypePermissions) == -1) {
            $('#tbl-StatementType th:last').remove();
            $('#tbl-StatementType tbody tr:first td:last').remove();
            $('#tbl-StatementType tfoot td').attr('colspan', $('#tbl-StatementType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataStatementType(pageRecord) {
    if ($.inArray("/BaseInformation/StatementType/_List", StatementTypePermissions) > -1) {
        var totalRecords = DataRefreshStatementType(pageRecord, $('#tbl-StatementType .page-size').val(), $('#sort-StatementType').val());

        Pager(pageRecord, $('#tbl-StatementType .page-size').val(), "StatementType", totalRecords);
    }
}

function ClearFormStatementType() {
    
    $('#frm-StatementType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/StatementType/_Create", StatementTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-StatementType').validate();
    $('#frm-StatementType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateStatementType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/StatementType/_Update', { Id: id }, '#FormContainer-StatementType', 'UpdateStatementTypeCallback();');
}



function DeleteStatementType(id) {
    Ajax('Post', '/BaseInformation/StatementType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-StatementType tbody tr').length != 2) {
            pageRecord = $('#tbl-StatementType .page-record').val();
        }
        else {
            if ($('#tbl-StatementType .page-record').val() != 1)
                pageRecord = $('#tbl-StatementType .page-record').val() - 1;
        }

        LoadDataStatementType(pageRecord);
    }, 'json');
}