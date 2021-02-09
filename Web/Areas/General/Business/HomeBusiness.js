$(function () {
    EventHandlerHome();
});

function EventHandlerHome() {
    $("#Default").on("submit", "#frm-ChangeMemberPassword", function (e) {
        e.preventDefault();

        Ajax('Post', '/General/Home/_SaveChanges', $('#frm-ChangeMemberPassword').serialize(), function (data, textStatus, xhr) {
            Messages(data.type, data.message);
        }, 'json');
    });
}