var ProjectSectionCeremonialPermissions;


$(function () {
    ProjectSectionCeremonialPermissions = $('#permission-ProjectSectionCeremonial').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_List/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionCeremonial/_List', '', '#FormList-ProjectSectionCeremonial', 'ListProjectSectionCeremonialCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Create/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionCeremonial/_Create', '', '#FormContainer-ProjectSectionCeremonial', 'CreateProjectSectionCeremonialCallback();');
    }

    EventHandlerProjectSectionCeremonial();
});

function CreateProjectSectionCeremonialCallback() {
    CheckValueProjectSectionCeremonial();
    HandleValidation();

    DatePic('#ProjectSectionCeremonial_JustificationDate');
}

function UpdateProjectSectionCeremonialCallback() {
    CreateProjectSectionCeremonialCallback();
}

function ListProjectSectionCeremonialCallback() {
    Pager(1, 5, "ProjectSectionCeremonial", DataRefreshProjectSectionCeremonial(1, 5, $("#sort-ProjectSectionCeremonial").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionCeremonial() {
    $("#FormContainer-ProjectSectionCeremonial").on("submit", "#frm-ProjectSectionCeremonial", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionCeremonial/_Create', 'Files=' + $('#images-ProjectSectionCeremonial').val() + "&" + 'ProjectSectionCeremonial.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionCeremonial').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionCeremonial();

            if ($('#tbl-ProjectSectionCeremonial .page-record').val() == null)
                LoadDataProjectSectionCeremonial(1);
            else
                LoadDataProjectSectionCeremonial($('#tbl-ProjectSectionCeremonial .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Create/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) == -1) {
                $('#FormContainer-ProjectSectionCeremonial').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionCeremonial").on("click", "#frm-ProjectSectionCeremonial .btnNew", function () {
        ClearFormProjectSectionCeremonial();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionCeremonial").on("keypress", "#tbl-ProjectSectionCeremonial tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionCeremonial(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionCeremonial").on("change keyup", "#tbl-ProjectSectionCeremonial tbody tr:first select", function (e) {
        LoadDataProjectSectionCeremonial(1);
    });


    $("#FormContainer-ProjectSectionCeremonial").on("click", "#btnShowProjectSectionCeremonialFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionCeremonial/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionCeremonial').val('');
    $('.ProjectSectionCeremonialFiles').each(function (i, row) {
        $('#images-ProjectSectionCeremonial').val($('#images-ProjectSectionCeremonial').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionCeremonial-fileupload').fileupload();

    $('#ProjectSectionCeremonial-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionCeremonialFiles($('#ProjectSectionCeremonialId').val());
}

function DataRefreshProjectSectionCeremonial(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionCeremonial.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionCeremonial').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionCeremonial/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionCeremonial tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionCeremonial.JustificationNumber)'>" + json[i].ProjectSectionCeremonial.JustificationNumber + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionCeremonial.JustificationDate)'>" + json[i].ProjectSectionCeremonial.JustificationDate + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Update/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionCeremonial/_Delete/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionCeremonial(" + json[i].ProjectSectionCeremonial.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionCeremonial'," + json[i].ProjectSectionCeremonial.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Update/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionCeremonial(" + json[i].ProjectSectionCeremonial.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Delete/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionCeremonial'," + json[i].ProjectSectionCeremonial.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionCeremonial tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Update/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionCeremonial/_Delete/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) == -1) {
            $('#tbl-ProjectSectionCeremonial th:last').remove();
            $('#tbl-ProjectSectionCeremonial tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionCeremonial tfoot td').attr('colspan', $('#tbl-ProjectSectionCeremonial tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionCeremonial(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_List/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionCeremonial(pageRecord, $('#tbl-ProjectSectionCeremonial .page-size').val(), $('#sort-ProjectSectionCeremonial').val());

        Pager(pageRecord, $('#tbl-ProjectSectionCeremonial .page-size').val(), "ProjectSectionCeremonial", totalRecords);
    }
}

function ClearFormProjectSectionCeremonial() {
    
    $('#frm-ProjectSectionCeremonial input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionCeremonialFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionCeremonial/_Create/" + SelectedProjectAssignmentType, ProjectSectionCeremonialPermissions) > -1) {
        $('#ProjectSectionCeremonialId').val("-1");
        $('#btnSaveProjectSectionCeremonial').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionCeremonial').validate();
    $('#frm-ProjectSectionCeremonial').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionCeremonial(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionCeremonial/_Update', { Id: id }, '#FormContainer-ProjectSectionCeremonial', 'UpdateProjectSectionCeremonialCallback();');
}



function DeleteProjectSectionCeremonial(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionCeremonial/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionCeremonial tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionCeremonial .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionCeremonial .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionCeremonial .page-record').val() - 1;
        }

        LoadDataProjectSectionCeremonial(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionCeremonial() {
    if ($('#ProjectSectionCeremonialId').val() != '-1')
        $('#btnSaveProjectSectionCeremonial').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionCeremonialFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionCeremonial/GetProjectSectionCeremonialFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionCeremonial-fileupload').fileupload('option', 'done').call($('#ProjectSectionCeremonial-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionCeremonialFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionCeremonial/ReloadFiles', {}, function (data, textStatus, xhr) { });
}