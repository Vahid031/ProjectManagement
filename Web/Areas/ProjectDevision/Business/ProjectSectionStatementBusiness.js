var ProjectSectionStatementPermissions;


$(function () {
    ProjectSectionStatementPermissions = $('#permission-ProjectSectionStatement').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionStatement/_List", ProjectSectionStatementPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatement/_List', '', '#FormList-ProjectSectionStatement', 'ListProjectSectionStatementCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Create", ProjectSectionStatementPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatement/_Create', '', '#FormContainer-ProjectSectionStatement', 'CreateProjectSectionStatementCallback();');
    }

    EventHandlerProjectSectionStatement();

});

function CreateProjectSectionStatementCallback() {
    DatePic('#ProjectSectionStatement_ContractDate');

    CheckValueProjectSectionStatement();
    HandleValidation();
}

function UpdateProjectSectionStatementCallback() {
    CreateProjectSectionStatementCallback();
}

function ListProjectSectionStatementCallback() {

    Pager(1, 5, "ProjectSectionStatement", DataRefreshProjectSectionStatement(1, 5, $("#sort-ProjectSectionStatement").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionStatement() {
    $("#FormContainer-ProjectSectionStatement").on("submit", "#frm-ProjectSectionStatement", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectSectionStatement_ConfirmPrice').val($('#ProjectSectionStatement_ConfirmPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionStatement_ContractPrice').val($('#ProjectSectionStatement_ContractPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionStatement_AdvisorPrice').val($('#ProjectSectionStatement_AdvisorPrice').val().replace(/\,/g, ''));


        Ajax('Post', '/ProjectDevision/ProjectSectionStatement/_Create', 'Files=' + $('#images-ProjectSectionStatement').val() + "&" + 'ProjectSectionStatement.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionStatement').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionStatement();

            if ($('#tbl-ProjectSectionStatement .page-record').val() == null)
                LoadDataProjectSectionStatement(1);
            else
                LoadDataProjectSectionStatement($('#tbl-ProjectSectionStatement .page-record').val());

            LoadDataProjectSectionFinance($('#tbl-ProjectSectionFinance .page-record').val());
            SelectProjectSectionFinance();

            if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Create", ProjectSectionStatementPermissions) == -1) {
                $('#FormContainer-ProjectSectionStatement').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionStatement").on("click", "#frm-ProjectSectionStatement .btnNew", function () {
        ClearFormProjectSectionStatement();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionStatement").on("keypress", "#tbl-ProjectSectionStatement tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionStatement(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionStatement").on("change keyup", "#tbl-ProjectSectionStatement tbody tr:first select", function (e) {
        LoadDataProjectSectionStatement(1);
    });


    $("#FormContainer-ProjectSectionStatement").on("click", "#btnShowProjectSectionStatementFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionStatement/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-ProjectSectionStatement").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionStatement_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionStatement_StatementNumberId option").remove();
        }

        $('#ProjectSectionStatement_StatementNumberId').removeData('previousValue');
        $('#ProjectSectionStatement_StatementNumberId').valid();
    });

    $("#FormContainer-ProjectSectionStatement").on("change keyup", "#ProjectSectionStatement_StatementTypeId", function () {
        $('#ProjectSectionStatement_StatementNumberId').removeData('previousValue');
        $('#ProjectSectionStatement_StatementNumberId').valid();
    });

}

function YesFilePopupClick() {
    $('#images-ProjectSectionStatement').val('');
    $('.ProjectSectionStatementFiles').each(function (i, row) {
        $('#images-ProjectSectionStatement').val($('#images-ProjectSectionStatement').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionStatement-fileupload').fileupload();

    $('#ProjectSectionStatement-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionStatementFiles($('#ProjectSectionStatementId').val());
}

function DataRefreshProjectSectionStatement(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionStatement.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionStatement').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionStatement/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionStatement tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='نوع صورت وضعیت'>" + json[i].StatementType.Title + "</td>");
            tr.append("<td data-th='موقت/قطعی'>" + json[i].TempFixed.Title + "</td>");
            tr.append("<td data-th='شماره صورت وضعیت'>" + json[i].StatementNumber.Title + "</td>");
            tr.append("<td data-th='مبلغ اعلامی پیمانکار'>" + Seprator(json[i].ProjectSectionStatement.ContractPrice) + "</td>");
            tr.append("<td data-th='مبلغ پیشنهادی مشاور'>" + Seprator(json[i].ProjectSectionStatement.AdvisorPrice) + "</td>");
            tr.append("<td data-th='مبلغ پیشنهادی ناظر'>" + Seprator(json[i].ProjectSectionStatement.ConfirmPrice) + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Update", ProjectSectionStatementPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionStatement/_Delete", ProjectSectionStatementPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionStatement(" + json[i].ProjectSectionStatement.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionStatement'," + json[i].ProjectSectionStatement.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Update", ProjectSectionStatementPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionStatement(" + json[i].ProjectSectionStatement.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Delete", ProjectSectionStatementPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionStatement'," + json[i].ProjectSectionStatement.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionStatement tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Update", ProjectSectionStatementPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionStatement/_Delete", ProjectSectionStatementPermissions) == -1) {
            $('#tbl-ProjectSectionStatement th:last').remove();
            $('#tbl-ProjectSectionStatement tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionStatement tfoot td').attr('colspan', $('#tbl-ProjectSectionStatement tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionStatement(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionStatement/_List", ProjectSectionStatementPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionStatement(pageRecord, $('#tbl-ProjectSectionStatement .page-size').val(), $('#sort-ProjectSectionStatement').val());

        Pager(pageRecord, $('#tbl-ProjectSectionStatement .page-size').val(), "ProjectSectionStatement", totalRecords);
    }
}

function ClearFormProjectSectionStatement() {

    $('#frm-ProjectSectionStatement input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionStatementFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionStatement/_Create", ProjectSectionStatementPermissions) > -1) {
        $('#ProjectSectionStatementId').val("-1");
        $('#btnSaveProjectSectionStatement').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-ProjectSectionStatement').validate();
    $('#frm-ProjectSectionStatement').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionStatement(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionStatement/_Update', { Id: id }, '#FormContainer-ProjectSectionStatement', 'UpdateProjectSectionStatementCallback();');
}



function DeleteProjectSectionStatement(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatement/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionStatement tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionStatement .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionStatement .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionStatement .page-record').val() - 1;
        }

        LoadDataProjectSectionStatement(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionStatement() {
    if ($('#ProjectSectionStatementId').val() != '-1')
        $('#btnSaveProjectSectionStatement').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionStatementFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatement/GetProjectSectionStatementFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionStatement-fileupload').fileupload('option', 'done').call($('#ProjectSectionStatement-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionStatementFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatement/ReloadFiles', {}, function (data, textStatus, xhr) { });
}


function GetStatementNumbersByTempFixedId(container, tempFixedId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatement/GetStatementNumbersByTempFixedId', 'tempFixedId=' + tempFixedId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}