var CreditProvidePlacePermissions;

$(function () {
    CreditProvidePlacePermissions = $('#permission-CreditProvidePlace').val().split(',');
    
    if ($.inArray("/BaseInformation/CreditProvidePlace/_List", CreditProvidePlacePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/CreditProvidePlace/_List', '', '#FormList-CreditProvidePlace', 'ListCreditProvidePlaceCallback();');
    }

    if ($.inArray("/BaseInformation/CreditProvidePlace/_Create", CreditProvidePlacePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/CreditProvidePlace/_Create', '', '#FormContainer-CreditProvidePlace', 'CreateCreditProvidePlaceCallback();');
    }

    EventHandlerCreditProvidePlace();
});

function CreateCreditProvidePlaceCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateCreditProvidePlaceCallback() {
    CheckValue();

    HandleValidation();
}

function ListCreditProvidePlaceCallback() {
    Pager(1, 5, "CreditProvidePlace", DataRefreshCreditProvidePlace(1, 5, $("#sort-CreditProvidePlace").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerCreditProvidePlace() {
    $("#FormContainer-CreditProvidePlace").on("submit", "#frm-CreditProvidePlace", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/CreditProvidePlace/_Create', $('#frm-CreditProvidePlace').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormCreditProvidePlace();

            if ($('#tbl-CreditProvidePlace .page-record').val() == null)
                LoadDataCreditProvidePlace(1);
            else
                LoadDataCreditProvidePlace($('#tbl-CreditProvidePlace .page-record').val());

            if ($.inArray("/BaseInformation/CreditProvidePlace/_Create", CreditProvidePlacePermissions) == -1) {
                $('#FormContainer-CreditProvidePlace').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-CreditProvidePlace").on("click", "#frm-CreditProvidePlace .btnNew", function () {
        ClearFormCreditProvidePlace();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-CreditProvidePlace").on("keypress", "#tbl-CreditProvidePlace tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataCreditProvidePlace(1);
            return false;
        }
    });

    $("#FormList-CreditProvidePlace").on("change keyup", "#tbl-CreditProvidePlace tbody tr:first select", function (e) {
        LoadDataCreditProvidePlace(1);
    });

}

function DataRefreshCreditProvidePlace(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-CreditProvidePlace').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/CreditProvidePlace/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-CreditProvidePlace tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].CreditProvidePlace.Title + "</td>");

            if ($.inArray("/BaseInformation/CreditProvidePlace/_Update", CreditProvidePlacePermissions) > -1 && $.inArray("/BaseInformation/CreditProvidePlace/_Delete", CreditProvidePlacePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateCreditProvidePlace(" + json[i].CreditProvidePlace.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteCreditProvidePlace'," + json[i].CreditProvidePlace.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/CreditProvidePlace/_Update", CreditProvidePlacePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateCreditProvidePlace(" + json[i].CreditProvidePlace.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/CreditProvidePlace/_Delete", CreditProvidePlacePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteCreditProvidePlace'," + json[i].CreditProvidePlace.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-CreditProvidePlace tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/CreditProvidePlace/_Update", CreditProvidePlacePermissions) == -1 && $.inArray("/BaseInformation/CreditProvidePlace/_Delete", CreditProvidePlacePermissions) == -1) {
            $('#tbl-CreditProvidePlace th:last').remove();
            $('#tbl-CreditProvidePlace tbody tr:first td:last').remove();
            $('#tbl-CreditProvidePlace tfoot td').attr('colspan', $('#tbl-CreditProvidePlace tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataCreditProvidePlace(pageRecord) {
    if ($.inArray("/BaseInformation/CreditProvidePlace/_List", CreditProvidePlacePermissions) > -1) {
        var totalRecords = DataRefreshCreditProvidePlace(pageRecord, $('#tbl-CreditProvidePlace .page-size').val(), $('#sort-CreditProvidePlace').val());

        Pager(pageRecord, $('#tbl-CreditProvidePlace .page-size').val(), "CreditProvidePlace", totalRecords);
    }
}

function ClearFormCreditProvidePlace() {
    
    $('#frm-CreditProvidePlace input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/CreditProvidePlace/_Create", CreditProvidePlacePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-CreditProvidePlace').validate();
    $('#frm-CreditProvidePlace').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateCreditProvidePlace(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/CreditProvidePlace/_Update', { Id: id }, '#FormContainer-CreditProvidePlace', 'UpdateCreditProvidePlaceCallback();');
}



function DeleteCreditProvidePlace(id) {
    Ajax('Post', '/BaseInformation/CreditProvidePlace/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-CreditProvidePlace tbody tr').length != 2) {
            pageRecord = $('#tbl-CreditProvidePlace .page-record').val();
        }
        else {
            if ($('#tbl-CreditProvidePlace .page-record').val() != 1)
                pageRecord = $('#tbl-CreditProvidePlace .page-record').val() - 1;
        }

        LoadDataCreditProvidePlace(pageRecord);
    }, 'json');
}