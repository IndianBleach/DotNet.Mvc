$(document).ready(() => {

    // CONNECTION SETTINGS
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    let _connectionId = '';

    connection.start()
        .then(() => {
            console.log("Connect open");
            connection.invoke("getConnectionId")
                .then(connect => {
                    _connectionId = connect;
                });
        })
        .catch(() => {
            console.log("Connect failed");
        });
    //END OF CONNECTION SETTINGS

    // SHOW NEW-CHAT WINDOW
    $(".showNewChatWindow").on("click", (e) => {
        $.post("/chat/newchats", {}, (resp) => {
            
            resp.forEach(item => {
                $("#newChatLoad").append(`<form data-user="${item.userGuid}" class="newChatForm"><div class="newChatChoiceSection"><button type="submit" class="btn newChatBtn"><img src="media/userAvatars/${item.avatarImageName}" /><p>${item.userName}</p><span class="t-muted t-sm">Let's chat</span></button></div></form>`)
            })            

            $("#newChatWindow").toggleClass("d-none");
            $("#hideBackgroundWrapper").toggleClass("d-none");
            $("body").toggleClass("overflow-hidden");

            // request
            $(".newChatForm").on("submit", (e) => {
                e.preventDefault();
                let userGuid = e.target.dataset.user;
                let elems = e.target.children[0].children[0].children;
                let userAvatarImage = elems[0].getAttribute("src");                
                let userName = elems[1].textContent;

                // remove prev created chat
                $(".chatCreated").remove();

                $("#allChatsContainer").append(`<section><form data-chat="0" data-user="${userGuid}" class="chatSection chatCreated"><button type="submit" class="p-0 btn chatUserLink"><div class="sectionUserChat"><img src="${userAvatarImage}"/><div class="ps-2"><p>${userName}</p><div class="text-truncate text-start"><span>Write first message!</span></div></div></div></button></form></section>`)

                // close window
                $(".newChatForm").remove();
                $("#newChatWindow").toggleClass("d-none");
                $("#hideBackgroundWrapper").toggleClass("d-none");
                $("body").toggleClass("overflow-hidden");                                

                loadNewChat(userAvatarImage, userName, userGuid);

                // set NEW listener
                $(".chatSection").on("submit", (e) => {
                    e.preventDefault();

                    let chatGuid = e.target.dataset.chat;

                    let userGuid = e.target.dataset.user;
                    let avatar = e.target.getElementsByTagName("img")[0].src;
                    let userName = e.target.getElementsByTagName("p")[0].textContent;

                    if (chatGuid != 0 & chatGuid != null & chatGuid != "undefined") {

                        $.get("chat/getDetail", { chatGuid }, (resp) => {
                            console.log(resp);

                            loadExistChat(avatar, userName, userGuid, chatGuid, resp.messages);

                            let authorAvatarSrc = "/media/userAvatars/" + resp.authorInfo.avatarImageName;
                            let authorUserName = resp.authorInfo.userName;
                            let authorGuid = resp.authorInfo.guid;

                            setAuthorSessionData(authorUserName, authorAvatarSrc, authorGuid);
                        })

                    }
                    else {
                        loadNewChat(avatar, userName, userGuid);
                    }
                });
            });
            //
        })
    });

    // HIDE NEW-CHAT WINDOW
    $(".hideNewChatWindow").on("click", () => {
        $(".note-newchat").remove();
        $("#newChatWindow").toggleClass("d-none");
        $("#hideBackgroundWrapper").toggleClass("d-none");
        $("body").toggleClass("overflow-hidden");
    });
    
    // CHAT LOAD: New
    const loadNewChat = (avatar, username, userGuid) => {

        $("#existChatContainer").addClass("d-none");

        // open active chat
        $("#choiceChatDefault").addClass("d-none");
        $("#activeChat").removeClass("d-none");
        $("#chatInviteUserAvatar").attr("src", avatar)
        $("#chatUserName").text(username);
        $("#beginChatContainer").removeClass("d-none");

        sessionStorage.setItem("chatWith", userGuid);
        sessionStorage.setItem("chatGuid", 0);

        //$("#beginChatContainer").addClass("d-none");
        //$("#existChatContainer").removeClass("d-none");
    };

    // CHAT LOAD: Exist
    const loadExistChat = (avatar, username, userGuid, chatGuid, messages) => {
        //remove prev
        $(".messageWrapper").remove();

        $("#beginChatContainer").addClass("d-none");
        $("#existChatContainer").removeClass("d-none");

        // open active chat
        $("#choiceChatDefault").addClass("d-none");
        $("#activeChat").removeClass("d-none");
        $("#chatInviteUserAvatar").attr("src", avatar)
        $("#chatUserName").text(username);
        $("#beginChatContainer").removeClass("d-none");

        sessionStorage.setItem("chatWith", userGuid);
        sessionStorage.setItem("chatGuid", chatGuid);

        // set session AUTHOR data
        sessionStorage.setItem("authorAvatar", avatar);
        sessionStorage.setItem("authorUserName", username);

        messages.forEach(x => {
            if (x.isAuthorMessage) {
                $("#existChatContainer").append(`<div class="messageWrapper myMessage"><div><a href="/user/${x.authorName}"><img src="/media/userAvatars/${x.avatarImageName}" /></a><p>${x.message}</p></div></div>`)
            }
            else {
                $("#existChatContainer").append(`<div class="messageWrapper"><div><a href="/user/${x.authorName}"><img src="/media/userAvatars/${x.avatarImageName}" /></a><p>${x.message}</p></div></div>`)
            }
        })

        $("#beginChatContainer").addClass("d-none");
        $("#existChatContainer").removeClass("d-none");
    };

    // AFTER SENDED MESSAGE
    const setFakeChatToExist = (chatGuid, userGuid) => {
        $(".chatSection[data-chat=0]")[0].setAttribute("data-chat", chatGuid);
        sessionStorage.setItem("chatGuid", chatGuid);
        sessionStorage.setItem("chatWith", userGuid);
    };

    // NEW MESSAGE HANDLER
    $("#newMessageForm").on("submit", (e) => {
        e.preventDefault();

        let message = e.target.getElementsByTagName("input")[0].value;
        let chatGuid = sessionStorage.getItem("chatGuid");

        sessionStorage.setItem("isMyMessage", true);

        if (chatGuid != 0) {

            // send message to exist
            $.post("chat/sendMessage", { chatGuid, message }, (resp) => {

                $("#newMessageInput").val("");

                if (resp) {

                    let avatarSrc = sessionStorage.getItem("authorAvatar");
                    let userName = sessionStorage.getItem("authorUserName");

                    //$("#existChatContainer").append(`<div class="messageWrapper myMessage"><div><a href="/user/${userName}"><img src="${avatarSrc}" /></a><p>${message}</p></div></div>`);

                }
            })

        }
        else {

            let toUserGuid = sessionStorage.getItem("chatWith");
            $.post("chat/create", { toUserGuid, message }, (resp) => {

                setFakeChatToExist(resp.guid, toUserGuid);
                loadExistChat("media/userAvatars/" + resp.avatarImageName, resp.userName, resp.userGuid, resp.guid, resp.messages);
            })

        }
    })

    // SESSION: Author
    const setAuthorSessionData = (userName, avatarSrc, guid) => {
        sessionStorage.setItem("authorUserName", userName);
        sessionStorage.setItem("authorGuid", guid);
        sessionStorage.setItem("authorAvatarSrc", avatarSrc);
    }    

    // SESSION: Member
    const setChatMemberSessionData = (userGuid, userAvatar) => {
        sessionStorage.setItem("chatUserGuid", userGuid);
        sessionStorage.setItem("chatUserAvatar", userAvatar);
    };

    // set OLD listener
    $(".chatSection").on("submit", (e) => {
        e.preventDefault();

        let chatGuid = e.target.dataset.chat;

        let userGuid = e.target.dataset.user;
        let avatar = e.target.getElementsByTagName("img")[0].src;
        let userName = e.target.getElementsByTagName("p")[0].textContent;

        setChatMemberSessionData(userGuid, avatar);

        if (chatGuid != 0 & chatGuid != null & chatGuid != "undefined") {
            $.get("chat/getDetail", { chatGuid }, (resp) => {                

                loadExistChat(avatar, userName, userGuid, chatGuid, resp.messages);

                let authorAvatarSrc = "/media/userAvatars/" + resp.authorInfo.avatarImageName;
                let authorUserName = resp.authorInfo.userName;
                let authorGuid = resp.authorInfo.guid;

                console.log(authorGuid);
                setAuthorSessionData(authorUserName, authorAvatarSrc, authorGuid);
                
                // JOIN
                joinChat(chatGuid);
            })

        }
        else {
            loadNewChat(avatar, userName, userGuid);
        }
    });

    // SEND MESSAGE
    connection.on("RecieveMessage", (message) => {
        let avatar = sessionStorage.getItem("chatUserAvatar");
        let userGuid = sessionStorage.getItem("chatUserGuid");        

        let isMy = sessionStorage.getItem("isMyMessage");
        if (isMy) {
            $("#existChatContainer").append(`<div class="messageWrapper myMessage"><div><a href="/user/${userGuid}"><img src="${avatar}" /></a><p>${message.text}</p></div></div>`)
        }
        else {
            $("#existChatContainer").append(`<div class="messageWrapper"><div><a href="/user/${userGuid}"><img src="${avatar}" /></a><p>${message.text}</p></div></div>`)
        }

        sessionStorage.removeItem("isMyMessage");
    });
   
    // JOIN ROOM
    const joinChat = (chatGuid) => {

        let url = "chat/join/" + _connectionId + "/" + chatGuid;
        $.post(url, null, (resp) => {
            console.log("Join chat stabile", resp);
        })
    };    
});
