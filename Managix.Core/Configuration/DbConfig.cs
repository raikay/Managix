﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Configuration
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        public string TestName { set; get; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        //public DataType Type { get; set; } = System.ComponentModel.DataAnnotations.DataType.Sqlite;

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 生成数据
        /// </summary>
        public bool GenerateData { get; set; }

        /// <summary>
        /// 同步结构
        /// </summary>
        public bool SyncStructure { get; set; } 

        /// <summary>
        /// 同步数据
        /// </summary>
        public bool SyncData { get; set; } 

        /// <summary>
        /// 建库
        /// </summary>
        public bool CreateDb { get; set; } 

        /// <summary>
        /// 建库连接字符串
        /// </summary>
        public string CreateDbConnectionString { get; set; }

        /// <summary>
        /// 建库脚本
        /// </summary>
        public string CreateDbSql { get; set; }

        /// <summary>
        /// 监听所有操作
        /// </summary>
        public bool MonitorCommand { get; set; } 

        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public bool Curd { get; set; } 
    }
}
