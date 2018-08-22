// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： PicConfig.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 17:13
// UPDATE TIME ： 2018-08-19 17:13
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

using NewLife.Json;

namespace Apex.Player.PFile.Pic
{
    [JsonConfigFile("pic.config")]
    public class PicConfig : JsonConfig<PicConfig>
    {
        /// <summary>
        /// 播放时间：秒
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 支持的后缀
        /// </summary>
        public string Ext { get; set; }


        /// <summary>新创建配置文件时执行</summary>
        protected override void OnNew()
        {
            Interval = 5;
            Ext = ".jpg;.jpeg;.bmp;.png;";

            base.OnNew();
        }
    }
}