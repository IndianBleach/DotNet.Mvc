$(document).ready(() => {
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

    $(".showHideSettingsModders").on("click", (e) => {
        e.preventDefault();
        $("#settingsModdersWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    $(".showHideCreateTopic").on("click", (e) => {
        e.preventDefault();
        $("#settingsCreateTopicWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });

    $(".showHideRemoveIdea").on("click", (e) => {
        e.preventDefault();
        $("#settingsRemoveIdeaWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
});
