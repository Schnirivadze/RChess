using Shared;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Servertest
{
	internal class Program
	{
		static void Main(string[] args)
		{
			File.WriteAllText("boards.txt", "");
			Console.OutputEncoding = Encoding.Unicode;
			string ip = "192.168.178.21";
			string name = Console.ReadLine();
			TcpClient client = new TcpClient();
			client.Connect(ip, 13000);
			Packet packet = new();
			packet.AddInfo(InfoType.Username, name);
			StreamWriter sw = new(client.GetStream());
			StreamReader sr = new(client.GetStream());

			//send info
			packet = new();
			packet.AddInfo(InfoType.Username, name);
			packet.Assemble();
			sw.WriteLine(packet.ToString());
			sw.Flush();
			Console.WriteLine($"name: {name}");
			//get id
			packet = new(sr.ReadLine());
			string enemyid = packet.GetInfo(InfoType.EnemyId);
			Console.WriteLine($"enemyid: {enemyid}");

			string pies = "♔♕♗♘♖♙";

			while (true)
			{
				Thread.Sleep(100000);
				////write
				//packet = new();
				//packet.AddInfo(InfoType.Id, enemyid);
				//packet.Assemble();
				//sw.WriteLine(packet.ToString());
				//sw.Flush();
				////read
				//packet = new(sr.ReadLine());
				////process
				////string board = "                                                                 ";
				//char[,] board =
				//{
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//	{' ',' ',' ',' ',' ',' ',' ',' '},
				//};
				//foreach (XmlNode figure_node in packet.GetBoard())
				//{
				//	board[Convert.ToInt32(figure_node.Attributes["y"]), Convert.ToInt32(figure_node.Attributes["x"])] = pies[Convert.ToInt32(figure_node.Attributes["type"])];
				//}
				//for (int y = 0; y < 8; y++)
				//{
				//	for (int x = 0; x < 8; x++)
				//	{
				//		File.AppendAllText("boards.txt", $"{board[y, x]}");
				//	}
				//	File.AppendAllText("boards.txt", $"\n");

				//}
				//File.AppendAllText("boards.txt", $"\n\n");
			}
		}
	}
}