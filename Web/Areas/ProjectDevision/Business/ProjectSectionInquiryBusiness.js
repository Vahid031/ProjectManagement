var ProjectSectionInquiryPermissions;


$(function () {
    ProjectSectionInquiryPermissions = $('#permission-ProjectSectionInquiry').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_List/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionInquiry/_List', '', '#FormList-ProjectSectionInquiry', 'ListProjectSectionInquiryCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Create/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionInquiry/_Create', '', '#FormContainer-ProjectSectionInquiry', 'CreateProjectSectionInquiryCallback();');
    }

    EventHandlerProjectSectionInquiry();
});

function CreateProjectSectionInquiryCallback() {
    CheckValueProjectSectionInquiry();
    HandleValidation();

    DatePic('#ProjectSectionInquiry_InquiryDate');
}

function UpdateProjectSectionInquiryCallback() {
    CreateProjectSectionInquiryCallback();
}

function ListProjectSectionInquiryCallback() {
    Pager(1, 5, "ProjectSectionInquiry", DataRefreshProjectSectionInquiry(1, 5, $("#sort-ProjectSectionInquiry").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionInquiry() {
    $("#FormContainer-ProjectSectionInquiry").on("submit", "#frm-ProjectSectionInquiry", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionInquiry/_Create', 'Files=' + $('#images-ProjectSectionInquiry').val() + "&" + 'ProjectSectionInquiry.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionInquiry').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionInquiry();

            if ($('#tbl-ProjectSectionInquiry .page-record').val() == null)
                LoadDataProjectSectionInquiry(1);
            else
                LoadDataProjectSectionInquiry($('#tbl-ProjectSectionInquiry .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Create/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) == -1) {
                $('#FormContainer-ProjectSectionInquiry').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionInquiry").on("click", "#frm-ProjectSectionInquiry .btnNew", function () {
        ClearFormProjectSectionInquiry();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionInquiry").on("keypress", "#tbl-ProjectSectionInquiry tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionInquiry(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionInquiry").on("change keyup", "#tbl-ProjectSectionInquiry tbody tr:first select", function (e) {
        LoadDataProjectSectionInquiry(1);
    });


    $("#FormContainer-ProjectSectionInquiry").on("click", "#btnShowInqueryFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionInquiry/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionInquiry').val('');
    $('.ProjectSectionInquiryFiles').each(function (i, row) {
        $('#images-ProjectSectionInquiry').val($('#images-ProjectSectionInquiry').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionInquiry-fileupload').fileupload();

    $('#ProjectSectionInquiry-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionInquiryFiles($('#ProjectSectionInquiryId').val());
}

function DataRefreshProjectSectionInquiry(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionInquiry.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionInquiry').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionInquiry/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionInquiry tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionInquiry.InquiryNumber)'>" + json[i].ProjectSectionInquiry.InquiryNumber + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionInquiry.InquiryDate)'>" + json[i].ProjectSectionInquiry.InquiryDate + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Update/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionInquiry/_Delete/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionInquiry(" + json[i].ProjectSectionInquiry.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionInquiry'," + json[i].ProjectSectionInquiry.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Update/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionInquiry(" + json[i].ProjectSectionInquiry.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Delete/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionInquiry'," + json[i].ProjectSectionInquiry.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionInquiry tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Update/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionInquiry/_Delete/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) == -1) {
            $('#tbl-ProjectSectionInquiry th:last').remove();
            $('#tbl-ProjectSectionInquiry tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionInquiry tfoot td').attr('colspan', $('#tbl-ProjectSectionInquiry tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionInquiry(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_List/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionInquiry(pageRecord, $('#tbl-ProjectSectionInquiry .page-size').val(), $('#sort-ProjectSectionInquiry').val());

        Pager(pageRecord, $('#tbl-ProjectSectionInquiry .page-size').val(), "ProjectSectionInquiry", totalRecords);
    }
}

function ClearFormProjectSectionInquiry() {
    
    $('#frm-ProjectSectionInquiry input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionInquiryFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionInquiry/_Create/" + SelectedProjectAssignmentType, ProjectSectionInquiryPermissions) > -1) {
        $('#ProjectSectionInquiryId').val("-1");
        $('#btnSaveProjectSectionInquiry').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionInquiry').validate();
    $('#frm-ProjectSectionInquiry').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionInquiry(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionInquiry/_Update', { Id: id }, '#FormContainer-ProjectSectionInquiry', 'UpdateProjectSectionInquiryCallback();');
}



function DeleteProjectSectionInquiry(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionInquiry/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionInquiry tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionInquiry .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionInquiry .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionInquiry .page-record').val() - 1;
        }

        LoadDataProjectSectionInquiry(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionInquiry() {
    if ($('#ProjectSectionInquiryId').val() != '-1')
        $('#btnSaveProjectSectionInquiry').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionInquiryFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionInquiry/GetProjectSectionInquiryFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionInquiry-fileupload').fileupload('option', 'done').call($('#ProjectSectionInquiry-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionInquiryFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionInquiry/ReloadFiles', {}, function (data, textStatus, xhr) { });
}