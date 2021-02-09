var ProjectSectionLetterPermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionLetterPermissions = $('#permission-ProjectSectionLetter').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionLetter/_Search', '', '#FormSearch-ProjectSectionLetter', 'SearchProjectSectionLetterCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionLetter/_List', '', '#FormList-ProjectSectionLetter', 'ListProjectSectionLetterCallback();');
    
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionCorrespondence/_Index', '', '#Delivery-ProjectSectionLetter');

    EventHandlerProjectSectionLetter();
});

function SearchProjectSectionLetterCallback() {
    HandleValidation();
}

function ListProjectSectionLetterCallback() {
    Pager(1, 5, "ProjectSectionLetter", DataRefreshProjectSectionLetter(1, 5, $("#sort-ProjectSectionLetter").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionLetter() {
    $("#FormList-ProjectSectionLetter").on("keypress", "#tbl-ProjectSectionLetter tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionLetter(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionLetter").on("change keyup", "#tbl-ProjectSectionLetter tbody tr:first select", function (e) {
        LoadDataProjectSectionLetter(1);
    });

    $("#FormSearch-ProjectSectionLetter").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionLetter(1);
    });

    $("#FormSearch-ProjectSectionLetter").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionLetter").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionLetter").on("change keyup", "#StateId, #CityId, #ProjectSectionLetterStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionLetter(1);
    });


    $("#FormSearch-ProjectSectionLetter").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionLetter').on("click", "#lprojectSectionSchedule", function () {
        
    });
}

function DataRefreshProjectSectionLetter(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionLetter').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionLetter/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionLetter tbody tr').not(':first').remove();

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

            $('#tbl-ProjectSectionLetter tbody').append(tr);
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
    $('#tbl-ProjectSectionLetter tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    LoadDataProjectSectionCorrespondence(1);

    $('#FormTabs-ProjectSectionLetter .active').removeClass('active');
}


function LoadDataProjectSectionLetter(pageRecord) {
    var totalRecords = DataRefreshProjectSectionLetter(pageRecord, $('#tbl-ProjectSectionLetter .page-size').val(), $('#sort-ProjectSectionLetter').val());
    Pager(pageRecord, $('#tbl-ProjectSectionLetter .page-size').val(), "ProjectSectionLetter", totalRecords);
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