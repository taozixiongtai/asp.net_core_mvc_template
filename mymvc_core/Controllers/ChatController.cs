using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mymvc_core.Controllers
{
    public class ChatController : Controller
    {
        /// <summary>
        /// 聊天室。
        /// </summary>
        /// <returns></returns>
        public IActionResult ChatRoom()
        {
            
            return View();
        }

        /// <summary>
        /// 拼图游戏
        /// </summary>
        /// <returns></returns>
        public IActionResult Jigsaw()
        {

            List<int> c = new List<int> { 1,2,3,4,5,6,7,8};
            c.ListRandom();
           
            if (!c.ListCheck())
            {
                c.ListChange();
            }
            ViewBag.c = c;

            return View();
        }
    }
}