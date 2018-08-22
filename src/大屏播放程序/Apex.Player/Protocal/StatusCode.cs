// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： StatusCode.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 16:51
// UPDATE TIME ： 2018-08-19 16:53
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

#region 命名空间

using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace Apex.Player.Protocal
{
    /// <summary>
    ///     错误类型
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        ///     正常
        /// </summary>
        [Description("正常")] [EnumMember(Value = "正常")] Success,


        /// <summary>
        ///     参数错误
        /// </summary>
        [Description("参数错误")] [EnumMember(Value = "参数错误")] ArgumentError,


        /// <summary>
        ///     不支持的数据格式
        /// </summary>
        [Description("不支持的数据格式")] [EnumMember(Value = "不支持的数据格式")] NotSupportedFormat,


        /// <summary>
        ///     内部错误
        /// </summary>
        [Description("内部错误")] [EnumMember(Value = "内部错误")] InternalError,


        /// <summary>
        ///     无数据内容
        /// </summary>
        [Description("无数据内容")] [EnumMember(Value = "无数据内容")] NoContent,


        /// <summary>
        ///     无法处理请求的数据
        /// </summary>
        [Description("无法处理请求的数据")] [EnumMember(Value = "无法处理请求的数据")] CanNotProcessRequestData,


        [Description("ws server 为空")] [EnumMember(Value = "ws server 为空")] WsServerIsNull,
        [Description("ws server 未启动")] [EnumMember(Value = "ws server 未启动")] WsServerNotStart,
        [Description("ws server 已启动")] [EnumMember(Value = "ws server 已启动")] WsServerStarted,
        [Description("ws server 已停止")] [EnumMember(Value = "ws server 已停止")] WsServerStoped,
    }
}