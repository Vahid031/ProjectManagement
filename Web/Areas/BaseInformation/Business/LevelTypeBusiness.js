var LevelTypePermissions;

$(function () {
    LevelTypePermissions = $('#permission-LevelType').val().split(',');
    
    if ($.inArray("/BaseInformation/LevelType/_List", LevelTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/LevelType/_List', '', '#FormList-LevelType', 'ListLevelTypeCallback();');
    }

    if ($.inArray("/BaseInformation/LevelType/_Create", LevelTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/LevelType/_Create', '', '#FormContainer-LevelType', 'CreateLevelTypeCallback();');
    }

    EventHandlerLevelType();
});

function CreateLevelTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateLevelTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListLevelTypeCallback() {
    Pager(1, 5, "LevelType", DataRefreshLevelType(1, 5, $("#sort-LevelType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerLevelType() {
    $("#FormContainer-LevelType").on("submit", "#frm-LevelType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/LevelType/_Create', $('#frm-LevelType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormLevelType();

            if ($('#tbl-LevelType .page-record').val() == null)
                LoadDataLevelType(1);
            else
                LoadDataLevelType($('#tbl-LevelType .page-record').val());

            if ($.inArray("/BaseInformation/LevelType/_Create", LevelTypePermissions) == -1) {
                $('#FormContainer-LevelType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-LevelType").on("click", "#frm-LevelType .btnNew", function () {
        ClearFormLevelType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-LevelType").on("keypress", "#tbl-LevelType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataLevelType(1);
            return false;
        }
    });

    $("#FormList-LevelType").on("change keyup", "#tbl-LevelType tbody tr:first select", function (e) {
        LoadDataLevelType(1);
    });

}

function DataRefreshLevelType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-LevelType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/LevelType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-LevelType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان رتبه'>" + json[i].LevelType.Title + "</td>");

            if ($.inArray("/BaseInformation/LevelType/_Update", LevelTypePermissions) > -1 && $.inArray("/BaseInformation/LevelType/_Delete", LevelTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateLevelType(" + json[i].LevelType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteLevelType'," + json[i].LevelType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/LevelType/_Update", LevelTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateLevelType(" + json[i].LevelType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/LevelType/_Delete", LevelTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteLevelType'," + json[i].LevelType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-LevelType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/LevelType/_Update", LevelTypePermissions) == -1 && $.inArray("/BaseInformation/LevelType/_Delete", LevelTypePermissions) == -1) {
            $('#tbl-LevelType th:last').remove();
            $('#tbl-LevelType tbody tr:first td:last').remove();
            $('#tbl-LevelType tfoot td').attr('colspan', $('#tbl-LevelType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataLevelType(pageRecord) {
    if ($.inArray("/BaseInformation/LevelType/_List", LevelTypePermissions) > -1) {
        var totalRecords = DataRefreshLevelType(pageRecord, $('#tbl-LevelType .page-size').val(), $('#sort-LevelType').val());

        Pager(pageRecord, $('#tbl-LevelType .page-size').val(), "LevelType", totalRecords);
    }
}

function ClearFormLevelType() {
    
    $('#frm-LevelType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/LevelType/_Create", LevelTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-LevelType').validate();
    $('#frm-LevelType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateLevelType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/LevelType/_Update', { Id: id }, '#FormContainer-LevelType', 'UpdateLevelTypeCallback();');
}



function DeleteLevelType(id) {
    Ajax('Post', '/BaseInformation/LevelType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-LevelType tbody tr').length != 2) {
            pageRecord = $('#tbl-LevelType .page-record').val();
        }
        else {
            if ($('#tbl-LevelType .page-record').val() != 1)
                pageRecord = $('#tbl-LevelType .page-record').val() - 1;
        }

        LoadDataLevelType(pageRecord);
    }, 'json');
}