var CityPermissions;

$(function () {
    CityPermissions = $('#permission-City').val().split(',');
    
    if ($.inArray("/BaseInformation/City/_List", CityPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/City/_List', '', '#FormList-City', 'ListCityCallback();');
    }

    if ($.inArray("/BaseInformation/City/_Create", CityPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/City/_Create', '', '#FormContainer-City', 'CreateCityCallback();');
    }

    EventHandlerCity();
});

function CreateCityCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateCityCallback() {
    CheckValue();

    HandleValidation();
}

function ListCityCallback() {
    Pager(1, 5, "City", DataRefreshCity(1, 5, $("#sort-City").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerCity() {
    $("#FormContainer-City").on("submit", "#frm-City", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/City/_Create', $('#frm-City').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormCity();

            if ($('#tbl-City .page-record').val() == null)
                LoadDataCity(1);
            else
                LoadDataCity($('#tbl-City .page-record').val());

            if ($.inArray("/BaseInformation/City/_Create", CityPermissions) == -1) {
                $('#FormContainer-City').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-City").on("click", "#frm-City .btnNew", function () {
        ClearFormCity();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-City").on("keypress", "#tbl-City tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataCity(1);
            return false;
        }
    });

    $("#FormList-City").on("change keyup", "#tbl-City tbody tr:first select", function (e) {
        LoadDataCity(1);
    });

    $("#FormContainer-City").on("change keyup", "#City_StateId", function () {
        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#City_Code').removeData('previousValue');
        $('#City_Code').valid();

        $('#City_Title').removeData('previousValue');
        $('#City_Title').valid();
    });
}

function DataRefreshCity(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-City').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/City/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-City tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان استان'>" + json[i].State.Title + "</td>");
            tr.append("<td data-th='کد شهرستان'>" + json[i].City.Code + "</td>");
            tr.append("<td data-th='عنوان شهرستان'>" + json[i].City.Title + "</td>");

            if ($.inArray("/BaseInformation/City/_Update", CityPermissions) > -1 && $.inArray("/BaseInformation/City/_Delete", CityPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateCity(" + json[i].City.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteCity'," + json[i].City.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/City/_Update", CityPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateCity(" + json[i].City.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/City/_Delete", CityPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteCity'," + json[i].City.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-City tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/City/_Update", CityPermissions) == -1 && $.inArray("/BaseInformation/City/_Delete", CityPermissions) == -1) {
            $('#tbl-City th:last').remove();
            $('#tbl-City tbody tr:first td:last').remove();
            $('#tbl-City tfoot td').attr('colspan', $('#tbl-City tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataCity(pageRecord) {
    if ($.inArray("/BaseInformation/City/_List", CityPermissions) > -1) {
        var totalRecords = DataRefreshCity(pageRecord, $('#tbl-City .page-size').val(), $('#sort-City').val());

        Pager(pageRecord, $('#tbl-City .page-size').val(), "City", totalRecords);
    }
}

function ClearFormCity() {
    
    $('#frm-City input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/City/_Create", CityPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-City').validate();
    $('#frm-City').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateCity(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/City/_Update', { Id: id }, '#FormContainer-City', 'UpdateCityCallback();');
}



function DeleteCity(id) {
    Ajax('Post', '/BaseInformation/City/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-City tbody tr').length != 2) {
            pageRecord = $('#tbl-City .page-record').val();
        }
        else {
            if ($('#tbl-City .page-record').val() != 1)
                pageRecord = $('#tbl-City .page-record').val() - 1;
        }

        LoadDataCity(pageRecord);
    }, 'json');
}