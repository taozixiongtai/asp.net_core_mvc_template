using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace mymvc_core.Hubs
{
    public class ChatHub : Hub
    {
        //public async Task SendMessage(string user, string message)    //------这是微软的实例的代码。
        //{
        //    //注意里面的all，这表示所有的客户端都会受到。当然这个也是可以进行修改的。
        //    await Clients.All.SendAsync("ReceiveMessages", user, message + "当前的时间是：" + DateTime.Now);
        //}





        public async Task SendMessage(string message)
        {
            //注意里面的all，这表示所有的客户端都会受到。当然这个也是可以进行修改的。
            await Clients.Others.SendAsync("ReceiveMessages", message, DateTime.Now.ToString());


        }



        /// <summary>
        /// 这是拼图游戏的服务器端
        /// </summary>
        /// <param name="direction">方向。本来可以通过枚举来解决的</param>
        /// <returns></returns>
        public async Task Move(string direction)
        {

            await Clients.All.SendAsync("Move", direction);
        }

        public async Task zhuanfa(int[] shuzu)
        {

            await Clients.Client(UserList.ConnectedIds[1]).SendAsync("kaishitongbu", shuzu); //把消息转发给新连接的，让他开始同步
        }



        /// <summary>
        /// 当有用户连接到这个集线器的时候
        /// </summary>
        /// <returns></returns>

        public override async Task OnConnectedAsync()
        {

            UserList.ConnectedIds.Add(Context.ConnectionId);

            if (UserList.ConnectedIds.Count > 1)   //这是当里面有用户的时候,就要同步他当前的状态。
            {

                await Clients.Client(UserList.ConnectedIds[0]).SendAsync("tongbu"); //给客户端发消息，让他开始同步,每刷新一次就会产生一个新ID

            }


            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 当有用户断开连接到这个集线器的时候
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserList.ConnectedIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

    }

    public static class UserList
    {
        public static List<string> ConnectedIds = new List<string>();

    }
}
