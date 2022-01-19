$(document).ready(() => {

    const sendNotifyMessage = (text, isSuccess) => {
        if (isSuccess == true) {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActive");
            $("#preNotifyMessage").addClass("preNotifyActive");
        }
    };

    $("#hideBackgroundWrapper").mouseup(function (e) {
        var container = $("#checkOutContainer");
        if (container.has(e.target).length === 0) {
            $("#inviteWindow").addClass("d-none");
            $("body").addClass("overflow-hidden");
            $("#hideBackgroundWrapper").addClass("d-none");
        }
    });

    $("#preCheckOutIdea").mouseup(function (e) {
        var container = $("#checkOutIdea");
        if (container.has(e.target).length === 0) {
            $("#inviteWindow").addClass("d-none");
            $("body").addClass("overflow-hidden");
            $("#hideBackgroundWrapper").addClass("d-none");
        }
    });

    $("#hideNotifyMessage").on("click", (e) => {
        e.preventDefault();
        $("#notifyMessage").addClass("d-none");
    });

    // Invite - send
    $("#sendInviteForm").on("submit", (e) => {
        e.preventDefault();

        let userGuid = e.target.getElementsByTagName("input").item(0).value;
        let toIdea = e.target.getElementsByTagName("select").item(0).value;
        let description = e.target.getElementsByTagName("textarea").item(0).value;

        let model = {
            Description: description,
            InvitedUserGuid: userGuid,
            InvitedToIdeaName: toIdea
        }

        $.post("/user/invite", { model }, (data) => {
            if (data == true) {
                $("#notifyMessageText").text("Invite sended!");
                $("#notifyMessage").addClass("notifyActive");
            }
            $("#inviteWindow").addClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        })
    });

    // Invite - reject
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

    // Invite - accept
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
        $("body").toggleClass("overflow-hidden");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("#inviteWindow").toggleClass("d-none");
    });
});
