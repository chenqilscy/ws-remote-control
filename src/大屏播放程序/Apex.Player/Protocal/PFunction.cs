// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： PFunction.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 16:42
// UPDATE TIME ： 2018-08-20 11:01
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

#region 命名空间

using System.Runtime.Serialization;

#endregion

namespace Apex.Player.Protocal
{
    public enum PFunction
    {
        //config
        [EnumMember] GetServerState,
        [EnumMember] StartServer,
        [EnumMember] StopServer,

        //视频
        [EnumMember] GetVideoList,
        [EnumMember] GetVideoConfig,
        [EnumMember] SetVideoConfig,
        [EnumMember] PlayVideo,
        [EnumMember] StopVideo,
        [EnumMember] NextVideo,
        [EnumMember] PreVideo,

        //图片
        [EnumMember] GetPicList,
        [EnumMember] GetPicConfig,
        [EnumMember] SetPicConfig,
        [EnumMember] PlayPic,
        [EnumMember] StopPic,
        [EnumMember] PrePic,
        [EnumMember] NextPic,
    }
}