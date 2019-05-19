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

        /// <summary>
        /// 消息视图
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Messages()
        {

            return View(await _messageServer.GetAllMessageAsync());

        }

        /// <summary>
        /// 添加消息的视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            catch (NotFoundException e)  
            {
                TempData["ErrorMessage"] = e.Message;
                return StatusCode(404);          

            }
            return View();

        }

        /// <summary>
        /// 添加消息的表单提交action
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 编辑消息
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _messageServer.DelectOne(id);
                return RedirectToAction(nameof(Messages));
            }
            catch (DbUpdateException e)                  
            {
                TempData["ErrorMessage"] = e.Message;
                return StatusCode(400);
            }
        }
    }
}