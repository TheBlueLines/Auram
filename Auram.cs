using System.Text;

namespace Auram
{
	public class Database
	{
		public Dictionary<string, byte[]> data = new();
		public Database(string? path = null)
		{
			if (!string.IsNullOrEmpty(path))
			{
				Load(path);
			}
		}
		public bool Add(string key, byte[] value)
		{
			return data.TryAdd(key, value);
		}
		public bool Add(string key, string value)
		{
			return Add(key, Encoding.UTF8.GetBytes(value));
		}
		public bool Add(string key, int value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, uint value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, short value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, ushort value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, long value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, ulong value)
		{
			return Add(key, BitConverter.GetBytes(value));
		}
		public bool Add(string key, Guid value)
		{
			return Add(key, value.ToByteArray());
		}
		public bool Add(string key, DateTime value)
		{
			return Add(key, value.ToBinary());
		}
		public void Set(string key, byte[] value)
		{
			data[key] = value;
		}
		public void Set(string key, string value)
		{
			Set(key, Encoding.UTF8.GetBytes(value));
		}
		public void Set(string key, int value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, uint value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, short value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, ushort value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, long value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, ulong value)
		{
			Set(key, BitConverter.GetBytes(value));
		}
		public void Set(string key, Guid value)
		{
			Set(key, value.ToByteArray());
		}
		public void Set(string key, DateTime value)
		{
			Set(key, value.ToBinary());
		}
		public T? Get<T>(string key)
		{
			if (data.ContainsKey(key))
			{
				if (typeof(T) == typeof(byte[]))
				{
					return (T)(object)data[key];
				}
				if (typeof(T) == typeof(string))
				{
					return (T)(object)Encoding.UTF8.GetString(data[key]);
				}
				if (typeof(T) == typeof(int))
				{
					return (T)(object)BitConverter.ToInt32(data[key]);
				}
				if (typeof(T) == typeof(uint))
				{
					return (T)(object)BitConverter.ToUInt32(data[key]);
				}
				if (typeof(T) == typeof(short))
				{
					return (T)(object)BitConverter.ToInt16(data[key]);
				}
				if (typeof(T) == typeof(ushort))
				{
					return (T)(object)BitConverter.ToUInt16(data[key]);
				}
				if (typeof(T) == typeof(long))
				{
					return (T)(object)BitConverter.ToInt64(data[key]);
				}
				if (typeof(T) == typeof(ulong))
				{
					return (T)(object)BitConverter.ToUInt64(data[key]);
				}
				if (typeof(T) == typeof(Guid))
				{
					return (T)(object)new Guid(data[key]);
				}
				if (typeof(T) == typeof(DateTime))
				{
					return (T)(object)DateTime.FromBinary(BitConverter.ToInt64(data[key]));
				}
			}
			return default(T);
		}
		public bool Remove(string key)
		{
			return data.Remove(key);
		}
		public void Clear()
		{
			data.Clear();
		}
		public void Save(string path)
		{
			List<byte[]> tmp = new();
			foreach (string value in data.Keys)
			{
				tmp.Add(Engine.PrefixedKey(value));
				tmp.Add(Engine.PrefixedValue(data[value]));
			}
			File.WriteAllBytes(path, Engine.Zip(Engine.Combine(tmp.ToArray())));
		}
		public void Load(string path)
		{
			List<byte> data = Engine.Unzip(File.ReadAllBytes(path)).ToList();
			while (data.Count > 0)
			{
				string key = Engine.GetKey(data.ToArray());
				byte[] raw = Encoding.UTF8.GetBytes(key);
				data.RemoveRange(0, raw.Length + VarintBitConverter.GetVarintBytes(raw.Length).Length);
				byte[] value = Engine.GetValue(data.ToArray());
				data.RemoveRange(0, value.Length + VarintBitConverter.GetVarintBytes(value.Length).Length);
				Add(key, value);
			}
		}
	}
}