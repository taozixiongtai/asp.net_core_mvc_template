using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 生成路径的扩展
    /// </summary>
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: "ConfirmEmailResult",
                controller: "Identity",
                values: new { userId, code },               //这是通过GET方法进行传值。
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string UserId, string code, string scheme)
        {
            return urlHelper.Action(
                action: "SetPassword",
                controller: "Identity",
                values: new { UserId, code },
                protocol: scheme);
        }

    }

    /// <summary>
    /// 这是把集合里面的数字随机的静态扩展类。
    /// </summary>

    public static class ListHelperExtensions
    {


        /// <summary>
        /// 打乱里面的排序。
        /// </summary>
        /// <param name="myList"></param>
        public static void ListRandom(this List<int> myList)
        {

            Random ran = new Random();
            int index = 0;
            int temp = 0;
            for (int i = 0; i < myList.Count; i++)
            {

                index = ran.Next(0, myList.Count - 1);
                if (index != i)
                {
                    temp = myList[i];
                    myList[i] = myList[index];
                    myList[index] = temp;
                }
            }
        }

        /// <summary>
        /// 检查这个集合的逆序数。如果能用返回true
        /// </summary>
        /// <param name="myList"></param>
        public static bool ListCheck(this List<int> myList)
        {

            int nixushu = 0;

            for (int i = 0; i < myList.Count - 1; i++)
            {
                if (myList[i] > myList[(i + 1)])
                {
                    nixushu++;
                }
            }

            if (nixushu % 2 == 0)
                return true;
            return false;

        }


        /// <summary>
        /// 改变逆序数，将他改成偶
        /// </summary>
        /// <param name="myList"></param>
        public static void ListChange(this List<int> myList)
        {


            int temp;
            for (int i = 0; i < myList.Count - 1; i++)
            {
                if (myList[i] > myList[(i + 1)])
                {
                    ;temp = myList[i];
                    myList[i] = myList[i + 1];
                    myList[i + 1] = temp;
                    return;
                }
            }

        }


    }


}
