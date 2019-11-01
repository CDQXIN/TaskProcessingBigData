# TaskProcessingBigData
大数据同步方案（数据库分页，内存分页，同步，异步，多线程运用）
Sqlserver 高性能分页语句：

SELECT TOP 页大小 *

FROM( SELECT ROW_NUMBER() OVER (ORDER BY id) AS RowNumber,* FROM table1 )as A 

WHERE RowNumber > 页大小*(页数-1)  order by 字段 

 

--注解：首先利用Row_number()为table1表的每一行添加一个行号，给行号这一列取名'RowNumber' 在over()方法中将'RowNumber'做了升序排列

--然后将'RowNumber'列 与table1表的所有列 形成一个表A

--重点在where条件。假如当前页(currentPage)是第2页，每页显示10个数据(pageSzie)。那么第一页的数据就是第11-20条

--所以为了显示第二页的数据，即显示第11-20条数据，那么就让RowNumber大于 10*(2-1)
--order by 字段 对于同步数据非常重要，便于对比

分页的总页数算法：

        public static int GetTotalPage(int totalRecord, int pageSize)
        {
            return (totalRecord + pageSize - 1) / pageSize;
        }
        
集合求差集（此方法比较高效）：

        listLX.ForEach(e => e.Key = e.ToJson().GetMd5());
        listBS.ForEach(e => e.Key = e.ToJson().GetMd5());
        listExcept = listLX.Where(e => listBS.All(o => o.Key != e.Key)).ToList();
        listExceptBSNone = listExcept.Where(p => !listBS.Exists(m => m.CoursewareId == p.CoursewareId)).ToList();
        
异步多线程：

        private void TaskCourseBSNone(IBaseDataSynchronizationService synchService, List<CoursePo> listExceptBSNone)
        {
            var pagelistNone = listExceptBSNone.Paging(1000);
            var tasklistNone = pagelistNone.Select(page => Task.Run(() =>
            {
                InsertBaseData_Course(synchService, page);
            }));
            Task.WaitAll(tasklistNone.ToArray());
        }
        
多线程同步：

Parallel.Invoke(() => TaskCoursewareBSNone(listExceptBSNone), () => TaskCoursewareBSHave(listExceptBSHave));

