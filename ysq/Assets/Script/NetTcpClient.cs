using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class NetTcpClient
{
	public delegate void OnConnectCallback(int result);

	public delegate void OnDataRecevieCallback(byte[] buffer, uint offset, uint size);

	public delegate void OnDisconnectCallback();

	private const int recvBufferSize = 524288;

	private const int sendBufferSize = 524288;

	public NetTcpClient.OnConnectCallback OnConnectEvent;

	public NetTcpClient.OnDataRecevieCallback OnDataRecevieEvent;

	public NetTcpClient.OnDisconnectCallback OnDisconnectEvent;

	private TcpClient client;

	private byte[] recvBuffer = new byte[524288];

	private int bytesRead;

	private MemoryStream[] sendBuffer = new MemoryStream[]
	{
		new MemoryStream(524288),
		new MemoryStream(524288)
	};

	private object obj = new object();

	private int sendIndex;

	private bool isSending;

	private uint msgHead;

	public bool Connect(string host, int port)
	{
		object obj = this.obj;
		lock (obj)
		{
			try
			{
				if (this.client != null && this.client.Connected)
				{
					bool result = false;
					return result;
				}
				this.client = new TcpClient();
				if (this.client == null)
				{
					bool result = false;
					return result;
				}
				this.client.ReceiveBufferSize = 524288;
				this.client.SendBufferSize = 524288;
				this.client.NoDelay = false;
				this.client.LingerState.LingerTime = 0;
				this.client.LingerState.Enabled = true;
				this.sendBuffer[0].Seek(0L, SeekOrigin.Begin);
				this.sendBuffer[0].SetLength(0L);
				this.sendBuffer[1].Seek(0L, SeekOrigin.Begin);
				this.sendBuffer[1].SetLength(0L);
				this.sendIndex = 0;
				this.isSending = false;
				this.msgHead = 0u;
				IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
				this.client.BeginConnect(hostAddresses, port, new AsyncCallback(this.ConnectCallBack), this.client);
			}
			catch (Exception ex)
			{
				Debug.LogException("TcpClient Connect :", ex);
				this.client = null;
				bool result = false;
				return result;
			}
		}
		return true;
	}

	public void Close()
	{
		object obj = this.obj;
		lock (obj)
		{
			if (this.client != null)
			{
				if (this.client != null && this.client.Connected)
				{
					this.client.Close();
				}
			}
		}
	}

	public bool Send(byte[] buffer, uint size)
	{
		if (this.client == null || !this.client.Connected || size == 0u || size > 524288u)
		{
			return false;
		}
		object obj = this.obj;
		lock (obj)
		{
			int num = (this.sendIndex + 1) % 2;
			if (this.sendBuffer[num].Length + (long)((ulong)size) > (long)this.sendBuffer[num].Capacity)
			{
				bool result = false;
				return result;
			}
			this.sendBuffer[num].Write(BitConverter.GetBytes(size ^ 1702u), 0, 4);
			this.sendBuffer[num].Write(buffer, 0, (int)size);
			if (!this.isSending)
			{
				this.isSending = true;
				this.sendIndex = num;
				try
				{
					this.client.GetStream().BeginWrite(this.sendBuffer[this.sendIndex].GetBuffer(), 0, (int)this.sendBuffer[this.sendIndex].Length, new AsyncCallback(this.WriteCallBack), this.client);
				}
				catch (Exception ex)
				{
					Debug.LogException("TcpClient Send :", ex);
					this.isSending = false;
					bool result = false;
					return result;
				}
			}
		}
		return true;
	}

	private void Shutdown(TcpClient tcp)
	{
		object obj = this.obj;
		lock (obj)
		{
			tcp.Close();
			if (tcp == this.client)
			{
				this.client = null;
				this.OnDisconnectEvent();
			}
		}
	}

	private void ConnectCallBack(IAsyncResult ar)
	{
		TcpClient tcpClient = (TcpClient)ar.AsyncState;
		int num = 0;
		try
		{
			tcpClient.EndConnect(ar);
			if (!tcpClient.Connected)
			{
				num = 2;
			}
		}
		catch (Exception ex)
		{
			Debug.LogException("TcpClient Connect CallBack :", ex);
			num = 1;
		}
		this.OnConnectEvent(num);
		if (num == 0)
		{
			try
			{
				this.bytesRead = 0;
				tcpClient.GetStream().BeginRead(this.recvBuffer, 0, 4, new AsyncCallback(this.ReadCallBack), tcpClient);
			}
			catch (Exception ex2)
			{
				this.Shutdown(tcpClient);
				Debug.LogException("TcpClient Connect Callback:", ex2);
			}
		}
	}

	private void ReadCallBack(IAsyncResult ar)
	{
		TcpClient tcpClient = (TcpClient)ar.AsyncState;
		if (tcpClient == null || !tcpClient.Connected)
		{
			this.Shutdown(tcpClient);
			return;
		}
		try
		{
			NetworkStream stream = tcpClient.GetStream();
			this.bytesRead += stream.EndRead(ar);
			if (this.msgHead == 0u)
			{
				if (this.bytesRead != 4)
				{
					stream.BeginRead(this.recvBuffer, this.bytesRead, 4, new AsyncCallback(this.ReadCallBack), tcpClient);
				}
				else
				{
					this.msgHead = BitConverter.ToUInt32(this.recvBuffer, 0);
					this.msgHead = (this.msgHead ^ 131989912u) >> 1;
					if (this.msgHead == 0u || this.msgHead > 524288u)
					{
						this.Shutdown(tcpClient);
					}
					else
					{
						this.bytesRead = 0;
						stream.BeginRead(this.recvBuffer, 0, (int)this.msgHead, new AsyncCallback(this.ReadCallBack), tcpClient);
					}
				}
			}
			else if ((ulong)this.msgHead != (ulong)((long)this.bytesRead))
			{
				stream.BeginRead(this.recvBuffer, this.bytesRead, (int)(this.msgHead - (uint)this.bytesRead), new AsyncCallback(this.ReadCallBack), tcpClient);
			}
			else
			{
				uint num2;
				for (uint num = 0u; num < this.msgHead; num += num2)
				{
					if (num + 4u > this.msgHead)
					{
						break;
					}
					num2 = BitConverter.ToUInt32(this.recvBuffer, (int)num);
					num2 ^= 209u;
					num += 4u;
					if (num + num2 > this.msgHead)
					{
						break;
					}
					this.OnDataRecevieEvent(this.recvBuffer, num, num2);
				}
				this.msgHead = 0u;
				this.bytesRead = 0;
				stream.BeginRead(this.recvBuffer, 0, 4, new AsyncCallback(this.ReadCallBack), tcpClient);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException("TcpClient Read Callback:", ex);
			this.Shutdown(tcpClient);
		}
	}

	private void WriteCallBack(IAsyncResult ar)
	{
		TcpClient tcpClient = (TcpClient)ar.AsyncState;
		if (tcpClient == null || !tcpClient.Connected)
		{
			this.Shutdown(tcpClient);
			return;
		}
		try
		{
			NetworkStream stream = tcpClient.GetStream();
			stream.EndWrite(ar);
			object obj = this.obj;
			lock (obj)
			{
				this.sendBuffer[this.sendIndex].Seek(0L, SeekOrigin.Begin);
				this.sendBuffer[this.sendIndex].SetLength(0L);
				this.sendIndex = (this.sendIndex + 1) % 2;
				if (this.sendBuffer[this.sendIndex].Length == 0L)
				{
					this.isSending = false;
				}
				else
				{
					this.isSending = true;
					stream.BeginWrite(this.sendBuffer[this.sendIndex].GetBuffer(), 0, (int)this.sendBuffer[this.sendIndex].Length, new AsyncCallback(this.WriteCallBack), tcpClient);
				}
			}
		}
		catch (Exception ex)
		{
			this.Shutdown(tcpClient);
			Debug.LogException("TcpClient Write Callback:", ex);
		}
	}
}
