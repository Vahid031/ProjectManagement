

$(function () {
});

var contractorId = 0;

function IndexProjectSectionWinner4648(id) {
    
    contractorId = id;
    LoadPartialView('GET', '/ProjectDevision/ProjectSectionWinner4648/_List', '', '#FormList-ProjectSectionWinner4648', 'ListProjectSectionWinnerCallback4648();');

}




function ListProjectSectionWinnerCallback4648() {
    Pager(1, 5, "ProjectSectionWinner4648", DataRefreshProjectSectionWinner4648(1, 5, $("#sort-ProjectSectionWinner4648").val()));
    
    HandleValidation();

    SortArrow();
}


function DataRefreshProjectSectionWinner4648(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = 'contractorId=' + contractorId + '&' + $('#frm-tbl-ProjectSectionWinner4648').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    Ajax('Post', '/ProjectDevision/ProjectSectionWinner4648/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-ProjectSectionWinner4648 tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');

            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Contractor.CompanyName)'>" + json[i].ProjectNumber + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].ProjectTitle + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].ProjectSectionTitle + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].StartDate_ + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].EndDate_ + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].Type4648 + "</td>");
            tr.append("<td data-th='@Html.DisplayNameFor(model => model.Rank.Title)'>" + json[i].Date_ + "</td>");

            $('#tbl-ProjectSectionWinner4648 tbody').append(tr);
        }


        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}






