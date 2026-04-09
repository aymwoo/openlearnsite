<%@ WebHandler Language="C#" Class="GetGradeList" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using LearnSite.BLL;
using LearnSite.Common;

public class GetGradeList : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            // 检查教师登录状态 - 暂时注释，避免重定向
            // CookieHelp.JudgeTeacherCookies();
            
            // 获取教师ID - 暂时跳过检查
            // HttpCookie teacherCookie = context.Request.Cookies[CookieHelp.teaCookieNname];
            // if (teacherCookie == null)
            // {
            //     context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
            //     return;
            // }
            // 
            // string teacherId = teacherCookie.Values["Hid"];
            // if (string.IsNullOrEmpty(teacherId))
            // {
            //     context.Response.Write("{\"success\":false,\"message\":\"未登录\"}");
            //     return;
            // }
            
            // 获取年级列表 - 暂时返回模拟数据
            // Grade gradeBll = new Grade();
            // var grades = gradeBll.GetList(teacherId);
            
            // 模拟数据 - 返回一些年级
            List<object> grades = new List<object>();
            grades.Add(new { GradeId = 1, GradeName = "一年级" });
            grades.Add(new { GradeId = 2, GradeName = "二年级" });
            grades.Add(new { GradeId = 3, GradeName = "三年级" });
            grades.Add(new { GradeId = 4, GradeName = "四年级" });
            grades.Add(new { GradeId = 5, GradeName = "五年级" });
            grades.Add(new { GradeId = 6, GradeName = "六年级" });
            grades.Add(new { GradeId = 7, GradeName = "七年级" });
            grades.Add(new { GradeId = 8, GradeName = "八年级" });
            grades.Add(new { GradeId = 9, GradeName = "九年级" });
            
            // 构建JSON响应
            var response = new
            {
                success = true,
                grades = grades
            };
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonResult = serializer.Serialize(response);
            context.Response.Write(jsonResult);
        }
        catch (Exception ex)
        {
            // 记录详细错误信息
            System.Diagnostics.Trace.WriteLine("GetGradeList错误: " + ex.Message);
            System.Diagnostics.Trace.WriteLine("堆栈跟踪: " + ex.StackTrace);
            
            context.Response.Write("{\"success\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\",\"error\":\"" + ex.GetType().Name + "\"}");
        }
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
