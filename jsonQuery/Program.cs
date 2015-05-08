using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace jsonQuery
{
	class Program
	{
		static string DATA_FILE = "../../data.json";

		static void Main(string[] args)
		{
			var path = "/people/001";

			JsonNode data = new JsonNode(DATA_FILE);

			var entity = data.Get(path).Value();

			data.Post("/favorites/people/real", entity);

			Console.WriteLine("\n\n\nPress any key to exit.");
			Console.ReadKey(true);
		}
	}

	class JsonNode
	{
		private Type OBJ_TYPE = typeof(Dictionary<string, object>);
		private Type ARR_TYPE = typeof(ArrayList);
		private static JavaScriptSerializer JSS = new JavaScriptSerializer();
		private dynamic _jsonObj;
		private string _jsonObjKey;
		private dynamic _parentObj;
		private string _filename;
		private bool _autoSave;

		public JsonNode(string filename, bool autoSave = true)
		{
			_filename = filename;
			_autoSave = autoSave;
			_jsonObj = JSS.Deserialize<Dictionary<string, dynamic>>(File.ReadAllText(filename));
			_jsonObjKey = null;
			_parentObj = null;
		}
		private JsonNode(ref dynamic obj, string key, dynamic parent, string filename, bool autoSave)
		{
			_filename = filename;
			_autoSave = autoSave;
			_jsonObj = obj;
			_jsonObjKey = key;
			_parentObj = parent;
		}

		public void Save()
		{
			File.WriteAllText(
				_filename, 
				JsonFormatter.Beautify(
					JSS.Serialize(_jsonObj)
				)
			);
		}

		public string AsJson()
		{
			return JSS.Serialize(_jsonObj);
		}

		public dynamic Value()
		{
			return _jsonObj;
		}

		public JsonNode Delete(string path)
		{
			JsonPath jPath = new JsonPath(path);
			JsonNode node = read(jPath.PathParts);
			dynamic parentNode = node.Parent();
			Type parentNodeType = parentNode.GetType();
			if (parentNodeType == OBJ_TYPE)
			{
				((IDictionary<string, dynamic>)parentNode.Value()).Remove(jPath.Key);
			}
			else if (parentNodeType == ARR_TYPE)
			{
				((ArrayList)parentNode.Value()).Remove(node.Value());
			}
			else
			{
				throw new InvalidOperationException("Deleting node of unknown parent node type is unsupported; parent node type: " + parentNodeType.ToString());
			}
			Save();
			return node;
		}

		public JsonNode Post(string path, dynamic entity)
		{
			string key = Guid.NewGuid().ToString();
			JsonPath jPath = new JsonPath(path);
			JsonNode node = read(jPath.PathParts, true);
			node.Value()[key] = entity;
			Save();
			return node.Get(key);
		}

		public JsonNode Put(string path, dynamic entity)
		{
			JsonPath jPath = new JsonPath(path);
			Save();
			return null;
		}

		public JsonNode Get(string path)
		{
			JsonPath jPath = new JsonPath(path);
			return read(jPath.PathParts, false);
		}

		public dynamic Parent()
		{
			return _parentObj;
		}

		private JsonNode read(List<string> pathParts, bool create = false)
		{
			if (create && _jsonObj == null) _jsonObj = new Dictionary<string, dynamic>();
			dynamic node = _jsonObj;
			string nodeKey = _jsonObjKey;
			dynamic parentObj = _parentObj;

			foreach (string key in pathParts)
			{
				if (node == null) return null;
				Type nodeType = node.GetType();
				if (nodeType == ARR_TYPE)
				{
					int index;
					if (int.TryParse(key, out index))
					{
						if (index >= 0 && index < ((ArrayList)node).Count)
						{
							parentObj = node;
							node = node[index];
							nodeKey = key;
							continue;
						}
						else if (!create)
						{
							throw new IndexOutOfRangeException("Index '" + index + "' is out of range for array of length '" + ((ArrayList)node).Count);
						}
					}
					else if (!create)
					{
						throw new InvalidOperationException("Index invalid for array: '" + key + "'.");
					}
				}
				if (nodeType != OBJ_TYPE)
				{
					if (parentObj == null)
					{
						node = _jsonObj = new Dictionary<string, dynamic>();
					}
					else
					{
						parentObj[nodeKey] = node = new Dictionary<string, dynamic>();
					}
				}
				if (!((IDictionary<string, dynamic>)node).ContainsKey(key))
				{
					if (create)
					{
						node[key] = new Dictionary<string, dynamic>();
					}
					else
					{
						return null;
					}
				}
				parentObj = node;
				node = node[key];
				nodeKey = key;
			}
			return new JsonNode(ref node, nodeKey, parentObj, _filename, _autoSave);
		}

		class JsonPath
		{
			public string Key;
			public List<string> PathParts;
			public List<string> ParentPathParts;

			public JsonPath(string path)
			{
				Key = path.Substring(path.LastIndexOf('/') + 1);
				PathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
				ParentPathParts = PathParts.Count > 1 ? PathParts.GetRange(0, this.PathParts.Count - 1) : new List<string>();
			}
		}

		class JsonFormatter
		{
			class Parser
			{
				int cursor;
				string _content;

				public Parser(string json)
				{
					cursor = 0;
					_content = json;
				}

				public bool hasNext()
				{
					return (cursor < _content.Length);
				}
				public char next()
				{
					return _content[cursor++];
				}
				public char last()
				{
					return _content[cursor - 2];
				}
				public void insertAfter(string str, bool skip = true)
				{
					_content = _content.Insert(cursor, str);
					if (skip) cursor += str.Length;
				}
				public void insertBefore(string str, bool skip = true)
				{
					_content = _content.Insert(cursor - 1, str);
					if (skip) cursor += str.Length;
				}
				public string getContent()
				{
					return _content;
				}
			}

			public static string Beautify(string json)
			{
				int depth = 0;
				bool insideQuotes = false;
				Parser parser = new Parser(json);
				while (parser.hasNext())
				{
					char c = parser.next();
					if (c == '"' && parser.last() != '\\')
					{
						insideQuotes = !insideQuotes;
					}
					if (c == '[' || c == '{')
					{
						depth++;
						parser.insertAfter("\n" + new String('\t', depth));
					}
					if (c == ']' || c == '}')
					{
						depth--;
						parser.insertBefore("\n" + new String('\t', depth));
					}
					if (c == ',' && !insideQuotes)
					{
						parser.insertAfter("\n" + new String('\t', depth));
					}
					if (c == ':' && !insideQuotes)
					{
						parser.insertAfter(" ");
					}
				}
				return parser.getContent();
			}
		}

	}
}
