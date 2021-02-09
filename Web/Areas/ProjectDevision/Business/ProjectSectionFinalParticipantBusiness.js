var ProjectSectionFinalParticipantPermissions;


$(function () {
    ProjectSectionFinalParticipantPermissions = $('#permission-ProjectSectionFinalParticipant').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_List/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinalParticipant/_List', '', '#FormList-ProjectSectionFinalParticipant', 'ListProjectSectionFinalParticipantCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinalParticipant/_Create', '', '#FormContainer-ProjectSectionFinalParticipant', 'CreateProjectSectionFinalParticipantCallback();');
    }

    EventHandlerProjectSectionFinalParticipant();
});

function CreateProjectSectionFinalParticipantCallback() {
    CheckValueProjectSectionFinalParticipant();
    HandleValidation();

    DatePic('#ProjectSectionFinalParticipant_WarrantyDate');
}

function UpdateProjectSectionFinalParticipantCallback() {
    CreateProjectSectionFinalParticipantCallback();
}

function ListProjectSectionFinalParticipantCallback() {
    Pager(1, 5, "ProjectSectionFinalParticipant", DataRefreshProjectSectionFinalParticipant(1, 5, $("#sort-ProjectSectionFinalParticipant").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionFinalParticipant() {
    $("#FormContainer-ProjectSectionFinalParticipant").on("submit", "#frm-ProjectSectionFinalParticipant", function (e) {
        e.preventDefault();

        $('#ProjectSectionFinalParticipant_SuggestPrice').val($('#ProjectSectionFinalParticipant_SuggestPrice').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDevision/ProjectSectionFinalParticipant/_Create', 'Files=' + $('#images-ProjectSectionFinalParticipant').val() + "&" + 'ProjectSectionFinalParticipant.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionFinalParticipant').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionFinalParticipant();

            if ($('#tbl-ProjectSectionFinalParticipant .page-record').val() == null)
                LoadDataProjectSectionFinalParticipant(1);
            else
                LoadDataProjectSectionFinalParticipant($('#tbl-ProjectSectionFinalParticipant .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) == -1) {
                $('#FormContainer-ProjectSectionFinalParticipant').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionFinalParticipant").on("click", "#frm-ProjectSectionFinalParticipant .btnNew", function () {
        ClearFormProjectSectionFinalParticipant();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionFinalParticipant").on("keypress", "#tbl-ProjectSectionFinalParticipant tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionFinalParticipant(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionFinalParticipant").on("change keyup", "#tbl-ProjectSectionFinalParticipant tbody tr:first select", function (e) {
        LoadDataProjectSectionFinalParticipant(1);
    });


    $("#FormContainer-ProjectSectionFinalParticipant").on("click", "#btnShowProjectSectionFinalParticipantFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionFinalParticipant/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionFinalParticipant').val('');
    $('.ProjectSectionFinalParticipantFiles').each(function (i, row) {
        $('#images-ProjectSectionFinalParticipant').val($('#images-ProjectSectionFinalParticipant').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionFinalParticipant-fileupload').fileupload();

    $('#ProjectSectionFinalParticipant-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionFinalParticipantFiles($('#ProjectSectionFinalParticipantId').val());
}

function DataRefreshProjectSectionFinalParticipant(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionFinalParticipant.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionFinalParticipant').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionFinalParticipant/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionFinalParticipant tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام شرکت'>" + json[i].Contractor.CompanyName + "</td>");
            tr.append("<td data-th='مبلغ پیشنهادی'>" + Seprator(json[i].ProjectSectionFinalParticipant.SuggestPrice) + "</td>");
            tr.append("<td data-th='تاریخ اتمام ضمانت نامه'>" + json[i].ProjectSectionFinalParticipant.WarrantyDate + "</td>");
            tr.append("<td data-th='شماره تضمین'>" + json[i].ProjectSectionFinalParticipant.WarrantyNumber + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionFinalParticipant(" + json[i].ProjectSectionFinalParticipant.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionFinalParticipant'," + json[i].ProjectSectionFinalParticipant.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionFinalParticipant(" + json[i].ProjectSectionFinalParticipant.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionFinalParticipant'," + json[i].ProjectSectionFinalParticipant.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionFinalParticipant tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) == -1) {
            $('#tbl-ProjectSectionFinalParticipant th:last').remove();
            $('#tbl-ProjectSectionFinalParticipant tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionFinalParticipant tfoot td').attr('colspan', $('#tbl-ProjectSectionFinalParticipant tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionFinalParticipant(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_List/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionFinalParticipant(pageRecord, $('#tbl-ProjectSectionFinalParticipant .page-size').val(), $('#sort-ProjectSectionFinalParticipant').val());

        Pager(pageRecord, $('#tbl-ProjectSectionFinalParticipant .page-size').val(), "ProjectSectionFinalParticipant", totalRecords);
    }
}

function ClearFormProjectSectionFinalParticipant() {
    
    $('#frm-ProjectSectionFinalParticipant input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionFinalParticipantFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionFinalParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinalParticipantPermissions) > -1) {
        $('#ProjectSectionFinalParticipantId').val("-1");
        $('#btnSaveProjectSectionFinalParticipant').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionFinalParticipant').validate();
    $('#frm-ProjectSectionFinalParticipant').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionFinalParticipant(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionFinalParticipant/_Update', { Id: id }, '#FormContainer-ProjectSectionFinalParticipant', 'UpdateProjectSectionFinalParticipantCallback();');
}



function DeleteProjectSectionFinalParticipant(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionFinalParticipant/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionFinalParticipant tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionFinalParticipant .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionFinalParticipant .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionFinalParticipant .page-record').val() - 1;
        }

        LoadDataProjectSectionFinalParticipant(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionFinalParticipant() {
    if ($('#ProjectSectionFinalParticipantId').val() != '-1')
        $('#btnSaveProjectSectionFinalParticipant').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionFinalParticipantFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionFinalParticipant/GetProjectSectionFinalParticipantFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionFinalParticipant-fileupload').fileupload('option', 'done').call($('#ProjectSectionFinalParticipant-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionFinalParticipantFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionFinalParticipant/ReloadFiles', {}, function (data, textStatus, xhr) { });
}