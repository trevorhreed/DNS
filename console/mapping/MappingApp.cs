using MyApp.utils;
using System;
using System.Collections;
using System.Reflection;

namespace MyApp.mapping
{
    class MappingApp : IApp
    {
        public void run()
        {
            Foo foo = new Foo();
            foo.Name = "Trevor";
            foo.Age = 31;
            foo.Email = "trevorhreed@gmail.com";

            Bar bar = ObjectMapper.Map<Bar>(foo);

            Debug.Dump(bar);

            Console.ReadKey(true);
        }
    }

    class ObjectMapper
    {
        public static D Map<D>(object s) where D : new()
        {
            D d = new D();

            Type dType = typeof(D);
            IEnumerable props = s.GetType().GetProperties();
            foreach (PropertyInfo sProp in props)
            {
                PropertyInfo dProp = dType.GetProperty(sProp.Name);
                if (dProp != null)
                {
                    dType.GetProperty(sProp.Name).SetValue(d, sProp.GetValue(s));
                }
            }

            return d;
        }
    }
    class Foo
    {
        public String Name { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }
    }
    class Bar
    {
        public String Name { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }
    }
}