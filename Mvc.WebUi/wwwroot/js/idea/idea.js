$(document).ready(() => {   

    const sendNotifyMessage = (text, isSuccess) => {
        if (isSuccess == true) {
            $("#notifyMessageText").text(text);
            $("#notifyMessage").addClass("notifyActive");
        }
    };

    $("#hideBackgroundWrapper").mouseup(function (e) {
        var container = $("#checkOutContainer");
        if (container.has(e.target).length === 0) {
            $("#windowStarred").addClass("d-none");
            $("#windowCreateIdea").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("body").removeClass("overflow-hidden");
            $("#settingsModdersWindow").addClass("d-none");
            $("#settingsGeneralWindow").addClass("d-none");
            $("#settingsCreateTopicWindow").addClass("d-none");
            $("#settingsRemoveIdeaWindow").addClass("d-none");
            $("#topicWindow").addClass("d-none");
            $("#goalWindow").addClass("d-none");
            $("#joinWindow").addClass("d-none");
            $("#windowParticipation").addClass("d-none");
        }
    });

    $(".showHideJoin").on("click", (e) => {
        e.preventDefault();
        $("#joinWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    $("#preCheckOutIdea").mouseup(function (e) {
        var container = $("#checkOutIdea");
        if (container.has(e.target).length === 0) {
            $("#topicWindow").addClass("d-none");
            $("#joinWindow").addClass("d-none");
            $("#goalWindow").addClass("d-none");
            $("#windowParticipation").addClass("d-none");
            $("#settingsModdersWindow").addClass("d-none");
            $("#settingsGeneralWindow").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("#settingsRemoveIdeaWindow").addClass("d-none");
            $("#settingsCreateTopicWindow").addClass("d-none");            
            $("body").removeClass("overflow-hidden");
        }
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

    $(".showHideGeneralSettings").on("click", (e) => {
        e.preventDefault();
        $("#settingsGeneralWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });


    // topic new comment async
    $("#topicCommentForm").on("submit", (e) => {
        e.preventDefault();

        let topicGuid = e.target.dataset.topic;
        let message = e.target.getElementsByTagName("input")[0].value;

        $.post("/idea/topics/createcomment", { topicGuid, message }, (resp) => {
            $("#topicDetailComments").append(`<div class="topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}"/></a><p><span class="pe-1">${resp.authorName}</span>${resp.comment}<br><span class="date-publ-span t-sm text-muted">${resp.dateCreated}</span></p></div>`);

            $("#topicCommentForm input").val("");
        });

    })

    // hideTopicBtn
    $(".hideTopicBtn").on("click", (e) => {
        e.preventDefault();

        $("#topicDetailTitle").text("");
        $("#topicDetailDescription").text("");
        //$("")

        $("#topicWindow").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").removeClass("overflow-hidden");
    })

    // async show topic
    showTopicBtn = (topicGuid) => {
        $.get("/idea/topicdetail", { topicGuid }, resp => {
            if (resp) {

                console.log(resp);

                $("#topicDetailTitle").text(resp.title);
                $("#topicDetailDescription").text(resp.description);
                $("#topicDetailAuthorAvatar").attr("src", "/media/userAvatars/" + resp.authorAvatarImage);
                $("#topicDetailPublish").text(resp.datePublished);
                resp.comments.forEach(item => {
                    $("#topicDetailComments").append(`<div class="topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}"/></a><p><span class="ps-1 pe-1">${item.authorName}</span>${item.comment}<br><span class="date-publ-span t-sm text-muted">${item.dateCreated}</span></p></div>`);
                });
                $("#topicCommentForm").attr("data-topic", resp.guid);

                $("#topicWindow").removeClass("d-none");
                $("#hideBackgroundWrapper").removeClass("d-none");
                $("body").addClass("overflow-hidden");
            }
        });
    }

    



    // settings modders
    $("#hideSettingsModders").on("click", (e) => {
        e.preventDefault();

        $("#settingsModdersWindow").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").removeClass("overflow-hidden");
    })

    // settings modders
    $("#showSettingsModders").on("click", (e) => {
        e.preventDefault();

        $("#settingsModdersWindow").removeClass("d-none");
        $("#hideBackgroundWrapper").removeClass("d-none");
        $("body").addClass("overflow-hidden");

        let ideaGuid = e.target.dataset.idea;

        $.get("/load/ideaRoles", { ideaGuid }, (resp) => {
            resp.forEach(item => {
                if (item.isModder) {
                    $("#settingsModdersLoad").append(`<div class="modder-note"><a href="/user/${item.userGuid}"><img src="/media/userAvatars/${item.userAvatarImage}"/><span>${item.userName}</span></a><div class="modder-note-buttons"><button data-role="${item.roleGuid}" class="settingsRemoveModderBtn window-btn t-primary">To Member</button><button data-role="${item.roleGuid}" class="settingsCickMemberBtn window-btn t-muted">Cick</button></div></div>`);
                }
                else if (item.isModder == false) {
                    $("#settingsModdersLoad").append(`<div class="modder-note"><a href="/user/${item.userGuid}"><img src="/media/userAvatars/${item.userAvatarImage}"/><span>${item.userName}</span></a><div class="modder-note-buttons"><button data-role="${item.roleGuid}" class="settingsAddModderBtn window-btn t-primary">To Modder</button><button data-role="${item.roleGuid}" class="settingsCickMemberBtn window-btn t-muted">Cick</button></div></div>`);
                }
            });

            // set listeners
            $(".settingsAddModderBtn").on("click", e => {
                e.preventDefault();

                let roleGuid = e.target.dataset.role;

                $.post("/idea/addModder", { roleGuid }, resp => {
                    sendNotifyMessage("Modder added!", true);

                    $("#settingsModdersWindow").addClass("d-none");
                    $("#hideBackgroundWrapper").addClass("d-none");
                    $("body").removeClass("overflow-hidden");

                    $(".modder-note").remove();
                });
            });

            $(".settingsRemoveModderBtn").on("click", e => {
                e.preventDefault();

                let roleGuid = e.target.dataset.role;

                $.post("/idea/removeModder", { roleGuid }, resp => {
                    sendNotifyMessage("Modder remove!", true);

                    $("#settingsModdersWindow").addClass("d-none");
                    $("#hideBackgroundWrapper").addClass("d-none");
                    $("body").removeClass("overflow-hidden");

                    $(".modder-note").remove();
                });
            });

            $(".settingsCickMemberBtn").on("click", (e) => {
                e.preventDefault();

                let roleGuid = e.target.dataset.role;

                $.post("cickMember", { roleGuid }, (resp) => {
                    sendNotifyMessage(`${resp} cicked!`, true);

                    $("#settingsModdersWindow").addClass("d-none");
                    $("#hideBackgroundWrapper").addClass("d-none");
                    $("body").removeClass("overflow-hidden");

                    $(".modder-note").remove();
                });
            });
        });
    });

    // create topic
    $("#formCreateTopic").on("submit", (e) => {
        e.preventDefault();

        let inputs = e.target.getElementsByTagName("input");
        let ideaGuid = inputs[0].value;
        let title = inputs[1].value;
        let description = e.target.getElementsByTagName("textarea")[0].value;

        $.post("/idea/createTopic", { title, description, ideaGuid }, response => {
            $("#settingsCreateTopicWindow").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        });
    });
    $(".showHideCreateTopic").on("click", (e) => {
        e.preventDefault();
        $("#settingsCreateTopicWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
    // ----

    //remove idea
    $(".showHideRemoveIdea").on("click", (e) => {
        e.preventDefault();
        $("#settingsRemoveIdeaWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
});
