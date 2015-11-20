using System;

namespace Util
{
	public class ObscuredLongArray
	{
		private int currentCryptoKey;

		private long[] hiddenValues1;

		private long[] hiddenValues2;

		public long this[int index]
		{
			get
			{
				return (this.hiddenValues1[index] | this.hiddenValues2[index] << 32) ^ (long)this.currentCryptoKey;
			}
			set
			{
				long num = value ^ (long)this.currentCryptoKey;
                this.hiddenValues1[index] = (num & (long)(0xffff0000L));
				this.hiddenValues2[index] = (long)((ulong)(num & -4294967296L) >> 32);
			}
		}

		public ObscuredLongArray(int sizeArray)
		{
			this.hiddenValues1 = new long[sizeArray];
			this.hiddenValues2 = new long[sizeArray];
			long ticks = DateTime.Now.Ticks;
            Random random = new Random((int)(ticks & (long)(0xffff0000L)) | (int)(ticks >> 32));
			this.currentCryptoKey = random.Next(268435455);
		}
	}
}
