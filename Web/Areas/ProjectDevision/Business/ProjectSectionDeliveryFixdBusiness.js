var ProjectSectionDeliveryFixdPermissions;


$(function () {
    ProjectSectionDeliveryFixdPermissions = $('#permission-ProjectSectionDeliveryFixd').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_List", ProjectSectionDeliveryFixdPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryFixd/_List', '', '#FormList-ProjectSectionDeliveryFixd', 'ListProjectSectionDeliveryFixdCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Create", ProjectSectionDeliveryFixdPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryFixd/_Create', '', '#FormContainer-ProjectSectionDeliveryFixd', 'CreateProjectSectionDeliveryFixdCallback();');
    }

    EventHandlerProjectSectionDeliveryFixd();

});

function CreateProjectSectionDeliveryFixdCallback() {
    CheckValueProjectSectionDeliveryFixd();
    HandleValidation();

    DatePic('#ProjectSectionDeliveryFixd_ConveneDate');
    DatePic('#ProjectSectionDeliveryFixd_RequestDate');
    DatePic('#ProjectSectionDeliveryFixd_MeetingDate');
}

function UpdateProjectSectionDeliveryFixdCallback() {
    CreateProjectSectionDeliveryFixdCallback();
}

function ListProjectSectionDeliveryFixdCallback() {

    Pager(1, 5, "ProjectSectionDeliveryFixd", DataRefreshProjectSectionDeliveryFixd(1, 5, $("#sort-ProjectSectionDeliveryFixd").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionDeliveryFixd() {
    $("#FormContainer-ProjectSectionDeliveryFixd").on("submit", "#frm-ProjectSectionDeliveryFixd", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        if ($('#images-ProjectSectionDeliveryFixd').val() == '') {
            Messages('warning', 'پیوست الزامی میباشد');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryFixd/_Create', 'Files=' + $('#images-ProjectSectionDeliveryFixd').val() + "&" + 'ProjectSectionDeliveryFixd.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionDeliveryFixd').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionDeliveryFixd();

            if ($('#tbl-ProjectSectionDeliveryFixd .page-record').val() == null)
                LoadDataProjectSectionDeliveryFixd(1);
            else
                LoadDataProjectSectionDeliveryFixd($('#tbl-ProjectSectionDeliveryFixd .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Create", ProjectSectionDeliveryFixdPermissions) == -1) {
                $('#FormContainer-ProjectSectionDeliveryFixd').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionDeliveryFixd").on("click", "#frm-ProjectSectionDeliveryFixd .btnNew", function () {
        ClearFormProjectSectionDeliveryFixd();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionDeliveryFixd").on("keypress", "#tbl-ProjectSectionDeliveryFixd tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionDeliveryFixd(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionDeliveryFixd").on("change keyup", "#tbl-ProjectSectionDeliveryFixd tbody tr:first select", function (e) {
        LoadDataProjectSectionDeliveryFixd(1);
    });


    $("#FormContainer-ProjectSectionDeliveryFixd").on("click", "#btnShowProjectSectionDeliveryFixdFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionDeliveryFixd/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionDeliveryFixd').val('');
    $('.ProjectSectionDeliveryFixdFiles').each(function (i, row) {
        $('#images-ProjectSectionDeliveryFixd').val($('#images-ProjectSectionDeliveryFixd').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionDeliveryFixd-fileupload').fileupload();

    $('#ProjectSectionDeliveryFixd-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionDeliveryFixdFiles($('#ProjectSectionDeliveryFixdId').val());
}

function DataRefreshProjectSectionDeliveryFixd(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionDeliveryFixd.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionDeliveryFixd').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryFixd/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionDeliveryFixd tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].Defect.Title + "</td>");
            tr.append("<td data-th='تاریخ تشکیل جلسه'>" + json[i].ProjectSectionDeliveryFixd.ConveneDate + "</td>");
            tr.append("<td data-th='شماره تشکیل جلسه'>" + json[i].ProjectSectionDeliveryFixd.ConveneNumber + "</td>");
            tr.append("<td data-th='ریز نواقص'>" + json[i].ProjectSectionDeliveryFixd.DefectDetails + "</td>");
            tr.append("<td data-th='تاریخ برگزاری جلسه'>" + json[i].ProjectSectionDeliveryFixd.MeetingDate + "</td>");
            
            if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Update", ProjectSectionDeliveryFixdPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Delete", ProjectSectionDeliveryFixdPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionDeliveryFixd(" + json[i].ProjectSectionDeliveryFixd.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionDeliveryFixd'," + json[i].ProjectSectionDeliveryFixd.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Update", ProjectSectionDeliveryFixdPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionDeliveryFixd(" + json[i].ProjectSectionDeliveryFixd.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Delete", ProjectSectionDeliveryFixdPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionDeliveryFixd'," + json[i].ProjectSectionDeliveryFixd.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionDeliveryFixd tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Update", ProjectSectionDeliveryFixdPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Delete", ProjectSectionDeliveryFixdPermissions) == -1) {
            $('#tbl-ProjectSectionDeliveryFixd th:last').remove();
            $('#tbl-ProjectSectionDeliveryFixd tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionDeliveryFixd tfoot td').attr('colspan', $('#tbl-ProjectSectionDeliveryFixd tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionDeliveryFixd(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_List", ProjectSectionDeliveryFixdPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionDeliveryFixd(pageRecord, $('#tbl-ProjectSectionDeliveryFixd .page-size').val(), $('#sort-ProjectSectionDeliveryFixd').val());

        Pager(pageRecord, $('#tbl-ProjectSectionDeliveryFixd .page-size').val(), "ProjectSectionDeliveryFixd", totalRecords);
    }
}

function ClearFormProjectSectionDeliveryFixd() {
    
    $('#frm-ProjectSectionDeliveryFixd input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionDeliveryFixdFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionDeliveryFixd/_Create", ProjectSectionDeliveryFixdPermissions) > -1) {
        $('#ProjectSectionDeliveryFixdId').val("-1");
        $('#btnSaveProjectSectionDeliveryFixd').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionDeliveryFixd').validate();
    $('#frm-ProjectSectionDeliveryFixd').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionDeliveryFixd(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionDeliveryFixd/_Update', { Id: id }, '#FormContainer-ProjectSectionDeliveryFixd', 'UpdateProjectSectionDeliveryFixdCallback();');
}



function DeleteProjectSectionDeliveryFixd(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryFixd/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionDeliveryFixd tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionDeliveryFixd .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionDeliveryFixd .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionDeliveryFixd .page-record').val() - 1;
        }

        LoadDataProjectSectionDeliveryFixd(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionDeliveryFixd() {
    if ($('#ProjectSectionDeliveryFixdId').val() != '-1')
        $('#btnSaveProjectSectionDeliveryFixd').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionDeliveryFixdFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryFixd/GetProjectSectionDeliveryFixdFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionDeliveryFixd-fileupload').fileupload('option', 'done').call($('#ProjectSectionDeliveryFixd-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionDeliveryFixdFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionDeliveryFixd/ReloadFiles', {}, function (data, textStatus, xhr) { });
}