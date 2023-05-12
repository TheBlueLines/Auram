using System.IO.Compression;
using System.Text;

namespace Auram
{
	internal class Engine
	{
		public static string GetKey(byte[] data)
		{
			int length = VarintBitConverter.ToInt32(data);
			byte[] lengthx = VarintBitConverter.GetVarintBytes(length);
			return Encoding.UTF8.GetString(data, lengthx.Length, length);
		}
		public static byte[] PrefixedKey(string text)
		{
			byte[] data = Encoding.UTF8.GetBytes(text);
			return Combine(VarintBitConverter.GetVarintBytes(data.Length), data);
		}
		public static byte[] GetValue(byte[] value)
		{
			List<byte> bytes = value.ToList();
			int length = VarintBitConverter.ToInt32(value);
			byte[] lengthx = VarintBitConverter.GetVarintBytes(length);
			return bytes.GetRange(lengthx.Length, length).ToArray();
		}
		public static byte[] PrefixedValue(byte[] value)
		{
			return Combine(VarintBitConverter.GetVarintBytes(value.Length), value);
		}
		public static byte[] Combine(params byte[][] arrays)
		{
			byte[] rv = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;
			foreach (byte[] array in arrays)
			{
				Buffer.BlockCopy(array, 0, rv, offset, array.Length);
				offset += array.Length;
			}
			return rv;
		}
		public static void CopyTo(Stream src, Stream dest)
		{
			byte[] bytes = new byte[4096];
			int cnt;
			while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
			{
				dest.Write(bytes, 0, cnt);
			}
		}
		public static byte[] Zip(byte[] data)
		{
			using (var msi = new MemoryStream(data))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
				{
					CopyTo(msi, gs);
				}
				return mso.ToArray();
			}
		}
		public static byte[] Unzip(byte[] bytes)
		{
			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				{
					CopyTo(gs, mso);
				}
				return mso.ToArray();
			}
		}
	}
}
