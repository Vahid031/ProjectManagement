var ProjectSectionOpenDatePermissions;


$(function () {
    ProjectSectionOpenDatePermissions = $('#permission-ProjectSectionOpenDate').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_List/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionOpenDate/_List', '', '#FormList-ProjectSectionOpenDate', 'ListProjectSectionOpenDateCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Create/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionOpenDate/_Create', '', '#FormContainer-ProjectSectionOpenDate', 'CreateProjectSectionOpenDateCallback();');
    }

    EventHandlerProjectSectionOpenDate();
});

function CreateProjectSectionOpenDateCallback() {
    CheckValueProjectSectionOpenDate();
    HandleValidation();

    DatePic('#ProjectSectionOpenDate_OpenDate');
}

function UpdateProjectSectionOpenDateCallback() {
    CreateProjectSectionOpenDateCallback();
}

function ListProjectSectionOpenDateCallback() {
    Pager(1, 5, "ProjectSectionOpenDate", DataRefreshProjectSectionOpenDate(1, 5, $("#sort-ProjectSectionOpenDate").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionOpenDate() {
    $("#FormContainer-ProjectSectionOpenDate").on("submit", "#frm-ProjectSectionOpenDate", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionOpenDate/_Create', 'ProjectSectionOpenDate.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionOpenDate').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionOpenDate();

            if ($('#tbl-ProjectSectionOpenDate .page-record').val() == null)
                LoadDataProjectSectionOpenDate(1);
            else
                LoadDataProjectSectionOpenDate($('#tbl-ProjectSectionOpenDate .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Create/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) == -1) {
                $('#FormContainer-ProjectSectionOpenDate').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionOpenDate").on("click", "#frm-ProjectSectionOpenDate .btnNew", function () {
        ClearFormProjectSectionOpenDate();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionOpenDate").on("keypress", "#tbl-ProjectSectionOpenDate tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionOpenDate(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionOpenDate").on("change keyup", "#tbl-ProjectSectionOpenDate tbody tr:first select", function (e) {
        LoadDataProjectSectionOpenDate(1);
    });

}

function DataRefreshProjectSectionOpenDate(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionOpenDate.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionOpenDate').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionOpenDate/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionOpenDate tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ بازگشایی'>" + json[i].ProjectSectionOpenDate.OpenDate + "</td>");
            tr.append("<td data-th='متن پیامک'>" + json[i].ProjectSectionOpenDate.MessageContent + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Update/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionOpenDate/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionOpenDate(" + json[i].ProjectSectionOpenDate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionOpenDate'," + json[i].ProjectSectionOpenDate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Update/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionOpenDate(" + json[i].ProjectSectionOpenDate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionOpenDate'," + json[i].ProjectSectionOpenDate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionOpenDate tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Update/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionOpenDate/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) == -1) {
            $('#tbl-ProjectSectionOpenDate th:last').remove();
            $('#tbl-ProjectSectionOpenDate tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionOpenDate tfoot td').attr('colspan', $('#tbl-ProjectSectionOpenDate tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionOpenDate(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_List/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionOpenDate(pageRecord, $('#tbl-ProjectSectionOpenDate .page-size').val(), $('#sort-ProjectSectionOpenDate').val());

        Pager(pageRecord, $('#tbl-ProjectSectionOpenDate .page-size').val(), "ProjectSectionOpenDate", totalRecords);
    }
}

function ClearFormProjectSectionOpenDate() {
    
    $('#frm-ProjectSectionOpenDate input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionOpenDate/_Create/" + SelectedProjectAssignmentType, ProjectSectionOpenDatePermissions) > -1) {
        $('#ProjectSectionOpenDateId').val("-1");
        $('#btnSaveProjectSectionOpenDate').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionOpenDate').validate();
    $('#frm-ProjectSectionOpenDate').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionOpenDate(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionOpenDate/_Update', { Id: id }, '#FormContainer-ProjectSectionOpenDate', 'UpdateProjectSectionOpenDateCallback();');
}



function DeleteProjectSectionOpenDate(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionOpenDate/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionOpenDate tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionOpenDate .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionOpenDate .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionOpenDate .page-record').val() - 1;
        }

        LoadDataProjectSectionOpenDate(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionOpenDate() {
    if ($('#ProjectSectionOpenDateId').val() != '-1')
        $('#btnSaveProjectSectionOpenDate').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}