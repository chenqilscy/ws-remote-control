// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： SetPicConfig.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 17:17
// UPDATE TIME ： 2018-08-19 17:17
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

using Newtonsoft.Json;

namespace Apex.Player.Protocal.Pic
{
    public class SetPicConfig
    {
        /// <summary>
        /// 播放时间：秒
        /// </summary>
        [JsonProperty("interval")] public int Interval { get; set; } = 5;

        /// <summary>
        /// 支持的后缀
        /// </summary>
        [JsonProperty("ext")] public string Ext { get; set; } = "jpg;jpeg;bmp;png;";
    }
}