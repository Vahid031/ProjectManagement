var SectionPermissions;

$(function () {
    SectionPermissions = $('#permission-Section').val().split(',');
    
    if ($.inArray("/BaseInformation/Section/_List", SectionPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Section/_List', '', '#FormList-Section', 'ListSectionCallback();');
    }

    if ($.inArray("/BaseInformation/Section/_Create", SectionPermissions) > -1) {
        LoadPartialView('GET', '/BaseInformation/Section/_Create', '', '#FormContainer-Section', 'CreateSectionCallback();');
    }

    EventHandlerSection();
});

function CreateSectionCallback() {
    CheckValue();

    HandleValidation();
}

function UpdateSectionCallback() {
    CheckValue();

    HandleValidation();
}

function ListSectionCallback() {
    Pager(1, 5, "Section", DataRefreshSection(1, 5, $("#sort-Section").val()));
    
    HandleValidation();
    SortArrow();
}

function EventHandlerSection() {
    $("#FormContainer-Section").on("submit", "#frm-Section", function (e) {
        e.preventDefault();

        Ajax('Post', '/BaseInformation/Section/_Create', $('#frm-Section').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormSection();

            if ($('#tbl-Section .page-record').val() == null)
                LoadDataSection(1);
            else
                LoadDataSection($('#tbl-Section .page-record').val());

            if ($.inArray("/BaseInformation/Section/_Create", SectionPermissions) == -1) {
                $('#FormContainer-Section').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-Section").on("click", "#frm-Section .btnNew", function () {
        ClearFormSection();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-Section").on("keypress", "#tbl-Section tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataSection(1);
            return false;
        }
    });

    $("#FormList-Section").on("change keyup", "#tbl-Section tbody tr:first select", function (e) {
        LoadDataSection(1);
    });

    $("#FormContainer-Section").on("change keyup", "#StateId", function () {
        if ($(this).val() != '') {
            GetCitiesByStateId("#Section_CityId", $(this).val());
        }
        else {
            $("#Section_CityId option").remove();
        }

        $($(this)).removeData('previousValue');
        $($(this)).valid();

        $('#Section_Code').removeData('previousValue');
        $('#Section_Code').valid();

        $('#Section_Title').removeData('previousValue');
        $('#Section_Title').valid();
    });
}

function DataRefreshSection(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;

    var jsonParams = $('#frm-tbl-Section').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;
    
    Ajax('Post', '/BaseInformation/Section/_List', jsonParams, function (data, textStatus, xhr) {

        $('#tbl-Section tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);

        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');


            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='عنوان استان'>" + json[i].State.Title + "</td>");
            tr.append("<td data-th='عنوان شهرستان'>" + json[i].City.Title + "</td>");
            tr.append("<td data-th='کد بخش'>" + json[i].Section.Code + "</td>");
            tr.append("<td data-th='عنوان بخش'>" + json[i].Section.Title + "</td>");

            if ($.inArray("/BaseInformation/Section/_Update", SectionPermissions) > -1 && $.inArray("/BaseInformation/Section/_Delete", SectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateSection(" + json[i].Section.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteSection'," + json[i].Section.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Section/_Update", SectionPermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateSection(" + json[i].Section.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/BaseInformation/Section/_Delete", SectionPermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteSection'," + json[i].Section.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-Section tbody').append(tr);
        }


        if ($.inArray("/BaseInformation/Section/_Update", SectionPermissions) == -1 && $.inArray("/BaseInformation/Section/_Delete", SectionPermissions) == -1) {
            $('#tbl-Section th:last').remove();
            $('#tbl-Section tbody tr:first td:last').remove();
            $('#tbl-Section tfoot td').attr('colspan', $('#tbl-Section tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataSection(pageRecord) {
    if ($.inArray("/BaseInformation/Section/_List", SectionPermissions) > -1) {
        var totalRecords = DataRefreshSection(pageRecord, $('#tbl-Section .page-size').val(), $('#sort-Section').val());

        Pager(pageRecord, $('#tbl-Section .page-size').val(), "Section", totalRecords);
    }
}

function ClearFormSection() {
    
    $('#frm-Section input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    if ($.inArray("/BaseInformation/Section/_Create", SectionPermissions) > -1) {
        $('#Id').val("-1");
        $('#btnSave').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }
    
    var $validator = $('#frm-Section').validate();
    $('#frm-Section').find(".field-validation-error span").each(function ()
    {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateSection(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/BaseInformation/Section/_Update', { Id: id }, '#FormContainer-Section', 'UpdateSectionCallback();');
}



function DeleteSection(id) {
    Ajax('Post', '/BaseInformation/Section/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-Section tbody tr').length != 2) {
            pageRecord = $('#tbl-Section .page-record').val();
        }
        else {
            if ($('#tbl-Section .page-record').val() != 1)
                pageRecord = $('#tbl-Section .page-record').val() - 1;
        }

        LoadDataSection(pageRecord);
    }, 'json');
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