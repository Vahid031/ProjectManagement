var ProgramPermissions;

$(function () {
    ProgramPermissions = $('#permission-Program').val().split(',');
    
    if ($.inArray("/BaseInformation/Program/_List", ProgramPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Program/_List', '', '#FormList-Program', 'ListProgramCallback();');
    }

    if ($.inArray("/BaseInformation/Program/_Create", ProgramPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Program/_Create', '', '#FormContainer-Program', 'CreateProgramCallback();');
    }

    EventHandlerProgram();
});

function CreateProgramCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateProgramCallback() {
    CheckValue();

    HandleValidation();
}

function ListProgramCallback() {
    Pager(1, 5, "Program", DataRefreshProgram(1, 5, $("#sort-Program").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerProgram() {
    $("#FormContainer-Program").on("submit", "#frm-Program", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Program/_Create', $('#frm-Program').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProgram();

            if ($('#tbl-Program .page-record').val() == null)
                LoadDataProgram(1);
            else
                LoadDataProgram($('#tbl-Program .page-record').val());

            if ($.inArray("/BaseInformation/Program/_Create", ProgramPermissions) == -1) {
                $('#FormContainer-Program').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Program").on("click", "#frm-Program .btnNew", function () {
        ClearFormProgram();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Program").on("keypress", "#tbl-Program tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProgram(1);
            return false;
        }
    });

    $("#FormList-Program").on("change keyup", "#tbl-Program tbody tr:first select", function (e) {
        LoadDataProgram(1);
    });

}

function DataRefreshProgram(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Program').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Program/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Program tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان برنامه'>" + json[i].Program.Title + "</td>");
            tr.append("<td data-th='از سال'>" + json[i].Program.FromYear + "</td>");
            tr.append("<td data-th='تا سال'>" + json[i].Program.ToYear + "</td>");
            //tr.append("<td data-th='@Html.DisplayNameFor(model => model.FinantialYear.Title)'>" + json[i].FinantialYear.Title + "</td>");

            if ($.inArray("/BaseInformation/Program/_Update", ProgramPermissions) > -1 && $.inArray("/BaseInformation/Program/_Delete", ProgramPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProgram(" + json[i].Program.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProgram'," + json[i].Program.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Program/_Update", ProgramPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProgram(" + json[i].Program.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Program/_Delete", ProgramPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProgram'," + json[i].Program.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Program tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Program/_Update", ProgramPermissions) == -1 && $.inArray("/BaseInformation/Program/_Delete", ProgramPermissions) == -1) {
            $('#tbl-Program th:last').remove();
            $('#tbl-Program tbody tr:first td:last').remove();
            $('#tbl-Program tfoot td').attr('colspan', $('#tbl-Program tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProgram(pageRecord) {
    if ($.inArray("/BaseInformation/Program/_List", ProgramPermissions) > -1) {
        var totalRecords = DataRefreshProgram(pageRecord, $('#tbl-Program .page-size').val(), $('#sort-Program').val());

        Pager(pageRecord, $('#tbl-Program .page-size').val(), "Program", totalRecords);
    }
}

function ClearFormProgram() {
    
    $('#frm-Program input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Program/_Create", ProgramPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Program').validate();
    $('#frm-Program').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProgram(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Program/_Update', { Id: id }, '#FormContainer-Program', 'UpdateProgramCallback();');
}



function DeleteProgram(id) {
    Ajax('Post', '/BaseInformation/Program/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Program tbody tr').length != 2) {
            pageRecord = $('#tbl-Program .page-record').val();
        }
        else {
            if ($('#tbl-Program .page-record').val() != 1)
                pageRecord = $('#tbl-Program .page-record').val() - 1;
        }

        LoadDataProgram(pageRecord);
    }, 'json');
}