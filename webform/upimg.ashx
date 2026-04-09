<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;

public class UploadHandler : IHttpHandler 
{
    private const int MAX_FILE_SIZE = 200 * 1024; // 200KB
    private const int JPEG_QUALITY_START = 85; // 起始压缩质量
    
    public void ProcessRequest(HttpContext context) 
    {
        context.Response.ContentType = "text/plain";
        context.Response.Charset = "UTF-8";
        
        try
        {
            // 获取操作类型参数
            string action = context.Request.QueryString["action"] ?? "image";
            
            switch (action.ToLower())
            {
                case "image":
                    ProcessImageUpload(context);
                    break;
                case "json":
                    ProcessJsonSave(context);
                    break;
                case "read":
                    ProcessJsonRead(context);
                    break;
                case "answer":
                    ProcessExamAnswer(context);
                    break;
                default:
                    context.Response.Write("ERROR:不支持的操作类型");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("ERROR:处理失败：" + ex.Message);
        }
    }


    private void ProcessExamAnswer(HttpContext context)
    {
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        if (cook.IsExist())
        {
            // 获取POST数据
            string Lid = context.Request.Form["Lid"];
            string Eid = context.Request.Form["Eid"];
            string Cid = context.Request.Form["Cid"];
            string Ascore = context.Request.Form["Ascore"];
            string Aspent = context.Request.Form["Aspend"];
            string Adata = context.Request.Form["Adata"];
            
            LearnSite.Model.Answers emodel = new LearnSite.Model.Answers();
            LearnSite.BLL.Answers ebll = new LearnSite.BLL.Answers();

            emodel = ebll.GetModelme(Int32.Parse(Eid), cook.Sid);
            int Aid = 0;
            
            if (emodel != null)
            {
                Aid = emodel.Aid;
                emodel.Atime = DateTime.Now;
                emodel.Ascore = Int32.Parse(Ascore);
                emodel.Aspent = Int32.Parse(Aspent);
                emodel.Adata = Adata;

                ebll.Update(emodel);
            }
            else
            {
                LearnSite.Model.Answers newmodel = new LearnSite.Model.Answers();//要重新定义一个模型，因为null无法赋值
                newmodel.Eid = Int32.Parse(Eid);
                newmodel.Asid = cook.Sid;
                newmodel.Asnum = cook.Snum;
                newmodel.Asname = cook.Sname;
                newmodel.Asgrade = cook.Sgrade;
                newmodel.Asclass = cook.Sclass;
                newmodel.Atime = DateTime.Now;
                newmodel.Ascore = Int32.Parse(Ascore);
                newmodel.Aspent = Int32.Parse(Aspent);
                newmodel.Adata = Adata;

                Aid = ebll.Add(newmodel);//添加考试记录

                //添加课堂活动记录
                string Wtime = cook.LoginTime;
                DateTime Wdate = DateTime.Now;
                LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
                kmodel.Klid = Int32.Parse(Lid);
                kmodel.Ksid = cook.Sid;
                kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                kmodel.Kcheck = false;
                LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                kbll.Add(kmodel);
            }

            // 返回文件名
            context.Response.Write(Aid.ToString());
        }
        else
        {
            context.Response.Write("ERROR:保存权限不足");
        }

    }
    
    private void ProcessJsonRead(HttpContext context)
    {
        try
        {
            string eid = context.Request.Form["eid"];
            if (eid!="0")
            {
                LearnSite.Model.Exams emodel = new LearnSite.Model.Exams();
                LearnSite.BLL.Exams ebll = new LearnSite.BLL.Exams();
                emodel = ebll.GetModel(Int32.Parse(eid));
                context.Response.Write(emodel.Edata);
            }
            else {
                context.Response.Write("ERROR:未知eid");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("ERROR:未知出错"+ex.Message);
        }
    }
    private void ProcessJsonSave(HttpContext context)
    {
        try
        {
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            if (tcook.IsExist())
            {
                // 获取POST数据
                // C# 示例
                string eid = context.Request.Form["eid"];
                string cid = context.Request.Form["cid"];
                string title = context.Request.Form["title"];
                string description = context.Request.Form["description"];
                string totalScore = context.Request.Form["total_score"];
                string questionCount = context.Request.Form["question_count"];
                string examJson = context.Request.Form["exam_json"];

                if (string.IsNullOrEmpty(examJson))
                {
                    context.Response.Write("ERROR:JSON数据为空");
                    return;
                }

                LearnSite.Model.Exams emodel = new LearnSite.Model.Exams();
                LearnSite.BLL.Exams ebll = new LearnSite.BLL.Exams();
                emodel.Cid = Int32.Parse(cid);
                emodel.Eclose = true;
                emodel.Ecount = Int32.Parse(questionCount);
                emodel.Edata = examJson;
                emodel.Edescription = description;
                emodel.Escore = Int32.Parse(totalScore);
                emodel.Etime = DateTime.Now;
                emodel.Etitle = title;
                emodel.Hid = tcook.Hid;

                if (eid=="0")
                {
                    int neweid = ebll.Add(emodel);
                    eid = neweid.ToString();
                    LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                    int maxSort = lbll.GetMaxLsort(Int32.Parse(cid)) + 1;
                    LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                    lmodel.Lcid = emodel.Cid;
                    lmodel.Lshow = false;
                    lmodel.Lsort = maxSort;
                    lmodel.Ltitle = emodel.Etitle;
                    lmodel.Ltype = 39;//页面类型为39 考试
                    lmodel.Lxid = neweid;
                    
                    lbll.Add(lmodel);
                }
                else {
                    emodel.Eid = Int32.Parse(eid);
                    ebll.Update(emodel);
                    LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                    LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                    lmodel.Lcid = Int32.Parse(cid);
                    lmodel.Ltitle = title;
                    lmodel.Ltype = 39;//页面类型为39 考试
                    lmodel.Lxid = Int32.Parse(eid);
                    
                    lbll.UpdateLtitle(lmodel);
                }
                
                // 创建上传目录
                string uploadDir = context.Server.MapPath("~/webform/uploads/");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // 生成唯一文件名
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string randomNum = eid.ToString();
                string fileName = "exam_" + randomNum + "_" + timestamp + ".json";
                string filePath = Path.Combine(uploadDir, fileName);

                // 保存JSON文件
                File.WriteAllText(filePath, examJson, Encoding.UTF8);

                // 返回文件名
                context.Response.Write(eid);
            }
            else {
                context.Response.Write("ERROR:保存权限不足");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("ERROR:保存未知出错"+ex.Message);
        }
    }
   
    
    private void ProcessImageUpload(HttpContext context)
    {
        // 检查是否有文件上传
        if (context.Request.Files.Count == 0)
        {
            context.Response.Write("ERROR:没有选择文件");
            return;
        }
        
        HttpPostedFile uploadFile = context.Request.Files[0];
        
        // 验证文件
        if (uploadFile == null || uploadFile.ContentLength == 0)
        {
            context.Response.Write("ERROR:文件为空");
            return;
        }
        
        // 验证文件大小 (限制10MB原始文件)
        if (uploadFile.ContentLength > 10 * 1024 * 1024)
        {
            context.Response.Write("ERROR:文件大小不能超过10MB");
            return;
        }
        
        // 验证文件类型
        string fileExtension = Path.GetExtension(uploadFile.FileName).ToLower();
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        bool isValidExtension = false;
        
        foreach (string ext in allowedExtensions)
        {
            if (fileExtension == ext)
            {
                isValidExtension = true;
                break;
            }
        }
        
        if (!isValidExtension)
        {
            context.Response.Write("ERROR:只支持 JPG、PNG、GIF、BMP 格式的图片");
            return;
        }
        
        // 创建上传目录
        string uploadDir = context.Server.MapPath("~/webform/uploads/");
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }
        
        // 生成唯一文件名
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string randomNum = new Random().Next(1000, 9999).ToString();
        
        // 检查文件大小，小于200KB直接保存原格式
        if (uploadFile.ContentLength <= MAX_FILE_SIZE)
        {
            string originalFileName = timestamp + "_" + randomNum + fileExtension;
            string originalFilePath = Path.Combine(uploadDir, originalFileName);
            
            // 直接保存原文件
            uploadFile.SaveAs(originalFilePath);
            
            // 返回图片相对路径 - 只返回文件名
            string originalRelativePath = originalFileName;
            context.Response.Write(originalRelativePath);
            return;
        }
        
        // 大于200KB需要压缩，统一保存为JPG
        string newFileName = timestamp + "_" + randomNum + ".jpg";
        string filePath = Path.Combine(uploadDir, newFileName);
        
        // 处理图片并保存
        if (!ProcessAndSaveImage(uploadFile.InputStream, filePath))
        {
            context.Response.Write("ERROR:图片处理失败");
            return;
        }
        
        // 返回图片相对路径 - 只返回文件名
        string relativePath = newFileName;
        context.Response.Write(relativePath);
    }
    
    private bool ProcessAndSaveImage(Stream inputStream, string outputPath)
    {
        try
        {
            using (Image originalImage = Image.FromStream(inputStream))
            {
                // 检查是否为PNG格式，需要处理透明背景
                bool isPng = originalImage.RawFormat.Equals(ImageFormat.Png);
                
                // 创建新的位图
                using (Bitmap bitmap = new Bitmap(originalImage.Width, originalImage.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // 如果是PNG，设置白色背景
                        if (isPng)
                        {
                            graphics.Clear(Color.White);
                        }
                        
                        // 设置高质量绘制 (ASP.NET 2.0兼容)
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        
                        // 绘制图片
                        graphics.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                    }
                    
                    // 压缩并保存
                    return SaveWithSizeControl(bitmap, outputPath);
                }
            }
        }
        catch
        {
            return false;
        }
    }
    
    private bool SaveWithSizeControl(Bitmap image, string outputPath)
    {
        int quality = JPEG_QUALITY_START;
        
        // 尝试不同的压缩质量，直到文件大小满足要求
        while (quality >= 20) // 最低质量20%
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 设置JPEG编码参数
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                if (jpegCodec != null)
                {
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    
                    // 保存到内存流
                    image.Save(ms, jpegCodec, encoderParams);
                }
                else
                {
                    // 如果找不到JPEG编码器，使用默认方式
                    image.Save(ms, ImageFormat.Jpeg);
                }
                
                // 如果文件大小满足要求，保存到文件
                if (ms.Length <= MAX_FILE_SIZE)
                {
                    File.WriteAllBytes(outputPath, ms.ToArray());
                    return true;
                }
                
                // 降低质量重试
                quality -= 10;
            }
        }
        
        // 如果仍然太大，使用最低质量保存
        using (MemoryStream ms = new MemoryStream())
        {
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            if (jpegCodec != null)
            {
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 20);
                image.Save(ms, jpegCodec, encoderParams);
            }
            else
            {
                image.Save(ms, ImageFormat.Jpeg);
            }
            
            File.WriteAllBytes(outputPath, ms.ToArray());
            return true;
        }
    }
    
    private ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.MimeType == mimeType)
            {
                return codec;
            }
        }
        return null;
    }
    
    public bool IsReusable 
    {
        get { return false; }
    }
}