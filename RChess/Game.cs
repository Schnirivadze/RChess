﻿namespace RChess
{
	using Raylib_cs;
	using System.Collections.Generic;
	using System.Numerics;
	using static Raylib_cs.Raylib;
	class Game
	{
		public Game() { }
		public static int chessboard_width = 90;
		public static Rectangle boardrect = new(100, 100, chessboard_width * 8, chessboard_width * 8);
		public static int selected_piece = -1;
		public static List<Vector2> move = new();
		public static readonly List<Figure> figuresF = new();
		public static List<FigureE> figuresE = new();
		//-----------get-----------------------------------
		public static int getboard(float x, float y)
		{
			if (y >= 0 && x >= 0 && y <= 7 && x <= 7)
			{
				if (figuresF.Where(ff => ff.pos.X == x && ff.pos.Y == y).Any()) return 0;
				else if (figuresE.Where(ff => ff.pos.X == x && ff.pos.Y == y).Any()) return 2;
				else return 1;
			}
			else return -1;
		}
		public static int getboard(Vector2 coor)
		{
			if (coor.Y >= 0 && coor.X >= 0 && coor.Y <= 7 && coor.X <= 7)
			{
				if (figuresF.Where(ff => ff.pos == coor).Any()) return 0;
				else if (figuresE.Where(ff => ff.pos == coor).Any()) return 2;
				else return 1;
			}
			else return -1;
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
				int fi = figuresE.FindIndex(f => f.pos == click);
				move.Clear();
				figuresF[selected_piece].moveTo(click);
				figuresF[selected_piece].moved = true;
				if (fi != -1) figuresE.RemoveAt(fi);
				selected_piece = -1;
				return;
			};
			move.Clear();
			for (int p = 0; p < figuresF.Count; p++)
			{
				if (click == figuresF[p].pos)
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
				if (getboard(move[m]) == 1)
				{
					DrawCircle(
						(int)(move[m].X * chessboard_width + chessboard_width / 2 + boardrect.x),
						(int)(move[m].Y * chessboard_width + chessboard_width / 2 + boardrect.y),
						chessboard_width / 5f,
						new(0, 0, 0, 50));
				}
				else if (getboard(move[m]) == 2)
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
			for (int e = 0; e < figuresE.Count; e++)
			{
				figuresE[e].draw();
			}
			for (int f = 0; f < figuresF.Count; f++)
			{
				figuresF[f].draw();
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
		}
	}
}