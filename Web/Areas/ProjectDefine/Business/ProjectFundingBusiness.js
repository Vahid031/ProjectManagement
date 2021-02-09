var ProjectFundingPermissions;


$(function () {
    ProjectFundingPermissions = $('#permission-ProjectFunding').val().split(',');

    if ($.inArray("/ProjectDefine/ProjectFunding/_List", ProjectFundingPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectFunding/_List', '', '#FormList-ProjectFunding', 'ListProjectFundingCallback();');
    }

    if ($.inArray("/ProjectDefine/ProjectFunding/_Create", ProjectFundingPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectFunding/_Create', 'projectId=' + SelectedProject, '#FormContainer-ProjectFunding', 'CreateProjectFundingCallback();');
    }


    

    EventHandlerProjectFunding();
});

function CreateProjectFundingCallback() {
    CheckValueProjectFunding();
    HandleValidation();
}

function UpdateProjectFundingCallback() {
    CreateProjectFundingCallback();
}

function ListProjectFundingCallback() {
    Pager(1, 5, "ProjectFunding", DataRefreshProjectFunding(1, 5, $("#sort-ProjectFunding").val()));
    
    HandleValidation();

    SortArrow();
}

// این کد برای جدا سازی سه رقم سه قم اعداد هنگام لود صفحه در زمان آپدیت میباشد
function SepreateProjectFunding() {
    SepratePrice('#ProjectFunding_Price');
}

function EventHandlerProjectFunding() {
    $("#FormContainer-ProjectFunding").on("submit", "#frm-ProjectFunding", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectFunding_Price').val($('#ProjectFunding_Price').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDefine/ProjectFunding/_Create', 'Files=' + $('#images-ProjectFunding').val() + "&" + 'ProjectFunding.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectFunding').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectFunding();

            if ($('#tbl-ProjectFunding .page-record').val() == null)
                LoadDataProjectFunding(1);
            else
                LoadDataProjectFunding($('#tbl-ProjectFunding .page-record').val());

            // تغییر رنگ فونت جدول بالایی
            LoadDataProjectAgreement($('#tbl-ProjectAgreement .page-record').val());
            SelectProject();


            if ($.inArray("/ProjectDefine/ProjectFunding/_Create", ProjectFundingPermissions) == -1) {
                $('#FormContainer-ProjectFunding').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectFunding").on("click", "#frm-ProjectFunding .btnNew", function () {
        ClearFormProjectFunding();

        $('#Alert,#AlertDown').slideUp(300);
    });

    


    $("#FormList-ProjectFunding").on("keypress", "#tbl-ProjectFunding tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectFunding(1);
            return false;
        }
    });

    $("#FormList-ProjectFunding").on("change keyup", "#tbl-ProjectFunding tbody tr:first select", function (e) {
        LoadDataProjectFunding(1);
    });

    $("#FormContainer-ProjectFunding").on("change keyup", "#ResourceId", function () {
        if ($(this).val() != '') {
            GetResourceTypesByResourceId("#ProjectFunding_ResourceTypeId", $(this).val());
        }
        else {
            $("#ProjectFunding_ResourceTypeId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });


    $("#FormContainer-ProjectFunding").on("click", "#btnShowProjectFundingFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDefine/ProjectFunding/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectFunding').val('');
    $('.ProjectFundingFiles').each(function (i, row) {
        $('#images-ProjectFunding').val($('#images-ProjectFunding').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectFunding-fileupload').fileupload();

    $('#ProjectFunding-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectFundingFiles($('#ProjectFundingId').val());
}


function DataRefreshProjectFunding(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectFunding.ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectFunding').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProjectFunding/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectFunding tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان سال مالی'>" + json[i].FinantialYear.Title + "</td>");
            tr.append("<td data-th='نوع منبع اعتبار'>" + json[i].Resource.Title + "</td>");
            tr.append("<td data-th='منبع اعتبار'>" + json[i].ResourceType.Title + "</td>");
            tr.append("<td data-th='مبلغ'>" + Seprator(json[i].ProjectFunding.Price) + "</td>");

            if ($.inArray("/ProjectDefine/ProjectFunding/_Update", ProjectFundingPermissions) > -1 && $.inArray("/ProjectDefine/ProjectFunding/_Delete", ProjectFundingPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectFunding(" + json[i].ProjectFunding.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectFunding'," + json[i].ProjectFunding.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectFunding/_Update", ProjectFundingPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectFunding(" + json[i].ProjectFunding.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectFunding/_Delete", ProjectFundingPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectFunding'," + json[i].ProjectFunding.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectFunding tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProjectFunding/_Update", ProjectFundingPermissions) == -1 && $.inArray("/ProjectDefine/ProjectFunding/_Delete", ProjectFundingPermissions) == -1) {
            $('#tbl-ProjectFunding th:last').remove();
            $('#tbl-ProjectFunding tbody tr:first td:last').remove();
            $('#tbl-ProjectFunding tfoot td').attr('colspan', $('#tbl-ProjectFunding tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectFunding(pageRecord) {
    if ($.inArray("/ProjectDefine/ProjectFunding/_List", ProjectFundingPermissions) > -1) {
        var totalRecords = DataRefreshProjectFunding(pageRecord, $('#tbl-ProjectFunding .page-size').val(), $('#sort-ProjectFunding').val());

        Pager(pageRecord, $('#tbl-ProjectFunding .page-size').val(), "ProjectFunding", totalRecords);
    }
}

function ClearFormProjectFunding() {
    
    $('#frm-ProjectFunding input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectFundingFiles();

    if ($.inArray("/ProjectDefine/ProjectFunding/_Create", ProjectFundingPermissions) > -1) {
        $('#ProjectFundingId').val("-1");
        $('#btnSaveProjectFunding').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectFunding').validate();
    $('#frm-ProjectFunding').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectFunding(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProjectFunding/_Update', { Id: id }, '#FormContainer-ProjectFunding', 'UpdateProjectFundingCallback();SepreateProjectFunding();');
}



function DeleteProjectFunding(id) {
    Ajax('Post', '/ProjectDefine/ProjectFunding/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectFunding tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectFunding .page-record').val();
        }
        else {
            if ($('#tbl-ProjectFunding .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectFunding .page-record').val() - 1;
        }

        LoadDataProjectFunding(pageRecord);
    }, 'json');
}

function GetResourceTypesByResourceId(container, resourceId) {
    Ajax('Post', '/BaseInformation/ResourceType/GetResourceTypesByResourceId', 'resourceId=' + resourceId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}


function CheckValueProjectFunding() {
    if ($('#ProjectFundingId').val() != '-1')
        $('#btnSaveProjectFunding').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectFundingFiles(projectSectionId) {
    Ajax('Post', '/ProjectDefine/ProjectFunding/GetProjectFundingFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectFunding-fileupload').fileupload('option', 'done').call($('#ProjectFunding-fileupload'), $.Event('done'), { result: { files: files } });
    }, 'json');
}


function ReloadProjectFundingFiles() {
    Ajax('Post', '/ProjectDefine/ProjectFunding/ReloadFiles', {}, function (data, textStatus, xhr) { });
}

