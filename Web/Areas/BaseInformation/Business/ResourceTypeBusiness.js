
$(function () {
    
});

function IndexResourceTypeCallback(Id) {
    if ($.inArray("/BaseInformation/ResourceType/_Create", ResourcePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ResourceType/_Create', '', '#FormContainer-ResourceType', 'CreateResourceTypeCallback(' + Id + ');');
    }

    if ($.inArray("/BaseInformation/ResourceType/_List", ResourcePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ResourceType/_List', '', '#FormList-ResourceType', 'ListResourceTypeCallback();');
    }

    EventHandlerResourceType();
}

function CreateResourceTypeCallback(Id) {
    $('#ResourceType_ResourceId').val(Id);
    
    CheckValue();

    HandleValidation();
}

function UpdateResourceTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListResourceTypeCallback() {
    Pager(1, 5, "ResourceType", DataRefreshResourceType(1, 5, $("#sort-ResourceType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerResourceType() {
    $("#FormContainer-ResourceType").on("submit", "#frm-ResourceType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/ResourceType/_Create', $('#frm-ResourceType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormResourceType();

            if ($('#tbl-ResourceType .page-record').val() == null)
                LoadDataResourceType(1);
            else
                LoadDataResourceType($('#tbl-ResourceType .page-record').val());

            if ($.inArray("/BaseInformation/ResourceType/_Create", ResourcePermissions) == -1) {
                $('#FormContainer-ResourceType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-ResourceType").on("click", "#frm-ResourceType .btnNew", function () {
        ClearFormResourceType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ResourceType").on("keypress", "#tbl-ResourceType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataResourceType(1);
            return false;
        }
    });

    $("#FormList-ResourceType").on("change keyup", "#tbl-ResourceType tbody tr:first select", function (e) {
        LoadDataResourceType(1);
    });

}

function DataRefreshResourceType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ResourceType.ResourceId=' + $('#ResourceType_ResourceId').val() + "&" + $('#frm-tbl-ResourceType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/ResourceType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ResourceType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ResourceType.Title)'>" + json[i].ResourceType.Title + "</td>");

            if ($.inArray("/BaseInformation/ResourceType/_Update", ResourcePermissions) > -1 && $.inArray("/BaseInformation/ResourceType/_Delete", ResourcePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateResourceType(" + json[i].ResourceType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteResourceType'," + json[i].ResourceType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ResourceType/_Update", ResourcePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateResourceType(" + json[i].ResourceType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ResourceType/_Delete", ResourcePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteResourceType'," + json[i].ResourceType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ResourceType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/ResourceType/_Update", ResourcePermissions) == -1 && $.inArray("/BaseInformation/ResourceType/_Delete", ResourcePermissions) == -1) {
            $('#tbl-ResourceType th:last').remove();
            $('#tbl-ResourceType tbody tr:first td:last').remove();
            $('#tbl-ResourceType tfoot td').attr('colspan', $('#tbl-ResourceType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataResourceType(pageRecord) {
    if ($.inArray("/BaseInformation/ResourceType/_List", ResourcePermissions) > -1) {
        var totalRecords = DataRefreshResourceType(pageRecord, $('#tbl-ResourceType .page-size').val(), $('#sort-ResourceType').val());

        Pager(pageRecord, $('#tbl-ResourceType .page-size').val(), "ResourceType", totalRecords);
    }
}

function ClearFormResourceType() {
    
    $('#frm-ResourceType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/ResourceType/_Create", ResourcePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ResourceType').validate();
    $('#frm-ResourceType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateResourceType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/ResourceType/_Update', { Id: id }, '#FormContainer-ResourceType', 'UpdateResourceTypeCallback();');
}



function DeleteResourceType(id) {
    Ajax('Post', '/BaseInformation/ResourceType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ResourceType tbody tr').length != 2) {
            pageRecord = $('#tbl-ResourceType .page-record').val();
        }
        else {
            if ($('#tbl-ResourceType .page-record').val() != 1)
                pageRecord = $('#tbl-ResourceType .page-record').val() - 1;
        }

        LoadDataResourceType(pageRecord);
    }, 'json');
}