var ProjectSectionFinancePermissions;
var SelectedProjectSection = -1;


$(function () {
    ProjectSectionFinancePermissions = $('#permission-ProjectSectionFinance').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinance/_Search', '', '#FormSearch-ProjectSectionFinance', 'SearchProjectSectionFinanceCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinance/_List', '', '#FormList-ProjectSectionFinance', 'ListProjectSectionFinanceCallback();');

    EventHandlerProjectSectionFinance();
});

function SearchProjectSectionFinanceCallback() {
    HandleValidation();
}

function ListProjectSectionFinanceCallback() {
    Pager(1, 5, "ProjectSectionFinance", DataRefreshProjectSectionFinance(1, 5, $("#sort-ProjectSectionFinance").val()));

    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionFinance() {
    $("#FormList-ProjectSectionFinance").on("keypress", "#tbl-ProjectSectionFinance tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionFinance(1);
            return false;
        }
    });

   

    $("#FormList-ProjectSectionFinance").on("change keyup", "#tbl-ProjectSectionFinance tbody tr:first select", function (e) {
        LoadDataProjectSectionFinance(1);
    });

    $("#FormSearch-ProjectSectionFinance").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionFinance(1);
    });

    $("#FormSearch-ProjectSectionFinance").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionFinance").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionFinance").on("change keyup", "#StateId, #CityId, #ProjectSectionFinanceStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionFinance(1);
    });


    $("#FormSearch-ProjectSectionFinance").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionFinance').on("click", "#lprojectSectionStatement", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatement/_Index', '', '#projectSectionStatement-ProjectSectionFinance');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });


    $('#FormTabs-ProjectSectionFinance').on("click", "#lprojectSectionStatementConfirm", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatementConfirmList/_Index', '', '#projectSectionStatementConfirm-ProjectSectionFinance');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });


    $('#FormTabs-ProjectSectionFinance').on("click", "#lprojectSectionDraftSend", function () {
        if (SelectedProjectSection != -1) {
            $('#projectSectionDraft-ProjectSectionFinance').children().remove();
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraftList/_Index/1', '', '#projectSectionDraftSend-ProjectSectionFinance');
        }
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionFinance').on("click", "#lprojectSectionDraft", function () {
        if (SelectedProjectSection != -1) {
            $('#projectSectionDraftSend-ProjectSectionFinance').children().remove();
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionDraftList/_Index/2', '', '#projectSectionDraft-ProjectSectionFinance');
        }
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionFinance').on("click", "#lprojectSectionPaidPrice", function () {
        if (SelectedProjectSection != -1) {
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionPaidPriceList/_Index', '', '#projectSectionPaidPrice-ProjectSectionFinance');
        }
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });
    $('#FormTabs-ProjectSectionFinance').on("click", "#lprintDraft", function () {
        if (SelectedProjectSection != -1) {
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionPrintDraft/_Index', '', '#projectSectionPrintDraft-ProjectSectionFinance');

        }
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

}





function DataRefreshProjectSectionFinance(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionFinance').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionFinance/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionFinance tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {
            if (json[i].ProjectSectionFinanceStateId == 2)
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
            //tr.append("<td data-th='وضعیت پروژه'>" + json[i].ProjectSectionOperationStateTitle + "</td>");
            tr.append("<td style='display:none;' data-th='@Html.DisplayNameFor(model => model.Color)'>" + json[i].Color + "</td>");

            $('#tbl-ProjectSectionFinance tbody').append(tr);
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
    $('#tbl-ProjectSectionFinance tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    $('#FormTabs-ProjectSectionFinance .active').removeClass('active');
}


function LoadDataProjectSectionFinance(pageRecord) {
    var totalRecords = DataRefreshProjectSectionFinance(pageRecord, $('#tbl-ProjectSectionFinance .page-size').val(), $('#sort-ProjectSectionFinance').val());
    Pager(pageRecord, $('#tbl-ProjectSectionFinance .page-size').val(), "ProjectSectionFinance", totalRecords);
}

function SelectProjectSectionFinance() {
    $('#tbl-ProjectSectionFinance tbody tr').each(function (i, row) {
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