var ProjectSectionStatementConfirmPermissions;


$(function () {
    ProjectSectionStatementConfirmPermissions = $('#permission-ProjectSectionStatementConfirmList').val().split(',');
 
    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirmList/_List", ProjectSectionStatementConfirmPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionStatementConfirmList/_List', '', '#FormList-ProjectSectionStatementConfirmList', 'ListProjectSectionStatementConfirmListCallback();');
    } 

    handel();
});


function handel() {
    $("#FormList-ProjectSectionStatementConfirmList").on("keypress", "#tbl-ProjectSectionStatementConfirmList tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionStatementConfirm(1);
            return false;
        }
    });
}



function ListProjectSectionStatementConfirmListCallback() {
    Pager(1, 5, "ProjectSectionStatementConfirmList", DataRefreshProjectSectionStatementConfirmList(1, 5, $("#sort-ProjectSectionStatementConfirmList").val()));
    HandleValidation();

    SortArrow();
}


//------------
// لیست صورت وضعیت
function DataRefreshProjectSectionStatementConfirmList(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    var jsonParams = 'ProjectSectionStatement.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-tbl-ProjectSectionStatementConfirmList').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionStatementConfirmList/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionStatementConfirmList tbody tr').not(':first').remove();
        var json = JSON.parse(data.value);
        var tr;

        for (var i = 0; i < json.length; i++) {
            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='نظر واحد فنی'>" + json[i].Opinion.Title + "</td>");
            tr.append("<td data-th='نوع صورت وضعیت'>" + json[i].StatementType.Title + "</td>");
            tr.append("<td data-th='موقت/قطعی'>" + json[i].TempFixed.Title + "</td>");
            tr.append("<td data-th='شماره صورت وضعیت'>" + json[i].StatementNumber.Title + "</td>");
            tr.append("<td data-th='مبلغ اعلامی پیمانکار'>" + Seprator(json[i].ProjectSectionStatement.ContractPrice) + "</td>");
            tr.append("<td data-th='مبلغ پیشنهادی مشاور'>" + Seprator(json[i].ProjectSectionStatement.AdvisorPrice) + "</td>");
            tr.append("<td data-th='مبلغ پیشنهادی ناظر'>" + Seprator(json[i].ProjectSectionStatement.ConfirmPrice) + "</td>");
            tr.append("<td data-th='انتخاب'><a onmousedown = 'PopupFormHtml(\"تایید صورت وضعیت\", \"/ProjectDevision/ProjectSectionStatementConfirm/_Index\", \"IndexProjectSectionStatementConfirmCallback(" + json[i].ProjectSectionStatement.Id + "," + json[i].ProjectSectionStatement.ConfirmPrice + ");\", false)'  title='انتخاب'><input type='button' class='btn btn-warning' style='width:100px;' value='تایید'></a></td>");
            
            $('#tbl-ProjectSectionStatementConfirmList tbody').append(tr);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }

        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}
//------------



function LoadDataProjectSectionStatementConfirmList(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionStatementConfirm/_List", ProjectSectionStatementConfirmPermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionStatementConfirm(pageRecord, $('#tbl-ProjectSectionStatementConfirm .page-size').val(), $('#sort-ProjectSectionStatementConfirm').val());

        Pager(pageRecord, $('#tbl-ProjectSectionStatementConfirm .page-size').val(), "ProjectSectionStatementConfirm", totalRecords);
    }
}

