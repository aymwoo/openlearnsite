using System;
using System.Data;
using LearnSite.DAL;

namespace LearnSite.BLL
{
    public class Skdj
    {
        private LearnSite.DAL.Skdj dal = new LearnSite.DAL.Skdj();

        public DataSet GetAllList()
        {
            return dal.GetAllList();
        }

        /*/// <summary>
        /// 增加当前教师上课内容登记
        /// </summary>
        public int Addskdj(LearnSite.Model.skdj model)
        {
            return dal.Addskdj(model);
        }*/
        public int addskdj(int Sstid, DateTime Ssdate, int Ssgrade, int Ssclass, string Ssctitle, int Ssyear, int Ssmonth, int Ssday, string Ssweek, string Ssession, string Ssnotes, string Sstname)
        {
            // 实现添加 skdj 记录的逻辑
            return dal.addskdj(Sstid, Ssdate, Ssgrade, Ssclass, Ssctitle, Ssyear, Ssmonth, Ssday, Ssweek, Ssession, Ssnotes, Sstname);
        }
        
        public bool Exists(string condition)
        {
            return dal.Exists(condition);
        }

        public int Add(LearnSite.Model.Skdj model)
        {
           return dal.Add(model);
        }
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }
       public LearnSite.Model.Skdj GetModel(int id)
        {
            return dal.GetModel(id);
        }
        
        public bool Update(LearnSite.Model.Skdj model)
        {
            return dal.Update(model);
        }
        
        /// <summary>
        /// 获取今日课程记录
        /// </summary>
        public DataTable GetTodayRecords()
        {
            return dal.GetTodayRecords();
        }

        /// <summary>
        /// 导出上课登记表到Excel
        /// </summary>
        /// <param name="currentYearOnly">是否只导出当前年份</param>
        public void SkdjExcel(bool currentYearOnly)
        {
            dal.SkdjExcel(currentYearOnly);
        }
    }
}