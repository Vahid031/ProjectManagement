var DeductionPercentPermissions;

$(function () {
    DeductionPercentPermissions = $('#permission-DeductionPercent').val().split(',');
    
    if ($.inArray("/BaseInformation/DeductionPercent/_List", DeductionPercentPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/DeductionPercent/_List', '', '#FormList-DeductionPercent', 'ListDeductionPercentCallback();');
    }

    if ($.inArray("/BaseInformation/DeductionPercent/_Create", DeductionPercentPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/DeductionPercent/_Create', '', '#FormContainer-DeductionPercent', 'CreateDeductionPercentCallback();');
    }

    EventHandlerDeductionPercent();
});

function CreateDeductionPercentCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateDeductionPercentCallback() {
    CheckValue();

    HandleValidation();
}

function ListDeductionPercentCallback() {
    Pager(1, 5, "DeductionPercent", DataRefreshDeductionPercent(1, 5, $("#sort-DeductionPercent").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerDeductionPercent() {
    $("#FormContainer-DeductionPercent").on("submit", "#frm-DeductionPercent", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/DeductionPercent/_Create', $('#frm-DeductionPercent').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormDeductionPercent();

            if ($('#tbl-DeductionPercent .page-record').val() == null)
                LoadDataDeductionPercent(1);
            else
                LoadDataDeductionPercent($('#tbl-DeductionPercent .page-record').val());

            if ($.inArray("/BaseInformation/DeductionPercent/_Create", DeductionPercentPermissions) == -1) {
                $('#FormContainer-DeductionPercent').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-DeductionPercent").on("click", "#frm-DeductionPercent .btnNew", function () {
        ClearFormDeductionPercent();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-DeductionPercent").on("keypress", "#tbl-DeductionPercent tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataDeductionPercent(1);
            return false;
        }
    });

    $("#FormList-DeductionPercent").on("change keyup", "#tbl-DeductionPercent tbody tr:first select", function (e) {
        LoadDataDeductionPercent(1);
    });

}

function DataRefreshDeductionPercent(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-DeductionPercent').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/DeductionPercent/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-DeductionPercent tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].DeductionPercent.Title + "</td>");
            tr.append("<td data-th='درصد کسری'>" + json[i].DeductionPercent.PercentAmount + "</td>");

            if ($.inArray("/BaseInformation/DeductionPercent/_Update", DeductionPercentPermissions) > -1 && $.inArray("/BaseInformation/DeductionPercent/_Delete", DeductionPercentPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateDeductionPercent(" + json[i].DeductionPercent.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteDeductionPercent'," + json[i].DeductionPercent.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/DeductionPercent/_Update", DeductionPercentPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateDeductionPercent(" + json[i].DeductionPercent.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/DeductionPercent/_Delete", DeductionPercentPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteDeductionPercent'," + json[i].DeductionPercent.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-DeductionPercent tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/DeductionPercent/_Update", DeductionPercentPermissions) == -1 && $.inArray("/BaseInformation/DeductionPercent/_Delete", DeductionPercentPermissions) == -1) {
            $('#tbl-DeductionPercent th:last').remove();
            $('#tbl-DeductionPercent tbody tr:first td:last').remove();
            $('#tbl-DeductionPercent tfoot td').attr('colspan', $('#tbl-DeductionPercent tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataDeductionPercent(pageRecord) {
    if ($.inArray("/BaseInformation/DeductionPercent/_List", DeductionPercentPermissions) > -1) {
        var totalRecords = DataRefreshDeductionPercent(pageRecord, $('#tbl-DeductionPercent .page-size').val(), $('#sort-DeductionPercent').val());

        Pager(pageRecord, $('#tbl-DeductionPercent .page-size').val(), "DeductionPercent", totalRecords);
    }
}

function ClearFormDeductionPercent() {
    
    $('#frm-DeductionPercent input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/DeductionPercent/_Create", DeductionPercentPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-DeductionPercent').validate();
    $('#frm-DeductionPercent').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateDeductionPercent(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/DeductionPercent/_Update', { Id: id }, '#FormContainer-DeductionPercent', 'UpdateDeductionPercentCallback();');
}



function DeleteDeductionPercent(id) {
    Ajax('Post', '/BaseInformation/DeductionPercent/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-DeductionPercent tbody tr').length != 2) {
            pageRecord = $('#tbl-DeductionPercent .page-record').val();
        }
        else {
            if ($('#tbl-DeductionPercent .page-record').val() != 1)
                pageRecord = $('#tbl-DeductionPercent .page-record').val() - 1;
        }

        LoadDataDeductionPercent(pageRecord);
    }, 'json');
}