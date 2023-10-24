namespace RChess
{
	using Raylib_cs;
	using System.Numerics;
	using static Raylib_cs.Raylib;
	class Game
	{
		public Game() { }
		public static byte[,] board = {
			{2,2,2,2,2,2,2,2},
			{2,2,2,2,2,2,2,2},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0}
		};
		public static int chessboard_width = 75;
		public static Rectangle boardrect = new(100, 100, chessboard_width * 8, chessboard_width * 8);
		public static int selected_piece = -1;
		public static List<Vector2> move = new List<Vector2>();
		static List<Figure> figuresF = new List<Figure>();
		static List<FigureE> figuresE = new List<FigureE>();
		//-----------get-----------------------------------
		public static int getboard(float x, float y)
		{
			if (y >= 0 && x >= 0 && y <= 7 && x <= 7) return Game.board[(int)y, (int)x];
			return -1;
		}
		public static Vector2 getmouseV()
		{
			Vector2 v = GetMousePosition();
			if (CheckCollisionPointRec(v, Game.boardrect))
			{
				v.X = (int)(v.X - boardrect.x) / chessboard_width; if (v.X > 7) v.X = 7; if (v.X < 0) v.X = 0;
				v.Y = (int)(v.Y - boardrect.y) / chessboard_width; if (v.Y > 7) v.Y = 7; if (v.Y < 0) v.Y = 0;
				return v;
			}
			else
			{
				v.X = -1;
				v.Y = -1;
				return v;
			}
		}
		//-----------other---------------------------------
		public static void generatePieces()
		{
			figuresF.Clear();
			figuresF.Add(new Figure(FigureType.Rook, 0, 7));
			figuresF.Add(new Figure(FigureType.Knight, 1, 7));
			figuresF.Add(new Figure(FigureType.Bishop, 2, 7));
			figuresF.Add(new Figure(FigureType.Queen, 3, 7));
			figuresF.Add(new Figure(FigureType.King, 4, 7));
			figuresF.Add(new Figure(FigureType.Bishop, 5, 7));
			figuresF.Add(new Figure(FigureType.Knight, 6, 7));
			figuresF.Add(new Figure(FigureType.Rook, 7, 7));
			figuresF.Add(new Figure(FigureType.Pawn, 0, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 1, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 2, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 3, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 4, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 5, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 6, 6));
			figuresF.Add(new Figure(FigureType.Pawn, 7, 6));


			figuresE.Clear();
			figuresE.Add(new FigureE(FigureType.Rook, 0, 0));
			figuresE.Add(new FigureE(FigureType.Knight, 1, 0));
			figuresE.Add(new FigureE(FigureType.Bishop, 2, 0));
			figuresE.Add(new FigureE(FigureType.King, 3, 0));
			figuresE.Add(new FigureE(FigureType.Queen, 4, 0));
			figuresE.Add(new FigureE(FigureType.Bishop, 5, 0));
			figuresE.Add(new FigureE(FigureType.Knight, 6, 0));
			figuresE.Add(new FigureE(FigureType.Pawn, 7, 0));
			figuresE.Add(new FigureE(FigureType.Pawn, 0, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 1, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 2, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 3, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 4, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 5, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 6, 1));
			figuresE.Add(new FigureE(FigureType.Pawn, 7, 1));
		}
		public static void handleClick()
		{
			Vector2 click = getmouseV();
			if (move.Contains(click))
			{
				move.Clear();
				figuresF[selected_piece].moveTo(click);
				selected_piece = -1;
				return;
			};
			for (int p = 0; p < figuresF.Count; p++)
			{
				if ((int)click.X == figuresF[p].x && (int)click.Y == figuresF[p].y)
				{
					figuresF[p].getMove(ref move);
					selected_piece = p;
					return;
				}
			}
		}
		//-----------draw----------------------------------
		public static void drawMove()
		{

			for (int m = 0; m < move.Count; m++)
			{
				if (Game.board[(int)move[m].Y, (int)move[m].X] == 1)
				{
					DrawCircle(
						(int)(move[m].X * chessboard_width + chessboard_width / 2 + boardrect.x),
						(int)(move[m].Y * chessboard_width + chessboard_width / 2 + boardrect.y),
						chessboard_width / 5f,
						new(0, 0, 0, 50));
				}
				else if (Game.board[(int)move[m].Y, (int)move[m].X] == 2)
				{
					DrawRing(
						new(
							(int)(move[m].X * chessboard_width + chessboard_width / 2 + boardrect.x),
							(int)(move[m].Y * chessboard_width + chessboard_width / 2 + boardrect.y)
							),
						chessboard_width / 2.5f, chessboard_width / 2, 0, 360, 36, new(0, 0, 0, 50)
						);
				}
				else
				{
					move.RemoveAt(m);
					//DrawCircle(
					//	(int)(move[m].X * chessboard_width + chessboard_width / 2 + boardrect.x),
					//	(int)(move[m].Y * chessboard_width + chessboard_width / 2 + boardrect.y),
					//	chessboard_width / 5f,
					//	new(255, 0, 0, 255));
				}
			}
		}
		public static void drawChessboard()
		{
			Vector2 v = getmouseV();

			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					DrawRectangle(
						x * chessboard_width + (int)boardrect.x,
						y * chessboard_width + (int)boardrect.y,
						chessboard_width,
						chessboard_width,
						((int)v.X == x && (int)v.Y == y) ? (y % 2 == x % 2) ? new Color(189, 203, 67, 255) : new Color(244, 246, 126, 255) : (y % 2 == x % 2) ? new Color(121, 151, 81, 255) : new Color(232, 236, 201, 255)
						);
				}
			}
		}
		public static void drawPieces()
		{
			for (int f = 0; f < figuresF.Count; f++)
			{
				figuresF[f].draw();
			}
			for (int e = 0; e < figuresE.Count; e++)
			{
				figuresE[e].draw();
			}
		}
		public static void drawBoardNum()
		{
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					DrawText(board[(int)y, (int)x].ToString(), x * chessboard_width + chessboard_width / 2 + (int)boardrect.x, y * chessboard_width + chessboard_width / 2 + (int)boardrect.y, 20, Color.RED); ;
				}
			}
		}
		public static void drawCoordinates()
		{
			string b = "abcdefgh";
			for (int n = 0; n < 8; n++)
			{
				DrawText((8 - n).ToString(), 7 + (int)boardrect.x, n * chessboard_width + 10 + (int)boardrect.y, 20, (n % 2 == 1) ? new Color(121, 151, 81, 255) : new Color(232, 236, 201, 255));
			}
			for (int n = 0; n < 8; n++)
			{
				DrawText(b[n].ToString(), n * chessboard_width + (int)(chessboard_width * 0.6) + 10 + (int)boardrect.x, (int)(chessboard_width * 7.7) + (int)boardrect.y, 20, (n % 2 == 0) ? new Color(121, 151, 81, 255) : new Color(232, 236, 201, 255));
			}
		}
		public static void drawAll()
		{
			BeginDrawing();
			DrawFPS(0, 0);
			ClearBackground(new(48, 46, 43, 255));
			drawChessboard();
			drawPieces();
			drawMove();
			drawCoordinates();
			EndDrawing();

			//drawBoardNum();
		}
	}
}