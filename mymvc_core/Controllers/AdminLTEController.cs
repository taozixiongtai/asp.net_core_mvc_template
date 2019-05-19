using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mymvc_core.Models;
using mymvc_core.Services;
using Microsoft.EntityFrameworkCore;
using mymvc_core.Services.Exceptions;
using mymvc_core.Services.interfaces;
using Microsoft.AspNetCore.Authorization;

namespace mymvc_core.Controllers
{  
    public class AdminLTEController : Controller
    {


        private readonly IMessageServer _messageServer;

        public AdminLTEController(IMessageServer messageServer)
        {

            _messageServer = messageServer;
        }


        public async Task<IActionResult> Messages()
        {

            return View(await _messageServer.GetAllMessageAsync());

        }

        public IActionResult Add(int? id)
        {
            try
            {
                if (id != null)
                {
                    TempData["edit"] = 1;
                    return View(_messageServer.GetMessagesById(id.GetValueOrDefault()));
                }
            }
            catch (NotFoundException e) //捕获异常
            {
                TempData["ErrorMessage"] = e.Message;
                return StatusCode(404);         //返回到错误页面。

            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Messages messages)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _messageServer.Add(messages);
                    return RedirectToAction(nameof(Messages));

                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("模型有错误", e.Message);
                    return View(messages);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return StatusCode(500);
                }
            }
            else
            {
                return View(messages);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Messages messages)
        {
            if (messages?.Id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _messageServer.UpdateOne(messages);
                    return RedirectToAction(nameof(Messages));

                }
                catch (ModelValidationException e)
                {

                    ModelState.AddModelError("模型有错误", e.Message);
                    return View(messages);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return View(messages);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _messageServer.DelectOne(id);
                return RedirectToAction(nameof(Messages));
            }
            catch (DbUpdateException e)                 //更新失败，没有找到的异常在服务类中去实现去了。
            {
                TempData["ErrorMessage"] = e.Message;
                return StatusCode(400);
            }
        }
    }
}