#pragma warning disable CS0618 // Type or member is obsolete
using Shared;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using static Shared.Logging;
using static Shared.Logging.LogType;

namespace RChessS
{
	class User
	{
		public bool connected = false;
		public string? name;
		public XmlNodeList? figures;
		public User() { }
		public User(string name) { this.name = name; }

	}
	class Server
	{
		static Dictionary<int, User> users = new();
		static void Main()
		{
			PrepareLog();
			TcpListener server = new(13000);
			try
			{
				server.Start();
			}
			catch (Exception e)
			{
				Log(Error, "Failed to start server :" + e.Message);
				return;
			}
			Log(Info, "Server is ready for connections");
			while (true)
			{
				try
				{
					TcpClient newuser = server.AcceptTcpClient();
					if (newuser != null)
					{
						Log(Info, "Accepted new user: " + newuser.Client.RemoteEndPoint);
						Thread thread = new Thread(new ParameterizedThreadStart(HandleUser));
						thread.Start(newuser);
					}
					else Log(Warning, "Failed to accept user");
				}
				catch (Exception e)
				{
					Log(Warning, "Eror while accepting user :" + e.Message);
				}
			}
		}

		private static void HandleUser(object? obj)
		{
			//Handling
			if (obj == null) { Log(Error, "User object was null"); return; }
			TcpClient client = (TcpClient)obj;
			Log(Info, $"Connection {client.Client.RemoteEndPoint} getting ready");
			client.Client.Ttl = 120;
			NetworkStream stream = client.GetStream();
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);
			StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			Packet packet;
			Log(Info, $"Connection {client.Client.RemoteEndPoint} is ready");


			//info exchange
			//	get info
			int id = Random.Shared.Next(int.MaxValue);
			users.Add(id, new());
			packet = new(reader.ReadLine());
			users[id].name = packet.GetInfo(InfoType.Username);

			//	wait for enemy
			ushort waiting_cycles = 0;
			while (users.Count < 2)
			{
				if (waiting_cycles == client.Client.Ttl)
				{
					Log(Warning, "There wasnt enough people");
					Log(Info, $"[{users[id].name}] User disconnected");
					return;
				}
				waiting_cycles++; 
				Thread.Sleep(1000);
			}


			//	send info
			int enid = users.Where(e => e.Key != id && e.Value.connected == false).First().Key;
			packet = new Packet();
			packet.AddInfo(InfoType.EnemyName, users[enid].name);
			packet.Assemble();
			writer.WriteLine(packet.ToString());
			writer.Flush();

			//chess cycle
			while (client.Client.Connected)
			{
				try
				{
					Thread.Sleep(1000);
					//read
					packet = new(reader.ReadLine());
					users[id].figures = packet.GetBoard();

					//write
					packet = new();
					packet.SetBoard(users[enid].figures);
					packet.Assemble();
					writer.WriteLine(packet.ToString());
					writer.Flush();
				}
				catch (Exception e)
				{
					Log(Warning, $"[{e.GetType()}][{users[enid].name}] {e}");
				}
			}
			Log(Info, $"[{users[id].name}] User disconnected");
		}
	}
}