var ProjectSectionRecoupmentPermissions;


$(function () {
    ProjectSectionRecoupmentPermissions = $('#permission-ProjectSectionRecoupment').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_List", ProjectSectionRecoupmentPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionRecoupment/_List', '', '#FormList-ProjectSectionRecoupment', 'ListProjectSectionRecoupmentCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Create", ProjectSectionRecoupmentPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionRecoupment/_Create', '', '#FormContainer-ProjectSectionRecoupment', 'CreateProjectSectionRecoupmentCallback();');
    }

    EventHandlerProjectSectionRecoupment();

});

function CreateProjectSectionRecoupmentCallback() {
    CheckValueProjectSectionRecoupment();
    HandleValidation();

    DatePic('#ProjectSectionRecoupment_PaidDate');
}

function UpdateProjectSectionRecoupmentCallback() {
    CreateProjectSectionRecoupmentCallback();
}

function ListProjectSectionRecoupmentCallback() {

    Pager(1, 5, "ProjectSectionRecoupment", DataRefreshProjectSectionRecoupment(1, 5, $("#sort-ProjectSectionRecoupment").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionRecoupment() {
    $("#FormContainer-ProjectSectionRecoupment").on("submit", "#frm-ProjectSectionRecoupment", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionRecoupment/_Create', 'Files=' + $('#images-ProjectSectionRecoupment').val() + "&" + 'ProjectSectionRecoupment.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionRecoupment').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionRecoupment();

            if ($('#tbl-ProjectSectionRecoupment .page-record').val() == null)
                LoadDataProjectSectionRecoupment(1);
            else
                LoadDataProjectSectionRecoupment($('#tbl-ProjectSectionRecoupment .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Create", ProjectSectionRecoupmentPermissions) == -1) {
                $('#FormContainer-ProjectSectionRecoupment').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionRecoupment").on("click", "#frm-ProjectSectionRecoupment .btnNew", function () {
        ClearFormProjectSectionRecoupment();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionRecoupment").on("keypress", "#tbl-ProjectSectionRecoupment tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionRecoupment(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionRecoupment").on("change keyup", "#tbl-ProjectSectionRecoupment tbody tr:first select", function (e) {
        LoadDataProjectSectionRecoupment(1);
    });


    $("#FormContainer-ProjectSectionRecoupment").on("click", "#btnShowProjectSectionRecoupmentFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionRecoupment/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionRecoupment').val('');
    $('.ProjectSectionRecoupmentFiles').each(function (i, row) {
        $('#images-ProjectSectionRecoupment').val($('#images-ProjectSectionRecoupment').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionRecoupment-fileupload').fileupload();

    $('#ProjectSectionRecoupment-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionRecoupmentFiles($('#ProjectSectionRecoupmentId').val());
}

function DataRefreshProjectSectionRecoupment(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionRecoupment.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionRecoupment').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionRecoupment/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionRecoupment tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نوع مفاصا حساب'>" + json[i].RecoupmentType.Title + "</td>");
            tr.append("<td data-th='شماره'>" + json[i].ProjectSectionRecoupment.Number + "</td>");
            tr.append("<td data-th='تاریخ'>" + json[i].ProjectSectionRecoupment.Date_ + "</td>");
            tr.append("<td data-th='شعبه صادر کننده'>" + json[i].ProjectSectionRecoupment.BranchExporter + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Update", ProjectSectionRecoupmentPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionRecoupment/_Delete", ProjectSectionRecoupmentPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionRecoupment(" + json[i].ProjectSectionRecoupment.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionRecoupment'," + json[i].ProjectSectionRecoupment.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Update", ProjectSectionRecoupmentPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionRecoupment(" + json[i].ProjectSectionRecoupment.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Delete", ProjectSectionRecoupmentPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionRecoupment'," + json[i].ProjectSectionRecoupment.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionRecoupment tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Update", ProjectSectionRecoupmentPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionRecoupment/_Delete", ProjectSectionRecoupmentPermissions) == -1) {
            $('#tbl-ProjectSectionRecoupment th:last').remove();
            $('#tbl-ProjectSectionRecoupment tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionRecoupment tfoot td').attr('colspan', $('#tbl-ProjectSectionRecoupment tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionRecoupment(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_List", ProjectSectionRecoupmentPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionRecoupment(pageRecord, $('#tbl-ProjectSectionRecoupment .page-size').val(), $('#sort-ProjectSectionRecoupment').val());

        Pager(pageRecord, $('#tbl-ProjectSectionRecoupment .page-size').val(), "ProjectSectionRecoupment", totalRecords);
    }
}

function ClearFormProjectSectionRecoupment() {
    
    $('#frm-ProjectSectionRecoupment input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionRecoupmentFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionRecoupment/_Create", ProjectSectionRecoupmentPermissions) > -1) {
        $('#ProjectSectionRecoupmentId').val("-1");
        $('#btnSaveProjectSectionRecoupment').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionRecoupment').validate();
    $('#frm-ProjectSectionRecoupment').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionRecoupment(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionRecoupment/_Update', { Id: id }, '#FormContainer-ProjectSectionRecoupment', 'UpdateProjectSectionRecoupmentCallback();');
}



function DeleteProjectSectionRecoupment(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionRecoupment/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionRecoupment tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionRecoupment .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionRecoupment .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionRecoupment .page-record').val() - 1;
        }

        LoadDataProjectSectionRecoupment(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionRecoupment() {
    if ($('#ProjectSectionRecoupmentId').val() != '-1')
        $('#btnSaveProjectSectionRecoupment').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionRecoupmentFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionRecoupment/GetProjectSectionRecoupmentFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionRecoupment-fileupload').fileupload('option', 'done').call($('#ProjectSectionRecoupment-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionRecoupmentFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionRecoupment/ReloadFiles', {}, function (data, textStatus, xhr) { });
}