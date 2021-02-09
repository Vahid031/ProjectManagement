var ProjectSectionPaidPriceListPermissions;


$(function () {

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionPaidPriceList/_List', '', '#FormList-ProjectSectionPaidPriceList', 'ListProjectSectionPaidPriceListCallback()');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionPaidPriceList/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionPaidPriceCreate');

    ProjectSectionPaidPriceListPermissions = $('#permission-ProjectSectionPaidPriceList').val().split(',');


    //if ($.inArray("/ProjectDevision/ProjectSectionPaidPriceList/_List", ProjectSectionPaidPriceListPermissions) > -1) {
        
    //}

    EventHandlerProjectSectionPaidPriceList();
});


function IndexProjectSectionPaidPriceListCallback(Id) {

   
}


function CreateProjectSectionPaidPriceListCallback(Id) {
    $('#ProjectSectionPaidPriceList_ProjectSectionStatementConfirmId').val(Id);
    CheckValueProjectSectionPaidPriceList();
    HandleValidation();
}

function UpdateProjectSectionPaidPriceListCallback() {
    CreateProjectSectionPaidPriceListCallback();
}

function ListProjectSectionPaidPriceListCallback() {
    Pager(1, 5, "ProjectSectionPaidPriceList", DataRefreshProjectSectionPaidPriceList(1, 5, $("#sort-ProjectSectionPaidPriceList").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionPaidPriceList() {
    $("#FormContainer-ProjectSectionPaidPriceList").on("submit", "#frm-ProjectSectionPaidPriceList", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionPaidPriceList/_Create', 'Files=' + $('#images-ProjectSectionPaidPriceList').val() + "&" + 'ProjectSectionPaidPriceList.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionPaidPriceList').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionPaidPriceList();

            if ($('#tbl-ProjectSectionPaidPriceList .page-record').val() == null)
                LoadDataProjectSectionPaidPriceList(1);
            else
                LoadDataProjectSectionPaidPriceList($('#tbl-ProjectSectionPaidPriceList .page-record').val());

            LoadDataProjectSectionFinance($('#tbl-ProjectSectionFinance .page-record').val());
            SelectProjectSectionFinance();

            if ($.inArray("/ProjectDevision/ProjectSectionPaidPriceList/_Create", ProjectSectionPaidPriceListPermissions) == -1) {
                $('#FormContainer-ProjectSectionPaidPriceList').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionPaidPriceList").on("click", "#frm-ProjectSectionPaidPriceList .btnNew", function () {
        ClearFormProjectSectionPaidPriceList();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionPaidPriceList").on("keypress", "#tbl-ProjectSectionPaidPriceList tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionPaidPriceList(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionPaidPriceList").on("change keyup", "#tbl-ProjectSectionPaidPriceList tbody tr:first select", function (e) {
        LoadDataProjectSectionPaidPriceList(1);
    });


    $("#FormContainer-ProjectSectionPaidPriceList").on("click", "#btnShowProjectSectionPaidPriceListFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionPaidPriceList/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-ProjectSectionPaidPriceList").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionPaidPriceList_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionPaidPriceList_StatementNumberId option").remove();
        }
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionPaidPriceList').val('');
    $('.ProjectSectionPaidPriceListFiles').each(function (i, row) {
        $('#images-ProjectSectionPaidPriceList').val($('#images-ProjectSectionPaidPriceList').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionPaidPriceList-fileupload').fileupload();

    $('#ProjectSectionPaidPriceList-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionPaidPriceListFiles($('#ProjectSectionPaidPriceListId').val());
}

function DataRefreshProjectSectionPaidPriceList(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    var jsonParams = 'ProjectSectionPaidPriceList.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionPaidPriceList').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPriceList/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionPaidPriceList tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);
        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='مبلغ حواله'>" + Seprator(json[i].ProjectSectionDraft.DraftPrice) + "</td>");
            tr.append("<td data-th='تاریخ حواله'>" + json[i].ProjectSectionDraft.DraftDate_ + "</td>");
            tr.append("<td data-th='انتخاب'><a onmousedown = 'PopupFormHtml(\"پرداخت صورت وضعیت\", \"/ProjectDevision/ProjectSectionPaidPrice/_Index\", \"IndexProjectSectionPaidPriceCallback(" + json[i].ProjectSectionDraft.Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='محاسبه'></a></td>");

            $('#tbl-ProjectSectionPaidPriceList tbody').append(tr);
        }


        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionPaidPriceList(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionPaidPriceList/_List", ProjectSectionPaidPriceListPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionPaidPriceList(pageRecord, $('#tbl-ProjectSectionPaidPriceList .page-size').val(), $('#sort-ProjectSectionPaidPriceList').val());

        Pager(pageRecord, $('#tbl-ProjectSectionPaidPriceList .page-size').val(), "ProjectSectionPaidPriceList", totalRecords);
    }
}

function ClearFormProjectSectionPaidPriceList() {

    $('#frm-ProjectSectionPaidPriceList input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionPaidPriceListFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionPaidPriceList/_Create", ProjectSectionPaidPriceListPermissions) > -1) {
        $('#ProjectSectionPaidPriceListId').val("-1");
        $('#btnSaveProjectSectionPaidPriceList').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-ProjectSectionPaidPriceList').validate();
    $('#frm-ProjectSectionPaidPriceList').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionPaidPriceList(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionPaidPriceList/_Update', { Id: id }, '#FormContainer-ProjectSectionPaidPriceList', 'UpdateProjectSectionPaidPriceListCallback();');
}



function DeleteProjectSectionPaidPriceList(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPriceList/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionPaidPriceList tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionPaidPriceList .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionPaidPriceList .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionPaidPriceList .page-record').val() - 1;
        }

        LoadDataProjectSectionPaidPriceList(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionPaidPriceList() {
    if ($('#ProjectSectionPaidPriceListId').val() != '-1')
        $('#btnSaveProjectSectionPaidPriceList').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionPaidPriceListFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPriceList/GetProjectSectionPaidPriceListFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionPaidPriceList-fileupload').fileupload('option', 'done').call($('#ProjectSectionPaidPriceList-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionPaidPriceListFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPriceList/ReloadFiles', {}, function (data, textStatus, xhr) { });
}


