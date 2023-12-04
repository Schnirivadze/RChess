#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using Raylib_cs;
using Shared;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using static Raylib_cs.Raylib;
using static Shared.Logging;
using static Shared.Logging.LogType;
namespace RChess
{
	class Program
	{
		static string ip = File.ReadAllText("ip.txt");
		static void Main()
		{
			PrepareLog();
			Console.OutputEncoding = Encoding.Unicode;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);

			Thread networkthread = new Thread(new ThreadStart(NetworkFunction));
			networkthread.Start();
			Thread filesthread = new Thread(new ParameterizedThreadStart(filesthreadf));
			filesthread.Start(ip);

			SetTraceLogLevel(TraceLogLevel.LOG_WARNING);
			InitWindow(Game.chessboard_width * 8 + 200, Game.chessboard_width * 8 + 200, "Schache 2D");
			SetTargetFPS(60);

			Game.generatePieces();
			while (!WindowShouldClose())
			{
				if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) Game.handleClick();
				Game.drawAll();
			}
			CloseWindow();
			while(Console.ReadKey().Key!=ConsoleKey.E) continue;
		}

		private static void filesthreadf(object ip)
		{

			//Files.FilesFunc((string)ip);
		}

		static void NetworkFunction()
		{
			
			Log(Info,"network started");
			
			Console.Write("Enter name: ");
			string name = Console.ReadLine();

			TcpClient client = new TcpClient();
			client.Client.Ttl = 120;
			client.Connect(ip, 13000);
			Log(Info,"Connected");

			Packet packet = new();
			StreamWriter sw = new(client.GetStream(), Encoding.UTF8);
			StreamReader sr = new(client.GetStream(), Encoding.UTF8);

			//send info
			packet = new();
			packet.AddInfo(InfoType.Username, name);
			packet.Assemble();
			sw.WriteLine(packet.ToString());
			sw.Flush();
			Thread.Sleep(1000);
			string p = sr.ReadLine();
			Log(Debug,p);
			packet = new(p);
			string enemy_name = packet.GetInfo(InfoType.EnemyName);
			Log(Info,enemy_name);
			List<FigureE> enemytemp = new();

			while (true)
			{
				Thread.Sleep(1000);

				//write
				packet = new();
				for (int i = 0; i < Game.figuresF.Count; i++) packet.AddFigure(Game.figuresF[i].type, 7 - Game.figuresF[i].pos.X, 7 - Game.figuresF[i].pos.Y);
				packet.Assemble();

				sw.WriteLine(packet.ToString());
				sw.Flush();

				//read
				packet = new(sr.ReadLine());

				//process
				enemytemp.Clear();
				foreach (XmlNode figure_node in packet.GetBoard())
				{
					int e_figure_y = Convert.ToInt32(figure_node.Attributes["y"].Value);
					int e_figure_x = Convert.ToInt32(figure_node.Attributes["x"].Value);
					int e_figure_type = Convert.ToInt32(figure_node.Attributes["type"].Value);
					enemytemp.Add(new((FigureType)e_figure_type, e_figure_x, e_figure_y));
				}
				Game.figuresE = enemytemp;
			}
		}
		
		static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			// Обработка исключений на глобальном уровне
			Exception ex = (Exception)e.ExceptionObject;


			Log(Error,"Глобальный обработчик исключений перехватил исключение: " + ex.Message);
			// Можно выполнить дополнительную обработку или логирование здесь
			
		}
	}
}