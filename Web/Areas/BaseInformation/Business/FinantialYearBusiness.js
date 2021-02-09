var FinantialYearPermissions;

$(function () {
    FinantialYearPermissions = $('#permission-FinantialYear').val().split(',');
    
    if ($.inArray("/BaseInformation/FinantialYear/_List", FinantialYearPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/FinantialYear/_List', '', '#FormList-FinantialYear', 'ListFinantialYearCallback();');
    }

    if ($.inArray("/BaseInformation/FinantialYear/_Create", FinantialYearPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/FinantialYear/_Create', '', '#FormContainer-FinantialYear', 'CreateFinantialYearCallback();');
    }

    EventHandlerFinantialYear();
});

function CreateFinantialYearCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateFinantialYearCallback() {
    CheckValue();

    HandleValidation();
}

function ListFinantialYearCallback() {
    Pager(1, 5, "FinantialYear", DataRefreshFinantialYear(1, 5, $("#sort-FinantialYear").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerFinantialYear() {
    $("#FormContainer-FinantialYear").on("submit", "#frm-FinantialYear", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/FinantialYear/_Create', $('#frm-FinantialYear').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormFinantialYear();

            if ($('#tbl-FinantialYear .page-record').val() == null)
                LoadDataFinantialYear(1);
            else
                LoadDataFinantialYear($('#tbl-FinantialYear .page-record').val());

            if ($.inArray("/BaseInformation/FinantialYear/_Create", FinantialYearPermissions) == -1) {
                $('#FormContainer-FinantialYear').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-FinantialYear").on("click", "#frm-FinantialYear .btnNew", function () {
        ClearFormFinantialYear();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-FinantialYear").on("keypress", "#tbl-FinantialYear tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataFinantialYear(1);
            return false;
        }
    });

    $("#FormList-FinantialYear").on("change keyup", "#tbl-FinantialYear tbody tr:first select", function (e) {
        LoadDataFinantialYear(1);
    });

}

function DataRefreshFinantialYear(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-FinantialYear').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/FinantialYear/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-FinantialYear tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان سال مالی'>" + json[i].FinantialYear.Title + "</td>");

            if (json[i].FinantialYear.IsActive == true)
                tr.append("<td  data-th='وضعیت'><input type='checkbox' checked disabled></td>");
            else
                tr.append("<td  data-th='وضعیت'><input type='checkbox' disabled></td>");

            if ($.inArray("/BaseInformation/FinantialYear/_Update", FinantialYearPermissions) > -1 && $.inArray("/BaseInformation/FinantialYear/_Delete", FinantialYearPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateFinantialYear(" + json[i].FinantialYear.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteFinantialYear'," + json[i].FinantialYear.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/FinantialYear/_Update", FinantialYearPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateFinantialYear(" + json[i].FinantialYear.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/FinantialYear/_Delete", FinantialYearPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteFinantialYear'," + json[i].FinantialYear.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-FinantialYear tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/FinantialYear/_Update", FinantialYearPermissions) == -1 && $.inArray("/BaseInformation/FinantialYear/_Delete", FinantialYearPermissions) == -1) {
            $('#tbl-FinantialYear th:last').remove();
            $('#tbl-FinantialYear tbody tr:first td:last').remove();
            $('#tbl-FinantialYear tfoot td').attr('colspan', $('#tbl-FinantialYear tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataFinantialYear(pageRecord) {
    if ($.inArray("/BaseInformation/FinantialYear/_List", FinantialYearPermissions) > -1) {
        var totalRecords = DataRefreshFinantialYear(pageRecord, $('#tbl-FinantialYear .page-size').val(), $('#sort-FinantialYear').val());

        Pager(pageRecord, $('#tbl-FinantialYear .page-size').val(), "FinantialYear", totalRecords);
    }
}

function ClearFormFinantialYear() {
    
    $('#frm-FinantialYear input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/FinantialYear/_Create", FinantialYearPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-FinantialYear').validate();
    $('#frm-FinantialYear').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateFinantialYear(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/FinantialYear/_Update', { Id: id }, '#FormContainer-FinantialYear', 'UpdateFinantialYearCallback();');
}



function DeleteFinantialYear(id) {
    Ajax('Post', '/BaseInformation/FinantialYear/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-FinantialYear tbody tr').length != 2) {
            pageRecord = $('#tbl-FinantialYear .page-record').val();
        }
        else {
            if ($('#tbl-FinantialYear .page-record').val() != 1)
                pageRecord = $('#tbl-FinantialYear .page-record').val() - 1;
        }

        LoadDataFinantialYear(pageRecord);
    }, 'json');
}