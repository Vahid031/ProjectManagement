var ProjectSectionAdvisorVisitPermissions;


$(function () {
    ProjectSectionAdvisorVisitPermissions = $('#permission-ProjectSectionAdvisorVisit').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_List", ProjectSectionAdvisorVisitPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorVisit/_List', '', '#FormList-ProjectSectionAdvisorVisit', 'ListProjectSectionAdvisorVisitCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Create", ProjectSectionAdvisorVisitPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorVisit/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionAdvisorVisit', 'CreateProjectSectionAdvisorVisitCallback();');
    }

    EventHandlerProjectSectionAdvisorVisit();
});

function CreateProjectSectionAdvisorVisitCallback() {
    CheckValueProjectSectionAdvisorVisit();
    HandleValidation();

    DatePic('#ProjectSectionAdvisorVisit_VisitDate');
}

function UpdateProjectSectionAdvisorVisitCallback() {
    CreateProjectSectionAdvisorVisitCallback();
}

function ListProjectSectionAdvisorVisitCallback() {
    Pager(1, 5, "ProjectSectionAdvisorVisit", DataRefreshProjectSectionAdvisorVisit(1, 5, $("#sort-ProjectSectionAdvisorVisit").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionAdvisorVisit() {
    $("#FormContainer-ProjectSectionAdvisorVisit").on("submit", "#frm-ProjectSectionAdvisorVisit", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorVisit/_Create', 'Files=' + $('#images-ProjectSectionAdvisorVisit').val() + "&" + 'ProjectSectionAdvisorVisit.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionAdvisorVisit').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionAdvisorVisit();

            if ($('#tbl-ProjectSectionAdvisorVisit .page-record').val() == null)
                LoadDataProjectSectionAdvisorVisit(1);
            else
                LoadDataProjectSectionAdvisorVisit($('#tbl-ProjectSectionAdvisorVisit .page-record').val());

            LoadDataProjectSectionOperation($('#tbl-ProjectSectionOperation .page-record').val());
            SelectProjectSectionOperation();

            if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Create", ProjectSectionAdvisorVisitPermissions) == -1) {
                $('#FormContainer-ProjectSectionAdvisorVisit').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionAdvisorVisit").on("click", "#frm-ProjectSectionAdvisorVisit .btnNew", function () {
        ClearFormProjectSectionAdvisorVisit();

        $('#Alert,#AlertDown').slideUp(300);
    });
    

    $("#FormList-ProjectSectionAdvisorVisit").on("keypress", "#tbl-ProjectSectionAdvisorVisit tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionAdvisorVisit(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionAdvisorVisit").on("change keyup", "#tbl-ProjectSectionAdvisorVisit tbody tr:first select", function (e) {
        LoadDataProjectSectionAdvisorVisit(1);
    });


    $("#FormContainer-ProjectSectionAdvisorVisit").on("click", "#btnShowProjectSectionAdvisorVisitFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionAdvisorVisit/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionAdvisorVisit').val('');
    $('.ProjectSectionAdvisorVisitFiles').each(function (i, row) {
        $('#images-ProjectSectionAdvisorVisit').val($('#images-ProjectSectionAdvisorVisit').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionAdvisorVisit-fileupload').fileupload();

    $('#ProjectSectionAdvisorVisit-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionAdvisorVisitFiles($('#ProjectSectionAdvisorVisitId').val());
}

function DataRefreshProjectSectionAdvisorVisit(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionAdvisorVisit.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionAdvisorVisit').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorVisit/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionAdvisorVisit tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ بازدید'>" + json[i].ProjectSectionAdvisorVisit.VisitDate + "</td>");
            tr.append("<td data-th='دستور کار'>" + json[i].ProjectSectionAdvisorVisit.Agendum + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionAdvisorVisit.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Update", ProjectSectionAdvisorVisitPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Delete", ProjectSectionAdvisorVisitPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionAdvisorVisit(" + json[i].ProjectSectionAdvisorVisit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionAdvisorVisit'," + json[i].ProjectSectionAdvisorVisit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Update", ProjectSectionAdvisorVisitPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionAdvisorVisit(" + json[i].ProjectSectionAdvisorVisit.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Delete", ProjectSectionAdvisorVisitPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionAdvisorVisit'," + json[i].ProjectSectionAdvisorVisit.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionAdvisorVisit tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Update", ProjectSectionAdvisorVisitPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Delete", ProjectSectionAdvisorVisitPermissions) == -1) {
            $('#tbl-ProjectSectionAdvisorVisit th:last').remove();
            $('#tbl-ProjectSectionAdvisorVisit tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionAdvisorVisit tfoot td').attr('colspan', $('#tbl-ProjectSectionAdvisorVisit tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionAdvisorVisit(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_List", ProjectSectionAdvisorVisitPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionAdvisorVisit(pageRecord, $('#tbl-ProjectSectionAdvisorVisit .page-size').val(), $('#sort-ProjectSectionAdvisorVisit').val());

        Pager(pageRecord, $('#tbl-ProjectSectionAdvisorVisit .page-size').val(), "ProjectSectionAdvisorVisit", totalRecords);
    }
}

function ClearFormProjectSectionAdvisorVisit() {
    
    $('#frm-ProjectSectionAdvisorVisit input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionAdvisorVisitFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionAdvisorVisit/_Create", ProjectSectionAdvisorVisitPermissions) > -1) {
        $('#ProjectSectionAdvisorVisitId').val("-1");
        $('#btnSaveProjectSectionAdvisorVisit').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionAdvisorVisit').validate();
    $('#frm-ProjectSectionAdvisorVisit').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionAdvisorVisit(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionAdvisorVisit/_Update', { Id: id }, '#FormContainer-ProjectSectionAdvisorVisit', 'UpdateProjectSectionAdvisorVisitCallback();');
}



function DeleteProjectSectionAdvisorVisit(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorVisit/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionAdvisorVisit tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionAdvisorVisit .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionAdvisorVisit .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionAdvisorVisit .page-record').val() - 1;
        }

        LoadDataProjectSectionAdvisorVisit(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionAdvisorVisit() {
    if ($('#ProjectSectionAdvisorVisitId').val() != '-1')
        $('#btnSaveProjectSectionAdvisorVisit').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionAdvisorVisitFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorVisit/GetProjectSectionAdvisorVisitFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionAdvisorVisit-fileupload').fileupload('option', 'done').call($('#ProjectSectionAdvisorVisit-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionAdvisorVisitFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionAdvisorVisit/ReloadFiles', {}, function (data, textStatus, xhr) { });
}