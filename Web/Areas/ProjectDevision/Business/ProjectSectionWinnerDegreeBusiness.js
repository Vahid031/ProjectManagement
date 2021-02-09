var ProjectSectionWinnerDegreePermissions;


$(function () {
    ProjectSectionWinnerDegreePermissions = $('#permission-ProjectSectionWinnerDegree').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_List/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinnerDegree/_List', '', '#FormList-ProjectSectionWinnerDegree', 'ListProjectSectionWinnerDegreeCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinnerDegree/_Create', '', '#FormContainer-ProjectSectionWinnerDegree', 'CreateProjectSectionWinnerDegreeCallback();');
    }

    EventHandlerProjectSectionWinnerDegree();
});

function CreateProjectSectionWinnerDegreeCallback() {
    CheckValueProjectSectionWinnerDegree();
    HandleValidation();

    DatePic('#ProjectSectionWinnerDegree_ReciveDate');
}

function UpdateProjectSectionWinnerDegreeCallback() {
    CreateProjectSectionWinnerDegreeCallback();
}

function ListProjectSectionWinnerDegreeCallback() {
    Pager(1, 5, "ProjectSectionWinnerDegree", DataRefreshProjectSectionWinnerDegree(1, 5, $("#sort-ProjectSectionWinnerDegree").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionWinnerDegree() {
    $("#FormContainer-ProjectSectionWinnerDegree").on("submit", "#frm-ProjectSectionWinnerDegree", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionWinnerDegree/_Create', 'Files=' + $('#images-ProjectSectionWinnerDegree').val() + "&" + 'ProjectSectionWinnerDegree.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionWinnerDegree').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionWinnerDegree();

            if ($('#tbl-ProjectSectionWinnerDegree .page-record').val() == null)
                LoadDataProjectSectionWinnerDegree(1);
            else
                LoadDataProjectSectionWinnerDegree($('#tbl-ProjectSectionWinnerDegree .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) == -1) {
                $('#FormContainer-ProjectSectionWinnerDegree').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionWinnerDegree").on("click", "#frm-ProjectSectionWinnerDegree .btnNew", function () {
        ClearFormProjectSectionWinnerDegree();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionWinnerDegree").on("keypress", "#tbl-ProjectSectionWinnerDegree tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionWinnerDegree(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionWinnerDegree").on("change keyup", "#tbl-ProjectSectionWinnerDegree tbody tr:first select", function (e) {
        LoadDataProjectSectionWinnerDegree(1);
    });


    $("#FormContainer-ProjectSectionWinnerDegree").on("click", "#btnShowProjectSectionWinnerDegreeFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionWinnerDegree/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionWinnerDegree').val('');
    $('.ProjectSectionWinnerDegreeFiles').each(function (i, row) {
        $('#images-ProjectSectionWinnerDegree').val($('#images-ProjectSectionWinnerDegree').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionWinnerDegree-fileupload').fileupload();

    $('#ProjectSectionWinnerDegree-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionWinnerDegreeFiles($('#ProjectSectionWinnerDegreeId').val());
}

function DataRefreshProjectSectionWinnerDegree(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionWinnerDegree.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionWinnerDegree').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionWinnerDegree/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionWinnerDegree tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ دریافت'>" + json[i].ProjectSectionWinnerDegree.ReciveDate + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionWinnerDegree(" + json[i].ProjectSectionWinnerDegree.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionWinnerDegree'," + json[i].ProjectSectionWinnerDegree.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionWinnerDegree(" + json[i].ProjectSectionWinnerDegree.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionWinnerDegree'," + json[i].ProjectSectionWinnerDegree.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionWinnerDegree tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) == -1) {
            $('#tbl-ProjectSectionWinnerDegree th:last').remove();
            $('#tbl-ProjectSectionWinnerDegree tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionWinnerDegree tfoot td').attr('colspan', $('#tbl-ProjectSectionWinnerDegree tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionWinnerDegree(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_List/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionWinnerDegree(pageRecord, $('#tbl-ProjectSectionWinnerDegree .page-size').val(), $('#sort-ProjectSectionWinnerDegree').val());

        Pager(pageRecord, $('#tbl-ProjectSectionWinnerDegree .page-size').val(), "ProjectSectionWinnerDegree", totalRecords);
    }
}

function ClearFormProjectSectionWinnerDegree() {
    
    $('#frm-ProjectSectionWinnerDegree input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionWinnerDegreeFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionWinnerDegree/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerDegreePermissions) > -1) {
        $('#ProjectSectionWinnerDegreeId').val("-1");
        $('#btnSaveProjectSectionWinnerDegree').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionWinnerDegree').validate();
    $('#frm-ProjectSectionWinnerDegree').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionWinnerDegree(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionWinnerDegree/_Update', { Id: id }, '#FormContainer-ProjectSectionWinnerDegree', 'UpdateProjectSectionWinnerDegreeCallback();');
}



function DeleteProjectSectionWinnerDegree(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionWinnerDegree/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionWinnerDegree tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionWinnerDegree .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionWinnerDegree .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionWinnerDegree .page-record').val() - 1;
        }

        LoadDataProjectSectionWinnerDegree(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionWinnerDegree() {
    if ($('#ProjectSectionWinnerDegreeId').val() != '-1')
        $('#btnSaveProjectSectionWinnerDegree').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionWinnerDegreeFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionWinnerDegree/GetProjectSectionWinnerDegreeFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
        var json = JSON.parse(data.Values);
        var files = [];

        for (var i = 0; i < json.length; i++) {
            files.push({
                "url": json[i].url,
                "thumbnail_url": json[i].thumbnail_url,
                "name": json[i].name,
                "type": json[i].type,
                "size": json[i].size,
                "delete_url": json[i].delete_url + "/" + json[i].imageId,
                "delete_type": json[i].delete_type,
                "imageId": json[i].imageId
            });
        }

        $('#ProjectSectionWinnerDegree-fileupload').fileupload('option', 'done').call($('#ProjectSectionWinnerDegree-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionWinnerDegreeFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionWinnerDegree/ReloadFiles', {}, function (data, textStatus, xhr) { });
}