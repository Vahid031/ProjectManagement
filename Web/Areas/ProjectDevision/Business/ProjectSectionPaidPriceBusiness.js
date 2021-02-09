var ProjectSectionPaidPricePermissions;


$(function () {

});


function IndexProjectSectionPaidPriceCallback(Id) {

    ProjectSectionPaidPricePermissions = $('#permission-ProjectSectionPaidPrice').val().split(',');

    if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Create", ProjectSectionPaidPricePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionPaidPrice/_Create/'+ Id, '', '#FormContainer-ProjectSectionPaidPrice', 'CreateProjectSectionPaidPriceCallback('+Id+');');
    }


    if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_List", ProjectSectionPaidPricePermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionPaidPrice/_List', '', '#FormList-ProjectSectionPaidPrice', 'ListProjectSectionPaidPriceCallback();');
    }

    EventHandlerProjectSectionPaidPrice();
}


function CreateProjectSectionPaidPriceCallback(Id) {
    $('#ProjectSectionPaidPrice_ProjectSectionDraftId').val(Id);
    CheckValueProjectSectionPaidPrice();
    HandleValidation();
}

function UpdateProjectSectionPaidPriceCallback() {
    CreateProjectSectionPaidPriceCallback();
}

function ListProjectSectionPaidPriceCallback() {
    //$('#ProjectSectionPaidPrice_ProjectSectionDraftId').val(Id);
    Pager(1, 5, "ProjectSectionPaidPrice", DataRefreshProjectSectionPaidPrice(1, 5, $("#sort-ProjectSectionPaidPrice").val()));
    HandleValidation();

    SortArrow();
}

function EventHandlerProjectSectionPaidPrice() {
    $("#FormContainer-ProjectSectionPaidPrice").on("submit", "#frm-ProjectSectionPaidPrice", function (e) {
        e.preventDefault();

        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#DraftPrice').val($('#DraftPrice').val().replace(/\,/g, ''));
        $('#Deposit').val($('#Deposit').val().replace(/\,/g, ''));
        $('#PrePayment').val($('#PrePayment').val().replace(/\,/g, ''));
        $('#Forfeit').val($('#Forfeit').val().replace(/\,/g, ''));
        $('#ContractorInsurance').val($('#ContractorInsurance').val().replace(/\,/g, ''));
        $('#Tax').val($('#Tax').val().replace(/\,/g, ''));
        $('#OtherDept').val($('#OtherDept').val().replace(/\,/g, ''));
        $('#TotalContractorDeductions').val($('#TotalContractorDeductions').val().replace(/\,/g, ''));
        $('#MasterInsurance').val($('#MasterInsurance').val().replace(/\,/g, ''));
        $('#ValueTax').val($('#ValueTax').val().replace(/\,/g, ''));
        $('#TotalMasterDeductions').val($('#TotalMasterDeductions').val().replace(/\,/g, ''));
        $('#PayablePrice').val($('#PayablePrice').val().replace(/\,/g, ''));
        $('#PaidPrice').val($('#PaidPrice').val().replace(/\,/g, ''));


        Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/_Create', 'Files=' + $('#images-ProjectSectionPaidPrice').val() + "&" + 'ProjectSectionPaidPrice.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionPaidPrice').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearFormProjectSectionPaidPrice();

            if ($('#tbl-ProjectSectionPaidPrice .page-record').val() == null)
                LoadDataProjectSectionPaidPrice(1);
            else
                LoadDataProjectSectionPaidPrice($('#tbl-ProjectSectionPaidPrice .page-record').val());

            LoadDataProjectSectionFinance($('#tbl-ProjectSectionFinance .page-record').val());
            SelectProjectSectionFinance();

            if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Create", ProjectSectionPaidPricePermissions) == -1) {
                $('#FormContainer-ProjectSectionPaidPrice').fadeOut('fast');
            }

        }, 'json');
    });


    $("#FormContainer-ProjectSectionPaidPrice").on("click", "#frm-ProjectSectionPaidPrice .btnNew", function () {
        ClearFormProjectSectionPaidPrice();

        $('#Alert,#AlertDown').slideUp(300);
    });


    $("#FormList-ProjectSectionPaidPrice").on("keypress", "#tbl-ProjectSectionPaidPrice tbody tr:first input", function (e) {
        if (e.which == 13) {
            LoadDataProjectSectionPaidPrice(1);
            return false;
        }
    });

    $("#FormList-ProjectSectionPaidPrice").on("change keyup", "#tbl-ProjectSectionPaidPrice tbody tr:first select", function (e) {
        LoadDataProjectSectionPaidPrice(1);
    });


    $("#FormContainer-ProjectSectionPaidPrice").on("click", "#btnShowProjectSectionPaidPriceFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionPaidPrice/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();", "#cpb2");
    });

    $("#FormContainer-ProjectSectionPaidPrice").on("click", "#btnGovermentWorthyDocumentsPaidPrice", function () {
        var PaidPriceId = $('#ProjectSectionPaidPrice_ProjectSectionDraftId').val();
        PopupFormHtml("پرداخت به صورت اوراق بهادار دولتی", "/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_Index", "IndexProjectSectionGovermentWorthyDocumentsPaidPriceCallBack(" + PaidPriceId + ")", '', '', "#cpb2");
    });

    $("#FormContainer-ProjectSectionPaidPrice").on("change keyup", "#TempFixedId", function () {
        if ($(this).val() != '') {
            GetStatementNumbersByTempFixedId("#ProjectSectionPaidPrice_StatementNumberId", $(this).val());
        }
        else {
            $("#ProjectSectionPaidPrice_StatementNumberId option").remove();
        }
    });

    $("#FormContainer-ProjectSectionPaidPrice").on("change keyup", "#ProjectSectionPaidPrice_InsuranceType", function () {
        GetInsurances($(this).val(), $('#DraftPrice').val().replace(/\,/g, ''));
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionPaidPrice').val('');
    $('.ProjectSectionPaidPriceFiles').each(function (i, row) {
        $('#images-ProjectSectionPaidPrice').val($('#images-ProjectSectionPaidPrice').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionPaidPrice-fileupload').fileupload();

    $('#ProjectSectionPaidPrice-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionPaidPriceFiles($('#ProjectSectionPaidPriceId').val());
}

function DataRefreshProjectSectionPaidPrice(pageNumber, pageSize, orderColumn) {
    var rowcount = 0;
    
    var jsonParams = 'ProjectSectionPaidPrice.ProjectSectionId=' + SelectedProjectSection + '&' + 'ProjectSectionPaidPrice.ProjectSectionDraftId=' + $('#ProjectSectionPaidPrice_ProjectSectionDraftId').val() + '&' + $('#frm-tbl-ProjectSectionPaidPrice').serialize() + "&_pageNumber=" + pageNumber + "&_pageSize=" + pageSize + "&_orderColumn=" + orderColumn;

    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/_List', jsonParams, function (data, textStatus, xhr) {
        $('#tbl-ProjectSectionPaidPrice tbody tr').not(':first').remove();

        var json = JSON.parse(data.Values);
        var tr;

        for (var i = 0; i < json.length; i++) {

            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + GetRowNumber(pageNumber, pageSize, orderColumn, data.RowCount, i + 1) + "</td>");
            tr.append("<td data-th='مبلغ حواله'>" + Seprator(json[i].ProjectSectionPaidPrice.DraftPrice) + "</td>");
            tr.append("<td data-th='مبلغ قابل پرداخت'>" + Seprator(json[i].ProjectSectionPaidPrice.PayablePrice) + "</td>");
            tr.append("<td data-th='مبلغ پرداختی'>" + Seprator(json[i].ProjectSectionPaidPrice.PaidPrice) + "</td>");
            tr.append("<td data-th='تاریخ پرداخت'>" + json[i].ProjectSectionPaidPrice.PaidDate_ + "</td>");
            
            if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Update", ProjectSectionPaidPricePermissions) > -1 && $.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Delete", ProjectSectionPaidPricePermissions) > -1) {
                tr.append("<td data-th='ویرایش/حذف'><a onmousedown = UpdateProjectSectionPaidPrice(" + json[i].ProjectSectionPaidPrice.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a> <a onmousedown = MvcAlert('DeleteProjectSectionPaidPrice'," + json[i].ProjectSectionPaidPrice.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Update", ProjectSectionPaidPricePermissions) > -1) {
                tr.append("<td data-th='ویرایش'><a onmousedown = UpdateProjectSectionPaidPrice(" + json[i].ProjectSectionPaidPrice.Id + ") title='ویرایش'><span class='glyphicon glyphicon-pencil icon'></span></a></td>");
            }
            else if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Delete", ProjectSectionPaidPricePermissions) > -1) {
                tr.append("<td data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionPaidPrice'," + json[i].ProjectSectionPaidPrice.Id + ") title='حذف'><span class='glyphicon glyphicon-trash icon'></span></a></td>");
            }

            $('#tbl-ProjectSectionPaidPrice tbody').append(tr);
        }


        if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Update", ProjectSectionPaidPricePermissions) == -1 && $.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Delete", ProjectSectionPaidPricePermissions) == -1) {
            $('#tbl-ProjectSectionPaidPrice th:last').remove();
            $('#tbl-ProjectSectionPaidPrice tbody tr:first td:last').remove();
            $('#tbl-ProjectSectionPaidPrice tfoot td').attr('colspan', $('#tbl-ProjectSectionPaidPrice tfoot td').attr('colspan') - 1);
        }

        if (data.type != 'none') {
            Messages(data.type, data.message);
            $('#Alert').delay(4000).slideUp(300);
        }


        rowcount = data.RowCount;
    }, 'json');

    return rowcount;
}



function LoadDataProjectSectionPaidPrice(pageRecord) {
    if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_List", ProjectSectionPaidPricePermissions) > -1) {
        var totalRecords = DataRefreshProjectSectionPaidPrice(pageRecord, $('#tbl-ProjectSectionPaidPrice .page-size').val(), $('#sort-ProjectSectionPaidPrice').val());

        Pager(pageRecord, $('#tbl-ProjectSectionPaidPrice .page-size').val(), "ProjectSectionPaidPrice", totalRecords);
    }
}

function ClearFormProjectSectionPaidPrice() {

    $('#frm-ProjectSectionPaidPrice input[type=text]').each(function () {
        $(this).val("");
        $(this).removeData('previousValue');
    });

    ReloadProjectSectionPaidPriceFiles();

    if ($.inArray("/ProjectDevision/ProjectSectionPaidPrice/_Create", ProjectSectionPaidPricePermissions) > -1) {
        $('#ProjectSectionPaidPriceId').val("-1");
        $('#btnSaveProjectSectionPaidPrice').html('<span class="glyphicon glyphicon-ok" style="float:right;"></span>ذخیره');
    }

    var $validator = $('#frm-ProjectSectionPaidPrice').validate();
    $('#frm-ProjectSectionPaidPrice').find(".field-validation-error span").each(function () {
        $validator.settings.success($(this));
    })
    $validator.resetForm();
}


function UpdateProjectSectionPaidPrice(id) {
    $('#Alert,#AlertDown').slideUp(300);
    LoadPartialView('POST', '/ProjectDevision/ProjectSectionPaidPrice/_Update', { Id: id }, '#FormContainer-ProjectSectionPaidPrice', 'UpdateProjectSectionPaidPriceCallback();');
}



function DeleteProjectSectionPaidPrice(id) {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/_Delete', { Id: id }, function (data, textStatus, xhr) {
        Messages(data.type, data.message);
        var pageRecord = 1;

        if ($('#tbl-ProjectSectionPaidPrice tbody tr').length != 2) {
            pageRecord = $('#tbl-ProjectSectionPaidPrice .page-record').val();
        }
        else {
            if ($('#tbl-ProjectSectionPaidPrice .page-record').val() != 1)
                pageRecord = $('#tbl-ProjectSectionPaidPrice .page-record').val() - 1;
        }

        LoadDataProjectSectionPaidPrice(pageRecord);

    }, 'json');
}


function CheckValueProjectSectionPaidPrice() {
    if ($('#ProjectSectionPaidPriceId').val() != '-1')
        $('#btnSaveProjectSectionPaidPrice').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}


function GetProjectSectionPaidPriceFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/GetProjectSectionPaidPriceFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
        var json = JSON.parse(data.Values);
        var files = [];

        for (var i = 0; i < json.length; i++) {
            files.push({
                "url": json[i].url,
                "thumbnail_url": json[i].thumbnail_url,
                "name": json[i].name,
                "type": json[i].type,
                "size": json[i].size,
                "delete_url": json[i].delete_url + "/" + json[i].imageId,
                "delete_type": json[i].delete_type,
                "imageId": json[i].imageId
            });
        }

        $('#ProjectSectionPaidPrice-fileupload').fileupload('option', 'done').call($('#ProjectSectionPaidPrice-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionPaidPriceFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/ReloadFiles', {}, function (data, textStatus, xhr) { });
}

function GetInsurances(insuranceType, price) {
    Ajax('Post', '/ProjectDevision/ProjectSectionPaidPrice/GetInsurances', 'insuranceType=' + insuranceType + '&' + 'price=' + price, function (data, textStatus, xhr) {
        $('#ContractorInsurance').val(SeprateNumberByValue(data.contractInsurance));
        $('#MasterInsurance').val(SeprateNumberByValue(data.masterInsurance));

        //---------------- کسورات
        var Deposit = TryParseInt(GetNumber($('#Deposit').val()), 0);
        //alert('Deposit : ' + Deposit);
        var PrePayment = TryParseInt(GetNumber($('#PrePayment').val()), 0);
        //alert('PrePayment : '+PrePayment);
        var Forfeit = TryParseInt(GetNumber($('#Forfeit').val()), 0);
        //alert('Forfeit : ' + Forfeit);
        var ContractorInsurance = TryParseInt(GetNumber($('#ContractorInsurance').val()), 0);
        //alert('ContractorInsurance : ' + ContractorInsurance);
        var Tax = TryParseInt(GetNumber($('#Tax').val()), 0);
        //alert('Tax : ' +Tax);
        var OtherDept = TryParseInt(GetNumber($('#OtherDept').val()), 0);
        //alert('OtherDept : ' + OtherDept);

        var Total1 = Deposit + PrePayment + Forfeit + ContractorInsurance + Tax + OtherDept;
        //alert('Total1 : ' + Total1);
        $('#TotalContractorDeductions').val(Total1);
        //-----------------------------------------------


        //---------------------- اضافات
        var MasterInsurance = TryParseInt(GetNumber($('#MasterInsurance').val()), 0);
        //alert('MasterInsurance : ' + MasterInsurance);
        var ValueTax = TryParseInt(GetNumber($('#ValueTax').val()), 0);
        //alert('ValueTax : ' + ValueTax);

        var Total2 = MasterInsurance + ValueTax;
        $('#TotalMasterDeductions').val(Total2);
        //alert('Total2 : ' + Total2);
        //--------------------------------------------


        //--------------------------- پرداختی
        var DraftPrice = TryParseInt(GetNumber($('#DraftPrice').val()), 0);
        //alert('DraftPrice : ' + DraftPrice);

        $('#PayablePrice,#PaidPrice').val(DraftPrice - Total1 + Total2);
        //------------------------------------------------
        SepratePrice('#DraftPrice');
        SepratePrice('#Deposit');
        SepratePrice('#PrePayment');
        SepratePrice('#Forfeit');
        SepratePrice('#ContractorInsurance');
        SepratePrice('#Tax');
        SepratePrice('#OtherDept');
        SepratePrice('#TotalContractorDeductions');
        SepratePrice('#MasterInsurance');
        SepratePrice('#ValueTax');
        SepratePrice('#TotalMasterDeductions');
        SepratePrice('#PayablePrice');
        SepratePrice('#PaidPrice');

    }, 'json');
}