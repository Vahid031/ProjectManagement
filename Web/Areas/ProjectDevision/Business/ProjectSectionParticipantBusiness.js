var ProjectSectionParticipantPermissions;


$(function () {
    ProjectSectionParticipantPermissions = $('#permission-ProjectSectionParticipant').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_List/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionParticipant/_List', '', '#FormList-ProjectSectionParticipant', 'ListProjectSectionParticipantCallback();');
    }

    if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionParticipant/_Create', '', '#FormContainer-ProjectSectionParticipant', 'CreateProjectSectionParticipantCallback();');
    }

    EventHandlerProjectSectionParticipant();
});

function CreateProjectSectionParticipantCallback() {
    CheckValueProjectSectionParticipant();
    HandleValidation();
}

function UpdateProjectSectionParticipantCallback() {
    CreateProjectSectionParticipantCallback();
}

function ListProjectSectionParticipantCallback() {
    Pager(1, 5, "ProjectSectionParticipant", DataRefreshProjectSectionParticipant(1, 5, $("#sort-ProjectSectionParticipant").val()));
    
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionParticipant() {
    $("#FormContainer-ProjectSectionParticipant").on("submit", "#frm-ProjectSectionParticipant", function (e) {
        e.preventDefault();

        Ajax('Post', '/ProjectDevision/ProjectSectionParticipant/_Create', 'ProjectSectionParticipant.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionParticipant').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionParticipant();

            if ($('#tbl-ProjectSectionParticipant .page-record').val() == null)
                LoadDataProjectSectionParticipant(1);
            else
                LoadDataProjectSectionParticipant($('#tbl-ProjectSectionParticipant .page-record').val());

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();

            if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) == -1) {
                $('#FormContainer-ProjectSectionParticipant').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionParticipant").on("click", "#frm-ProjectSectionParticipant .btnNew", function () {
        ClearFormProjectSectionParticipant();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionParticipant").on("keypress", "#tbl-ProjectSectionParticipant tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionParticipant(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionParticipant").on("change keyup", "#tbl-ProjectSectionParticipant tbody tr:first select", function (e) {
        LoadDataProjectSectionParticipant(1);
    });

}

function DataRefreshProjectSectionParticipant(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'ProjectSectionParticipant.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionParticipant').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/ProjectDevision/ProjectSectionParticipant/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionParticipant tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نام شرکت کننده'>" + json[i].ProjectSectionParticipant.ParticipantName + "</td>");

            if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionParticipant(" + json[i].ProjectSectionParticipant.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionParticipant'," + json[i].ProjectSectionParticipant.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionParticipant(" + json[i].ProjectSectionParticipant.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionParticipant'," + json[i].ProjectSectionParticipant.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionParticipant tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Update/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionParticipant/_Delete/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) == -1) {
            $('#tbl-ProjectSectionParticipant th:last').remove();
            $('#tbl-ProjectSectionParticipant tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionParticipant tfoot td').attr('colspan', $('#tbl-ProjectSectionParticipant tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionParticipant(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_List/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionParticipant(pageRecord, $('#tbl-ProjectSectionParticipant .page-size').val(), $('#sort-ProjectSectionParticipant').val());

        Pager(pageRecord, $('#tbl-ProjectSectionParticipant .page-size').val(), "ProjectSectionParticipant", totalRecords);
    }
}

function ClearFormProjectSectionParticipant() {
    
    $('#frm-ProjectSectionParticipant input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/ProjectDevision/ProjectSectionParticipant/_Create/" + SelectedProjectAssignmentType, ProjectSectionParticipantPermissions) > -1) {
        $('#ProjectSectionParticipantId').val("-1");
        $('#btnSaveProjectSectionParticipant').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectSectionParticipant').validate();
    $('#frm-ProjectSectionParticipant').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionParticipant(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionParticipant/_Update', { Id: id }, '#FormContainer-ProjectSectionParticipant', 'UpdateProjectSectionParticipantCallback();');
}



function DeleteProjectSectionParticipant(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionParticipant/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionParticipant tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionParticipant .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionParticipant .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionParticipant .page-record').val() - 1;
        }

        LoadDataProjectSectionParticipant(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionParticipant() {
    if ($('#ProjectSectionParticipantId').val() != '-1')
        $('#btnSaveProjectSectionParticipant').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}