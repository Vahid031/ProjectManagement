var ProjectSectionSchedulePermissions;


$(function () {
    ProjectSectionSchedulePermissions = $('#permission-ProjectSectionSchedule').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_List", ProjectSectionSchedulePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSchedule/_List', '', '#FormList-ProjectSectionSchedule', 'ListProjectSectionScheduleCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Create", ProjectSectionSchedulePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSchedule/_Create', '', '#FormContainer-ProjectSectionSchedule', 'CreateProjectSectionScheduleCallback();');
    }

    EventHandlerProjectSectionSchedule();
});

function CreateProjectSectionScheduleCallback() {
    CheckValueProjectSectionSchedule();
    HandleValidation();

    DatePic('#ProjectSectionSchedule_DiffusionDate');
}

function UpdateProjectSectionScheduleCallback() {
    CreateProjectSectionScheduleCallback();
}

function ListProjectSectionScheduleCallback() {
    Pager(1, 5, "ProjectSectionSchedule", DataRefreshProjectSectionSchedule(1, 5, $("#sort-ProjectSectionSchedule").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionSchedule() {
    $("#FormContainer-ProjectSectionSchedule").on("submit", "#frm-ProjectSectionSchedule", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionSchedule/_Create', 'Files=' + $('#images-ProjectSectionSchedule').val() + "&" + 'ProjectSectionSchedule.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionSchedule').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionSchedule();

            if ($('#tbl-ProjectSectionSchedule .page-record').val() == null)
                LoadDataProjectSectionSchedule(1);
            else
                LoadDataProjectSectionSchedule($('#tbl-ProjectSectionSchedule .page-record').val());

            LoadDataProjectSectionOperation($('#tbl-ProjectSectionOperation .page-record').val());
            SelectProjectSectionOperation();

            if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Create", ProjectSectionSchedulePermissions) == -1) {
                $('#FormContainer-ProjectSectionSchedule').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionSchedule").on("click", "#frm-ProjectSectionSchedule .btnNew", function () {
        ClearFormProjectSectionSchedule();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionSchedule").on("keypress", "#tbl-ProjectSectionSchedule tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionSchedule(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionSchedule").on("change keyup", "#tbl-ProjectSectionSchedule tbody tr:first select", function (e) {
        LoadDataProjectSectionSchedule(1);
    });


    $("#FormContainer-ProjectSectionSchedule").on("click", "#btnShowProjectSectionScheduleFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionSchedule/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionSchedule').val('');
    $('.ProjectSectionScheduleFiles').each(function (i, row) {
        $('#images-ProjectSectionSchedule').val($('#images-ProjectSectionSchedule').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionSchedule-fileupload').fileupload();

    $('#ProjectSectionSchedule-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionScheduleFiles($('#ProjectSectionScheduleId').val());
}

function DataRefreshProjectSectionSchedule(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionSchedule.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionSchedule').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionSchedule/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionSchedule tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionSchedule.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Update", ProjectSectionSchedulePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionSchedule/_Delete", ProjectSectionSchedulePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionSchedule(" + json[i].ProjectSectionSchedule.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionSchedule'," + json[i].ProjectSectionSchedule.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Update", ProjectSectionSchedulePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionSchedule(" + json[i].ProjectSectionSchedule.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Delete", ProjectSectionSchedulePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionSchedule'," + json[i].ProjectSectionSchedule.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionSchedule tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Update", ProjectSectionSchedulePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionSchedule/_Delete", ProjectSectionSchedulePermissions) == -1) {
            $('#tbl-ProjectSectionSchedule th:last').remove();
            $('#tbl-ProjectSectionSchedule tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionSchedule tfoot td').attr('colspan', $('#tbl-ProjectSectionSchedule tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionSchedule(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_List", ProjectSectionSchedulePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionSchedule(pageRecord, $('#tbl-ProjectSectionSchedule .page-size').val(), $('#sort-ProjectSectionSchedule').val());

        Pager(pageRecord, $('#tbl-ProjectSectionSchedule .page-size').val(), "ProjectSectionSchedule", totalRecords);
    }
}

function ClearFormProjectSectionSchedule() {
    
    $('#frm-ProjectSectionSchedule input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionScheduleFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionSchedule/_Create", ProjectSectionSchedulePermissions) > -1) {
        $('#ProjectSectionScheduleId').val("-1");
        $('#btnSaveProjectSectionSchedule').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionSchedule').validate();
    $('#frm-ProjectSectionSchedule').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionSchedule(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionSchedule/_Update', { Id: id }, '#FormContainer-ProjectSectionSchedule', 'UpdateProjectSectionScheduleCallback();');
}



function DeleteProjectSectionSchedule(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionSchedule/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionSchedule tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionSchedule .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionSchedule .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionSchedule .page-record').val() - 1;
        }

        LoadDataProjectSectionSchedule(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionSchedule() {
    if ($('#ProjectSectionScheduleId').val() != '-1')
        $('#btnSaveProjectSectionSchedule').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionScheduleFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionSchedule/GetProjectSectionScheduleFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionSchedule-fileupload').fileupload('option', 'done').call($('#ProjectSectionSchedule-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionScheduleFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionSchedule/ReloadFiles', {}, function (data, textStatus, xhr) { });
}