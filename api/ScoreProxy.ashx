<%@ WebHandler Language="C#" Class="ScoreProxy" %>using System;
using System.Web;
using System.Data.SqlClient;
using LearnSite.BLL;
using LearnSite.Model;

public class ScoreProxy : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        string action = context.Request.QueryString["action"];
        
        try
        {
            switch (action)
            {
                case "deduct":
                    DeductScore(context);
                    break;
                case "getinfo":
                    GetResourceInfo(context);
                    break;
                default:
                    context.Response.Write("{\"code\": 0, \"msg\": \"未知操作\"}");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"服务器错误\"}");
        }
    }
    
    private void DeductScore(HttpContext context)
    {
        string fid = context.Request.Form["fid"];
        
        if (string.IsNullOrEmpty(fid))
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"参数错误\"}");
            return;
        }
        
        // 检查学生是否登录
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"请先登录\"}");
            return;
        }
        
        // 获取学生信息
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        
        // 获取资源信息
        LearnSite.BLL.Soft softBll = new LearnSite.BLL.Soft();
        int fileId = int.Parse(fid);
        LearnSite.Model.Soft softModel = softBll.GetModel(fileId);
        
        if (softModel == null)
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"资源不存在\"}");
            return;
        }
        
        // 获取所需学分
        int requiredScore = softModel.Fopen ?? 0;
        if (requiredScore >= 10000)
        {
            requiredScore = requiredScore - 10000;
        }
        
        // 检查学生综合得分是否足够
        LearnSite.BLL.StudentScoreService scoreService = new LearnSite.BLL.StudentScoreService();
        int comprehensiveScore = scoreService.GetComprehensiveScore(cook.Snum);
        
        if (comprehensiveScore < requiredScore)
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"综合得分不足\"}");
            return;
        }
        
        // 检查学生是否已经扣除过该资源的学分
        if (HasDeductedScore(cook.Snum, fileId))
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"已经扣除过该资源的学分，无法重复扣除\"}");
            return;
        }
        
        // 记录学分扣除（不实际修改数据库中的 Sscore，因为综合得分是计算出来的）
        RecordScoreDeduction(cook.Snum, fileId, requiredScore, softModel.Ftitle);
        context.Response.Write("{\"code\": 1, \"msg\": \"学分扣除成功\"}");
    }

    /// <summary>
    /// 检查学生是否已经扣除过该资源的学分
    /// </summary>
    /// <param name="snum">学生学号</param>
    /// <param name="fid">资源ID</param>
    /// <returns>是否已经扣除</returns>
    private bool HasDeductedScore(string snum, int fid)
    {
        string sql = "SELECT COUNT(*) FROM ScoreDeduction WHERE Snum = @Snum AND Fid = @Fid";
        SqlParameter[] parameters = {
            new SqlParameter("@Snum", snum),
            new SqlParameter("@Fid", fid)
        };
        object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql, parameters);
        return result != null && Convert.ToInt32(result) > 0;
    }
    
    /// <summary>
    /// 扣除学生学分
    /// </summary>
    /// <param name="snum">学生学号</param>
    /// <param name="score">扣分数值</param>
    /// <param name="reason">扣除原因</param>
    /// <returns>是否扣除成功</returns>
    private bool DeductStudentScore(string snum, int score, string reason)
    {
        try
        {
            // 获取学生ID
            LearnSite.BLL.Students studentBll = new LearnSite.BLL.Students();
            LearnSite.Model.Students studentModel = studentBll.SnumGetModel(snum);
            if (studentModel == null)
            {
                return false;
            }
            
            // 扣除学生作品分
            string sql = "UPDATE Students SET Sscore = ISNULL(Sscore, 0) - @Score WHERE Sid = @Sid";
            SqlParameter[] parameters = {
                new SqlParameter("@Score", score),
                new SqlParameter("@Sid", studentModel.Sid)
            };
            int result = LearnSite.DBUtility.DbHelperSQL.ExecuteSql(sql, parameters);
            return result > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    /// <summary>
    /// 记录学分扣除
    /// </summary>
    /// <param name="snum">学生学号</param>
    /// <param name="fid">资源ID</param>
    /// <param name="score">扣分数值</param>
    /// <param name="resourceName">资源名称</param>
    private void RecordScoreDeduction(string snum, int fid, int score, string resourceName)
    {
        try
        {
            // 检查 ScoreDeduction 表是否存在，如果不存在则创建
            CreateScoreDeductionTable();
            
            // 插入扣除记录
            string sql = "INSERT INTO ScoreDeduction (Snum, Fid, Score, ResourceName, DeductDate, Reason) VALUES (@Snum, @Fid, @Score, @ResourceName, GETDATE(), @Reason)";
            SqlParameter[] parameters = {
                new SqlParameter("@Snum", snum),
                new SqlParameter("@Fid", fid),
                new SqlParameter("@Score", score),
                new SqlParameter("@ResourceName", resourceName),
                new SqlParameter("@Reason", "访问资源")
            };
            LearnSite.DBUtility.DbHelperSQL.ExecuteSql(sql, parameters);
        }
        catch (Exception)
        {
            // 记录失败不影响主流程
        }
    }
    
    /// <summary>
    /// 创建学分扣除记录表
    /// </summary>
    private void CreateScoreDeductionTable()
    {
        try
        {
            string checkTableSql = "SELECT COUNT(*) FROM sysobjects WHERE name = 'ScoreDeduction' AND type = 'U'";
            object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(checkTableSql);
            if (result == null || Convert.ToInt32(result) == 0)
            {
                string createTableSql = @"
                    CREATE TABLE ScoreDeduction (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Snum NVARCHAR(50) NOT NULL,
                        Fid INT NOT NULL,
                        Score INT NOT NULL,
                        ResourceName NVARCHAR(255) NOT NULL,
                        DeductDate DATETIME NOT NULL,
                        Reason NVARCHAR(255) NOT NULL
                    )
                ";
                LearnSite.DBUtility.DbHelperSQL.ExecuteSql(createTableSql);
            }
        }
        catch (Exception)
        {
            // 创建表失败不影响主流程
        }
    }

    /// <summary>
    /// 获取资源的学分信息
    /// </summary>
    /// <param name="context">HttpContext</param>
    private void GetResourceInfo(HttpContext context)
    {
        string fid = context.Request.Form["fid"];
        
        if (string.IsNullOrEmpty(fid))
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"参数错误\"}");
            return;
        }
        
        // 检查学生是否登录
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"请先登录\"}");
            return;
        }
        
        // 获取学生信息
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        
        // 获取资源信息
        LearnSite.BLL.Soft softBll = new LearnSite.BLL.Soft();
        int fileId = int.Parse(fid);
        LearnSite.Model.Soft softModel = softBll.GetModel(fileId);
        
        if (softModel == null)
        {
            context.Response.Write("{\"code\": 0, \"msg\": \"资源不存在\"}");
            return;
        }
        
        // 获取所需学分
        int requiredScore = softModel.Fopen ?? 0;
        if (requiredScore >= 10000)
        {
            requiredScore = requiredScore - 10000;
        }
        
        // 获取学生当前综合得分
        LearnSite.BLL.StudentScoreService scoreService = new LearnSite.BLL.StudentScoreService();
        int currentScore = scoreService.GetComprehensiveScore(cook.Snum);
        
        // 检查是否已经扣除过该资源的学分
        bool hasDeducted = HasDeductedScore(cook.Snum, fileId);
        
        context.Response.Write("{\"code\": 1, \"score\": " + requiredScore + ", \"currentScore\": " + currentScore + ", \"hasDeducted\": " + (hasDeducted ? "true" : "false") + "}");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}