$(document).ready(() => {

    const clearNotify = () => {
        $("#notifyMessageText").text("");
        $("#notifyMessage").removeClass("notifyActiveSuccess");
        $("#notifyMessage").removeClass("notifyActiveFailed");
        $("#preNotifyMessage").removeClass("preNotifyActive");
    };
    const sendNotifyMessage = (text, isSuccess) => {
        clearNotify();
        if (isSuccess == true) {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActiveSuccess");
            $("#preNotifyMessage").addClass("preNotifyActive");
        }
        else {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActiveFailed");
            $("#preNotifyMessage").addClass("preNotifyActive");
        }
        setTimeout(() => {
            clearNotify();
        }, 5000);
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

        $.post("/user/invite", { model }, (resp) => {
            if (resp == true) {
                sendNotifyMessage("Invite sended!", true);
                $("#inviteWindow").addClass("d-none");
                $("#hideBackgroundWrapper").toggleClass("d-none");
                $("body").toggleClass("overflow-hidden");
            }
            else {
                sendNotifyMessage("Incorrect data when entering!", false);
            }
           
        })
    });

    // Invite - reject
    $(".inviteRejectBtn").on("click", e => {
        e.preventDefault();

        let inviteGuid = e.target.dataset.invite;

        $.post("/user/invite-reject", { inviteGuid }, (resp) => {
            if (resp) {
                $(`.inviteSection[data-invite="${resp}"]`).remove();
                sendNotifyMessage("Invite reject!", false);
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
