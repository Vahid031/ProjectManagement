var ProjectSectionContractPermissions;


$(function () {
    ProjectSectionContractPermissions = $('#permission-ProjectSectionContract').val().split(',');
    
    if ($.inArray("/ProjectDevision/ProjectSectionContract/_Create/" + SelectedProjectAssignmentType, ProjectSectionContractPermissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionContract/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionContract', 'CreateProjectSectionContractCallback();');
    }

    EventHandlerProjectSectionContract();
});

function CreateProjectSectionContractCallback() {
    CheckValueProjectSectionContract();
    HandleValidation();

    SepratePrice('#ProjectSectionContract_UnitPrice');
    
    DatePic('#ProjectSectionContract_ContractDate');
    DatePic('#ProjectSectionContract_EndDate');
    DatePic('#ProjectSectionContract_FixedDeliveryDate');
    DatePic('#ProjectSectionContract_StartDate');
    DatePic('#ProjectSectionContract_TempDelivaryDate');
    DatePic('#ProjectSectionContract_WorkshopDeliveryDate');
    DatePic('#ProjectSectionContract_WorkshopRemoveDate');
    DatePic('#ProjectSectionContract_WorkshopToolingDate');

    var ContractDetails = $('#ContractDetails-ProjectSectionContract').val().split('#');
    
    for (var i = 0; i < ContractDetails.length; i++) {
        if (ContractDetails[i] != '')
        {
            var innerContractDetails = ContractDetails[i].split('|');
            var tr;

            tr = $('<tr/>');
            tr.append("<td data-th='ردیف'>" + ($('#tbl-ProjectSectionContractDetail tbody tr').length + 1) + "</td>")
            tr.append("<td data-th=''>" + innerContractDetails[0] + "</td>")
            tr.append("<td data-th=''>" + innerContractDetails[1] + "</td>")
            tr.append("<td data-th=''>" + innerContractDetails[2] + "</td>")
            tr.append("<td style='display:none' data-th=''>" + innerContractDetails[3] + "</td>")
            tr.append("<td data-th=''>" + innerContractDetails[4] + "</td>")
            tr.append("<td data-th=''>" + innerContractDetails[5] + "</td>")
            tr.append("<td class='table-edit' data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionContractDetail'," + ($('#tbl-ProjectSectionContractDetail tbody tr').length + 1) + ") title='حذف' ><span class='fa fa-times fa-md'></span></a></td>");

            $('#tbl-ProjectSectionContractDetail tbody').append(tr);
        }
    }


}

function UpdateProjectSectionContractCallback() {
    CreateProjectSectionContractCallback();
}

function EventHandlerProjectSectionContract() {
    $("#FormContainer-ProjectSectionContract").on("submit", "#frm-ProjectSectionContract", function (e) {
        e.preventDefault();        
        
        // این کد برای برداشتن کاما ، از مقدارهای عددی می باشد
        $('#ProjectSectionContractDetail_TotalAmonts').val($('#ProjectSectionContractDetail_TotalAmonts').val().replace(/\,/g, ''));
        $('#ProjectSectionContractDetail_TotalPrice').val($('#ProjectSectionContractDetail_TotalPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionContractDetail_UnitPrice').val($('#ProjectSectionContractDetail_UnitPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionContract_ContractPrice').val($('#ProjectSectionContract_ContractPrice').val().replace(/\,/g, ''));
        $('#ProjectSectionContract_UnitPrice').val($('#ProjectSectionContract_UnitPrice').val().replace(/\,/g, ''));

        var ContractDetails = '';

        $('#tbl-ProjectSectionContractDetail tbody tr').each(function (i, row) {
            ContractDetails += $(row).find('td').eq(1).html() + '|';
            ContractDetails += $(row).find('td').eq(2).html().replace(/\,/g, '') + '|';
            ContractDetails += $(row).find('td').eq(3).html().replace(/\,/g, '') + '|';
            ContractDetails += $(row).find('td').eq(4).html() + '|';
            ContractDetails += $(row).find('td').eq(6).html().replace(/\,/g, '');
            ContractDetails += '#';
        });

        Ajax('Post', '/ProjectDevision/ProjectSectionContract/_Create', 'ContractDetails=' + ContractDetails + "&" + 'Files=' + $('#images-ProjectSectionContract').val() + "&" + 'ProjectSectionContract.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionContract').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);

            LoadDataProjectSectionAssignment($('#tbl-ProjectSectionAssignment .page-record').val());
            SelectProjectSectionAssignment();
            
            LoadPartialView('GET', '/ProjectDevision/ProjectSectionContract/_Index/' + SelectedProjectAssignmentType, '', '#ProjectSectionContract-ProjectSectionAssignment');

        }, 'json');
    });

    $("#FormContainer-ProjectSectionContract").on("click", "#btnShowProjectSectionContractFiles", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionContract/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });

    $("#FormContainer-ProjectSectionContract").on("click", "#btnSaveProjectSectionContractDetail", function () {
        var tr;

        tr = $('<tr/>');
        tr.append("<td data-th='ردیف'>" + ($('#tbl-ProjectSectionContractDetail tbody tr').length + 1) + "</td>")
        tr.append("<td data-th='عنوان ریز پروژه'>" + $('#ProjectSectionContractDetail_ProjectDetails').val() + "</td>")
        tr.append("<td data-th='مقدار کار'>" + $('#ProjectSectionContractDetail_TotalAmonts').val() + "</td>")
        tr.append("<td data-th='هزینه کل'>" + $('#ProjectSectionContractDetail_TotalPrice').val() + "</td>")

        if ($('#ProjectSectionContractDetail_UnitId').val() == '') {
            tr.append("<td style='display:none' data-th=''>-1</td>")
            tr.append("<td data-th='مقدار کار'></td>")
        }
        else {
            tr.append("<td style='display:none' data-th=''>" + $('#ProjectSectionContractDetail_UnitId').val() + "</td>")
            tr.append("<td data-th='مقدار کار'>" + $('#ProjectSectionContractDetail_UnitId option:selected').text() + "</td>")
        }
        tr.append("<td data-th='هزینه واحد'>" + $('#ProjectSectionContractDetail_UnitPrice').val() + "</td>")
        tr.append("<td class='table-edit' data-th='حذف'><a onmousedown = MvcAlert('DeleteProjectSectionContractDetail'," + ($('#tbl-ProjectSectionContractDetail tbody tr').length + 1) + ") title='حذف' ><span class='fa fa-times fa-md'></span></a></td>");

        $('#tbl-ProjectSectionContractDetail tbody').append(tr);
    });

    $("#FormContainer-ProjectSectionContract").on("change keydown", "#ProjectSectionContract_PaulusPercent", function () {
        $('#ProjectSectionContract_MinosPercent').val(0);
    });

    $("#FormContainer-ProjectSectionContract").on("change keydown", "#ProjectSectionContract_MinosPercent", function () {
        $('#ProjectSectionContract_PaulusPercent').val(0);
    });

    $("#FormContainer-ProjectSectionContract").on("change", "#ProjectSectionContract_ContractDate", function () {
        var which = 0;

        if ($('#ProjectSectionContract_ContractDuration').val() == '' && $('#ProjectSectionContract_EndDate').val() == '')
            which = 0;
        else if ($('#ProjectSectionContract_ContractDuration').val() == '')
            which = 2;
        else
            which = 3;

        if (which != 0)
            GetDate($('#ProjectSectionContract_ContractDate').val(), $('#ProjectSectionContract_ContractDuration').val(), $('#ProjectSectionContract_EndDate').val(), which)
    });

    $("#FormContainer-ProjectSectionContract").on("change keyup", "#ProjectSectionContract_ContractDuration", function () {
        var which = 0;

        if ($('#ProjectSectionContract_ContractDate').val() == '' && $('#ProjectSectionContract_EndDate').val() == '')
            which = 0;
        else if ($('#ProjectSectionContract_ContractDate').val() == '')
            which = 1;
        else
            which = 3;

        if (which != 0)
            GetDate($('#ProjectSectionContract_ContractDate').val(), $('#ProjectSectionContract_ContractDuration').val(), $('#ProjectSectionContract_EndDate').val(), which)
    });

    $("#FormContainer-ProjectSectionContract").on("change", "#ProjectSectionContract_EndDate", function () {
        var which = 0;

        if ($('#ProjectSectionContract_ContractDate').val() == '' && $('#ProjectSectionContract_ContractDuration').val() == '')
            which = 0;
        else if ($('#ProjectSectionContract_ContractDate').val() == '')
            which = 1;
        else
            which = 2;

        if (which != 0)
            GetDate($('#ProjectSectionContract_ContractDate').val(), $('#ProjectSectionContract_ContractDuration').val(), $('#ProjectSectionContract_EndDate').val(), which)
    });

}

function YesFilePopupClick() {
    $('#images-ProjectSectionContract').val('');
    $('.ProjectSectionContractFiles').each(function (i, row) {
        $('#images-ProjectSectionContract').val($('#images-ProjectSectionContract').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionContract-fileupload').fileupload();

    $('#ProjectSectionContract-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionContractFiles($('#ProjectSectionContractId').val());
}

function DeleteProjectSectionContractDetail(Id) {
    $('#tbl-ProjectSectionContractDetail tbody tr').each(function (i, row) {
        if ($(row).find('td').eq(0).html() == Id)
        {
            $(row).remove();
        }
    });

    $('#tbl-ProjectSectionContractDetail tbody tr').each(function (i, row) {
        $(row).find('td').eq(0).html(i + 1);
    });
}

function CheckValueProjectSectionContract() {
    if ($('#ProjectSectionContractId').val() != '-1')
        $('#btnSaveProjectSectionContract').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}

function GetProjectSectionContractFiles(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionContract/GetProjectSectionContractFiles', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionContract-fileupload').fileupload('option', 'done').call($('#ProjectSectionContract-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionContractFiles() {
    Ajax('Post', '/ProjectDevision/ProjectSectionContract/ReloadFiles', {}, function (data, textStatus, xhr) { });
}

function GetDate(beginDate, duration, endDate, which) {
    if (which == 1)
        duration = '-' + duration;
    Ajax('Post', '/ProjectDevision/ProjectSectionContract/GetDate', 'beginDate=' + beginDate + "&" + 'duration=' + duration + "&" + 'endDate=' + endDate + "&" + 'which=' + which, function (data, textStatus, xhr) {
        $('#ProjectSectionContract_ContractDate').val(data.beginDate);
        $('#ProjectSectionContract_ContractDuration').val(data.duration);
        $('#ProjectSectionContract_EndDate').val(data.endDate);
    }, 'json');
}
