#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.Xml;
namespace Shared
{
	public class Figure
	{

		public FigureType type;
		public int x, y;
		public Figure(FigureType type, int x, int y)
		{
			this.type = type;
			this.x = x;
			this.y = y;
		}
		public Figure(ref XmlNode fromnode)
		{
			type = (FigureType)Convert.ToInt32(fromnode.Attributes["type"].Value);
			x = Convert.ToInt32(fromnode.Attributes["x"].Value);
			y = Convert.ToInt32(fromnode.Attributes["y"].Value);
		}
	}
	public class Packet
	{
		public XmlDocument doc;
		List<XmlNode> infos;
		List<XmlNode> figurelist;
		XmlNodeList board;
		//====================================================CONSTRUCTORS
		public Packet()
		{
			doc = new XmlDocument();
			infos = new();
			figurelist = new();
		}
		public Packet(string xmlstring)
		{
			doc = new XmlDocument();
			doc.LoadXml(xmlstring);
			infos = new List<XmlNode>();
			XmlNodeList nodes = doc.GetElementsByTagName("info");
			for (int i = 0; i < nodes.Count; i++)
			{
				infos.Add(nodes[i]);
			}
		}
		//====================================================SETTERS
		public Packet AddInfo(InfoType type, string value)
		{
			XmlElement info = doc.CreateElement("info");
			info.SetAttribute("type", ((int)type).ToString());
			info.InnerText = value;
			infos.Add(info);
			return this;
		}
		public Packet AddFigure(FigureType type,float x, float y)
		{
			XmlElement figure = doc.CreateElement("figure");
			figure.SetAttribute("type", ((int)type).ToString());
			figure.SetAttribute("x", ((int)x).ToString());
			figure.SetAttribute("y", ((int)y).ToString());
			figurelist.Add(figure);
			return this;
		}
		public Packet SetBoard(XmlNodeList board)
		{
			this.board=board;
			return this;
		}
		//====================================================GETTERS
		public string GetInfo(InfoType type)
		{
			foreach (XmlNode node in infos)
			{
				if (node.Attributes["type"].Value == ((int)type).ToString())
				{
					return node.InnerText;
				}
			}
			return "";
		}
		public XmlNodeList GetBoard()
		{
			return doc.GetElementsByTagName("figure");
		}
		//====================================================CHECKERS
		public bool HasBoard()
		{
			return doc.GetElementsByTagName("figure").Count > 0;
			//====================================================CONSTRUCTORS
		}
		//====================================================OTHER
		public void Assemble()
		{
			XmlElement data = doc.CreateElement("data");
			for (int x = 0; x < infos.Count; x++)
			{
				data.AppendChild(infos[x]);
			}
			for (int x = 0; x < figurelist.Count; x++)
			{
				data.AppendChild(figurelist[x]);
			}
			if (board != null)
			{
				for (int x = 0; x < board.Count; x++)
				{
					XmlNode nodeCopy = data.OwnerDocument.ImportNode(board[x], true);

					// Вставляем копию узла в другой документ
					data.AppendChild(nodeCopy);
				}
			}
			
			doc.AppendChild(data);
		}
		public override string ToString()
		{
			Logging.Log(Logging.LogType.Debug, doc.OuterXml);
			return doc.OuterXml;
		}
	}
}
