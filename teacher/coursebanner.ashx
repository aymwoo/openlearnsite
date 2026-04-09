<%@ WebHandler Language="C#" Class="coursebanner" %>

using System;
using System.IO;
using System.Web;

public class coursebanner : IHttpHandler
{
    private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".webp" };

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;

        if (context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] == null)
        {
            WriteJson(context, false, "未登录或登录已失效");
            return;
        }

        string action = (context.Request["action"] ?? string.Empty).Trim().ToLowerInvariant();

        try
        {
            switch (action)
            {
                case "upload":
                    UploadBanner(context);
                    break;
                default:
                    WriteJson(context, false, "不支持的操作");
                    break;
            }
        }
        catch (Exception ex)
        {
            WriteJson(context, false, ex.Message.Replace("\"", "'").Replace("\r", " ").Replace("\n", " "));
        }
    }

    private void UploadBanner(HttpContext context)
    {
        string cidValue = context.Request["cid"];
        int cid;
        if (!Int32.TryParse(cidValue, out cid) || cid <= 0)
        {
            WriteJson(context, false, "课程编号无效");
            return;
        }

        if (context.Request.Files.Count == 0)
        {
            WriteJson(context, false, "请选择要上传的横幅图片");
            return;
        }

        HttpPostedFile file = context.Request.Files[0];
        if (file == null || file.ContentLength <= 0)
        {
            WriteJson(context, false, "上传文件为空");
            return;
        }

        string extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension))
        {
            WriteJson(context, false, "无法识别图片类型");
            return;
        }

        extension = extension.ToLowerInvariant();
        bool allowed = false;
        for (int i = 0; i < AllowedExtensions.Length; i++)
        {
            if (AllowedExtensions[i] == extension)
            {
                allowed = true;
                break;
            }
        }

        if (!allowed)
        {
            WriteJson(context, false, "仅支持 png、jpg、jpeg、gif、webp 图片");
            return;
        }

        if (file.ContentLength > 5 * 1024 * 1024)
        {
            WriteJson(context, false, "图片大小不能超过 5MB");
            return;
        }

        LearnSite.BLL.Courses courseBll = new LearnSite.BLL.Courses();
        LearnSite.Model.Courses course = courseBll.GetModel(cid);
        if (course == null)
        {
            WriteJson(context, false, "课程不存在");
            return;
        }

        string saveDirectoryUrl = LearnSite.Store.CourseStore.GetSaveUrl("Course", cid.ToString());
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string fileName = "banner_" + timestamp + extension;
        string relativePath = saveDirectoryUrl + fileName;
        string physicalPath = context.Server.MapPath(relativePath);

        file.SaveAs(physicalPath);

        course.Cbanner = relativePath;
        course.Cdate = DateTime.Now;
        courseBll.UpdateCourse(course);

        string bannerUrl = VirtualPathUtility.ToAbsolute(relativePath);
        WriteJson(context, true, "横幅已更新", bannerUrl);
    }

    private void WriteJson(HttpContext context, bool success, string message)
    {
        context.Response.Write("{\"success\":" + (success ? "true" : "false") + ",\"message\":\"" + Escape(message) + "\"}");
    }

    private void WriteJson(HttpContext context, bool success, string message, string bannerUrl)
    {
        context.Response.Write("{\"success\":" + (success ? "true" : "false") + ",\"message\":\"" + Escape(message) + "\",\"bannerUrl\":\"" + Escape(bannerUrl) + "\"}");
    }

    private string Escape(string value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", " ").Replace("\n", " ");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
