using Raylib_cs;
using static Raylib_cs.Raylib;
namespace RChess
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			InitWindow(Game.chessboard_width * 8+200, Game.chessboard_width * 8+200, "Schache 2D");
			SetTargetFPS(15);
			game.generatePieces();
			while (!WindowShouldClose())
			{
				if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) game.handleClick();
				BeginDrawing();
				ClearBackground(Color.WHITE);
				game.drawChessboard();
				game.drawPieces();
				game.drawMove();
				game.drawCoordinates();
				//game.drawBoardNum();
				EndDrawing();
			}
			CloseWindow();
		}
	}
}