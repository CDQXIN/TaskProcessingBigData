using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingBigData.Model
{
    /// <summary>
    /// 课件表
    /// </summary>
    public class CoursewarePo
    {
        /// <summary>
        /// 课件编号
        /// </summary>
        public Guid CoursewareId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// 课件名称
        /// </summary>
        public string CoursewareName { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime Schooltime { get; set; }

        /// <summary>
        /// 上课预计结束时间
        /// </summary>
        public DateTime? SchoolEndTime { get; set; }

        /// <summary>
        /// 课件类型(1:普通视频,2:高清视频,3:板书,4:讲义,5:直播(展示互动),6:CC视频,7:能力天空,8:保利威视,9:月考,10 exe文件)
        /// </summary>
        public byte CoursewareType { get; set; }

        /// <summary>
        /// 在线地址
        /// </summary>
        public string OnLineUrl { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// 手机下载网址
        /// </summary>
        public string DownloadUrlMobile { get; set; }

        /// <summary>
        /// 是否免费 :  0:VIP  1:免费
        /// </summary>
        public bool IsFree { get; set; }

        /// <summary>
        /// 课件描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public Guid ModifiedBy { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 是否禁用(0:启用 1:禁用)
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否删除(0:未删除 1:已删除)
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int? TmpId { get; set; }

        /// <summary>
        /// 品牌id，做数据区分用，默认值都为链学的id‘d5a8cb67-54ba-448c-bab9-4044d74081a7’
        /// </summary>
        public Guid BrandId { get; set; }

        /// <summary>
        /// 苹果在线地址
        /// </summary>
        public string AppOnlineAddress { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        public int a { get; set; }

        /// <summary>
        /// 通用下载地址
        /// </summary>
        public string GeneralDownloadUrl { get; set; }

        /// <summary>
        /// 课程时长(该字段不用)
        /// </summary>
        public string CourseTime { get; set; }

        /// <summary>
        /// 课程资料下载地址（该字段不用）
        /// </summary>
        public string CourseData { get; set; }

        /// <summary>
        /// 前台是否显示   1: 不显示  2:显示
        /// </summary>
        public int? CourseState { get; set; }

        /// <summary>
        /// 直播课件的回放地址(该字段不用)
        /// </summary>
        public string CourseBackPlay { get; set; }

        /// <summary>
        /// 直播课件的是否免费(该字段不用)
        /// </summary>
        public int? CourseIsFree { get; set; }

        /// <summary>
        /// 课件关联试卷ID
        /// </summary>
        public int SJID { get; set; }

        /// <summary>
        /// 文件大小(单位:byte 字节)
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 时长(单位:s 秒)
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 百度文档Id
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// 文档后缀
        /// </summary>
        public string CoursewareSuffix { get; set; }

        /// <summary>
        /// 百度文档是否发布1:未发布;2:发布成功;3:发布失败
        /// </summary>
        public byte? DocumentStatus { get; set; }
        /// <summary>
        /// 数据同步Key
        /// </summary>
        public string Key { get; set; }
    }
}
