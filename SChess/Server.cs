using Shared;
using System.Net.Sockets;
using System.Text;

namespace SChess
{
	class Server
	{
		public static List<User> users = new List<User>();
		public static List<Thread> threads = new List<Thread>();
		static Random rnd = new();
		static void Main()
		{
			Console.WriteLine("Server getting ready");
			File.WriteAllText("logs\\outsend.txt", "");
			File.WriteAllText("logs\\insend.txt", "");
			TcpListener server = new(13000);
			try
			{
				server.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to start server :" + e.Message);
				Console.ReadLine();
				return;
			}
			Console.WriteLine("Server is ready for connections");
			while (true)
			{
				try
				{
					TcpClient newuser = server.AcceptTcpClient();
					if (newuser != null)
					{
						Console.WriteLine("Accepted new user: " + newuser.Client.RemoteEndPoint);
						Thread thread = new Thread(new ParameterizedThreadStart(HandleUser));
						thread.Start(newuser);
					}
					else Console.WriteLine("Failed to accept user");
				}
				catch (Exception e)
				{
					Console.WriteLine("Eror while accepting user :" + e.Message);
					Console.ReadLine();
					return;
				}
			}
		}
		static int GetUserPlace(string id)
		{
			return users.FindIndex(u => u.id == id);
		}
		private static void HandleUser(object? obj)
		{
			Console.WriteLine("Client getting ready to be handeled");
			TcpClient client = obj as TcpClient;
			NetworkStream stream = client.GetStream();
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);
			StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			Packet packet = new();
			Console.WriteLine("Client is handeled");


			//get name
			packet = new(reader.ReadLine());
			string username = packet.GetInfo(InfoType.Username);

			Console.WriteLine($"Clients username: {username}");
			string id = $"{rnd.Next(int.MaxValue)}";
			Console.WriteLine($"{username}'s id: {id}");
			users.Add(new(id, username));

			while (users.Count < 2) { Thread.Sleep(1000); }
			//send enemyid
			packet = new();
			string enid = users.Where(e => e.id != id && e.conected == false).First().id;
			Console.WriteLine($"{username}'s enemy id: {enid}");
			packet.AddInfo(InfoType.EnemyId, enid);
			packet.Assemble();
			writer.WriteLine(packet.ToString());
			writer.Flush();

			while (client.Connected)
			{
				Thread.Sleep(100000);
				////read
				//packet = new(reader.ReadLine());
				//int enemyindex=GetUserPlace(packet.GetInfo(InfoType.Id));
				//File.AppendAllText("logs\\insend.xml", $"<insend time=\"{DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}\">\t{packet.ToString()}\n");

				////write
				//File.AppendAllText("logs\\outsend.xml", $"<outsend time=\"{DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}\">\t{packet.ToString()}\n");
				//writer.WriteLine(packet.ToString());
				//writer.Flush();

			}
			Console.WriteLine("Client disconnected");
		}
	}
}