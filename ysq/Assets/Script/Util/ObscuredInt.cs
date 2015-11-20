using System;

namespace Util
{
	public class ObscuredInt
	{
		private int hiddenValue1;

		private int currentCryptoKey;

		private int hiddenValue2;

		public int Value
		{
			get
			{
				return (this.hiddenValue1 | this.hiddenValue2 << 16) ^ this.currentCryptoKey;
			}
			set
			{
				int num = value ^ this.currentCryptoKey;
				this.hiddenValue1 = (num & 65535);
				this.hiddenValue2 = (int)((long)num & (long)(0xfe)) >> 16;
			}
		}

		public ObscuredInt(int initValue)
		{
			long ticks = DateTime.Now.Ticks;
			Random random = new Random((int)(ticks & (long)(0xfffffffe)) | (int)(ticks >> 32));
			this.currentCryptoKey = random.Next(268435455);
			this.Value = initValue;
		}
	}
}
