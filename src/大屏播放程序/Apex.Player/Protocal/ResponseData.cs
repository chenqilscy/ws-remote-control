// 文件说明
// SOLUTION    ： SmartLightBox V2.0
// PROJECT     ： SmartLightBox.Service
// FILENAME    ： ResponseData.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-04 14:41
// UPDATE TIME ： 2018-08-04 14:59
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

#region 命名空间

using System;
using NewLife.Serialization;
using Newtonsoft.Json;

#endregion

namespace Apex.Player.Protocal
{
    /// <summary>
    ///     客户端调用返回数据
    /// </summary>
    public class ResponseData 
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public ResponseData()
        {
            Status = StatusCode.Success;
        }

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        public ResponseData(bool result, dynamic data = null)
        {
            Set(result, data);
        }

        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ResponseData(StatusCode status, string message = "", dynamic data = null)
        {
            Set(status, message, data);
        }

        /// <summary>
        ///     状态码
        /// </summary>
        [JsonProperty("status")]
        public StatusCode Status { get; set; }

        /// <summary>
        ///     信息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        ///     数据内容
        /// </summary>
        [JsonProperty("data")]
        public dynamic Data { get; set; }

        /// <summary>
        ///     设置数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        public void Set(bool result, dynamic data = null)
        {
            Status = result ? StatusCode.Success : StatusCode.InternalError;
            Message = result.ToString();
            Data = data;
        }

        /// <summary>
        ///     设置数据
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        public void Set(StatusCode status, string message, dynamic data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        /// <summary>返回表示当前对象的字符串。</summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            if (Message.IsNullOrWhiteSpace())
            {
                Message = Status.GetDescription();
            }

            return JsonConvert.SerializeObject(this);
        }
    }
}