var ResourcePermissions;

$(function () {
    ResourcePermissions = $('#permission-Resource').val().split(',');

    if ($.inArray("/BaseInformation/Resource/_List", ResourcePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Resource/_List', '', '#FormList-Resource', 'ListResourceCallback();');
    }

    if ($.inArray("/BaseInformation/Resource/_Create", ResourcePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Resource/_Create', '', '#FormContainer-Resource', 'CreateResourceCallback();');
    }

    EventHandlerResource();
});

function CreateResourceCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateResourceCallback() {
    CheckValue();

    HandleValidation();
}

function ListResourceCallback() {
    Pager(1, 5, "Resource", DataRefreshResource(1, 5, $("#sort-Resource").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerResource() {
    $("#FormContainer-Resource").on("submit", "#frm-Resource", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Resource/_Create', $('#frm-Resource').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormResource();

            if ($('#tbl-Resource .page-record').val() == null)
                LoadDataResource(1);
            else
                LoadDataResource($('#tbl-Resource .page-record').val());

            if ($.inArray("/BaseInformation/Resource/_Create", ResourcePermissions) == -1) {
                $('#FormContainer-Resource').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Resource").on("click", "#frm-Resource .btnNew", function () {
        ClearFormResource();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Resource").on("keypress", "#tbl-Resource tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataResource(1);
            return false;
        }
    });

    $("#FormList-Resource").on("change keyup", "#tbl-Resource tbody tr:first select", function (e) {
        LoadDataResource(1);
    });

}

function DataRefreshResource(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Resource').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Resource/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Resource tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].Resource.Title + "</td>");

            if ($.inArray("/BaseInformation/ResourceType/_List", ResourcePermissions) > -1) {
                tr.append("<td data-th='انتخاب'><a onmousedown = 'PopupFormHtml(\"نوع منابع\", \"/BaseInformation/ResourceType/_Index\", \"IndexResourceTypeCallback(" + json[i].Resource.Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='افزودن'></a></td>");
            }

            if ($.inArray("/BaseInformation/Resource/_Update", ResourcePermissions) > -1 && $.inArray("/BaseInformation/Resource/_Delete", ResourcePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateResource(" + json[i].Resource.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteResource'," + json[i].Resource.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Resource/_Update", ResourcePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateResource(" + json[i].Resource.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Resource/_Delete", ResourcePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteResource'," + json[i].Resource.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Resource tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Resource/_Update", ResourcePermissions) == -1 && $.inArray("/BaseInformation/Resource/_Delete", ResourcePermissions) == -1) {
            $('#tbl-Resource th:last').remove();
            $('#tbl-Resource tbody tr:first td:last').remove();
            $('#tbl-Resource tfoot td').attr('colspan', $('#tbl-Resource tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataResource(pageRecord) {
    if ($.inArray("/BaseInformation/Resource/_List", ResourcePermissions) > -1) {
        var totalRecords = DataRefreshResource(pageRecord, $('#tbl-Resource .page-size').val(), $('#sort-Resource').val());

        Pager(pageRecord, $('#tbl-Resource .page-size').val(), "Resource", totalRecords);
    }
}

function ClearFormResource() {
    
    $('#frm-Resource input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Resource/_Create", ResourcePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Resource').validate();
    $('#frm-Resource').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateResource(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Resource/_Update', { Id: id }, '#FormContainer-Resource', 'UpdateResourceCallback();');
}



function DeleteResource(id) {
    Ajax('Post', '/BaseInformation/Resource/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Resource tbody tr').length != 2) {
            pageRecord = $('#tbl-Resource .page-record').val();
        }
        else {
            if ($('#tbl-Resource .page-record').val() != 1)
                pageRecord = $('#tbl-Resource .page-record').val() - 1;
        }

        LoadDataResource(pageRecord);
    }, 'json');
}