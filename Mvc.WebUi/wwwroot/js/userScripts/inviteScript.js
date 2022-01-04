$(document).ready(() => {

    const sendNotifyMessage = (text, isSuccess) => {
        if (isSuccess == true) {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActive");
        }
    };

    $(".inviteRejectBtn").on("click", e => {
        e.preventDefault();

        let inviteGuid = e.target.dataset.invite;

        $.post("/user/invite-reject", { inviteGuid }, (resp) => {
            if (resp) {
                $(`.inviteSection[data-invite="${resp}"]`).remove();
                sendNotifyMessage("Invite reject!", true);
            }
        });
    });

    $(".inviteAcceptBtn").on("click", e => {
        e.preventDefault();

        let inviteGuid = e.target.dataset.invite;

        $.post("/user/invite-accept", { inviteGuid }, (resp) => {
            if (resp) {
                $(`.inviteSection[data-invite="${resp}"]`).remove();
                sendNotifyMessage("Invite accept!", true);
            }
        });
    });

    $(".showHideInvite").on("click", (e) => {
        e.preventDefault();
        $("#inviteWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
});
