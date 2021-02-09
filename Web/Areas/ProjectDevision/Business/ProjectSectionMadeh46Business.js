var ProjectSectionMadeh46Permissions;


$(function () {
    ProjectSectionMadeh46Permissions = $('#permission-ProjectSectionMadeh46').val().split(',');
    
    if ($.inArray("/ProjectDevision/ProjectSectionMadeh46/_Create", ProjectSectionMadeh46Permissions) > -1) {
        LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh46/_Create/' + SelectedProjectSection, '', '#FormContainer-ProjectSectionMadeh46', 'CreateProjectSectionMadeh46Callback();');
    }

    EventHandlerProjectSectionMadeh46();
});

function CreateProjectSectionMadeh46Callback() {
    CheckValueProjectSectionMadeh46();
    HandleValidation();

    DatePic('#ProjectSectionMadeh46_LetterDate');
}

function UpdateProjectSectionMadeh46Callback() {
    CreateProjectSectionMadeh46Callback();
}

function EventHandlerProjectSectionMadeh46() {
    $("#FormContainer-ProjectSectionMadeh46").on("submit", "#frm-ProjectSectionMadeh46", function (e) {
        e.preventDefault();

        //------- زمانی که هیچ فازی انتخاب نشده بود پیغام میدهد
        if (SelectedProjectSection == -1) {
            Messages('warning', 'فازی انتخاب نشده است');
            return;
        }

        if ($('#images-ProjectSectionMadeh46').val() == '')
        {
            Messages('warning', 'پیوست الزامی میباشد');
            return;
        }

        Ajax('Post', '/ProjectDevision/ProjectSectionMadeh46/_Create', 'Files=' + $('#images-ProjectSectionMadeh46').val() + "&" + 'ProjectSectionMadeh46.ProjectSectionId=' + SelectedProjectSection + '&' + $('#frm-ProjectSectionMadeh46').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);

            LoadPartialView('GET', '/ProjectDevision/ProjectSectionMadeh46/_Index', '', '#Delivery-ProjectSection46');

        }, 'json');
    });

    $("#FormContainer-ProjectSectionMadeh46").on("click", "#btnShowProjectSectionMadeh46Files", function () {
        PopupFormHtml("پیوست ها", "/ProjectDevision/ProjectSectionMadeh46/_FileUpload", "ShowSelectedFiles()", true, "YesFilePopupClick();")
    });
}

function YesFilePopupClick() {
    $('#images-ProjectSectionMadeh46').val('');
    $('.ProjectSectionMadeh46Files').each(function (i, row) {
        $('#images-ProjectSectionMadeh46').val($('#images-ProjectSectionMadeh46').val() + $(this).val() + ",");
    });
}

function ShowSelectedFiles() {
    $('#ProjectSectionMadeh46-fileupload').fileupload();

    $('#ProjectSectionMadeh46-fileupload').fileupload('option', {
        maxFileSize: 500000000,
        resizeMaxWidth: 1920,
        resizeMaxHeight: 1200,
        autoUpload: true,
    });

    GetProjectSectionMadeh46Files($('#ProjectSectionMadeh46Id').val());
}

function CheckValueProjectSectionMadeh46() {
    if ($('#ProjectSectionMadeh46Id').val() != '-1')
        $('#btnSaveProjectSectionMadeh46').html('<span class="glyphicon glyphicon-pencil float-rtl"></span>ویرایش');
}

function GetProjectSectionMadeh46Files(projectSectionId) {
    Ajax('Post', '/ProjectDevision/ProjectSectionMadeh46/GetProjectSectionMadeh46Files', 'projectSectionId=' + projectSectionId, function (data, textStatus, xhr) {
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

        $('#ProjectSectionMadeh46-fileupload').fileupload('option', 'done').call($('#ProjectSectionMadeh46-fileupload'), $.Event('done'), { result: { files: files } });

    }, 'json');
}


function ReloadProjectSectionMadeh46Files() {
    Ajax('Post', '/ProjectDevision/ProjectSectionMadeh46/ReloadFiles', {}, function (data, textStatus, xhr) { });
}