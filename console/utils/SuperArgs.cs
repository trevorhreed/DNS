using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public interface ISuperArgs
    {
        List<string> Raw { get; set; }
    }
    public class BaseSuperArgs<T> : ISuperArgs where T : ISuperArgs, new()
    {
        public List<string> Raw { get; set; }

        public static T Create(string[] rawArgs)
        {
            T superArgs = new T();
            superArgs.Raw = new List<string>();
            Dictionary<string, object> pairedArgs = new Dictionary<string, object>();
            foreach (string rawArg in rawArgs)
            {
                superArgs.Raw.Add(rawArg);

                if (rawArg.StartsWith("-"))
                {
                    pairedArgs.Add(rawArg.TrimStart('-').ToLower(), true);
                }
                else if (rawArg.Contains(':'))
                {
                    string[] parts = rawArg.Split(new char[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        pairedArgs.Add(parts[0].ToLower(), parts[1]);
                    }
                }
                else
                {
                    pairedArgs.Add(rawArg, rawArg);
                }
            }
            IEnumerable<PropertyInfo> infos = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Argument)));
            foreach (PropertyInfo info in infos)
            {
                string key = info.Name.ToLower();
                string shortKey = ((Argument)info.GetCustomAttribute(typeof(Argument))).Value;
                object value = pairedArgs.ContainsKey(key)
                        ? pairedArgs[key] 
                        : shortKey != null && pairedArgs.ContainsKey(shortKey) 
                            ? pairedArgs[shortKey] 
                            : null;
                if (info.PropertyType == typeof(Boolean))
                {
                    if (value == null)
                    {
                        value = false;
                    }
                    else if (value.GetType() == typeof(String))
                    {
                        value = !String.IsNullOrEmpty(((String)value).Trim());
                    }
                }
                else if (info.PropertyType == typeof(String))
                {
                    value = Convert.ToString(value);
                }
                info.SetValue(superArgs, value);
            }
            return superArgs;
        }
    }

    public class Argument : System.Attribute
    {
        private string _alias;
        public string Value { get { return _alias; } }

        public Argument()
        {
            this._alias = null;
        }
        public Argument(string alias)
        {
            this._alias = alias;
        }
    }

    public class DefaultSuperArgs : BaseSuperArgs<DefaultSuperArgs> { }

    public class MySuperArgs : BaseSuperArgs<MySuperArgs>
    {
        [Argument()]
        public bool apple { get; set; }

        [Argument(alias: "b")]
        public string banana { get; set; }
    }
}
