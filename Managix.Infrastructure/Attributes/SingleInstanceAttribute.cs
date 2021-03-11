using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure.Attributes
{
    /// <summary>
    /// 单例注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class SingleInstanceAttribute : Attribute
    {
    }
}
