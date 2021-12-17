$(document).ready(() => {
    $(".showHideInvite").on("click", () => {
        $("#inviteWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
});
