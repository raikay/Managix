using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Dtos
{
    public class DocumentUploadImageInput
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }
        /// <summary>
        /// 上传文件
        /// </summary>
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }
        /// <summary>
        /// 文档编号
        /// </summary>
        public long Id { get; set; }
    }

    public class DocumentAddImageInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Url { get; set; }
    }
}
