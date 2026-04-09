<%@ WebHandler Language="C#" Class="GetGradeList" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using LearnSite.BLL;
using LearnSite.Common;

public class GetGradeList : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            // 检查教师登录状态
            CookieHelp.JudgeTeacherCookies();
            
            // 获取教师ID
            HttpCookie teacherCookie = context.Request.Cookies[CookieHelp.teaCookieNname];
            if (teacherCookie == null)
            {
                context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
                return;
            }
            
            string teacherId = teacherCookie.Values["Hid"];
            if (string.IsNullOrEmpty(teacherId))
            {
                context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
                return;
            }
            
            // 获取年级列表
            Classes classesBll = new Classes();
            var classList = classesBll.GetClassList(teacherId);
            
            // 从班级列表中提取年级列表，去重
            var grades = classList
                .Where(c => c.Grade.HasValue)
                .Select(c => c.Grade.Value)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
            
            // 构建JSON响应
            var response = new
            {
                success = true,
                grades = grades
            };
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(response));
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
        }
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
