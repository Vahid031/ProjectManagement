var RolePermissions;


$(function () {
    RolePermissions = $('#permission-Role').val().split(',');

    if ($.inArray("/General/RolePermission/_List", RolePermissions) > -1) {
        LoadPartialView('GET', '/General/RolePermission/_List', '', '#FormList', 'GetTree(); GetPermission();');
    }

    if ($.inArray("/General/RolePermission/_Create", RolePermissions) > -1) {
        LoadPartialView('GET', '/General/RolePermission/_Create', '', '#FormContainer');
    }

    CreateHandler();

    // Tree -> Colups -> Expand
    $("#FormList").on("click", "#treeview ul li div", function () {
        $(this).parent('li').children('ul').slideToggle('1500');
        $(this).toggleClass('expand');
    });

    $("#FormList").on("click", "#treeviewPermission ul li div", function () {
        $(this).parent('li').children('ul').slideToggle('1500');
        $(this).toggleClass('expand');
    });


    $("#FormList").on("click", "#treeviewPermission input:checkbox", function () {
        $(this).siblings("ul").find("input:checkbox").prop("checked", $(this).prop('checked'));
    });
    //  انتخاب نود و انتصاب آیدی پرنت و لول
    $("#FormList").on("click", "#treeview ul li .tree-text", function () {
        $('#treeview .tree-select').removeClass('tree-select');

        $('#tree-parent-text').html($(this).parent('li').children('.tree-text').text());

        var txt = $(this).parent('li').children('.tree-text');
        txt.addClass("tree-select");
        
        GetRolePermission($(this).parent('li').children('.tree-text').attr('id'));

    });

});


var treeParent = "";
function GetTreeParent() {

    if (treeParent == "")
        $('#tree-parent-text').html("هیچ والدی انتخاب نشده است");
    else
        $('#tree-parent-text').html(treeParent);
}



function GetTree() {
    Ajax('Post', '/General/RolePermission/_List', '', function (data, textStatus, xhr) {
        $('#RoleTree').html(data.Values);
        $('#tree-parent-text').html("هیچ والدی انتخاب نشده است");

        $('#RoleTree span.tree-update').remove();
        $('#RoleTree span.tree-delete').remove();
    }, 'json');
}

function GetPermission() {
    Ajax('Post', '/General/RolePermission/_ListPermission', '', function (data, textStatus, xhr) {
        $('#PermissionTree').html(data.Values);
    }, 'json');
}

function GetRolePermission(RoleId) {

    Ajax('Post', '/General/RolePermission/_ListRolePermission', 'RoleId=' + RoleId, function (data, textStatus, xhr) {
        $("#treeviewPermission input").prop("checked", false);

        var json = JSON.parse(data.Values);

        for (var i = 0; i < json.length; i++) {
            $("#treeviewPermission input[value=" + json[i] + "]").prop("checked", true);
        }

    }, 'json');
}


function ClearTextbox() {
    $("#treeviewPermission input").prop("checked", false);

    if ($.inArray("/General/RolePermission/_Create", RolePermissions) > -1) {
        $('#tree-parent-text').html("هیچ والدی انتخاب نشده است");
        $('#RoleTree .tree-select').removeClass('tree-select');
    }
}



function CreateHandler() {
    $("#FormContainer").on("submit", "#frm-Role", function (e) {
        e.preventDefault();

        // این کد در زمان ثبت چک می کند آیا والد انتخاب شده است
        if (typeof $('.tree-select').attr('level') === 'undefined') {
            Messages('warning', 'هیچ والدی انتخاب نشده است');
            return;
        }

        var PermissionIds = "1,";

        PermissionIds += $("#treeviewPermission input:checkbox:checked").map(function () {
            return $(this).attr('Value');
        }).get();

        var id = $("#treeview .tree-select").attr('id');

        Ajax('Post', '/General/RolePermission/_Create', "Role.Id=" + id + "&PermissionIds=" + PermissionIds, function (data, textStatus, xhr) {
            Messages(data.type, data.message);
            ClearTextbox();

            if ($.inArray("/General/RolePermission/_Create", RolePermissions) == -1) {
                $('#FormContainer').fadeOut('fast');
            }

            if ($.inArray("/General/RolePermission/_List", RolePermissions) > -1) {
                GetTree();
            }

        }, 'json');
    });

}



