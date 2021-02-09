var ProjectSectionDraftListPermissions;
var FormType;

$(function () {
    
    FormType = $('#formType-ProjectSectionDraftList').val();
    ProjectSectionDraftListPermissions = $('#permission-ProjectSectionDraftList').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionDraftList/_List", ProjectSectionDraftListPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraftList/_List', '', '#FormList-ProjectSectionDraftList', 'ListProjectSectionDraftListCallback();');
    }

    EventHandlerProjectSectionDraftList();

});



function ListProjectSectionDraftListCallback() {

    Pager(1, 5, "ProjectSectionDraftList", DataRefreshProjectSectionDraftList(1, 5, $("#sort-ProjectSectionDraftList").val()));

    SortArrow();
}

function EventHandlerProjectSectionDraftList() {

    $("#FormContainer-ProjectSectionDraftList").on("click", "#frm-ProjectSectionDraftList .btnNew", function () {
        ClearFormProjectSectionDraftList();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionDraftList").on("keypress", "#tbl-ProjectSectionDraftList tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionDraftList(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionDraftList").on("change keyup", "#tbl-ProjectSectionDraftList tbody tr:first select", function (e) {
        LoadDataProjectSectionDraftList(1);
    });


    $("#FormContainer-ProjectSectionDraftList").on("click", "#btnShowProjectSectionDraftListFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionDraftList/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-ProjectSectionDraftList").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionDraftList_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionDraftList_StatementNumberId option").remove();
        }
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionDraftList').val('');
    $('.ProjectSectionDraftListFiles').each(function (i, row) {
        $('#images-ProjectSectionDraftList').val($('#images-ProjectSectionDraftList').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionDraftList-fileupload').fileupload();

    $('#ProjectSectionDraftList-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionDraftListFiles($('#ProjectSectionDraftListId').val());
}

function ShowStatementConfirmSelectedFiles(selectedProjectSectionStatementConfirm) {
    
    $('#ProjectSectionStatementConfirm-fileupload').fileupload();

    $('#ProjectSectionStatementConfirm-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    if (FormType == 2) {
        $('#ProjectSectionStatementConfirm-fileupload #addFile').css('display', 'none');
    }

    GetProjectSectionStatementConfirmFiles(selectedProjectSectionStatementConfirm);
}

function YesStatementConfirmFilePopupClick(selectedProjectSectionStatementConfirm) {
    var files = ''
    $('.ProjectSectionStatementConfirmFiles').each(function (i, row) {
        files = files + $(this).val() + ",";
    });

    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/SaveFiles', 'Files=' + files + "&" + 'ProjectSectionStatementConfirm.Id=' + selectedProjectSectionStatementConfirm, function (data, textStatus, xhr) {
        Messages(data.type, data.message);

    }, 'json');
}

function DataRefreshProjectSectionDraftList(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    var jsonParams = 'ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionDraftList').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionDraftList/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionDraftList tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);
        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='شماره صورت وضعیت'>" + json[i].StatementNumberTitle + "</td>");
            tr.append("<td data-th='مبلغ صورت وضعیت'>" + Seprator(json[i].PriceStatement) + "</td>");
            tr.append("<td data-th='مبلغ تایید شده صورت وضعیت'>" + Seprator(json[i].ConfirmPrice) + "</td>");
            tr.append("<td data-th='مانده حساب تایید نشده'>" + Seprator(json[i].AccountBalance) + "</td>");

            if (FormType == 1) // صدور فرم حواله
            {
                //tr.append("<td data-th='افزودن حواله'><a onmousedown = LoadProjectSectionDraft(" + json[i].Id +  ")  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='صدور'></a> </td>");
                tr.append("<td data-th='افزودن حواله'><a onmousedown = 'PopupFormHtml(\"ثبت حواله\", \"/ProjectDevision/ProjectSectionDraft/_Index\", \"IndexProjectSectionDraftCallback(" + json[i].Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='صدور'></a> </td>");
                ////<a onmousedown='PopupFormHtml(\"پیوست ها\", \"/ProjectDevision/ProjectSectionStatementConfirm/_FileUpload\", \"ShowStatementConfirmSelectedFiles(" + json[i].Id + ")\", true, \"YesStatementConfirmFilePopupClick(" + json[i].Id + ");\", \"#cpb2\", \"ReloadProjectSectionStatementConfirmFiles()\")' title='افزودن پیوست'><input type='button' class='btn btn-warning' style='width:150px;' value='افزودن پیوست'></a>
            }
            else if (FormType == 2) // تایید فرم حواله 
            {
                tr.append("<td data-th='افزودن حواله'><a onmousedown = 'PopupFormHtml(\"ثبت حواله\", \"/ProjectDevision/ProjectSectionDraft/_Index\", \"IndexProjectSectionDraftCallback(" + json[i].Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='تایید'></a></td>");
                //<a onmousedown='PopupFormHtml(\"پیوست ها\", \"/ProjectDevision/ProjectSectionStatementConfirm/_FileUpload\", \"ShowStatementConfirmSelectedFiles(" + json[i].Id + ")\", false, \"\", \"#cpb2\", \"ReloadProjectSectionStatementConfirmFiles()\")' title='مشاهده پیوست'><input type='button' class='btn btn-warning' style='width:150px;' value='مشاهده پیوست'></a> 
            }
            $('#tbl-ProjectSectionDraftList tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}


function LoadProjectSectionDraft(Id) {
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraft/_Index', '', '#FormPopup-ProjectSectionDraftList #modal-main', 'IndexProjectSectionDraftCallback(' + Id + ');');
    $('#FormPopup-ProjectSectionDraftList').modal();
}



function LoadDataProjectSectionDraftList(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionDraftList/_List", ProjectSectionDraftListPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionDraftList(pageRecord, $('#tbl-ProjectSectionDraftList .page-size').val(), $('#sort-ProjectSectionDraftList').val());

        Pager(pageRecord, $('#tbl-ProjectSectionDraftList .page-size').val(), "ProjectSectionDraftList", totalRecords);
    }
}

function GetProjectSectionStatementConfirmFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/GetProjectSectionStatementConfirmFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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
        $('#ProjectSectionStatementConfirm-fileupload').fileupload('option', 'done').call($('#ProjectSectionStatementConfirm-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}

function ReloadProjectSectionStatementConfirmFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/ReloadFiles', {}, function (data, textStatus, xhr) { });
}