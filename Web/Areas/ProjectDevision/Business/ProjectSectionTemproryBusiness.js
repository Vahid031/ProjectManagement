var ProjectSectionTemproryPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionTemproryPermissions = $('#permission-ProjectSectionTemprory').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionTemprory/_Search', '', '#FormSearch-ProjectSectionTemprory', 'SearchProjectSectionTemproryCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionTemprory/_List', '', '#FormList-ProjectSectionTemprory', 'ListProjectSectionTemproryCallback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionDeliveryTemprory/_Index', '', '#Delivery-ProjectSectionTemprory');

    EventHandlerProjectSectionTemprory();
});

function SearchProjectSectionTemproryCallback() {
    HandleValidation();
}

function ListProjectSectionTemproryCallback() {
    Pager(1, 5, "ProjectSectionTemprory", DataRefreshProjectSectionTemprory(1, 5, $("#sort-ProjectSectionTemprory").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionTemprory() {
    $("#FormList-ProjectSectionTemprory").on("keypress", "#tbl-ProjectSectionTemprory tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionTemprory(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionTemprory").on("change keyup", "#tbl-ProjectSectionTemprory tbody tr:first select", function (e) {
        LoadDataProjectSectionTemprory(1);
    });

    $("#FormSearch-ProjectSectionTemprory").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionTemprory(1);
    });

    $("#FormSearch-ProjectSectionTemprory").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionTemprory").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionTemprory").on("change keyup", "#StateId, #CityId, #ProjectSectionTemproryStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionTemprory(1);
    });


    $("#FormSearch-ProjectSectionTemprory").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionTemprory').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSectionTemprory(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionTemprory').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionTemprory/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionTemprory tbody tr').not(':first').remove();

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

            $('#tbl-ProjectSectionTemprory tbody').append(tr);
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
    $('#tbl-ProjectSectionTemprory tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadDataProjectSectionDeliveryTemprory(1);

    $('#FormTabs-ProjectSectionTemprory .active').removeClass('active');
}


function LoadDataProjectSectionTemprory(pageRecord) {
    var totalRecords = DataRefreshProjectSectionTemprory(pageRecord, $('#tbl-ProjectSectionTemprory .page-size').val(), $('#sort-ProjectSectionTemprory').val());
    Pager(pageRecord, $('#tbl-ProjectSectionTemprory .page-size').val(), "ProjectSectionTemprory", totalRecords);
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