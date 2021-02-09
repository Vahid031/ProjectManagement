
$(function () {
    
});

function IndexProjectSectionProductCallback(Id) {
    if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Create", ProjectSectionPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionProduct/_Create/'+Id, '', '#FormContainer-ProjectSectionProduct', 'CreateProjectSectionProductCallback(' + Id + ');');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionProduct/_List", ProjectSectionPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionProduct/_List', '', '#FormList-ProjectSectionProduct', 'ListProjectSectionProductCallback();');
    }

    EventHandlerProjectSectionProduct();
}

function CreateProjectSectionProductCallback(Id) {
    $('#ProjectSectionProduct_ProjectSectionId').val(Id);
    
    CheckValue();

    HandleValidation();
}

function UpdateProjectSectionProductCallback() {
    CheckValue();

    HandleValidation();
}

function ListProjectSectionProductCallback() {
    Pager(1, 5, "ProjectSectionProduct", DataRefreshProjectSectionProduct(1, 5, $("#sort-ProjectSectionProduct").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerProjectSectionProduct() {
    $("#FormContainer-ProjectSectionProduct").on("submit", "#frm-ProjectSectionProduct", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionProduct/_Create', 'Files=' + $('#images-ProjectSectionProduct').val() + "&" + $('#frm-ProjectSectionProduct').serialize(), function (data, textStatus, xhr) {
            if (data.isAllocatePriceBiger == true)
                Messages(data.type, data.message);
            else
                Messages('danger', 'مجموع مبلغ پیش بینی بیشتر از مبلغ تخصیص شده است'); 

            ClearFormProjectSectionProduct();

            $('#sumProduct').val(data.product);
            $('#sumServices').val(data.services);
            $('#sumTotal').val(data.total);

            if ($('#tbl-ProjectSectionProduct .page-record').val() == null)
                LoadDataProjectSectionProduct(1);
            else
                LoadDataProjectSectionProduct($('#tbl-ProjectSectionProduct .page-record').val());

            LoadDataProjectSection($('#frm-tbl-ProjectSection select.page-record').val());

            if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Create", ProjectSectionPermissions) == -1) {
                $('#FormContainer-ProjectSectionProduct').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-ProjectSectionProduct").on("click", "#frm-ProjectSectionProduct .btnNew", function () {
        ClearFormProjectSectionProduct();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionProduct").on("keypress", "#tbl-ProjectSectionProduct tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionProduct(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionProduct").on("change keyup", "#tbl-ProjectSectionProduct tbody tr:first select", function (e) {
        LoadDataProjectSectionProduct(1);
    });

    $("#FormContainer-ProjectSectionProduct").on("click", "#btnShowProjectSectionProductFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionProduct/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();", "#cpb2");
    });
}


function YesFilePopupClick() {
    $('#images-ProjectSectionProduct').val('');
    $('.ProjectSectionProductFiles').each(function (i, row) {
        $('#images-ProjectSectionProduct').val($('#images-ProjectSectionProduct').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionProduct-fileupload').fileupload();

    $('#ProjectSectionProduct-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionProductFiles($('#Id').val());
}


function DataRefreshProjectSectionProduct(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionProduct.ProjectSectionId=' + $('#ProjectSectionProduct_ProjectSectionId').val() + "&" + $('#frm-tbl-ProjectSectionProduct').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionProduct/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionProduct tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ServiceProduct.Title)'>" + json[i].ServiceProduct.Title + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionProduct.ServiceProductContent)'>" + json[i].ProjectSectionProduct.ServiceProductContent + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionProduct.UsedAmount)'>" + json[i].ProjectSectionProduct.UsedAmount + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionProduct.EstimatesPrice)'>" + json[i].ProjectSectionProduct.EstimatesPrice + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Update", ProjectSectionPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionProduct/_Delete", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionProduct(" + json[i].ProjectSectionProduct.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionProduct'," + json[i].ProjectSectionProduct.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Update", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionProduct(" + json[i].ProjectSectionProduct.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Delete", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionProduct'," + json[i].ProjectSectionProduct.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionProduct tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Update", ProjectSectionPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionProduct/_Delete", ProjectSectionPermissions) == -1) {
            $('#tbl-ProjectSectionProduct th:last').remove();
            $('#tbl-ProjectSectionProduct tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionProduct tfoot td').attr('colspan', $('#tbl-ProjectSectionProduct tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionProduct(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionProduct/_List", ProjectSectionPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionProduct(pageRecord, $('#tbl-ProjectSectionProduct .page-size').val(), $('#sort-ProjectSectionProduct').val());

        Pager(pageRecord, $('#tbl-ProjectSectionProduct .page-size').val(), "ProjectSectionProduct", totalRecords);
    }
}

function ClearFormProjectSectionProduct() {
    
    $('#frm-ProjectSectionProduct input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionProductFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionProduct/_Create", ProjectSectionPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionProduct').validate();
    $('#frm-ProjectSectionProduct').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionProduct(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionProduct/_Update', { Id: id }, '#FormContainer-ProjectSectionProduct', 'UpdateProjectSectionProductCallback();');
}



function DeleteProjectSectionProduct(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionProduct/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;
        
        $('#sumProduct').val(data.product);
        $('#sumServices').val(data.services);
        $('#sumTotal').val(data.total);

        if ($('#tbl-ProjectSectionProduct tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionProduct .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionProduct .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionProduct .page-record').val() - 1;
        }

        LoadDataProjectSectionProduct(pageRecord);
        LoadDataProjectSection($('#frm-tbl-ProjectSection select.page-record').val());
    }, 'json');
}


function GetProjectSectionProductFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionProduct/GetProjectSectionProductFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionProduct-fileupload').fileupload('option', 'done').call($('#ProjectSectionProduct-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionProductFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionProduct/ReloadFiles', {}, function (data, textStatus, xhr) { });
}
