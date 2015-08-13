using AutoMapper;
using MyApp.menu.CmsUtils;
using MyApp.menu.CmsUtils.Mock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyApp.menu.Example
{
    public class MockPerson : AbstractMockModel<MockPerson>
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class ViewPerson : AbstractViewModel
    {
        [Required]
        public string Name;

        [Required]
        public string Age;
    }

    public class PersonHelper : MockHelper<ViewPerson, MockPerson>
    {
        static PersonHelper()
        {
            Mapper.CreateMap<ViewPerson, MockPerson>();
            Mapper.CreateMap<MockPerson, ViewPerson>();
        }

        public static new ViewPerson Get(Guid id)
        {
            return null;
        }

        public static IEnumerable<ViewPerson> GetByName(string Name)
        {
            return PersonHelper.MapList(
                MockPerson
                    .All()
                    .Where(x => x.Name.StartsWith(Name))
            );
        }
    }

    public class ExampleController
    {
        public IEnumerable<ViewPerson> List()
        {
            return PersonHelper.All();
        }

        public ViewPerson Get(Guid id)
        {
            return PersonHelper.Get(id);
        }

        public IEnumerable<ViewPerson> ListByName(string Name)
        {
            return PersonHelper.GetByName(Name);
        }

        public void SavePerson(ViewPerson viewPerson)
        {
            PersonHelper.Put(viewPerson);
        }

        public void DeletePerson(Guid id)
        {
            PersonHelper.Delete(id);
        }

        public void dud()
        {
            ViewPerson person = PersonHelper.Get(Guid.Empty);
            IEnumerable<ViewPerson> people = PersonHelper.All();
            IEnumerable<ViewPerson> peopleNamedFrank = PersonHelper.GetByName("Frank");
            PersonHelper.Put(new ViewPerson());
            PersonHelper.Put(person);
            PersonHelper.Delete(person);
            PersonHelper.Delete(person.Id);
            PersonHelper.Delete(peopleNamedFrank);
        }
    }
}