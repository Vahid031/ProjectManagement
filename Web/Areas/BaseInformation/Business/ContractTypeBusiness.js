var ContractTypePermissions;

$(function () {
    ContractTypePermissions = $('#permission-ContractType').val().split(',');
    
    if ($.inArray("/BaseInformation/ContractType/_List", ContractTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ContractType/_List', '', '#FormList-ContractType', 'ListContractTypeCallback();');
    }

    if ($.inArray("/BaseInformation/ContractType/_Create", ContractTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ContractType/_Create', '', '#FormContainer-ContractType', 'CreateContractTypeCallback();');
    }

    EventHandlerContractType();
});

function CreateContractTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateContractTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListContractTypeCallback() {
    Pager(1, 5, "ContractType", DataRefreshContractType(1, 5, $("#sort-ContractType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerContractType() {
    $("#FormContainer-ContractType").on("submit", "#frm-ContractType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/ContractType/_Create', $('#frm-ContractType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormContractType();

            if ($('#tbl-ContractType .page-record').val() == null)
                LoadDataContractType(1);
            else
                LoadDataContractType($('#tbl-ContractType .page-record').val());

            if ($.inArray("/BaseInformation/ContractType/_Create", ContractTypePermissions) == -1) {
                $('#FormContainer-ContractType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-ContractType").on("click", "#frm-ContractType .btnNew", function () {
        ClearFormContractType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ContractType").on("keypress", "#tbl-ContractType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataContractType(1);
            return false;
        }
    });

    $("#FormList-ContractType").on("change keyup", "#tbl-ContractType tbody tr:first select", function (e) {
        LoadDataContractType(1);
    });

}

function DataRefreshContractType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ContractType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/ContractType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ContractType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].ContractType.Title + "</td>");

            if ($.inArray("/BaseInformation/ContractType/_Update", ContractTypePermissions) > -1 && $.inArray("/BaseInformation/ContractType/_Delete", ContractTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateContractType(" + json[i].ContractType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteContractType'," + json[i].ContractType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ContractType/_Update", ContractTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateContractType(" + json[i].ContractType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ContractType/_Delete", ContractTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteContractType'," + json[i].ContractType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ContractType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/ContractType/_Update", ContractTypePermissions) == -1 && $.inArray("/BaseInformation/ContractType/_Delete", ContractTypePermissions) == -1) {
            $('#tbl-ContractType th:last').remove();
            $('#tbl-ContractType tbody tr:first td:last').remove();
            $('#tbl-ContractType tfoot td').attr('colspan', $('#tbl-ContractType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataContractType(pageRecord) {
    if ($.inArray("/BaseInformation/ContractType/_List", ContractTypePermissions) > -1) {
        var totalRecords = DataRefreshContractType(pageRecord, $('#tbl-ContractType .page-size').val(), $('#sort-ContractType').val());

        Pager(pageRecord, $('#tbl-ContractType .page-size').val(), "ContractType", totalRecords);
    }
}

function ClearFormContractType() {
    
    $('#frm-ContractType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/ContractType/_Create", ContractTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ContractType').validate();
    $('#frm-ContractType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateContractType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/ContractType/_Update', { Id: id }, '#FormContainer-ContractType', 'UpdateContractTypeCallback();');
}



function DeleteContractType(id) {
    Ajax('Post', '/BaseInformation/ContractType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ContractType tbody tr').length != 2) {
            pageRecord = $('#tbl-ContractType .page-record').val();
        }
        else {
            if ($('#tbl-ContractType .page-record').val() != 1)
                pageRecord = $('#tbl-ContractType .page-record').val() - 1;
        }

        LoadDataContractType(pageRecord);
    }, 'json');
}