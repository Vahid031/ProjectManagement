var MonitoringTypePermissions;

$(function () {
    MonitoringTypePermissions = $('#permission-MonitoringType').val().split(',');
    
    if ($.inArray("/BaseInformation/MonitoringType/_List", MonitoringTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/MonitoringType/_List', '', '#FormList-MonitoringType', 'ListMonitoringTypeCallback();');
    }

    if ($.inArray("/BaseInformation/MonitoringType/_Create", MonitoringTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/MonitoringType/_Create', '', '#FormContainer-MonitoringType', 'CreateMonitoringTypeCallback();');
    }

    EventHandlerMonitoringType();
});

function CreateMonitoringTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateMonitoringTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListMonitoringTypeCallback() {
    Pager(1, 5, "MonitoringType", DataRefreshMonitoringType(1, 5, $("#sort-MonitoringType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerMonitoringType() {
    $("#FormContainer-MonitoringType").on("submit", "#frm-MonitoringType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/MonitoringType/_Create', $('#frm-MonitoringType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormMonitoringType();

            if ($('#tbl-MonitoringType .page-record').val() == null)
                LoadDataMonitoringType(1);
            else
                LoadDataMonitoringType($('#tbl-MonitoringType .page-record').val());

            if ($.inArray("/BaseInformation/MonitoringType/_Create", MonitoringTypePermissions) == -1) {
                $('#FormContainer-MonitoringType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-MonitoringType").on("click", "#frm-MonitoringType .btnNew", function () {
        ClearFormMonitoringType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-MonitoringType").on("keypress", "#tbl-MonitoringType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataMonitoringType(1);
            return false;
        }
    });

    $("#FormList-MonitoringType").on("change keyup", "#tbl-MonitoringType tbody tr:first select", function (e) {
        LoadDataMonitoringType(1);
    });

}

function DataRefreshMonitoringType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-MonitoringType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/MonitoringType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-MonitoringType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].MonitoringType.Title + "</td>");

            if ($.inArray("/BaseInformation/MonitoringType/_Update", MonitoringTypePermissions) > -1 && $.inArray("/BaseInformation/MonitoringType/_Delete", MonitoringTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateMonitoringType(" + json[i].MonitoringType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteMonitoringType'," + json[i].MonitoringType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/MonitoringType/_Update", MonitoringTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateMonitoringType(" + json[i].MonitoringType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/MonitoringType/_Delete", MonitoringTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteMonitoringType'," + json[i].MonitoringType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-MonitoringType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/MonitoringType/_Update", MonitoringTypePermissions) == -1 && $.inArray("/BaseInformation/MonitoringType/_Delete", MonitoringTypePermissions) == -1) {
            $('#tbl-MonitoringType th:last').remove();
            $('#tbl-MonitoringType tbody tr:first td:last').remove();
            $('#tbl-MonitoringType tfoot td').attr('colspan', $('#tbl-MonitoringType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataMonitoringType(pageRecord) {
    if ($.inArray("/BaseInformation/MonitoringType/_List", MonitoringTypePermissions) > -1) {
        var totalRecords = DataRefreshMonitoringType(pageRecord, $('#tbl-MonitoringType .page-size').val(), $('#sort-MonitoringType').val());

        Pager(pageRecord, $('#tbl-MonitoringType .page-size').val(), "MonitoringType", totalRecords);
    }
}

function ClearFormMonitoringType() {
    
    $('#frm-MonitoringType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/MonitoringType/_Create", MonitoringTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-MonitoringType').validate();
    $('#frm-MonitoringType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateMonitoringType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/MonitoringType/_Update', { Id: id }, '#FormContainer-MonitoringType', 'UpdateMonitoringTypeCallback();');
}



function DeleteMonitoringType(id) {
    Ajax('Post', '/BaseInformation/MonitoringType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-MonitoringType tbody tr').length != 2) {
            pageRecord = $('#tbl-MonitoringType .page-record').val();
        }
        else {
            if ($('#tbl-MonitoringType .page-record').val() != 1)
                pageRecord = $('#tbl-MonitoringType .page-record').val() - 1;
        }

        LoadDataMonitoringType(pageRecord);
    }, 'json');
}