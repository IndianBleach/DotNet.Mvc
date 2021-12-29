$(document).ready(() => {
    $("#hideBackgroundWrapper").mouseup(function (e) {
        var container = $("#checkOutContainer");
        if (container.has(e.target).length === 0) {
            $("#windowStarred").addClass("d-none");
            $("#windowCreateIdea").addClass("d-none");
            $("#windowParticipation").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("body").removeClass("overflow-hidden");
        }
    });    

    $(".showHideJoin").on("click", (e) => {
        e.preventDefault();
        $("#joinWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });


    $(".showHideCreateIdea").on("click", (e) => {
        e.preventDefault();
        $("body").toggleClass("overflow-hidden");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("#windowCreateIdea").toggleClass("d-none");
    });

    $(".showHideStarred").on("click", (e) => {
        e.preventDefault();
        $("body").toggleClass("overflow-hidden");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("#windowStarred").toggleClass("d-none");
    });

    // INVITE
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
    $("#hideNotifyMessage").on("click", (e) => {
        e.preventDefault();
        $("#notifyMessage").addClass("d-none");
    });
    // ****

    // PARTICIPATION
    $(".showParticipation").on("click", (e) => {
        e.preventDefault();

        let userName = $("#hiddenUserName").text();

        $.post("/load/LoadParticipation", { userName }, (resp) => {
            resp.forEach(elem => {
                $("#participationLoad").append(
                    `<div class='note-participation'><a href='idea/${elem.ideaGuid}'><span>${elem.ideaName}</span><br /><p>Role:<span class='t-sm text-white t-semi-bold'>${elem.roleName}</span></p></a></div>`
                );
            })

            $("#windowParticipation").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        })
    });
    $(".hideParticipation").on("click", () => {
        $(".note-participation").remove();
        $("#windowParticipation").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
    // ****

    // FOLLOW
    $("#userFollowForm").on("submit", (e) => {
        e.preventDefault();

        let followGuid = e.target.getElementsByTagName("input").item(0).value;
        let isUnfollow = e.target.dataset.unfollow == "true";

        if (isUnfollow == true) {
            $.post("/user/unfollow", { followGuid }, (boolResponse) => {
                if (boolResponse == true) {
                    e.target.dataset.unfollow = false;
                    $("#userFollowForm button").removeClass("t-muted").text("Follow");
                }
            })
        }
        else {
            $.post("/user/follow", { followGuid }, (boolResponse) => {
                if (boolResponse == true) {
                    e.target.dataset.unfollow = true;
                    $("#userFollowForm button").addClass("t-muted").text("Unfollow");
                }
            })
        }
    });
    // ****

    


});
