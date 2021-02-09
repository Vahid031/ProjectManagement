var ProjectSectionTenderPermissions;


$(function () {
    ProjectSectionTenderPermissions = $('#permission-ProjectSectionTender').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionTender/_List/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionTender/_List', '', '#FormList-ProjectSectionTender', 'ListProjectSectionTenderCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionTender/_Create/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionTender/_Create', '', '#FormContainer-ProjectSectionTender', 'CreateProjectSectionTenderCallback();');
    }

    EventHandlerProjectSectionTender();
});

function CreateProjectSectionTenderCallback() {
    CheckValueProjectSectionTender();
    HandleValidation();

    DatePic('#ProjectSectionTender_DiffusionDate');
}

function UpdateProjectSectionTenderCallback() {
    CreateProjectSectionTenderCallback();
}

function ListProjectSectionTenderCallback() {
    Pager(1, 5, "ProjectSectionTender", DataRefreshProjectSectionTender(1, 5, $("#sort-ProjectSectionTender").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionTender() {
    $("#FormContainer-ProjectSectionTender").on("submit", "#frm-ProjectSectionTender", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionTender/_Create', 'Files=' + $('#images-ProjectSectionTender').val() + "&" + 'ProjectSectionTender.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionTender').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionTender();

            if ($('#tbl-ProjectSectionTender .page-record').val() == null)
                LoadDataProjectSectionTender(1);
            else
                LoadDataProjectSectionTender($('#tbl-ProjectSectionTender .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionTender/_Create/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) == -1) {
                $('#FormContainer-ProjectSectionTender').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionTender").on("click", "#frm-ProjectSectionTender .btnNew", function () {
        ClearFormProjectSectionTender();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionTender").on("keypress", "#tbl-ProjectSectionTender tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionTender(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionTender").on("change keyup", "#tbl-ProjectSectionTender tbody tr:first select", function (e) {
        LoadDataProjectSectionTender(1);
    });


    $("#FormContainer-ProjectSectionTender").on("click", "#btnShowProjectSectionTenderFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionTender/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionTender').val('');
    $('.ProjectSectionTenderFiles').each(function (i, row) {
        $('#images-ProjectSectionTender').val($('#images-ProjectSectionTender').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionTender-fileupload').fileupload();

    $('#ProjectSectionTender-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionTenderFiles($('#ProjectSectionTenderId').val());
}

function DataRefreshProjectSectionTender(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionTender.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionTender').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionTender/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionTender tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان روزنامه'>" + json[i].ProjectSectionTender.NewspaperTitle + "</td>");
            tr.append("<td data-th='شماره روزنامه'>" + json[i].ProjectSectionTender.NewspaperNumber + "</td>");
            tr.append("<td data-th='تاریخ انتشار'>" + json[i].ProjectSectionTender.DiffusionDate + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionTender/_Update/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionTender/_Delete/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionTender(" + json[i].ProjectSectionTender.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionTender'," + json[i].ProjectSectionTender.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionTender/_Update/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionTender(" + json[i].ProjectSectionTender.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionTender/_Delete/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionTender'," + json[i].ProjectSectionTender.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionTender tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionTender/_Update/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionTender/_Delete/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) == -1) {
            $('#tbl-ProjectSectionTender th:last').remove();
            $('#tbl-ProjectSectionTender tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionTender tfoot td').attr('colspan', $('#tbl-ProjectSectionTender tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionTender(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionTender/_List/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionTender(pageRecord, $('#tbl-ProjectSectionTender .page-size').val(), $('#sort-ProjectSectionTender').val());

        Pager(pageRecord, $('#tbl-ProjectSectionTender .page-size').val(), "ProjectSectionTender", totalRecords);
    }
}

function ClearFormProjectSectionTender() {
    
    $('#frm-ProjectSectionTender input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionTenderFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionTender/_Create/" + SelectedProjectAssignmentType, ProjectSectionTenderPermissions) > -1) {
        $('#ProjectSectionTenderId').val("-1");
        $('#btnSaveProjectSectionTender').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionTender').validate();
    $('#frm-ProjectSectionTender').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionTender(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionTender/_Update', { Id: id }, '#FormContainer-ProjectSectionTender', 'UpdateProjectSectionTenderCallback();');
}



function DeleteProjectSectionTender(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionTender/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionTender tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionTender .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionTender .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionTender .page-record').val() - 1;
        }

        LoadDataProjectSectionTender(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionTender() {
    if ($('#ProjectSectionTenderId').val() != '-1')
        $('#btnSaveProjectSectionTender').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionTenderFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionTender/GetProjectSectionTenderFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionTender-fileupload').fileupload('option', 'done').call($('#ProjectSectionTender-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionTenderFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionTender/ReloadFiles', {}, function (data, textStatus, xhr) { });
}