var ProjectTypePermissions;

$(function () {
    ProjectTypePermissions = $('#permission-ProjectType').val().split(',');
    
    if ($.inArray("/BaseInformation/ProjectType/_List", ProjectTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ProjectType/_List', '', '#FormList-ProjectType', 'ListProjectTypeCallback();');
    }

    if ($.inArray("/BaseInformation/ProjectType/_Create", ProjectTypePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/ProjectType/_Create', '', '#FormContainer-ProjectType', 'CreateProjectTypeCallback();');
    }

    EventHandlerProjectType();
});

function CreateProjectTypeCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateProjectTypeCallback() {
    CheckValue();

    HandleValidation();
}

function ListProjectTypeCallback() {
    Pager(1, 5, "ProjectType", DataRefreshProjectType(1, 5, $("#sort-ProjectType").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerProjectType() {
    $("#FormContainer-ProjectType").on("submit", "#frm-ProjectType", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/ProjectType/_Create', $('#frm-ProjectType').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectType();

            if ($('#tbl-ProjectType .page-record').val() == null)
                LoadDataProjectType(1);
            else
                LoadDataProjectType($('#tbl-ProjectType .page-record').val());

            if ($.inArray("/BaseInformation/ProjectType/_Create", ProjectTypePermissions) == -1) {
                $('#FormContainer-ProjectType').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-ProjectType").on("click", "#frm-ProjectType .btnNew", function () {
        ClearFormProjectType();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectType").on("keypress", "#tbl-ProjectType tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectType(1);
            return false;
        }
    });

    $("#FormList-ProjectType").on("change keyup", "#tbl-ProjectType tbody tr:first select", function (e) {
        LoadDataProjectType(1);
    });

}

function DataRefreshProjectType(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectType').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/ProjectType/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectType tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان'>" + json[i].ProjectType.Title + "</td>");

            if ($.inArray("/BaseInformation/ProjectType/_Update", ProjectTypePermissions) > -1 && $.inArray("/BaseInformation/ProjectType/_Delete", ProjectTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectType(" + json[i].ProjectType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectType'," + json[i].ProjectType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ProjectType/_Update", ProjectTypePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectType(" + json[i].ProjectType.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/ProjectType/_Delete", ProjectTypePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectType'," + json[i].ProjectType.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectType tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/ProjectType/_Update", ProjectTypePermissions) == -1 && $.inArray("/BaseInformation/ProjectType/_Delete", ProjectTypePermissions) == -1) {
            $('#tbl-ProjectType th:last').remove();
            $('#tbl-ProjectType tbody tr:first td:last').remove();
            $('#tbl-ProjectType tfoot td').attr('colspan', $('#tbl-ProjectType tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectType(pageRecord) {
    if ($.inArray("/BaseInformation/ProjectType/_List", ProjectTypePermissions) > -1) {
        var totalRecords = DataRefreshProjectType(pageRecord, $('#tbl-ProjectType .page-size').val(), $('#sort-ProjectType').val());

        Pager(pageRecord, $('#tbl-ProjectType .page-size').val(), "ProjectType", totalRecords);
    }
}

function ClearFormProjectType() {
    
    $('#frm-ProjectType input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/ProjectType/_Create", ProjectTypePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-ProjectType').validate();
    $('#frm-ProjectType').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectType(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/ProjectType/_Update', { Id: id }, '#FormContainer-ProjectType', 'UpdateProjectTypeCallback();');
}



function DeleteProjectType(id) {
    Ajax('Post', '/BaseInformation/ProjectType/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectType tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectType .page-record').val();
        }
        else {
            if ($('#tbl-ProjectType .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectType .page-record').val() - 1;
        }

        LoadDataProjectType(pageRecord);
    }, 'json');
}