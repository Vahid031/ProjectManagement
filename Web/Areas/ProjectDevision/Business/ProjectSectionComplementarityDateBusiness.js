var ProjectSectionComplementarityDatePermissions;


$(function () {
    ProjectSectionComplementarityDatePermissions = $('#permission-ProjectSectionComplementarityDate').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_List", ProjectSectionComplementarityDatePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityDate/_List', '', '#FormList-ProjectSectionComplementarityDate', 'ListProjectSectionComplementarityDateCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Create", ProjectSectionComplementarityDatePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityDate/_Create', '', '#FormContainer-ProjectSectionComplementarityDate', 'CreateProjectSectionComplementarityDateCallback();');
    }

    EventHandlerProjectSectionComplementarityDate();
});

function CreateProjectSectionComplementarityDateCallback() {
    CheckValueProjectSectionComplementarityDate();
    HandleValidation();

    DatePic('#ProjectSectionComplementarityDate_Date');
    DatePic('#ProjectSectionComplementarityDate_NewDate');
}

function UpdateProjectSectionComplementarityDateCallback() {
    CreateProjectSectionComplementarityDateCallback();
}

function ListProjectSectionComplementarityDateCallback() {
    Pager(1, 5, "ProjectSectionComplementarityDate", DataRefreshProjectSectionComplementarityDate(1, 5, $("#sort-ProjectSectionComplementarityDate").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionComplementarityDate() {
    $("#FormContainer-ProjectSectionComplementarityDate").on("submit", "#frm-ProjectSectionComplementarityDate", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityDate/_Create', 'Files=' + $('#images-ProjectSectionComplementarityDate').val() + "&" + 'ProjectSectionComplementarityDate.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionComplementarityDate').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionComplementarityDate();

            if ($('#tbl-ProjectSectionComplementarityDate .page-record').val() == null)
                LoadDataProjectSectionComplementarityDate(1);
            else
                LoadDataProjectSectionComplementarityDate($('#tbl-ProjectSectionComplementarityDate .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Create", ProjectSectionComplementarityDatePermissions) == -1) {
                $('#FormContainer-ProjectSectionComplementarityDate').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionComplementarityDate").on("click", "#frm-ProjectSectionComplementarityDate .btnNew", function () {
        ClearFormProjectSectionComplementarityDate();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionComplementarityDate").on("keypress", "#tbl-ProjectSectionComplementarityDate tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionComplementarityDate(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionComplementarityDate").on("change keyup", "#tbl-ProjectSectionComplementarityDate tbody tr:first select", function (e) {
        LoadDataProjectSectionComplementarityDate(1);
    });


    $("#FormContainer-ProjectSectionComplementarityDate").on("click", "#btnShowProjectSectionComplementarityDateFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionComplementarityDate/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionComplementarityDate').val('');
    $('.ProjectSectionComplementarityDateFiles').each(function (i, row) {
        $('#images-ProjectSectionComplementarityDate').val($('#images-ProjectSectionComplementarityDate').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionComplementarityDate-fileupload').fileupload();

    $('#ProjectSectionComplementarityDate-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionComplementarityDateFiles($('#ProjectSectionComplementarityDateId').val());
}

function DataRefreshProjectSectionComplementarityDate(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionComplementarityDate.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionComplementarityDate').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityDate/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionComplementarityDate tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ'>" + json[i].ProjectSectionComplementarityDate.Date + "</td>");
            tr.append("<td data-th='تاریخ جدید'>" + json[i].ProjectSectionComplementarityDate.NewDate + "</td>");
            tr.append("<td data-th='شماره'>" + json[i].ProjectSectionComplementarityDate.Number + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Update", ProjectSectionComplementarityDatePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Delete", ProjectSectionComplementarityDatePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionComplementarityDate(" + json[i].ProjectSectionComplementarityDate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionComplementarityDate'," + json[i].ProjectSectionComplementarityDate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Update", ProjectSectionComplementarityDatePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionComplementarityDate(" + json[i].ProjectSectionComplementarityDate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Delete", ProjectSectionComplementarityDatePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionComplementarityDate'," + json[i].ProjectSectionComplementarityDate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionComplementarityDate tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Update", ProjectSectionComplementarityDatePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Delete", ProjectSectionComplementarityDatePermissions) == -1) {
            $('#tbl-ProjectSectionComplementarityDate th:last').remove();
            $('#tbl-ProjectSectionComplementarityDate tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionComplementarityDate tfoot td').attr('colspan', $('#tbl-ProjectSectionComplementarityDate tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionComplementarityDate(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_List", ProjectSectionComplementarityDatePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionComplementarityDate(pageRecord, $('#tbl-ProjectSectionComplementarityDate .page-size').val(), $('#sort-ProjectSectionComplementarityDate').val());

        Pager(pageRecord, $('#tbl-ProjectSectionComplementarityDate .page-size').val(), "ProjectSectionComplementarityDate", totalRecords);
    }
}

function ClearFormProjectSectionComplementarityDate() {
    
    $('#frm-ProjectSectionComplementarityDate input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionComplementarityDateFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityDate/_Create", ProjectSectionComplementarityDatePermissions) > -1) {
        $('#ProjectSectionComplementarityDateId').val("-1");
        $('#btnSaveProjectSectionComplementarityDate').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionComplementarityDate').validate();
    $('#frm-ProjectSectionComplementarityDate').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionComplementarityDate(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionComplementarityDate/_Update', { Id: id }, '#FormContainer-ProjectSectionComplementarityDate', 'UpdateProjectSectionComplementarityDateCallback();');
}



function DeleteProjectSectionComplementarityDate(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityDate/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionComplementarityDate tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionComplementarityDate .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionComplementarityDate .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionComplementarityDate .page-record').val() - 1;
        }

        LoadDataProjectSectionComplementarityDate(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionComplementarityDate() {
    if ($('#ProjectSectionComplementarityDateId').val() != '-1')
        $('#btnSaveProjectSectionComplementarityDate').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionComplementarityDateFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityDate/GetProjectSectionComplementarityDateFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionComplementarityDate-fileupload').fileupload('option', 'done').call($('#ProjectSectionComplementarityDate-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionComplementarityDateFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityDate/ReloadFiles', {}, function (data, textStatus, xhr) { });
}