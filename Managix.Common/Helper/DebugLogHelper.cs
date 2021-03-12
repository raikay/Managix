namespace Managix.Infrastructure
{
    /// <summary>
    /// 日志
    /// </summary>
    public class DebugLogHelper
    {
        private static readonly object obj = new object();

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="message">记录信息</param>
        /// <param name="businessName">业务名称</param>
        /// <param name="logPath">保存路径</param>
        public static void Log(string message, string businessName = "", string logPath = "DebugLogs")
        {
            lock (obj)
            {
                System.DateTime dt = System.DateTime.Now;
                message = "\r\n" + dt.ToString() + ":\r\n" + message + "\r\n";
                var filePath = $@"{System.AppDomain.CurrentDomain.BaseDirectory}\\{logPath}";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                businessName = string.IsNullOrWhiteSpace(businessName) ? string.Empty : "-" + businessName;
                string fileName = $@"{ filePath}\\{dt.ToString("yyyyMMddHH")}{businessName}.log";
                using (System.IO.FileStream myFileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite))
                {
                    byte[] byteArr = System.Text.Encoding.Default.GetBytes(message);
                    myFileStream.Write(byteArr, 0, byteArr.Length);
                    myFileStream.Close();
                }
            }

        }
    }
}
