var ProjectSectionWinnerPermissions;


$(function () {
    ProjectSectionWinnerPermissions = $('#permission-ProjectSectionWinner').val().split(',');
    
    if ($.inArray("/ProjectDevision/ProjectSectionWinner/_List/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinner/_List', '', '#FormList-ProjectSectionWinner', 'ListProjectSectionWinnerCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinner/_Create', '', '#FormContainer-ProjectSectionWinner', 'CreateProjectSectionWinnerCallback();');
    }

    EventHandlerProjectSectionWinner();
});

function CreateProjectSectionWinnerCallback() {
    CheckValueProjectSectionWinner();
    HandleValidation();

    if (SelectedProjectAssignmentType == 5) {
        $('#WinnerRankId').css('display', 'none');
    }
}

function UpdateProjectSectionWinnerCallback() {
    CreateProjectSectionWinnerCallback();
}

function ListProjectSectionWinnerCallback() {
    Pager(1, 5, "ProjectSectionWinner", DataRefreshProjectSectionWinner(1, 5, $("#sort-ProjectSectionWinner").val()));
    
    HandleValidation();

    SortArrow();
}

function InsertWinner() {

    Ajax('Post', '/ProjectDevision/ProjectSectionWinner/_Create', 'ProjectSectionWinner.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionWinner').serialize(), function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        ClearFormProjectSectionWinner();

        if ($('#tbl-ProjectSectionWinner .page-record').val() == null)
            LoadDataProjectSectionWinner(1);
        else
            LoadDataProjectSectionWinner($('#tbl-ProjectSectionWinner .page-record').val());

        LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
        SelectProjectSectionAssignment();

        if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) == -1) {
            $('#FormContainer-ProjectSectionWinner').fadeOut('fast');
        }

    }, 'json');
}



function EventHandlerProjectSectionWinner() {
    $("#FormContainer-ProjectSectionWinner").on("submit", "#frm-ProjectSectionWinner", function (e) {
        e.preventDefault();

        PopupFormHtml("سوابق ماده 46 و ماده 48 پیمانکار", "/ProjectDevision/ProjectSectionWinner4648/_Index", "IndexProjectSectionWinner4648(" + $('#ProjectSectionWinner_ContractorId').val() + ");", true, 'InsertWinner();');

    });




    $("#FormContainer-ProjectSectionWinner").on("click", "#frm-ProjectSectionWinner .btnNew", function () {
        ClearFormProjectSectionWinner();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionWinner").on("keypress", "#tbl-ProjectSectionWinner tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionWinner(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionWinner").on("change keyup", "#tbl-ProjectSectionWinner tbody tr:first select", function (e) {
        LoadDataProjectSectionWinner(1);
    });

}

function DataRefreshProjectSectionWinner(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionWinner.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionWinner').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionWinner/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionWinner tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام شرکت'>" + json[i].Contractor.CompanyName + "</td>");
            tr.append("<td data-th='رتبه'>" + json[i].Rank.Title + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionWinner/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionWinner(" + json[i].ProjectSectionWinner.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionWinner'," + json[i].ProjectSectionWinner.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionWinner(" + json[i].ProjectSectionWinner.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionWinner'," + json[i].ProjectSectionWinner.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionWinner tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Update/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionWinner/_Delete/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) == -1) {
            $('#tbl-ProjectSectionWinner th:last').remove();
            $('#tbl-ProjectSectionWinner tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionWinner tfoot td').attr('colspan', $('#tbl-ProjectSectionWinner tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionWinner(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionWinner/_List/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionWinner(pageRecord, $('#tbl-ProjectSectionWinner .page-size').val(), $('#sort-ProjectSectionWinner').val());

        Pager(pageRecord, $('#tbl-ProjectSectionWinner .page-size').val(), "ProjectSectionWinner", totalRecords);
    }
}

function ClearFormProjectSectionWinner() {
    
    $('#frm-ProjectSectionWinner input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionWinner/_Create/" + SelectedProjectAssignmentType, ProjectSectionWinnerPermissions) > -1) {
        $('#ProjectSectionWinnerId').val("-1");
        $('#btnSaveProjectSectionWinner').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionWinner').validate();
    $('#frm-ProjectSectionWinner').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionWinner(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionWinner/_Update', { Id: id }, '#FormContainer-ProjectSectionWinner', 'UpdateProjectSectionWinnerCallback();');
}



function DeleteProjectSectionWinner(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionWinner/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionWinner tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionWinner .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionWinner .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionWinner .page-record').val() - 1;
        }

        LoadDataProjectSectionWinner(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionWinner() {
    if ($('#ProjectSectionWinnerId').val() != '-1')
        $('#btnSaveProjectSectionWinner').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}