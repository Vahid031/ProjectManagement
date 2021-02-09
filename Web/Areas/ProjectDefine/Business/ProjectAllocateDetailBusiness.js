var ProjectAllocateDetailPermissions;


$(function () {
    ProjectAllocateDetailPermissions = $('#permission-ProjectAllocateDetail').val().split(',');

    if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_List", ProjectAllocateDetailPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectAllocateDetail/_List', '', '#FormList-ProjectAllocateDetail', 'ListProjectAllocateDetailCallback();');
    }

    if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Create", ProjectAllocateDetailPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectAllocateDetail/_Create', '', '#FormContainer-ProjectAllocateDetail', 'CreateProjectAllocateDetailCallback();');
    }

    EventHandlerProjectAllocateDetail();
});

function CreateProjectAllocateDetailCallback() {
    CheckValueProjectAllocateDetail();
    HandleValidation();

    DatePic('#ProjectAllocate_AllocateDate');
}

function UpdateProjectAllocateDetailCallback() {
    CheckValueProjectAllocateDetail();
    HandleValidation();

    DatePic('#ProjectAllocate_AllocateDate');

}

function ListProjectAllocateDetailCallback() {
    Pager(1, 5, "ProjectAllocateDetail", DataRefreshProjectAllocateDetail(1, 5, $("#sort-ProjectAllocateDetail").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectAllocateDetail() {
    $("#FormContainer-ProjectAllocateDetail").on("submit", "#frm-ProjectAllocateDetail", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProject == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
            }

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectAllocate_CreditAmount').val($('#ProjectAllocate_CreditAmount').val().replace(/\,/g, ''));
        $('#ProjectAllocate_AllocateAmount').val($('#ProjectAllocate_AllocateAmount').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/_Create', 'Files=' + $('#images-ProjectAllocate').val() + "&" + 'ProjectAllocate.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectAllocateDetail').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectAllocateDetail();

            if ($('#tbl-ProjectAllocateDetail .page-record').val() == null)
                LoadDataProjectAllocateDetail(1);
            else
                LoadDataProjectAllocateDetail($('#tbl-ProjectAllocateDetail .page-record').val());


            // تغییر رنگ فونت جدول بالایی
            LoadDataProjectAllocate($('#tbl-ProjectAllocate .page-record').val());
            SelectProject();


            if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Create", ProjectAllocateDetailPermissions) == -1) {
                $('#FormContainer-ProjectAllocateDetail').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectAllocateDetail").on("click", "#frm-ProjectAllocateDetail .btnNew", function () {
        ClearFormProjectAllocateDetail();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectAllocateDetail").on("keypress", "#tbl-ProjectAllocateDetail tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectAllocateDetail(1);
            return false;
        }
    });

    $("#FormList-ProjectAllocateDetail").on("change keyup", "#tbl-ProjectAllocateDetail tbody tr:first select", function (e) {
        LoadDataProjectAllocateDetail(1);
    });

    $("#FormContainer-ProjectAllocateDetail").on("click", "#btnShowProjectAllocateFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDefine/ProjectAllocateDetail/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectAllocate').val('');
    $('.ProjectAllocateFiles').each(function (i, row) {
        $('#images-ProjectAllocate').val($('#images-ProjectAllocate').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectAllocate-fileupload').fileupload();

    $('#ProjectAllocate-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectAllocateFiles($('#ProjectAllocateId').val());
}


function DataRefreshProjectAllocateDetail(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectAllocate.ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectAllocateDetail').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectAllocateDetail tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='مبلغ اعتبار'>" + Seprator(json[i].ProjectAllocate.CreditAmount) + "</td>");
            tr.append("<td data-th='مبلغ تخصیص'>" + Seprator(json[i].ProjectAllocate.AllocateAmount) + "</td>");
            tr.append("<td data-th='درصد اعبتار'>" + json[i].ProjectAllocate.CreditPercent + "</td>");
            tr.append("<td data-th='تاریخ تخصیص'>" + json[i].ProjectAllocate.AllocateDate + "</td>");

            if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Update", ProjectAllocateDetailPermissions) > -1 && $.inArray("/ProjectDefine/ProjectAllocateDetail/_Delete", ProjectAllocateDetailPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectAllocateDetail(" + json[i].ProjectAllocate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectAllocateDetail'," + json[i].ProjectAllocate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Update", ProjectAllocateDetailPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectAllocateDetail(" + json[i].ProjectAllocate.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Delete", ProjectAllocateDetailPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectAllocateDetail'," + json[i].ProjectAllocate.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectAllocateDetail tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Update", ProjectAllocateDetailPermissions) == -1 && $.inArray("/ProjectDefine/ProjectAllocateDetail/_Delete", ProjectAllocateDetailPermissions) == -1) {
            $('#tbl-ProjectAllocateDetail th:last').remove();
            $('#tbl-ProjectAllocateDetail tbody tr:first td:last').remove();
            $('#tbl-ProjectAllocateDetail tfoot td').attr('colspan', $('#tbl-ProjectAllocateDetail tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}





function LoadDataProjectAllocateDetail(pageRecord) {
    if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_List", ProjectAllocateDetailPermissions) > -1) {
        var totalRecords = DataRefreshProjectAllocateDetail(pageRecord, $('#tbl-ProjectAllocateDetail .page-size').val(), $('#sort-ProjectAllocateDetail').val());

        Pager(pageRecord, $('#tbl-ProjectAllocateDetail .page-size').val(), "ProjectAllocateDetail", totalRecords);
    }
}

function ClearFormProjectAllocateDetail() {
    
    $('#frm-ProjectAllocateDetail input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectAllocateFiles();

    if ($.inArray("/ProjectDefine/ProjectAllocateDetail/_Create", ProjectAllocateDetailPermissions) > -1) {
        $('#ProjectAllocateId').val("-1");
        $('#btnSaveProjectAllocateDetail').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectAllocateDetail').validate();
    $('#frm-ProjectAllocateDetail').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectAllocateDetail(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProjectAllocateDetail/_Update', { Id: id }, '#FormContainer-ProjectAllocateDetail', 'UpdateProjectAllocateDetailCallback();');
}



function DeleteProjectAllocateDetail(id) {
    Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectAllocateDetail tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectAllocateDetail .page-record').val();
        }
        else {
            if ($('#tbl-ProjectAllocateDetail .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectAllocateDetail .page-record').val() - 1;
        }

        LoadDataProjectAllocateDetail(pageRecord);
    }, 'json');
}


function CheckValueProjectAllocateDetail() {
    if ($('#ProjectAllocateId').val() != '-1')
        $('#btnSaveProjectAllocateDetail').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}

function GetProjectAllocateFiles(projectSectionId) {
    Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/GetProjectAllocateFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectAllocate-fileupload').fileupload('option', 'done').call($('#ProjectAllocate-fileupload'), $.Event('done'), { result: { files: files } });
    }, 'json');
}


function ReloadProjectAllocateFiles() {
    Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/ReloadFiles', {}, function (data, textStatus, xhr) { });
}

