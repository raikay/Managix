using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Managix.Repository.Entities.Base
{
    /// <summary>
    /// 权限
    /// </summary>
	[Table("ad_permission")]
    public class PermissionEntity : Root
    {
        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [MaxLength(50)]
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        [MaxLength(550)]
        public string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 视图
        /// </summary>
        public string ViewPath { get; set; }

        
        /// <summary>
        /// 菜单访问地址
        /// </summary>
        [MaxLength(500)]
        public string Path { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        [MaxLength(5)]
        public string HttpMethod { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(500)]
        public string Icon { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; } = false;

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 可关闭
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// 打开组
        /// </summary>
        public bool? Opened { get; set; }

        /// <summary>
        /// 打开新窗口
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// 链接外显
        /// </summary>
        public bool? External { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
