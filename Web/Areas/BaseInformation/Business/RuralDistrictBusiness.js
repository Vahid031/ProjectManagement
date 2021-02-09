var RuralDistrictPermissions;

$(function () {
    RuralDistrictPermissions = $('#permission-RuralDistrict').val().split(',');
    
    if ($.inArray("/BaseInformation/RuralDistrict/_List", RuralDistrictPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/RuralDistrict/_List', '', '#FormList-RuralDistrict', 'ListRuralDistrictCallback();');
    }

    if ($.inArray("/BaseInformation/RuralDistrict/_Create", RuralDistrictPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/RuralDistrict/_Create', '', '#FormContainer-RuralDistrict', 'CreateRuralDistrictCallback();');
    }

    EventHandlerRuralDistrict();
});

function CreateRuralDistrictCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateRuralDistrictCallback() {
    CheckValue();

    HandleValidation();
}

function ListRuralDistrictCallback() {
    Pager(1, 5, "RuralDistrict", DataRefreshRuralDistrict(1, 5, $("#sort-RuralDistrict").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerRuralDistrict() {
    $("#FormContainer-RuralDistrict").on("submit", "#frm-RuralDistrict", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/RuralDistrict/_Create', $('#frm-RuralDistrict').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormRuralDistrict();

            if ($('#tbl-RuralDistrict .page-record').val() == null)
                LoadDataRuralDistrict(1);
            else
                LoadDataRuralDistrict($('#tbl-RuralDistrict .page-record').val());

            if ($.inArray("/BaseInformation/RuralDistrict/_Create", RuralDistrictPermissions) == -1) {
                $('#FormContainer-RuralDistrict').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-RuralDistrict").on("click", "#frm-RuralDistrict .btnNew", function () {
        ClearFormRuralDistrict();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-RuralDistrict").on("keypress", "#tbl-RuralDistrict tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataRuralDistrict(1);
            return false;
        }
    });

    $("#FormList-RuralDistrict").on("change keyup", "#tbl-RuralDistrict tbody tr:first select", function (e) {
        LoadDataRuralDistrict(1);
    });


    $("#FormContainer-RuralDistrict").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
            $("#SectionId option").remove();
            $("#RuralDistrict_VillageId option").remove();
        }
        else {
            $("#CityId option").remove();
            $("#SectionId option").remove();
            $("#RuralDistrict_VillageId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#RuralDistrict_Code').removeData('previousValue');
        $('#RuralDistrict_Code').valid();

        $('#RuralDistrict_Title').removeData('previousValue');
        $('#RuralDistrict_Title').valid();
    });


    $("#FormContainer-RuralDistrict").on("change keyup", "#CityId", function () {
        if ($(this).val() != '') {
            GetSectionsByCityId("#SectionId", $(this).val());
            $("#RuralDistrict_VillageId option").remove();
        }
        else {
            $("#SectionId option").remove();
            $("#RuralDistrict_VillageId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#RuralDistrict_Code').removeData('previousValue');
        $('#RuralDistrict_Code').valid();

        $('#RuralDistrict_Title').removeData('previousValue');
        $('#RuralDistrict_Title').valid();
    });

    $("#FormContainer-RuralDistrict").on("change keyup", "#SectionId", function () {
        if ($(this).val() != '') {
            GetVillagesBySectionId("#RuralDistrict_VillageId", $(this).val());
        }
        else {
            $("#RuralDistrict_VillageId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#RuralDistrict_Code').removeData('previousValue');
        $('#RuralDistrict_Code').valid();

        $('#RuralDistrict_Title').removeData('previousValue');
        $('#RuralDistrict_Title').valid();
    });

    $("#FormContainer-RuralDistrict").on("change keyup", "#RuralDistrict_VillageId", function () {
        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#RuralDistrict_Code').removeData('previousValue');
        $('#RuralDistrict_Code').valid();

        $('#RuralDistrict_Title').removeData('previousValue');
        $('#RuralDistrict_Title').valid();
    });
}

function DataRefreshRuralDistrict(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-RuralDistrict').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/RuralDistrict/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-RuralDistrict tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان استان'>" + json[i].State.Title + "</td>");
            tr.append("<td data-th='عنوان شهرستان'>" + json[i].City.Title + "</td>");
            tr.append("<td data-th='عنوان بخش'>" + json[i].Section.Title + "</td>");
            tr.append("<td data-th='عنوان روستا'>" + json[i].Village.Title + "</td>");
            tr.append("<td data-th='کد دهستان'>" + json[i].RuralDistrict.Code + "</td>");
            tr.append("<td data-th='عنوان دهستان'>" + json[i].RuralDistrict.Title + "</td>");

            if ($.inArray("/BaseInformation/RuralDistrict/_Update", RuralDistrictPermissions) > -1 && $.inArray("/BaseInformation/RuralDistrict/_Delete", RuralDistrictPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateRuralDistrict(" + json[i].RuralDistrict.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteRuralDistrict'," + json[i].RuralDistrict.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/RuralDistrict/_Update", RuralDistrictPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateRuralDistrict(" + json[i].RuralDistrict.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/RuralDistrict/_Delete", RuralDistrictPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteRuralDistrict'," + json[i].RuralDistrict.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-RuralDistrict tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/RuralDistrict/_Update", RuralDistrictPermissions) == -1 && $.inArray("/BaseInformation/RuralDistrict/_Delete", RuralDistrictPermissions) == -1) {
            $('#tbl-RuralDistrict th:last').remove();
            $('#tbl-RuralDistrict tbody tr:first td:last').remove();
            $('#tbl-RuralDistrict tfoot td').attr('colspan', $('#tbl-RuralDistrict tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataRuralDistrict(pageRecord) {
    if ($.inArray("/BaseInformation/RuralDistrict/_List", RuralDistrictPermissions) > -1) {
        var totalRecords = DataRefreshRuralDistrict(pageRecord, $('#tbl-RuralDistrict .page-size').val(), $('#sort-RuralDistrict').val());

        Pager(pageRecord, $('#tbl-RuralDistrict .page-size').val(), "RuralDistrict", totalRecords);
    }
}

function ClearFormRuralDistrict() {
    
    $('#frm-RuralDistrict input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/RuralDistrict/_Create", RuralDistrictPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-RuralDistrict').validate();
    $('#frm-RuralDistrict').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateRuralDistrict(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/RuralDistrict/_Update', { Id: id }, '#FormContainer-RuralDistrict', 'UpdateRuralDistrictCallback();');
}



function DeleteRuralDistrict(id) {
    Ajax('Post', '/BaseInformation/RuralDistrict/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-RuralDistrict tbody tr').length != 2) {
            pageRecord = $('#tbl-RuralDistrict .page-record').val();
        }
        else {
            if ($('#tbl-RuralDistrict .page-record').val() != 1)
                pageRecord = $('#tbl-RuralDistrict .page-record').val() - 1;
        }

        LoadDataRuralDistrict(pageRecord);
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

function GetSectionsByCityId(container, cityId) {
    Ajax('Post', '/BaseInformation/Section/GetSectionsByCityId', 'cityId=' + cityId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}

function GetVillagesBySectionId(container, sectionId) {
    Ajax('Post', '/BaseInformation/Village/GetVillagesBySectionId', 'sectionId=' + sectionId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}