var InvestmentChapterPermissions;

$(function () {
    InvestmentChapterPermissions = $('#permission-InvestmentChapter').val().split(',');

    if ($.inArray("/BaseInformation/InvestmentChapter/_List", InvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/InvestmentChapter/_List', '', '#FormList-InvestmentChapter', 'ListInvestmentChapterCallback();');
    }

    if ($.inArray("/BaseInformation/InvestmentChapter/_Create", InvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/InvestmentChapter/_Create', '', '#FormContainer-InvestmentChapter', 'CreateInvestmentChapterCallback();');
    }

    EventHandlerInvestmentChapter();
});

function CreateInvestmentChapterCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateInvestmentChapterCallback() {
    CheckValue();

    HandleValidation();
}

function ListInvestmentChapterCallback() {
    Pager(1, 5, "InvestmentChapter", DataRefreshInvestmentChapter(1, 5, $("#sort-InvestmentChapter").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerInvestmentChapter() {
    $("#FormContainer-InvestmentChapter").on("submit", "#frm-InvestmentChapter", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/InvestmentChapter/_Create', $('#frm-InvestmentChapter').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormInvestmentChapter();

            if ($('#tbl-InvestmentChapter .page-record').val() == null)
                LoadDataInvestmentChapter(1);
            else
                LoadDataInvestmentChapter($('#tbl-InvestmentChapter .page-record').val());

            if ($.inArray("/BaseInformation/InvestmentChapter/_Create", InvestmentChapterPermissions) == -1) {
                $('#FormContainer-InvestmentChapter').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-InvestmentChapter").on("click", "#frm-InvestmentChapter .btnNew", function () {
        ClearFormInvestmentChapter();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-InvestmentChapter").on("keypress", "#tbl-InvestmentChapter tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataInvestmentChapter(1);
            return false;
        }
    });

    $("#FormList-InvestmentChapter").on("change keyup", "#tbl-InvestmentChapter tbody tr:first select", function (e) {
        LoadDataInvestmentChapter(1);
    });

}

function DataRefreshInvestmentChapter(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-InvestmentChapter').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/InvestmentChapter/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-InvestmentChapter tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].InvestmentChapter.Title + "</td>");

            if ($.inArray("/BaseInformation/InvestmentChapterType/_List", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='انتخاب'><a onmousedown = 'PopupFormHtml(\"نوع فصل سرمایه گذاری\", \"/BaseInformation/InvestmentChapterType/_Index\", \"IndexInvestmentChapterTypeCallback(" + json[i].InvestmentChapter.Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='افزودن'></a></td>");
            }

            if ($.inArray("/BaseInformation/InvestmentChapter/_Update", InvestmentChapterPermissions) > -1 && $.inArray("/BaseInformation/InvestmentChapter/_Delete", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateInvestmentChapter(" + json[i].InvestmentChapter.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteInvestmentChapter'," + json[i].InvestmentChapter.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/InvestmentChapter/_Update", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateInvestmentChapter(" + json[i].InvestmentChapter.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/InvestmentChapter/_Delete", InvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteInvestmentChapter'," + json[i].InvestmentChapter.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-InvestmentChapter tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/InvestmentChapter/_Update", InvestmentChapterPermissions) == -1 && $.inArray("/BaseInformation/InvestmentChapter/_Delete", InvestmentChapterPermissions) == -1) {
            $('#tbl-InvestmentChapter th:last').remove();
            $('#tbl-InvestmentChapter tbody tr:first td:last').remove();
            $('#tbl-InvestmentChapter tfoot td').attr('colspan', $('#tbl-InvestmentChapter tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataInvestmentChapter(pageRecord) {
    if ($.inArray("/BaseInformation/InvestmentChapter/_List", InvestmentChapterPermissions) > -1) {
        var totalRecords = DataRefreshInvestmentChapter(pageRecord, $('#tbl-InvestmentChapter .page-size').val(), $('#sort-InvestmentChapter').val());

        Pager(pageRecord, $('#tbl-InvestmentChapter .page-size').val(), "InvestmentChapter", totalRecords);
    }
}

function ClearFormInvestmentChapter() {
    
    $('#frm-InvestmentChapter input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/InvestmentChapter/_Create", InvestmentChapterPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-InvestmentChapter').validate();
    $('#frm-InvestmentChapter').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateInvestmentChapter(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/InvestmentChapter/_Update', { Id: id }, '#FormContainer-InvestmentChapter', 'UpdateInvestmentChapterCallback();');
}



function DeleteInvestmentChapter(id) {
    Ajax('Post', '/BaseInformation/InvestmentChapter/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-InvestmentChapter tbody tr').length != 2) {
            pageRecord = $('#tbl-InvestmentChapter .page-record').val();
        }
        else {
            if ($('#tbl-InvestmentChapter .page-record').val() != 1)
                pageRecord = $('#tbl-InvestmentChapter .page-record').val() - 1;
        }

        LoadDataInvestmentChapter(pageRecord);
    }, 'json');
}