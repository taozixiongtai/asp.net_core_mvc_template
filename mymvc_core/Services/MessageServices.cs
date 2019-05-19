using mymvc_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mymvc_core.Services.Exceptions;
using mymvc_core.Services.interfaces;

namespace mymvc_core.Services
{
    public class MessageServer : IMessageServer
    {
        /// <summary>
        /// 这是初始化的时候使用。
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Messages>> GetAllMessageAsync()
        {
            using (CoremvcContext context = new CoremvcContext())
            {
                return await context.Messages.ToListAsync();
            }
        }

        /// <summary>
        /// 通过EF添加一条数据进去
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Add(Messages message)
        {
            using (CoremvcContext context = new CoremvcContext())
            {
                context.Messages.Add(message);
                await context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id">被删除的记录的ID</param>
        /// <returns>一条留言</returns>
        public async Task DelectOne(int id)
        {
            using (CoremvcContext context = new CoremvcContext())
            {
                var TheOne = GetMessagesById(id);
                context.Remove(TheOne);
                await context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 通过ID去查询
        /// </summary>
        /// <param name="id">被查找的留言的ID</param>
        /// <returns>一条留言</returns>
        public Messages GetMessagesById(int id)
        {
            using (CoremvcContext context = new CoremvcContext())
            {
                var result = context.Messages.SingleOrDefault(m => m.Id == id);

                if (result == null)
                {
                    throw new NotFoundException($"ID为{id}的留言已经找不到了");  //在这里就判断了数据库中有没有，所以其他地方不用再判定了
                }
                return result;
            }
        }

        public async Task UpdateOne(Messages  messages)
        {
            using (CoremvcContext context = new CoremvcContext())
            {
                context.Entry(messages).State = EntityState.Modified; //第一种更新的方法
                //context.Update(messages);           //第二种更新的方法
                await context.SaveChangesAsync();                
            }
        }
    }
}
