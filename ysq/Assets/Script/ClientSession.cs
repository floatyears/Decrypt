using NtUniSdk.Unity3d;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public sealed class ClientSession : MonoBehaviour
{
	private enum ESessionStatus
	{
		ESS_None,
		ESS_Connect,
		ESS_Connecting,
		ESS_Loading,
		ESS_CreateChar,
		ESS_Gaming
	}

	private enum ESessionError
	{
		ESE_RequestConnectError,
		ESE_ConnectTimeout,
		ESE_LoadingTimeout,
		ESE_SendTimeout,
		ESE_ConnectDisconnect,
		ESE_LoadingDisconnect,
		ESE_SendDisconnect,
		ESE_GamingDisconnect
	}

	internal class HttpResponeState
	{
		public ushort responseMsgID;

		public HttpWebRequest httpWebRequest;

		public bool responseBinary;

		public byte[] userData;
	}

	internal class HttpRequestState : ClientSession.HttpResponeState
	{
		public MemoryStream stream;
	}

	public delegate void MsgHandler(MemoryStream stream);

	private const int DefaultTimeout = 15000;

	private const string boundary = "----------ThIs_Is_tHe_bouNdaRY_$";

	private const string crlf = "\r\n";

	public bool IsConnected;

	private NetTcpClient tcpClient = new NetTcpClient();

	private Queue<MemoryStream>[] msgQueue = new Queue<MemoryStream>[]
	{
		new Queue<MemoryStream>(),
		new Queue<MemoryStream>()
	};

	private object obj = new object();

	private int recvIndex;

	private Dictionary<ushort, ClientSession.MsgHandler> msgHandlers = new Dictionary<ushort, ClientSession.MsgHandler>();

	private MemoryStream sendStream = new MemoryStream(32);

	private ClientSession.ESessionStatus status;

	private float timeout;

	private float heartBeatTimer;

	private int heartBeatFrame;

	private bool showReconnect;

	private ushort serialID;

	private bool stopTimer;

	private int retryCount;

	private string serverHost = string.Empty;

	private int serverPort;

	private string accountID = string.Empty;

	private string accountKey = string.Empty;

	private string platform = string.Empty;

	private string channel = string.Empty;

	private int publisher;

	private bool newConnect;

	public bool serverShutDown;

	private int playerGender = -1;

	private string playerName = string.Empty;

	private RC4 encryptKey;

	private RC4 decryptKey;

	private Dictionary<ushort, ushort> msgPair = new Dictionary<ushort, ushort>();

	private Dictionary<ushort, bool> msgFlag = new Dictionary<ushort, bool>();

	private ushort requestMsgID;

	private ushort requestSerialID;

	private object requestMsg;

	private bool waitingSend;

	private bool hideFlag;

	public float LagTime
	{
		get;
		private set;
	}

	public int Privilege
	{
		get;
		private set;
	}

	public int WorldID
	{
		get;
		private set;
	}

	public string UQPayCbUrl
	{
		get;
		private set;
	}

	public void Connect(string host, int port, string id, string key, string pf, string ch, int pub)
	{
		if (host == string.Empty)
		{
			global::Debug.LogError(new object[]
			{
				"host is empty"
			});
			return;
		}
		if (this.status != ClientSession.ESessionStatus.ESS_None && this.serverHost == host && this.serverPort == port && this.accountID == id && this.accountKey == key && this.platform == pf && this.channel == ch)
		{
			return;
		}
		this.serverHost = host;
		this.serverPort = port;
		this.accountID = id;
		this.accountKey = key;
		this.platform = pf;
		this.channel = ch;
		this.publisher = pub;
		this.requestMsgID = 0;
		this.requestSerialID = 0;
		this.requestMsg = null;
		this.waitingSend = false;
		global::Debug.LogFormat("Request Connect, IP = {0}, Port = {1}, Account = {2}, Key = {3}, Platform = {4}, Channel = {5}", new object[]
		{
			this.serverHost,
			this.serverPort,
			this.accountID,
			this.accountKey,
			this.platform,
			this.channel
		});
		this.newConnect = true;
		this.RequestConnect();
		GameUIManager.mInstance.ShowIndicate();
	}

	public void Reconnect()
	{
		if (this.status != ClientSession.ESessionStatus.ESS_None)
		{
			global::Debug.LogErrorFormat("Reconnect error, status = {1}", new object[]
			{
				this.status
			});
			return;
		}
		global::Debug.LogFormat("Request Connect, IP = {0}, Port = {1}, Account = {2}, Key = {3}", new object[]
		{
			this.serverHost,
			this.serverPort,
			this.accountID,
			this.accountKey
		});
		if (this.requestMsgID != 0)
		{
			this.waitingSend = true;
		}
		this.RequestConnect();
		GameUIManager.mInstance.ShowIndicate();
	}

	private void RequestConnect()
	{
		this.Close();
		this.status = ClientSession.ESessionStatus.ESS_Connect;
	}

	public void Close()
	{
		this.tcpClient.Close();
	}

	public bool SendPacket(ushort msgID, object ojb)
	{
		return this.SendPacket(msgID, ojb, 0);
	}

	private bool SendPacket(ushort msgID, object ojb, ushort msgSerialID)
	{
		this.sendStream.Position = 0L;
		this.sendStream.SetLength(0L);
		this.sendStream.WriteByte((byte)(msgID & 255));
		this.sendStream.WriteByte((byte)((msgID & 65280) >> 8));
		this.sendStream.WriteByte((byte)(msgSerialID & 255));
		this.sendStream.WriteByte((byte)((msgSerialID & 65280) >> 8));
		Serializer.NonGeneric.Serialize(this.sendStream, ojb);
		if (this.encryptKey != null)
		{
			this.encryptKey.Process(this.sendStream.GetBuffer(), 0, (int)this.sendStream.Length);
		}
		return this.tcpClient.Send(this.sendStream.GetBuffer(), (uint)this.sendStream.Length);
	}

	public bool Send(ushort msgID, object ojb)
	{
		return this.Send(msgID, ojb, 0);
	}

	private bool Send(ushort msgID, object ojb, ushort msgSerialID)
	{
		bool flag = false;
		if (this.msgPair.ContainsKey(msgID))
		{
			if (this.requestMsgID != 0)
			{
				if (this.status == ClientSession.ESessionStatus.ESS_None)
				{
					global::Debug.LogFormat("Request Connect, IP = {0}, Port = {1}, Account = {2}, Key = {3}", new object[]
					{
						this.serverHost,
						this.serverPort,
						this.accountID,
						this.accountKey
					});
					this.waitingSend = true;
					this.RequestConnect();
				}
				else
				{
					global::Debug.LogErrorFormat("request MsgID = {1} error, waiting MsgID = {0} reply, ", new object[]
					{
						msgID,
						this.requestMsgID
					});
				}
				return false;
			}
			this.timeout = Time.time;
			this.requestMsgID = msgID;
			this.requestMsg = ojb;
			flag = true;
			if (msgSerialID == 0)
			{
				bool flag2 = false;
				if (this.msgFlag.TryGetValue(this.requestMsgID, out flag2) && flag2)
				{
					this.serialID += 1;
					if (this.serialID == 0)
					{
						this.serialID += 1;
					}
					msgSerialID = this.serialID;
				}
			}
			this.requestSerialID = msgSerialID;
			GameUIManager.mInstance.ShowIndicate();
		}
		if (!this.SendPacket(msgID, ojb, msgSerialID))
		{
			if (!flag)
			{
				return false;
			}
			global::Debug.LogFormat("Request Connect, IP = {0}, Port = {1}, Account = {2}, Key = {3}", new object[]
			{
				this.serverHost,
				this.serverPort,
				this.accountID,
				this.accountKey
			});
			this.waitingSend = true;
			this.RequestConnect();
		}
		return true;
	}

	public void OnConnect(int result)
	{
		MemoryStream memoryStream = new MemoryStream(16);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(0);
		binaryWriter.Write(1501);
		if (result == 0)
		{
			binaryWriter.Write(0);
		}
		else
		{
			binaryWriter.Write(1);
		}
		memoryStream.Position = 0L;
		object obj = this.obj;
		lock (obj)
		{
			this.msgQueue[this.recvIndex].Enqueue(memoryStream);
		}
	}

	public void OnDataRecevie(byte[] buffer, uint offset, uint size)
	{
		MemoryStream memoryStream = new MemoryStream((int)size);
		memoryStream.WriteByte(1);
		memoryStream.Write(buffer, (int)offset, (int)size);
		memoryStream.Position = 0L;
		object obj = this.obj;
		lock (obj)
		{
			this.msgQueue[this.recvIndex].Enqueue(memoryStream);
		}
	}

	public void OnDisconnect()
	{
		MemoryStream memoryStream = new MemoryStream(16);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(0);
		binaryWriter.Write(1502);
		memoryStream.Position = 0L;
		object obj = this.obj;
		lock (obj)
		{
			this.msgQueue[this.recvIndex].Enqueue(memoryStream);
		}
	}

	public bool UploadVoice(string url, byte[] buffer, ushort msgID)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] bytes = Encoding.ASCII.GetBytes("------------ThIs_Is_tHe_bouNdaRY_$\r\n");
		memoryStream.Write(bytes, 0, bytes.Length);
		bytes = Encoding.ASCII.GetBytes("Content-Type:audio/amr\r\n\r\n");
		memoryStream.Write(bytes, 0, bytes.Length);
		memoryStream.Write(buffer, 0, buffer.Length);
		bytes = Encoding.ASCII.GetBytes("\r\n------------ThIs_Is_tHe_bouNdaRY_$\r\n\r\n");
		memoryStream.Write(bytes, 0, bytes.Length);
		return this.HttpPost(url, memoryStream.GetBuffer(), (uint)memoryStream.Length, msgID, false);
	}

	public bool HttpPost(string url, byte[] buffer, uint size, ushort msgID, bool binary = false)
	{
		try
		{
			ClientSession.HttpRequestState httpRequestState = new ClientSession.HttpRequestState();
			httpRequestState.httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpRequestState.responseMsgID = msgID;
			httpRequestState.responseBinary = binary;
			httpRequestState.userData = null;
			httpRequestState.httpWebRequest.ContentType = "multipart/form-data; boundary=----------ThIs_Is_tHe_bouNdaRY_$";
			httpRequestState.httpWebRequest.Method = "POST";
			httpRequestState.httpWebRequest.UserAgent = "ma32";
			httpRequestState.httpWebRequest.Accept = "*/*";
			if (buffer != null && size != 0u)
			{
				httpRequestState.stream = new MemoryStream(buffer, 0, (int)size);
			}
			IAsyncResult asyncResult = httpRequestState.httpWebRequest.BeginGetRequestStream(new AsyncCallback(this.GetRequestStreamCallBack), httpRequestState);
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.TimeoutCallback), httpRequestState.httpWebRequest, 15000, true);
		}
		catch (WebException ex)
		{
			global::Debug.LogErrorFormat("HttpPost Exception raised! \nMessage:{0} \nStatus:{1} !", new object[]
			{
				ex.Message,
				ex.Status
			});
			bool result = false;
			return result;
		}
		catch (Exception ex2)
		{
			global::Debug.LogException("HttpPost Exception raised!", ex2);
			bool result = false;
			return result;
		}
		return true;
	}

	private void GetRequestStreamCallBack(IAsyncResult ar)
	{
		ClientSession.HttpRequestState httpRequestState = (ClientSession.HttpRequestState)ar.AsyncState;
		ClientSession.HttpResponeState httpResponeState = new ClientSession.HttpResponeState();
		httpResponeState.httpWebRequest = httpRequestState.httpWebRequest;
		httpResponeState.responseMsgID = httpRequestState.responseMsgID;
		try
		{
			Stream stream = httpRequestState.httpWebRequest.EndGetRequestStream(ar);
			if (httpRequestState.stream != null)
			{
				stream.Write(httpRequestState.stream.ToArray(), 0, (int)httpRequestState.stream.Length);
			}
			stream.Close();
			IAsyncResult asyncResult = httpRequestState.httpWebRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallBack), httpResponeState);
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.TimeoutCallback), httpRequestState.httpWebRequest, 15000, true);
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("GetRequestStreamCallBack Exception, msg = {0}", new object[]
			{
				ex.Message
			});
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(0);
			binaryWriter.Write(httpRequestState.responseMsgID);
			binaryWriter.Write(503);
			object obj = this.obj;
			lock (obj)
			{
				memoryStream.Position = 0L;
				this.msgQueue[this.recvIndex].Enqueue(memoryStream);
			}
		}
	}

	private void GetResponseCallBack(IAsyncResult ar)
	{
		ClientSession.HttpResponeState httpResponeState = (ClientSession.HttpResponeState)ar.AsyncState;
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(0);
		binaryWriter.Write(httpResponeState.responseMsgID);
		if (httpResponeState.userData != null)
		{
			binaryWriter.Write(httpResponeState.userData.Length);
			binaryWriter.Write(httpResponeState.userData, 0, httpResponeState.userData.Length);
		}
		try
		{
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpResponeState.httpWebRequest.EndGetResponse(ar);
			Stream responseStream = httpWebResponse.GetResponseStream();
			binaryWriter.Write((int)httpWebResponse.StatusCode);
			if (httpResponeState.responseBinary)
			{
				BinaryReader binaryReader = new BinaryReader(responseStream);
				binaryWriter.Write(binaryReader.ReadBytes((int)httpWebResponse.ContentLength));
				binaryReader.Close();
			}
			else
			{
				StreamReader streamReader = new StreamReader(responseStream);
				string value = streamReader.ReadToEnd();
				binaryWriter.Write(value);
				streamReader.Close();
			}
			responseStream.Close();
			httpWebResponse.Close();
		}
		catch (WebException ex)
		{
			global::Debug.LogErrorFormat("GetResponseCallBack Exception raised! \nMessage:{0} \nStatus:{1} !", new object[]
			{
				ex.Message,
				ex.Status
			});
			global::Debug.LogException("Exception", ex);
			binaryWriter.Write(404);
		}
		catch (Exception ex2)
		{
			global::Debug.LogException("HttpGet Exception raised!", ex2);
			binaryWriter.Write(417);
		}
		finally
		{
			object obj = this.obj;
			lock (obj)
			{
				memoryStream.Position = 0L;
				this.msgQueue[this.recvIndex].Enqueue(memoryStream);
			}
		}
	}

	public bool HttpGet(string url, ushort msgID, bool binary = false, byte[] customData = null)
	{
		try
		{
			ClientSession.HttpResponeState httpResponeState = new ClientSession.HttpResponeState();
			httpResponeState.httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpResponeState.responseMsgID = msgID;
			httpResponeState.responseBinary = binary;
			httpResponeState.userData = customData;
			httpResponeState.httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpResponeState.httpWebRequest.Method = "GET";
			httpResponeState.httpWebRequest.UserAgent = "ma32";
			IAsyncResult asyncResult = httpResponeState.httpWebRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallBack), httpResponeState);
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.TimeoutCallback), httpResponeState.httpWebRequest, 15000, true);
		}
		catch (WebException ex)
		{
			global::Debug.LogErrorFormat("HttpGet Exception raised! \nMessage:{0} \nStatus:{1} !", new object[]
			{
				ex.Message,
				ex.Status
			});
			bool result = false;
			return result;
		}
		catch (Exception ex2)
		{
			global::Debug.LogException("HttpGet Exception raised!", ex2);
			bool result = false;
			return result;
		}
		return true;
	}

	private void TimeoutCallback(object state, bool timedOut)
	{
		if (timedOut)
		{
			HttpWebRequest httpWebRequest = state as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.Abort();
			}
		}
	}

	public void Register(ushort msgID, ClientSession.MsgHandler handler)
	{
		if (this.msgHandlers.ContainsKey(msgID))
		{
			Dictionary<ushort, ClientSession.MsgHandler> dictionary;
			Dictionary<ushort, ClientSession.MsgHandler> expr_17 = dictionary = this.msgHandlers;
			ClientSession.MsgHandler a = dictionary[msgID];
			expr_17[msgID] = (ClientSession.MsgHandler)Delegate.Combine(a, handler);
		}
		else
		{
			this.msgHandlers[msgID] = handler;
		}
	}

	public void Unregister(ushort msgID, ClientSession.MsgHandler handler)
	{
		if (this.msgHandlers.ContainsKey(msgID))
		{
			Dictionary<ushort, ClientSession.MsgHandler> dictionary;
			Dictionary<ushort, ClientSession.MsgHandler> expr_17 = dictionary = this.msgHandlers;
			ClientSession.MsgHandler source = dictionary[msgID];
			expr_17[msgID] = (ClientSession.MsgHandler)Delegate.Remove(source, handler);
		}
	}

	public void RegisterMsgPair(ushort requestMsgID, ushort replyMsgID, bool flag)
	{
		this.msgPair[requestMsgID] = replyMsgID;
		this.msgFlag[requestMsgID] = flag;
	}

	private void Awake()
	{
		NetTcpClient expr_06 = this.tcpClient;
		expr_06.OnConnectEvent = (NetTcpClient.OnConnectCallback)Delegate.Combine(expr_06.OnConnectEvent, new NetTcpClient.OnConnectCallback(this.OnConnect));
		NetTcpClient expr_2D = this.tcpClient;
		expr_2D.OnDataRecevieEvent = (NetTcpClient.OnDataRecevieCallback)Delegate.Combine(expr_2D.OnDataRecevieEvent, new NetTcpClient.OnDataRecevieCallback(this.OnDataRecevie));
		NetTcpClient expr_54 = this.tcpClient;
		expr_54.OnDisconnectEvent = (NetTcpClient.OnDisconnectCallback)Delegate.Combine(expr_54.OnDisconnectEvent, new NetTcpClient.OnDisconnectCallback(this.OnDisconnect));
		this.Register(1501, new ClientSession.MsgHandler(this.HandleConnect));
		this.Register(1502, new ClientSession.MsgHandler(this.HandleDisconnect));
		this.Register(102, new ClientSession.MsgHandler(this.HandleEnterGame));
		this.Register(108, new ClientSession.MsgHandler(this.HandleHeartBeat));
		this.Register(109, new ClientSession.MsgHandler(this.HandleSerialIDError));
		this.Register(110, new ClientSession.MsgHandler(this.HandleKickPlayer));
		this.Register(1499, new ClientSession.MsgHandler(this.HandleGMCommand));
		this.RegisterMsgPair(193, 194, true);
		this.RegisterMsgPair(195, 196, true);
		this.RegisterMsgPair(197, 198, true);
		this.RegisterMsgPair(199, 200, true);
		this.RegisterMsgPair(201, 202, true);
		this.RegisterMsgPair(203, 204, true);
		this.RegisterMsgPair(206, 207, true);
		this.RegisterMsgPair(209, 210, true);
		this.RegisterMsgPair(212, 213, true);
		this.RegisterMsgPair(214, 215, true);
		this.RegisterMsgPair(216, 217, true);
		this.RegisterMsgPair(220, 221, true);
		this.RegisterMsgPair(222, 223, true);
		this.RegisterMsgPair(224, 225, true);
		this.RegisterMsgPair(226, 227, true);
		this.RegisterMsgPair(228, 229, true);
		this.RegisterMsgPair(232, 233, true);
		this.RegisterMsgPair(234, 235, true);
		this.RegisterMsgPair(236, 237, true);
		this.RegisterMsgPair(238, 239, true);
		this.RegisterMsgPair(240, 241, true);
		this.RegisterMsgPair(242, 243, true);
		this.RegisterMsgPair(286, 287, false);
		this.RegisterMsgPair(246, 247, true);
		this.RegisterMsgPair(248, 249, true);
		this.RegisterMsgPair(255, 256, true);
		this.RegisterMsgPair(258, 259, true);
		this.RegisterMsgPair(402, 403, true);
		this.RegisterMsgPair(404, 405, true);
		this.RegisterMsgPair(406, 407, true);
		this.RegisterMsgPair(417, 418, true);
		this.RegisterMsgPair(419, 420, true);
		this.RegisterMsgPair(421, 422, true);
		this.RegisterMsgPair(423, 424, true);
		this.RegisterMsgPair(425, 426, true);
		this.RegisterMsgPair(502, 503, true);
		this.RegisterMsgPair(504, 505, true);
		this.RegisterMsgPair(506, 507, true);
		this.RegisterMsgPair(542, 543, true);
		this.RegisterMsgPair(508, 509, true);
		this.RegisterMsgPair(510, 511, true);
		this.RegisterMsgPair(512, 513, false);
		this.RegisterMsgPair(514, 515, true);
		this.RegisterMsgPair(516, 517, true);
		this.RegisterMsgPair(520, 521, true);
		this.RegisterMsgPair(522, 523, true);
		this.RegisterMsgPair(524, 525, true);
		this.RegisterMsgPair(526, 527, true);
		this.RegisterMsgPair(536, 537, true);
		this.RegisterMsgPair(538, 539, true);
		this.RegisterMsgPair(540, 541, true);
		this.RegisterMsgPair(544, 545, true);
		this.RegisterMsgPair(600, 601, true);
		this.RegisterMsgPair(602, 603, false);
		this.RegisterMsgPair(604, 605, true);
		this.RegisterMsgPair(606, 607, false);
		this.RegisterMsgPair(609, 610, true);
		this.RegisterMsgPair(612, 613, true);
		this.RegisterMsgPair(614, 615, false);
		this.RegisterMsgPair(645, 646, false);
		this.RegisterMsgPair(622, 623, true);
		this.RegisterMsgPair(624, 625, true);
		this.RegisterMsgPair(626, 627, true);
		this.RegisterMsgPair(638, 639, true);
		this.RegisterMsgPair(628, 629, true);
		this.RegisterMsgPair(630, 631, false);
		this.RegisterMsgPair(632, 633, true);
		this.RegisterMsgPair(634, 635, true);
		this.RegisterMsgPair(636, 637, true);
		this.RegisterMsgPair(641, 642, false);
		this.RegisterMsgPair(643, 644, true);
		this.RegisterMsgPair(700, 701, false);
		this.RegisterMsgPair(702, 703, true);
		this.RegisterMsgPair(704, 705, false);
		this.RegisterMsgPair(708, 709, false);
		this.RegisterMsgPair(710, 711, true);
		this.RegisterMsgPair(712, 713, false);
		this.RegisterMsgPair(714, 715, true);
		this.RegisterMsgPair(724, 725, true);
		this.RegisterMsgPair(719, 720, true);
		this.RegisterMsgPair(722, 723, true);
		this.RegisterMsgPair(731, 732, true);
		this.RegisterMsgPair(733, 734, true);
		this.RegisterMsgPair(735, 736, true);
		this.RegisterMsgPair(737, 738, true);
		this.RegisterMsgPair(739, 740, true);
		this.RegisterMsgPair(727, 728, false);
		this.RegisterMsgPair(751, 752, false);
		this.RegisterMsgPair(753, 754, true);
		this.RegisterMsgPair(759, 760, true);
		this.RegisterMsgPair(764, 765, true);
		this.RegisterMsgPair(768, 769, true);
		this.RegisterMsgPair(771, 772, true);
		this.RegisterMsgPair(773, 774, false);
		this.RegisterMsgPair(775, 776, true);
		this.RegisterMsgPair(777, 778, true);
		this.RegisterMsgPair(781, 782, false);
		this.RegisterMsgPair(783, 784, true);
		this.RegisterMsgPair(785, 786, true);
		this.RegisterMsgPair(801, 802, false);
		this.RegisterMsgPair(803, 804, true);
		this.RegisterMsgPair(806, 807, false);
		this.RegisterMsgPair(808, 809, false);
		this.RegisterMsgPair(810, 811, false);
		this.RegisterMsgPair(824, 825, true);
		this.RegisterMsgPair(812, 813, true);
		this.RegisterMsgPair(814, 815, false);
		this.RegisterMsgPair(816, 817, true);
		this.RegisterMsgPair(818, 819, false);
		this.RegisterMsgPair(820, 821, false);
		this.RegisterMsgPair(822, 823, true);
		this.RegisterMsgPair(826, 827, true);
		this.RegisterMsgPair(901, 902, false);
		this.RegisterMsgPair(903, 904, true);
		this.RegisterMsgPair(905, 906, true);
		this.RegisterMsgPair(907, 908, true);
		this.RegisterMsgPair(909, 910, false);
		this.RegisterMsgPair(911, 912, true);
		this.RegisterMsgPair(913, 914, false);
		this.RegisterMsgPair(915, 916, true);
		this.RegisterMsgPair(917, 918, true);
		this.RegisterMsgPair(919, 920, true);
		this.RegisterMsgPair(921, 922, false);
		this.RegisterMsgPair(923, 924, true);
		this.RegisterMsgPair(925, 926, true);
		this.RegisterMsgPair(930, 931, true);
		this.RegisterMsgPair(934, 935, false);
		this.RegisterMsgPair(948, 949, true);
		this.RegisterMsgPair(950, 951, true);
		this.RegisterMsgPair(952, 953, true);
		this.RegisterMsgPair(954, 955, true);
		this.RegisterMsgPair(958, 959, false);
		this.RegisterMsgPair(960, 961, false);
		this.RegisterMsgPair(962, 963, true);
		this.RegisterMsgPair(967, 968, true);
		this.RegisterMsgPair(946, 947, true);
		this.RegisterMsgPair(944, 945, true);
		this.RegisterMsgPair(940, 941, true);
		this.RegisterMsgPair(942, 943, true);
		this.RegisterMsgPair(956, 957, false);
		this.RegisterMsgPair(975, 976, false);
		this.RegisterMsgPair(977, 978, false);
		this.RegisterMsgPair(979, 980, true);
		this.RegisterMsgPair(981, 982, false);
		this.RegisterMsgPair(983, 984, true);
		this.RegisterMsgPair(987, 988, false);
		this.RegisterMsgPair(989, 990, true);
		this.RegisterMsgPair(991, 992, true);
		this.RegisterMsgPair(997, 998, true);
		this.RegisterMsgPair(981, 982, true);
		this.RegisterMsgPair(1003, 1004, true);
		this.RegisterMsgPair(1001, 1002, true);
		this.RegisterMsgPair(999, 1000, true);
		this.RegisterMsgPair(1005, 1006, true);
		this.RegisterMsgPair(1015, 1016, true);
		this.RegisterMsgPair(1017, 1018, true);
		this.RegisterMsgPair(986, 995, true);
		this.RegisterMsgPair(262, 263, false);
		this.RegisterMsgPair(264, 265, true);
		this.RegisterMsgPair(266, 267, true);
		this.RegisterMsgPair(268, 269, true);
		this.RegisterMsgPair(270, 271, true);
		this.RegisterMsgPair(272, 273, true);
		this.RegisterMsgPair(274, 275, true);
		this.RegisterMsgPair(276, 277, true);
		this.RegisterMsgPair(278, 279, true);
		this.RegisterMsgPair(292, 293, true);
		this.RegisterMsgPair(1494, 1495, false);
		this.RegisterMsgPair(1496, 1497, false);
		this.RegisterMsgPair(284, 285, false);
		this.RegisterMsgPair(290, 291, false);
		this.RegisterMsgPair(294, 295, false);
		this.RegisterMsgPair(288, 289, true);
		this.RegisterMsgPair(394, 395, true);
		this.RegisterMsgPair(396, 397, true);
		this.RegisterMsgPair(398, 399, true);
		this.RegisterMsgPair(745, 746, false);
		this.RegisterMsgPair(743, 744, false);
		this.RegisterMsgPair(747, 748, false);
		this.RegisterMsgPair(741, 742, true);
		this.RegisterMsgPair(755, 756, true);
		this.RegisterMsgPair(534, 535, true);
		this.RegisterMsgPair(201, 202, true);
		this.RegisterMsgPair(413, 414, true);
		this.RegisterMsgPair(530, 531, true);
		this.RegisterMsgPair(415, 416, true);
		this.RegisterMsgPair(532, 533, true);
		this.RegisterMsgPair(932, 933, true);
		this.RegisterMsgPair(647, 648, true);
		this.RegisterMsgPair(650, 651, true);
		this.RegisterMsgPair(1498, 1499, true);
		this.RegisterMsgPair(298, 299, false);
		this.RegisterMsgPair(300, 301, true);
		this.RegisterMsgPair(302, 303, true);
		this.RegisterMsgPair(307, 308, true);
		this.RegisterMsgPair(309, 310, true);
		this.RegisterMsgPair(311, 312, true);
		this.RegisterMsgPair(313, 314, true);
		this.RegisterMsgPair(315, 316, true);
		this.RegisterMsgPair(317, 318, true);
		this.RegisterMsgPair(319, 320, true);
		this.RegisterMsgPair(321, 322, true);
		this.RegisterMsgPair(324, 325, false);
		this.RegisterMsgPair(326, 327, true);
		this.RegisterMsgPair(328, 329, true);
		this.RegisterMsgPair(1020, 1021, false);
		this.RegisterMsgPair(1022, 1023, false);
		this.RegisterMsgPair(1024, 1025, true);
		this.RegisterMsgPair(1026, 1027, true);
		this.RegisterMsgPair(1041, 1042, true);
		this.RegisterMsgPair(1028, 1029, false);
		this.RegisterMsgPair(1032, 1033, true);
		this.RegisterMsgPair(1030, 1031, true);
		this.RegisterMsgPair(1035, 1036, false);
		this.RegisterMsgPair(1039, 1040, false);
		this.RegisterMsgPair(1037, 1038, false);
		this.RegisterMsgPair(969, 970, false);
		this.RegisterMsgPair(1060, 1061, true);
		this.RegisterMsgPair(1076, 1077, true);
		this.RegisterMsgPair(1062, 1063, true);
		this.RegisterMsgPair(1064, 1065, true);
		this.RegisterMsgPair(1066, 1067, true);
		this.RegisterMsgPair(1068, 1069, true);
		this.RegisterMsgPair(652, 653, true);
		this.RegisterMsgPair(1101, 1102, false);
		this.RegisterMsgPair(1115, 1116, true);
		this.RegisterMsgPair(1113, 1114, true);
		this.RegisterMsgPair(1111, 1112, true);
		this.RegisterMsgPair(1109, 1110, true);
		this.RegisterMsgPair(1107, 1108, true);
		this.RegisterMsgPair(1103, 1104, true);
		this.RegisterMsgPair(1105, 1106, true);
	}

	private void Start()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_0A.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInit));
	}

	private void Update()
	{
		int num = 0;
		object obj = this.obj;
		lock (obj)
		{
			num = this.recvIndex;
			this.recvIndex = (this.recvIndex + 1) % 2;
		}
		while (this.msgQueue[num].Count > 0)
		{
			MemoryStream memoryStream = this.msgQueue[num].Dequeue();
			if (memoryStream.Length < 3L)
			{
				global::Debug.Log(new object[]
				{
					"error msg!"
				});
			}
			else
			{
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				byte b = binaryReader.ReadByte();
				if (this.decryptKey != null && b != 0)
				{
					this.decryptKey.Process(memoryStream.GetBuffer(), 1, (int)memoryStream.Length - 1);
				}
				ushort num2 = binaryReader.ReadUInt16();
				this.OnMsgReply(num2);
				ClientSession.MsgHandler msgHandler = null;
				if (this.msgHandlers.TryGetValue(num2, out msgHandler) && msgHandler != null)
				{
					Delegate[] invocationList = msgHandler.GetInvocationList();
					int i = 0;
					int num3 = invocationList.Length;
					while (i < num3)
					{
						memoryStream.Position = 3L;
						((ClientSession.MsgHandler)invocationList[i])(memoryStream);
						i++;
					}
				}
				else
				{
					global::Debug.LogFormat("unknown msg, msgID = {0}", new object[]
					{
						num2
					});
				}
				if (this.hideFlag && (this.requestMsgID == 0 || !this.msgPair.ContainsKey(this.requestMsgID)))
				{
					GameUIManager.mInstance.HideIndicate();
				}
			}
		}
		this.UpdateSessionStatus(Time.deltaTime);
	}

	public void HandleConnect(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		if (binaryReader.ReadInt32() == 0)
		{
			this.IsConnected = true;
			global::Debug.Log(new object[]
			{
				"Connect OK!"
			});
		}
		else
		{
			global::Debug.Log(new object[]
			{
				"Connect Error!"
			});
		}
	}

	public void HandleDisconnect(MemoryStream stream)
	{
		this.IsConnected = false;
		bool flag = false;
		switch (this.status)
		{
		case ClientSession.ESessionStatus.ESS_Connecting:
			this.HandleError(ClientSession.ESessionError.ESE_ConnectDisconnect, false);
			flag = true;
			break;
		case ClientSession.ESessionStatus.ESS_Loading:
			this.HandleError(ClientSession.ESessionError.ESE_LoadingDisconnect, false);
			flag = true;
			break;
		case ClientSession.ESessionStatus.ESS_Gaming:
			if (this.requestMsgID != 0)
			{
				this.HandleError(ClientSession.ESessionError.ESE_SendDisconnect, false);
				flag = true;
			}
			this.status = ClientSession.ESessionStatus.ESS_None;
			break;
		}
		global::Debug.Log(new object[]
		{
			"Disconnect!"
		});
		if (!flag && this.showReconnect)
		{
			NetworkMessageBox.ShowMessageBox(7);
		}
	}

	public void OnMsgReply(ushort msgID)
	{
		this.hideFlag = false;
		if (this.requestMsgID != 0 && this.msgPair.ContainsKey(this.requestMsgID) && this.msgPair[this.requestMsgID] == msgID)
		{
			this.requestMsgID = 0;
			this.requestSerialID = 0;
			this.requestMsg = null;
			this.waitingSend = false;
			this.hideFlag = true;
		}
	}

	public void OnDataInit(bool versionInit, bool newPlayer)
	{
		this.status = ClientSession.ESessionStatus.ESS_Gaming;
		this.heartBeatTimer = Time.realtimeSinceStartup;
		this.heartBeatFrame = Time.frameCount;
		this.playerGender = -1;
		if (this.requestMsgID == 0 || !this.msgFlag.ContainsKey(this.requestMsgID) || !this.waitingSend)
		{
			GameUIManager.mInstance.HideIndicate();
		}
	}

	private void UpdateSessionStatus(float elapse)
	{
		switch (this.status)
		{
		case ClientSession.ESessionStatus.ESS_Connect:
			this.IsConnected = false;
			if (this.tcpClient.Connect(this.serverHost, this.serverPort))
			{
				this.status = ClientSession.ESessionStatus.ESS_Connecting;
				this.timeout = Time.time + 15f;
			}
			else
			{
				this.HandleError(ClientSession.ESessionError.ESE_RequestConnectError, true);
			}
			break;
		case ClientSession.ESessionStatus.ESS_Connecting:
			if (this.IsConnected)
			{
				this.status = ClientSession.ESessionStatus.ESS_Loading;
				this.timeout = Time.time + 30f;
				this.decryptKey = null;
				this.encryptKey = null;
				this.SendEnterGameMsg();
			}
			else if (Time.time > this.timeout)
			{
				this.HandleError(ClientSession.ESessionError.ESE_ConnectTimeout, true);
			}
			break;
		case ClientSession.ESessionStatus.ESS_Loading:
			if (this.stopTimer)
			{
				this.timeout += Time.deltaTime;
			}
			if (Time.time > this.timeout)
			{
				this.HandleError(ClientSession.ESessionError.ESE_LoadingTimeout, true);
			}
			break;
		case ClientSession.ESessionStatus.ESS_Gaming:
			if (this.waitingSend)
			{
				this.waitingSend = false;
				ushort msgID = this.requestMsgID;
				ushort msgSerialID = this.requestSerialID;
				object ojb = this.requestMsg;
				this.requestMsgID = 0;
				this.requestSerialID = 0;
				this.requestMsg = null;
				this.Send(msgID, ojb, msgSerialID);
			}
			else if (this.requestMsgID != 0 && Time.time - this.timeout > 15f)
			{
				this.HandleError(ClientSession.ESessionError.ESE_SendTimeout, true);
			}
			if (Time.realtimeSinceStartup - this.heartBeatTimer > 15f && Time.frameCount - this.heartBeatFrame > 300)
			{
				this.heartBeatTimer = Time.realtimeSinceStartup;
				this.heartBeatFrame = Time.frameCount;
				MC2S_HeartBeat ojb2 = new MC2S_HeartBeat();
				this.SendPacket(107, ojb2);
			}
			break;
		}
	}

	private void SetCryptoKey(byte[] CryptoKey)
	{
		byte[] array = new byte[16];
		byte[] array2 = new byte[16];
		if (CryptoKey.Length >= 8)
		{
			Array.Copy(CryptoKey, 0, array, 0, 8);
		}
		if (CryptoKey.Length >= 16)
		{
			Array.Copy(CryptoKey, 8, array2, 0, 8);
		}
		if (CryptoKey.Length >= 24)
		{
			Array.Copy(CryptoKey, 16, array, 8, 8);
		}
		if (CryptoKey.Length >= 32)
		{
			Array.Copy(CryptoKey, 24, array2, 8, 8);
		}
		this.decryptKey = new RC4(array);
		this.encryptKey = new RC4(array2);
	}

	public void HandleEnterGame(MemoryStream stream)
	{
		MS2C_EnterGame mS2C_EnterGame = Serializer.NonGeneric.Deserialize(typeof(MS2C_EnterGame), stream) as MS2C_EnterGame;
		if (mS2C_EnterGame.Result != 0 && mS2C_EnterGame.Result != 2)
		{
			GameUIManager.mInstance.HideIndicate();
			if (mS2C_EnterGame.Result == 1 || mS2C_EnterGame.Result == 9 || mS2C_EnterGame.Result == 12 || mS2C_EnterGame.Result == 13)
			{
				SdkU3d.setPropStr("SESSION", string.Empty);
				SdkU3d.ntLogout();
			}
			if (mS2C_EnterGame.Result == 16)
			{
				string @string = Singleton<StringManager>.Instance.GetString("EnterR_39", new object[]
				{
					mS2C_EnterGame.UID
				});
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OK, null);
				gameMessageBox.CanCloseByFadeBGClicked = false;
				GameMessageBox expr_1F2 = gameMessageBox;
				expr_1F2.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1F2.OkClick, new MessageBox.MessageDelegate(gameMessageBox.OnServerMsgOKClicked));
			}
			else
			{
				GameMessageBox.ShowServerMessageBox("EnterR", mS2C_EnterGame.Result);
			}
			return;
		}
		this.SetCryptoKey(mS2C_EnterGame.CryptoKey);
		if (mS2C_EnterGame.UID != string.Empty && (mS2C_EnterGame.UID != this.accountID || mS2C_EnterGame.AccessToken != this.accountKey))
		{
			this.accountID = mS2C_EnterGame.UID;
			this.accountKey = mS2C_EnterGame.AccessToken;
			SdkU3d.setPropStr("UIN", this.accountID);
			SdkU3d.setPropStr("SESSION", this.accountKey);
		}
		if (this.newConnect)
		{
			string a = SdkU3d.getChannel();
			if (a == "pps")
			{
				SdkU3d.setUserInfo("USERINFO_HOSTID", "ppsmobile_s" + GameSetting.Data.ServerID.ToString());
			}
			else
			{
				SdkU3d.setUserInfo("USERINFO_HOSTID", GameSetting.Data.ServerID.ToString());
			}
			if (a != "caohua")
			{
				SdkU3d.ntGameLoginSuccess();
			}
		}
		GameSetting.Data.Account = this.accountID;
		GameAnalytics.SetAccount();
		GameSetting.Data.LastGMLogin = (mS2C_EnterGame.Privilege > 0);
		GameSetting.UpdateNow = true;
		this.Privilege = mS2C_EnterGame.Privilege;
		GameManager gameMgr = Globals.Instance.GameMgr;
		if (mS2C_EnterGame.Result == 0)
		{
			gameMgr.Status = GameManager.EGameStatus.EGS_Loading;
			if (this.newConnect)
			{
				this.newConnect = false;
				if (mS2C_EnterGame.GMAutoPatch)
				{
					this.stopTimer = true;
					gameMgr.ReCheckVersion(new Action(this.SendGetPlayerDataMsg));
					return;
				}
			}
			this.SendGetPlayerDataMsg();
			return;
		}
		if (mS2C_EnterGame.Result == 2)
		{
			if (this.playerGender == 0 || this.playerGender == 1)
			{
				this.timeout = Time.time + 30f;
				MC2S_CreatePlayer mC2S_CreatePlayer = new MC2S_CreatePlayer();
				mC2S_CreatePlayer.Gender = this.playerGender;
				mC2S_CreatePlayer.Name = this.playerName;
				Globals.Instance.CliSession.Send(103, mC2S_CreatePlayer);
			}
			else
			{
				this.status = ClientSession.ESessionStatus.ESS_CreateChar;
				GameUIManager.mInstance.HideIndicate();
				if (gameMgr.Status == GameManager.EGameStatus.EGS_Login)
				{
					gameMgr.Status = GameManager.EGameStatus.EGS_CreateChar;
					if (mS2C_EnterGame.GMAutoPatch)
					{
						gameMgr.ReCheckVersion(delegate
						{
							GameUIManager.mInstance.LoadSessionScene<GUICreateCharacterScene>();
						});
					}
					else
					{
						GameUIManager.mInstance.LoadSessionScene<GUICreateCharacterScene>();
					}
				}
				else
				{
					GameMessageBox.ShowServerMessageBox("EnterR", 38);
				}
			}
			return;
		}
	}

	public void HandleHeartBeat(MemoryStream stream)
	{
		this.LagTime = Time.realtimeSinceStartup - this.heartBeatTimer;
	}

	public void HandleSerialIDError(MemoryStream stream)
	{
		MS2C_SerialIDError mS2C_SerialIDError = Serializer.NonGeneric.Deserialize(typeof(MS2C_SerialIDError), stream) as MS2C_SerialIDError;
		this.ResetSerialID((ushort)mS2C_SerialIDError.MsgSerialID);
	}

	public void HandleKickPlayer(MemoryStream stream)
	{
		MS2C_KickPlayer mS2C_KickPlayer = Serializer.NonGeneric.Deserialize(typeof(MS2C_KickPlayer), stream) as MS2C_KickPlayer;
		SdkU3d.setPropStr("SESSION", string.Empty);
		if (SdkU3d.getChannel() != "caohua")
		{
			SdkU3d.ntLogout();
		}
		if (SdkU3d.getChannel() == "kuaifa")
		{
			Globals.Instance.GameMgr.Status = GameManager.EGameStatus.EGS_None;
		}
		GameMessageBox.ShowServerMessageBox("EnterR", mS2C_KickPlayer.Reason);
	}

	public void HandleGMCommand(MemoryStream stream)
	{
		MS2C_GMCommand mS2C_GMCommand = Serializer.NonGeneric.Deserialize(typeof(MS2C_GMCommand), stream) as MS2C_GMCommand;
		global::Debug.LogFormat("GMCommand result = {0}", new object[]
		{
			mS2C_GMCommand.Result
		});
	}

	private void HandleError(ClientSession.ESessionError error, bool retry = false)
	{
		this.status = ClientSession.ESessionStatus.ESS_None;
		if (retry)
		{
			if (this.retryCount < 1)
			{
				this.retryCount++;
				if (this.requestMsgID != 0)
				{
					this.waitingSend = true;
				}
				this.RequestConnect();
				GameUIManager.mInstance.ShowIndicate();
				return;
			}
			this.Close();
		}
		this.retryCount = 0;
		NetworkMessageBox.ShowMessageBox((int)error);
		GameUIManager.mInstance.HideIndicate();
	}

	public void ShowReconnect(bool value)
	{
		this.showReconnect = value;
		if (this.showReconnect && !this.IsConnected)
		{
			NetworkMessageBox.ShowMessageBox(7);
		}
	}

	public void SendEnterGameMsg()
	{
		MC2S_EnterGame mC2S_EnterGame = new MC2S_EnterGame();
		mC2S_EnterGame.ID = this.accountID;
		mC2S_EnterGame.Platform = this.platform;
		mC2S_EnterGame.Channel = this.channel;
		mC2S_EnterGame.key = this.accountKey;
		mC2S_EnterGame.Publisher = this.publisher;
		mC2S_EnterGame.Reconnect = !this.newConnect;
		mC2S_EnterGame.ClientVersion = GameSetting.GameVersion;
		mC2S_EnterGame.DeviceID = SdkU3d.getPropStr("DEVICE_ID");
		mC2S_EnterGame.MAC = PlatformTools.GetMacAddress();
		mC2S_EnterGame.OSName = Tools.GetOSName();
		mC2S_EnterGame.OSVersion = SystemInfo.operatingSystem;
		mC2S_EnterGame.DeviceHeight = Screen.height;
		mC2S_EnterGame.DeviceWidth = Screen.width;
		mC2S_EnterGame.UDID = SdkU3d.getUdid();
		mC2S_EnterGame.ISP = PlatformTools.GetNetBusiness();
		mC2S_EnterGame.Network = PlatformTools.GetNetType();
		mC2S_EnterGame.AppChannel = SdkU3d.getAppChannel();
		mC2S_EnterGame.SDKVersion = SdkU3d.getSDKVersion(this.channel);
		if (this.platform == "ios")
		{
			mC2S_EnterGame.PayChannel = "app_store";
		}
		else
		{
			mC2S_EnterGame.PayChannel = this.channel;
		}
		string text = SystemInfo.deviceModel;
		text = text.Replace("%20", "_");
		text = Regex.Replace(text, "[^a-zA-Z0-9_.-]", "_");
		text = Regex.Replace(text, "_{1,}", "_");
		if (text.Length > 32)
		{
			text = text.Substring(0, 32);
		}
		mC2S_EnterGame.DeviceModel = text;
		if (this.channel == "netease")
		{
			mC2S_EnterGame.GuestID = SdkU3d.getPropStr("ORIGIN_GUEST_UID");
		}
		mC2S_EnterGame.DLLCrc = (uint)DownloadManager.Instance.AssemblyCrc;
		mC2S_EnterGame.ResCrc = (uint)DownloadManager.Instance.AttInfoCrc;
		mC2S_EnterGame.Break = PlatformTools.IsYueYuDevice();
		mC2S_EnterGame.ServerID = GameSetting.Data.ServerID;
		if (this.SendPacket(101, mC2S_EnterGame))
		{
			global::Debug.Log(new object[]
			{
				"Send C2S_EnterGame success!"
			});
		}
	}

	public void SendGetPlayerDataMsg()
	{
		this.stopTimer = false;
		LocalPlayer player = Globals.Instance.Player;
		if (this.SendPacket(105, new MC2S_GetPlayerData
		{
			StatsVersion = player.Version,
			ItemVersion = player.ItemSystem.Version,
			FashionVersion = player.ItemSystem.FashionVersion,
			PetVersion = player.PetSystem.Version,
			SocketVersion = player.TeamSystem.Version,
			SceneVersion = player.SceneVersion,
			MapRewardVersion = player.MapRewardVersion,
			AchievementVersion = player.AchievementSystem.Version,
			MailVersion = player.MailVersion,
			BuyDataVersion = player.BuyRecordVersion,
			SevenDayVersion = player.ActivitySystem.SevenDayVersion,
			ShareVersion = player.ActivitySystem.ShareVersion,
			ActivityAchievementVersion = player.ActivitySystem.ActivityAchievementVersion,
			ActivityValueVersion = player.ActivitySystem.ActivityValueVersion,
			ActivityShopVersion = player.ActivitySystem.ActivityShopVersion
		}))
		{
			global::Debug.Log(new object[]
			{
				"Send C2S_GetPlayerData success!"
			});
		}
	}

	public void SendCreatePlayerMsg(int gender, string name)
	{
		this.status = ClientSession.ESessionStatus.ESS_Loading;
		this.timeout = Time.time + 30f;
		this.playerGender = gender;
		this.playerName = name;
		GameUIManager.mInstance.ShowIndicate();
		if (!this.SendPacket(103, new MC2S_CreatePlayer
		{
			Gender = gender,
			Name = name
		}))
		{
			global::Debug.LogFormat("Request Connect, IP = {0}, Port = {1}, Account = {2}, Key = {3}", new object[]
			{
				this.serverHost,
				this.serverPort,
				this.accountID,
				this.accountKey
			});
			this.waitingSend = false;
			this.RequestConnect();
		}
	}

	public void OnCreatePlayerError()
	{
		this.status = ClientSession.ESessionStatus.ESS_CreateChar;
		GameUIManager.mInstance.HideIndicate();
	}

	public void ReturnLogin()
	{
		this.requestMsgID = 0;
		this.requestSerialID = 0;
		this.requestMsg = null;
		this.waitingSend = false;
		this.showReconnect = false;
		this.Close();
		this.status = ClientSession.ESessionStatus.ESS_None;
		this.serverShutDown = false;
	}

	public void ResetSerialID(ushort msgSerialID)
	{
		if (this.serialID != msgSerialID)
		{
			this.serialID = msgSerialID;
			if (Globals.Instance.Player.DataVersionEvent != null)
			{
				Globals.Instance.Player.DataVersionEvent();
			}
		}
		if (this.requestMsgID != 0 && this.serialID + 1 != this.requestSerialID)
		{
			bool flag = false;
			if (this.msgFlag.TryGetValue(this.requestMsgID, out flag) && flag)
			{
				this.requestMsgID = 0;
				this.requestSerialID = 0;
				this.requestMsg = null;
				this.waitingSend = false;
				GameUIManager.mInstance.HideIndicate();
			}
		}
	}

	public void RefreshToken(string accessToken)
	{
		this.accountKey = accessToken;
		SdkU3d.setPropStr("SESSION", accessToken);
	}

	public void SendMsgCancel()
	{
		this.requestMsgID = 0;
		this.requestSerialID = 0;
		this.requestMsg = null;
		this.waitingSend = false;
	}

	public void ServerShutDown()
	{
		this.serverShutDown = true;
	}
}
