using angular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace angular.Controllers
{
    public class PeopleController : ApiController
    {
		private static Guid TrevorsId = Guid.NewGuid();

		private static Dictionary<Guid, Person> _people = new Dictionary<Guid, Person>()
		{
			{
				TrevorsId,
				new Person() {
					Id = TrevorsId,
					Name = "Trevor",
					Email = "trevorhreed@gmail.com",
					Phones = new List<String>() {
						{
							"801.836.3202"
						}
					},
					Address = new Address() {
						Street = "223 W. 2230 N.",
						Apt = "11",
						City = "Provo",
						State = "UT",
						Zip = "84604"
					}
				}
			}
		};

        // GET: api/Default
		public Json Get()
        {
			return new Json(_people.Values.ToList());
        }

        // GET: api/Default/5
		public Json Get(Guid id)
        {
			return new Json(_people[id]);
        }

        // POST: api/Default
		public Json Post(Person person)
        {
			if (person.Id == null || Guid.Empty.Equals(person.Id))
			{
				person.Id = Guid.NewGuid();
			}
			_people.Add(person.Id, person);

			return new Json(person.Id);
        }

        // PUT: api/Default/5
		public Json Put(Guid id, Person person)
        {
			if (!_people.ContainsKey(id))
			{
				_people.Add(id, person);
			}
			else
			{
				_people[id] = person;
			}

			return new Json();
        }

        // DELETE: api/Default/5
		public Json Delete(Guid id)
        {
			_people.Remove(id);

			return new Json();
        }
    }
}
