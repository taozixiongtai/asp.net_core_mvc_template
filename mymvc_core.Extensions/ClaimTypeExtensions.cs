using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace mymvc_core.Extensions
{
    /// <summary>
    /// 在razor里面生成的扩展
    /// </summary>
    public static class ClaimTypeExtensions
    {
        public static String ClaimType(this IHtmlHelper html, string claimType)
        {
            FieldInfo[] fields = typeof(ClaimTypes).GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(null).ToString() == claimType)
                {
                    return field.Name.ToString();
                }
            }
            return  string.Format("{0}",
            claimType.Split('/', '.').Last());
        }
    }
}
