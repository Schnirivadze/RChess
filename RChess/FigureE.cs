using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
namespace RChess
{
	internal class FigureE
	{
		public int x, y;
		FigureType type;
		static Texture2D[] textures = {
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
			this.x = x;
			this.y = y;
		}
		public void draw()
		{
			DrawTextureEx(textures[(int)type], new(x * Game.chessboard_width + Game.boardrect.x, y * Game.chessboard_width + Game.boardrect.y), 0, 75f / 128f, WHITE);
		}

	}
}
