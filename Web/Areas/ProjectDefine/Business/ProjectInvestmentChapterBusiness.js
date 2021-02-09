var ProjectInvestmentChapterPermissions;


$(function () {
    ProjectInvestmentChapterPermissions = $('#permission-ProjectInvestmentChapter').val().split(',');

    if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_List", ProjectInvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectInvestmentChapter/_List', '', '#FormList-ProjectInvestmentChapter', 'ListProjectInvestmentChapterCallback();');
    }

    if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Create", ProjectInvestmentChapterPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectInvestmentChapter/_Create', '', '#FormContainer-ProjectInvestmentChapter', 'CreateProjectInvestmentChapterCallback();');
    }
   
    EventHandlerProjectInvestmentChapter();
});

function CreateProjectInvestmentChapterCallback() {
    CheckValueProjectInvestmentChapter();
    HandleValidation();
}

function UpdateProjectInvestmentChapterCallback() {
    CreateProjectInvestmentChapterCallback(); 
}

function ListProjectInvestmentChapterCallback() {
    Pager(1, 5, "ProjectInvestmentChapter", DataRefreshProjectInvestmentChapter(1, 5, $("#sort-ProjectInvestmentChapter").val()));
    
    HandleValidation();

    SortArrow();
   
}


// این کد برای جدا سازی سه رقم سه قم اعداد هنگام لود صفحه در زمان آپدیت میباشد
function SepreateProjectInvestmentChapter() {
    SepratePrice('#ProjectInvestmentChapter_Price');
}

function EventHandlerProjectInvestmentChapter() {
    $("#FormContainer-ProjectInvestmentChapter").on("submit", "#frm-ProjectInvestmentChapter", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectInvestmentChapter_Price').val($('#ProjectInvestmentChapter_Price').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDefine/ProjectInvestmentChapter/_Create', 'ProjectInvestmentChapter.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectInvestmentChapter').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectInvestmentChapter();

            if ($('#tbl-ProjectInvestmentChapter .page-record').val() == null)
                LoadDataProjectInvestmentChapter(1);
            else
                LoadDataProjectInvestmentChapter($('#tbl-ProjectInvestmentChapter .page-record').val());


            // تغییر رنگ فونت جدول بالایی
            LoadDataProjectAgreement($('#tbl-ProjectAgreement .page-record').val());
            SelectProject();


            if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Create", ProjectInvestmentChapterPermissions) == -1) {
                $('#FormContainer-ProjectInvestmentChapter').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectInvestmentChapter").on("click", "#frm-ProjectInvestmentChapter .btnNew", function () {
        ClearFormProjectInvestmentChapter();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectInvestmentChapter").on("keypress", "#tbl-ProjectInvestmentChapter tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectInvestmentChapter(1);
            return false;
        }
    });

    $("#FormList-ProjectInvestmentChapter").on("change keyup", "#tbl-ProjectInvestmentChapter tbody tr:first select", function (e) {
        LoadDataProjectInvestmentChapter(1);
    });

    $("#FormContainer-ProjectInvestmentChapter").on("change keyup", "#InvestmentChapterId", function () {
        if ($(this).val() != '') {
            GetInvestmentChapterTypesByInvestmentChapterId("#ProjectInvestmentChapter_InvestmentChapterTypeId", $(this).val());
        }
        else {
            $("#ProjectInvestmentChapter_InvestmentChapterTypeId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });
}

function DataRefreshProjectInvestmentChapter(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectInvestmentChapter.ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectInvestmentChapter').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProjectInvestmentChapter/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectInvestmentChapter tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان سال مالی'>" + json[i].FinantialYear.Title + "</td>");
            tr.append("<td data-th='فصل سرمایه گذاری'>" + json[i].InvestmentChapter.Title + "</td>");
            tr.append("<td data-th='نوع فصول سرمایه گذاری'>" + json[i].InvestmentChapterType.Title + "</td>");
            tr.append("<td data-th='مبلغ'>" + Seprator(json[i].ProjectInvestmentChapter.Price) + "</td>");

            if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Update", ProjectInvestmentChapterPermissions) > -1 && $.inArray("/ProjectDefine/ProjectInvestmentChapter/_Delete", ProjectInvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectInvestmentChapter(" + json[i].ProjectInvestmentChapter.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectInvestmentChapter'," + json[i].ProjectInvestmentChapter.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Update", ProjectInvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectInvestmentChapter(" + json[i].ProjectInvestmentChapter.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Delete", ProjectInvestmentChapterPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectInvestmentChapter'," + json[i].ProjectInvestmentChapter.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectInvestmentChapter tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Update", ProjectInvestmentChapterPermissions) == -1 && $.inArray("/ProjectDefine/ProjectInvestmentChapter/_Delete", ProjectInvestmentChapterPermissions) == -1) {
            $('#tbl-ProjectInvestmentChapter th:last').remove();
            $('#tbl-ProjectInvestmentChapter tbody tr:first td:last').remove();
            $('#tbl-ProjectInvestmentChapter tfoot td').attr('colspan', $('#tbl-ProjectInvestmentChapter tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectInvestmentChapter(pageRecord) {
    if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_List", ProjectInvestmentChapterPermissions) > -1) {
        var totalRecords = DataRefreshProjectInvestmentChapter(pageRecord, $('#tbl-ProjectInvestmentChapter .page-size').val(), $('#sort-ProjectInvestmentChapter').val());

        Pager(pageRecord, $('#tbl-ProjectInvestmentChapter .page-size').val(), "ProjectInvestmentChapter", totalRecords);
    }
}

function ClearFormProjectInvestmentChapter() {
    
    $('#frm-ProjectInvestmentChapter input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Create", ProjectInvestmentChapterPermissions) > -1) {
        $('#ProjectInvestmentChapterId').val("-1");
        $('#btnSaveProjectInvestmentChapter').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectInvestmentChapter').validate();
    $('#frm-ProjectInvestmentChapter').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectInvestmentChapter(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProjectInvestmentChapter/_Update', { Id: id }, '#FormContainer-ProjectInvestmentChapter', 'UpdateProjectInvestmentChapterCallback(); SepreateProjectInvestmentChapter();');
}



function DeleteProjectInvestmentChapter(id) {
    Ajax('Post', '/ProjectDefine/ProjectInvestmentChapter/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectInvestmentChapter tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectInvestmentChapter .page-record').val();
        }
        else {
            if ($('#tbl-ProjectInvestmentChapter .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectInvestmentChapter .page-record').val() - 1;
        }

        LoadDataProjectInvestmentChapter(pageRecord);
    }, 'json');
}

function GetInvestmentChapterTypesByInvestmentChapterId(container, investmentChapterId) {
    Ajax('Post', '/BaseInformation/InvestmentChapterType/GetInvestmentChapterTypesByInvestmentChapterId', 'investmentChapterId=' + investmentChapterId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}


function CheckValueProjectInvestmentChapter() {
    if ($('#ProjectInvestmentChapterId').val() != '-1')
        $('#btnSaveProjectInvestmentChapter').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}
