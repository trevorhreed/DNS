using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace angular.Models
{
	public class Json
	{
		public int Code;
		public string Message;
		public object Data;

		public Json(int code, string message, object data)
		{
			this.Code = code;
			this.Message = message;
			this.Data = data;
		}

		public Json(object data)
		{
			this.Code = 200;
			this.Message = "OK";
			this.Data = data;
		}

		public Json()
		{
			this.Code = 200;
			this.Message = "OK";
			this.Data = new object();
		}
	}
}