using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Db
{
    private interface IModel
    {
        Guid getId();
    }
    private abstract class AModel<T> : IModel where T : IModel
    {
        private Guid _id;
        public Guid getId()
        {
            if (_id == Guid.Empty)
            {
                _id = Guid.NewGuid();
            }
            return _id;
        }

        private static Dictionary<Guid, T> rows;
        static AModel()
        {
            rows = new Dictionary<Guid, T>();
        }
        public static T Get(Guid id)
        {
            return rows[id];
        }
        public static Dictionary<Guid, T> All()
        {
            return rows;
        }
        public static void Put(T model)
        {
            rows.Add(model.getId(), model);
        }
        public static void Del(Guid id)
        {
            rows.Remove(id);
        }
        public static void Del(T model)
        {
            rows.Remove(model.getId());
        }
    }

    public class Model : AModel<Model>
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class Example
    {
        public void run()
        {
            
        }
    }
}
