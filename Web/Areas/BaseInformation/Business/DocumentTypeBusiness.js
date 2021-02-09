var DocumentTypePermissions;

$(function () {
    DocumentTypePermissions = $('#permission-DocumentType').val().split(',');
    
    if ($.inArray("/BaseInformation/DocumentType/_List", DocumentTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/DocumentType/_List', '', '#FormList-DocumentType', 'ListDocumentTypeCallback();');
    }

    if ($.inArray("/BaseInformation/DocumentType/_Create", DocumentTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/DocumentType/_Create', '', '#FormContainer-DocumentType', 'CreateDocumentTypeCallback();');
    }

    EventHandlerDocumentType();
});

function CreateDocumentTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateDocumentTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListDocumentTypeCallback() {
    Pager(1, 5, "DocumentType", DataRefreshDocumentType(1, 5, $("#sort-DocumentType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerDocumentType() {
    $("#FormContainer-DocumentType").on("submit", "#frm-DocumentType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/DocumentType/_Create', $('#frm-DocumentType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormDocumentType();

            if ($('#tbl-DocumentType .page-record').val() == null)
                LoadDataDocumentType(1);
            else
                LoadDataDocumentType($('#tbl-DocumentType .page-record').val());

            if ($.inArray("/BaseInformation/DocumentType/_Create", DocumentTypePermissions) == -1) {
                $('#FormContainer-DocumentType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-DocumentType").on("click", "#frm-DocumentType .btnNew", function () {
        ClearFormDocumentType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-DocumentType").on("keypress", "#tbl-DocumentType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataDocumentType(1);
            return false;
        }
    });

    $("#FormList-DocumentType").on("change keyup", "#tbl-DocumentType tbody tr:first select", function (e) {
        LoadDataDocumentType(1);
    });

}

function DataRefreshDocumentType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-DocumentType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/DocumentType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-DocumentType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان نوع مدرک'>" + json[i].DocumentType.Title + "</td>");

            if ($.inArray("/BaseInformation/DocumentType/_Update", DocumentTypePermissions) > -1 && $.inArray("/BaseInformation/DocumentType/_Delete", DocumentTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateDocumentType(" + json[i].DocumentType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteDocumentType'," + json[i].DocumentType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/DocumentType/_Update", DocumentTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateDocumentType(" + json[i].DocumentType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/DocumentType/_Delete", DocumentTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteDocumentType'," + json[i].DocumentType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-DocumentType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/DocumentType/_Update", DocumentTypePermissions) == -1 && $.inArray("/BaseInformation/DocumentType/_Delete", DocumentTypePermissions) == -1) {
            $('#tbl-DocumentType th:last').remove();
            $('#tbl-DocumentType tbody tr:first td:last').remove();
            $('#tbl-DocumentType tfoot td').attr('colspan', $('#tbl-DocumentType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataDocumentType(pageRecord) {
    if ($.inArray("/BaseInformation/DocumentType/_List", DocumentTypePermissions) > -1) {
        var totalRecords = DataRefreshDocumentType(pageRecord, $('#tbl-DocumentType .page-size').val(), $('#sort-DocumentType').val());

        Pager(pageRecord, $('#tbl-DocumentType .page-size').val(), "DocumentType", totalRecords);
    }
}

function ClearFormDocumentType() {
    
    $('#frm-DocumentType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/DocumentType/_Create", DocumentTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-DocumentType').validate();
    $('#frm-DocumentType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateDocumentType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/DocumentType/_Update', { Id: id }, '#FormContainer-DocumentType', 'UpdateDocumentTypeCallback();');
}



function DeleteDocumentType(id) {
    Ajax('Post', '/BaseInformation/DocumentType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-DocumentType tbody tr').length != 2) {
            pageRecord = $('#tbl-DocumentType .page-record').val();
        }
        else {
            if ($('#tbl-DocumentType .page-record').val() != 1)
                pageRecord = $('#tbl-DocumentType .page-record').val() - 1;
        }

        LoadDataDocumentType(pageRecord);
    }, 'json');
}