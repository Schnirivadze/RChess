namespace RChess
{
	using Raylib_cs;
	using System.Numerics;
	using static Raylib_cs.Raylib;
	using static Raylib_cs.Color;
	public enum FigureType
	{
		King,
		Queen,
		Bishop,
		Knight,
		Rook,
		Pawn
	}
	class Figure
	{
		static Texture2D[] textures = {
			LoadTexture("pieces\\white-king.png"),
			LoadTexture("pieces\\white-queen.png"),
			LoadTexture("pieces\\white-bishop.png"),
			LoadTexture("pieces\\white-knight.png"),
			LoadTexture("pieces\\white-rook.png"),
			LoadTexture("pieces\\white-pawn.png"),
		};
		public FigureType type;
		public int x, y;

		public Figure(FigureType type, int x, int y)
		{
			this.type = type;
			this.y = y;
			this.x = x;
		}
		public void draw()
		{
			DrawTextureEx(textures[(int)type], new(x * Game.chessboard_width, y * Game.chessboard_width), 0, 0.5859375f, WHITE);
		}
		public void getMove(ref List<Vector2> move)
		{
			int rad = 1;
			bool cont = false;
			switch (type)
			{
				case FigureType.King:
					if (Game.getboard(x - 1, y - 1) > 0) move.Add(new(x - 1, y - 1));
					if (Game.getboard(x + 1, y - 1) > 0) move.Add(new(x + 1, y - 1));
					if (Game.getboard(x - 1, y + 1) > 0) move.Add(new(x - 1, y + 1));
					if (Game.getboard(x + 1, y + 1) > 0) move.Add(new(x + 1, y + 1));
					if (Game.getboard(x, y - 1) > 0) move.Add(new(x, y - 1));
					if (Game.getboard(x, y + 1) > 0) move.Add(new(x, y + 1));
					if (Game.getboard(x - 1, y) > 0) move.Add(new(x - 1, y));
					if (Game.getboard(x + 1, y) > 0) move.Add(new(x + 1, y));
					break;
				case FigureType.Queen:
					bool[] dir = { true, true, true, true, true, true, true, true };
					while (true)
					{
						cont = false;
						if (dir[0] && Game.getboard(x - rad, y - rad) > 0) { if (Game.getboard(x - rad, y - rad) == 2) { dir[0] = false; } move.Add(new(x - rad, y - rad)); cont = true; } else { dir[0] = false; }
						if (dir[1] && Game.getboard(x + rad, y - rad) > 0) { if (Game.getboard(x + rad, y - rad) == 2) { dir[1] = false; } move.Add(new(x + rad, y - rad)); cont = true; } else { dir[1] = false; }
						if (dir[2] && Game.getboard(x - rad, y + rad) > 0) { if (Game.getboard(x - rad, y + rad) == 2) { dir[2] = false; } move.Add(new(x - rad, y + rad)); cont = true; } else { dir[2] = false; }
						if (dir[3] && Game.getboard(x + rad, y + rad) > 0) { if (Game.getboard(x + rad, y + rad) == 2) { dir[3] = false; } move.Add(new(x + rad, y + rad)); cont = true; } else { dir[3] = false; }
						if (dir[4] && Game.getboard(x, y - rad) > 0) { if (Game.getboard(x, y - rad) == 2) { dir[4] = false; } move.Add(new(x, y - rad)); cont = true; } else { dir[4] = false; }
						if (dir[5] && Game.getboard(x, y + rad) > 0) { if (Game.getboard(x, y + rad) == 2) { dir[5] = false; } move.Add(new(x, y + rad)); cont = true; } else { dir[5] = false; }
						if (dir[6] && Game.getboard(x - rad, y) > 0) { if (Game.getboard(x - rad, y) == 2) { dir[6] = false; } move.Add(new(x - rad, y)); cont = true; } else { dir[6] = false; }
						if (dir[7] && Game.getboard(x + rad, y) > 0) { if (Game.getboard(x + rad, y) == 2) { dir[7] = false; } move.Add(new(x + rad, y)); cont = true; } else { dir[7] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Bishop:
					bool[] dirB = { true, true, true, true };
					while (true)
					{
						cont = false;
						if (dirB[0] && Game.getboard(x - rad, y - rad) > 0) { if (Game.getboard(x - rad, y - rad) == 2) { dirB[0] = false; } move.Add(new(x - rad, y - rad)); cont = true; } else { dirB[0] = false; }
						if (dirB[1] && Game.getboard(x + rad, y - rad) > 0) { if (Game.getboard(x + rad, y - rad) == 2) { dirB[1] = false; } move.Add(new(x + rad, y - rad)); cont = true; } else { dirB[1] = false; }
						if (dirB[2] && Game.getboard(x - rad, y + rad) > 0) { if (Game.getboard(x - rad, y + rad) == 2) { dirB[2] = false; } move.Add(new(x - rad, y + rad)); cont = true; } else { dirB[2] = false; }
						if (dirB[3] && Game.getboard(x + rad, y + rad) > 0) { if (Game.getboard(x + rad, y + rad) == 2) { dirB[3] = false; } move.Add(new(x + rad, y + rad)); cont = true; } else { dirB[3] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Knight:
					if (Game.getboard(x - 1, y - 2) > 0) { move.Add(new(x - 1, y - 2)); }
					if (Game.getboard(x + 1, y - 2) > 0) { move.Add(new(x + 1, y - 2)); }
					if (Game.getboard(x - 2, y - 1) > 0) { move.Add(new(x - 2, y - 1)); }
					if (Game.getboard(x + 2, y - 1) > 0) { move.Add(new(x + 2, y - 1)); }
					if (Game.getboard(x - 2, y + 1) > 0) { move.Add(new(x - 2, y + 1)); }
					if (Game.getboard(x + 2, y + 1) > 0) { move.Add(new(x + 2, y + 1)); }
					if (Game.getboard(x - 1, y + 2) > 0) { move.Add(new(x - 1, y + 2)); }
					if (Game.getboard(x + 1, y + 2) > 0) { move.Add(new(x + 1, y + 2)); }
					break;
				case FigureType.Rook:
					bool[] dirR = { true, true, true, true };
					while (true)
					{
						cont = false;
						if (dirR[0] && Game.getboard(x, y - rad) > 0) { if (Game.getboard(x, y - rad) == 2) { dirR[0] = false; } move.Add(new(x, y - rad)); cont = true; } else { dirR[0] = false; }
						if (dirR[1] && Game.getboard(x, y + rad) > 0) { if (Game.getboard(x, y + rad) == 2) { dirR[1] = false; } move.Add(new(x, y + rad)); cont = true; } else { dirR[1] = false; }
						if (dirR[2] && Game.getboard(x - rad, y) > 0) { if (Game.getboard(x - rad, y) == 2) { dirR[2] = false; } move.Add(new(x - rad, y)); cont = true; } else { dirR[2] = false; }
						if (dirR[3] && Game.getboard(x + rad, y) > 0) { if (Game.getboard(x + rad, y) == 2) { dirR[3] = false; } move.Add(new(x + rad, y)); cont = true; } else { dirR[3] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Pawn:

					if (Game.getboard(x, y - 1) == 1) move.Add(new(x, y - 1));
					if (Game.getboard(x, y - 2) == 1) move.Add(new(x, y - 2));
					if (Game.getboard(x - 1, y - 1) == 2) move.Add(new(x - 1, y - 1));
					if (Game.getboard(x + 1, y - 1) == 2) move.Add(new(x + 1, y - 1));
					break;
				default:
					break;
			}

		}
		public void moveTo(Vector2 pos)
		{
			Game.board[y, x] = 1;
			x = (int)pos.X;
			y = (int)pos.Y;
			Game.board[y, x] = 0;

		}

	}
}
