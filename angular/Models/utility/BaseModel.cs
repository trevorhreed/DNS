using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace angular.Models
{
	public class BaseModel
	{
		public Guid Id;

		public static Boolean isNew(BaseModel model)
		{
			return model.Id == null || Guid.Empty.Equals(model.Id);
		}
	}
}