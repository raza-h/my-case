
var connectedusers = [];
//const connection = new signalR.HubConnectionBuilder().withUrl("/cms/messageHub").build();
const connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();


connection.start().then(() => {
    connection.invoke("GetConnectedUsers").then((result) => {
        connectedusers = result;
    }).catch(err => console.error(err.toString()));
}).catch(function (err) {
    //failed to connect
    return console.error(err.toString());
});

connection.on("UserConnected", (allusersconnected) => {
    connectedusers = allusersconnected;
    var otherUserId = $(".selected-contact #other-userid").val();
    var contactType = $(".selected-contact #contact-type").val();
    if (contactType == "individual") {
        if (connectedusers.includes(otherUserId))
            $("#contact-online").text('online');
        else
            $("#contact-online").text('offline');
    }
});

//This method receive the message and Append to our list  
connection.on("ReceiveMessage", (user, message) => {
    var currentUserId = $("#current-userid").val();
    var isNewContact = true;
    var unreadCount = 0;
    var model = JSON.parse(message);
    if (model.IsGroupMessage == true) {
        //for group
        if ($(".selected-contact #groupid").val() == model.GroupId) {
            isNewContact = false;
            var html = "";
            if (model.Image != null && model.Image.length > 0)
                html = `<div class='d-flex justify-content-start mb-4'><div class='img_cont_msg'><img src='${model.UserImagePath}' class='rounded-circle user_img_msg'>
                        </div><div class='msg_cotainer' style='min-width:110px;'><img src="data:image/png;base64,${model.Image}" class="chat-image"><br/>${model.MessageText}
                        <span class='msg_time'>just now</span></div></div>`;
            else
                html = `<div class='d-flex justify-content-start mb-4'><div class='img_cont_msg'><img src='${model.UserImagePath}' class='rounded-circle user_img_msg'></div>
                        <div class='msg_cotainer' style='min-width:110px;'>${model.MessageText}<span class='msg_time'>just now</span></div></div>`;
            $("#private-chat").append(html);
            $(".selected-contact #last-seen").text('Just now');
            connection.invoke("ReadGroupMessage", currentUserId, model.GroupId, model.Id).then(() => {
            }).catch(err => alert(err.toString()));
        }
        else {
            $('#message-contacts li').each(function () {
                if ($(this).find('#groupid').val() == model.GroupId) {
                    isNewContact = false;
                    if ($.isNumeric($(this).find('#unread-message').text().charAt(0))) {
                        unreadCount = $(this).find('#unread-message').text().charAt(0);
                        unreadCount = parseInt(unreadCount) + 1;
                        $(this).find('#unread-message').text(`${unreadCount} Unread messages`)
                    }
                    else {
                        $(this).find('#unread-message').text('1 Unread message');
                    }
                    $(this).find('#unread-message').removeClass('d-none');
                    $(this).find('#last-seen').addClass('d-none');
                }
            });
        }
        if (isNewContact) {
            var html = `<li class="contact c-pointer" onclick="loadChat(this)" id="contact-${model.GroupId}"><input type="hidden" id="contact-type" value="group" />
                        <input type="hidden" id="groupid" value="${model.GroupId}" />`;
            html += `<input type="hidden" id="senderid" value="${model.SenderId}" /><input type="hidden" id="contact-name-user" value="${model.GroupName}"/>
                    <div class="d-flex bd-highlight"><div class="img_cont">`;
            html += `<img src="data:image/png;base64,${model.Image}" class="rounded-circle user_img"></span></div>`;
            html += `<div class="user_info"><span>${model.GroupName}</span><p class="d-none" style="margin-bottom: 0px" id="last-seen"></p><p class="fw-bold" style="margin-bottom: 0px" id="unread-message">1 unread message</p>
                    </div><div style="margin-left: auto; align-self: center;"><button onclick="toggleArchive(event, '', '', 'archive', '${model.GroupId}', '${model.GroupId}')" class="btn" style="border: 1px solid black; color: black !important; ">
                    Archive</button></div></div></li></div></li></div></div></li>`;
            $("#unarchived-contacts").prepend(html);
        }
    }
    else {
        //for user to user
        if ($(".selected-contact #other-userid").val() == model.SenderId) {
            isNewContact = false
            var html = "";
            if (model.Image != null && model.Image.length > 0)
                html = `<div class='d-flex justify-content-start mb-4'><div class='img_cont_msg'><img src='${model.UserImagePath}' class='rounded-circle user_img_msg'>
                        </div><div class='msg_cotainer' style='min-width:110px;'><img src="data:image/png;base64,${model.Image}" class="chat-image"><br/>${model.MessageText}
                        <span class='msg_time'>just now</span></div></div>`;
            else
                html = `<div class='d-flex justify-content-start mb-4'><div class='img_cont_msg'><img src='${model.UserImagePath}' class='rounded-circle user_img_msg'></div>
                    <div class='msg_cotainer' style='min-width:110px;'>${model.MessageText}<span class='msg_time'>just now</span></div></div>`;
            $("#private-chat").append(html);
            $(".selected-contact #last-seen").text('Just now');
            connection.invoke("ReadMessage", model.Id).then(() => {
            }).catch(err => alert(err.toString()));
        }
        else {
            $('#message-contacts li').each(function () {
                if ($(this).find('#other-userid').val() == model.SenderId && $(this).find('#contact-type').val() != 'group') {
                    isNewContact = false;
                    if ($.isNumeric($(this).find('#unread-message').text().charAt(0))) {
                        unreadCount = $(this).find('#unread-message').text().charAt(0);
                        unreadCount = parseInt(unreadCount) + 1;
                        $(this).find('#unread-message').text(`${unreadCount} Unread messages`)
                    }
                    else {
                        $(this).find('#unread-message').text('1 unread message');
                    }
                    $(this).find('#unread-message').removeClass('d-none');
                    $(this).find('#last-seen').addClass('d-none');
                }
            });
        }
        if (isNewContact) {
            var html = `<li class="contact c-pointer" onclick="loadChat(this)" id="contact-${model.MessageId}"><input type="hidden" id="contact-type" value="individual"/>
                        <input type="hidden" id="senderid" value="${model.SenderId}" /><input type="hidden" id="other-userid" value="${model.SenderId}" />`;
            html += `<input type="hidden" id="receiverid" value="${model.ReceiverId}"/><input type="hidden" id="contact-name-user" value="${user}"/>
                    <div class="d-flex bd-highlight"><div class="img_cont">`;
            html += `<img src="${model.UserImagePath}" class="rounded-circle user_img"></div>`;
            html += `<div class="user_info"><span>${user}</span><p class="d-none" style="margin-bottom: 0px" id="last-seen"></p><p class="fw-bold" style="margin-bottom: 0px" id="unread-message">1 unread message</p></div>
                    <div style="margin-left: auto; align-self: center;"><button onclick="toggleArchive(event, '${message.SenderId}', '${message.ReceiverId}', 'archive', '${message.GroupId}', '${message.Id}')" class="btn" style="border: 1px solid black; color: black !important; ">
                    Archive</button></div></div></li>`;
            $("#unarchived-contacts").prepend(html);
        }
    }
});

function reply(userid, imagePath) {
    var contactType = $(".selected-contact #contact-type").val();
    var groupId = $(".selected-contact #groupid").val();

    if (contactType == "individual")
        sendReply(userid);
    else
        sendReplyInGroup(userid, groupId);
}
//Send message to single users 
async function sendReply(userid) {

    var message = $("#message-text").val();
    var selectedImage = $("#message-file");
    if (message == "" && selectedImage && selectedImage.get(0).files.length < 1)
        return;
    var senderId = $(".selected-contact #senderid").val();
    var receiverId = $(".selected-contact #receiverid").val();
    if (userid == receiverId)
        receiverId = senderId;

    var imageHtml = "";
    var image;
    if (selectedImage && selectedImage.get(0).files.length > 0) {
        let myFile = document.getElementById('message-file').files[0];
        var reader = new FileReader();
        reader.onload = function () {
            imageHtml = `<img src="${reader.result}" class="chat-image"><br/>`;
        }
        reader.readAsDataURL(myFile);
        //Wait for the file to be converted to a byteArray
        image = await fileToByteArray(myFile);
    };
    var model = {
        MessageText: $("#message-text").val(),
        SenderId: senderId,
        ReceiverId: receiverId,
        Image: image,
        UserImagePath: $("#user-profile-layout").find('img').attr("src")
    };
    var modelData = JSON.stringify(model);
    connection.invoke("Send", modelData).then(() => {
        var html = "";

        if (model.Image != null && model.Image.length > 0) {
            html = `<div class="d-flex justify-content-end mb-4"><div class="msg_cotainer_send">${imageHtml}${message}<span class="msg_time_send">just now</span>
                    </div><div class="img_cont_msg"><img src="${model.UserImagePath}" class="rounded-circle user_img_msg"></div></div>`;
        }
        else {
            html = `<div class="d-flex justify-content-end mb-4"><div class="msg_cotainer_send" style="min-width:110px;">${message}
                   <span class="msg_time_send">just now</span></div><div class="img_cont_msg"><img src="${model.UserImagePath}" class="rounded-circle user_img_msg"></div></div>`;
        }
        $("#private-chat").append(html);
        $(".selected-contact #last-seen").text('Just now');
        $(".selected-contact #last-seen").removeClass('d-none');
        $("#message-text").val('');
        $("#message-file").val('');
        if ($("#private-chat")[0])
            $("#private-chat").animate({ scrollTop: $("#private-chat")[0].scrollHeight }, 10);
    }).catch(err => alert(err.toString()));
};

async function sendReplyInGroup(userid, groupId) {

    var message = $("#message-text").val();
    var selectedImage = $("#message-file");
    if (message == "" && selectedImage && selectedImage.get(0).files.length < 1)
        return;

    var imageHtml = "";
    var image;
    var imagePath = $("#user-profile-layout").find('img').attr("src");
    if (selectedImage && selectedImage.get(0).files.length > 0) {
        let myFile = document.getElementById('message-file').files[0];
        var reader = new FileReader();
        reader.onload = function () {
            imageHtml = `<img src="${reader.result}" class="chat-image"><br/>`;
        }
        reader.readAsDataURL(myFile);
        //Wait for the file to be converted to a byteArray
        image = await fileToByteArray(myFile);
    };
    var model = {
        MessageText: message,
        SenderId: userid,
        GroupId: groupId,
        Image: image,
        GroupName: $(".selected-contact #contact-name-user").val()
    };
    var modelData = JSON.stringify(model);
    connection.invoke("SendMessageInGroup", modelData).then(() => {
        var html = "";

        if (model.Image != null && model.Image.length > 0) {
            html = `<div class="d-flex justify-content-end mb-4"><div class="msg_cotainer_send">${imageHtml}${message}<span class="msg_time_send">just now</span>
                    </div><div class="img_cont_msg"><img src="${imagePath}" class="rounded-circle user_img_msg"></div></div>`;
        }
        else {
            html = `<div class="d-flex justify-content-end mb-4"><div class="msg_cotainer_send" style="min-width:110px;">${message}
                    <span class="msg_time_send">just now</span></div><div class="img_cont_msg"><img src="${imagePath}" class="rounded-circle user_img_msg">
                    </div></div>`;
        }
        $("#private-chat").append(html);
        $(".selected-contact #last-seen").text('Just now');
        $("#message-text").val('');
        $("#message-file").val('');
        if ($("#private-chat")[0])
            $("#private-chat").animate({ scrollTop: $("#private-chat")[0].scrollHeight }, 10);
    }).catch(err => alert(err.toString()));
};

function initiateChat(contactName, senderId, receiverId, imagePath) {
    var Id = "";
    $("#message-contacts .active").removeClass("active selected-contact");

    $('#message-contacts li').each(function () {
        if ($(this).find('#other-userid').val() == receiverId && $(this).find('#contact-type').val() == 'individual') {
            Id = receiverId;
            $(this).addClass("active selected-contact");
        }
    });
    if (Id != receiverId) {
        var html = `<li class="contact active selected-contact c-pointer" onclick="loadChat(this)"><input type="hidden" id="contact-type" value="individual"/>
                    <input type="hidden" id="senderid" value="${senderId}" /><input type="hidden" id="other-userid" value="${receiverId}" />`;
        html += `<input type="hidden" id="receiverid" value="${receiverId}"/><input type="hidden" id="contact-name-user" value="${contactName}"/>
                <div class="d-flex bd-highlight"><div class="img_cont">`;
        html += `<img src="${imagePath}" class="rounded-circle user_img"></div>`;
        html += `<div class="user_info"><span>${contactName}</span><p class="d-none" style="margin-bottom: 0px" id="last-seen"></p>
                <p class="fw-bold" style="margin-bottom: 0px" id="unread-message"></p></div></div></li>`;
        $("#unarchived-contacts").prepend(html);
    }
    var imageSource = $(".selected-contact").find('img').attr("src");

    $.ajax({
        type: 'GET',
        data: { senderId: senderId, receiverId: receiverId },
        url: "http://localhost:25601/" + "Attorney/Message/LoadChatMessages"
        //url: "http://localhost:25601/" + "Attorney/Admin/LoadChatMessages"
    }).done(function (data) {
        $(".users-chat").html(data);
        $("#chat-with-name").text(contactName);
        $("#user-profile-image").attr("src", imageSource);
        if ($("#contact-type").val() == "individual") {
            if (connectedusers.includes(receiverId))
                $("#contact-online").text('online');
            else
                $("#contact-online").text('offline');
        }
    });
};

function loadChat(elem) {
    var url = "";
    if (elem != undefined) {
        $("#message-contacts .active").removeClass("active selected-contact");
        $(elem).addClass("active selected-contact");
        if ($(elem).find('#last-seen').text() == 'new message arrived') {
            $(elem).find('#last-seen').text('Last chat Just now')
        }
    }
    var contactType = $(".selected-contact #contact-type").val();
    var groupId = $(".selected-contact #groupid").val();
    var senderId = $(".selected-contact #senderid").val();
    var receiverId = $(".selected-contact #receiverid").val();
    var otherUserId = $(".selected-contact #other-userid").val();
    if (contactType == "individual") {
        //url: baseURL + "Attorney/Message/LoadChatMessages"
        url = "http://localhost:25601/" + "Attorney/Message/LoadChatMessages"
        //url: "http://localhost:25601/" + "Attorney/Admin/LoadChatMessages"
    }
    else {
        //url: baseURL + "Attorney/Message/LoadGroupMessages"
        url = "http://localhost:25601/" + "Attorney/Message/LoadGroupMessages"
        //url: "http://localhost:25601/" + "Attorney/Admin/LoadGroupMessages"
    }
    var imageSource = $(".selected-contact").find('img').attr("src");
    if (contactType == "individual") {
        $.ajax({
            type: 'GET',
            data: { senderId: senderId, receiverId: receiverId },
            url: url,
        }).done(function (data) {
            $(".users-chat").html(data);
            $("#chat-with-name").text($(".selected-contact #contact-name-user").val());
            $(".selected-contact #unread-message").addClass('d-none');
            $(".selected-contact #unread-message").text('');
            $(".selected-contact #last-seen").removeClass('d-none');
            if ($('#private-chat .mb-4').last().find('.msg_time_send').length > 0)
                $(".selected-contact #last-seen").text($('#private-chat .mb-4').last().find('.msg_time_send').text());
            else
                $(".selected-contact #last-seen").text($('#private-chat .mb-4').last().find('.msg_time').text());
            if (contactType == "individual") {
                if (connectedusers.includes(otherUserId))
                    $("#contact-online").text('online');
                else
                    $("#contact-online").text('offline');
            }
            $("#user-profile-image").attr("src", imageSource);
            if ($("#private-chat")[0])
                $("#private-chat").animate({ scrollTop: $("#private-chat")[0].scrollHeight }, 10);
        });
    } else {
        $.ajax({
            type: 'GET',
            data: { groupId: groupId },
            url: url,
        }).done(function (data) {
            $(".users-chat").html(data);
            $("#chat-with-name").text($(".selected-contact #contact-name-user").val());
            $(".selected-contact #unread-message").addClass('d-none');
            $(".selected-contact #unread-message").text('');
            $(".selected-contact #last-seen").removeClass('d-none');
            if ($('#private-chat .mb-4').last().find('.msg_time_send').length > 0)
                $(".selected-contact #last-seen").text($('#private-chat .mb-4').last().find('.msg_time_send').text());
            else
                $(".selected-contact #last-seen").text($('#private-chat .mb-4').last().find('.msg_time').text());
            if (contactType == "individual") {
                if (connectedusers.includes(otherUserId))
                    $("#contact-online").text('online');
                else
                    $("#contact-online").text('offline');
            }
            $("#user-profile-image").attr("src", imageSource);
            if ($("#private-chat")[0])
                $("#private-chat").animate({ scrollTop: $("#private-chat")[0].scrollHeight }, 10);
        });
    }

};

async function addGroup(userId) {
    var selectedImage = $("#group-image");
    if ($("#groupname").val() == '') {
        toastr.error("Please enter group name to create");
        return;
    }
    if ($("#chat-contacts").val().length == 0) {
        toastr.error("Please select users to create group");
        return;
    }

    var userImagePath = $("#user-profile-layout").find('img').attr("src");
    var image;
    if (selectedImage && selectedImage.get(0).files.length > 0) {
        let myFile = document.getElementById('group-image').files[0];
        var reader = new FileReader();
        reader.onload = function () {
            imageHtml = `<img src="${reader.result}" class="rounded-circle user_img">`;
        }
        reader.readAsDataURL(myFile);
        //Wait for the file to be converted to a byteArray
        image = await fileToByteArray(myFile);
    }
    else {
        toastr.error("Please select group image");
        return;
    }
    var message = {
        UserImagePath: userImagePath
    }
    var model = {
        GroupName: $("#groupname").val(),
        UserIds: $("#chat-contacts").val(),
        Image: image,
        message: message
    };
    var modelData = JSON.stringify(model);

    connection.invoke("CreateGroup", modelData).then((result) => {
        closepopups();
        $("#message-contacts .active").removeClass("active selected-contact");
        var html = `<li class="contact active selected-contact c-pointer" onclick="loadChat(this)" id="contact-${result}"><input type="hidden" id="contact-type" value="group" />
                    <input type="hidden" id="senderid" value="${userId}" />`;
        html += `<input type="hidden" id="groupid" value="${result}" /><input type="hidden" id="contact-name-user" value="${model.GroupName}"/>
                <div class="d-flex bd-highlight"><div class="img_cont">`;
        html += `${imageHtml}</div>`;
        html += `<div class="user_info"><span>${model.GroupName}</span><p style="margin-bottom: 0px" id="last-seen">just now</p>
                <p class="fw-bold" style="margin-bottom: 0px" id="unread-message"></p></div><div style="margin-left: auto; align-self: center;">
                <button onclick="toggleArchive(event, '', '', 'archive', '${result}', '${result}')" class="btn" style="border: 1px solid black; color: black !important; ">Archive</button></div></div></li>`;
        $("#unarchived-contacts").prepend(html);
        $("#chat-with-name").text(model.GroupName);
        $("#contact-online").text('');
        var imageSource = $(".selected-contact").find('img').attr("src");
        $("#user-profile-image").attr("src", imageSource);
        html = `<div class='d-flex justify-content-start mb-4'><div class='img_cont_msg'><img src='${userImagePath}' class='rounded-circle user_img_msg'>
                </div><div class='msg_cotainer' style='min-width:110px;'>You created group ${model.GroupName}<span class='msg_time'>just now</span></div></div>`;
        $("#private-chat").html(html);
    }).catch(err => alert(err.toString()));
}

function toggleArchive(e, senderId = '', receiverId = '', toDo = '', groupId = '', messageId = '') {
    if (!e) e = window.event;
    e.stopPropagation();
    var model = {
        ContactOne: senderId,
        ContactTwo: receiverId,
        GroupId: groupId
    };
    var modelData = JSON.stringify(model);
    connection.invoke("ArchiveChat", modelData, toDo).then((result) => {
        if (result) {
            if (toDo == 'archive') {
                var messageElement = $("#contact-" + messageId);
                messageElement.find("button").attr("onclick", messageElement.find("button").attr("onclick").replace('archive', 'unArchive'));
                messageElement.find("button").text('UnArchive');
                $("#archived-contacts").prepend(messageElement);
                if ($('#archived-contacts li').length > 0) {
                    $("#archived-button").removeClass('d-none');
                }
            }
            else {
                var messageElement = $("#contact-" + messageId);
                messageElement.find("button").attr("onclick", messageElement.find("button").attr("onclick").replace('unArchive', 'archive'));
                messageElement.find("button").text('Archive');
                $("#unarchived-contacts").prepend(messageElement);
                if ($('#archived-contacts li').length < 1) {
                    $("#archived-contacts").addClass('d-none');
                    $("#archived-button").addClass('d-none');
                }
            }
        }
    }).catch(err => alert(err.toString()));
};

function openGroupPopUp() {
    $("#group-modal").modal('show');
    getChatUsers();
}
function closepopups() {
    $("#group-modal").modal('hide');
    $("#chat-contacts").empty();
    $("#groupname").val('');
    $("#group-image").val('');
}

function toggleArchiveSection() {
    $("#archived-contacts").toggleClass('d-none');
}

function pickImage() {
    $("#message-file").trigger('click');
}

function fileToByteArray(file) {
    return new Promise((resolve, reject) => {
        try {
            let reader = new FileReader();
            let fileByteArray = [];
            reader.readAsArrayBuffer(file);
            reader.onloadend = (evt) => {
                if (evt.target.readyState == FileReader.DONE) {
                    let arrayBuffer = evt.target.result,
                        array = new Uint8Array(arrayBuffer);
                    for (byte of array) {
                        fileByteArray.push(byte);
                    }
                }
                resolve(fileByteArray);
            }
        } catch (e) {
            reject(e);
        }
    })
}
