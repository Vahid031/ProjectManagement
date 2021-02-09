var ProjectSectionComplementarityPricePermissions;


$(function () {
    ProjectSectionComplementarityPricePermissions = $('#permission-ProjectSectionComplementarityPrice').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_List", ProjectSectionComplementarityPricePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityPrice/_List', '', '#FormList-ProjectSectionComplementarityPrice', 'ListProjectSectionComplementarityPriceCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Create", ProjectSectionComplementarityPricePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityPrice/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionComplementarityPrice', 'CreateProjectSectionComplementarityPriceCallback();');
    }

    EventHandlerProjectSectionComplementarityPrice();
});

function CreateProjectSectionComplementarityPriceCallback() {
    CheckValueProjectSectionComplementarityPrice();
    HandleValidation();

    DatePic('#ProjectSectionComplementarityPrice_Date');
}

function UpdateProjectSectionComplementarityPriceCallback() {
    CreateProjectSectionComplementarityPriceCallback();
}

function ListProjectSectionComplementarityPriceCallback() {
    Pager(1, 5, "ProjectSectionComplementarityPrice", DataRefreshProjectSectionComplementarityPrice(1, 5, $("#sort-ProjectSectionComplementarityPrice").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionComplementarityPrice() {
    $("#FormContainer-ProjectSectionComplementarityPrice").on("submit", "#frm-ProjectSectionComplementarityPrice", function (e) {
        e.preventDefault();
        
        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#Price').val($('#Price').val().replace(/\,/g, ''));

        if ($('#images-ProjectSectionComplementarityPrice').val() == '') {
            Messages('warning', 'پیوست الزامی میباشد');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityPrice/_Create', 'Files=' + $('#images-ProjectSectionComplementarityPrice').val() + "&" + 'ProjectSectionComplementarityPrice.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionComplementarityPrice').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionComplementarityPrice();

            if ($('#tbl-ProjectSectionComplementarityPrice .page-record').val() == null)
                LoadDataProjectSectionComplementarityPrice(1);
            else
                LoadDataProjectSectionComplementarityPrice($('#tbl-ProjectSectionComplementarityPrice .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Create", ProjectSectionComplementarityPricePermissions) == -1) {
                $('#FormContainer-ProjectSectionComplementarityPrice').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionComplementarityPrice").on("click", "#frm-ProjectSectionComplementarityPrice .btnNew", function () {
        ClearFormProjectSectionComplementarityPrice();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionComplementarityPrice").on("keypress", "#tbl-ProjectSectionComplementarityPrice tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionComplementarityPrice(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionComplementarityPrice").on("change keyup", "#tbl-ProjectSectionComplementarityPrice tbody tr:first select", function (e) {
        LoadDataProjectSectionComplementarityPrice(1);
    });


    $("#FormContainer-ProjectSectionComplementarityPrice").on("click", "#btnShowProjectSectionComplementarityPriceFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionComplementarityPrice/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionComplementarityPrice').val('');
    $('.ProjectSectionComplementarityPriceFiles').each(function (i, row) {
        $('#images-ProjectSectionComplementarityPrice').val($('#images-ProjectSectionComplementarityPrice').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionComplementarityPrice-fileupload').fileupload();

    $('#ProjectSectionComplementarityPrice-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionComplementarityPriceFiles($('#ProjectSectionComplementarityPriceId').val());
}

function DataRefreshProjectSectionComplementarityPrice(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionComplementarityPrice.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionComplementarityPrice').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityPrice/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionComplementarityPrice tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='تاریخ'>" + json[i].ProjectSectionComplementarityPrice.Date + "</td>");
            tr.append("<td data-th='درصد کاهش'>" + json[i].ProjectSectionComplementarityPrice.DecreasePercent + "</td>");
            tr.append("<td data-th='درصد افزایش'>" + json[i].ProjectSectionComplementarityPrice.IncreasePercent + "</td>");
            tr.append("<td data-th='مبلغ جدید'>" + Seprator(json[i].ProjectSectionComplementarityPrice.NewPrice) + "</td>");
            tr.append("<td data-th='شماره'>" + json[i].ProjectSectionComplementarityPrice.Number + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Update", ProjectSectionComplementarityPricePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Delete", ProjectSectionComplementarityPricePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionComplementarityPrice(" + json[i].ProjectSectionComplementarityPrice.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionComplementarityPrice'," + json[i].ProjectSectionComplementarityPrice.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Update", ProjectSectionComplementarityPricePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionComplementarityPrice(" + json[i].ProjectSectionComplementarityPrice.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Delete", ProjectSectionComplementarityPricePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionComplementarityPrice'," + json[i].ProjectSectionComplementarityPrice.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionComplementarityPrice tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Update", ProjectSectionComplementarityPricePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Delete", ProjectSectionComplementarityPricePermissions) == -1) {
            $('#tbl-ProjectSectionComplementarityPrice th:last').remove();
            $('#tbl-ProjectSectionComplementarityPrice tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionComplementarityPrice tfoot td').attr('colspan', $('#tbl-ProjectSectionComplementarityPrice tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionComplementarityPrice(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_List", ProjectSectionComplementarityPricePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionComplementarityPrice(pageRecord, $('#tbl-ProjectSectionComplementarityPrice .page-size').val(), $('#sort-ProjectSectionComplementarityPrice').val());

        Pager(pageRecord, $('#tbl-ProjectSectionComplementarityPrice .page-size').val(), "ProjectSectionComplementarityPrice", totalRecords);
    }
}

function ClearFormProjectSectionComplementarityPrice() {
    
    $('#frm-ProjectSectionComplementarityPrice input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionComplementarityPriceFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionComplementarityPrice/_Create", ProjectSectionComplementarityPricePermissions) > -1) {
        $('#ProjectSectionComplementarityPriceId').val("-1");
        $('#btnSaveProjectSectionComplementarityPrice').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionComplementarityPrice').validate();
    $('#frm-ProjectSectionComplementarityPrice').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionComplementarityPrice(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionComplementarityPrice/_Update', { Id: id }, '#FormContainer-ProjectSectionComplementarityPrice', 'UpdateProjectSectionComplementarityPriceCallback();');
}



function DeleteProjectSectionComplementarityPrice(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityPrice/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionComplementarityPrice tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionComplementarityPrice .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionComplementarityPrice .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionComplementarityPrice .page-record').val() - 1;
        }

        LoadDataProjectSectionComplementarityPrice(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionComplementarityPrice() {
    if ($('#ProjectSectionComplementarityPriceId').val() != '-1')
        $('#btnSaveProjectSectionComplementarityPrice').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionComplementarityPriceFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityPrice/GetProjectSectionComplementarityPriceFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionComplementarityPrice-fileupload').fileupload('option', 'done').call($('#ProjectSectionComplementarityPrice-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionComplementarityPriceFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionComplementarityPrice/ReloadFiles', {}, function (data, textStatus, xhr) { });
}