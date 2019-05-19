
"use strict";

//这是在引用代理。（集线器Hub）
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//这是连接建立
connection.start().catch(function (err) {
    return console.error(err.toString());
});


connection.on("Move", function (direction) {
    if (direction == "Down") {
        Down();
  Done();
    }
    if (direction == "Right") {
        Right();
  Done();
    }
    if (direction == "Up") {
        Up();
  Done();
    }
    if (direction == "Left") {
        Left();
  Done();
    }
});




connection.on("tongbu", function () {

    
        var shuzu = new Array(9);
         for (var i = 0; i < 9; i++) {

             if ($("ul[id='box'] >li:eq(" + i + ")").text() == "") {
                 shuzu[i] = 0;
                 continue;
             }
                shuzu[i]=$("ul[id='box'] >li:eq(" + i + ")").text()
             
    }
    
  
        connection.invoke("zhuanfa", shuzu);

});

connection.on("kaishitongbu", function (shuzu) {

 
    for (var i = 0; i < 9; i++) {

        if (shuzu[i]==0) {
            $("ul[id='box'] >li:eq(" + i + ")").text("");
            continue;
        }
      
        $("ul[id='box'] >li:eq(" + i + ")").text(shuzu[i]);
    }

});


$(document).ready(function () {
    $(document).keydown(function (event) {
        if (event.keyCode == 40) {   //下箭头

            connection.invoke("Move", "Down").catch(function (err) {
                return console.error(err.toString());
            });
        }


        if (event.keyCode == 39) { //右箭头

            connection.invoke("Move", "Right").catch(function (err) {
                return console.error(err.toString());
            });
        }


        if (event.keyCode == 38) {  //上箭头

            connection.invoke("Move", "Up").catch(function (err) {
                return console.error(err.toString());
            });
        }


        if (event.keyCode == 37) {  //左箭头

            connection.invoke("Move", "Left").catch(function (err) {
                return console.error(err.toString());
            });
        }

      

    });
});


function CheckUp(x, y) {  //检查越界了没有

    if (x < 1 || x > 3 || y < 1 || y > 3) {

        return true;
    }

    return false;
}



function Down(x, y) {  //下


    var x = $(".empty").attr('x');
    var y = $(".empty").attr('y');

    var new_y = parseInt(y) - 1;


    var moved = "[x='" + x + "'][y='" + new_y + "']";           //将要被移动的那个格子的坐标
    var old = "[x='" + x + "'][y='" + y + "']";               //原先的空白格的坐标

    if (CheckUp(x, new_y)) {
        return;
    }

    $(old).text($(moved).text()).removeClass("empty");      //空白格获得值，然后移除空标记
    $(moved).addClass("empty").text("");                //被移动的获得标记，然后再删除其值。

}
function Right(x, y) {  //右
    var x = $(".empty").attr('x');
    var y = $(".empty").attr('y');

    var new_x = parseInt(x) - 1;


    var moved = "[x='" + new_x + "'][y='" + y + "']";           //将要被移动的那个格子的坐标
    var old = "[x='" + x + "'][y='" + y + "']";               //原先的空白格的坐标

    if (CheckUp(new_x, y)) {
        return;
    }

    $(old).text($(moved).text()).removeClass("empty");      //空白格获得值，然后移除空标记
    $(moved).addClass("empty").text("");                //被移动的获得标记，然后再删除其值。
}
function Up(x, y) {  //上

    var x = $(".empty").attr('x');
    var y = $(".empty").attr('y');


    var new_y = parseInt(y) + 1;


    var moved = "[x='" + x + "'][y='" + new_y + "']";           //将要被移动的那个格子的坐标
    var old = "[x='" + x + "'][y='" + y + "']";               //原先的空白格的坐标

    if (CheckUp(x, new_y)) {
        return;
    }

    $(old).text($(moved).text()).removeClass("empty");      //空白格获得值，然后移除空标记
    $(moved).addClass("empty").text("");                //被移动的获得标记，然后再删除其值。
}
function Left(x, y) {  //左


    var x = $(".empty").attr('x');
    var y = $(".empty").attr('y');

    var new_x = parseInt(x) + 1;


    var moved = "[x='" + new_x + "'][y='" + y + "']";           //将要被移动的那个格子的坐标
    var old = "[x='" + x + "'][y='" + y + "']";               //原先的空白格的坐标

    if (CheckUp(new_x, y)) {
        return;
    }

    $(old).text($(moved).text()).removeClass("empty");      //空白格获得值，然后移除空标记
    $(moved).addClass("empty").text("");                //被移动的获得标记，然后再删除其值。
}

function Done() {  //检查完成了没



    for (var i = 0; i < 8; i++) {
    
    console.log($("ul[id='box'] >li:eq(" + i + ")").text());            
         console.log(i+1)

        if ($("ul[id='box'] >li:eq(" + i + ")").text() != parseInt(i+1) ){
         
            console.log("你没做完");
            return false;
        }


    }
    console.log("你做完了");
    return true;

}


