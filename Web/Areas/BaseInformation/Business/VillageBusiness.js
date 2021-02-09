var VillagePermissions;

$(function () {
    VillagePermissions = $('#permission-Village').val().split(',');
    
    if ($.inArray("/BaseInformation/Village/_List", VillagePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Village/_List', '', '#FormList-Village', 'ListVillageCallback();');
    }

    if ($.inArray("/BaseInformation/Village/_Create", VillagePermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Village/_Create', '', '#FormContainer-Village', 'CreateVillageCallback();');
    }

    EventHandlerVillage();
});

function CreateVillageCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateVillageCallback() {
    CheckValue();

    HandleValidation();
}

function ListVillageCallback() {
    Pager(1, 5, "Village", DataRefreshVillage(1, 5, $("#sort-Village").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerVillage() {
    $("#FormContainer-Village").on("submit", "#frm-Village", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Village/_Create', $('#frm-Village').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormVillage();

            if ($('#tbl-Village .page-record').val() == null)
                LoadDataVillage(1);
            else
                LoadDataVillage($('#tbl-Village .page-record').val());

            if ($.inArray("/BaseInformation/Village/_Create", VillagePermissions) == -1) {
                $('#FormContainer-Village').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-Village").on("click", "#frm-Village .btnNew", function () {
        ClearFormVillage();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Village").on("keypress", "#tbl-Village tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataVillage(1);
            return false;
        }
    });

    $("#FormList-Village").on("change keyup", "#tbl-Village tbody tr:first select", function (e) {
        LoadDataVillage(1);
    });

    $("#FormContainer-Village").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
            $("#Village_SectionId option").remove();
        }
        else {
            $("#CityId option").remove();
            $("#Village_SectionId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#Village_Code').removeData('previousValue');
        $('#Village_Code').valid();

        $('#Village_Title').removeData('previousValue');
        $('#Village_Title').valid();
    });

    $("#FormContainer-Village").on("change keyup", "#CityId", function () {
        if ($(this).val() != '') {
            GetSectionsByCityId("#Village_SectionId", $(this).val());
        }
        else {
            $("#Village_SectionId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#Village_Code').removeData('previousValue');
        $('#Village_Code').valid();

        $('#Village_Title').removeData('previousValue');
        $('#Village_Title').valid();
    });

    $("#FormContainer-Village").on("change keyup", "#Village_SectionId", function () {
        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#Village_Code').removeData('previousValue');
        $('#Village_Code').valid();

        $('#Village_Title').removeData('previousValue');
        $('#Village_Title').valid();
    });
}

function DataRefreshVillage(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Village').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Village/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Village tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان استان'>" + json[i].State.Title + "</td>");
            tr.append("<td data-th='عنوان شهرستان'>" + json[i].City.Title + "</td>");
            tr.append("<td data-th='عنوان بخش'>" + json[i].Section.Title + "</td>");
            tr.append("<td data-th='کد روستا'>" + json[i].Village.Code + "</td>");
            tr.append("<td data-th='عنوان روستا'>" + json[i].Village.Title + "</td>");

            if ($.inArray("/BaseInformation/Village/_Update", VillagePermissions) > -1 && $.inArray("/BaseInformation/Village/_Delete", VillagePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateVillage(" + json[i].Village.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteVillage'," + json[i].Village.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Village/_Update", VillagePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateVillage(" + json[i].Village.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Village/_Delete", VillagePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteVillage'," + json[i].Village.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Village tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Village/_Update", VillagePermissions) == -1 && $.inArray("/BaseInformation/Village/_Delete", VillagePermissions) == -1) {
            $('#tbl-Village th:last').remove();
            $('#tbl-Village tbody tr:first td:last').remove();
            $('#tbl-Village tfoot td').attr('colspan', $('#tbl-Village tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataVillage(pageRecord) {
    if ($.inArray("/BaseInformation/Village/_List", VillagePermissions) > -1) {
        var totalRecords = DataRefreshVillage(pageRecord, $('#tbl-Village .page-size').val(), $('#sort-Village').val());

        Pager(pageRecord, $('#tbl-Village .page-size').val(), "Village", totalRecords);
    }
}

function ClearFormVillage() {
    
    $('#frm-Village input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Village/_Create", VillagePermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Village').validate();
    $('#frm-Village').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateVillage(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Village/_Update', { Id: id }, '#FormContainer-Village', 'UpdateVillageCallback();');
}



function DeleteVillage(id) {
    Ajax('Post', '/BaseInformation/Village/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Village tbody tr').length != 2) {
            pageRecord = $('#tbl-Village .page-record').val();
        }
        else {
            if ($('#tbl-Village .page-record').val() != 1)
                pageRecord = $('#tbl-Village .page-record').val() - 1;
        }

        LoadDataVillage(pageRecord);
    }, 'json');
}

function GetCitiesByStateId(container, stateId) {
    Ajax('Post', '/BaseInformation/City/GetCitiesByStateId', 'stateId=' + stateId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}

function GetSectionsByCityId(container, stateId) {
    Ajax('Post', '/BaseInformation/Section/GetSectionsByCityId', 'cityId=' + stateId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}