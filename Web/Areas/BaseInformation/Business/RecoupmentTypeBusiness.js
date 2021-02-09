var RecoupmentTypePermissions;

$(function () {
    RecoupmentTypePermissions = $('#permission-RecoupmentType').val().split(',');
    
    if ($.inArray("/BaseInformation/RecoupmentType/_List", RecoupmentTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/RecoupmentType/_List', '', '#FormList-RecoupmentType', 'ListRecoupmentTypeCallback();');
    }

    if ($.inArray("/BaseInformation/RecoupmentType/_Create", RecoupmentTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/RecoupmentType/_Create', '', '#FormContainer-RecoupmentType', 'CreateRecoupmentTypeCallback();');
    }

    EventHandlerRecoupmentType();
});

function CreateRecoupmentTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateRecoupmentTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListRecoupmentTypeCallback() {
    Pager(1, 5, "RecoupmentType", DataRefreshRecoupmentType(1, 5, $("#sort-RecoupmentType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerRecoupmentType() {
    $("#FormContainer-RecoupmentType").on("submit", "#frm-RecoupmentType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/RecoupmentType/_Create', $('#frm-RecoupmentType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormRecoupmentType();

            if ($('#tbl-RecoupmentType .page-record').val() == null)
                LoadDataRecoupmentType(1);
            else
                LoadDataRecoupmentType($('#tbl-RecoupmentType .page-record').val());

            if ($.inArray("/BaseInformation/RecoupmentType/_Create", RecoupmentTypePermissions) == -1) {
                $('#FormContainer-RecoupmentType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-RecoupmentType").on("click", "#frm-RecoupmentType .btnNew", function () {
        ClearFormRecoupmentType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-RecoupmentType").on("keypress", "#tbl-RecoupmentType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataRecoupmentType(1);
            return false;
        }
    });

    $("#FormList-RecoupmentType").on("change keyup", "#tbl-RecoupmentType tbody tr:first select", function (e) {
        LoadDataRecoupmentType(1);
    });

}

function DataRefreshRecoupmentType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-RecoupmentType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/RecoupmentType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-RecoupmentType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].RecoupmentType.Title + "</td>");

            if ($.inArray("/BaseInformation/RecoupmentType/_Update", RecoupmentTypePermissions) > -1 && $.inArray("/BaseInformation/RecoupmentType/_Delete", RecoupmentTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateRecoupmentType(" + json[i].RecoupmentType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteRecoupmentType'," + json[i].RecoupmentType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/RecoupmentType/_Update", RecoupmentTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateRecoupmentType(" + json[i].RecoupmentType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/RecoupmentType/_Delete", RecoupmentTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteRecoupmentType'," + json[i].RecoupmentType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-RecoupmentType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/RecoupmentType/_Update", RecoupmentTypePermissions) == -1 && $.inArray("/BaseInformation/RecoupmentType/_Delete", RecoupmentTypePermissions) == -1) {
            $('#tbl-RecoupmentType th:last').remove();
            $('#tbl-RecoupmentType tbody tr:first td:last').remove();
            $('#tbl-RecoupmentType tfoot td').attr('colspan', $('#tbl-RecoupmentType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataRecoupmentType(pageRecord) {
    if ($.inArray("/BaseInformation/RecoupmentType/_List", RecoupmentTypePermissions) > -1) {
        var totalRecords = DataRefreshRecoupmentType(pageRecord, $('#tbl-RecoupmentType .page-size').val(), $('#sort-RecoupmentType').val());

        Pager(pageRecord, $('#tbl-RecoupmentType .page-size').val(), "RecoupmentType", totalRecords);
    }
}

function ClearFormRecoupmentType() {
    
    $('#frm-RecoupmentType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/RecoupmentType/_Create", RecoupmentTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-RecoupmentType').validate();
    $('#frm-RecoupmentType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateRecoupmentType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/RecoupmentType/_Update', { Id: id }, '#FormContainer-RecoupmentType', 'UpdateRecoupmentTypeCallback();');
}



function DeleteRecoupmentType(id) {
    Ajax('Post', '/BaseInformation/RecoupmentType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-RecoupmentType tbody tr').length != 2) {
            pageRecord = $('#tbl-RecoupmentType .page-record').val();
        }
        else {
            if ($('#tbl-RecoupmentType .page-record').val() != 1)
                pageRecord = $('#tbl-RecoupmentType .page-record').val() - 1;
        }

        LoadDataRecoupmentType(pageRecord);
    }, 'json');
}