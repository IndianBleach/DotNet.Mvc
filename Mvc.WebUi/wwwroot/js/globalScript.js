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

    $("input[type='file']").on("change", (e) => {
        e.preventDefault();
        console.log(e.target.files[0].name);
        $("#avatarPreRenderName").text(e.target.files[0].name);
    });

    /*
    $("#hideNotifyMessage").on("click", (e) => {
        e.preventDefault();
        $("#notifyMessage").addClass("d-none");
    });
    */

    $("#hideBackgroundWrapper").mouseup(function (e) {
        var container = $("#checkOutContainer");
        if (container.has(e.target).length === 0) {
            $("#windowCreateIdea").addClass("d-none");
            hideParticipationWindow();
            $("#hideBackgroundWrapper").addClass("d-none");
            $("body").removeClass("overflow-hidden");
        }
    });    

    // create idea
    $("#createIdeaForm").on("submit", (e) => {
        e.preventDefault();

        let tags = Array(...e.target.getElementsByTagName("select")[0].selectedOptions).reduce((acc, option) => {
            if (option.selected === true) {
                acc.push(option.value);
            }
            return acc;
        }, []);


        let idea = {
            author: e.target.getElementsByTagName("input")[0].value,
            title: e.target.getElementsByTagName("input")[1].value,
            description: e.target.getElementsByTagName("textarea")[0].value,
            tags: tags,
            isSecret: e.target.getElementsByTagName("input")[2].checked
        };

        $.post("/jscreate/idea", { idea }, resp => {
            if (resp == null) {
                sendNotifyMessage("Incorrect data when entering", false);
            }
            else {
                sendNotifyMessage("Idea created! Check profile", true);

                $("body").removeClass("overflow-hidden");
                $("#hideBackgroundWrapper").addClass("d-none");
                $("#windowCreateIdea").addClass("d-none");
            }
        });
        
        
    });

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
                    `<div class='note-participation'><a href='/idea/${elem.ideaGuid}'><span>${elem.ideaName}</span><br /><p class="t-medium t-muted">${elem.roleName}</p></a></div>`
                );
            })

            $("#windowParticipation").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        })
    });
    const hideParticipationWindow = () => {
        $(".note-participation").remove();
        $("#windowParticipation").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").toggleClass("overflow-hidden");
    };
    $(".hideParticipation").on("click", () => {
        hideParticipationWindow();
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
                    $("#userFollowForm button").removeClass("t-primary");
                }
            })
        }
    });
    // -----
});
