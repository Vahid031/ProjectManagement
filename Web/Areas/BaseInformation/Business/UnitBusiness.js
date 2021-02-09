var UnitPermissions;

$(function () {
    UnitPermissions = $('#permission-Unit').val().split(',');
    
    if ($.inArray("/BaseInformation/Unit/_List", UnitPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Unit/_List', '', '#FormList-Unit', 'ListUnitCallback();');
    }

    if ($.inArray("/BaseInformation/Unit/_Create", UnitPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Unit/_Create', '', '#FormContainer-Unit', 'CreateUnitCallback();');
    }

    EventHandlerUnit();
});

function CreateUnitCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateUnitCallback() {
    CheckValue();

    HandleValidation();
}

function ListUnitCallback() {
    Pager(1, 5, "Unit", DataRefreshUnit(1, 5, $("#sort-Unit").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerUnit() {
    $("#FormContainer-Unit").on("submit", "#frm-Unit", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Unit/_Create', $('#frm-Unit').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormUnit();

            if ($('#tbl-Unit .page-record').val() == null)
                LoadDataUnit(1);
            else
                LoadDataUnit($('#tbl-Unit .page-record').val());

            if ($.inArray("/BaseInformation/Unit/_Create", UnitPermissions) == -1) {
                $('#FormContainer-Unit').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Unit").on("click", "#frm-Unit .btnNew", function () {
        ClearFormUnit();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Unit").on("keypress", "#tbl-Unit tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataUnit(1);
            return false;
        }
    });

    $("#FormList-Unit").on("change keyup", "#tbl-Unit tbody tr:first select", function (e) {
        LoadDataUnit(1);
    });

}

function DataRefreshUnit(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Unit').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Unit/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Unit tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].Unit.Title + "</td>");

            if ($.inArray("/BaseInformation/Unit/_Update", UnitPermissions) > -1 && $.inArray("/BaseInformation/Unit/_Delete", UnitPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateUnit(" + json[i].Unit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteUnit'," + json[i].Unit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Unit/_Update", UnitPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateUnit(" + json[i].Unit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Unit/_Delete", UnitPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteUnit'," + json[i].Unit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Unit tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Unit/_Update", UnitPermissions) == -1 && $.inArray("/BaseInformation/Unit/_Delete", UnitPermissions) == -1) {
            $('#tbl-Unit th:last').remove();
            $('#tbl-Unit tbody tr:first td:last').remove();
            $('#tbl-Unit tfoot td').attr('colspan', $('#tbl-Unit tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataUnit(pageRecord) {
    if ($.inArray("/BaseInformation/Unit/_List", UnitPermissions) > -1) {
        var totalRecords = DataRefreshUnit(pageRecord, $('#tbl-Unit .page-size').val(), $('#sort-Unit').val());

        Pager(pageRecord, $('#tbl-Unit .page-size').val(), "Unit", totalRecords);
    }
}

function ClearFormUnit() {
    
    $('#frm-Unit input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Unit/_Create", UnitPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Unit').validate();
    $('#frm-Unit').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateUnit(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Unit/_Update', { Id: id }, '#FormContainer-Unit', 'UpdateUnitCallback();');
}



function DeleteUnit(id) {
    Ajax('Post', '/BaseInformation/Unit/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Unit tbody tr').length != 2) {
            pageRecord = $('#tbl-Unit .page-record').val();
        }
        else {
            if ($('#tbl-Unit .page-record').val() != 1)
                pageRecord = $('#tbl-Unit .page-record').val() - 1;
        }

        LoadDataUnit(pageRecord);
    }, 'json');
}