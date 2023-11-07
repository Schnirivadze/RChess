namespace RChess
{
	using Raylib_cs;
	using System.Numerics;
	using static Raylib_cs.Color;
	using static Raylib_cs.Raylib;
	class Figure
	{

		static readonly Texture2D[] textures = {
			LoadTexture("pieces\\white-king.png"),
			LoadTexture("pieces\\white-queen.png"),
			LoadTexture("pieces\\white-bishop.png"),
			LoadTexture("pieces\\white-knight.png"),
			LoadTexture("pieces\\white-rook.png"),
			LoadTexture("pieces\\white-pawn.png"),
		};
		public FigureType type;
		public Vector2 pos;
		public bool moved = false;
		public Figure(FigureType type, int x, int y)
		{
			this.type = type;
			pos = new(x, y);
		}
		public void draw()
		{
			DrawTextureEx(textures[(int)type], new(pos.X * Game.chessboard_width + Game.boardrect.x, pos.Y * Game.chessboard_width + Game.boardrect.y), 0, Game.chessboard_width / 128f, WHITE);
		}
		public void getMove(ref List<Vector2> move)
		{
			int rad = 1;
			bool cont = false;
			switch (type)
			{
				case FigureType.King:
					if (Game.getboard(pos.X - 1, pos.Y - 1) > 0) move.Add(new(pos.X - 1, pos.Y - 1));
					if (Game.getboard(pos.X + 1, pos.Y - 1) > 0) move.Add(new(pos.X + 1, pos.Y - 1));
					if (Game.getboard(pos.X - 1, pos.Y + 1) > 0) move.Add(new(pos.X - 1, pos.Y + 1));
					if (Game.getboard(pos.X + 1, pos.Y + 1) > 0) move.Add(new(pos.X + 1, pos.Y + 1));
					if (Game.getboard(pos.X, pos.Y - 1) > 0) move.Add(new(pos.X, pos.Y - 1));
					if (Game.getboard(pos.X, pos.Y + 1) > 0) move.Add(new(pos.X, pos.Y + 1));
					if (Game.getboard(pos.X - 1, pos.Y) > 0) move.Add(new(pos.X - 1, pos.Y));
					if (Game.getboard(pos.X + 1, pos.Y) > 0) move.Add(new(pos.X + 1, pos.Y));
					break;
				case FigureType.Queen:
					bool[] dir = { true, true, true, true, true, true, true, true };
					while (true)
					{
						cont = false;
						if (dir[0] && Game.getboard(pos.X - rad, pos.Y - rad) > 0) { if (Game.getboard(pos.X - rad, pos.Y - rad) == 2) { dir[0] = false; } move.Add(new(pos.X - rad, pos.Y - rad)); cont = true; } else { dir[0] = false; }
						if (dir[1] && Game.getboard(pos.X + rad, pos.Y - rad) > 0) { if (Game.getboard(pos.X + rad, pos.Y - rad) == 2) { dir[1] = false; } move.Add(new(pos.X + rad, pos.Y - rad)); cont = true; } else { dir[1] = false; }
						if (dir[2] && Game.getboard(pos.X - rad, pos.Y + rad) > 0) { if (Game.getboard(pos.X - rad, pos.Y + rad) == 2) { dir[2] = false; } move.Add(new(pos.X - rad, pos.Y + rad)); cont = true; } else { dir[2] = false; }
						if (dir[3] && Game.getboard(pos.X + rad, pos.Y + rad) > 0) { if (Game.getboard(pos.X + rad, pos.Y + rad) == 2) { dir[3] = false; } move.Add(new(pos.X + rad, pos.Y + rad)); cont = true; } else { dir[3] = false; }
						if (dir[4] && Game.getboard(pos.X, pos.Y - rad) > 0) { if (Game.getboard(pos.X, pos.Y - rad) == 2) { dir[4] = false; } move.Add(new(pos.X, pos.Y - rad)); cont = true; } else { dir[4] = false; }
						if (dir[5] && Game.getboard(pos.X, pos.Y + rad) > 0) { if (Game.getboard(pos.X, pos.Y + rad) == 2) { dir[5] = false; } move.Add(new(pos.X, pos.Y + rad)); cont = true; } else { dir[5] = false; }
						if (dir[6] && Game.getboard(pos.X - rad, pos.Y) > 0) { if (Game.getboard(pos.X - rad, pos.Y) == 2) { dir[6] = false; } move.Add(new(pos.X - rad, pos.Y)); cont = true; } else { dir[6] = false; }
						if (dir[7] && Game.getboard(pos.X + rad, pos.Y) > 0) { if (Game.getboard(pos.X + rad, pos.Y) == 2) { dir[7] = false; } move.Add(new(pos.X + rad, pos.Y)); cont = true; } else { dir[7] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Bishop:
					bool[] dirB = { true, true, true, true };
					while (true)
					{
						cont = false;
						if (dirB[0] && Game.getboard(pos.X - rad, pos.Y - rad) > 0) { if (Game.getboard(pos.X - rad, pos.Y - rad) == 2) { dirB[0] = false; } move.Add(new(pos.X - rad, pos.Y - rad)); cont = true; } else { dirB[0] = false; }
						if (dirB[1] && Game.getboard(pos.X + rad, pos.Y - rad) > 0) { if (Game.getboard(pos.X + rad, pos.Y - rad) == 2) { dirB[1] = false; } move.Add(new(pos.X + rad, pos.Y - rad)); cont = true; } else { dirB[1] = false; }
						if (dirB[2] && Game.getboard(pos.X - rad, pos.Y + rad) > 0) { if (Game.getboard(pos.X - rad, pos.Y + rad) == 2) { dirB[2] = false; } move.Add(new(pos.X - rad, pos.Y + rad)); cont = true; } else { dirB[2] = false; }
						if (dirB[3] && Game.getboard(pos.X + rad, pos.Y + rad) > 0) { if (Game.getboard(pos.X + rad, pos.Y + rad) == 2) { dirB[3] = false; } move.Add(new(pos.X + rad, pos.Y + rad)); cont = true; } else { dirB[3] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Knight:
					if (Game.getboard(pos.X - 1, pos.Y - 2) > 0) { move.Add(new(pos.X - 1, pos.Y - 2)); }
					if (Game.getboard(pos.X + 1, pos.Y - 2) > 0) { move.Add(new(pos.X + 1, pos.Y - 2)); }
					if (Game.getboard(pos.X - 2, pos.Y - 1) > 0) { move.Add(new(pos.X - 2, pos.Y - 1)); }
					if (Game.getboard(pos.X + 2, pos.Y - 1) > 0) { move.Add(new(pos.X + 2, pos.Y - 1)); }
					if (Game.getboard(pos.X - 2, pos.Y + 1) > 0) { move.Add(new(pos.X - 2, pos.Y + 1)); }
					if (Game.getboard(pos.X + 2, pos.Y + 1) > 0) { move.Add(new(pos.X + 2, pos.Y + 1)); }
					if (Game.getboard(pos.X - 1, pos.Y + 2) > 0) { move.Add(new(pos.X - 1, pos.Y + 2)); }
					if (Game.getboard(pos.X + 1, pos.Y + 2) > 0) { move.Add(new(pos.X + 1, pos.Y + 2)); }
					break;
				case FigureType.Rook:
					bool[] dirR = { true, true, true, true };
					while (true)
					{
						cont = false;
						if (dirR[0] && Game.getboard(pos.X, pos.Y - rad) > 0) { if (Game.getboard(pos.X, pos.Y - rad) == 2) { dirR[0] = false; } move.Add(new(pos.X, pos.Y - rad)); cont = true; } else { dirR[0] = false; }
						if (dirR[1] && Game.getboard(pos.X, pos.Y + rad) > 0) { if (Game.getboard(pos.X, pos.Y + rad) == 2) { dirR[1] = false; } move.Add(new(pos.X, pos.Y + rad)); cont = true; } else { dirR[1] = false; }
						if (dirR[2] && Game.getboard(pos.X - rad, pos.Y) > 0) { if (Game.getboard(pos.X - rad, pos.Y) == 2) { dirR[2] = false; } move.Add(new(pos.X - rad, pos.Y)); cont = true; } else { dirR[2] = false; }
						if (dirR[3] && Game.getboard(pos.X + rad, pos.Y) > 0) { if (Game.getboard(pos.X + rad, pos.Y) == 2) { dirR[3] = false; } move.Add(new(pos.X + rad, pos.Y)); cont = true; } else { dirR[3] = false; }
						if (!cont) break;
						rad++;
					}
					break;
				case FigureType.Pawn:

					if (Game.getboard(pos.X, pos.Y - 1) == 1)
					{
						move.Add(new(pos.X, pos.Y - 1));
						if (!moved && Game.getboard(pos.X, pos.Y - 2) == 1) move.Add(new(pos.X, pos.Y - 2));
					}
					if (Game.getboard(pos.X - 1, pos.Y - 1) == 2) move.Add(new(pos.X - 1, pos.Y - 1));
					if (Game.getboard(pos.X + 1, pos.Y - 1) == 2) move.Add(new(pos.X + 1, pos.Y - 1));
					break;
				default:
					break;
			}

		}
		public void moveTo(Vector2 movement)
		{
			Game.move.Clear();
			float ammount_of_frames = Settings.PieceMovementTime / (1f / GetFPS());
			Vector2 dist = Vector2.Divide(Vector2.Subtract(movement, pos), ammount_of_frames);
			for (float frames = 0; frames < ammount_of_frames; frames++)
			{
				pos.X += dist.X;
				pos.Y += dist.Y;
				Game.drawAll();
			}
			pos = movement;
		}

	}
}
