function Pager(pageNum, pageRecords, selector, totalRecords) {
    var pageIndex = (typeof pageNum === "undefined") ? 1 : parseInt(pageNum);

    var sortSelector = '#sort-' + selector;
    var tableSelector = '#tbl-' + selector;
    var functionSelector = "DataRefresh" + selector;

    var pageBoxCount = 5;
    var pageCount = Math.ceil(totalRecords / pageRecords);

    var pageFrom = 1;
    var pageTo = pageCount;
    if (totalRecords == 0)
    {
        pageFrom = 0;
        pageTo = 0;
    }
  

    var needFirst = false;
    var needNext = true;
    var needBack = true;
    var needLast = false;

    if (pageIndex == 1 || totalRecords == 0) {
        needBack = false;
    }
    if (pageIndex == pageCount || totalRecords == 0) {
        needNext = false;
    }

    if (pageCount > pageBoxCount && totalRecords != 0) {
        if (pageIndex <= (Math.ceil(pageBoxCount / 2))) {
            pageFrom = 1;
            pageTo = pageBoxCount;
            needFirst = false;
            needLast = true;
        }
        else if (pageIndex >= pageCount - (Math.floor(pageBoxCount / 2))) {
            pageFrom = pageCount - pageBoxCount + 1;
            pageTo = pageCount;
            needFirst = true;
            needLast = false;
        }
        else {
            pageFrom = pageIndex - (Math.ceil(pageBoxCount / 2)) + 1;
            pageTo = pageIndex + (Math.floor(pageBoxCount / 2));
            needFirst = true;
            needLast = true;
        }
    }

    $(tableSelector + ' tfoot tr td').html('');

    pageHtml = "<div class='paging'><nav><ul class='pagination' style='margin:0px;'>";
  
    if (needFirst) {
        pageHtml += "<li><a onmousedown='Pager(\"1\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'> <span>&lt;&lt;</span></a></li>";
    }
    else {
        pageHtml += "<li class='disabled'><a><span>&lt;&lt;</span></a></li>";
    }

    if (needBack) {
        pageHtml += "<li><a onmousedown='Pager(\"" + (pageIndex - 1).toString() + "\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'><span>&lt;</span></a></li>";
    }
    else {
        pageHtml += "<li class='disabled'><a><span>&lt;</span></a></li>";
    }
  
    if (totalRecords != 0) {
        for (var i = pageFrom; i <= pageTo; i++) {
            if (i == pageIndex) {
                pageHtml += "<li class='active'><a onmousedown='Pager(\"" + i + "\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'><span> " + i + "</span></a></li>";
            }
            else {
                pageHtml += "<li><a onmousedown='Pager(\"" + i + "\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'><span> " + i + "</span></a></li>";
            }
        }
    }

    if (needNext) {
        pageHtml += "<li><a onmousedown='Pager(\"" + (parseInt(pageIndex) + 1).toString() + "\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'><span>&gt;</span></a></li>";
    }
    else {
        pageHtml += "<li class='disabled'><a><span>&gt;</span></a></li>";
    }

    if (needLast) {
        pageHtml += "<li><a onmousedown='Pager(\"" + pageCount + "\", \"" + pageRecords + "\", \"" + selector + "\", \"" + totalRecords + "\")'><span>&gt;&gt;</span></a></li>";
    }
    else {
        pageHtml += "<li class='disabled'><a><span>&gt;&gt;</span></a></li>";
    }

    pageHtml += '</ul></nav></div>';


    pageHtml += "<div class='record-count'>";
    pageHtml += "<span class='record-count-text'>تعداد رکورد ها</span>";
    pageHtml += "<select class='form-control txtbox grid-ddl page-size'>";
    pageHtml += "<option selected='selected' value='5'>5</option>";
    pageHtml += "<option value='10'>10</option>";
    pageHtml += "<option value='15'>15</option>";
    pageHtml += "<option value='20'>20</option>";
    pageHtml += "<option value='25'>25</option>";
    pageHtml += "</select>";
    pageHtml += "</div>";

    pageHtml += "<div class='page-count'>";
    pageHtml += "<span class='page-count-text'>شماره صفحه</span>";
    pageHtml += "<select class='form-control txtbox grid-ddl page-record'>";
  
    for (var i = 1; i <= pageCount; i++) {
        pageHtml += "<option value='" + i + "'>" + i + "</option>";
    }
    pageHtml += "</select>";
    pageHtml += "</div>";

    pageHtml += "<div class='pager-text'>";
    pageHtml += "<span> نمایش </span>";
    if (totalRecords != 0)
        pageHtml += "<span> " + ((pageIndex * pageRecords) - pageRecords + 1).toString() + " </span>";
    else
        pageHtml += "<span> 0 </span>";

    pageHtml += "<span> تا </span>";
    if (totalRecords != 0) {
        if (pageIndex == pageCount)
            pageHtml += "<span> " + totalRecords + " </span>";
        else
            pageHtml += "<span> " + (pageIndex * pageRecords).toString() + " </span>";
    }
    else
        pageHtml += "<span> 0 </span>";

    pageHtml += "<span> از </span>";
    pageHtml += "<span> " + totalRecords + " </span>";
    pageHtml += "</div>";

    pageHtml += "<script> $(function () { $('" + tableSelector + " .page-size').val(" + pageRecords + "); $('" + tableSelector + " .page-record').val(" + pageIndex + ") });<\/script> ";

    pageHtml += "<script> $('" + tableSelector + " .page-size').bind('change', function () {";
    pageHtml += "if (" + pageIndex * pageRecords + " >= " + totalRecords + ") ";
    pageHtml += "Pager(Math.ceil(" + totalRecords + " / parseInt($('" + tableSelector + " .page-size').val()))" + ", $('" + tableSelector + " .page-size').val() , \"" + selector + "\", \"" + totalRecords + "\"); ";
    pageHtml += " else  ";
    pageHtml += "Pager(Math.ceil(" + pageIndex * pageRecords + " / parseInt($('" + tableSelector + " .page-size').val()))" + ", $('" + tableSelector + " .page-size').val() , \"" + selector + "\", \"" + totalRecords + "\"); ";
    pageHtml += functionSelector + "($('" + tableSelector + " .page-record').val()" + ", $('" + tableSelector + " .page-size').val() , '" + $(sortSelector).val() + "');";
    pageHtml += "});";
    pageHtml += "<\/script>";

    pageHtml += "<script> $('" + tableSelector + " .page-record').bind('change', function () {";
    pageHtml += "Pager($('" + tableSelector + " .page-record').val()" + ", $('" + tableSelector + " .page-size').val() , \"" + selector + "\", \"" + totalRecords + "\"); ";
    pageHtml += functionSelector + "($('" + tableSelector + " .page-record').val()" + ", $('" + tableSelector + " .page-size').val() , '" + $(sortSelector).val() + "');";
    pageHtml += "});";
    pageHtml += "<\/script>";

    //***
    pageHtml += "<script> $('" + tableSelector + " tfoot a').on('mouseup', function () {";
    pageHtml += functionSelector + "(" + pageIndex + ", " + pageRecords + ", '" + $(sortSelector).val() + "');";
    pageHtml += "});";
    pageHtml += "<\/script>";
  
    $(tableSelector + ' tfoot tr td').html(pageHtml);
}

function GetRowNumber(pageNumber, pageSize, orderColumn, totalCount, index) {
    
    if (orderColumn.split(' ')[1] == 'ascending') {
        return totalCount - ((pageNumber - 1) * pageSize + (index - 1));
    }
    else {
        return ((pageNumber - 1) * pageSize) + index;
    }
}


function Sort(newOrderColumnName, selector) {
    var sortSelector = '#sort-' + selector;
    var tableSelector = '#tbl-' + selector;
    var functionSelector = 'DataRefresh' + selector;

    var preOrderColumnName = $(sortSelector).val().split(' ')[0];
    var preOrderColumnType = $(sortSelector).val().split(' ')[1];
    var newOrderColumnType = "ascending";

    if (preOrderColumnName == newOrderColumnName && preOrderColumnType == 'ascending') {
        newOrderColumnType = 'descending';
    }

    $(sortSelector).val(newOrderColumnName + ' ' + newOrderColumnType);

  
    var recordCount = eval(functionSelector + "($('" + tableSelector + " .page-record').val(), $('" + tableSelector + " .page-size').val(), '" + $(sortSelector).val() + "');");
  
    Pager($(tableSelector + " .page-record").val(), $(tableSelector + " .page-size").val(), selector, recordCount);
}



//------------- Start Sort Header Arrow Image -------------------->
function SortArrow() {
    $('.tbl-grid thead tr th a').on('mousedown', function () {
        if (!$(this).select('span').hasClass('arrow-none')) {

            var tbl_name = $(this).parent('th').parent('tr').parent('thead').parent('.tbl-grid').attr('id');

            $('#' + tbl_name + ' thead tr th a').each(function () {
                if (!$(this).children('span').hasClass('arrow-none')) {
                    $(this).children('span').removeClass('arrow-ascending arrow-descending');
                    $(this).children('span').addClass('arrow-sortable');
                }
            });

            tbl_name = tbl_name.substring(tbl_name.lastIndexOf('-'));
            var tbl_sort = $('#sort' + tbl_name).val().split(" ");

            $(this).children('span').removeClass('arrow-sortable arrow-ascending arrow-descending');

            $(this).children('span').addClass('arrow-' + tbl_sort[1]);
        }

    });
}
//------------- End Sort Header Arrow Image -------------------->
