var ProjectProgrammingPermissions;
var SelectedProject = -1;


$(function () {
    ProjectProgrammingPermissions = $('#permission-ProjectProgramming').val().split(',');

    LoadPartialView('GET', '/ProjectDefine/ProjectProgramming/_Search', '', '#FormSearch-ProjectProgramming', 'SearchProjectProgrammingCallback();');
    LoadPartialView('GET', '/ProjectDefine/ProjectProgramming/_List', '', '#FormList-ProjectProgramming', 'ListProjectProgrammingCallback();');

    LoadPartialView('GET', '/ProjectDevision/ProjectSection/_Index', '', '#ProjectSection-ProjectProgramming');

    EventHandlerProjectProgramming();
});

function SearchProjectProgrammingCallback() {
    HandleValidation();
}

function ListProjectProgrammingCallback() {
    Pager(1, 5, "ProjectProgramming", DataRefreshProjectProgramming(1, 5, $("#sort-ProjectProgramming").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectProgramming() {

    $("#FormList-ProjectProgramming").on("keypress", "#tbl-ProjectProgramming tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectProgramming(1);
            return false;
        }
    });

    $("#FormList-ProjectProgramming").on("change keyup", "#tbl-ProjectProgramming tbody tr:first select", function (e) {
        LoadDataProjectProgramming(1);
    });

    $("#FormSearch-ProjectProgramming").on("change", "#FileCode, #ProjectTitle", function (e) {
        LoadDataProjectProgramming(1);
    });

    $("#FormSearch-ProjectProgramming").on("keyup", "#FileCode, #ProjectTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectProgramming").on("keyup", "input[name=FileCode], input[name=ProjectTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectProgramming").on("change keyup", "#ProjectTypeId, #StateId, #CityId, #ProjectStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectProgramming(1);
    });


    $("#FormSearch-ProjectProgramming").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });
}

function DataRefreshProjectProgramming(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectProgramming').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDefine/ProjectProgramming/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectProgramming tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {


            if (json[i].ProjectStateId == 3)
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' class='tr-change' />");
            else
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' />");

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کدپروژه'>" + json[i].FileCode + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='نوع پروژه'>" + json[i].ProjectTypeTitle + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].StartFinantialYearTitle + "</td>");
            tr.append("<td data-th='پیش بینی تاریخ پایان'>" + json[i].ForecastEndFinantialYearTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهرستان'>" + json[i].CityTitle + "</td>");
            tr.append("<td style='display:none' data-th='@Html.DisplayNameFor(model => model.ProjectStateId)'>" + json[i].ProjectStateId + "</td>");
            //tr.append("<td data-th='@Html.DisplayNameFor(model => model.ProjectStateTitle)'>" + json[i].ProjectStateTitle + "</td>");
            tr.append("<td style='display:none;' data-th='@Html.DisplayNameFor(model => model.Color)'>" + json[i].Color + "</td>");

            $('#tbl-ProjectProgramming tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }

        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}


function ProjectSelect(selectedRow, Id) {
    $('#tbl-ProjectProgramming tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProject = Id;

    LoadDataProjectSection(1);
}


function LoadDataProjectProgramming(pageRecord) {
    var totalRecords = DataRefreshProjectProgramming(pageRecord, $('#tbl-ProjectProgramming .page-size').val(), $('#sort-ProjectProgramming').val());
    Pager(pageRecord, $('#tbl-ProjectProgramming .page-size').val(), "ProjectProgramming", totalRecords);
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