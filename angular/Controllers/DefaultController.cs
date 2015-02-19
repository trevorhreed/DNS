using angular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace angular.Controllers
{
    public class DefaultController : ApiController
    {
        new List<Person> people = new List<Person>()
        {
            new Person(){
                Name = "Trevor",
                Email = "trevor@example.com",
                Phones = new List<Phone>(){
                    new Phone(){
                        CountryCode = 1,
                        Number = "801-555-2225",
                        Extension = ""
                    }
                },
                HomeAddress = new Address(){
                    Street = "222 W 333 E",
                    Apt = "111",
                    City = "Provo",
                    State = "UT",
                    Zip = "84604"
                },
                WorkAddress = new Address(){
                    Street = "444 N 555 S",
                    Apt = "",
                    City = "Orem",
                    State = "UT",
                    Zip = "84097"
                }
            }
        };

        // GET: api/Default
        public List<Person> Get()
        {
            return people;
        }

        // GET: api/Default/5
        public Person Get(int id)
        {
            return people[0];
        }

        // POST: api/Default
        public void Post(Person person)
        {
            object p = person;
        }

        // PUT: api/Default/5
        public void Put(int id, Person person)
        {
            object p = person;
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
            object i = id;
        }
    }
}
