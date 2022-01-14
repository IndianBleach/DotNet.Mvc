$(document).ready(() => {

    const sendNotifyMessage = (text, isSuccess) => {
        if (isSuccess == true) {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActive");
        }
    };

    $("input[type='file']").on("change", (e) => {
        e.preventDefault();
        console.log(e.target.files[0].name);
        $("#avatarPreRenderName").text(e.target.files[0].name);
    });

    $("#hideNotifyMessage").on("click", (e) => {
        e.preventDefault();
        $("#notifyMessage").addClass("d-none");
    });

    $("#hideBackgroundWrapper").mouseup(function (e) {
        var container = $("#checkOutContainer");
        if (container.has(e.target).length === 0) {
            $("#windowCreateIdea").addClass("d-none");
            $("#windowParticipation").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("body").removeClass("overflow-hidden");
        }
    });    

    // create idea window
    $(".showHideCreateIdea").on("click", (e) => {
        e.preventDefault();
        $("body").toggleClass("overflow-hidden");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("#windowCreateIdea").toggleClass("d-none");
    });
    
    // Participation
    $(".showParticipation").on("click", (e) => {
        e.preventDefault();

        let userName = $("#hiddenUserName").text();

        $.post("/load/LoadParticipation", { userName }, (resp) => {
            resp.forEach(elem => {
                $("#participationLoad").append(
                    `<div class='note-participation'><a href='/idea/${elem.ideaGuid}'><span>${elem.ideaName}</span><br /><p>Role:<span class='t-sm text-white t-semi-bold'>${elem.roleName}</span></p></a></div>`
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
    // -----

    // Followings
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
    // -----
});
