// 文件说明
// SOLUTION    ： 大屏播放程序
// PROJECT     ： Apex.Player
// FILENAME    ： Form1.cs
// AUTHOR      ： 陈奇
// CREATE TIME ： 2018-08-19 16:30
// UPDATE TIME ： 2018-08-19 16:54
// COPYRIGHT   ： 版权所有 (C) 安徽斯玛特物联网科技有限公司 http://www.smartiot.cc/ 2011~2018

#region 命名空间

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apex.Player.PFile.Pic;
using Apex.Player.Protocal;
using NewLife.Log;
using NewLife.Net;
using NewLife.Serialization;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;

#endregion

namespace Apex.Player
{
    public partial class Form1 : Form
    {
        private WebSocketServer _wsServer;

        private string _pfilePath;

        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.Dock = DockStyle.None;
            this.pictureBox1.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = () =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                //空值处理
                setting.NullValueHandling = NullValueHandling.Ignore;

                return setting;
            };

            _pfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PFile");

            Task.Run(() =>
            {
                StartServer();
            });

        }

        private void StartServer()
        {
            if (_wsServer == null)
            {
                var wsconfig = new ServerConfig
                {
                    Ip = "0.0.0.0",
                    Port = 50000,
                    SyncSend = false,
                    Name = "ws",
                    TextEncoding = "UTF-8"
                };
                if (_wsServer == null) _wsServer = new WebSocketServer();
                var setup = _wsServer.Setup(wsconfig);
                if (setup)
                {
                    _wsServer.NewMessageReceived += AppServer_NewMessageReceived;
                    var start = _wsServer.Start();
                    if (start) txtLog.Append($"ws server 已启动").Append(Environment.NewLine);
                    else
                        txtLog.Append($"ws server 启动失败").Append(Environment.NewLine);
                }
                else
                {
                    txtLog.Append("ws setup 失败").Append(Environment.NewLine);
                }
            }
        }

        private void StopServer()
        {
            if (_wsServer != null && _wsServer.State == ServerState.Running)
            {
                _wsServer.Stop();
                txtLog.Append($"ws server 已停止");
            }
        }

        private void AppServer_NewMessageReceived(WebSocketSession session, string str)
        {
            if (!str.StartsWith("{") || !str.EndsWith("}"))
            {
                txtLog.Append(str).Append(Environment.NewLine);
                Send(new ResponseData(StatusCode.ArgumentError, "不是json格式数据包"), session);
                return;
            }

            var model = str.ToJsonEntity<AppModel>();
            if (model == null)
            {
                txtLog.Append($"消息不符合协议：{str}");
                return;
            }

            switch (model.PFunction)
            {
                //config
                case PFunction.GetServerState:
                    {
                        if (_wsServer == null) Send(new ResponseData(StatusCode.WsServerIsNull), session);
                        var res = new ResponseData(_wsServer.State == ServerState.Running ? StatusCode.WsServerStarted : StatusCode.WsServerNotStart, _wsServer.State.GetDescription());
                        Send(res, session);
                    }
                    break;
                case PFunction.StartServer:
                    StartServer();
                    break;
                case PFunction.StopServer:
                    StopServer();
                    break;
                //视频
                case PFunction.GetVideoList:
                    break;
                case PFunction.SetVideoConfig:
                    break;
                case PFunction.GetVideoConfig:
                    break;
                case PFunction.PlayVideo:
                    break;
                case PFunction.StopVideo:
                    break;
                case PFunction.NextVideo:
                    break;
                case PFunction.PreVideo:
                    break;

                //图片
                case PFunction.GetPicList:
                    GetPicList(model, session);
                    break;
                case PFunction.GetPicConfig:
                    GetPicConfig(model, session);
                    break;
                case PFunction.SetPicConfig:
                    SetPicConfig(model, session); break;
                case PFunction.PlayPic:
                    PlayPic(model);
                    break;
                case PFunction.StopPic:
                    StopPic(model);
                    break;
                case PFunction.PrePic:
                    PrePic(model);
                    break;
                case PFunction.NextPic:
                    NextPic(model);
                    break;

                //
                default:
                    Send(new ResponseData(StatusCode.CanNotProcessRequestData), session);
                    break;
            }
        }

        #region 图片
        /// <summary>
        /// 获取图片文件
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FileInfo> GetPicFiles()
        {
            var path = Path.Combine(_pfilePath, "pic");
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles().Where(f => PicConfig.Current.Ext.Contains(f.Extension));
            return files;
        }
        /// <summary>
        /// 获取图片播放列表
        /// </summary>
        /// <param name="model"></param>
        private void GetPicList(AppModel model, WebSocketSession session)
        {
            var files = GetPicFiles();
            var data = files.Select(f => new
            {
                dir = f.DirectoryName,
                name = f.Name,
                ext = f.Extension,
                length = f.Length,
            });
            var res = new ResponseData();
            res.Data = new
            {
                function = (int)PFunction.GetPicList,
                data = data
            };
            Send(res, session);
        }

        private void GetPicConfig(AppModel model, WebSocketSession session)
        {
            var res = new ResponseData();
            res.Data = new
            {
                function = (int)PFunction.GetPicConfig,
                data = new
                {
                    interval = PicConfig.Current.Interval,
                    ext = PicConfig.Current.Ext
                }
            };
            Send(res, session);
        }

        private void SetPicConfig(AppModel model, WebSocketSession session)
        {
            PicConfig.Current.Interval = Convert.ToInt32("0" + model.Data["interval"]);
            PicConfig.Current.Ext = model.Data["ext"];
            PicConfig.Current.SaveAsync();
            //定时器重新启动
            InitPicPlayTimer();
            Send(new ResponseData(), session);
        }

        /// <summary>
        /// 当前图片的索引。当值为-1时，表示当前非图片播放模式
        /// </summary>
        private int picIndex = 0;
        /// <summary>
        /// 图片播放定时器
        /// </summary>
        private System.Threading.Timer picPlayTimer;
        /// <summary>
        /// 初始化图片播放定时器
        /// </summary>
        private void InitPicPlayTimer()
        {
            if(picIndex ==-1)return;
            var config = PicConfig.Current;
            var files = GetPicFiles();
            picPlayTimer = new System.Threading.Timer(obj =>
           {
               this.pictureBox1.Image = Image.FromFile(files.ElementAt(picIndex).FullName);
               //修改下一次播放文件的索引
               picIndex++;
               if (picIndex == files.Count())
                   picIndex = 0;
           }, null, 100, config.Interval * 1000);
        }
        /// <summary>
        /// 播放图片
        /// </summary>
        /// <param name="model"></param>
        private void PlayPic(AppModel model)
        {
            picIndex = 0;
            this.Invoke(new Action(() =>
            {
                this.pictureBox1.Dock = DockStyle.Fill;
                this.pictureBox1.Visible = true;
            }));

            picPlayTimer?.Dispose();
            InitPicPlayTimer();
        }
        /// <summary>
        /// 停止播放图片
        /// </summary>
        /// <param name="model"></param>
        private void StopPic(AppModel model)
        {
            picIndex = -1;
            this.Invoke(new Action(() =>
            {
                this.pictureBox1.Dock = DockStyle.None;
                this.pictureBox1.Visible = false;
            }));

            picPlayTimer?.Dispose();

        }
        /// <summary>
        /// 上一张图片
        /// </summary>
        /// <param name="model"></param>
        private void PrePic(AppModel model)
        {
            var files = GetPicFiles();
            picIndex--;
            if (picIndex == -1)
                picIndex = files.Count() - 1;
            picPlayTimer?.Dispose();
            InitPicPlayTimer();
        }
        /// <summary>
        /// 下一张图片
        /// </summary>
        /// <param name="model"></param>
        private void NextPic(AppModel model)
        {
            var files = GetPicFiles();
            picPlayTimer?.Dispose();

            picIndex++;
            if (picIndex == files.Count())
                picIndex = 0;
            InitPicPlayTimer();
        }
        #endregion


        private void Send(ResponseData res, WebSocketSession ns)
        {
            var json = res.ToString();
            ns.Send(json);
            txtLog.Append($"{ns.Connection + ""}回发：{json}").Append(Environment.NewLine);
        }
    }
}