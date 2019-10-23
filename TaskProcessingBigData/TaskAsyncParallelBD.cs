using Framework.CDQXIN.Utils;
using Framework.CDQXIN.Utils.ExtensionHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessingBigData.Model;

namespace TaskProcessingBigData
{
    public class TaskAsyncParallelBD
    {
        #region 同步Courseware表
        /// <summary>
        /// 同步Courseware表数据（从lianxue库同步到BSchool）
        /// </summary>
        /// <param name="synchService"></param>
        public  void SynchCourseware()
        {
            List<CoursewarePo> listExcept = null;
            List<CoursewarePo> listExceptBSNone = null;
            List<CoursewarePo> listExceptBSHave = null;
            List<CoursewarePo> listLX = null;
            List<CoursewarePo> listBS = null;
            int totalRecordLX = 0, totalRecordBS = 0;//总记录数
            int pageSize = 10000;//每页最大记录数
            int totalPageLX = 0, totalPageBS = 0, totalPage = 0;//总页数
            try
            {
                totalRecordLX = GetCoursewareTotalCount_Lianxue();
                totalPageLX = CommonMethod.GetTotalPage(totalRecordLX, pageSize);
                totalRecordBS = GetCoursewareTotalCount_BSchool();
                totalPageBS = CommonMethod.GetTotalPage(totalRecordBS, pageSize);
                totalPage = totalPageLX >= totalPageBS ? totalPageLX : totalPageBS;
                for (int pindex = 1; pindex <= totalPage; pindex++)
                {
                    if (totalPageLX >= pindex)
                    {
                        listLX = GetCoursewareList_Lianxue(pindex, pageSize);
                    }
                    else
                    {
                        listLX = new List<CoursewarePo>();
                    }
                    if (totalPageBS >= pindex)
                    {
                        listBS = GetCoursewareList_BSchool(pindex, pageSize);
                    }
                    else
                    {
                        listBS = new List<CoursewarePo>();
                    }
                    //listExcept = listLX.Except(listBS, new CoursewareListListEquality()).ToList();//差集
                    listLX.ForEach(e => e.Key = e.ToJson().GetMd5());
                    listBS.ForEach(e => e.Key = e.ToJson().GetMd5());
                    listExcept = listLX.Where(e => listBS.All(o => o.Key != e.Key)).ToList();
                    listExceptBSNone = listExcept.Where(p => !listBS.Exists(m => m.CoursewareId == p.CoursewareId)).ToList();
                    //TaskGradeBSNone(synchService, listExceptBSNone);
                    listExceptBSHave = listExcept.Where(p => listBS.Exists(m => m.CoursewareId == p.CoursewareId)).ToList();
                    //TaskGradeBSHave(synchService, listExceptBSHave);
                    Parallel.Invoke(() => TaskCoursewareBSNone(listExceptBSNone), () => TaskCoursewareBSHave(listExceptBSHave));
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (listExcept != null)
                {
                    listExcept.Clear();
                    listExcept = null;
                }
                if (listLX != null)
                {
                    listLX.Clear();
                    listLX = null;
                }
                if (listBS != null)
                {
                    listBS.Clear();
                    listBS = null;
                }
                if (listExceptBSNone != null)
                {
                    listExceptBSNone.Clear();
                    listExceptBSNone = null;
                }
                if (listExceptBSHave != null)
                {
                    listExceptBSHave.Clear();
                    listExceptBSHave = null;
                }
            }

        }
        /// <summary>
        /// 更新Courseware数据
        /// </summary>
        /// <param name="synchService"></param>
        /// <param name="listExceptBSHave"></param>
        private void TaskCoursewareBSHave( List<CoursewarePo> listExceptBSHave)
        {
            var pagelistHave = listExceptBSHave.Paging(1000);
            var tasklistHave = pagelistHave.Select(page => Task.Run(() =>
            {
                UpdateBaseData_Courseware(page);
            }));
            Task.WaitAll(tasklistHave.ToArray());
        }
        /// <summary>
        /// 插入Courseware数据
        /// </summary>
        /// <param name="synchService"></param>
        /// <param name="listExceptBSNone"></param>
        private void TaskCoursewareBSNone( List<CoursewarePo> listExceptBSNone)
        {
            var pagelistNone = listExceptBSNone.Paging(1000);
            var tasklistNone = pagelistNone.Select(page => Task.Run(() =>
            {
                InsertBaseData_Courseware( page);
            }));
            Task.WaitAll(tasklistNone.ToArray());
        }
        /// <summary>
        /// 更新Courseware表
        /// </summary>
        /// <param name="synchService"></param>
        /// <param name="listExceptBSHave"></param>
        private void UpdateBaseData_Courseware( List<CoursewarePo> listExceptBSHave)
        {
            if (listExceptBSHave.Count <= 0)
            {
                /*this.OpenOperator.Log("[任务][SynchCourseware]暂无执行UpdateBaseData_Courseware更新数据计划\r\n")*/;
            }
            else
            {
                //this.OpenOperator.Log("[任务][SynchCourseware]开始执行UpdateBaseData_Courseware更新数据\r\n");
                var flagUpdate = UpdateBaseData_CoursewareDal(listExceptBSHave);
                //this.OpenOperator.Log($"[任务][SynchCourseware]结束执行UpdateBaseData_Courseware更新数据,影响条数{flagUpdate}\r\n");
            }

        }
        /// <summary>
        /// 插入Courseware表
        /// </summary>
        /// <param name="synchService"></param>
        /// <param name="listExceptBSNone"></param>
        private void InsertBaseData_Courseware(List<CoursewarePo> listExceptBSNone)
        {
            if (listExceptBSNone.Count <= 0)
            {
                //this.OpenOperator.Log("[任务][SynchCourseware]暂无执行InsertBaseData_Courseware插入数据计划\r\n");
            }
            else
            {
                //this.OpenOperator.Log("[任务][SynchCourseware]开始执行InsertBaseData_Courseware插入数据\r\n");
                var flagInsert = InsertBaseData_CoursewareDal(listExceptBSNone);
                //this.OpenOperator.Log($"[任务][SynchCourseware]结束执行InsertBaseData_Courseware插入数据,影响条数{flagInsert}\r\n");
            }

        }

        /// <summary>
        /// 获取lianxue库Courseware总条数
        /// </summary>
        /// <returns></returns>
        public int GetCoursewareTotalCount_Lianxue()
        {
            string sql = "SELECT COUNT(CoursewareId)  FROM [Courseware](nolock) ";
            object obj = SQLHelper.ExecuteScalar("", CommandType.Text, sql);
            return ConvertHelper.GetInteger(obj);
        }
        /// <summary>
        /// 获取Courseware列表Lianxue
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<CoursewarePo> GetCoursewareList_Lianxue(int pageIndex, int pageSize)
        {
            var parm = new[]
          {
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageIndex", pageIndex)
            };
            string sqlstr = @" SELECT TOP  " + pageSize + " CoursewareId, CourseId, CoursewareName, Schooltime, SchoolEndTime, CoursewareType, OnLineUrl, DownloadUrl, DownloadUrlMobile, IsFree, Describe, DisplayOrder, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsDisabled, IsDeleted, TmpId, BrandId, AppOnlineAddress, a, GeneralDownloadUrl, CourseTime, CourseData, CourseState, CourseBackPlay, CourseIsFree, SJID, FileSize, Duration, DocumentId, CoursewareSuffix, DocumentStatus FROM( SELECT ROW_NUMBER() OVER (ORDER BY CoursewareId) AS RowNumber,CoursewareId, CourseId, CoursewareName, Schooltime, SchoolEndTime, CoursewareType, OnLineUrl, DownloadUrl, DownloadUrlMobile, IsFree, Describe, DisplayOrder, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsDisabled, IsDeleted, TmpId, BrandId, AppOnlineAddress, a, GeneralDownloadUrl, CourseTime, CourseData, CourseState, CourseBackPlay, CourseIsFree, SJID, FileSize, Duration, DocumentId, CoursewareSuffix, DocumentStatus FROM Courseware(nolock) )as A WHERE RowNumber > @PageSize*(@PageIndex-1) order by CoursewareId";
            List<CoursewarePo> list = new List<CoursewarePo>();

            DataTable dtbl = SQLHelper.ExecuteDataset("", CommandType.Text, sqlstr, parm).Tables[0];
            if (dtbl.Rows.Count > 0)
            {
                list = EntityHelper.ConvertToEntityList<CoursewarePo>(dtbl);
            }

            return list;
        }

        /// <summary>
        /// 获取BSchool库Courseware总条数
        /// </summary>
        /// <returns></returns>
        public int GetCoursewareTotalCount_BSchool()
        {
            string sql = "SELECT COUNT(CoursewareId)  FROM [Courseware](nolock) ";
            object obj = SQLHelper.ExecuteScalar("", CommandType.Text, sql);
            return ConvertHelper.GetInteger(obj);
        }

        /// <summary>
        /// 获取Courseware列表BSchool
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<CoursewarePo> GetCoursewareList_BSchool(int pageIndex, int pageSize)
        {
            var parm = new[]
          {
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageIndex", pageIndex)
            };
            string sqlstr = @" SELECT TOP  " + pageSize + " CoursewareId, CourseId, CoursewareName, Schooltime, SchoolEndTime, CoursewareType, OnLineUrl, DownloadUrl, DownloadUrlMobile, IsFree, Describe, DisplayOrder, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsDisabled, IsDeleted, TmpId, BrandId, AppOnlineAddress, a, GeneralDownloadUrl, CourseTime, CourseData, CourseState, CourseBackPlay, CourseIsFree, SJID, FileSize, Duration, DocumentId, CoursewareSuffix, DocumentStatus FROM( SELECT ROW_NUMBER() OVER (ORDER BY CoursewareId) AS RowNumber,CoursewareId, CourseId, CoursewareName, Schooltime, SchoolEndTime, CoursewareType, OnLineUrl, DownloadUrl, DownloadUrlMobile, IsFree, Describe, DisplayOrder, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsDisabled, IsDeleted, TmpId, BrandId, AppOnlineAddress, a, GeneralDownloadUrl, CourseTime, CourseData, CourseState, CourseBackPlay, CourseIsFree, SJID, FileSize, Duration, DocumentId, CoursewareSuffix, DocumentStatus FROM Courseware(nolock) )as A WHERE RowNumber > @PageSize*(@PageIndex-1) order by CoursewareId";
            List<CoursewarePo> list = new List<CoursewarePo>();

            DataTable dtbl = SQLHelper.ExecuteDataset("", CommandType.Text, sqlstr, parm).Tables[0];
            if (dtbl.Rows.Count > 0)
            {
                list = EntityHelper.ConvertToEntityList<CoursewarePo>(dtbl);
            }

            return list;
        }

        /// <summary>
        /// 插入Courseware
        /// </summary>
        /// <returns></returns>
        public int InsertBaseData_CoursewareDal(List<CoursewarePo> list)
        {
            DataTable dt = DataTableExtHelper.ToDataTableExtend<CoursewarePo>(list, new List<string>() { "key" });
            var parm = new[]
           {
                new SqlParameter("@CoursewareList", dt)
            };
            int result = SQLHelper.ExecuteNonQuery("", CommandType.StoredProcedure, "PR_BDSynch_Insert_Courseware", parm);
            return result;
        }

        /// <summary>
        /// 更新Courseware
        /// </summary>
        /// <returns></returns>
        public int UpdateBaseData_CoursewareDal(List<CoursewarePo> list)
        {
            DataTable dt = DataTableExtHelper.ToDataTableExtend<CoursewarePo>(list, new List<string>() { "key" });
            var parm = new[]
           {
                new SqlParameter("@CoursewareList", dt)
            };
            int result = SQLHelper.ExecuteNonQuery("", CommandType.StoredProcedure, "PR_BDSynch_Update_Courseware", parm);
            return result;
        }
        #endregion
    }
}
