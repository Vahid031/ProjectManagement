var ProjectSectionAssignmentPermissions;
var SelectedProjectSection = -1;
var SelectedProjectAssignmentType = -1;


$(function () {
    ProjectSectionAssignmentPermissions = $('#permission-ProjectSectionAssignment').val().split(',');

    LoadPartialView('GET', '/ProjectDevision/ProjectSectionAssignment/_Search', '', '#FormSearch-ProjectSectionAssignment', 'SearchProjectSectionAssignmentCallback();');
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionAssignment/_List', '', '#FormList-ProjectSectionAssignment', 'ListProjectSectionAssignmentCallback();');

    EventHandlerProjectSectionAssignment();
});

function SearchProjectSectionAssignmentCallback() {
    HandleValidation();

    $('#lprojectSectionInquiry').css('display', 'none');
    $('#lprojectSectionCeremonial').css('display', 'none');
}

function ListProjectSectionAssignmentCallback() {
    Pager(1, 5, "ProjectSectionAssignment", DataRefreshProjectSectionAssignment(1, 5, $("#sort-ProjectSectionAssignment").val()));

    HandleValidation();

    SortArrow();
}


function EventHandlerProjectSectionAssignment() {
    $("#FormList-ProjectSectionAssignment").on("keypress", "#tbl-ProjectSectionAssignment tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionAssignment(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionAssignment").on("change keyup", "#tbl-ProjectSectionAssignment tbody tr:first select", function (e) {
        LoadDataProjectSectionAssignment(1);
    });

    $("#FormSearch-ProjectSectionAssignment").on("change", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        LoadDataProjectSectionAssignment(1);
    });

    $("#FormSearch-ProjectSectionAssignment").on("keyup", "#FileCode, #ProjectTitle, #ProjectSectionTitle", function (e) {
        $('input[name=' + $(this).attr('Id') + ']').val($(this).val());
    });


    $("#FormList-ProjectSectionAssignment").on("keyup", "input[name=FileCode], input[name=ProjectTitle], input[name=ProjectSectionTitle]", function (e) {
        $('#' + $(this).attr('Name')).val($(this).val());
    });

    $("#FormSearch-ProjectSectionAssignment").on("change keyup", "#StateId, #CityId, #ProjectSectionAssignmentStateId", function (e) {
        if ($(this).val() != '')
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val($('#' + $(this).attr('Id') + ' option:selected').text());
        else
            $('input[name=' + $(this).attr('Id').substr(0, $(this).attr('Id').length - 2) + 'Title' + ']').val('');
        LoadDataProjectSectionAssignment(1);
    });


    $("#FormSearch-ProjectSectionAssignment").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#CityId", $(this).val());
        }
        else {
            $("#CityId option").remove();
        }
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionInquiry", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionInquiry/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionInquiry-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionCeremonial", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionCeremonial/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionCeremonial-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionTender", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionTender/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionTender-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionParticipant", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionParticipant/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionParticipant-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionFinanceEvaluation", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinanceEvaluation/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionFinanceEvaluation-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionFinalParticipant", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionFinalParticipant/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionFinalParticipant-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionOpenDate", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionOpenDate/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionOpenDate-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionWinner", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinner/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionWinner-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionWinnerDegree", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinnerDegree/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionWinnerDegree-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionContract", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionContract/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionContract-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionOperationTitle", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionOperationTitle/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionOperationTitle-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionComplementarityPrice", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityPrice/_Index', '', '#ProjectSectionComplementarityPrice-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

    $('#FormTabs-ProjectSectionAssignment').on("click", "#lprojectSectionComplementarityDate", function () {
        if (SelectedProjectSection != -1)
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionComplementarityDate/_Index', '', '#ProjectSectionComplementarityDate-ProjectSectionAssignment');
        else
            Messages('warning', 'فازی انتخاب نشده است');
    });

}

function DataRefreshProjectSectionAssignment(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-ProjectSectionAssignment').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionAssignment/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionAssignment tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {


            if (json[i].ProjectSectionAssignmentStateId == 1 || json[i].ProjectSectionAssignmentStateId == 2 || json[i].ProjectSectionAssignmentStateId == 3)
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ", " + json[i].AssignmentTypeId + ");' style='line-height: 28px' class='tr-change' />");
            else
                tr = $("<tr onmousedown='ProjectSelect(this, " + json[i].Id + ", " + json[i].AssignmentTypeId + ");' style='line-height: 28px' />");

            tr.append("<td style='display:none' data-th='ردیف'>" + json[i].Id + "</td>");
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='کد پروژه'>" + json[i].FileCode + "</td>");
            tr.append("<td data-th='عنوان پروژه'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='عنوان فاز'>" + json[i].ProjectSectionTitle + "</td>");
            tr.append("<td data-th='استان'>" + json[i].StateTitle + "</td>");
            tr.append("<td data-th='شهرستان'>" + json[i].CityTitle + "</td>");
            tr.append("<td data-th='نحوه واگذاری'>" + json[i].AssignmentTypeTitle + "</td>");
            tr.append("<td style='display:none' data-th='@Html.DisplayNameFor(model => model.ProjectSectionAssignmentStateId)'>" + json[i].ProjectSectionAssignmentStateId + "</td>");
            tr.append("<td data-th='وضعیت پروژه'>" + json[i].ProjectSectionAssignmentStateTitle + "</td>");
            tr.append("<td style='display:none;' data-th='@Html.DisplayNameFor(model => model.Color)'>" + json[i].Color + "</td>");

            $('#tbl-ProjectSectionAssignment tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }

        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}


function ProjectSelect(selectedRow, Id, AssignmentTypeId) {
    $('#tbl-ProjectSectionAssignment tbody tr').removeClass('selectedRow');
    $(selectedRow).addClass('selectedRow');

    SelectedProjectSection = Id;

    SelectedProjectAssignmentType = AssignmentTypeId;

    $('#lprojectSectionWinner a').html('انتخاب برنده')
    if (AssignmentTypeId == 1 || AssignmentTypeId == 2) // استعلام
    {
        $('#lprojectSectionInquiry').css('display', 'block');

        $('#lprojectSectionTender').css('display', 'none');
        $('#lprojectSectionCeremonial').css('display', 'none');
        $('#lprojectSectionParticipant').css('display', 'none');
        $('#lprojectSectionFinanceEvaluation').css('display', 'none');
        $('#lprojectSectionFinalParticipant').css('display', 'none');
        $('#lprojectSectionOpenDate').css('display', 'none');
    }
    else if (AssignmentTypeId == 3) // مناقصه
    {
        $('#lprojectSectionTender').css('display', 'block');
        $('#lprojectSectionParticipant').css('display', 'block');
        $('#lprojectSectionFinanceEvaluation').css('display', 'block');
        $('#lprojectSectionFinalParticipant').css('display', 'block');
        $('#lprojectSectionOpenDate').css('display', 'block');

        $('#lprojectSectionInquiry').css('display', 'none');
        $('#lprojectSectionCeremonial').css('display', 'none');
    }
    else if (AssignmentTypeId == 4) // ترک تشریفات
    {
        $('#lprojectSectionCeremonial').css('display', 'block');

        $('#lprojectSectionTender').css('display', 'none');
        $('#lprojectSectionInquiry').css('display', 'none');
        $('#lprojectSectionParticipant').css('display', 'none');
        $('#lprojectSectionFinanceEvaluation').css('display', 'none');
        $('#lprojectSectionFinalParticipant').css('display', 'none');
        $('#lprojectSectionOpenDate').css('display', 'none');
    }
    else if (AssignmentTypeId == 5) // معاملات جزء
    {
        $('#lprojectSectionCeremonial').css('display', 'none');
        $('#lprojectSectionTender').css('display', 'none');
        $('#lprojectSectionInquiry').css('display', 'none');
        $('#lprojectSectionParticipant').css('display', 'none');
        $('#lprojectSectionFinanceEvaluation').css('display', 'none');
        $('#lprojectSectionFinalParticipant').css('display', 'none');
        $('#lprojectSectionOpenDate').css('display', 'none');
        $('#lprojectSectionWinner a').html('انتخاب پیمانکار');
    }

    $('#FormTabs-ProjectSectionAssignment .active').removeClass('active');
}


function LoadDataProjectSectionAssignment(pageRecord) {
    var totalRecords = DataRefreshProjectSectionAssignment(pageRecord, $('#tbl-ProjectSectionAssignment .page-size').val(), $('#sort-ProjectSectionAssignment').val());
    Pager(pageRecord, $('#tbl-ProjectSectionAssignment .page-size').val(), "ProjectSectionAssignment", totalRecords);
}

function SelectProjectSectionAssignment() {
    $('#tbl-ProjectSectionAssignment tbody tr').each(function (i, row) {
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

