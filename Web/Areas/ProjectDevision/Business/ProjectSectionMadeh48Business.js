var ProjectSectionMadeh48Permissions;


$(function () {
    ProjectSectionMadeh48Permissions = $('#permission-ProjectSectionMadeh48').val().split(',');
    
    if ($.inArray("/ProjectDevision/ProjectSectionMadeh48/_Create", ProjectSectionMadeh48Permissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh48/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionMadeh48', 'CreateProjectSectionMadeh48Callback();');
    }

    EventHandlerProjectSectionMadeh48();
});

function CreateProjectSectionMadeh48Callback() {
    CheckValueProjectSectionMadeh48();
    HandleValidation();

    DatePic('#ProjectSectionMadeh48_LetterDate');
}

function UpdateProjectSectionMadeh48Callback() {
    CreateProjectSectionMadeh48Callback();
}

function EventHandlerProjectSectionMadeh48() {
    $("#FormContainer-ProjectSectionMadeh48").on("submit", "#frm-ProjectSectionMadeh48", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        if ($('#images-ProjectSectionMadeh48').val() == '') {
            Messages('warning', 'پیوست الزامی میباشد');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionMadeh48/_Create', 'Files=' + $('#images-ProjectSectionMadeh48').val() + "&" + 'ProjectSectionMadeh48.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionMadeh48').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);

            LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh48/_Index', '', '#Delivery-ProjectSection48');

        }, 'json');
    });

    $("#FormContainer-ProjectSectionMadeh48").on("click", "#btnShowProjectSectionMadeh48Files", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionMadeh48/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionMadeh48').val('');
    $('.ProjectSectionMadeh48Files').each(function (i, row) {
        $('#images-ProjectSectionMadeh48').val($('#images-ProjectSectionMadeh48').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionMadeh48-fileupload').fileupload();

    $('#ProjectSectionMadeh48-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionMadeh48Files($('#ProjectSectionMadeh48Id').val());
}

function CheckValueProjectSectionMadeh48() {
    if ($('#ProjectSectionMadeh48Id').val() != '-1')
        $('#btnSaveProjectSectionMadeh48').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}

function GetProjectSectionMadeh48Files(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionMadeh48/GetProjectSectionMadeh48Files', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionMadeh48-fileupload').fileupload('option', 'done').call($('#ProjectSectionMadeh48-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionMadeh48Files() {
    Ajax('Post', '/ProjectDevision/ProjectSectionMadeh48/ReloadFiles', {}, function (data, textStatus, xhr) { });
}