using System.Xml;

namespace SChess
{
	class User
	{
		public string id;
		public string username;
		public bool conected;
		XmlNodeList? figures;
		public User(string id, string username)
		{
			this.id = id;
			this.username = username;
			this.conected = false;
		}
		public void setfigures(ref XmlNodeList figures)
		{
			this.figures = figures;
		}
	}
}
