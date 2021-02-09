var ProjectSection46Permissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSection46Permissions = $('#permission-ProjectSection46').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSection46/_Search', '', '#FormSearch-ProjectSection46', 'SearchProjectSection46Callback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSection46/_List', '', '#FormList-ProjectSection46', 'ListProjectSection46Callback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh46/_Index', '', '#Delivery-ProjectSection46');

    EventHandlerProjectSection46();
});

function SearchProjectSection46Callback() {
    HandleValidation();
}

function ListProjectSection46Callback() {
    Pager(1, 5, "ProjectSection46", DataRefreshProjectSection46(1, 5, $("#sort-ProjectSection46").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSection46() {
    $("#FormList-ProjectSection46").on("keypress", "#tbl-ProjectSection46 tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSection46(1);
            return false;
        }
    });

    $("#FormList-ProjectSection46").on("change keyup", "#tbl-ProjectSection46 tbody tr:first select", function (e) {
        LoadDataProjectSection46(1);
    });

    $("#FormSearch-ProjectSection46").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSection46(1);
    });

    $("#FormSearch-ProjectSection46").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSection46").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSection46").on("change keyup", "#StateId, #CityId, #ProjectSection46StateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSection46(1);
    });


    $("#FormSearch-ProjectSection46").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSection46').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSection46(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSection46').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSection46/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSection46 tbody tr').not(':first').remove();

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
            tr.append("<td data-th='شماره قرارداد'>" + json[i].ContractNumber + "</td>");
            tr.append("<td data-th='پیمانکار'>" + json[i].CompanyName + "</td>");
            tr.append("<td data-th='تاریخ شروع'>" + json[i].StartDate + "</td>");
            tr.append("<td data-th='تاریخ پایان'>" + json[i].EndDate + "</td>");

            $('#tbl-ProjectSection46 tbody').append(tr);
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
    $('#tbl-ProjectSection46 tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh46/_Index', '', '#Delivery-ProjectSection46');

    $('#FormTabs-ProjectSection46 .active').removeClass('active');
}


function LoadDataProjectSection46(pageRecord) {
    var totalRecords = DataRefreshProjectSection46(pageRecord, $('#tbl-ProjectSection46 .page-size').val(), $('#sort-ProjectSection46').val());
    Pager(pageRecord, $('#tbl-ProjectSection46 .page-size').val(), "ProjectSection46", totalRecords);
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