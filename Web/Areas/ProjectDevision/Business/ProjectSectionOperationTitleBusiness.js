var ProjectSectionOperationTitlePermissions;


$(function () {
    ProjectSectionOperationTitlePermissions = $('#permission-ProjectSectionOperationTitle').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_List/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionOperationTitle/_List', '', '#FormList-ProjectSectionOperationTitle', 'ListProjectSectionOperationTitleCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Create/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionOperationTitle/_Create', '', '#FormContainer-ProjectSectionOperationTitle', 'CreateProjectSectionOperationTitleCallback();');
    }

    EventHandlerProjectSectionOperationTitle();
});

function CreateProjectSectionOperationTitleCallback() {
    CheckValueProjectSectionOperationTitle();
    HandleValidation();
}

function UpdateProjectSectionOperationTitleCallback() {
    CreateProjectSectionOperationTitleCallback();
}

function ListProjectSectionOperationTitleCallback() {
    Pager(1, 5, "ProjectSectionOperationTitle", DataRefreshProjectSectionOperationTitle(1, 5, $("#sort-ProjectSectionOperationTitle").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionOperationTitle() {
    $("#FormContainer-ProjectSectionOperationTitle").on("submit", "#frm-ProjectSectionOperationTitle", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionOperationTitle/_Create', 'ProjectSectionOperationTitle.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionOperationTitle').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionOperationTitle();

            if ($('#tbl-ProjectSectionOperationTitle .page-record').val() == null)
                LoadDataProjectSectionOperationTitle(1);
            else
                LoadDataProjectSectionOperationTitle($('#tbl-ProjectSectionOperationTitle .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Create/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) == -1) {
                $('#FormContainer-ProjectSectionOperationTitle').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionOperationTitle").on("click", "#frm-ProjectSectionOperationTitle .btnNew", function () {
        ClearFormProjectSectionOperationTitle();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionOperationTitle").on("keypress", "#tbl-ProjectSectionOperationTitle tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionOperationTitle(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionOperationTitle").on("change keyup", "#tbl-ProjectSectionOperationTitle tbody tr:first select", function (e) {
        LoadDataProjectSectionOperationTitle(1);
    });

}

function DataRefreshProjectSectionOperationTitle(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionOperationTitle.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionOperationTitle').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionOperationTitle/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionOperationTitle tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].ProjectSectionOperationTitle.Title + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Update/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionOperationTitle(" + json[i].ProjectSectionOperationTitle.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionOperationTitle'," + json[i].ProjectSectionOperationTitle.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Update/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionOperationTitle(" + json[i].ProjectSectionOperationTitle.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionOperationTitle'," + json[i].ProjectSectionOperationTitle.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionOperationTitle tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Update/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Delete/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) == -1) {
            $('#tbl-ProjectSectionOperationTitle th:last').remove();
            $('#tbl-ProjectSectionOperationTitle tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionOperationTitle tfoot td').attr('colspan', $('#tbl-ProjectSectionOperationTitle tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionOperationTitle(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_List/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionOperationTitle(pageRecord, $('#tbl-ProjectSectionOperationTitle .page-size').val(), $('#sort-ProjectSectionOperationTitle').val());

        Pager(pageRecord, $('#tbl-ProjectSectionOperationTitle .page-size').val(), "ProjectSectionOperationTitle", totalRecords);
    }
}

function ClearFormProjectSectionOperationTitle() {
    
    $('#frm-ProjectSectionOperationTitle input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionOperationTitle/_Create/" + SelectedProjectAssignmentType, ProjectSectionOperationTitlePermissions) > -1) {
        $('#ProjectSectionOperationTitleId').val("-1");
        $('#btnSaveProjectSectionOperationTitle').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionOperationTitle').validate();
    $('#frm-ProjectSectionOperationTitle').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionOperationTitle(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionOperationTitle/_Update', { Id: id }, '#FormContainer-ProjectSectionOperationTitle', 'UpdateProjectSectionOperationTitleCallback();');
}



function DeleteProjectSectionOperationTitle(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionOperationTitle/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionOperationTitle tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionOperationTitle .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionOperationTitle .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionOperationTitle .page-record').val() - 1;
        }

        LoadDataProjectSectionOperationTitle(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionOperationTitle() {
    if ($('#ProjectSectionOperationTitleId').val() != '-1')
        $('#btnSaveProjectSectionOperationTitle').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}