var ProjectPermissions;
var ProgramPlans = [];
var map;
var ProjectPlans;

$(function () {
    ProjectPermissions = $('#permission-Project').val().split(',');
    
    if ($.inArray("/ProjectDefine/Project/_List", ProjectPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/Project/_List', '', '#FormList-Project', 'ListProjectCallback();');
    }
    
    if ($.inArray("/ProjectDefine/Project/_Create", ProjectPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDefine/Project/_Create', '', '#FormContainer-Project', 'CreateProjectCallback();');
    }
    
    EventHandlerProject();

    ProjectPlans = '';
});

function CreateProjectCallback() {
    CheckValue();
    HandleValidation();

    SepratePrice('#Project_EstimateCost');
}

function UpdateProjectCallback() {
    CreateProjectCallback();

    DisableForm();

    //Get ProjectPlans That Exists
    ProjectPlans = '';

    Ajax('Post', '/ProjectDefine/Project/GetProjectPlanByProjectId', 'projectId=' + $('#Id').val(), function (data, textStatus, xhr) {
        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            var creditProvidePlaceTitle = '';
            var fromFinantialYearTitle = '';
            var toFinantialYearTitle = '';

            if (json[i].CreditProvidePlaceTitle != null)
                creditProvidePlaceTitle = json[i].CreditProvidePlaceTitle;

            if (json[i].FromFinantialYearTitle != null)
                fromFinantialYearTitle = json[i].FromFinantialYearTitle;

            if (json[i].ToFinantialYearTitle != null)
                toFinantialYearTitle = json[i].ToFinantialYearTitle;

            ProjectPlans += 'ProgramId:' + json[i].ProgramId + ',ProgramTitle:' + json[i].ProgramTitle + ',ProgramPlanId:' + json[i].ProgramPlanId + ',ProgramPlanTitle:' + json[i].ProgramPlanTitle + ',CreditProvidePlaceId:' + json[i].CreditProvidePlaceId + ',CreditProvidePlaceTitle:' + creditProvidePlaceTitle + ',FromFinantialYearId:' + json[i].FromFinantialYearId + ',FromFinantialYearTitle:' + fromFinantialYearTitle + ',ToFinantialYearId:' + json[i].ToFinantialYearId + ',ToFinantialYearTitle:' + toFinantialYearTitle + '|';
        }
        
    }, 'json');
}

function ListProjectCallback() {
    Pager(1, 5, "Project", DataRefreshProject(1, 5, $("#sort-Project").val()));

    HandleValidation();

    SortArrow();
}

function DropDownSelected(id, code, title) {

    if (id != null) {
        $('#Project_ProgramPlanId').val(id);
        $('#ProgramPlanTitle').val(title + ' (' + code + ')');
    }
    else {
        $('#Project_ProgramPlanId').val('');
        $('#ProgramPlanTitle').val('انتخاب کنید...');
    }

    $('#ProgramPlanTitle').removeData('previousValue');
    $('#ProgramPlanTitle').valid();
}


function EventHandlerProject() {
    $("#FormContainer-Project").on("submit", "#frm-Project", function (e) {
        e.preventDefault();

        $('#Project_EstimateCost').val($('#Project_EstimateCost').val().replace(/\,/g, ''));

        Ajax('Post', '/ProjectDefine/Project/_Create', 'Files=' + $('#images-Project').val() + "&ProjectPlans=" + ProjectPlans + "&" + $('#frm-Project').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProject();

            if ($('#tbl-Project .page-record').val() == null)
                LoadDataProject(1);
            else
                LoadDataProject($('#tbl-Project .page-record').val());

            if ($.inArray("/ProjectDefine/Project/_Create", ProjectPermissions) == -1) {
                $('#FormContainer-Project').fadeOut('fast');
            }

        }, 'json');
    });

    $("#FormContainer-Project").on("click", "#frm-Project .btnNew", function () {
        ClearFormProject();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Project").on("keypress", "#tbl-Project tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProject(1);
            return false;
        }
    });

    $("#FormList-Project").on("change keyup", "#tbl-Project tbody tr:first select", function (e) {
        LoadDataProject(1);
    });


    $("#FormContainer-Project").on("keypress", "#ProgramPlanTitle", function () {
        $('#Project_ProgramPlanId').val('');

        $('#ProgramPlanTitle').removeData('previousValue');
        $('#ProgramPlanTitle').valid();
    });

    $("#FormContainer-Project").on("change keyup", "#Project_StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#Project_CityId", $(this).val());
            $("#Project_SectionId option").remove();
            $("#Project_VillageId option").remove();
            $("#Project_RuralDistrictId option").remove();
        }
        else {
            $("#Project_CityId option").remove();
            $("#Project_SectionId option").remove();
            $("#Project_VillageId option").remove();
            $("#Project_RuralDistrictId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });


    $("#FormContainer-Project").on("change keyup", "#Project_CityId", function () {
        if ($(this).val() != '') {
            GetSectionsByCityId("#Project_SectionId", $(this).val());
            $("#Project_VillageId option").remove();
            $("#Project_RuralDistrictId option").remove();
        }
        else {
            $("#Project_SectionId option").remove();
            $("#Project_VillageId option").remove();
            $("#Project_RuralDistrictId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });

    $("#FormContainer-Project").on("change keyup", "#Project_SectionId", function () {
        if ($(this).val() != '') {
            GetVillagesBySectionId("#Project_VillageId", $(this).val());
            $("#Project_RuralDistrictId option").remove();
        }
        else {
            $("#Project_VillageId option").remove();
            $("#Project_RuralDistrictId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });

    $("#FormContainer-Project").on("change keyup", "#Project_VillageId", function () {
        if ($(this).val() != '') {
            GetRuralDistrictByVillageId("#Project_RuralDistrictId", $(this).val());
        }
        else {
            $("#Project_RuralDistrictId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();
    });


    $("#FormContainer-Project").on("change keyup", "#Project_ProjectTypeId", function () {
        DisableForm();
    });

    $("#FormContainer-Project").on("click", "#btnShowMap", function () {
        PopupFormHtml("نقشه", "/ProjectDefine/Project/_Map", "ShowMap('" + $('#Project_CityId option:selected').text() + "', 9);", true, "YesPopupClick();")
    });

    $("#FormContainer-Project").on("click", "#btnShowFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDefine/Project/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-Project").on("click", "#btnAddPlan", function () {
        PopupFormHtml("انتخاب طرح های پروژه", "/ProjectDefine/Project/_Plan", "PlanCallBack()", true, "YesPlanPopupClick();")
    });
}


function PlanCallBack() {
    GetDropDown("ProgramPlanTitle", "DropDownPanel", "/ProjectDefine/ProgramPlan/GetProgramPlans", "programId=", "$('#Project_ProgramId').val()", "frm-ProjectPlan", "DropDownSelected", ProgramPlans);

    $("#frm-ProjectPlan").on("change keyup", "#Project_ProgramId", function () {
        resetVariable = true;
        
        $('#Project_ProgramPlanId').val('');
        $('#ProgramPlanTitle').val('انتخاب کنید...');
    });



    $("#frm-ProjectPlan").on("click", "#btnAddProjectPlan", function (e) {
        e.preventDefault();
        if ($('#Project_ProgramId').val() == '') {
            $('#Project_ProgramId').addClass('input-validation-error');
            Messages('danger', 'مقادیر را به درستی پر کنید');
        }

        if ($('#Project_ProgramPlanId').val() == '')
        {
            $('#ProgramPlanTitle').addClass('input-validation-error');
            Messages('danger', 'مقادیر را به درستی پر کنید');
        }
        if ($('#Project_ProgramId').val() != '' && $('#Project_ProgramPlanId').val() != '') {
            var tr = $('<tr/>');
            var rowId = $('#tbl-ProjectPlan tbody tr').length + 1;

            tr.append("<td data-th='ردیف'>" + rowId + "</td>");
            tr.append("<td style='display:none' data-th='برنامه'>" + $('#Project_ProgramId').val() + "</td>");
            tr.append("<td data-th='برنامه'>" + $('#Project_ProgramId option:selected').text() + "</td>");

            tr.append("<td style='display:none' data-th='طرح برنامه'>" + $('#Project_ProgramPlanId').val() + "</td>");
            tr.append("<td data-th='طرح برنامه'>" + $('#ProgramPlanTitle').val() + "</td>");

            tr.append("<td style='display:none' data-th='محل تامین اعتبار'>" + $('#ddlCreditProvidePlaceId').val() + "</td>");

            if ($('#ddlCreditProvidePlaceId').val() != '')
                tr.append("<td data-th='محل تامین اعتبار'>" + $('#ddlCreditProvidePlaceId option:selected').text() + "</td>");
            else
                tr.append("<td data-th='محل تامین اعتبار'></td>");


            tr.append("<td style='display:none' data-th='از سال'>" + $('#ddlFromYear').val() + "</td>");

            if ($('#ddlFromYear').val() != '')
                tr.append("<td data-th='از سال'>" + $('#ddlFromYear option:selected').text() + "</td>");
            else
                tr.append("<td data-th='از سال'></td>");

            tr.append("<td style='display:none' data-th='تا سال'>" + $('#ddlToYear').val() + "</td>");


            if ($('#ddlToYear').val() != '')
                tr.append("<td data-th='تا سال'>" + $('#ddlToYear option:selected').text() + "</td>");
            else
                tr.append("<td data-th='تا سال'></td>");

            tr.append("<td data-th='حذف'><a onmousedown = \"DeletePlan(" + rowId + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");

            $('#tbl-ProjectPlan tbody').append(tr);
        }
    });

    //if (ProjectPlans == '')
    //    ProjectPlans = 'MajorTypeId:1,MajorTypeTitle:sssss,LevelTypeId:2,LevelTypeTitle:ddddd,FinishCreditDate:1391/01/01|MajorTypeId:1,MajorTypeTitle:v  c c c ,LevelTypeId:2,LevelTypeTitle:wqwewewqe,FinishCreditDate:1391/01/01|MajorTypeId:1,MajorTypeTitle:aaasd,LevelTypeId:2,LevelTypeTitle:fff,FinishCreditDate:1392/01/01|'
    //    ProjectPlans += 'ProgramId:' + json[i].ProgramId + ',ProgramTitle:' + json[i].ProgramTitle + ',ProgramPlanId:' + json[i].ProgramPlanId + ',ProgramPlanTitle:' + json[i].ProgramPlanTitle + ',CreditProvidePlaceId:' + json[i].CreditProvidePlaceId + ',CreditProvidePlaceTitle:' + json[i].CreditProvidePlaceTitle + ',FromFinantialYearId:' + json[i].FromFinantialYearId + ',FromFinantialYearTitle:' + json[i].FromFinantialYearTitle + ',ToFinantialYearId:' + json[i].ToFinantialYearId + ',ToFinantialYearTitle:' + json[i].ToFinantialYearTitle + '|';
    $(ProjectPlans.split('|')).each(function (i, row) {
        if (row != '') {
            var tr = $('<tr/>');
            var rowId = $('#tbl-ProjectPlan tbody tr').length + 1;
            
            tr.append("<td data-th='ردیف'>" + rowId + "</td>");
            tr.append("<td style='display:none' data-th='برنامه'>" + row.split(',')[0].split(':')[1] + "</td>");
            tr.append("<td data-th='برنامه'>" + row.split(',')[1].split(':')[1] + "</td>");

            tr.append("<td style='display:none' data-th='طرح برنامه'>" + row.split(',')[2].split(':')[1] + "</td>");
            tr.append("<td data-th='طرح برنامه'>" + row.split(',')[3].split(':')[1] + "</td>");

            if (row.split(',')[4].split(':')[1] != 'null') {
                tr.append("<td style='display:none' data-th='محل تامین اعتبار'>" + row.split(',')[4].split(':')[1] + "</td>");
                tr.append("<td data-th='محل تامین اعتبار'>" + row.split(',')[5].split(':')[1] + "</td>");
            }
            else {
                tr.append("<td style='display:none' data-th='محل تامین اعتبار'></td>");
                tr.append("<td data-th='محل تامین اعتبار'></td>");
            }

            if (row.split(',')[6].split(':')[1] != 'null') {
                tr.append("<td style='display:none' data-th='از سال'>" + row.split(',')[6].split(':')[1] + "</td>");
                tr.append("<td data-th='از سال'>" + row.split(',')[7].split(':')[1] + "</td>");
            }
            else {
                tr.append("<td style='display:none' data-th='از سال'></td>");
                tr.append("<td data-th='از سال'></td>");
            }
            if (row.split(',')[8].split(':')[1] != 'null') {
                tr.append("<td style='display:none' data-th='تا سال'>" + row.split(',')[8].split(':')[1] + "</td>");
                tr.append("<td data-th='تا سال'>" + row.split(',')[9].split(':')[1] + "</td>");
            }
            else {
                tr.append("<td style='display:none' data-th='تا سال'></td>");
                tr.append("<td data-th='تا سال'></td>");
            }
            tr.append("<td data-th='حذف'><a onmousedown = \"DeletePlan(" + rowId + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            
            $('#tbl-ProjectPlan tbody').append(tr);
        }
    });
}

function YesPlanPopupClick() {
    ProjectPlans = '';

    $('#tbl-ProjectPlan tbody tr').each(function (i, row) {
        var ProgramId = $(row).find('td').eq(1).html();
        var ProgramTitle = $(row).find('td').eq(2).html();
        var ProgramPlanId = $(row).find('td').eq(3).html();
        var ProgramPlanTitle = $(row).find('td').eq(4).html();
        var CreditProvidePlaceId = $(row).find('td').eq(5).html();
        var CreditProvidePlaceTitle = $(row).find('td').eq(6).html();
        var FromFinantialYearId = $(row).find('td').eq(7).html();
        var FromFinantialYearTitle = $(row).find('td').eq(8).html();
        var ToFinantialYearId = $(row).find('td').eq(9).html();
        var ToFinantialYearTitle = $(row).find('td').eq(10).html();
        ProjectPlans += 'ProgramId:' + ProgramId + ',ProgramTitle:' + ProgramTitle + ',ProgramPlanId:' + ProgramPlanId + ',ProgramPlanTitle:' + ProgramPlanTitle + ',CreditProvidePlaceId:' + CreditProvidePlaceId + ',CreditProvidePlaceTitle:' + CreditProvidePlaceTitle + ',FromFinantialYearId:' + FromFinantialYearId + ',FromFinantialYearTitle:' + FromFinantialYearTitle + ',ToFinantialYearId:' + ToFinantialYearId + ',ToFinantialYearTitle:' + ToFinantialYearTitle + '|';
    });

    if (ProjectPlans == '')
        ProjectPlans = '|';
}

function DeletePlan(rowId) {
    $('#tbl-ProjectPlan tbody tr').each(function (i, row) {
        if ($(row).find('td').eq(0).html() == rowId) {
            $(row).remove();
        }
    });

    $('#tbl-ProjectPlan tbody tr').each(function (i, row) {
        $(row).find('td').eq(0).html(i + 1);
        $(row).find('td').eq(11).html("<a onmousedown = \"DeletePlan(" + (i + 1) + ")\" title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a>");
    });
}


function YesFilePopupClick() {
    $('#images-Project').val('');
    $('.projectFiles').each(function (i, row) {
        $('#images-Project').val($('#images-Project').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#fileupload').fileupload();

    $('#fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetByProjectId($('#Id').val());
}


function YesPopupClick() {
    $('#Project_LocationX').val(map.markers[0].getPosition().lat());
    $('#Project_LocationY').val(map.markers[0].getPosition().lng());
}

function ShowMap(location, zoom) {
    try
    {
        map = new GMaps({
            el: '#map',
            lat: 35.8392478,
            lng: 50.9434751,
            zoom: zoom,
            zoomControl: true,
            zoomControlOpt: {
                style: 'SMALL',
                position: 'TOP_LEFT'
            },
            panControl: false,
            streetViewControl: false,
            mapTypeControl: false,
            overviewMapControl: false
        });

        GMaps.geocode({
            address: location,
            callback: function (results, status) {
                if (status == 'OK') {
                    var latlng = results[0].geometry.location;
                    map.setCenter(latlng.lat(), latlng.lng());
                }
            }
        });

        if ($('#Project_LocationX').val() != '' && $('#Project_LocationY').val() != '') {
            map.addMarker({
                lat: $('#Project_LocationX').val(),
                lng: $('#Project_LocationY').val(),
                draggable: true,
                icon: 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png',
            });
        }
        else {
            var gc = new google.maps.Geocoder(),
                opts = { 'address': location };

            gc.geocode(opts, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var loc = results[0].geometry.location

                    map.addMarker({
                        lat: loc.lat(),
                        lng: loc.lng(),
                        draggable: true,
                        icon: 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png',
                    });
                }
            });
        }
    }
    catch(err){}
}

function DisableForm() {
    if ($("#Project_ProjectTypeId").val() != 1) {
        $("#ColOwnershipTypeId, #ColNumber, #ColAddress, #ColGasNumber, #ColPowerNumber, #ColWaterNumber, #ColLocationX, #ColLocationY, #mapContainer").css('display', 'none');
        $("#Project_OwnershipTypeId, #Project_Number, #Project_Address, #Project_GasNumber, #Project_PowerNumber, #Project_WaterNumber, #Project_LocationX, #Project_LocationY, #mapContainer").prop('disabled', true);
    }
    else {
        $("#ColOwnershipTypeId, #ColNumber, #ColAddress, #ColGasNumber, #ColPowerNumber, #ColWaterNumber, #ColLocationX, #ColLocationY, #mapContainer").css('display', 'block');
        $("#Project_OwnershipTypeId, #Project_Number, #Project_Address, #Project_GasNumber, #Project_PowerNumber, #Project_WaterNumber, #Project_LocationX, #Project_LocationY, #mapContainer").prop('disabled', false);
    }
}

function DataRefreshProject(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Project').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDefine/Project/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Project tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهر'>" + json[i].CityTitle + "</td>");
            tr.append("<td data-th='سال شروع'>" + json[i].StartFinantialYearTitle + "</td>");
            tr.append("<td data-th='پیش بینی سال خاتمه'>" + json[i].ForecastEndFinantialYearTitle + "</td>");
            tr.append("<td data-th='مساحت'>" + json[i].Area + "</td>");

            if ($.inArray("/ProjectDefine/Project/_Update", ProjectPermissions) > -1 && $.inArray("/ProjectDefine/Project/_Delete", ProjectPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProject(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProject'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/Project/_Update", ProjectPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProject(" + json[i].Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDefine/Project/_Delete", ProjectPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProject'," + json[i].Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Project tbody').append(tr);
        }


        if ($.inArray("/ProjectDefine/Project/_Update", ProjectPermissions) == -1 && $.inArray("/ProjectDefine/Project/_Delete", ProjectPermissions) == -1) {
            $('#tbl-Project th:last').remove();
            $('#tbl-Project tbody tr:first td:last').remove();
            $('#tbl-Project tfoot td').attr('colspan', $('#tbl-Project tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProject(pageRecord) {
    if ($.inArray("/ProjectDefine/Project/_List", ProjectPermissions) > -1) {
        var totalRecords = DataRefreshProject(pageRecord, $('#tbl-Project .page-size').val(), $('#sort-Project').val());

        Pager(pageRecord, $('#tbl-Project .page-size').val(), "Project", totalRecords);
    }
}

function ClearFormProject() {

    $('#frm-Project input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });
    ReloadFiles();

    if ($.inArray("/ProjectDefine/Project/_Create", ProjectPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-Project').validate();
    $('#frm-Project').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();

    ProjectPlans = '';
}


function UpdateProject(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDefine/Project/_Update', { Id: id }, '#FormContainer-Project', 'UpdateProjectCallback();');
}



function DeleteProject(id) {
    Ajax('Post', '/ProjectDefine/Project/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Project tbody tr').length != 2) {
            pageRecord = $('#tbl-Project .page-record').val();
        }
        else {
            if ($('#tbl-Project .page-record').val() != 1)
                pageRecord = $('#tbl-Project .page-record').val() - 1;
        }

        LoadDataProject(pageRecord);
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

function GetRuralDistrictByVillageId(container, villageId) {
    Ajax('Post', '/BaseInformation/RuralDistrict/GetRuralDistrictByVillageId', 'villageId=' + villageId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}

function GetByProjectId(projectId) {
    Ajax('Post', '/ProjectDefine/Project/GetProjectFiles', 'projectId=' + projectId, function (data, textStatus, xhr) {
        var json = JSON.parse(data.Values);
        var files = [];

        for (var i = 0; i < json.length; i++) {
            files.push({
                "url": json[i].url,
                "thumbnail_url": json[i].thumbnail_url,
                "name": json[i].name,
                "type": json[i].type,
                "size": json[i].size,
                "delete_url": json[i].delete_url + "/" + json[i].imageId,
                "delete_type": json[i].delete_type,
                "imageId": json[i].imageId
            });
        }
        
        $('#fileupload').fileupload('option', 'done').call($('#fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadFiles() {
    Ajax('Post', '/ProjectDefine/Project/ReloadFiles', {}, function (data, textStatus, xhr) { });
}