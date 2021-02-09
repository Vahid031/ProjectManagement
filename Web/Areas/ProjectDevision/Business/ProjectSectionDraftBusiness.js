var ProjectSectionDraftPermissions;
var ProjectSectionStatementConfirmId;

$(function () {
});


function IndexProjectSectionDraftCallback(Id) {
    ProjectSectionStatementConfirmId = Id;
    ProjectSectionDraftPermissions = $('#permission-ProjectSectionDraft').val().split(',');

    if (FormType == 1) // صدور فرم حواله 
    {
        if ($.inArray("/ProjectDevision/ProjectSectionDraft/_Create", ProjectSectionDraftPermissions) > -1) {
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraft/_Create/' + Id, '', '#FormContainer-ProjectSectionDraft', 'CreateProjectSectionDraftCallback(' + Id + ');');
        }
    }

    if ($.inArray("/ProjectDevision/ProjectSectionDraft/_List", ProjectSectionDraftPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraft/_List', '', '#FormList-ProjectSectionDraft', 'ListProjectSectionDraftCallback('+Id+');');
    }

    EventHandlerProjectSectionDraft();
}


function CreateProjectSectionDraftCallback(Id) {    
    $('#ProjectSectionDraft_ProjectSectionStatementConfirmId').val(Id);

    

    CheckValueProjectSectionDraft();
    HandleValidation();

    if (FormType == 1) // صدور فرم حواله
    {
        $('#divTempDraftPrice').css('display', 'block');
        $('#divDraftPrice').css('display', 'none');
    }
    else if (FormType == 2) // تایید فرم حواله 
    {
        $('#divTempDraftPrice').css('display', 'none');
        $('#divDraftPrice').css('display', 'block');
        $('#btnNewProjectSectionDraft').css('display', 'none');
    }
}

function UpdateProjectSectionDraftCallback(Id) {
    CheckValueProjectSectionDraft();
    HandleValidation();

    if (FormType == 1) // صدور فرم حواله
    {
        $('#divTempDraftPrice').css('display', 'block');
        $('#divDraftPrice').css('display', 'none');
    }
    else if (FormType == 2) // تایید فرم حواله 
    {
        $('#divTempDraftPrice').css('display', 'none');
        $('#divDraftPrice').css('display', 'block');
        $('#btnNewProjectSectionDraft').css('display', 'none');
    }
}

function ListProjectSectionDraftCallback() {
    Pager(1, 5, "ProjectSectionDraft", DataRefreshProjectSectionDraft(1, 5, $("#sort-ProjectSectionDraft").val()));
    HandleValidation();

    SortArrow();

    if (FormType == 1) // صدور فرم حواله
    {
        $('#tbl-ProjectSectionDraft thead').find('tr').find('th').eq(3).remove();
        $('#tbl-ProjectSectionDraft tbody').find('tr:first').eq(3).remove();
    }  
}

function EventHandlerProjectSectionDraft() {
    $("#FormContainer-ProjectSectionDraft").on("submit", "#frm-ProjectSectionDraft", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectSectionDraft_DraftPrice').val($('#ProjectSectionDraft_DraftPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionDraft_TempDraftPrice').val($('#ProjectSectionDraft_TempDraftPrice').val().replace(/\,/g, ''));
        
        if (FormType == 1) // صدور فرم حواله
        {
            $('#ProjectSectionDraft_DraftPrice').val('0');
        }
        
        Ajax('Post', '/ProjectDevision/ProjectSectionDraft/_Create', 'Files=' + $('#images-ProjectSectionDraft').val() + "&" + 'ProjectSectionDraft.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionDraft').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            
            
            
            if (FormType == 2 && (data.type == 'success' || data.type == 'info')) // صدور فرم حواله
            {
                $('#FormContainer-ProjectSectionDraft').fadeOut('fast');
            }

            if (data.type == 'success' || data.type == 'info')
            {
                ClearFormProjectSectionDraft();

                if ($('#tbl-ProjectSectionDraft .page-record').val() == null)
                    LoadDataProjectSectionDraft(1);
                else
                    LoadDataProjectSectionDraft($('#tbl-ProjectSectionDraft .page-record').val());

                LoadDataProjectSectionFinance($('#tbl-ProjectSectionFinance .page-record').val());
                SelectProjectSectionFinance();
            }

        }, 'json');
    });

    


    $("#FormContainer-ProjectSectionDraft").on("click", "#frm-ProjectSectionDraft .btnNew", function () {
        ClearFormProjectSectionDraft();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionDraft").on("keypress", "#tbl-ProjectSectionDraft tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionDraft(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionDraft").on("change keyup", "#tbl-ProjectSectionDraft tbody tr:first select", function (e) {
        LoadDataProjectSectionDraft(1);
    });


    $("#FormContainer-ProjectSectionDraft").on("click", "#btnShowProjectSectionDraftFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionDraft/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();", "#cpb2");
    });

    $("#FormContainer-ProjectSectionDraft").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionDraft_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionDraft_StatementNumberId option").remove();
        }
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionDraft').val('');
    $('.ProjectSectionDraftFiles').each(function (i, row) {
        $('#images-ProjectSectionDraft').val($('#images-ProjectSectionDraft').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionDraft-fileupload').fileupload();

    $('#ProjectSectionDraft-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionDraftFiles($('#ProjectSectionDraftId').val());
}

function DataRefreshProjectSectionDraft(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    //var Id = $('#ProjectSectionDraft_ProjectSectionStatementConfirmId').val();
    var jsonParams = 'ProjectSectionDraft.ProjectSectionId=' + SelectedProjectSection + '&' + 'ProjectSectionDraft.ProjectSectionStatementConfirmId=' + ProjectSectionStatementConfirmId + '&' + $('#frm-tbl-ProjectSectionDraft').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionDraft/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionDraft tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);
        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='شماره حواله'>" + json[i].ProjectSectionDraft.DraftNumber + "</td>");
            if (FormType == 1) // صدور فرم حواله
            {
                tr.append("<td data-th='مبلغ حواله'>" + Seprator(json[i].ProjectSectionDraft.TempDraftPrice) + "</td>");
            }
            else if (FormType == 2) // تایید فرم حواله 
            {
                tr.append("<td data-th='مبلغ حواله'>" + Seprator(json[i].ProjectSectionDraft.TempDraftPrice) + "</td>");
                tr.append("<td data-th='مبلغ تایید شده'>" + Seprator(json[i].ProjectSectionDraft.DraftPrice) + "</td>");
            }
            
            
            tr.append("<td data-th='تاریخ حواله'>" + json[i].ProjectSectionDraft.DraftDate_ + "</td>");
            
            if ($.inArray("/ProjectDevision/ProjectSectionDraft/_Update", ProjectSectionDraftPermissions) > -1) {
                tr.append("<td data-th='عملیات'><a onmousedown = UpdateProjectSectionDraft(" + json[i].ProjectSectionDraft.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else {
                tr.append("<td data-th='عملیات'></td>");    
            }

            if (FormType == 1) { // تایید فرم حواله 
                tr.find('td').eq(4).append("<a onmousedown = PrintDraftStimulSoft(" + json[i].ProjectSectionDraft.Id + ") title='چاپ'><span class='glyphicon glyphicon-print icon'></span></a>");

                if ($.inArray("/ProjectDevision/ProjectSectionDraft/_Delete", ProjectSectionDraftPermissions) > -1) {
                    tr.find('td').eq(4).append("<a onmousedown = MvcAlert('DeleteProjectSectionDraft'," + json[i].ProjectSectionDraft.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a>");
                }
            }    
            $('#tbl-ProjectSectionDraft tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionDraft(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionDraft/_List", ProjectSectionDraftPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionDraft(pageRecord, $('#tbl-ProjectSectionDraft .page-size').val(), $('#sort-ProjectSectionDraft').val());

        Pager(pageRecord, $('#tbl-ProjectSectionDraft .page-size').val(), "ProjectSectionDraft", totalRecords);
    }
}

function ClearFormProjectSectionDraft() {

    $('#frm-ProjectSectionDraft input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionDraftFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionDraft/_Create", ProjectSectionDraftPermissions) > -1) {
        $('#ProjectSectionDraftId').val("-1");
        $('#btnSaveProjectSectionDraft').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-ProjectSectionDraft').validate();
    $('#frm-ProjectSectionDraft').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionDraft(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionDraft/_Update', { Id: id }, '#FormContainer-ProjectSectionDraft', 'UpdateProjectSectionDraftCallback('+$('#ProjectSectionDraft_ProjectSectionStatementConfirmId').val()+');');
}



function DeleteProjectSectionDraft(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDraft/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionDraft tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionDraft .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionDraft .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionDraft .page-record').val() - 1;
        }

        LoadDataProjectSectionDraft(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionDraft() {
    if ($('#ProjectSectionDraftId').val() != '-1')
        $('#btnSaveProjectSectionDraft').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionDraftFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionDraft/GetProjectSectionDraftFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionDraft-fileupload').fileupload('option', 'done').call($('#ProjectSectionDraft-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionDraftFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionDraft/ReloadFiles', {}, function (data, textStatus, xhr) {});
}

function PrintDraftStimulSoft(Id) {
    //if ($.inArray("/ProjectDevision/ProjectSectionDraft/_Create", ProjectSectionDraftPermissions) > -1) {
    //    LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraft/_Report/' + Id, '', '#FormReport-ProjectSectionDraft', '');
    //}
    PopupFormHtml('چاپ حواله', '/ProjectDevision/ProjectSectionPrintDraft/_Index/' + Id, '', false,'','#cpb2');
    //LoadPartialView('GET', '/ProjectDevision/ProjectSectionPrintDraft/_Index/' + Id, '', '#modal-main');//.onload( function () { $('#FormReport-ProjectSectionDraft').modal(); });
    //$('#FormReport-ProjectSectionDraft').modal();
}