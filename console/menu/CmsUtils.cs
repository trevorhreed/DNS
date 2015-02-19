using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MyApp.menu.CmsUtils
{
    public class AbstractViewModel
    {
        private Guid _id;
        public Guid Id
        {
            get
            {
                if (_id == null)
                {
                    _id = Guid.Empty;
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public bool IsNew
        {
            get
            {
                return this.Id.Equals(Guid.Empty);
            }
        }

        private DateTime _published;
        public DateTime Published
        {
            get
            {
                if (_published == null)
                {
                    _published = DateTime.Now;
                }
                return _published;
            }
            set
            {
                _published = value;
            }
        }

        private DateTime _expired;
        public DateTime Expired
        {
            get
            {
                if (_expired == null)
                {
                    _expired = DateTime.MaxValue;
                }
                return _expired;
            }
            set
            {
                _expired = value;
            }
        }
    }

    public class AbstractHelper<F, B>
        where F : AbstractViewModel
    {
        protected static F Map(B b)
        {
            return Mapper.Map<B, F>(b);
        }
        protected static B Map(F f)
        {
            return Mapper.Map<F, B>(f);
        }
        protected static IEnumerable<F> MapList(IEnumerable<B> list)
        {
            return Mapper.Map<IEnumerable<B>, IEnumerable<F>>(list);
        }
        protected static IEnumerable<B> MapList(IEnumerable<F> list)
        {
            return Mapper.Map<IEnumerable<F>, IEnumerable<B>>(list);
        }
    }

    namespace Mock
    {
        public interface IMockModel
        {
            Guid Id { get; set; }
        }
        public abstract class AbstractMockModel<B> : IMockModel where B : IMockModel
        {
            private Guid _id;
            public Guid Id
            {
                get
                {
                    if (_id == null || _id == Guid.Empty)
                    {
                        _id = Guid.NewGuid();
                    }
                    return _id;
                }
                set
                {
                    _id = value;
                }
            }

            private static Dictionary<Guid, B> rows;
            static AbstractMockModel()
            {
                Reset();
            }
            public static B Get(Guid id)
            {
                return rows[id];
            }
            public static IEnumerable<B> All()
            {
                return rows.Values.ToList();
            }
            public static void Put(B model)
            {
                rows.Add(model.Id, model);
            }
            public static void Del(Guid id)
            {
                rows.Remove(id);
            }
            public static void Reset()
            {
                rows = new Dictionary<Guid, B>();
            }
        }

        public class MockHelper<F, B> : AbstractHelper<F, B>
            where F : AbstractViewModel
            where B : AbstractMockModel<B>
        {
            public static F Get(Guid id)
            {
                #warning This method is only for mock purposes

                return Map(AbstractMockModel<B>.Get(id));
            }
            public static IEnumerable<F> All()
            {
                #warning This method is only for mock purposes

                return MapList(AbstractMockModel<B>.All());
            }
            public static void Put(F f)
            {
                #warning This method is only for mock purposes

                AbstractMockModel<B>.Put(Map(f));
            }
            public static void Put(List<F> list)
            {
                #warning This method is only for mock purposes

                foreach (F f in list)
                {
                    Put(f);
                }
            }
            public static void Delete(Guid id)
            {
                #warning This method is only for mock purposes

                AbstractMockModel<B>.Del(id);
            }
            public static void Delete(F f)
            {
                #warning This method is only for mock purposes

                Delete(f.Id);
            }
            public static void Delete(IEnumerable<F> list)
            {
                #warning This method is only for mock purposes

                foreach (F f in list)
                {
                    Delete(f.Id);
                }
            }
        }
    }
}