using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
namespace RChess
{
	internal class FigureE
	{
		public Vector2 pos;
		readonly FigureType type;
		readonly static Texture2D[] textures = {
			LoadTexture("pieces\\black-king.png"),
			LoadTexture("pieces\\black-queen.png"),
			LoadTexture("pieces\\black-bishop.png"),
			LoadTexture("pieces\\black-knight.png"),
			LoadTexture("pieces\\black-rook.png"),
			LoadTexture("pieces\\black-pawn.png"),
		};
		public FigureE(FigureType type, int x, int y)
		{
			this.type = type;
			pos = new Vector2(x, y);
		}
		public void draw()
		{
			DrawTextureEx(textures[(int)type], new(pos.X * Game.chessboard_width + Game.boardrect.x, pos.Y * Game.chessboard_width + Game.boardrect.y), 0, Game.chessboard_width / 128f, WHITE);
		}

	}
}
