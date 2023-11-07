#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Xml;

namespace RChess
{
	class Logging
	{
		static string pies = "♔♕♗♘♖♙";

		public static void LogBoard(bool inorout, XmlNodeList board)
		{
			string path = "outboards.txt";
			if (inorout) path = "inboards.txt";
			char[,] boards =
				{
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
					{'·','·','·','·','·','·','·','·' },
				};
			string ps = "\n------------------------\n";
			foreach (XmlNode figure_node in board)
			{
				int e_figure_y = Convert.ToInt32(figure_node.Attributes["y"].Value);
				int e_figure_x = Convert.ToInt32(figure_node.Attributes["x"].Value);
				int e_figure_type = Convert.ToInt32(figure_node.Attributes["type"].Value);
				boards[e_figure_y, e_figure_x] = pies[e_figure_type];
			}
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					ps += boards[y, x];
				}
				ps += "\n";
			}
			File.AppendAllText(path, ps);

		}
	}
}
