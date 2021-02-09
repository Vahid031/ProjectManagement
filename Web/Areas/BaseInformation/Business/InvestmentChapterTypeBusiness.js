
var selectedInvestmentChapterId = -1;

$(function () {

    
});

function IndexInvestmentChapterTypeCallback(Id) {

    if ($.inArray("/BaseInformation/InvestmentChapterType/_Create", InvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/InvestmentChapterType/_Create', '', '#FormContainer-InvestmentChapterType', 'CreateInvestmentChapterTypeCallback(' + Id + ');');
    }

    if ($.inArray("/BaseInformation/InvestmentChapterType/_List", InvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/InvestmentChapterType/_List', '', '#FormList-InvestmentChapterType', 'ListInvestmentChapterTypeCallback();');
    }

    EventHandlerInvestmentChapterType();
}

function CreateInvestmentChapterTypeCallback(Id) {
    $('#InvestmentChapterType_InvestmentChapterId').val(Id);
    
    CheckValue();

    HandleValidation();
}

function UpdateInvestmentChapterTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListInvestmentChapterTypeCallback() {
    Pager(1, 5, "InvestmentChapterType", DataRefreshInvestmentChapterType(1, 5, $("#sort-InvestmentChapterType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerInvestmentChapterType() {
    $("#FormContainer-InvestmentChapterType").on("submit", "#frm-InvestmentChapterType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/InvestmentChapterType/_Create', $('#frm-InvestmentChapterType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormInvestmentChapterType();

            if ($('#tbl-InvestmentChapterType .page-record').val() == null)
                LoadDataInvestmentChapterType(1);
            else
                LoadDataInvestmentChapterType($('#tbl-InvestmentChapterType .page-record').val());

            if ($.inArray("/BaseInformation/InvestmentChapterType/_Create", InvestmentChapterPermissions) == -1) {
                $('#FormContainer-InvestmentChapterType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-InvestmentChapterType").on("click", "#frm-InvestmentChapterType .btnNew", function () {
        ClearFormInvestmentChapterType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-InvestmentChapterType").on("keypress", "#tbl-InvestmentChapterType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataInvestmentChapterType(1);
            return false;
        }
    });

    $("#FormList-InvestmentChapterType").on("change keyup", "#tbl-InvestmentChapterType tbody tr:first select", function (e) {
        LoadDataInvestmentChapterType(1);
    });

}

function DataRefreshInvestmentChapterType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'InvestmentChapterType.InvestmentChapterId=' + $('#InvestmentChapterType_InvestmentChapterId').val() + "&" + $('#frm-tbl-InvestmentChapterType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/InvestmentChapterType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-InvestmentChapterType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.InvestmentChapterType.Title)'>" + json[i].InvestmentChapterType.Title + "</td>");

            if ($.inArray("/BaseInformation/InvestmentChapterType/_Update", InvestmentChapterPermissions) > -1 && $.inArray("/BaseInformation/InvestmentChapterType/_Delete", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateInvestmentChapterType(" + json[i].InvestmentChapterType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteInvestmentChapterType'," + json[i].InvestmentChapterType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/InvestmentChapterType/_Update", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateInvestmentChapterType(" + json[i].InvestmentChapterType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/InvestmentChapterType/_Delete", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteInvestmentChapterType'," + json[i].InvestmentChapterType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-InvestmentChapterType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/InvestmentChapterType/_Update", InvestmentChapterPermissions) == -1 && $.inArray("/BaseInformation/InvestmentChapterType/_Delete", InvestmentChapterPermissions) == -1) {
            $('#tbl-InvestmentChapterType th:last').remove();
            $('#tbl-InvestmentChapterType tbody tr:first td:last').remove();
            $('#tbl-InvestmentChapterType tfoot td').attr('colspan', $('#tbl-InvestmentChapterType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataInvestmentChapterType(pageRecord) {
    if ($.inArray("/BaseInformation/InvestmentChapterType/_List", InvestmentChapterPermissions) > -1) {
        var totalRecords = DataRefreshInvestmentChapterType(pageRecord, $('#tbl-InvestmentChapterType .page-size').val(), $('#sort-InvestmentChapterType').val());

        Pager(pageRecord, $('#tbl-InvestmentChapterType .page-size').val(), "InvestmentChapterType", totalRecords);
    }
}

function ClearFormInvestmentChapterType() {
    
    $('#frm-InvestmentChapterType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/InvestmentChapterType/_Create", InvestmentChapterPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-InvestmentChapterType').validate();
    $('#frm-InvestmentChapterType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateInvestmentChapterType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/InvestmentChapterType/_Update', { Id: id }, '#FormContainer-InvestmentChapterType', 'UpdateInvestmentChapterTypeCallback();');
}



function DeleteInvestmentChapterType(id) {
    Ajax('Post', '/BaseInformation/InvestmentChapterType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-InvestmentChapterType tbody tr').length != 2) {
            pageRecord = $('#tbl-InvestmentChapterType .page-record').val();
        }
        else {
            if ($('#tbl-InvestmentChapterType .page-record').val() != 1)
                pageRecord = $('#tbl-InvestmentChapterType .page-record').val() - 1;
        }

        LoadDataInvestmentChapterType(pageRecord);
    }, 'json');
}