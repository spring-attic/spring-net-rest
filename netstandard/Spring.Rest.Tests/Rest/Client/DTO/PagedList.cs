using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring.Rest.Tests.Rest.Client.DTO
{/// <summary>
 /// 分页数据列表
 /// </summary>
 /// <typeparam name="T"></typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// 数据内容
        /// </summary>
        public IList<T> Content;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize;

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount;

        /// <summary>
        /// 授权状态
        /// </summary>
        public bool AuthorizeStatus { get; set; }

        /// <summary>
        /// 操作状态(false时将返回空数据)
        /// </summary>
        public bool OperationStatus { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 消息详细列表
        /// </summary>
        public List<string> MsgDetails { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedList()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            this.Content = source.ToList();
            this.PageIndex = index;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        public PagedList(IEnumerable<T> source, int index, int pageSize, long totalRecords)
        {
            this.Content = source.ToList();
            this.PageIndex = index;
            this.PageSize = pageSize;
            this.TotalCount = totalRecords;
        }
    }


    /// <summary>
    /// 业务平台登录日志Dto
    /// </summary>
    public class BusinessLoginLogDto
    {
        ///<summary>
        ///登录账号
        ///</summary>
        public virtual string Account { get; set; }

        ///<summary>
        ///尝试密码
        ///</summary>
        public virtual string TryPass { get; set; }

        /// <summary>
        /// 登录结果
        /// </summary>
        public virtual string LoginResultText { get; set; }

        ///<summary>
        ///登录时间
        ///</summary>
        public virtual DateTime OPTime { get; set; }

        ///<summary>
        ///访问IP
        ///</summary>
        public virtual string AccessIP { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        public virtual string Note { get; set; }
    }
}
