var ProjectSectionSupervisorVisitPermissions;


$(function () {
    ProjectSectionSupervisorVisitPermissions = $('#permission-ProjectSectionSupervisorVisit').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_List", ProjectSectionSupervisorVisitPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorVisit/_List', '', '#FormList-ProjectSectionSupervisorVisit', 'ListProjectSectionSupervisorVisitCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Create", ProjectSectionSupervisorVisitPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorVisit/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionSupervisorVisit', 'CreateProjectSectionSupervisorVisitCallback();');
    }

    EventHandlerProjectSectionSupervisorVisit();
});

function CreateProjectSectionSupervisorVisitCallback() {
    CheckValueProjectSectionSupervisorVisit();
    HandleValidation();

    DatePic('#ProjectSectionSupervisorVisit_VisitDate');
}

function UpdateProjectSectionSupervisorVisitCallback() {
    CreateProjectSectionSupervisorVisitCallback();
}

function ListProjectSectionSupervisorVisitCallback() {
    Pager(1, 5, "ProjectSectionSupervisorVisit", DataRefreshProjectSectionSupervisorVisit(1, 5, $("#sort-ProjectSectionSupervisorVisit").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionSupervisorVisit() {
    $("#FormContainer-ProjectSectionSupervisorVisit").on("submit", "#frm-ProjectSectionSupervisorVisit", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorVisit/_Create', 'Files=' + $('#images-ProjectSectionSupervisorVisit').val() + "&" + 'ProjectSectionSupervisorVisit.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionSupervisorVisit').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionSupervisorVisit();

            if ($('#tbl-ProjectSectionSupervisorVisit .page-record').val() == null)
                LoadDataProjectSectionSupervisorVisit(1);
            else
                LoadDataProjectSectionSupervisorVisit($('#tbl-ProjectSectionSupervisorVisit .page-record').val());

            LoadDataProjectSectionOperation($('#tbl-ProjectSectionOperation .page-record').val());
            SelectProjectSectionOperation();

            if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Create", ProjectSectionSupervisorVisitPermissions) == -1) {
                $('#FormContainer-ProjectSectionSupervisorVisit').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionSupervisorVisit").on("click", "#frm-ProjectSectionSupervisorVisit .btnNew", function () {
        ClearFormProjectSectionSupervisorVisit();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionSupervisorVisit").on("keypress", "#tbl-ProjectSectionSupervisorVisit tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionSupervisorVisit(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionSupervisorVisit").on("change keyup", "#tbl-ProjectSectionSupervisorVisit tbody tr:first select", function (e) {
        LoadDataProjectSectionSupervisorVisit(1);
    });


    $("#FormContainer-ProjectSectionSupervisorVisit").on("click", "#btnShowProjectSectionSupervisorVisitFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionSupervisorVisit/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionSupervisorVisit').val('');
    $('.ProjectSectionSupervisorVisitFiles').each(function (i, row) {
        $('#images-ProjectSectionSupervisorVisit').val($('#images-ProjectSectionSupervisorVisit').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionSupervisorVisit-fileupload').fileupload();

    $('#ProjectSectionSupervisorVisit-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionSupervisorVisitFiles($('#ProjectSectionSupervisorVisitId').val());
}

function DataRefreshProjectSectionSupervisorVisit(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionSupervisorVisit.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionSupervisorVisit').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorVisit/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionSupervisorVisit tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ بازدید'>" + json[i].ProjectSectionSupervisorVisit.VisitDate + "</td>");
            tr.append("<td data-th='عملیات'>" + json[i].ProjectSectionOperationTitle.Title + "</td>");
            tr.append("<td data-th='درصد پیشرفت'>" + json[i].ProjectSectionSupervisorVisit.DevelopmentPercent + "</td>");
            tr.append("<td data-th='دصتور کار'>" + json[i].ProjectSectionSupervisorVisit.Agendum + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionSupervisorVisit.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Update", ProjectSectionSupervisorVisitPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Delete", ProjectSectionSupervisorVisitPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionSupervisorVisit(" + json[i].ProjectSectionSupervisorVisit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionSupervisorVisit'," + json[i].ProjectSectionSupervisorVisit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Update", ProjectSectionSupervisorVisitPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionSupervisorVisit(" + json[i].ProjectSectionSupervisorVisit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Delete", ProjectSectionSupervisorVisitPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionSupervisorVisit'," + json[i].ProjectSectionSupervisorVisit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionSupervisorVisit tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Update", ProjectSectionSupervisorVisitPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Delete", ProjectSectionSupervisorVisitPermissions) == -1) {
            $('#tbl-ProjectSectionSupervisorVisit th:last').remove();
            $('#tbl-ProjectSectionSupervisorVisit tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionSupervisorVisit tfoot td').attr('colspan', $('#tbl-ProjectSectionSupervisorVisit tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionSupervisorVisit(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_List", ProjectSectionSupervisorVisitPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionSupervisorVisit(pageRecord, $('#tbl-ProjectSectionSupervisorVisit .page-size').val(), $('#sort-ProjectSectionSupervisorVisit').val());

        Pager(pageRecord, $('#tbl-ProjectSectionSupervisorVisit .page-size').val(), "ProjectSectionSupervisorVisit", totalRecords);
    }
}

function ClearFormProjectSectionSupervisorVisit() {
    
    $('#frm-ProjectSectionSupervisorVisit input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionSupervisorVisitFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionSupervisorVisit/_Create", ProjectSectionSupervisorVisitPermissions) > -1) {
        $('#ProjectSectionSupervisorVisitId').val("-1");
        $('#btnSaveProjectSectionSupervisorVisit').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionSupervisorVisit').validate();
    $('#frm-ProjectSectionSupervisorVisit').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionSupervisorVisit(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionSupervisorVisit/_Update', { Id: id }, '#FormContainer-ProjectSectionSupervisorVisit', 'UpdateProjectSectionSupervisorVisitCallback();');
}



function DeleteProjectSectionSupervisorVisit(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorVisit/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionSupervisorVisit tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionSupervisorVisit .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionSupervisorVisit .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionSupervisorVisit .page-record').val() - 1;
        }

        LoadDataProjectSectionSupervisorVisit(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionSupervisorVisit() {
    if ($('#ProjectSectionSupervisorVisitId').val() != '-1')
        $('#btnSaveProjectSectionSupervisorVisit').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionSupervisorVisitFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorVisit/GetProjectSectionSupervisorVisitFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionSupervisorVisit-fileupload').fileupload('option', 'done').call($('#ProjectSectionSupervisorVisit-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionSupervisorVisitFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionSupervisorVisit/ReloadFiles', {}, function (data, textStatus, xhr) { });
}