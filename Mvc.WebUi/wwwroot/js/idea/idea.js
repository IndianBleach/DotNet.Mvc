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
            //$("#windowCreateIdea").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("body").removeClass("overflow-hidden");
            $("#settingsModdersWindow").addClass("d-none");
            $("#settingsGeneralWindow").addClass("d-none");
            $("#settingsCreateTopicWindow").addClass("d-none");
            $("#settingsRemoveIdeaWindow").addClass("d-none");
            $("#topicWindow").addClass("d-none");
            //$("#goalWindow").addClass("d-none");
            $("#joinWindow").addClass("d-none");
            //$("#windowParticipation").addClass("d-none");
            $("#createBoxWindow").addClass("d-none");
            hideBoxWindow();
        }
    });


        
    $("#preCheckOutIdea").mouseup(function (e) {
        var container = $("#checkOutIdea");
        if (container.has(e.target).length === 0) {
            $("#topicWindow").addClass("d-none");
            $("#joinWindow").addClass("d-none");
            //$("#goalWindow").addClass("d-none");
            //$("#windowParticipation").addClass("d-none");
            $("#settingsModdersWindow").addClass("d-none");
            $("#settingsGeneralWindow").addClass("d-none");
            $("#hideBackgroundWrapper").addClass("d-none");
            $("#settingsRemoveIdeaWindow").addClass("d-none");
            $("#settingsCreateTopicWindow").addClass("d-none");            
            $("body").removeClass("overflow-hidden");
            $("#createBoxWindow").addClass("d-none");
            hideBoxWindow();
        }
    });

    $(".showHideGeneralSettings").on("click", (e) => {
        e.preventDefault();
        $("#settingsGeneralWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    $(".showHideJoin").on("click", (e) => {
        e.preventDefault();
        $("#joinWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });


    // Joinings
    $(".acceptJoinBtn").on("click", (e) => {
        e.preventDefault();

        let joinGuid = e.target.dataset.join;

        $.post("/idea/acceptJoin", { joinGuid }, resp => {
            if (resp) {
                sendNotifyMessage("Join request accepted!", true);
                $(`.joinRequestSection[data-join="${joinGuid}"]`).remove();
            }
        });
    });

    $(".declineJoinBtn").on("click", (e) => {
        e.preventDefault();

        let joinGuid = e.target.dataset.join;

        $.post("/idea/declineJoin", { joinGuid }, resp => {
            if (resp) {
                sendNotifyMessage("Join request decline!", true);
                $(`.joinRequestSection[data-join="${joinGuid}"]`).remove();
            }
        });
    });

    $("#joinRequestForm").on("submit", (e) => {
        e.preventDefault();

        let description = e.target.getElementsByTagName("textarea")[0].value;
        let ideaGuid = e.target.getElementsByTagName("input")[0].value;

        console.log(description);
        console.log(ideaGuid);

        $.post("/user/joinrequest", { description, ideaGuid }, (resp) => {
            if (resp) {
                sendNotifyMessage("Join request sended!", true);

                $("#joinWindow").addClass("d-none");
                $("#hideBackgroundWrapper").addClass("d-none");
                $("body").removeClass("overflow-hidden");
                e.target.getElementsByTagName("textarea")[0].value = "";
            }
        });

    });
    // -------

    // Box 
    $(".showHideCreateBox").on("click", (e) => {
        $("#createBoxWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    $("#formCreateBox").on("submit", (e) => {
        e.preventDefault();
        //string name, string description, string ideaGuid, bool isAuthored
        let name = e.target.getElementsByTagName("input")[0].value;
        let description = e.target.getElementsByTagName("textarea")[0].value;
        let isAuthored = e.target.getElementsByTagName("input")[1].checked;
        let ideaGuid = e.target.dataset.idea;
       
        $.post("/idea/boxes/create", { name, description, ideaGuid, isAuthored }, (resp) => {
            $("#createBoxWindow").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        });
    });

    setGoalListeners = () => {
        // Reset listenres
        $(".removeGoalBtn").on("click", (e) => {
            e.preventDefault();

            let goalGuid = e.target.dataset.goal;

            $.post("/idea/goals/remove", { goalGuid }, (resp) => {
                if (resp) {
                    $(`.changeGoalStatusForm[data-goal="${goalGuid}"]`).parents("div")[2].remove();
                    sendNotifyMessage("Goal removed!", true);
                }
            });
        });

        $(".changeGoalStatusForm").on("submit", (e) => {
            e.preventDefault();

            let goalGuid = e.target.dataset.goal;
            let goalStatus = e.target.getElementsByTagName("select")[0].value;

            $.post("/idea/goals/update", { goalGuid, goalStatus }, (resp) => {
                // remove old
                $(`.changeGoalStatusForm[data-goal="${goalGuid}"]`).parents("div")[2].remove();

                // add update
                switch (resp.status) {
                    case 1:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-wait me-1"><span>Waiting</span></div></div></div>`);
                        break;
                    case 2:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-failed me-1"><span>Failed</span></div></div></div>`);
                        break;
                    case 0:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-complete me-1"><span>Complete</span></div></div></div>`);
                        break;
                };

                setGoalListeners();

                // notify
                sendNotifyMessage("Goal updated!", true);

            });
        });
    };

    showBoxBtn = (boxGuid) => {
        $.get("/idea/boxes/detail", { boxGuid }, resp => {

            $("#boxDetailTitle").text(resp.title);
            $("#boxDetailDescription").text(resp.description);
            $("#boxDetailAuthorAvatar").attr("src", "/media/userAvatars/" + resp.authorAvatarImage);
            $("#boxDetailPublish").text(resp.datePublished)

            $("#boxNewGoalForm").attr("data-box", boxGuid);

            resp.goals.forEach(item => {
                if (item.canEdit == true) {
                    switch (item.status) {
                        case 1:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p><form class="changeGoalStatusForm" data-goal="${item.guid}"><select class="form-select form-control"><option selected>Wait</option><option value="1">Failed</option><option value="2">Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${item.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-wait me-1"><span>Waiting</span></div></div></div>`);
                            break;
                        case 2:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p><form class="changeGoalStatusForm" data-goal="${item.guid}"><select class="form-select form-control"><option selected>Wait</option><option value="1">Failed</option><option value="2">Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${item.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-failed me-1"><span>Failed</span></div></div></div>`);
                            break;
                        case 0:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p><form class="changeGoalStatusForm" data-goal="${item.guid}"><select class="form-select form-control"><option selected>Wait</option><option value="1">Failed</option><option value="2">Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${item.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-complete me-1"><span>Complete</span></div></div></div>`);
                            break;
                    };
                }
                else if (item.canEdit == false) {
                    switch (item.status) {
                        case 1:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p></div><div class="mb-0 t-tag t-goal-wait me-1"><span>Waiting</span></div></div></div>`);
                            break;
                        case 2:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p></div><div class="mb-0 t-tag t-goal-failed me-1"><span>Failed</span></div></div></div>`);
                            break;
                        case 0:
                            $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${item.authorGuid}"><img src="/media/userAvatars/${item.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${item.authorName}</span>${item.content}</p></div><div class="mb-0 t-tag t-goal-complete me-1"><span>Complete</span></div></div></div>`);
                            break;
                    };
                }
            });

            setGoalListeners();

            $("#boxWindow").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");
        });
    };
   
    $("#boxNewGoalForm").on("submit", (e) => {
        e.preventDefault();

        let boxGuid = e.target.dataset.box;
        let content = e.target.getElementsByTagName("input")[0].value;

        $.post("/idea/boxes/creategoal", { boxGuid, content }, (resp) => {
            if (resp.canEdit == true) {
                switch (resp.status) {
                    case 1:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-wait me-1"><span>Waiting</span></div></div></div>`);
                        break;
                    case 2:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-failed me-1"><span>Failed</span></div></div></div>`);
                        break;
                    case 0:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p><form class="changeGoalStatusForm" data-goal="${resp.guid}"><select class="form-select form-control"><option value="1">Wait</option><option value="2">Failed</option><option value="0" selected>Complete</option></select><button type="submit" class="btn t-primary">Save</button><button data-goal="${resp.guid}" class="btn text-muted removeGoalBtn">Remove goal</button></form></div><div class="mb-0 t-tag t-goal-complete me-1"><span>Complete</span></div></div></div>`);
                        break;
                };
            }
            else if (resp.canEdit == false) {
                switch (resp.status) {
                    case 1:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p></div><div class="mb-0 t-tag t-goal-wait me-1"><span>Waiting</span></div></div></div>`);
                        break;
                    case 2:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p></div><div class="mb-0 t-tag t-goal-failed me-1"><span>Failed</span></div></div></div>`);
                        break;
                    case 0:
                        $("#boxContentWrapper").append(`<div class="mb-3 topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}" /></a><div class="boxGoalContent"><div><p class="col mb-0 t-semi-bold t-muted"><span class="pe-1">${resp.authorName}</span>${resp.content}</p></div><div class="mb-0 t-tag t-goal-complete me-1"><span>Complete</span></div></div></div>`);
                        break;
                };
            }

            setGoalListeners();

            sendNotifyMessage("Goal created!", true);
        });
    });

    hideBoxWindow = () => {
        $(".topicDiscussMessage").remove();
        $("#boxDetailTitle").text("");
        $("#boxDetailDescription").text("");
        $("#boxDetailAuthorAvatar").attr("src", "");
        $("#boxDetailPublish").text("")
        $("#boxNewGoalForm").attr("data-box", "");
        $("#boxWindow").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").removeClass("overflow-hidden");
    };

    $(".hideBoxBtn").on("click", (e) => {
        e.preventDefault();
        hideBoxWindow();        
    });

    // -----
    
    //remove idea
    $(".showHideRemoveIdea").on("click", (e) => {
        e.preventDefault();
        $("#settingsRemoveIdeaWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    // settings modders
    $("#hideSettingsModders").on("click", (e) => {
        e.preventDefault();

        $("#settingsModdersWindow").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").removeClass("overflow-hidden");
    });
    
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
    // ----

    // Topic - create
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

    // Topic - new comment
    $("#topicCommentForm").on("submit", (e) => {
        e.preventDefault();

        let topicGuid = e.target.dataset.topic;
        let message = e.target.getElementsByTagName("input")[0].value;

        $.post("/idea/topics/createcomment", { topicGuid, message }, (resp) => {
            $("#topicDetailComments").append(`<div class="topicDiscussMessage"><a href="/user/${resp.authorGuid}"><img src="/media/userAvatars/${resp.authorAvatarImage}"/></a><p><span class="pe-1">${resp.authorName}</span>${resp.comment}<br><span class="date-publ-span t-sm text-muted">${resp.dateCreated}</span></p></div>`);

            $("#topicCommentForm input").val("");
        });
    });

    // Topic - close
    closeIdeaTopic = () => {
        $("#topicDetailTitle").text("");
        $("#topicDetailDescription").text("");
        $(".topicDiscussMessage").remove();
        $("#topicWindow").addClass("d-none");
        $("#hideBackgroundWrapper").addClass("d-none");
        $("body").removeClass("overflow-hidden");
    };
    $(".hideTopicBtn").on("click", (e) => {
        e.preventDefault();
        closeIdeaTopic();
    });

    // Topic - show
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
    };
    // ----

    
});
