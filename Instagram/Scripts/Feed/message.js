// Xử lý khi user click vào item message bên sidebar
function userItemClick(elementId, fullName, userId) {
    //remove all pre-existing active classes
    $('.list-group-item').removeClass('active');

    //add the active class to the link we clicked
    $("#" + elementId).addClass('active');

    event.preventDefault();

    //Lưu các giá trị vào hidden field để sử dụng lại khi submit form
    $("#userId, #toUserId").val(userId);
    $("#fullName").val(fullName);

    $("#userNameTitle").show();
    $("#toUserName").hide();

    //Thực hiện submit form lên mvc action để load lại message theo người dùng 
    $("#historyMessagePane").submit();
}

//Ẩn hiện các control khi người dùng click vào Tạo tin nhắn mới
function newMessageClick() {
    $("#userNameTitle").hide();
    $("#toUserName").val('');
    $("#toUserName").show().focus();
}

//Cấu hình autocomplete cho input text
$("#toUserName").autocomplete({
    //Data source của autocomplete được lấy từ mvc action
    source: function (request, response) {
        var users = new Array();
        $.ajax({
            type: "POST",
            url: "/Message/GetUsers",
            data: { "term": request.term },
            success: function (data) {
                for (var i = 0; i < data.length ; i++) {
                    users[i] = { label: data[i].Value, Id: data[i].Key };
                }
                response(users);
            }
        });
    },
    //Xử lý khi chọn 1 người dùng để gửi tin nhắn thì sẽ load lại nội dung message giữa 2 người
    select: function (event, ui) {
        $("#userNameTitle").show();
        $("#toUserName").hide();

        $("#userId, #toUserId").val(ui.item.Id);
        $("#fullName, #toUserName").val(ui.item.value);

        $("#historyMessagePane").submit();
    }
});


/*
 * Các hàm xử lý gửi tin nhắn realtime
 */

//// Declare a proxy to reference the hub. 
var messageHub = $.connection.messageHub;

$(function () {

    //Hàm test thử nhận message gửi cho tất cả client
    //messageHub.client.clientHello = function () {
    //    alert("Hello from server call");
    //};

    // Start Hub
    $.connection.hub.start().done(function () {
        //messageHub.server.serverHello();
        registerEvents(messageHub);
    });

    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Re-start connection after 5 seconds
    });

    setInterval(ResetTypingFlag, 1000);

    function registerEvents(messageHub) {
        //Lấy userId của người dùng đang đăng nhập
        var UserID = $("#currentUserId").val();
        //Gọi hàm đăng kí kết nối đến hub
        messageHub.server.connect(UserID);
    }

});

var TypingFlag = true;

function ResetTypingFlag() {
    TypingFlag = true;
}

messageHub.client.ReceiveTypingRequest = function (toUserId) {
    var userId = $('#toUserId').val();
    //Chỉ hiển thị typing indicator khi người nhận đang ở màn hình chat này
    if (userId === toUserId) {
        $('#typing').show();
        $('#typing').delay(1000).fadeOut("slow");
    }
}

// Calls when user successfully logged in
messageHub.client.onConnected = function (id, allUsers) {

}

// On New User Connected: Có thể xử lý đổi status -> online
messageHub.client.onNewUserConnected = function (userid) {

}

// On New User Disconnected: Có thể xử lý đổi status -> offline
messageHub.client.onUserDisconnected = function (userid) {

}

//Xử lý khi nhận được message được gửi từ Hub đến
messageHub.client.receivedPrivateMessage = function (message) {
    //alert(message);
    var userId = $('#toUserId').val();
    var logginUserId = $('#currentUserId').val();
    //Chỉ append vào messageList khi là chính người gửi hoặc người nhận
    if (logginUserId === message.FromUserId || (userId === message.FromUserId && logginUserId === message.ToUserId)) {
        //Thay thế giá trị message object từ server gửi về
        var messageDiv = $("#message-template").html().replace(/\{0}/g, message.UserName);
        messageDiv = messageDiv.replace(/\{1}/g, message.Avatar);
        messageDiv = messageDiv.replace(/\{2}/g, message.FullName);
        messageDiv = messageDiv.replace(/\{3}/g, message.CreateDate);
        messageDiv = messageDiv.replace(/\{4}/g, message.Body);
        //Thêm vào messageList
        $("#messageList").append(messageDiv);
    }
}

//Hàm xử lý khi người dùng typing vào input để gửi message
function messageTextTyping(e) {
    if (e.keyCode == 13) {//Bấm phím enter
        var message = $('#messageText').val();
        var userId = $('#toUserId').val();

        $.ajax({
            type: "POST",
            url: "/Message/SendMessage",
            data: { "toUserId": userId, "message": message },
            success: function (messageId) {
                if (messageId != null) {
                    messageHub.server.sendPrivateMessage(userId, message, messageId);
                    //Xóa trắng input message sau khi gửi xong
                    $('#messageText').val('');
                }
            }
        });
    }
    if (TypingFlag == true) {
        var userId = $('#toUserId').val();
        messageHub.server.sendUserTypingRequest(userId);
    }
    TypingFlag = false;
}

