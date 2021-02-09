var ProjectSectionRecoupmentListPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionRecoupmentListPermissions = $('#permission-ProjectSectionRecoupmentList').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionRecoupmentList/_Search', '', '#FormSearch-ProjectSectionRecoupmentList', 'SearchProjectSectionRecoupmentListCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionRecoupmentList/_List', '', '#FormList-ProjectSectionRecoupmentList', 'ListProjectSectionRecoupmentListCallback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionRecoupment/_Index', '', '#Delivery-ProjectSectionRecoupmentList');

    EventHandlerProjectSectionRecoupmentList();
});

function SearchProjectSectionRecoupmentListCallback() {
    HandleValidation();
}

function ListProjectSectionRecoupmentListCallback() {
    Pager(1, 5, "ProjectSectionRecoupmentList", DataRefreshProjectSectionRecoupmentList(1, 5, $("#sort-ProjectSectionRecoupmentList").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionRecoupmentList() {
    $("#FormList-ProjectSectionRecoupmentList").on("keypress", "#tbl-ProjectSectionRecoupmentList tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionRecoupmentList(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionRecoupmentList").on("change keyup", "#tbl-ProjectSectionRecoupmentList tbody tr:first select", function (e) {
        LoadDataProjectSectionRecoupmentList(1);
    });

    $("#FormSearch-ProjectSectionRecoupmentList").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionRecoupmentList(1);
    });

    $("#FormSearch-ProjectSectionRecoupmentList").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionRecoupmentList").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionRecoupmentList").on("change keyup", "#StateId, #CityId, #ProjectSectionRecoupmentListStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionRecoupmentList(1);
    });


    $("#FormSearch-ProjectSectionRecoupmentList").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionRecoupmentList').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSectionRecoupmentList(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionRecoupmentList').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionRecoupmentList/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionRecoupmentList tbody tr').not(':first').remove();

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

            $('#tbl-ProjectSectionRecoupmentList tbody').append(tr);
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
    $('#tbl-ProjectSectionRecoupmentList tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadDataProjectSectionRecoupment(1);

    $('#FormTabs-ProjectSectionRecoupmentList .active').removeClass('active');
}


function LoadDataProjectSectionRecoupmentList(pageRecord) {
    var totalRecords = DataRefreshProjectSectionRecoupmentList(pageRecord, $('#tbl-ProjectSectionRecoupmentList .page-size').val(), $('#sort-ProjectSectionRecoupmentList').val());
    Pager(pageRecord, $('#tbl-ProjectSectionRecoupmentList .page-size').val(), "ProjectSectionRecoupmentList", totalRecords);
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