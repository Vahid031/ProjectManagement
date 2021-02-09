var ProjectSectionDepositeLiberalizationPermissions;


$(function () {
    ProjectSectionDepositeLiberalizationPermissions = $('#permission-ProjectSectionDepositeLiberalization').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_List", ProjectSectionDepositeLiberalizationPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDepositeLiberalization/_List', '', '#FormList-ProjectSectionDepositeLiberalization', 'ListProjectSectionDepositeLiberalizationCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Create", ProjectSectionDepositeLiberalizationPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDepositeLiberalization/_Create', '', '#FormContainer-ProjectSectionDepositeLiberalization', 'CreateProjectSectionDepositeLiberalizationCallback();');
    }

    EventHandlerProjectSectionDepositeLiberalization();

});

function CreateProjectSectionDepositeLiberalizationCallback() {
    CheckValueProjectSectionDepositeLiberalization();
    HandleValidation();

    DatePic('#ProjectSectionDepositeLiberalization_PaidDate');
}

function UpdateProjectSectionDepositeLiberalizationCallback() {
    CreateProjectSectionDepositeLiberalizationCallback();
}

function ListProjectSectionDepositeLiberalizationCallback() {

    Pager(1, 5, "ProjectSectionDepositeLiberalization", DataRefreshProjectSectionDepositeLiberalization(1, 5, $("#sort-ProjectSectionDepositeLiberalization").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionDepositeLiberalization() {
    $("#FormContainer-ProjectSectionDepositeLiberalization").on("submit", "#frm-ProjectSectionDepositeLiberalization", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionDepositeLiberalization/_Create', 'Files=' + $('#images-ProjectSectionDepositeLiberalization').val() + "&" + 'ProjectSectionDepositeLiberalization.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionDepositeLiberalization').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionDepositeLiberalization();

            if ($('#tbl-ProjectSectionDepositeLiberalization .page-record').val() == null)
                LoadDataProjectSectionDepositeLiberalization(1);
            else
                LoadDataProjectSectionDepositeLiberalization($('#tbl-ProjectSectionDepositeLiberalization .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Create", ProjectSectionDepositeLiberalizationPermissions) == -1) {
                $('#FormContainer-ProjectSectionDepositeLiberalization').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionDepositeLiberalization").on("click", "#frm-ProjectSectionDepositeLiberalization .btnNew", function () {
        ClearFormProjectSectionDepositeLiberalization();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionDepositeLiberalization").on("keypress", "#tbl-ProjectSectionDepositeLiberalization tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionDepositeLiberalization(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionDepositeLiberalization").on("change keyup", "#tbl-ProjectSectionDepositeLiberalization tbody tr:first select", function (e) {
        LoadDataProjectSectionDepositeLiberalization(1);
    });


    $("#FormContainer-ProjectSectionDepositeLiberalization").on("click", "#btnShowProjectSectionDepositeLiberalizationFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionDepositeLiberalization/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionDepositeLiberalization').val('');
    $('.ProjectSectionDepositeLiberalizationFiles').each(function (i, row) {
        $('#images-ProjectSectionDepositeLiberalization').val($('#images-ProjectSectionDepositeLiberalization').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionDepositeLiberalization-fileupload').fileupload();

    $('#ProjectSectionDepositeLiberalization-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionDepositeLiberalizationFiles($('#ProjectSectionDepositeLiberalizationId').val());
}

function DataRefreshProjectSectionDepositeLiberalization(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionDepositeLiberalization.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionDepositeLiberalization').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionDepositeLiberalization/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionDepositeLiberalization tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ پرداخت'>" + json[i].ProjectSectionDepositeLiberalization.PaidDate + "</td>");
            tr.append("<td data-th='مبلغ (تحویل موقت)'>" + Seprator(json[i].ProjectSectionDepositeLiberalization.TemproryPrice) + "</td>");
            tr.append("<td data-th='مبلغ (تحویل قطعی)'>" + Seprator(json[i].ProjectSectionDepositeLiberalization.FixedPrice) + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionDepositeLiberalization.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Update", ProjectSectionDepositeLiberalizationPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Delete", ProjectSectionDepositeLiberalizationPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionDepositeLiberalization(" + json[i].ProjectSectionDepositeLiberalization.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionDepositeLiberalization'," + json[i].ProjectSectionDepositeLiberalization.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Update", ProjectSectionDepositeLiberalizationPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionDepositeLiberalization(" + json[i].ProjectSectionDepositeLiberalization.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Delete", ProjectSectionDepositeLiberalizationPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionDepositeLiberalization'," + json[i].ProjectSectionDepositeLiberalization.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionDepositeLiberalization tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Update", ProjectSectionDepositeLiberalizationPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Delete", ProjectSectionDepositeLiberalizationPermissions) == -1) {
            $('#tbl-ProjectSectionDepositeLiberalization th:last').remove();
            $('#tbl-ProjectSectionDepositeLiberalization tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionDepositeLiberalization tfoot td').attr('colspan', $('#tbl-ProjectSectionDepositeLiberalization tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionDepositeLiberalization(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_List", ProjectSectionDepositeLiberalizationPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionDepositeLiberalization(pageRecord, $('#tbl-ProjectSectionDepositeLiberalization .page-size').val(), $('#sort-ProjectSectionDepositeLiberalization').val());

        Pager(pageRecord, $('#tbl-ProjectSectionDepositeLiberalization .page-size').val(), "ProjectSectionDepositeLiberalization", totalRecords);
    }
}

function ClearFormProjectSectionDepositeLiberalization() {
    
    $('#frm-ProjectSectionDepositeLiberalization input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionDepositeLiberalizationFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionDepositeLiberalization/_Create", ProjectSectionDepositeLiberalizationPermissions) > -1) {
        $('#ProjectSectionDepositeLiberalizationId').val("-1");
        $('#btnSaveProjectSectionDepositeLiberalization').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionDepositeLiberalization').validate();
    $('#frm-ProjectSectionDepositeLiberalization').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionDepositeLiberalization(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionDepositeLiberalization/_Update', { Id: id }, '#FormContainer-ProjectSectionDepositeLiberalization', 'UpdateProjectSectionDepositeLiberalizationCallback();');
}



function DeleteProjectSectionDepositeLiberalization(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDepositeLiberalization/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionDepositeLiberalization tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionDepositeLiberalization .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionDepositeLiberalization .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionDepositeLiberalization .page-record').val() - 1;
        }

        LoadDataProjectSectionDepositeLiberalization(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionDepositeLiberalization() {
    if ($('#ProjectSectionDepositeLiberalizationId').val() != '-1')
        $('#btnSaveProjectSectionDepositeLiberalization').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionDepositeLiberalizationFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDepositeLiberalization/GetProjectSectionDepositeLiberalizationFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionDepositeLiberalization-fileupload').fileupload('option', 'done').call($('#ProjectSectionDepositeLiberalization-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionDepositeLiberalizationFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionDepositeLiberalization/ReloadFiles', {}, function (data, textStatus, xhr) { });
}