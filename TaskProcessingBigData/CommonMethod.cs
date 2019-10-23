using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingBigData
{
    public class CommonMethod
    {
        #region 获取总页数
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="totalRecord">总条数</param>
        /// <param name="pageSize">每页最大记录数</param>
        /// <returns></returns>
        public static int GetTotalPage(int totalRecord, int pageSize)
        {
            return (totalRecord + pageSize - 1) / pageSize;
        }
        #endregion
    }
}
