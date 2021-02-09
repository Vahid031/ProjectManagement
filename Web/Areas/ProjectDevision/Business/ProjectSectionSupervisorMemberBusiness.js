var ProjectSectionSupervisorMemberPermissions;

$(function () {
    ProjectSectionSupervisorMemberPermissions = $('#permission-ProjectSectionSupervisorMember').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_List", ProjectSectionSupervisorMemberPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorMember/_List', '', '#FormList-ProjectSectionSupervisorMember', 'ListProjectSectionSupervisorMemberCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Create", ProjectSectionSupervisorMemberPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorMember/_Create', '', '#FormContainer-ProjectSectionSupervisorMember', 'CreateProjectSectionSupervisorMemberCallback();');
    }

    EventHandlerProjectSectionSupervisorMember();
});

function CreateProjectSectionSupervisorMemberCallback() {
    CheckValueProjectSectionSupervisorMember();
    HandleValidation();

    DatePic('#ProjectSectionSupervisorMember_StartDate');
    DatePic('#ProjectSectionSupervisorMember_EndDate');
}

function UpdateProjectSectionSupervisorMemberCallback() {
    CreateProjectSectionSupervisorMemberCallback();
}

function ListProjectSectionSupervisorMemberCallback() {
    Pager(1, 5, "ProjectSectionSupervisorMember", DataRefreshProjectSectionSupervisorMember(1, 5, $("#sort-ProjectSectionSupervisorMember").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionSupervisorMember() {
    $("#FormContainer-ProjectSectionSupervisorMember").on("submit", "#frm-ProjectSectionSupervisorMember", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorMember/_Create', 'ProjectSectionSupervisorMember.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionSupervisorMember').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionSupervisorMember();

            if ($('#tbl-ProjectSectionSupervisorMember .page-record').val() == null)
                LoadDataProjectSectionSupervisorMember(1);
            else
                LoadDataProjectSectionSupervisorMember($('#tbl-ProjectSectionSupervisorMember .page-record').val());

            LoadDataProjectSectionOperation($('#tbl-ProjectSectionOperation .page-record').val());
            SelectProjectSectionOperation();

            if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Create", ProjectSectionSupervisorMemberPermissions) == -1) {
                $('#FormContainer-ProjectSectionSupervisorMember').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionSupervisorMember").on("click", "#frm-ProjectSectionSupervisorMember .btnNew", function () {
        ClearFormProjectSectionSupervisorMember();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionSupervisorMember").on("keypress", "#tbl-ProjectSectionSupervisorMember tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionSupervisorMember(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionSupervisorMember").on("change keyup", "#tbl-ProjectSectionSupervisorMember tbody tr:first select", function (e) {
        LoadDataProjectSectionSupervisorMember(1);
    });
}

function DataRefreshProjectSectionSupervisorMember(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionMember.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionSupervisorMember').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorMember/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionSupervisorMember tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام'>" + json[i].Member.FirstName + "</td>");
            tr.append("<td data-th='نام خانوادگی'>" + json[i].Member.LastName + "</td>");
            tr.append("<td data-th='نوع نظارت ناظر'>" + json[i].MonitoringType.Title + "</td>");
            tr.append("<td data-th='تاریخ شروع به کار'>" + json[i].ProjectSectionMember.StartDate + "</td>");
            tr.append("<td data-th='تاریخ پایان'>" + json[i].ProjectSectionMember.EndDate + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionMember.Description + "</td>");
            if (json[i].ProjectSectionMember.IsMainSupervisor == true)
                tr.append("<td data-th='سرناظر' style='text-align:center'><input type='checkbox' checked readonly disabled /></td>");
            else
                tr.append("<td data-th='سرناظر' style='text-align:center'><input type='checkbox' readonly disabled /></td>");

            if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Update", ProjectSectionSupervisorMemberPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Delete", ProjectSectionSupervisorMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionSupervisorMember(" + json[i].ProjectSectionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionSupervisorMember'," + json[i].ProjectSectionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Update", ProjectSectionSupervisorMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionSupervisorMember(" + json[i].ProjectSectionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Delete", ProjectSectionSupervisorMemberPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionSupervisorMember'," + json[i].ProjectSectionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionSupervisorMember tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Update", ProjectSectionSupervisorMemberPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Delete", ProjectSectionSupervisorMemberPermissions) == -1) {
            $('#tbl-ProjectSectionSupervisorMember th:last').remove();
            $('#tbl-ProjectSectionSupervisorMember tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionSupervisorMember tfoot td').attr('colspan', $('#tbl-ProjectSectionSupervisorMember tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionSupervisorMember(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_List", ProjectSectionSupervisorMemberPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionSupervisorMember(pageRecord, $('#tbl-ProjectSectionSupervisorMember .page-size').val(), $('#sort-ProjectSectionSupervisorMember').val());

        Pager(pageRecord, $('#tbl-ProjectSectionSupervisorMember .page-size').val(), "ProjectSectionSupervisorMember", totalRecords);
    }
}

function ClearFormProjectSectionSupervisorMember() {
    
    $('#frm-ProjectSectionSupervisorMember input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorMember/_Create", ProjectSectionSupervisorMemberPermissions) > -1) {
        $('#ProjectSectionSupervisorMemberId').val("-1");
        $('#btnSaveProjectSectionSupervisorMember').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionSupervisorMember').validate();
    $('#frm-ProjectSectionSupervisorMember').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionSupervisorMember(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionSupervisorMember/_Update', { Id: id }, '#FormContainer-ProjectSectionSupervisorMember', 'UpdateProjectSectionSupervisorMemberCallback();');
}



function DeleteProjectSectionSupervisorMember(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorMember/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionSupervisorMember tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionSupervisorMember .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionSupervisorMember .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionSupervisorMember .page-record').val() - 1;
        }

        LoadDataProjectSectionSupervisorMember(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionSupervisorMember() {
    if ($('#ProjectSectionSupervisorMemberId').val() != '-1')
        $('#btnSaveProjectSectionSupervisorMember').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}