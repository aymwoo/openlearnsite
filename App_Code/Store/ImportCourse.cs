using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
namespace LearnSite.Store
{
    /// <summary>
    ///ImportCourse 的摘要说明
    /// </summary>
    public class ImportCourse
    {
        /// <summary>
        /// 保存上传学案包，获得保存文件名及物理路径
        /// </summary>
        /// <param name="fudpackage"></param>
        /// <returns></returns>
        public static int PackageUpload(FileUpload fudpackage, int cobj, int Hid)
        {
            int msg = -1;
            string saveFile = "";
            try
            {
                string uploadfile = fudpackage.PostedFile.FileName;
                if (uploadfile != "")
                {
                    string fileName = System.IO.Path.GetFileName(uploadfile);
                    string fileType = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    string ftl = fileType.ToLower();
                    if (ftl == "rar" || ftl == "zip")
                    {
                        string rnd = DateTime.Now.Millisecond.ToString();
                        string savePath = CreateTempPath();//保存物理路径
                        saveFile = Common.Flatform.Checkbdir(savePath) + "file_" + rnd + "." + ftl;//保存文件名物理路径
                        fudpackage.SaveAs(saveFile);//按随机文件名保存
                        if (LearnSite.Store.SharpZip.UnpackFilesXml(saveFile, savePath)) //将上传的学案包中的xml文件解压到当前文件夹下                      
                        {
                            msg = -2;
                            int newCid = ImportXml(savePath, cobj, Hid); //从当前文件夹读取 xml文件，将学案导入到数据库中（自动修改学案内容的链接地址）
                            if (newCid != -1)    //再创建新Cid文件夹，再学案包再解压到新Cid文件夹中。
                            {
                                string newCidPath = HttpContext.Current.Server.MapPath(LearnSite.Store.CourseStore.CreateStore(newCid));
                                LearnSite.Store.SharpZip.UnpackFiles(saveFile, newCidPath);//将学案包重新解压到新学案目录下
                                DelOtherFiles(newCidPath);//递归调用自己，直接删除目录及子目录下后缀为asp|aspx|exe的非法文件
                                //LearnSite.Store.XmlCourse.CourseToXml(newCid);//再在新建Cid文件夹中重建xml 

                                msg = newCid;//导入成功则返回学案自动编号
                            }
                            else
                            {
                                msg = -3;
                            }
                        }
                        else
                        {
                            msg = -4;
                        }
                        System.Threading.Thread.Sleep(200);
                        Directory.Delete(savePath, true);//最后删除临时文件夹
                    }
                    else
                    {
                        msg = -5;
                    }
                }
                else
                {
                    msg = -6;
                }
            }
            catch( Exception ex)
            {
                LearnSite.Common.Log.Addlog("学案导入失败信息：", ex.ToString());
            }

            return msg;
        }
        /// <summary>
        /// 返回生成的临时解包文件夹
        /// </summary>
        /// <returns></returns>
        private static string CreateTempPath()
        {
            string tempPath = "~/tempcs" + "/";
            string tempRealPath = HttpContext.Current.Server.MapPath(tempPath);
            if (Directory.Exists(tempRealPath))
            {
                Directory.Delete(tempRealPath,true);
            }
            Directory.CreateDirectory(tempRealPath);
            return tempRealPath;
        }
        private static int ImportXml(string xmlPath, int cobj, int Hid)
        {
            int newCid = -1;
            string xmlFile = Common.Flatform.Checkbdir(xmlPath) + "Course.xml";
            if (File.Exists(xmlFile))
            {
                DataSet ds = new DataSet();
                DataTable dtCourse = new DataTable();
                DataTable dtMission = new DataTable();
                DataTable dtTopicDicuss = new DataTable();
                DataTable dtExam = new DataTable();
                DataTable dtTxtForm = new DataTable();
                DataTable dtListMenu = new DataTable();
                DataTable dtConsole = new DataTable();
                DataTable dtProblem = new DataTable();
                DataTable dtJudgeArg = new DataTable();

                ds.ReadXml(xmlFile);//读取xml文件到ds
                if (ds.Tables.Contains("Course"))
                    dtCourse = ds.Tables["Course"];//获得学案表course
                if (ds.Tables.Contains("Mission"))
                    dtMission = ds.Tables["Mission"];//获得活动表mission 
                if (ds.Tables.Contains("TopicDiscuss"))
                    dtTopicDicuss = ds.Tables["TopicDiscuss"];//获得讨论表
                if (ds.Tables.Contains("Exams"))
                    dtExam = ds.Tables["Exams"];//获得测验表
                if (ds.Tables.Contains("TxtForm"))
                    dtTxtForm = ds.Tables["TxtForm"];
                if (ds.Tables.Contains("ListMenu"))
                    dtListMenu = ds.Tables["ListMenu"];//获得学案导航表
                if (ds.Tables.Contains("Console"))
                    dtConsole = ds.Tables["Console"];//获得测评表
                if (ds.Tables.Contains("Problem"))
                    dtProblem = ds.Tables["Problem"];//获得测评试题表
                if (ds.Tables.Contains("JudgeArg"))
                    dtJudgeArg = ds.Tables["JudgeArg"];//获得自动批改表

                
                if (dtCourse != null && dtMission != null)
                {
                    newCid = CreateCourse(dtCourse, cobj, Hid);//创建新学案，返回学案编号
                    CreateMission(dtMission, dtJudgeArg, newCid, Hid);//将活动添加到新学案下，循环解决两张表编号关联
                    if (dtTopicDicuss != null)
                        CreateTopicDiscuss(dtTopicDicuss, newCid, Hid);
                    if (dtExam != null)
                        CreateExam(dtExam, newCid, Hid);//循环解决三张表编号关联
                    if (dtTxtForm != null)
                        CreateTxtForm(dtTxtForm, newCid);
                    if (dtConsole != null)
                        CreateConsole(dtConsole, dtProblem, newCid, Hid);//循环解决两张表编号关联

                    //要重建导航内编号
                    LearnSite.BLL.ListMenu lbll = new BLL.ListMenu();
                    lbll.importmenu(newCid);
                    if (dtListMenu != null)
                        lbll.importupsort(dtListMenu, newCid);
                }
                dtCourse.Dispose();
                dtMission.Dispose();
                dtTopicDicuss.Dispose();
                dtExam.Dispose();
                dtTxtForm.Dispose();
                dtListMenu.Dispose();
                ds.Dispose();
                dtConsole.Dispose();
                dtProblem.Dispose();
                dtJudgeArg.Dispose();

                //File.Delete(xmlFile);
            }
            return newCid;
        }

        /// <summary>
        /// 添加测评
        /// </summary>
        /// <param name="cdt"></param>
        /// <param name="pdt"></param>
        /// <param name="Cid"></param>
        /// <param name="Hid"></param>
        private static void CreateConsole(DataTable cdt, DataTable pdt, int Cid, int Hid)
        {
            int dCount = cdt.Rows.Count;
            LearnSite.BLL.Consoles bll = new LearnSite.BLL.Consoles();
            for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.Consoles model = new LearnSite.Model.Consoles();
                model = bll.GetModel(cdt, i);
                int oldMcid = model.Ncid.Value;
                string thisMcontent = model.Ncontent;
                model.Ncid = Cid;//更换成新学案编号
                model.Nhid = Hid;//换成导入老师
                string oldstr = "store/" + oldMcid.ToString();
                string newstr = "store/" + Cid.ToString();
                model.Ncontent = CaseInsenstiveReplace(thisMcontent, oldstr, newstr);//替换链接地址
                int oldNid = model.Nid;
                int newNid = bll.Add(model);//增加测评
                if (pdt != null)
                {
                    DataView dv = new DataView(pdt);
                    dv.RowFilter = "Pnid=" + oldNid.ToString();//直接过滤得到该旧测评的试题
                    CreateProblem(dv.ToTable(), newNid, Hid, Cid);
                }
            }
        }


        /// <summary>
        /// 添加测评试题
        /// </summary>
        /// <param name="dtTopicDiscuss"></param>
        /// <param name="Cid"></param>
        private static void CreateProblem(DataTable dt, int Nid, int Hid,int Cid)
        {
            int dCount = dt.Rows.Count;
            LearnSite.BLL.Problems bll = new LearnSite.BLL.Problems();
            for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.Problems model = new LearnSite.Model.Problems();
                model = bll.GetModel(dt,i);
                model.Pnid = Nid;//更换成测评试题编号
                model.Phid = Hid;//换成导入老师
                model.Pcid = Cid;
                bll.Add(model);//增加测评试题
            }
        }
       /// <summary>
        /// 添加学案调查ok
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Cid"></param>
        private static void CreateExam(DataTable dt, int Cid, int Hid)
        {
            int dCount = dt.Rows.Count;
            LearnSite.BLL.Exams bll = new LearnSite.BLL.Exams();
            for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.Exams model = new LearnSite.Model.Exams();
                model = bll.GetModelDataRow(dt, i);
                model.Cid = Cid;//更换成新学案编号
                model.Hid = Hid;//换成导入老师
                int newvid = bll.Add(model);//增加学案调查
            }
        }
        /// <summary>
        /// 添加表单
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Cid"></param>
        /// <param name="Hid"></param>
        private static void CreateTxtForm(DataTable dt, int Cid)
        {
            int dCount = dt.Rows.Count;
            LearnSite.BLL.TxtForm bll = new BLL.TxtForm();
            for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.TxtForm model = new Model.TxtForm();
                model = bll.DataRowToModel(dt.Rows[i]);
                int oldMcid = model.Mcid.Value;
                string thisMcontent = model.Mcontent;
                model.Mcid = Cid;
                string oldstr = "store/" + oldMcid.ToString();
                string newstr = "store/" + Cid.ToString();
                model.Mcontent = CaseInsenstiveReplace(thisMcontent, oldstr, newstr);//替换链接地址
                bll.Add(model);
            }        
        }

        /// <summary>
        /// 添加学案讨论ok
        /// </summary>
        /// <param name="dtTopicDiscuss"></param>
        /// <param name="Cid"></param>
        private static void CreateTopicDiscuss(DataTable dt, int Cid, int Hid)
        {
            int dCount = dt.Rows.Count;
            LearnSite.BLL.TopicDiscuss bll = new LearnSite.BLL.TopicDiscuss();
           for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.TopicDiscuss model = new LearnSite.Model.TopicDiscuss();
                model = bll.GetModel(dt, i);
                int oldMcid = model.Tcid.Value;
                string thisMcontent = model.Tcontent;
                model.Tcid = Cid;//更换成新学案编号
                model.Tteacher = Hid;//换成导入老师
                string oldstr = "store/" + oldMcid.ToString();
                string newstr = "store/" + Cid.ToString();
                model.Tcontent = CaseInsenstiveReplace(thisMcontent, oldstr, newstr);//替换链接地址
                bll.Add(model);//增加学案讨论
            }
        }
        /// <summary>
        /// 添加学案活动
        /// </summary>
        /// <param name="dtMission"></param>
        /// <param name="dtJudgeArg"></param>
        /// <param name="Cid"></param>
        /// <param name="Hid"></param>
        private static void CreateMission(DataTable dtMission, DataTable dtJudgeArg, int Cid,int Hid)
        {
            int mCount = dtMission.Rows.Count;
            LearnSite.BLL.Mission bll = new LearnSite.BLL.Mission();
            for (int i = 0; i < mCount; i++)
            {
                LearnSite.Model.Mission ms = new LearnSite.Model.Mission();
                ms = bll.GetTableModel(dtMission, i);
                int oldMcid = ms.Mcid.Value;
                string thisMcontent = ms.Mcontent;
                ms.Mcid = Cid;//更换成新学案编号
                string oldstr = "store/" + oldMcid.ToString();
                string newstr = "store/" + Cid.ToString();
                ms.Mcontent = CaseInsenstiveReplace(thisMcontent, oldstr, newstr);//替换链接地址
                string thisMexample = ms.Mexample;
                ms.Mexample = thisMexample.Replace(oldstr, newstr);
                ms.Mgroup = false;
                int oldMid = ms.Mid;
                int newMid= bll.Add(ms);//增加学案活动

                if (dtJudgeArg != null)
                {
                    DataView dv = new DataView(dtJudgeArg);
                    if (dv.Table.Columns.Contains("Jmid"))
                    {
                        dv.RowFilter = "Jmid=" + oldMid.ToString();//直接过滤得到该旧测评的试题
                        CreateJudgeArg(dv.ToTable(), newMid, Hid, Cid);
                    }
                }
            }        
        }

        /// <summary>
        /// 添加测评试题
        /// </summary>
        /// <param name="dtTopicDiscuss"></param>
        /// <param name="Cid"></param>
        private static void CreateJudgeArg(DataTable dt, int Mid, int Hid, int Cid)
        {
            int dCount = dt.Rows.Count;
            LearnSite.BLL.JudgeArg bll = new LearnSite.BLL.JudgeArg();
            for (int i = 0; i < dCount; i++)
            {
                LearnSite.Model.JudgeArg model = new LearnSite.Model.JudgeArg();
                model = bll.GetModel(dt, i);
                model.Jmid = Mid;//更换成测评试题编号
                model.Jhid = Hid;//换成导入老师
                string oldcidstr = "/" + model.Jcid.ToString() + "/";
                string newcidstr = "/" + Cid.ToString() + "/";
                model.Jthumb = model.Jthumb.Replace(oldcidstr, newcidstr);
                model.Jcid = Cid;
                bll.Add(model);//增加测评试题
            }
        }
        
        /// <summary>
        /// 新建学案，返回学案编号
        /// </summary>
        /// <param name="dtCourse"></param>
        /// <returns></returns>
        private static int CreateCourse(DataTable dtCourse, int cobj, int Hid)
        {
            int newCid = 0;
            LearnSite.Model.Courses cs = new LearnSite.Model.Courses();
            LearnSite.BLL.Courses bll = new LearnSite.BLL.Courses();
            cs = bll.GetTableModel(dtCourse);//获得学案的model
            int thisCid = cs.Cid;
            string thisCcontent = cs.Ccontent;
            cs.Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());//替换成当前学期
            cs.Cdate = DateTime.Now;
            cs.Cobj = cobj;//替换成当前年级
            cs.Cks = bll.CksMaxValue(cs.Cterm.Value, cs.Cobj.Value, Hid);//替换成当前年级的最新课节
            cs.Chid = Hid;
            cs.Cfiletype = "txt";
            newCid = bll.Add(cs);//获得新增学案的Cid

            string oldstr = "store/" + thisCid.ToString();
            string newstr = "store/" + newCid.ToString();
            string newCcontent =CaseInsenstiveReplace(thisCcontent,oldstr, newstr);//替换链接地址
            string newCbanner = cs.Cbanner.Replace(thisCid.ToString(), newCid.ToString());
            bll.UpdateCcontent(newCid, newCcontent,newCbanner);//更新内容

            return newCid;
        }

        /// <summary>
        /// 递归调用自己，直接删除目录下后缀为asp|aspx的非法文件
        /// </summary>
        /// <param name="strDir">物理目录地址</param>
        private static void DelOtherFiles (string strDir)
        {
            if (Directory.Exists(strDir))
            {
                string[] strDirs = Directory.GetDirectories(strDir);
                string[] strFiles = Directory.GetFiles(strDir);
                foreach (string strFile in strFiles)
                {
                    bool isdel = false;
                    string[] strType = GetFileType();
                    foreach (string myType in strType)
                    {
                        if (myType == GetSingleFileType(strFile))
                        {
                            isdel = true;
                        }
                    }
                    if (isdel)
                    {
                        File.Delete(strFile);
                    }
                }

                foreach (string strdir in strDirs)
                {
                    DelOtherFiles(strdir);                       
                }

            }

        }


        /// <summary>
        /// 获取非法文件的后缀名的集合
        /// </summary>
        /// <returns></returns>
        private static string[] GetFileType()
        {
            string AllFileType = "asp|aspx";
            string[] GetFileTypes = AllFileType.Split(new char[] { '|' });
            return GetFileTypes;
        }

        /// <summary>
        /// 获取文件名后缀
        /// </summary>
        /// <param name="myfile"></param>
        /// <returns></returns>
        private static string GetSingleFileType(string myfile)
        {
            return myfile.Substring(myfile.LastIndexOf(".") + 1).ToLower();
        }

        static string CaseInsenstiveReplace(string originalString, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue,RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(originalString, newValue);
        }
    }
}