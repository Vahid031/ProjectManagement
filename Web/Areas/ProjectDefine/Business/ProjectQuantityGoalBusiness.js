var ProjectQuantityGoalPermissions;


$(function () {
    ProjectQuantityGoalPermissions = $('#permission-ProjectQuantityGoal').val().split(',');

    if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_List", ProjectQuantityGoalPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectQuantityGoal/_List', '', '#FormList-ProjectQuantityGoal', 'ListProjectQuantityGoalCallback();');
    }

    if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Create", ProjectQuantityGoalPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/ProjectQuantityGoal/_Create', '', '#FormContainer-ProjectQuantityGoal', 'CreateProjectQuantityGoalCallback();');
    }

    EventHandlerProjectQuantityGoal();
});

function CreateProjectQuantityGoalCallback() {
    CheckValueProjectQuantityGoal();
    HandleValidation();
}

function UpdateProjectQuantityGoalCallback() {
    CreateProjectQuantityGoalCallback();
}

function ListProjectQuantityGoalCallback() {
    Pager(1, 5, "ProjectQuantityGoal", DataRefreshProjectQuantityGoal(1, 5, $("#sort-ProjectQuantityGoal").val()));
    
    HandleValidation();

    SortArrow();
}

// این کد برای جدا سازی سه رقم سه قم اعداد هنگام لود صفحه در زمان آپدیت میباشد
function SepreateProjectQuantityGoal() {
    SepratePrice('#ProjectQuantityGoal_UnitPrice');
}

function EventHandlerProjectQuantityGoal() {
    $("#FormContainer-ProjectQuantityGoal").on("submit", "#frm-ProjectQuantityGoal", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectQuantityGoal_UnitPrice').val($('#ProjectQuantityGoal_UnitPrice').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDefine/ProjectQuantityGoal/_Create', 'ProjectQuantityGoal.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectQuantityGoal').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectQuantityGoal();

            if ($('#tbl-ProjectQuantityGoal .page-record').val() == null)
                LoadDataProjectQuantityGoal(1);
            else
                LoadDataProjectQuantityGoal($('#tbl-ProjectQuantityGoal .page-record').val());


            // تغییر رنگ فونت جدول بالایی
            LoadDataProjectAgreement($('#tbl-ProjectAgreement .page-record').val());
            SelectProject();



            if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Create", ProjectQuantityGoalPermissions) == -1) {
                $('#FormContainer-ProjectQuantityGoal').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectQuantityGoal").on("click", "#frm-ProjectQuantityGoal .btnNew", function () {
        ClearFormProjectQuantityGoal();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectQuantityGoal").on("keypress", "#tbl-ProjectQuantityGoal tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectQuantityGoal(1);
            return false;
        }
    });

    $("#FormList-ProjectQuantityGoal").on("change keyup", "#tbl-ProjectQuantityGoal tbody tr:first select", function (e) {
        LoadDataProjectQuantityGoal(1);
    });

}

function DataRefreshProjectQuantityGoal(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectQuantityGoal.ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectQuantityGoal').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDefine/ProjectQuantityGoal/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectQuantityGoal tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نوع اهداف کمکی'>" + json[i].ProjectQuantityGoal.QuantityGoalType + "</td>");
            tr.append("<td data-th='واحد'>" + json[i].Unit.Title + "</td>");
            tr.append("<td data-th='مقدار'>" + Seprator(json[i].ProjectQuantityGoal.Amount) + "</td>");
            tr.append("<td data-th='قیمت هر واحد'>" + Seprator(json[i].ProjectQuantityGoal.UnitPrice) + "</td>");

            if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Update", ProjectQuantityGoalPermissions) > -1 && $.inArray("/ProjectDefine/ProjectQuantityGoal/_Delete", ProjectQuantityGoalPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectQuantityGoal(" + json[i].ProjectQuantityGoal.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectQuantityGoal'," + json[i].ProjectQuantityGoal.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Update", ProjectQuantityGoalPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectQuantityGoal(" + json[i].ProjectQuantityGoal.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Delete", ProjectQuantityGoalPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectQuantityGoal'," + json[i].ProjectQuantityGoal.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectQuantityGoal tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Update", ProjectQuantityGoalPermissions) == -1 && $.inArray("/ProjectDefine/ProjectQuantityGoal/_Delete", ProjectQuantityGoalPermissions) == -1) {
            $('#tbl-ProjectQuantityGoal th:last').remove();
            $('#tbl-ProjectQuantityGoal tbody tr:first td:last').remove();
            $('#tbl-ProjectQuantityGoal tfoot td').attr('colspan', $('#tbl-ProjectQuantityGoal tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectQuantityGoal(pageRecord) {
    if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_List", ProjectQuantityGoalPermissions) > -1) {
        var totalRecords = DataRefreshProjectQuantityGoal(pageRecord, $('#tbl-ProjectQuantityGoal .page-size').val(), $('#sort-ProjectQuantityGoal').val());

        Pager(pageRecord, $('#tbl-ProjectQuantityGoal .page-size').val(), "ProjectQuantityGoal", totalRecords);
    }
}

function ClearFormProjectQuantityGoal() {
    
    $('#frm-ProjectQuantityGoal input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Create", ProjectQuantityGoalPermissions) > -1) {
        $('#ProjectQuantityGoalId').val("-1");
        $('#btnSaveProjectQuantityGoal').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectQuantityGoal').validate();
    $('#frm-ProjectQuantityGoal').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectQuantityGoal(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/ProjectQuantityGoal/_Update', { Id: id }, '#FormContainer-ProjectQuantityGoal', 'UpdateProjectQuantityGoalCallback();SepreateProjectQuantityGoal();');
}



function DeleteProjectQuantityGoal(id) {
    Ajax('Post', '/ProjectDefine/ProjectQuantityGoal/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectQuantityGoal tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectQuantityGoal .page-record').val();
        }
        else {
            if ($('#tbl-ProjectQuantityGoal .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectQuantityGoal .page-record').val() - 1;
        }

        LoadDataProjectQuantityGoal(pageRecord);
    }, 'json');
}


function CheckValueProjectQuantityGoal() {
    if ($('#ProjectQuantityGoalId').val() != '-1')
        $('#btnSaveProjectQuantityGoal').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}
