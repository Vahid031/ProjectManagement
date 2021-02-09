var ProjectAveragePermissions;


$(function () {
    ProjectAveragePermissions = $('#permission-ProjectAverage').val().split(',');

    if ($.inArray("/ProjectDefine/ProjectAverage/_List", ProjectAveragePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectAverage/_List', '', '#FormList-ProjectAverage', 'ListProjectAverageCallback();');
    }

    if ($.inArray("/ProjectDefine/ProjectAverage/_Create", ProjectAveragePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectAverage/_Create', '', '#FormContainer-ProjectAverage', 'CreateProjectAverageCallback();');
    }

    EventHandlerProjectAverage();
});

function CreateProjectAverageCallback() {
    CheckValueProjectAverage();
    HandleValidation();
}

function UpdateProjectAverageCallback() {
    CreateProjectAverageCallback();
}

function ListProjectAverageCallback() {
    Pager(1, 5, "ProjectAverage", DataRefreshProjectAverage(1, 5, $("#sort-ProjectAverage").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectAverage() {
    $("#FormContainer-ProjectAverage").on("submit", "#frm-ProjectAverage", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectAverage_Average').val($('#ProjectAverage_Average').val().replace(/\,/g, ''));


        Ajax('Post', '/ProjectDefine/ProjectAverage/_Create', 'ProjectAverage.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectAverage').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectAverage();

            if ($('#tbl-ProjectAverage .page-record').val() == null)
                LoadDataProjectAverage(1);
            else
                LoadDataProjectAverage($('#tbl-ProjectAverage .page-record').val());


            // تغییر رنگ فونت جدول بالایی
            LoadDataProjectAgreement($('#tbl-ProjectAgreement .page-record').val());
            SelectProject();


            if ($.inArray("/ProjectDefine/ProjectAverage/_Create", ProjectAveragePermissions) == -1) {
                $('#FormContainer-ProjectAverage').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectAverage").on("click", "#frm-ProjectAverage .btnNew", function () {
        ClearFormProjectAverage();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectAverage").on("keypress", "#tbl-ProjectAverage tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectAverage(1);
            return false;
        }
    });

    $("#FormList-ProjectAverage").on("change keyup", "#tbl-ProjectAverage tbody tr:first select", function (e) {
        LoadDataProjectAverage(1);
    });

}

function DataRefreshProjectAverage(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectAverage.ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectAverage').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProjectAverage/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectAverage tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان سال مالی'>" + json[i].FinantialYear.Title + "</td>");
            tr.append("<td data-th='میانگین'>" + json[i].ProjectAverage.Average + "</td>");

            if ($.inArray("/ProjectDefine/ProjectAverage/_Update", ProjectAveragePermissions) > -1 && $.inArray("/ProjectDefine/ProjectAverage/_Delete", ProjectAveragePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectAverage(" + json[i].ProjectAverage.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectAverage'," + json[i].ProjectAverage.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectAverage/_Update", ProjectAveragePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectAverage(" + json[i].ProjectAverage.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectAverage/_Delete", ProjectAveragePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectAverage'," + json[i].ProjectAverage.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectAverage tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProjectAverage/_Update", ProjectAveragePermissions) == -1 && $.inArray("/ProjectDefine/ProjectAverage/_Delete", ProjectAveragePermissions) == -1) {
            $('#tbl-ProjectAverage th:last').remove();
            $('#tbl-ProjectAverage tbody tr:first td:last').remove();
            $('#tbl-ProjectAverage tfoot td').attr('colspan', $('#tbl-ProjectAverage tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectAverage(pageRecord) {
    if ($.inArray("/ProjectDefine/ProjectAverage/_List", ProjectAveragePermissions) > -1) {
        var totalRecords = DataRefreshProjectAverage(pageRecord, $('#tbl-ProjectAverage .page-size').val(), $('#sort-ProjectAverage').val());

        Pager(pageRecord, $('#tbl-ProjectAverage .page-size').val(), "ProjectAverage", totalRecords);
    }
}

function ClearFormProjectAverage() {
    
    $('#frm-ProjectAverage input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDefine/ProjectAverage/_Create", ProjectAveragePermissions) > -1) {
        $('#ProjectAverageId').val("-1");
        $('#btnSaveProjectAverage').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectAverage').validate();
    $('#frm-ProjectAverage').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectAverage(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProjectAverage/_Update', { Id: id }, '#FormContainer-ProjectAverage', 'UpdateProjectAverageCallback();');
}



function DeleteProjectAverage(id) {
    Ajax('Post', '/ProjectDefine/ProjectAverage/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectAverage tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectAverage .page-record').val();
        }
        else {
            if ($('#tbl-ProjectAverage .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectAverage .page-record').val() - 1;
        }

        LoadDataProjectAverage(pageRecord);
    }, 'json');
}


function CheckValueProjectAverage() {
    if ($('#ProjectAverageId').val() != '-1')
        $('#btnSaveProjectAverage').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}
