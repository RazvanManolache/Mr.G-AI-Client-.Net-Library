using MrG.AI.Client.Data;
using MrG.AI.Client.Data.Action;
using MrG.AI.Client.Data.Socket;
using MrG.AI.Client.Database;
using MrG.AI.Client.Database.Data;
using MrG.AI.Client.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MrG.AI.Client.Helpers
{
    public static class SocketHelper
    {
        private static ClientWebSocket _webSocket = null;
        private static CancellationTokenSource _cancellationTokenSource;
        private static bool _isConnected;

        public static event Action<Server> OnDisconnectedEvent;
        public static event Action<Server> OnLogoutEvent;
        public static event Action<Server> OnConnectedEvent;

        public static event Action<List<ActionApi>> OnActionsUpdated;

        public static event Action<string> OnMessageReceivedEvent;

        

        private static Server connectedServer = null;
        static SocketHelper()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (Server.Instance != null)
                    {
                        if(Server.Instance != connectedServer)
                        {
                            if(connectedServer != null)
                                await Disconnect();
                            connectedServer = Server.Instance;
                            await Connect();
                        }
                       
                    }
                    else
                    {
                        await Disconnect();
                    }
                    await Task.Delay(100);
                }
            });
            OnConnectedEvent += OnConnected;
            OnMessageReceivedEvent += OnMessageReceived;

        }
       
        private static async void OnMessageReceived(string message)
        {
            var messageObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketMessage>(message);
            if (messageObject == null) return; 
            if(messageObject.Type==null) return; 
            
          
            switch (messageObject.Type)
            {
                case SocketMessageEnum.ActionDefinitions:
                    var messageData = messageObject.Data;
                    var actions = new List<ActionApi>();
                    if (messageData != null)
                    {
                        if (messageData.Actions != null)
                        {
                            actions =  messageData.Actions;
                        }                        
                    }
                   
                    connectedServer?.SetActions(actions);
                    OnActionsUpdated?.Invoke(actions);
                    break;
                case SocketMessageEnum.ApiUpdated:
                    GetActions();
                    break;
                case SocketMessageEnum.ExecutionFinished:
                    {
                        var data = messageObject.Data;
                        if (messageObject.Data == null || messageObject.Data.RunUuid==null) return;
                        if (DatabaseHelper.Instance != null)
                        {
                            var runItem = await DatabaseHelper.Instance.Get<RunItem>(messageObject.Data.RunUuid);

                            if (runItem != null)
                            {
                                runItem.QueueStatus = "Finished";
                                await DatabaseHelper.Instance.Save(runItem);
                                var getResult = new JObject();
                                getResult["type"] = "getResult";
                                getResult["uuid"] = messageObject.Data.RunUuid;
                                SendMessage(getResult.ToString());
                            }
                        }
                    }
                    break;
                //case SocketMessageEnum.GetStatusResponse:
                //    {
                //        var data = messageObject["uuid"];
                //        if (data == null) return;
                //        var uuid = data.ToString();
                //        if (MrGDatabase.Database != null)
                //        {
                //            var request = await MrGDatabase.Database.Get<Request>(uuid);
                //            if (request != null)
                //            {
                //                var status = messageObject.GetValue("status");
                //                if (status == null || status.ToString() != "unknown")
                //                    await MrGDatabase.Database.UpdateStatus(uuid, RequestStatus.Failed);
                //                else
                //                {
                //                    var statusEnum = (RequestStatus)System.Enum.Parse(typeof(RequestStatus), status.ToString());
                //                    await MrGDatabase.Database.UpdateStatus(uuid, statusEnum);
                //                }
                //            }
                //        }
                //    }
                //    break;
                case SocketMessageEnum.ExecuteApiQueued:
                    {
                        if(messageObject.Data == null || messageObject.Data.RequestId==null) return;
                        if (DatabaseHelper.Instance != null)
                        {
                            var request = await DatabaseHelper.Instance.Get<Request>(messageObject.Data.RequestId);
                            if (request != null)
                            {
                                request.RequestStatus = RequestStatus.Queued;
                                if(messageObject.Data.Result != null)
                                {
                                    var run_uuid = messageObject.Data.Result.RunUuids;
                                    if(run_uuid!=null)
                                    {
                                        foreach (var ruuid in run_uuid)
                                        {
                                            RunItem runItem = new RunItem();
                                            runItem.Uuid = ruuid;
                                            runItem.QueueStatus = "Queued";
                                            runItem.RequestUuid = request.Uuid;
                                            await DatabaseHelper.Instance.Save(runItem);
                                        }
                                    }
                                }
                                await DatabaseHelper.Instance.Save(request);
                            }
                        }
                        
                    }
                    break;
                case SocketMessageEnum.ResultPushed:
                    {
                        var data = messageObject.Data;
                        if (messageObject.Data == null || messageObject.Data.RunUuid == null) return;

                        if (DatabaseHelper.Instance != null)
                        {
                            var runItem = await DatabaseHelper.Instance.Get<RunItem>(messageObject.Data.RunUuid);
                            if (runItem == null)
                            {
                                return;
                            }
                            if (messageObject.Data.Outputs != null)
                            {
                                foreach (var output in messageObject.Data.Outputs)
                                {
                                    var outputItem = new OutputItem(output, messageObject.Data.RunUuid);
                                    await DatabaseHelper.Instance.Save(outputItem);
                                }
                            }
                            runItem.QueueStatus = "Finished";
                            await DatabaseHelper.Instance.Save(runItem);
                            var runItems = DatabaseHelper.Instance.GetAll<RunItem>(a => a.RequestUuid == runItem.RequestUuid);
                            var allFinished = true;
                            foreach (var item in runItems.Result)
                            {
                                if (item.QueueStatus != "Finished")
                                {
                                    allFinished = false;
                                }
                            }
                            if (allFinished)
                            {
                                var request = await DatabaseHelper.Instance.Get<Request>(runItem.RequestUuid);
                                if (request != null)
                                {
                                    request.RequestStatus = RequestStatus.Finished;
                                    request.Seen = false;
                                    await DatabaseHelper.Instance.Save(request);
                                }
                            }

                        }
                    }
                    break;
                default:
                    //write to output type
                    Debug.WriteLine(messageObject.InternalType);
                    Debug.WriteLine(message);
                    break;
            }





         



            }


        public static void ExecuteApi(ActionApi action, object data)
        {
            if (_isConnected)
            {
                var jd = JToken.FromObject(data);
                var request = new Request();
                request.ActionUuid = action.Uuid;
                request.ActionName = action.Name;
                request.Value = jd.ToString() ;

                var message = new JObject();
                message["requestId"] = request.Uuid;
                message["type"] = "executeApi";
                message["api"] = action.Uuid;
                message["params"] = jd;
                SendMessage(message.ToString());
                if(DatabaseHelper.Instance != null)
                    DatabaseHelper.Instance.Save(request);
            }
        }

        public static void GetActions()
        {
            if (_isConnected)
            {
                var message = new JObject();
                message["type"] = "getActions";
                SendMessage(message.ToString());
            }
        }


        private static void OnConnected(Server server)
        {
            GetActions();
        }

        public static async Task Connect()
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            if (connectedServer != null)
            {
                var url = connectedServer.GetSocketUrl();
                if(url==null)
                {
                    OnLogoutEvent?.Invoke(connectedServer);
                    return;
                }
                if (connectedServer.ServerType == ServerType.Online)
                {
                    var token = connectedServer.Token;
                    _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                    
                }
                if (connectedServer.ServerType == ServerType.Local)
                {
                  
                }
                await _webSocket.ConnectAsync(new Uri(url), _cancellationTokenSource.Token);
                _isConnected = true;
                OnConnectedEvent?.Invoke(connectedServer);
                await ReceiveMessages();
            }
            

           
           
            
        }

        public static async Task Disconnect()
        {
            if (_isConnected)
            {
                if(_cancellationTokenSource != null)
                    _cancellationTokenSource.Cancel();
                if(_webSocket != null)
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client", CancellationToken.None);
                _isConnected = false;
                OnDisconnectedEvent?.Invoke(connectedServer);
            }
        }

        public static async void SendMessage(string message)
        {
            if (_isConnected)
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                if (_webSocket != null)
                    await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private static async Task ReceiveMessages()
        {
            var bigMessage= "";
            while (_isConnected)
            {
                if (_webSocket != null)
                {
                    var buffer = new byte[10*1024 * 1024];
                    var arrSeg = new ArraySegment<byte>(buffer);
                    var result = await _webSocket.ReceiveAsync(arrSeg, CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        bigMessage += message;
                        if(result.EndOfMessage)
                        {
                            OnMessageReceivedEvent?.Invoke(bigMessage);
                            bigMessage = "";
                        }
                        
                    }

                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await Disconnect();
                    }
                }
            }
        }
    }
}
