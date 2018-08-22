// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： AppModel.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 16:33
// UPDATE TIME ： 2018-08-19 17:18
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

#region 命名空间

using Newtonsoft.Json;

#endregion

namespace Apex.Player.Protocal
{
    public class AppModel
    {
        [JsonProperty("function")]
        public int Function { get; set; }

        [JsonIgnore]
        public PFunction PFunction => (PFunction)Function;

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}