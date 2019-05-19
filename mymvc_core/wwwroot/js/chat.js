"use strict";


//  ---------这是微软原本的实例聊天室的代码

//这是在引用代理。（集线器Hub）
//var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

////连接建立后才能发送消息
//document.getElementById("sendButton").disabled = true;

////这是在监听服务器发送过来的消息,里面包括了用正则表达式去替换字符串中的内容。为了防止恶意代码。
//connection.on("ReceiveMessages", function (user, message) {
//    console.log(message);
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

////这是连接建立后，按钮就可用了。
//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});


////这是发送事件，给按钮添加事件监控，当按下按钮就会触发这个事件         
//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});



//-----------这是自己新写的聊天室代码。


//这是在引用代理。（集线器Hub）
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//这是连接建立
connection.start().catch(function (err) {
    return console.error(err.toString());
});


//这是发送消息
$("#sendButton").click(function () {

    var message = $("#content").val();

    $("#content").val("");
    console.log(message);
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
      

    });
    fasongxiaoxi(message);
});

connection.on("ReceiveMessages", function (message, myTime) {

    jieshouxiaoxi(message, myTime);
});






function fasongxiaoxi(message) {


    var html = '<div class="direct-chat-msg right">';
    html += '<div class="direct-chat-info clearfix">';
    html += '<span class="direct-chat-name pull-right">' + '你自己 ' + '</span>';
    html += '<span class="direct-chat-timestamp pull-left">' + '23 Jan 2: 00 pm' + '</span>';
    html += ' </div>';
    html += "<img class='direct-chat-img' src='/img/user.jpg' >";
    html += '  <div class="direct-chat-text">' + message + '  </div>  ';
    html += '</div>';

    $(".direct-chat-messages").append(html);


}

function jieshouxiaoxi(message, myTime) {


    var html = '<div class="direct-chat-msg ">';
    html += '<div class="direct-chat-info clearfix">';
    html += '<span class="direct-chat-name pull-left">' + '其他人 ' + '</span>';
    html += '<span class="direct-chat-timestamp pull-right">' + myTime + '</span>';
    html += ' </div>';
    html += "<img class='direct-chat-img' src='/img/user.jpg' >";
    html += '  <div class="direct-chat-text">' + message + '  </div>  ';
    html += '</div>';


    $(".direct-chat-messages").append(html);


}



//获取本地时间
//function frontOneHour(fmt) {
//    var currentTime = new Date(new Date().getTime())
//    console.log(currentTime) // Wed Jun 20 2018 16:12:12 GMT+0800 (中国标准时间)
//    var o = {
//        'M+': currentTime.getMonth() + 1, // 月份
//        'd+': currentTime.getDate(), // 日
//        'h+': currentTime.getHours(), // 小时
//        'm+': currentTime.getMinutes(), // 分
//        's+': currentTime.getSeconds(), // 秒
//        'q+': Math.floor((currentTime.getMonth() + 3) / 3), // 季度
//        'S': currentTime.getMilliseconds() // 毫秒
//    }
//    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (currentTime.getFullYear() + '').substr(4 - RegExp.$1.length))
//    for (var k in o) {
//        if (new RegExp('(' + k + ')').test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)))
//    }
//    return fmt;
//}
 // 调用
 // frontOneHour('yyyy-MM-dd hh:mm:ss') // "2018-06-20 16:11:59"

