var CommissionMemberPermissions;

$(function () {
    CommissionMemberPermissions = $('#permission-CommissionMember').val().split(',');
    
    if ($.inArray("/BaseInformation/CommissionMember/_List", CommissionMemberPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/CommissionMember/_List', '', '#FormList-CommissionMember', 'ListCommissionMemberCallback();');
    }

    if ($.inArray("/BaseInformation/CommissionMember/_Create", CommissionMemberPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/CommissionMember/_Create', '', '#FormContainer-CommissionMember', 'CreateCommissionMemberCallback();');
    }

    EventHandlerCommissionMember();
});

function CreateCommissionMemberCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateCommissionMemberCallback() {
    CheckValue();

    HandleValidation();
}

function ListCommissionMemberCallback() {
    Pager(1, 5, "CommissionMember", DataRefreshCommissionMember(1, 5, $("#sort-CommissionMember").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerCommissionMember() {
    $("#FormContainer-CommissionMember").on("submit", "#frm-CommissionMember", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/CommissionMember/_Create', $('#frm-CommissionMember').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormCommissionMember();

            if ($('#tbl-CommissionMember .page-record').val() == null)
                LoadDataCommissionMember(1);
            else
                LoadDataCommissionMember($('#tbl-CommissionMember .page-record').val());

            if ($.inArray("/BaseInformation/CommissionMember/_Create", CommissionMemberPermissions) == -1) {
                $('#FormContainer-CommissionMember').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-CommissionMember").on("click", "#frm-CommissionMember .btnNew", function () {
        ClearFormCommissionMember();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-CommissionMember").on("keypress", "#tbl-CommissionMember tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataCommissionMember(1);
            return false;
        }
    });

    $("#FormList-CommissionMember").on("change keyup", "#tbl-CommissionMember tbody tr:first select", function (e) {
        LoadDataCommissionMember(1);
    });

}

function DataRefreshCommissionMember(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-CommissionMember').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/CommissionMember/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-CommissionMember tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام'>" + json[i].CommissionMember.FirstName + "</td>");
            tr.append("<td data-th='نام خانوادگی'>" + json[i].CommissionMember.LastName + "</td>");
            tr.append("<td data-th='موبایل'>" + json[i].CommissionMember.MobileNumber + "</td>");
            tr.append("<td data-th='سمت'>" + json[i].CommissionMember.Post + "</td>");

            if (json[i].CommissionMember.IsActive == true)
                tr.append("<td style='text-align:center' data-th='فعال'><input type='checkbox' checked disabled></td>");
            else
                tr.append("<td style='text-align:center' data-th='فعال'><input type='checkbox' disabled></td>");


            if ($.inArray("/BaseInformation/CommissionMember/_Update", CommissionMemberPermissions) > -1 && $.inArray("/BaseInformation/CommissionMember/_Delete", CommissionMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateCommissionMember(" + json[i].CommissionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteCommissionMember'," + json[i].CommissionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/CommissionMember/_Update", CommissionMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateCommissionMember(" + json[i].CommissionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/CommissionMember/_Delete", CommissionMemberPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteCommissionMember'," + json[i].CommissionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-CommissionMember tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/CommissionMember/_Update", CommissionMemberPermissions) == -1 && $.inArray("/BaseInformation/CommissionMember/_Delete", CommissionMemberPermissions) == -1) {
            $('#tbl-CommissionMember th:last').remove();
            $('#tbl-CommissionMember tbody tr:first td:last').remove();
            $('#tbl-CommissionMember tfoot td').attr('colspan', $('#tbl-CommissionMember tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataCommissionMember(pageRecord) {
    if ($.inArray("/BaseInformation/CommissionMember/_List", CommissionMemberPermissions) > -1) {
        var totalRecords = DataRefreshCommissionMember(pageRecord, $('#tbl-CommissionMember .page-size').val(), $('#sort-CommissionMember').val());

        Pager(pageRecord, $('#tbl-CommissionMember .page-size').val(), "CommissionMember", totalRecords);
    }
}

function ClearFormCommissionMember() {
    
    $('#frm-CommissionMember input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/CommissionMember/_Create", CommissionMemberPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-CommissionMember').validate();
    $('#frm-CommissionMember').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateCommissionMember(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/CommissionMember/_Update', { Id: id }, '#FormContainer-CommissionMember', 'UpdateCommissionMemberCallback();');
}



function DeleteCommissionMember(id) {
    Ajax('Post', '/BaseInformation/CommissionMember/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-CommissionMember tbody tr').length != 2) {
            pageRecord = $('#tbl-CommissionMember .page-record').val();
        }
        else {
            if ($('#tbl-CommissionMember .page-record').val() != 1)
                pageRecord = $('#tbl-CommissionMember .page-record').val() - 1;
        }

        LoadDataCommissionMember(pageRecord);
    }, 'json');
}