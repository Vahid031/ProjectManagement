var ProjectSectionPermissions;


$(function () {
    ProjectSectionPermissions = $('#permission-ProjectSection').val().split(',');
    
    if ($.inArray("/ProjectDevision/ProjectSection/_List", ProjectSectionPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSection/_List', '', '#FormList-ProjectSection', 'ListProjectSectionCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSection/_Create", ProjectSectionPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSection/_Create', '', '#FormContainer-ProjectSection', 'CreateProjectSectionCallback();');
    }

    EventHandlerProjectSection();
});

function CreateProjectSectionCallback() {
    CheckValueProjectSection();
    HandleValidation();

    DatePic('#ProjectSection_ForecastStartDate');
    DatePic('#ProjectSection_ForecastEndDate');
}

function UpdateProjectSectionCallback() {
    CreateProjectSectionCallback();
}

function ListProjectSectionCallback() {
    Pager(1, 5, "ProjectSection", DataRefreshProjectSection(1, 5, $("#sort-ProjectSection").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSection() {
    $("#FormContainer-ProjectSection").on("submit", "#frm-ProjectSection", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProject == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSection/_Create', 'ProjectSection.ProjectId=' + SelectedProject + '&' + $('#frm-ProjectSection').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSection();

            if ($('#tbl-ProjectSection .page-record').val() == null)
                LoadDataProjectSection(1);
            else
                LoadDataProjectSection($('#tbl-ProjectSection .page-record').val());

            if ($.inArray("/ProjectDevision/ProjectSection/_Create", ProjectSectionPermissions) == -1) {
                $('#FormContainer-ProjectSection').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSection").on("click", "#frm-ProjectSection .btnNew", function () {
        ClearFormProjectSection();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSection").on("keypress", "#tbl-ProjectSection tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSection(1);
            return false;
        }
    });

    $("#FormList-ProjectSection").on("change keyup", "#tbl-ProjectSection tbody tr:first select", function (e) {
        LoadDataProjectSection(1);
    });

}

function DataRefreshProjectSection(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectId=' + SelectedProject + '&' + $('#frm-tbl-ProjectSection').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSection/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSection tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان فاز پروژه'>" + json[i].ProjectSectionTitle + "</td>");
            tr.append("<td data-th='نحوه واگذاری'>" + json[i].AssignmentTypeTitle + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].ForecastStartDate + "</td>");
            tr.append("<td data-th='پیش بینی تاریخ خاتمه'>" + json[i].ForecastEndDate + "</td>");
            tr.append("<td data-th='مجموع مبالغ کالا و خدمات'>" + Seprator(json[i].TotalProductServicePrice) + "</td>");
            //tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionOperationState.Title)'>" + json[i].ProjectSectionOperationState.Title + "</td>");
            //tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectSectionAssignmentState.Title)'>" + json[i].ProjectSectionAssignmentState.Title + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionProduct/_List", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='انتخاب'><a onmousedown = 'PopupFormHtml(\"برآورد مبلغ کالا و خدمات\", \"/ProjectDevision/ProjectSectionProduct/_Index\", \"IndexProjectSectionProductCallback(" + json[i].Id + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='افزودن'></a></td>");
            }

            if ($.inArray("/ProjectDevision/ProjectSection/_Update", ProjectSectionPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSection/_Delete", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSection(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSection'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSection/_Update", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSection(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSection/_Delete", ProjectSectionPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSection'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSection tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSection/_Update", ProjectSectionPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSection/_Delete", ProjectSectionPermissions) == -1) {
            $('#tbl-ProjectSection th:last').remove();
            $('#tbl-ProjectSection tbody tr:first td:last').remove();
            $('#tbl-ProjectSection tfoot td').attr('colspan', $('#tbl-ProjectSection tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSection(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSection/_List", ProjectSectionPermissions) > -1) {
        var totalRecords = DataRefreshProjectSection(pageRecord, $('#tbl-ProjectSection .page-size').val(), $('#sort-ProjectSection').val());

        Pager(pageRecord, $('#tbl-ProjectSection .page-size').val(), "ProjectSection", totalRecords);
    }
}

function ClearFormProjectSection() {
    $('#frm-ProjectSection input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSection/_Create", ProjectSectionPermissions) > -1) {
        $('#ProjectSectionId').val("-1");
        $('#btnSaveProjectSection').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSection').validate();
    $('#frm-ProjectSection').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSection(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSection/_Update', { Id: id }, '#FormContainer-ProjectSection', 'UpdateProjectSectionCallback();');
}



function DeleteProjectSection(id) {
    Ajax('Post', '/ProjectDevision/ProjectSection/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSection tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSection .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSection .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSection .page-record').val() - 1;
        }

        LoadDataProjectSection(pageRecord);
    }, 'json');
}


function CheckValueProjectSection() {
    if ($('#ProjectSectionId').val() != '-1')
        $('#btnSaveProjectSection').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}
