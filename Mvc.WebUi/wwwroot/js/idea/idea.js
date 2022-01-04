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

    // settings modders
    $("#showSettingsModders").on("click", (e) => {
        e.preventDefault();

        $("#settingsModdersWindow").removeClass("d-none");
        $("#hideBackgroundWrapper").removeClass("d-none");
        $("body").addClass("overflow-hidden");

        $.get("/load/ideaRoles", (resp) => {
            console.log(resp);

            //FReACH APPEND
        })
    })

    /*
    $(".showHideSettingsModders").on("click", (e) => {
        e.preventDefault();
        $("#settingsModdersWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
    */

    

    // create topic
    $("#formCreateTopic").on("submit", (e) => {
        e.preventDefault();

        let inputs = e.target.getElementsByTagName("input");
        let ideaGuid = inputs[0].value;
        let title = inputs[1].value;
        let description = e.target.getElementsByTagName("textarea")[0].value;

        $.post("/idea/createTopic", { title, description, ideaGuid }, response => {
            if (response == true) {
                $("#settingsCreateTopicWindow").toggleClass("d-none");
                $("#hideBackgroundWrapper").toggleClass("d-none");
                $("body").toggleClass("overflow-hidden");
                sendNotifyMessage("Topic created!", true);
            }
            else {
                $("#settingsCreateTopicWindow").toggleClass("d-none");
                $("#hideBackgroundWrapper").toggleClass("d-none");
                $("body").toggleClass("overflow-hidden");
                sendNotifyMessage("Something failed", true);
            }
        });
    });
    $(".showHideCreateTopic").on("click", (e) => {
        e.preventDefault();
        $("#settingsCreateTopicWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
    // ----

    $(".showHideRemoveIdea").on("click", (e) => {
        e.preventDefault();
        $("#settingsRemoveIdeaWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
});
