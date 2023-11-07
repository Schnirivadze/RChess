#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS0618 // Type or member is obsolete
using Shared;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace SChess
{
	class Server
	{
		public static Dictionary<string, User> users = new();
		public static List<Thread> threads = new List<Thread>();
		static Random rnd = new();
		static void Main()
		{

			File.WriteAllText("logs\\insend.xml", "");
			File.WriteAllText("logs\\outsend.xml", "");
			Console.WriteLine("Server getting ready");
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
		private static void HandleUser(object? obj)
		{
			Console.WriteLine("Client getting ready to be handeled");
			TcpClient client = obj as TcpClient;
			client.Client.Ttl = 255;
			NetworkStream stream = client.GetStream();
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);
			StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			Packet packet = new();
			Console.WriteLine($"{client.Client.RemoteEndPoint} is handeled");


			//get name
			packet = new(reader.ReadLine());
			string username = packet.GetInfo(InfoType.Username);

			Console.WriteLine($"Clients username: {username}");
			string id = $"{rnd.Next(int.MaxValue)}";
			Console.WriteLine($"{username}'s id: {id}");
			users.Add(id, new(username));

			while (users.Count < 2) { Thread.Sleep(1000); }
			//send enemyid
			string enid = users.Where(e => e.Key != id && e.Value.conected == false).First().Key;
			Console.WriteLine($"{username}'s enemy id: {enid}");
			writer.WriteLine();
			writer.Flush();

			string readwrite = "";

			while (client.Client.Connected)
			{
				try
				{
					Thread.Sleep(1000);
					//read
					readwrite = "read";
					packet = new(reader.ReadLine());
					users[id].figures=packet.GetBoard();				//<----------
					File.AppendAllText("logs\\insend.xml", $"<insend time=\"{DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}\" target=\"{username}\">\t{packet.ToString()}\n");

					//write
					readwrite = "write";
					packet = new();
					packet.SetBoard(users[enid].figures);				//<----------
					packet.Assemble();
					writer.WriteLine(packet.ToString());
					writer.Flush();
					File.AppendAllText("logs\\outsend.xml", $"<outsend time=\"{DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}\" target=\"{username}\">\t{packet.ToString()}\n");
				}
				catch (IOException e)
				{
					Console.WriteLine($"{username} {readwrite} IoError>>>> {e.Message}");
				}
				catch (Exception e)
				{
					Console.WriteLine($"{username} {readwrite} ERROR>>>>" + e.ToString());
				}
			}
			Console.WriteLine($"{username} disconnected");
		}
	}
}