using System;
using System.Threading;

namespace ProtoBuf
{
	internal sealed class BufferPool
	{
		private const int PoolSize = 20;

		internal const int BufferLength = 1024;

		private static readonly object[] pool = new object[20];

		private BufferPool()
		{
		}

		internal static void Flush()
		{
			for (int i = 0; i < BufferPool.pool.Length; i++)
			{
				Interlocked.Exchange(ref BufferPool.pool[i], null);
			}
		}

		internal static byte[] GetBuffer()
		{
			for (int i = 0; i < BufferPool.pool.Length; i++)
			{
				object obj;
				if ((obj = Interlocked.Exchange(ref BufferPool.pool[i], null)) != null)
				{
					return (byte[])obj;
				}
			}
			return new byte[1024];
		}

		internal static void ResizeAndFlushLeft(ref byte[] buffer, int toFitAtLeastBytes, int copyFromIndex, int copyBytes)
		{
			int num = buffer.Length * 2;
			if (num < toFitAtLeastBytes)
			{
				num = toFitAtLeastBytes;
			}
			byte[] array = new byte[num];
			if (copyBytes > 0)
			{
				Helpers.BlockCopy(buffer, copyFromIndex, array, 0, copyBytes);
			}
			if (buffer.Length == 1024)
			{
				BufferPool.ReleaseBufferToPool(ref buffer);
			}
			buffer = array;
		}

		internal static void ReleaseBufferToPool(ref byte[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			if (buffer.Length == 1024)
			{
				for (int i = 0; i < BufferPool.pool.Length; i++)
				{
					if (Interlocked.CompareExchange(ref BufferPool.pool[i], buffer, null) == null)
					{
						break;
					}
				}
			}
			buffer = null;
		}
	}
}
