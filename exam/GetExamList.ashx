<%@ WebHandler Language="C#" Class="GetExamList" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data;
using LearnSite.BLL;

public class GetExamList : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        try
        {
            // 获取课堂测验列表
            LearnSite.BLL.Exams examsBll = new LearnSite.BLL.Exams();
            DataSet ds = examsBll.GetList("");
            
            // 检查数据是否为空
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                var emptyResponse = new
                {
                    success = true,
                    exams = new List<object>(),
                    message = "暂无课堂测验"
                };
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                context.Response.Write(serializer.Serialize(emptyResponse));
                return;
            }
            
            // 转换为List
            List<object> surveys = new List<object>();
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    surveys.Add(new
                    {
                        Vid = row["Eid"] != DBNull.Value ? Convert.ToInt32(row["Eid"]) : 0,
                        Vtitle = row["Etitle"] != DBNull.Value ? row["Etitle"].ToString() : "",
                        Vcontent = row["Edescription"] != DBNull.Value ? row["Edescription"].ToString() : "",
                        Vtype = 0,
                        Vtotal = row["Ecount"] != DBNull.Value ? Convert.ToInt32(row["Ecount"]) : 0,
                        Vscore = row["Escore"] != DBNull.Value ? Convert.ToInt32(row["Escore"]) : 0,
                        Vaverage = 0,
                        Vclose = row["Eclose"] != DBNull.Value && Convert.ToBoolean(row["Eclose"]),
                        Vpoint = false,
                        Vdate = row["Etime"] != DBNull.Value ? Convert.ToDateTime(row["Etime"]).ToString("yyyy-MM-dd HH:mm:ss") : ""
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("处理行数据错误: " + ex.Message);
                }
            }
            
            // 构建JSON响应
            var response = new
            {
                success = true,
                exams = surveys
            };
            
            JavaScriptSerializer serializer2 = new JavaScriptSerializer();
            string jsonResult = serializer2.Serialize(response);
            context.Response.Write(jsonResult);
        }
        catch (Exception ex)
        {
            // 记录详细错误信息
            System.Diagnostics.Trace.WriteLine("GetExamList异常: " + ex.Message);
            System.Diagnostics.Trace.WriteLine("异常类型: " + ex.GetType().Name);
            System.Diagnostics.Trace.WriteLine("堆栈跟踪: " + ex.StackTrace);
            
            var errorResponse = new
            {
                success = false,
                message = ex.Message,
                error = ex.GetType().Name
            };
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(errorResponse));
        }
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
