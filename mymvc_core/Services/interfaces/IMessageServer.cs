using mymvc_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Services.interfaces
{
    public interface IMessageServer
    {
        Task<IEnumerable<Messages>> GetAllMessageAsync();
        Task Add(Messages message);          //添加
        Task DelectOne(int id);             //删除
        Messages GetMessagesById(int id);   //查询
        Task UpdateOne(Messages messages); //更新
    }
}
