using Raylib_cs;
using static Raylib_cs.Raylib;
namespace RChess
{
	class Program
	{
		static void Main()
		{
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
	}
}