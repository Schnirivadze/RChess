#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using Raylib_cs;
using Shared;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using static Raylib_cs.Raylib;
namespace RChess
{
	class Program
	{
		static void Main()
		{
			Console.OutputEncoding = Encoding.Unicode;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalExceptionHandler);

			Thread networkthread = new Thread(new ThreadStart(NetworkFunction));
			networkthread.Start();
			Thread notclosethread = new Thread(new ThreadStart(NotCloseThread));
			notclosethread.Start();

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
		}
		static void NetworkFunction()
		{
			Console.WriteLine("network started");
			string ip = File.ReadAllText("ip.txt");
			string name = Console.ReadLine();

			TcpClient client = new TcpClient();
			client.Client.Ttl = 255;
			client.Connect(ip, 13000);
			Console.WriteLine("Connected");

			Packet packet = new();
			StreamWriter sw = new(client.GetStream());
			StreamReader sr = new(client.GetStream());

			//send info
			packet = new();
			packet.AddInfo(InfoType.Username, name);
			packet.Assemble();
			sw.WriteLine(packet.ToString());
			sw.Flush();

			sr.ReadLine();
			List<FigureE> enemytemp = new();

			while (true)
			{
				Thread.Sleep(1000);

				//write
				packet = new();
				for (int i = 0; i < Game.figuresF.Count; i++) packet.AddFigure(Game.figuresF[i].type, 7 - Game.figuresF[i].pos.X, 7 - Game.figuresF[i].pos.Y);
				packet.Assemble();

				Logging.LogBoard(false, packet.GetBoard());
				sw.WriteLine(packet.ToString());
				sw.Flush();

				//read
				packet = new(sr.ReadLine());

				//process
				Logging.LogBoard(true, packet.GetBoard());
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
		static void NotCloseThread()
		{
			while (true) { Thread.Sleep(100000000); }
		}
		static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			// Обработка исключений на глобальном уровне
			Exception ex = (Exception)e.ExceptionObject;


			Console.WriteLine("Глобальный обработчик исключений перехватил исключение: " + ex.Message);
			// Можно выполнить дополнительную обработку или логирование здесь
			try
			{
				if (!File.Exists("Errors.txt")) File.Create("Errors.txt");
				File.AppendAllText("Errors.txt", "\n-------------------------------------------------------------\n" + ex.Message);
			}
			catch (Exception)
			{
				Console.WriteLine("Error while logging error (how)");
			}
		}
	}
}