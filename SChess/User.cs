using System.Xml;

namespace SChess
{
	class User
	{
		public string username;
		public bool conected;
		public XmlNodeList? figures;
		public User(string username)
		{
			this.username = username;
			this.conected = false;
		}
		
	}
}
