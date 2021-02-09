var ProjectSectionFixedPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionFixedPermissions = $('#permission-ProjectSectionFixed').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionFixed/_Search', '', '#FormSearch-ProjectSectionFixed', 'SearchProjectSectionFixedCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionFixed/_List', '', '#FormList-ProjectSectionFixed', 'ListProjectSectionFixedCallback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryFixd/_Index', '', '#Delivery-ProjectSectionFixed');

    EventHandlerProjectSectionFixed();
});

function SearchProjectSectionFixedCallback() {
    HandleValidation();
}

function ListProjectSectionFixedCallback() {
    Pager(1, 5, "ProjectSectionFixed", DataRefreshProjectSectionFixed(1, 5, $("#sort-ProjectSectionFixed").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionFixed() {
    $("#FormList-ProjectSectionFixed").on("keypress", "#tbl-ProjectSectionFixed tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionFixed(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionFixed").on("change keyup", "#tbl-ProjectSectionFixed tbody tr:first select", function (e) {
        LoadDataProjectSectionFixed(1);
    });

    $("#FormSearch-ProjectSectionFixed").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionFixed(1);
    });

    $("#FormSearch-ProjectSectionFixed").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionFixed").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionFixed").on("change keyup", "#StateId, #CityId, #ProjectSectionFixedStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionFixed(1);
    });


    $("#FormSearch-ProjectSectionFixed").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionFixed').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSectionFixed(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionFixed').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionFixed/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionFixed tbody tr').not(':first').remove();

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

            $('#tbl-ProjectSectionFixed tbody').append(tr);
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
    $('#tbl-ProjectSectionFixed tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadDataProjectSectionDeliveryFixd(1);

    $('#FormTabs-ProjectSectionFixed .active').removeClass('active');
}


function LoadDataProjectSectionFixed(pageRecord) {
    var totalRecords = DataRefreshProjectSectionFixed(pageRecord, $('#tbl-ProjectSectionFixed .page-size').val(), $('#sort-ProjectSectionFixed').val());
    Pager(pageRecord, $('#tbl-ProjectSectionFixed .page-size').val(), "ProjectSectionFixed", totalRecords);
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