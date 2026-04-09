using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_GameProxy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            ShowError("请先登录", "您尚未登录，请先登录后再访问游戏。");
            return;
        }
        
        string token = Request.QueryString["token"] ?? "";
        string fid = Request.QueryString["fid"] ?? "";
        
        if (string.IsNullOrEmpty(token))
        {
            ShowError("无效访问", "缺少必要的访问参数。");
            return;
        }
        
        string decryptedUrl = LearnSite.Common.LinkEncryption.DecryptUrl(token, Session.SessionID);
        
        if (string.IsNullOrEmpty(decryptedUrl))
        {
            ShowError("链接已过期", "此链接已过期或无效，请重新从正确页面访问。");
            return;
        }
        
        if (!string.IsNullOrEmpty(fid))
        {
            if (!CheckScoreRequirement(fid))
            {
                return;
            }
        }
        
        string absoluteUrl = ResolveGameUrl(decryptedUrl);
        LoadGame(absoluteUrl);
    }
    
    private string ResolveGameUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return url;
        
        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            return url;
        }
        
        if (url.StartsWith("~/"))
        {
            return ResolveUrl(url);
        }
        
        if (url.StartsWith("/"))
        {
            return url;
        }
        
        return "../" + url;
    }
    
    private bool CheckScoreRequirement(string fid)
    {
        try
        {
            int fidInt = int.Parse(fid);
            LearnSite.BLL.Soft softBll = new LearnSite.BLL.Soft();
            LearnSite.Model.Soft softModel = softBll.GetModel(fidInt);
            
            if (softModel == null)
            {
                ShowError("资源不存在", "请求的资源不存在。");
                return false;
            }
            
            int fopenValue = softModel.Fopen ?? 0;
            
            if (fopenValue == 0)
            {
                return true;
            }
            
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            
            if (fopenValue >= 10000)
            {
                int requiredScore = fopenValue - 10000;
                LearnSite.BLL.StudentScoreService scoreService = new LearnSite.BLL.StudentScoreService();
                int studentScore = scoreService.GetComprehensiveScore(cook.Snum);
                
                if (studentScore < requiredScore)
                {
                    ShowError("分数不足", "您的综合得分为 " + studentScore + " 分，需要达到 " + requiredScore + " 分才能访问此游戏。");
                    return false;
                }
            }
            else
            {
                LearnSite.BLL.Works workBll = new LearnSite.BLL.Works();
                int todayScore = workBll.GetTodayWorkScores(cook.Snum);
                
                if (todayScore < fopenValue)
                {
                    ShowError("学分不足", "您今天的作品学分为 " + todayScore + " 分，需要达到 " + fopenValue + " 分才能访问此游戏。");
                    return false;
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            ShowError("验证失败", "分数验证过程中出现错误：" + ex.Message);
            return false;
        }
    }
    
    private void ShowError(string title, string message)
    {
        litScript.Text = string.Format(
            "<script type=\"text/javascript\">showError('{0}', '{1}');</script>",
            title.Replace("'", "\\'"),
            message.Replace("'", "\\'")
        );
    }
    
    private void LoadGame(string url)
    {
        litScript.Text = string.Format(
            "<script type=\"text/javascript\">loadGame('{0}');</script>",
            url.Replace("'", "\\'")
        );
    }
}
