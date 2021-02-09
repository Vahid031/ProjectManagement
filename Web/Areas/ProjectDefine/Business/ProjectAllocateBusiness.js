var ProjectAllocatePermissions;
var SelectedProject = -1;


$(function () {
    ProjectAllocatePermissions = $('#permission-ProjectAllocate').val().split(',');

    LoadPartialView('GET', '/ProjectDefine/ProjectAllocate/_Search', '', '#FormSearch-ProjectAllocate', 'SearchProjectAllocateCallback();');
    LoadPartialView('GET', '/ProjectDefine/ProjectAllocate/_List', '', '#FormList-ProjectAllocate', 'ListProjectAllocateCallback();');

    LoadPartialView('GET', '/ProjectDefine/ProjectAllocateDetail/_Index', '', '#ProjectAllocateDetail-ProjectAllocate');

    EventHandlerProjectAllocate();
});

function SearchProjectAllocateCallback() {
    HandleValidation();
}

function ListProjectAllocateCallback() {
    Pager(1, 5, "ProjectAllocate", DataRefreshProjectAllocate(1, 5, $("#sort-ProjectAllocate").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectAllocate() {

    $("#FormList-ProjectAllocate").on("keypress", "#tbl-ProjectAllocate tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectAllocate(1);
            return false;
        }
    });

    $("#FormList-ProjectAllocate").on("change keyup", "#tbl-ProjectAllocate tbody tr:first select", function (e) {
        LoadDataProjectAllocate(1);
    });

    $("#FormSearch-ProjectAllocate").on("change", "#FileCode, #ProjectTitle", function (e) {
        LoadDataProjectAllocate(1);
    });

    $("#FormSearch-ProjectAllocate").on("keyup", "#FileCode, #ProjectTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectAllocate").on("keyup", "input[name=FileCode], input[name=ProjectTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectAllocate").on("change keyup", "#ProjectTypeId, #StateId, #CityId, #ProjectStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectAllocate(1);
    });


    $("#FormSearch-ProjectAllocate").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });
}

function DataRefreshProjectAllocate(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectAllocate').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDefine/ProjectAllocate/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectAllocate tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {


            if (json[i].ProjectStateId == 2)
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' class='tr-change' />");
            else
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' />");

            tr.append("<td style='display:none;' data-th=''>" + json[i].Id + "</td>");
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد پروژه'>" + json[i].FileCode + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='نوع پروژه'>" + json[i].ProjectTypeTitle + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].StartFinantialYearTitle + "</td>");
            tr.append("<td data-th='پیش بینی تاریخ پایان'>" + json[i].ForecastEndFinantialYearTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهرستان'>" + json[i].CityTitle + "</td>");
            tr.append("<td style='display:none' data-th='@Html.DisplayNameFor(model => model.ProjectStateId)'>" + json[i].ProjectStateId + "</td>");
            //tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectStateTitle)'>" + json[i].ProjectStateTitle + "</td>");
            tr.append("<td style='display:none;' data-th='@Html.DisplayNameFor(model => model.Color)'>" + json[i].Color + "</td>");

            $('#tbl-ProjectAllocate tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }

        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}

function SelectProject() {
    $('#tbl-ProjectAllocate tbody tr').each(function (i, row) {
        if ($(this).find('td').eq(0).html() == SelectedProject) {
            $(this).addClass('selectedRow');
        }
    });
}


function ProjectSelect(selectedRow, Id) {
    $('#tbl-ProjectAllocate tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProject = Id;

    LoadDataProjectAllocateDetail(1);

    GetProjectPlanByProjectId(SelectedProject);
    GetProjectSectionByProjectId(SelectedProject);
}


function LoadDataProjectAllocate(pageRecord) {
    var totalRecords = DataRefreshProjectAllocate(pageRecord, $('#tbl-ProjectAllocate .page-size').val(), $('#sort-ProjectAllocate').val());
    Pager(pageRecord, $('#tbl-ProjectAllocate .page-size').val(), "ProjectAllocate", totalRecords);
}


function GetCitiesByStateId(container, stateId) {
    Ajax('Post', '/BaseInformation/City/GetCitiesByStateId', 'StateId=' + stateId, function (data, textStatus, xhr) {

        $(container + " option").remove();
        $(container).append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $(container).append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}


function GetProjectPlanByProjectId(projectId) {
    Ajax('Post', '/ProjectDefine/ProjectAllocateDetail/GetProjectPlanByProjectId', 'projectId=' + projectId, function (data, textStatus, xhr) {

        $("#ProjectAllocate_ProjectPlanId option").remove();
        $("#ProjectAllocate_ProjectPlanId").append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $("#ProjectAllocate_ProjectPlanId").append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}

function GetProjectSectionByProjectId(projcetId) {
    Ajax('Post', '/ProjectDevision/ProjectSection/GetProjectSectionByProjectId', 'projcetId=' + projcetId, function (data, textStatus, xhr) {

        $("#ProjectAllocate_ProjectSectionId option").remove();
        $("#ProjectAllocate_ProjectSectionId").append("<option value=>انتخاب کنید...</option>");

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $("#ProjectAllocate_ProjectSectionId").append("<option value='" + json[i].Id + "'>" + json[i].Title + "</option>");
        }

    }, 'json');
}