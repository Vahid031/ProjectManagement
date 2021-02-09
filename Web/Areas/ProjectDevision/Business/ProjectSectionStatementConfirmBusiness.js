var ProjectSectionStatementConfirmPermissions;
var ProjectSectionStatementId;
var sp;
//$(function () {
//    ProjectSectionStatementConfirmPermissions = $('#permission-ProjectSectionStatementConfirm').val().split(',');
    
//    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_List", ProjectSectionStatementConfirmPermissions) > -1) {
//        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatementConfirm/_ListStatement', '', '#FormList-ProjectSectionStatementConfirm', 'ListProjectSectionStatementCallback();');
//    } 

//});


function IndexProjectSectionStatementConfirmCallback(Id,SupervisorPrice) {
    ProjectSectionStatementId = Id;
    sp = SupervisorPrice;
    //alert(SupervisorPrice);
    $('#ProjectSectionStatementConfirm_Price').val(sp);
    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Create", ProjectSectionStatementConfirmPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatementConfirm/_Create', '', '#FormContainer-ProjectSectionStatementConfirm', 'CreateProjectSectionStatementConfirmCallback();');
        $('#ProjectSectionStatementConfirm_Price').val(sp);
    }
    $('#ProjectSectionStatementConfirm_Price').val(sp);
 
    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_List", ProjectSectionStatementConfirmPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatementConfirm/_List', '', '#FormList-ProjectSectionStatementConfirm', 'ListProjectSectionStatementConfirmCallback();');
    }
    $('#ProjectSectionStatementConfirm_Price').val(sp);
    EventHandlerProjectSectionStatementConfirm();
}





function CreateProjectSectionStatementConfirmCallback() {
    $('#ProjectSectionStatementConfirm_ProjectSectionStatementId').val(ProjectSectionStatementId);
    DatePic('#ProjectSectionStatementConfirm_Date');
    $('#ProjectSectionStatementConfirm_Price').val(sp);
    CheckValueProjectSectionStatementConfirm();
    HandleValidation();
    $('#ProjectSectionStatementConfirm_Price').val(sp);
}

function UpdateProjectSectionStatementConfirmCallback() {
    CreateProjectSectionStatementConfirmCallback();
}

function ListProjectSectionStatementConfirmCallback() {

    Pager(1, 5, "ProjectSectionStatementConfirm", DataRefreshProjectSectionStatementConfirm(1, 5, $("#sort-ProjectSectionStatementConfirm").val()));
    HandleValidation();

    SortArrow();
}

//function ListProjectSectionStatementConfirmCallback() {

//    Pager(1, 5, "ProjectSectionStatementConfirm", DataRefreshProjectSectionStatementConfirm(1, 5, $("#sort-ProjectSectionStatementConfirm").val()));
//    HandleValidation();

//    SortArrow();
//}

function EventHandlerProjectSectionStatementConfirm() {
    $("#FormContainer-ProjectSectionStatementConfirm").on("submit", "#frm-ProjectSectionStatementConfirm", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectSectionStatementConfirm_Price').val($('#ProjectSectionStatementConfirm_Price').val().replace(/\,/g, ''));


        Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/_Create',  'ProjectSectionStatementConfirm.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionStatementConfirm').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionStatementConfirm();

            if ($('#tbl-ProjectSectionStatementConfirm .page-record').val() == null)
                LoadDataProjectSectionStatementConfirm(1);
            else
                LoadDataProjectSectionStatementConfirm($('#tbl-ProjectSectionStatementConfirm .page-record').val());

            //LoadDataProjectSectionFinance($('#tbl-ProjectSectionFinance .page-record').val());
           // SelectProjectSectionFinance();

            if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Create", ProjectSectionStatementConfirmPermissions) == -1) {
                $('#FormContainer-ProjectSectionStatementConfirm').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionStatementConfirm").on("click", "#frm-ProjectSectionStatementConfirm .btnNew", function () {
        ClearFormProjectSectionStatementConfirm();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionStatementConfirm").on("keypress", "#tbl-ProjectSectionStatementConfirm tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionStatementConfirm(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionStatementConfirm").on("change keyup", "#tbl-ProjectSectionStatementConfirm tbody tr:first select", function (e) {
        LoadDataProjectSectionStatementConfirm(1);
    });


    $("#FormContainer-ProjectSectionStatementConfirm").on("click", "#btnShowProjectSectionStatementConfirmFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionStatementConfirm/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-ProjectSectionStatementConfirm").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionStatementConfirm_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionStatementConfirm_StatementNumberId option").remove();
        }
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionStatementConfirm').val('');
    $('.ProjectSectionStatementConfirmFiles').each(function (i, row) {
        $('#images-ProjectSectionStatementConfirm').val($('#images-ProjectSectionStatementConfirm').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionStatementConfirm-fileupload').fileupload();

    $('#ProjectSectionStatementConfirm-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionStatementConfirmFiles($('#ProjectSectionStatementConfirmId').val());
}


// لیست تایید صورت وضعیت
function DataRefreshProjectSectionStatementConfirm(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    var jsonParams = 'ProjectSectionStatementConfirm.ProjectSectionStatementId=' + ProjectSectionStatementId + '&' + $('#frm-tbl-ProjectSectionStatementConfirm').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionStatementConfirm tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);
        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='مبلغ'>" + Seprator(json[i].ProjectSectionStatementConfirm.Price) + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionStatementConfirm.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Update", ProjectSectionStatementConfirmPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Delete", ProjectSectionStatementConfirmPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionStatementConfirm(" + json[i].ProjectSectionStatementConfirm.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionStatementConfirm'," + json[i].ProjectSectionStatementConfirm.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Update", ProjectSectionStatementConfirmPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionStatementConfirm(" + json[i].ProjectSectionStatementConfirm.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Delete", ProjectSectionStatementConfirmPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionStatementConfirm'," + json[i].ProjectSectionStatementConfirm.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionStatementConfirm tbody').append(tr);
        }

        if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Update", ProjectSectionStatementConfirmPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Delete", ProjectSectionStatementConfirmPermissions) == -1) {
            $('#tbl-ProjectSectionStatementConfirm th:last').remove();
            $('#tbl-ProjectSectionStatementConfirm tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionStatementConfirm tfoot td').attr('colspan', $('#tbl-ProjectSectionStatementConfirm tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionStatementConfirm(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_List", ProjectSectionStatementConfirmPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionStatementConfirm(pageRecord, $('#tbl-ProjectSectionStatementConfirm .page-size').val(), $('#sort-ProjectSectionStatementConfirm').val());

        Pager(pageRecord, $('#tbl-ProjectSectionStatementConfirm .page-size').val(), "ProjectSectionStatementConfirm", totalRecords);
    }
}

function ClearFormProjectSectionStatementConfirm() {

    $('#frm-ProjectSectionStatementConfirm input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_Create", ProjectSectionStatementConfirmPermissions) > -1) {
        $('#ProjectSectionStatementConfirmId').val("-1");
        $('#btnSaveProjectSectionStatementConfirm').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-ProjectSectionStatementConfirm').validate();
    $('#frm-ProjectSectionStatementConfirm').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionStatementConfirm(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionStatementConfirm/_Update', { Id: id }, '#FormContainer-ProjectSectionStatementConfirm', 'UpdateProjectSectionStatementConfirmCallback();');
}



function DeleteProjectSectionStatementConfirm(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirm/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionStatementConfirm tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionStatementConfirm .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionStatementConfirm .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionStatementConfirm .page-record').val() - 1;
        }

        LoadDataProjectSectionStatementConfirm(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionStatementConfirm() {
    if ($('#ProjectSectionStatementConfirmId').val() != '-1')
        $('#btnSaveProjectSectionStatementConfirm').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


