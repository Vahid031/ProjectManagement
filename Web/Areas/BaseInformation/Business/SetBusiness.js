var SetPermissions;

$(function () {
    SetPermissions = $('#permission-Set').val().split(',');
    
    if ($.inArray("/BaseInformation/Set/_List", SetPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Set/_List', '', '#FormList-Set', 'ListSetCallback();');
    }

    if ($.inArray("/BaseInformation/Set/_Create", SetPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Set/_Create', '', '#FormContainer-Set', 'CreateSetCallback();');
    }

    EventHandlerSet();
});

function CreateSetCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateSetCallback() {
    CheckValue();

    HandleValidation();
}

function ListSetCallback() {
    Pager(1, 5, "Set", DataRefreshSet(1, 5, $("#sort-Set").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerSet() {
    $("#FormContainer-Set").on("submit", "#frm-Set", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Set/_Create', $('#frm-Set').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormSet();

            if ($('#tbl-Set .page-record').val() == null)
                LoadDataSet(1);
            else
                LoadDataSet($('#tbl-Set .page-record').val());

            if ($.inArray("/BaseInformation/Set/_Create", SetPermissions) == -1) {
                $('#FormContainer-Set').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Set").on("click", "#frm-Set .btnNew", function () {
        ClearFormSet();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Set").on("keypress", "#tbl-Set tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataSet(1);
            return false;
        }
    });

    $("#FormList-Set").on("change keyup", "#tbl-Set tbody tr:first select", function (e) {
        LoadDataSet(1);
    });

}

function DataRefreshSet(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Set').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Set/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Set tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد'>" + json[i].Set.Code + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].Set.Title + "</td>");

            if ($.inArray("/BaseInformation/Set/_Update", SetPermissions) > -1 && $.inArray("/BaseInformation/Set/_Delete", SetPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateSet(" + json[i].Set.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteSet'," + json[i].Set.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Set/_Update", SetPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateSet(" + json[i].Set.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Set/_Delete", SetPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteSet'," + json[i].Set.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Set tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Set/_Update", SetPermissions) == -1 && $.inArray("/BaseInformation/Set/_Delete", SetPermissions) == -1) {
            $('#tbl-Set th:last').remove();
            $('#tbl-Set tbody tr:first td:last').remove();
            $('#tbl-Set tfoot td').attr('colspan', $('#tbl-Set tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataSet(pageRecord) {
    if ($.inArray("/BaseInformation/Set/_List", SetPermissions) > -1) {
        var totalRecords = DataRefreshSet(pageRecord, $('#tbl-Set .page-size').val(), $('#sort-Set').val());

        Pager(pageRecord, $('#tbl-Set .page-size').val(), "Set", totalRecords);
    }
}

function ClearFormSet() {
    
    $('#frm-Set input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Set/_Create", SetPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Set').validate();
    $('#frm-Set').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateSet(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Set/_Update', { Id: id }, '#FormContainer-Set', 'UpdateSetCallback();');
}



function DeleteSet(id) {
    Ajax('Post', '/BaseInformation/Set/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Set tbody tr').length != 2) {
            pageRecord = $('#tbl-Set .page-record').val();
        }
        else {
            if ($('#tbl-Set .page-record').val() != 1)
                pageRecord = $('#tbl-Set .page-record').val() - 1;
        }

        LoadDataSet(pageRecord);
    }, 'json');
}