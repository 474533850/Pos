using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 数据上传状态
    /// </summary>
    public enum UploadStatus
    {
        /// <summary>
        /// 未上传
        /// </summary>
        NotUploaded=0,
        /// <summary>
        /// 上传中
        /// </summary>
        Uploading=1,
        /// <summary>
        /// 已上传
        /// </summary>
        Uploaded=2
    }
}
