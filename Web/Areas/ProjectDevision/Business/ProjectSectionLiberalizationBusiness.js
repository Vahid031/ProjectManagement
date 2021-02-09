var ProjectSectionLiberalizationPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionLiberalizationPermissions = $('#permission-ProjectSectionLiberalization').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionLiberalization/_Search', '', '#FormSearch-ProjectSectionLiberalization', 'SearchProjectSectionLiberalizationCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionLiberalization/_List', '', '#FormList-ProjectSectionLiberalization', 'ListProjectSectionLiberalizationCallback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionDepositeLiberalization/_Index', '', '#Delivery-ProjectSectionLiberalization');

    EventHandlerProjectSectionLiberalization();
});

function SearchProjectSectionLiberalizationCallback() {
    HandleValidation();
}

function ListProjectSectionLiberalizationCallback() {
    Pager(1, 5, "ProjectSectionLiberalization", DataRefreshProjectSectionLiberalization(1, 5, $("#sort-ProjectSectionLiberalization").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionLiberalization() {
    $("#FormList-ProjectSectionLiberalization").on("keypress", "#tbl-ProjectSectionLiberalization tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionLiberalization(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionLiberalization").on("change keyup", "#tbl-ProjectSectionLiberalization tbody tr:first select", function (e) {
        LoadDataProjectSectionLiberalization(1);
    });

    $("#FormSearch-ProjectSectionLiberalization").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionLiberalization(1);
    });

    $("#FormSearch-ProjectSectionLiberalization").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionLiberalization").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionLiberalization").on("change keyup", "#StateId, #CityId, #ProjectSectionLiberalizationStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionLiberalization(1);
    });


    $("#FormSearch-ProjectSectionLiberalization").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionLiberalization').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSectionLiberalization(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionLiberalization').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionLiberalization/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionLiberalization tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ");' style='line-height: 28px' />");

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد پروژه'>" + json[i].FileCode + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='عنوان فاز'>" + json[i].ProjectSectionTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهرستان'>" + json[i].CityTitle + "</td>");
            tr.append("<td data-th='نحوه واگذاری'>" + json[i].AssignmentTypeTitle + "</td>");
            tr.append("<td data-th='شماره قراداد'>" + json[i].ContractNumber + "</td>");
            tr.append("<td data-th='پیمانکار'>" + json[i].CompanyName + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].StartDate + "</td>");
            tr.append("<td data-th='تاریخ پایان'>" + json[i].EndDate + "</td>");

            $('#tbl-ProjectSectionLiberalization tbody').append(tr);
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
    $('#tbl-ProjectSectionLiberalization tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadDataProjectSectionDepositeLiberalization(1);

    $('#FormTabs-ProjectSectionLiberalization .active').removeClass('active');
}


function LoadDataProjectSectionLiberalization(pageRecord) {
    var totalRecords = DataRefreshProjectSectionLiberalization(pageRecord, $('#tbl-ProjectSectionLiberalization .page-size').val(), $('#sort-ProjectSectionLiberalization').val());
    Pager(pageRecord, $('#tbl-ProjectSectionLiberalization .page-size').val(), "ProjectSectionLiberalization", totalRecords);
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