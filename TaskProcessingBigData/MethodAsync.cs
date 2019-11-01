using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingBigData
{
    public class MethodAsync
    {
        /// <summary>
        /// 异步方法
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetReturnResult()
        {
            return await Task.Run(() =>
            {
               
                return "";
            });
        }
    }
}
