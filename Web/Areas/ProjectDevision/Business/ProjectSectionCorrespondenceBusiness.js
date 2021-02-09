var ProjectSectionCorrespondencePermissions;


$(function () {
    ProjectSectionCorrespondencePermissions = $('#permission-ProjectSectionCorrespondence').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_List", ProjectSectionCorrespondencePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionCorrespondence/_List', '', '#FormList-ProjectSectionCorrespondence', 'ListProjectSectionCorrespondenceCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Create", ProjectSectionCorrespondencePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionCorrespondence/_Create', '', '#FormContainer-ProjectSectionCorrespondence', 'CreateProjectSectionCorrespondenceCallback();');
    }

    EventHandlerProjectSectionCorrespondence();

});

function CreateProjectSectionCorrespondenceCallback() {
    CheckValueProjectSectionCorrespondence();
    HandleValidation();

    DatePic('#ProjectSectionCorrespondence_LetterDate');
    DatePic('#ProjectSectionCorrespondence_LetterIssue');
}

function UpdateProjectSectionCorrespondenceCallback() {
    CreateProjectSectionCorrespondenceCallback();
}

function ListProjectSectionCorrespondenceCallback() {

    Pager(1, 5, "ProjectSectionCorrespondence", DataRefreshProjectSectionCorrespondence(1, 5, $("#sort-ProjectSectionCorrespondence").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionCorrespondence() {
    $("#FormContainer-ProjectSectionCorrespondence").on("submit", "#frm-ProjectSectionCorrespondence", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionCorrespondence/_Create', 'Files=' + $('#images-ProjectSectionCorrespondence').val() + "&" + 'ProjectSectionCorrespondence.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionCorrespondence').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionCorrespondence();

            if ($('#tbl-ProjectSectionCorrespondence .page-record').val() == null)
                LoadDataProjectSectionCorrespondence(1);
            else
                LoadDataProjectSectionCorrespondence($('#tbl-ProjectSectionCorrespondence .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Create", ProjectSectionCorrespondencePermissions) == -1) {
                $('#FormContainer-ProjectSectionCorrespondence').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionCorrespondence").on("click", "#frm-ProjectSectionCorrespondence .btnNew", function () {
        ClearFormProjectSectionCorrespondence();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionCorrespondence").on("keypress", "#tbl-ProjectSectionCorrespondence tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionCorrespondence(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionCorrespondence").on("change keyup", "#tbl-ProjectSectionCorrespondence tbody tr:first select", function (e) {
        LoadDataProjectSectionCorrespondence(1);
    });


    $("#FormContainer-ProjectSectionCorrespondence").on("click", "#btnShowProjectSectionCorrespondenceFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionCorrespondence/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionCorrespondence').val('');
    $('.ProjectSectionCorrespondenceFiles').each(function (i, row) {
        $('#images-ProjectSectionCorrespondence').val($('#images-ProjectSectionCorrespondence').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionCorrespondence-fileupload').fileupload();

    $('#ProjectSectionCorrespondence-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionCorrespondenceFiles($('#ProjectSectionCorrespondenceId').val());
}

function DataRefreshProjectSectionCorrespondence(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionCorrespondence.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionCorrespondence').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionCorrespondence/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionCorrespondence tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].LetterType.Title + "</td>");
            tr.append("<td data-th='شماره نامه'>" + json[i].ProjectSectionCorrespondence.LetterNumber + "</td>");
            tr.append("<td data-th='تاریخ نامه'>" + json[i].ProjectSectionCorrespondence.LetterDate + "</td>");
            tr.append("<td data-th='تاریخ صدور نامه'>" + json[i].ProjectSectionCorrespondence.LetterIssue + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Update", ProjectSectionCorrespondencePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Delete", ProjectSectionCorrespondencePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionCorrespondence(" + json[i].ProjectSectionCorrespondence.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionCorrespondence'," + json[i].ProjectSectionCorrespondence.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Update", ProjectSectionCorrespondencePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionCorrespondence(" + json[i].ProjectSectionCorrespondence.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Delete", ProjectSectionCorrespondencePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionCorrespondence'," + json[i].ProjectSectionCorrespondence.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionCorrespondence tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Update", ProjectSectionCorrespondencePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Delete", ProjectSectionCorrespondencePermissions) == -1) {
            $('#tbl-ProjectSectionCorrespondence th:last').remove();
            $('#tbl-ProjectSectionCorrespondence tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionCorrespondence tfoot td').attr('colspan', $('#tbl-ProjectSectionCorrespondence tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionCorrespondence(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_List", ProjectSectionCorrespondencePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionCorrespondence(pageRecord, $('#tbl-ProjectSectionCorrespondence .page-size').val(), $('#sort-ProjectSectionCorrespondence').val());

        Pager(pageRecord, $('#tbl-ProjectSectionCorrespondence .page-size').val(), "ProjectSectionCorrespondence", totalRecords);
    }
}

function ClearFormProjectSectionCorrespondence() {
    
    $('#frm-ProjectSectionCorrespondence input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionCorrespondenceFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionCorrespondence/_Create", ProjectSectionCorrespondencePermissions) > -1) {
        $('#ProjectSectionCorrespondenceId').val("-1");
        $('#btnSaveProjectSectionCorrespondence').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionCorrespondence').validate();
    $('#frm-ProjectSectionCorrespondence').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionCorrespondence(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionCorrespondence/_Update', { Id: id }, '#FormContainer-ProjectSectionCorrespondence', 'UpdateProjectSectionCorrespondenceCallback();');
}



function DeleteProjectSectionCorrespondence(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionCorrespondence/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionCorrespondence tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionCorrespondence .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionCorrespondence .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionCorrespondence .page-record').val() - 1;
        }

        LoadDataProjectSectionCorrespondence(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionCorrespondence() {
    if ($('#ProjectSectionCorrespondenceId').val() != '-1')
        $('#btnSaveProjectSectionCorrespondence').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionCorrespondenceFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionCorrespondence/GetProjectSectionCorrespondenceFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionCorrespondence-fileupload').fileupload('option', 'done').call($('#ProjectSectionCorrespondence-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionCorrespondenceFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionCorrespondence/ReloadFiles', {}, function (data, textStatus, xhr) { });
}