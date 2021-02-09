var ProjectAgreementPermissions;
var SelectedProject = -1;


$(function () {
    ProjectAgreementPermissions = $('#permission-ProjectAgreement').val().split(',');

    LoadPartialView('GET', '/ProjectDefine/ProjectAgreement/_Search', '', '#FormSearch-ProjectAgreement', 'SearchProjectAgreementCallback();');
    LoadPartialView('GET', '/ProjectDefine/ProjectAgreement/_List', '', '#FormList-ProjectAgreement', 'ListProjectAgreementCallback();');

    if ($.inArray("/ProjectDefine/ProjectFunding/_Index", ProjectAgreementPermissions) == -1) {
        $('#lprojectFunding').css('display', 'none');
    }

    if ($.inArray("/ProjectDefine/ProjectInvestmentChapter/_Index", ProjectAgreementPermissions) == -1) {
         $('#lprojectInvestmentChapter').css('display', 'none');
    }

    if ($.inArray("/ProjectDefine/ProjectQuantityGoal/_Index", ProjectAgreementPermissions) == -1) {
         $('#lprojectQuantityGoal').css('display', 'none');
    }

    if ($.inArray("/ProjectDefine/ProjectAverage/_Index", ProjectAgreementPermissions) == -1) {
         $('#lprojectAverage').css('display', 'none');
    }

    // --------------- در صورت انتخاب نشدن فاز پروژه در گرید بالا خطا نمایش میدهد
    $('#FormTabs-ProjectAgreement').on("click", ".nav-pills", function () {
        if (SelectedProject == -1)
            Messages('warning', 'فازی انتخاب نشده است');
    });


    
    EventHandlerProjectAgreement();
});

function SearchProjectAgreementCallback() {
    HandleValidation();
}

function ListProjectAgreementCallback() {
    Pager(1, 5, "ProjectAgreement", DataRefreshProjectAgreement(1, 5, $("#sort-ProjectAgreement").val()));

    HandleValidation();

    SortArrow();
}



function EventHandlerProjectAgreement() {

    $("#FormList-ProjectAgreement").on("keypress", "#tbl-ProjectAgreement tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectAgreement(1);
            return false;
        }
    });

    $("#FormList-ProjectAgreement").on("change keyup", "#tbl-ProjectAgreement tbody tr:first select", function (e) {
        LoadDataProjectAgreement(1);
    });

    $("#FormSearch-ProjectAgreement").on("change", "#FileCode, #ProjectTitle", function (e) {
        LoadDataProjectAgreement(1);
    });

    $("#FormSearch-ProjectAgreement").on("keyup", "#FileCode, #ProjectTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectAgreement").on("keyup", "input[name=FileCode], input[name=ProjectTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectAgreement").on("change keyup", "#ProjectTypeId, #StateId, #CityId, #ProjectStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectAgreement(1);
    });


    $("#FormSearch-ProjectAgreement").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectAgreement').on("click", "#lprojectFunding", function () {
        if (SelectedProject != -1)
            LoadPartialView('GET', '/ProjectDefine/ProjectFunding/_Index', '', '#ProjectFunding-ProjectAgreement');
        else
            Messages('warning', 'پروژه انتخاب نشده است');
    });

    $('#FormTabs-ProjectAgreement').on("click", "#lprojectInvestmentChapter", function () {
        if (SelectedProject != -1)
            LoadPartialView('GET', '/ProjectDefine/ProjectInvestmentChapter/_Index', '', '#ProjectInvestmentChapter-ProjectAgreement');
        else
            Messages('warning', 'پروژه انتخاب نشده است');
    });

    $('#FormTabs-ProjectAgreement').on("click", "#lprojectQuantityGoal", function () {
        if (SelectedProject != -1)
            LoadPartialView('GET', '/ProjectDefine/ProjectQuantityGoal/_Index', '', '#ProjectQuantityGoal-ProjectAgreement');
        else
            Messages('warning', 'پروژه انتخاب نشده است');
    });

    $('#FormTabs-ProjectAgreement').on("click", "#lprojectAverage", function () {
        if (SelectedProject != -1)
            LoadPartialView('GET', '/ProjectDefine/ProjectAverage/_Index', '', '#ProjectAverage-ProjectAgreement');
        else
            Messages('warning', 'پروژه انتخاب نشده است');
    });
}

function DataRefreshProjectAgreement(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectAgreement').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDefine/ProjectAgreement/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectAgreement tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {


            if (json[i].ProjectStateId == 1)
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' class='tr-change' />");
            else
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' />");

            tr.append("<td style='display:none' data-th=''>" + json[i].Id + "</td>");
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

            $('#tbl-ProjectAgreement tbody').append(tr);
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
    $('#tbl-ProjectAgreement tbody tr').each(function (i, row) {
        if ($(this).find('td').eq(0).html() == SelectedProject) {
            $(this).addClass('selectedRow');
        }
    });
}


function ProjectSelect(selectedRow, Id) {
    $('#tbl-ProjectAgreement tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProject = Id;

    $('#FormTabs-ProjectAgreement .active').removeClass('active');
}


function LoadDataProjectAgreement(pageRecord) {
    var totalRecords = DataRefreshProjectAgreement(pageRecord, $('#tbl-ProjectAgreement .page-size').val(), $('#sort-ProjectAgreement').val());
    Pager(pageRecord, $('#tbl-ProjectAgreement .page-size').val(), "ProjectAgreement", totalRecords);
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