var StatePermissions;

$(function () {
    StatePermissions = $('#permission-State').val().split(',');
    
    if ($.inArray("/BaseInformation/State/_List", StatePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/State/_List', '', '#FormList-State', 'ListStateCallback();');
    }

    if ($.inArray("/BaseInformation/State/_Create", StatePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/State/_Create', '', '#FormContainer-State', 'CreateStateCallback();');
    }

    EventHandlerState();
});

function CreateStateCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateStateCallback() {
    CheckValue();

    HandleValidation();
}

function ListStateCallback() {
    Pager(1, 5, "State", DataRefreshState(1, 5, $("#sort-State").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerState() {
    $("#FormContainer-State").on("submit", "#frm-State", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/State/_Create', $('#frm-State').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormState();

            if ($('#tbl-State .page-record').val() == null)
                LoadDataState(1);
            else
                LoadDataState($('#tbl-State .page-record').val());

            if ($.inArray("/BaseInformation/State/_Create", StatePermissions) == -1) {
                $('#FormContainer-State').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-State").on("click", "#frm-State .btnNew", function () {
        ClearFormState();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-State").on("keypress", "#tbl-State tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataState(1);
            return false;
        }
    });

    $("#FormList-State").on("change keyup", "#tbl-State tbody tr:first select", function (e) {
        LoadDataState(1);
    });

}

function DataRefreshState(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-State').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/State/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-State tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد استان'>" + json[i].State.Code + "</td>");
            tr.append("<td data-th='عنوان استان'>" + json[i].State.Title + "</td>");

            if ($.inArray("/BaseInformation/State/_Update", StatePermissions) > -1 && $.inArray("/BaseInformation/State/_Delete", StatePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateState(" + json[i].State.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteState'," + json[i].State.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/State/_Update", StatePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateState(" + json[i].State.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/State/_Delete", StatePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteState'," + json[i].State.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-State tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/State/_Update", StatePermissions) == -1 && $.inArray("/BaseInformation/State/_Delete", StatePermissions) == -1) {
            $('#tbl-State th:last').remove();
            $('#tbl-State tbody tr:first td:last').remove();
            $('#tbl-State tfoot td').attr('colspan', $('#tbl-State tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataState(pageRecord) {
    if ($.inArray("/BaseInformation/State/_List", StatePermissions) > -1) {
        var totalRecords = DataRefreshState(pageRecord, $('#tbl-State .page-size').val(), $('#sort-State').val());

        Pager(pageRecord, $('#tbl-State .page-size').val(), "State", totalRecords);
    }
}

function ClearFormState() {
    
    $('#frm-State input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/State/_Create", StatePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-State').validate();
    $('#frm-State').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateState(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/State/_Update', { Id: id }, '#FormContainer-State', 'UpdateStateCallback();');
}



function DeleteState(id) {
    Ajax('Post', '/BaseInformation/State/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-State tbody tr').length != 2) {
            pageRecord = $('#tbl-State .page-record').val();
        }
        else {
            if ($('#tbl-State .page-record').val() != 1)
                pageRecord = $('#tbl-State .page-record').val() - 1;
        }

        LoadDataState(pageRecord);
    }, 'json');
}