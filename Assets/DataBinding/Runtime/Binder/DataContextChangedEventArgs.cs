using System;

namespace UnityEngine
{
    /// <summary>
    /// 数据上下文改变事件参数
    /// </summary>
    public sealed class DataContextChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 属性路径
        /// </summary>
        public readonly string Path;

        public DataContextChangedEventArgs(string path)
        {
            this.Path = path;
        }
    }
}