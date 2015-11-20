using System;

namespace Util
{
	public class ObscuredLong
	{
		private long hiddenValue1;

		private int currentCryptoKey;

		private long hiddenValue2;

		public long Value
		{
			get
			{
				return (this.hiddenValue1 | this.hiddenValue2 << 32) ^ (long)this.currentCryptoKey;
			}
			set
			{
				long num = value ^ (long)this.currentCryptoKey;
				this.hiddenValue1 = (num & (long)(0xfffffffe));
				this.hiddenValue2 = (long)((ulong)(num & -4294967296L) >> 32);
			}
		}

		public ObscuredLong(int initValue)
		{
			long ticks = DateTime.Now.Ticks;
            Random random = new Random((int)(ticks & (long)(0xfffffffe)) | (int)(ticks >> 32));
			this.currentCryptoKey = random.Next(268435455);
			this.Value = (long)initValue;
		}
	}
}
