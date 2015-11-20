using System;

namespace Util
{
	public class ObscuredIntArray
	{
		private int currentCryptoKey;

		private int[] hiddenValues1;

		private int[] hiddenValues2;

		public int this[int index]
		{
			get
			{
				return (this.hiddenValues1[index] | this.hiddenValues2[index] << 16) ^ this.currentCryptoKey;
			}
			set
			{
				int num = value ^ this.currentCryptoKey;
				this.hiddenValues1[index] = (num & 65535);
                this.hiddenValues2[index] = (int)(((long)num & (long)(0xfffffffe)) >> 16);
			}
		}

		public ObscuredIntArray(int sizeArray)
		{
			this.hiddenValues1 = new int[sizeArray];
			this.hiddenValues2 = new int[sizeArray];
			long ticks = DateTime.Now.Ticks;
			Random random = new Random((int)(ticks & (long)(0xfffffffe)) | (int)(ticks >> 32));
			this.currentCryptoKey = random.Next(268435455);
		}
	}
}
