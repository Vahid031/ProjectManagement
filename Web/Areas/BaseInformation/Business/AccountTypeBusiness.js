var AccountTypePermissions;

$(function () {
    AccountTypePermissions = $('#permission-AccountType').val().split(',');
    
    if ($.inArray("/BaseInformation/AccountType/_List", AccountTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/AccountType/_List', '', '#FormList-AccountType', 'ListAccountTypeCallback();');
    }

    if ($.inArray("/BaseInformation/AccountType/_Create", AccountTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/AccountType/_Create', '', '#FormContainer-AccountType', 'CreateAccountTypeCallback();');
    }

    EventHandlerAccountType();
});

function CreateAccountTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateAccountTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListAccountTypeCallback() {
    Pager(1, 5, "AccountType", DataRefreshAccountType(1, 5, $("#sort-AccountType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerAccountType() {
    $("#FormContainer-AccountType").on("submit", "#frm-AccountType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/AccountType/_Create', $('#frm-AccountType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormAccountType();

            if ($('#tbl-AccountType .page-record').val() == null)
                LoadDataAccountType(1);
            else
                LoadDataAccountType($('#tbl-AccountType .page-record').val());

            if ($.inArray("/BaseInformation/AccountType/_Create", AccountTypePermissions) == -1) {
                $('#FormContainer-AccountType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-AccountType").on("click", "#frm-AccountType .btnNew", function () {
        ClearFormAccountType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-AccountType").on("keypress", "#tbl-AccountType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataAccountType(1);
            return false;
        }
    });

    $("#FormList-AccountType").on("change keyup", "#tbl-AccountType tbody tr:first select", function (e) {
        LoadDataAccountType(1);
    });

}

function DataRefreshAccountType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-AccountType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/AccountType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-AccountType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.AccountType.Title)'>" + json[i].AccountType.Title + "</td>");

            if ($.inArray("/BaseInformation/AccountType/_Update", AccountTypePermissions) > -1 && $.inArray("/BaseInformation/AccountType/_Delete", AccountTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateAccountType(" + json[i].AccountType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteAccountType'," + json[i].AccountType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/AccountType/_Update", AccountTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateAccountType(" + json[i].AccountType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/AccountType/_Delete", AccountTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteAccountType'," + json[i].AccountType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-AccountType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/AccountType/_Update", AccountTypePermissions) == -1 && $.inArray("/BaseInformation/AccountType/_Delete", AccountTypePermissions) == -1) {
            $('#tbl-AccountType th:last').remove();
            $('#tbl-AccountType tbody tr:first td:last').remove();
            $('#tbl-AccountType tfoot td').attr('colspan', $('#tbl-AccountType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataAccountType(pageRecord) {
    if ($.inArray("/BaseInformation/AccountType/_List", AccountTypePermissions) > -1) {
        var totalRecords = DataRefreshAccountType(pageRecord, $('#tbl-AccountType .page-size').val(), $('#sort-AccountType').val());

        Pager(pageRecord, $('#tbl-AccountType .page-size').val(), "AccountType", totalRecords);
    }
}

function ClearFormAccountType() {
    
    $('#frm-AccountType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/AccountType/_Create", AccountTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-AccountType').validate();
    $('#frm-AccountType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateAccountType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/AccountType/_Update', { Id: id }, '#FormContainer-AccountType', 'UpdateAccountTypeCallback();');
}



function DeleteAccountType(id) {
    Ajax('Post', '/BaseInformation/AccountType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-AccountType tbody tr').length != 2) {
            pageRecord = $('#tbl-AccountType .page-record').val();
        }
        else {
            if ($('#tbl-AccountType .page-record').val() != 1)
                pageRecord = $('#tbl-AccountType .page-record').val() - 1;
        }

        LoadDataAccountType(pageRecord);
    }, 'json');
}