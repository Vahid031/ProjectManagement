var ProjectSectionOperationPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionOperationPermissions = $('#permission-ProjectSectionOperation').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionOperation/_Search', '', '#FormSearch-ProjectSectionOperation', 'SearchProjectSectionOperationCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionOperation/_List', '', '#FormList-ProjectSectionOperation', 'ListProjectSectionOperationCallback();');

    EventHandlerProjectSectionOperation();
});

function SearchProjectSectionOperationCallback() {
    HandleValidation();
}

function ListProjectSectionOperationCallback() {
    Pager(1, 5, "ProjectSectionOperation", DataRefreshProjectSectionOperation(1, 5, $("#sort-ProjectSectionOperation").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionOperation() {
    $("#FormList-ProjectSectionOperation").on("keypress", "#tbl-ProjectSectionOperation tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionOperation(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionOperation").on("change keyup", "#tbl-ProjectSectionOperation tbody tr:first select", function (e) {
        LoadDataProjectSectionOperation(1);
    });

    $("#FormSearch-ProjectSectionOperation").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionOperation(1);
    });

    $("#FormSearch-ProjectSectionOperation").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionOperation").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionOperation").on("change keyup", "#StateId, #CityId, #ProjectSectionOperationStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionOperation(1);
    });


    $("#FormSearch-ProjectSectionOperation").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionOperation').on("click", "#lprojectSectionSchedule", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionSchedule/_Index', '', '#ProjectSectionSchedule-ProjectSectionOperation');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionOperation').on("click", "#lprojectSectionSupervisorMember", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorMember/_Index', '', '#ProjectSectionSupervisorMember-ProjectSectionOperation');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionOperation').on("click", "#lprojectSectionAdvisorMember", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorMember/_Index', '', '#ProjectSectionAdvisorMember-ProjectSectionOperation');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionOperation').on("click", "#lprojectSectionSupervisorVisit", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionSupervisorVisit/_Index', '', '#ProjectSectionSupervisorVisit-ProjectSectionOperation');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionOperation').on("click", "#lprojectSectionAdvisorVisit", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionAdvisorVisit/_Index', '', '#ProjectSectionAdvisorVisit-ProjectSectionOperation');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });
}

function DataRefreshProjectSectionOperation(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionOperation').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionOperation/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionOperation tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {


            if (json[i].ProjectSectionOperationStateId == 1)
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' style='line-height: 28px; font-weight:bold;' />");
            else
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' style='line-height: 28px' />");

            tr.append("<td style='display:none' data-th='ردیف'>" + json[i].Id + "</td>");
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد پروژه'>" + json[i].FileCode + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='عنوان فاز'>" + json[i].ProjectSectionTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهرستان'>" + json[i].CityTitle + "</td>");
            tr.append("<td data-th='نحوه واگذاری'>" + json[i].AssignmentTypeTitle + "</td>");
            tr.append("<td data-th='شماره قرارداد'>" + json[i].ContractNumber + "</td>");
            tr.append("<td data-th='پیمانکار'>" + json[i].CompanyName + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].StartDate + "</td>");
            tr.append("<td data-th='تاریخ پایان'>" + json[i].EndDate + "</td>");
            tr.append("<td style='display:none' data-th='@Html.DisplayNameFor(model => model.ProjectSectionOperationStateId)'>" + json[i].ProjectSectionOperationStateId + "</td>");
            tr.append("<td data-th='وضعیت پروژه'>" + json[i].ProjectSectionOperationStateTitle + "</td>");
            tr.append("<td style='display:none;' data-th='@Html.DisplayNameFor(model => model.Color)'>" + json[i].Color + "</td>");

            $('#tbl-ProjectSectionOperation tbody').append(tr);
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
    $('#tbl-ProjectSectionOperation tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    $('#FormTabs-ProjectSectionOperation .active').removeClass('active');


    GetIsMainSupervisor(Id);
}


function LoadDataProjectSectionOperation(pageRecord) {
    var totalRecords = DataRefreshProjectSectionOperation(pageRecord, $('#tbl-ProjectSectionOperation .page-size').val(), $('#sort-ProjectSectionOperation').val());
    Pager(pageRecord, $('#tbl-ProjectSectionOperation .page-size').val(), "ProjectSectionOperation", totalRecords);
}

function SelectProjectSectionOperation() {
    $('#tbl-ProjectSectionOperation tbody tr').each(function (i, row) {
        if ($(this).find('td').eq(0).html() == SelectedProjectSection) {
            $(this).addClass('selectedRow');
        }
    });
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

function GetIsMainSupervisor(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionOperation/GetIsMainSupervisor', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
        if (data == 'True') {
            $('#lprojectSectionSchedule').css('display', 'inline-block');
        }
        else {
            $('#lprojectSectionSchedule').css('display', 'none');
        }
    });
}