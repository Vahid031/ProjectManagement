var ProjectSectionDeliveryTemproryPermissions;


$(function () {
    ProjectSectionDeliveryTemproryPermissions = $('#permission-ProjectSectionDeliveryTemprory').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_List", ProjectSectionDeliveryTemproryPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryTemprory/_List', '', '#FormList-ProjectSectionDeliveryTemprory', 'ListProjectSectionDeliveryTemproryCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Create", ProjectSectionDeliveryTemproryPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryTemprory/_Create', '', '#FormContainer-ProjectSectionDeliveryTemprory', 'CreateProjectSectionDeliveryTemproryCallback();');
    }

    EventHandlerProjectSectionDeliveryTemprory();

});

function CreateProjectSectionDeliveryTemproryCallback() {
    CheckValueProjectSectionDeliveryTemprory();
    HandleValidation();

    DatePic('#ProjectSectionDeliveryTemprory_ConveneDate');
    DatePic('#ProjectSectionDeliveryTemprory_RequestDate');
    DatePic('#ProjectSectionDeliveryTemprory_MeetingDate');
}

function UpdateProjectSectionDeliveryTemproryCallback() {
    CreateProjectSectionDeliveryTemproryCallback();
}

function ListProjectSectionDeliveryTemproryCallback() {

    Pager(1, 5, "ProjectSectionDeliveryTemprory", DataRefreshProjectSectionDeliveryTemprory(1, 5, $("#sort-ProjectSectionDeliveryTemprory").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionDeliveryTemprory() {
    $("#FormContainer-ProjectSectionDeliveryTemprory").on("submit", "#frm-ProjectSectionDeliveryTemprory", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        if ($('#images-ProjectSectionDeliveryTemprory').val() == '') {
            Messages('warning', 'پیوست الزامی میباشد');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryTemprory/_Create', 'Files=' + $('#images-ProjectSectionDeliveryTemprory').val() + "&" + 'ProjectSectionDeliveryTemprory.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionDeliveryTemprory').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionDeliveryTemprory();

            if ($('#tbl-ProjectSectionDeliveryTemprory .page-record').val() == null)
                LoadDataProjectSectionDeliveryTemprory(1);
            else
                LoadDataProjectSectionDeliveryTemprory($('#tbl-ProjectSectionDeliveryTemprory .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Create", ProjectSectionDeliveryTemproryPermissions) == -1) {
                $('#FormContainer-ProjectSectionDeliveryTemprory').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionDeliveryTemprory").on("click", "#frm-ProjectSectionDeliveryTemprory .btnNew", function () {
        ClearFormProjectSectionDeliveryTemprory();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionDeliveryTemprory").on("keypress", "#tbl-ProjectSectionDeliveryTemprory tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionDeliveryTemprory(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionDeliveryTemprory").on("change keyup", "#tbl-ProjectSectionDeliveryTemprory tbody tr:first select", function (e) {
        LoadDataProjectSectionDeliveryTemprory(1);
    });


    $("#FormContainer-ProjectSectionDeliveryTemprory").on("click", "#btnShowProjectSectionDeliveryTemproryFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionDeliveryTemprory/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionDeliveryTemprory').val('');
    $('.ProjectSectionDeliveryTemproryFiles').each(function (i, row) {
        $('#images-ProjectSectionDeliveryTemprory').val($('#images-ProjectSectionDeliveryTemprory').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionDeliveryTemprory-fileupload').fileupload();

    $('#ProjectSectionDeliveryTemprory-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionDeliveryTemproryFiles($('#ProjectSectionDeliveryTemproryId').val());
}

function DataRefreshProjectSectionDeliveryTemprory(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionDeliveryTemprory.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionDeliveryTemprory').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryTemprory/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionDeliveryTemprory tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].Defect.Title + "</td>");
            tr.append("<td data-th='تاریخ تشکیل جلسه'>" + json[i].ProjectSectionDeliveryTemprory.ConveneDate + "</td>");
            tr.append("<td data-th='شماره تشکیل جلسه'>" + json[i].ProjectSectionDeliveryTemprory.ConveneNumber + "</td>");
            tr.append("<td data-th='ریز نواقص'>" + json[i].ProjectSectionDeliveryTemprory.DefectDetails + "</td>");
            tr.append("<td data-th='تاریخ برگزاری جلسه'>" + json[i].ProjectSectionDeliveryTemprory.MeetingDate + "</td>");
            tr.append("<td data-th='مبلغ پرداختی'>" + Seprator(json[i].ProjectSectionDeliveryTemprory.PaidPrice) + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Update", ProjectSectionDeliveryTemproryPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Delete", ProjectSectionDeliveryTemproryPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionDeliveryTemprory(" + json[i].ProjectSectionDeliveryTemprory.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionDeliveryTemprory'," + json[i].ProjectSectionDeliveryTemprory.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Update", ProjectSectionDeliveryTemproryPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionDeliveryTemprory(" + json[i].ProjectSectionDeliveryTemprory.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Delete", ProjectSectionDeliveryTemproryPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionDeliveryTemprory'," + json[i].ProjectSectionDeliveryTemprory.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionDeliveryTemprory tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Update", ProjectSectionDeliveryTemproryPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Delete", ProjectSectionDeliveryTemproryPermissions) == -1) {
            $('#tbl-ProjectSectionDeliveryTemprory th:last').remove();
            $('#tbl-ProjectSectionDeliveryTemprory tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionDeliveryTemprory tfoot td').attr('colspan', $('#tbl-ProjectSectionDeliveryTemprory tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionDeliveryTemprory(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_List", ProjectSectionDeliveryTemproryPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionDeliveryTemprory(pageRecord, $('#tbl-ProjectSectionDeliveryTemprory .page-size').val(), $('#sort-ProjectSectionDeliveryTemprory').val());

        Pager(pageRecord, $('#tbl-ProjectSectionDeliveryTemprory .page-size').val(), "ProjectSectionDeliveryTemprory", totalRecords);
    }
}

function ClearFormProjectSectionDeliveryTemprory() {
    
    $('#frm-ProjectSectionDeliveryTemprory input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionDeliveryTemproryFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryTemprory/_Create", ProjectSectionDeliveryTemproryPermissions) > -1) {
        $('#ProjectSectionDeliveryTemproryId').val("-1");
        $('#btnSaveProjectSectionDeliveryTemprory').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionDeliveryTemprory').validate();
    $('#frm-ProjectSectionDeliveryTemprory').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionDeliveryTemprory(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionDeliveryTemprory/_Update', { Id: id }, '#FormContainer-ProjectSectionDeliveryTemprory', 'UpdateProjectSectionDeliveryTemproryCallback();');
}



function DeleteProjectSectionDeliveryTemprory(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryTemprory/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionDeliveryTemprory tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionDeliveryTemprory .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionDeliveryTemprory .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionDeliveryTemprory .page-record').val() - 1;
        }

        LoadDataProjectSectionDeliveryTemprory(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionDeliveryTemprory() {
    if ($('#ProjectSectionDeliveryTemproryId').val() != '-1')
        $('#btnSaveProjectSectionDeliveryTemprory').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionDeliveryTemproryFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryTemprory/GetProjectSectionDeliveryTemproryFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionDeliveryTemprory-fileupload').fileupload('option', 'done').call($('#ProjectSectionDeliveryTemprory-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionDeliveryTemproryFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryTemprory/ReloadFiles', {}, function (data, textStatus, xhr) { });
}