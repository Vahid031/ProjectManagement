var ProjectSectionFinanceEvaluationPermissions;


$(function () {
    ProjectSectionFinanceEvaluationPermissions = $('#permission-ProjectSectionFinanceEvaluation').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_List/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinanceEvaluation/_List', '', '#FormList-ProjectSectionFinanceEvaluation', 'ListProjectSectionFinanceEvaluationCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinanceEvaluation/_Create', '', '#FormContainer-ProjectSectionFinanceEvaluation', 'CreateProjectSectionFinanceEvaluationCallback();');
    }

    EventHandlerProjectSectionFinanceEvaluation();
});

function CreateProjectSectionFinanceEvaluationCallback() {
    CheckValueProjectSectionFinanceEvaluation();
    HandleValidation();
}

function UpdateProjectSectionFinanceEvaluationCallback() {
    CreateProjectSectionFinanceEvaluationCallback();
}

function ListProjectSectionFinanceEvaluationCallback() {
    Pager(1, 5, "ProjectSectionFinanceEvaluation", DataRefreshProjectSectionFinanceEvaluation(1, 5, $("#sort-ProjectSectionFinanceEvaluation").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionFinanceEvaluation() {
    $("#FormContainer-ProjectSectionFinanceEvaluation").on("submit", "#frm-ProjectSectionFinanceEvaluation", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionFinanceEvaluation/_Create', 'ProjectSectionFinanceEvaluation.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionFinanceEvaluation').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionFinanceEvaluation();

            if ($('#tbl-ProjectSectionFinanceEvaluation .page-record').val() == null)
                LoadDataProjectSectionFinanceEvaluation(1);
            else
                LoadDataProjectSectionFinanceEvaluation($('#tbl-ProjectSectionFinanceEvaluation .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) == -1) {
                $('#FormContainer-ProjectSectionFinanceEvaluation').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionFinanceEvaluation").on("click", "#frm-ProjectSectionFinanceEvaluation .btnNew", function () {
        ClearFormProjectSectionFinanceEvaluation();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionFinanceEvaluation").on("keypress", "#tbl-ProjectSectionFinanceEvaluation tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionFinanceEvaluation(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionFinanceEvaluation").on("change keyup", "#tbl-ProjectSectionFinanceEvaluation tbody tr:first select", function (e) {
        LoadDataProjectSectionFinanceEvaluation(1);
    });

}

function DataRefreshProjectSectionFinanceEvaluation(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionFinanceEvaluation.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionFinanceEvaluation').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionFinanceEvaluation/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionFinanceEvaluation tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام شرکت'>" + json[i].Contractor.CompanyName + "</td>");
            tr.append("<td data-th='نمره ارزیابی'>" + json[i].ProjectSectionFinanceEvaluation.ScoreEvaluation + "</td>");
            tr.append("<td data-th='شرح'>" + json[i].ProjectSectionFinanceEvaluation.Description + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionFinanceEvaluation(" + json[i].ProjectSectionFinanceEvaluation.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionFinanceEvaluation'," + json[i].ProjectSectionFinanceEvaluation.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionFinanceEvaluation(" + json[i].ProjectSectionFinanceEvaluation.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionFinanceEvaluation'," + json[i].ProjectSectionFinanceEvaluation.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionFinanceEvaluation tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Update/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Delete/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) == -1) {
            $('#tbl-ProjectSectionFinanceEvaluation th:last').remove();
            $('#tbl-ProjectSectionFinanceEvaluation tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionFinanceEvaluation tfoot td').attr('colspan', $('#tbl-ProjectSectionFinanceEvaluation tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionFinanceEvaluation(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_List/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionFinanceEvaluation(pageRecord, $('#tbl-ProjectSectionFinanceEvaluation .page-size').val(), $('#sort-ProjectSectionFinanceEvaluation').val());

        Pager(pageRecord, $('#tbl-ProjectSectionFinanceEvaluation .page-size').val(), "ProjectSectionFinanceEvaluation", totalRecords);
    }
}

function ClearFormProjectSectionFinanceEvaluation() {
    
    $('#frm-ProjectSectionFinanceEvaluation input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionFinanceEvaluation/_Create/" + SelectedProjectAssignmentType, ProjectSectionFinanceEvaluationPermissions) > -1) {
        $('#ProjectSectionFinanceEvaluationId').val("-1");
        $('#btnSaveProjectSectionFinanceEvaluation').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionFinanceEvaluation').validate();
    $('#frm-ProjectSectionFinanceEvaluation').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionFinanceEvaluation(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionFinanceEvaluation/_Update', { Id: id }, '#FormContainer-ProjectSectionFinanceEvaluation', 'UpdateProjectSectionFinanceEvaluationCallback();');
}



function DeleteProjectSectionFinanceEvaluation(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionFinanceEvaluation/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionFinanceEvaluation tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionFinanceEvaluation .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionFinanceEvaluation .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionFinanceEvaluation .page-record').val() - 1;
        }

        LoadDataProjectSectionFinanceEvaluation(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionFinanceEvaluation() {
    if ($('#ProjectSectionFinanceEvaluationId').val() != '-1')
        $('#btnSaveProjectSectionFinanceEvaluation').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}