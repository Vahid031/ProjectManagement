var MajorTypePermissions;

$(function () {
    MajorTypePermissions = $('#permission-MajorType').val().split(',');
    
    if ($.inArray("/BaseInformation/MajorType/_List", MajorTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/MajorType/_List', '', '#FormList-MajorType', 'ListMajorTypeCallback();');
    }

    if ($.inArray("/BaseInformation/MajorType/_Create", MajorTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/MajorType/_Create', '', '#FormContainer-MajorType', 'CreateMajorTypeCallback();');
    }

    EventHandlerMajorType();
});

function CreateMajorTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateMajorTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListMajorTypeCallback() {
    Pager(1, 5, "MajorType", DataRefreshMajorType(1, 5, $("#sort-MajorType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerMajorType() {
    $("#FormContainer-MajorType").on("submit", "#frm-MajorType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/MajorType/_Create', $('#frm-MajorType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormMajorType();

            if ($('#tbl-MajorType .page-record').val() == null)
                LoadDataMajorType(1);
            else
                LoadDataMajorType($('#tbl-MajorType .page-record').val());

            if ($.inArray("/BaseInformation/MajorType/_Create", MajorTypePermissions) == -1) {
                $('#FormContainer-MajorType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-MajorType").on("click", "#frm-MajorType .btnNew", function () {
        ClearFormMajorType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-MajorType").on("keypress", "#tbl-MajorType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataMajorType(1);
            return false;
        }
    });

    $("#FormList-MajorType").on("change keyup", "#tbl-MajorType tbody tr:first select", function (e) {
        LoadDataMajorType(1);
    });

}

function DataRefreshMajorType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-MajorType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/MajorType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-MajorType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان رتبه'>" + json[i].MajorType.Title + "</td>");

            if ($.inArray("/BaseInformation/MajorType/_Update", MajorTypePermissions) > -1 && $.inArray("/BaseInformation/MajorType/_Delete", MajorTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateMajorType(" + json[i].MajorType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteMajorType'," + json[i].MajorType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/MajorType/_Update", MajorTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateMajorType(" + json[i].MajorType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/MajorType/_Delete", MajorTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteMajorType'," + json[i].MajorType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-MajorType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/MajorType/_Update", MajorTypePermissions) == -1 && $.inArray("/BaseInformation/MajorType/_Delete", MajorTypePermissions) == -1) {
            $('#tbl-MajorType th:last').remove();
            $('#tbl-MajorType tbody tr:first td:last').remove();
            $('#tbl-MajorType tfoot td').attr('colspan', $('#tbl-MajorType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataMajorType(pageRecord) {
    if ($.inArray("/BaseInformation/MajorType/_List", MajorTypePermissions) > -1) {
        var totalRecords = DataRefreshMajorType(pageRecord, $('#tbl-MajorType .page-size').val(), $('#sort-MajorType').val());

        Pager(pageRecord, $('#tbl-MajorType .page-size').val(), "MajorType", totalRecords);
    }
}

function ClearFormMajorType() {
    
    $('#frm-MajorType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/MajorType/_Create", MajorTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-MajorType').validate();
    $('#frm-MajorType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateMajorType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/MajorType/_Update', { Id: id }, '#FormContainer-MajorType', 'UpdateMajorTypeCallback();');
}



function DeleteMajorType(id) {
    Ajax('Post', '/BaseInformation/MajorType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-MajorType tbody tr').length != 2) {
            pageRecord = $('#tbl-MajorType .page-record').val();
        }
        else {
            if ($('#tbl-MajorType .page-record').val() != 1)
                pageRecord = $('#tbl-MajorType .page-record').val() - 1;
        }

        LoadDataMajorType(pageRecord);
    }, 'json');
}