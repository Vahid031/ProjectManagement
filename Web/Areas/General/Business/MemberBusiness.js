var MemberPermissions;

$(function () {
    MemberPermissions = $('#permission-Member').val().split(',');
    
    if ($.inArray("/General/Member/_List", MemberPermissions) > -1) {
        LoadPartialView('GET', '/General/Member/_List', '', '#FormList-Member', 'ListMemberCallback();');
    }

    if ($.inArray("/General/Member/_Create", MemberPermissions) > -1) {
        LoadPartialView('GET', '/General/Member/_Create', '', '#FormContainer-Member', 'CreateMemberCallback();');
    }

    EventHandlerMember();
});

function CreateMemberCallback() {
    $('#MemberGroupId').val($('#MemberGroupId-Member').val());

    CheckValue();

    HandleValidation();

    DatePic('#Member_BirthdayDate');
}

function UpdateMemberCallback() {
    $('#MemberGroupId').val($('#MemberGroupId-Member').val());

    CheckValue();

    HandleValidation();

    DatePic('#Member_BirthdayDate');
}

function ListMemberCallback() {
    Pager(1, 5, "Member", DataRefreshMember(1, 5, $("#sort-Member").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerMember() {
    $("#FormContainer-Member").on("submit", "#frm-Member", function (e) {
        e.preventDefault();

        Ajax('Post', '/General/Member/_Create', $('#frm-Member').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormMember();

            if ($('#tbl-Member .page-record').val() == null)
                LoadDataMember(1);
            else
                LoadDataMember($('#tbl-Member .page-record').val());

            if ($.inArray("/General/Member/_Create", MemberPermissions) == -1) {
                $('#FormContainer-Member').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Member").on("click", "#frm-Member .btnNew", function () {
        ClearFormMember();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Member").on("keypress", "#tbl-Member tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataMember(1);
            return false;
        }
    });

    $("#FormList-Member").on("change keyup", "#tbl-Member tbody tr:first select", function (e) {
        LoadDataMember(1);
    });

}

function DataRefreshMember(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Member').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/General/Member/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Member tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان نقش'>" + json[i].Role.Title + "</td>");
            tr.append("<td data-th='نام'>" + json[i].Member.FirstName + "</td>");
            tr.append("<td data-th='نام خانوادگی'>" + json[i].Member.LastName + "</td>");
            tr.append("<td data-th='نام کاربری'>" + json[i].Member.UserName + "</td>");
            tr.append("<td data-th='کد ملی'>" + json[i].Member.NationalCode + "</td>");

            if (json[i].Member.IsActive == true)
                tr.append("<td  data-th='وضعیت'><input type='checkbox' checked disabled></td>");
            else
                tr.append("<td  data-th='وضعیت'><input type='checkbox' disabled></td>");

            if ($.inArray("/General/Member/_Update", MemberPermissions) > -1 && $.inArray("/General/Member/_Delete", MemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateMember(" + json[i].Member.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteMember'," + json[i].Member.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/General/Member/_Update", MemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateMember(" + json[i].Member.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/General/Member/_Delete", MemberPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteMember'," + json[i].Member.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Member tbody').append(tr);
        }


        if ($.inArray("/General/Member/_Update", MemberPermissions) == -1 && $.inArray("/General/Member/_Delete", MemberPermissions) == -1) {
            $('#tbl-Member th:last').remove();
            $('#tbl-Member tbody tr:first td:last').remove();
            $('#tbl-Member tfoot td').attr('colspan', $('#tbl-Member tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataMember(pageRecord) {
    if ($.inArray("/General/Member/_List", MemberPermissions) > -1) {
        var totalRecords = DataRefreshMember(pageRecord, $('#tbl-Member .page-size').val(), $('#sort-Member').val());

        Pager(pageRecord, $('#tbl-Member .page-size').val(), "Member", totalRecords);
    }
}

function ClearFormMember() {
    
    $('#frm-Member input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    $('#Member_Password').val('');
    $('#Member_PasswordCampare').val('');

    if ($.inArray("/General/Member/_Create", MemberPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Member').validate();
    $('#frm-Member').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateMember(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/General/Member/_Update', { Id: id }, '#FormContainer-Member', 'UpdateMemberCallback();');
}



function DeleteMember(id) {
    Ajax('Post', '/General/Member/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Member tbody tr').length != 2) {
            pageRecord = $('#tbl-Member .page-record').val();
        }
        else {
            if ($('#tbl-Member .page-record').val() != 1)
                pageRecord = $('#tbl-Member .page-record').val() - 1;
        }

        LoadDataMember(pageRecord);
    }, 'json');
}