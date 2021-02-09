var ProjectSectionAdvisorMemberPermissions;

$(function () {
    ProjectSectionAdvisorMemberPermissions = $('#permission-ProjectSectionAdvisorMember').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_List", ProjectSectionAdvisorMemberPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorMember/_List', '', '#FormList-ProjectSectionAdvisorMember', 'ListProjectSectionAdvisorMemberCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Create", ProjectSectionAdvisorMemberPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorMember/_Create', '', '#FormContainer-ProjectSectionAdvisorMember', 'CreateProjectSectionAdvisorMemberCallback();');
    }

    EventHandlerProjectSectionAdvisorMember();
});

function CreateProjectSectionAdvisorMemberCallback() {
    CheckValueProjectSectionAdvisorMember();
    HandleValidation();

    DatePic('#ProjectSectionAdvisorMember_StartDate');
    DatePic('#ProjectSectionAdvisorMember_EndDate');
}

function UpdateProjectSectionAdvisorMemberCallback() {
    CreateProjectSectionAdvisorMemberCallback();
}

function ListProjectSectionAdvisorMemberCallback() {
    Pager(1, 5, "ProjectSectionAdvisorMember", DataRefreshProjectSectionAdvisorMember(1, 5, $("#sort-ProjectSectionAdvisorMember").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionAdvisorMember() {
    $("#FormContainer-ProjectSectionAdvisorMember").on("submit", "#frm-ProjectSectionAdvisorMember", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorMember/_Create', 'ProjectSectionAdvisorMember.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionAdvisorMember').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionAdvisorMember();

            if ($('#tbl-ProjectSectionAdvisorMember .page-record').val() == null)
                LoadDataProjectSectionAdvisorMember(1);
            else
                LoadDataProjectSectionAdvisorMember($('#tbl-ProjectSectionAdvisorMember .page-record').val());

            LoadDataProjectSectionOperation($('#tbl-ProjectSectionOperation .page-record').val());
            SelectProjectSectionOperation();

            if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Create", ProjectSectionAdvisorMemberPermissions) == -1) {
                $('#FormContainer-ProjectSectionAdvisorMember').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionAdvisorMember").on("click", "#frm-ProjectSectionAdvisorMember .btnNew", function () {
        ClearFormProjectSectionAdvisorMember();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionAdvisorMember").on("keypress", "#tbl-ProjectSectionAdvisorMember tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionAdvisorMember(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionAdvisorMember").on("change keyup", "#tbl-ProjectSectionAdvisorMember tbody tr:first select", function (e) {
        LoadDataProjectSectionAdvisorMember(1);
    });
}

function DataRefreshProjectSectionAdvisorMember(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionMember.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionAdvisorMember').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorMember/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionAdvisorMember tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام'>" + json[i].Member.FirstName + "</td>");
            tr.append("<td data-th='نام خانوادگی'>" + json[i].Member.LastName + "</td>");
            tr.append("<td data-th='تاریخ شروع به کار'>" + json[i].ProjectSectionMember.StartDate + "</td>");
            tr.append("<td data-th='تاریخ پایان'>" + json[i].ProjectSectionMember.EndDate + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionMember.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Update", ProjectSectionAdvisorMemberPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Delete", ProjectSectionAdvisorMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionAdvisorMember(" + json[i].ProjectSectionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionAdvisorMember'," + json[i].ProjectSectionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Update", ProjectSectionAdvisorMemberPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionAdvisorMember(" + json[i].ProjectSectionMember.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Delete", ProjectSectionAdvisorMemberPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionAdvisorMember'," + json[i].ProjectSectionMember.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionAdvisorMember tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Update", ProjectSectionAdvisorMemberPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Delete", ProjectSectionAdvisorMemberPermissions) == -1) {
            $('#tbl-ProjectSectionAdvisorMember th:last').remove();
            $('#tbl-ProjectSectionAdvisorMember tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionAdvisorMember tfoot td').attr('colspan', $('#tbl-ProjectSectionAdvisorMember tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionAdvisorMember(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_List", ProjectSectionAdvisorMemberPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionAdvisorMember(pageRecord, $('#tbl-ProjectSectionAdvisorMember .page-size').val(), $('#sort-ProjectSectionAdvisorMember').val());

        Pager(pageRecord, $('#tbl-ProjectSectionAdvisorMember .page-size').val(), "ProjectSectionAdvisorMember", totalRecords);
    }
}

function ClearFormProjectSectionAdvisorMember() {
    
    $('#frm-ProjectSectionAdvisorMember input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorMember/_Create", ProjectSectionAdvisorMemberPermissions) > -1) {
        $('#ProjectSectionAdvisorMemberId').val("-1");
        $('#btnSaveProjectSectionAdvisorMember').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionAdvisorMember').validate();
    $('#frm-ProjectSectionAdvisorMember').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionAdvisorMember(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionAdvisorMember/_Update', { Id: id }, '#FormContainer-ProjectSectionAdvisorMember', 'UpdateProjectSectionAdvisorMemberCallback();');
}



function DeleteProjectSectionAdvisorMember(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorMember/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionAdvisorMember tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionAdvisorMember .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionAdvisorMember .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionAdvisorMember .page-record').val() - 1;
        }

        LoadDataProjectSectionAdvisorMember(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionAdvisorMember() {
    if ($('#ProjectSectionAdvisorMemberId').val() != '-1')
        $('#btnSaveProjectSectionAdvisorMember').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}