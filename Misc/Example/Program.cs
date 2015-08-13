using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WriteLine("\n  XML Reader Examples\n  ===================");
            printExamples();            
            Console.WriteLine("\n\n\n\n  Press any key to exit.");
            Console.ReadKey(true);
        }

        static void printExamples()
        {
            IEnumerable<Type> exampleClasses = typeof(Example).GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Example)));
            int maxClassNameSize = exampleClasses.Aggregate(0, (max, cur) => max > cur.Name.Length ? max : cur.Name.Length);
            foreach (Type exampleClass in exampleClasses)
            {
                ((Example)Activator.CreateInstance(exampleClass)).go(maxClassNameSize);
            }
        }

        public abstract class Example
        {
            protected static String file = "..\\..\\sample.xml";

            public void go(int classNameSize = 25)
            {
                Console.WriteLine("\n    " + this.GetType().Name.PadLeft(classNameSize, ' ') + ": " + this.get());
            }

            public abstract String get();

            public class XmlDocumentExample : Example
            {
                static String path = "./rss/channel/item/title";

                public override String get()
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    return doc
                        .CreateNavigator()
                        .SelectSingleNode(path)
                        .ToString();
                }
            }

            public class CsQueryExample : Example
            {
                public override String get()
                {
                    CQ dom = CQ.CreateDocumentFromFile(file);
                    return dom["rss channel item title"].First().Text();
                }
            }

            public class RegexExample : Example
            {
                public override string get()
                {
                    String content = File.ReadAllText(file);
                    Regex regex = new Regex(@"<title>([^<]*)</title>");
                    MatchCollection matches = regex.Matches(content);
                    return matches.Count > 0 ? matches[1].Groups[1].Value : "Not found!";
                }
            }

            public class SubStringExample : Example
            {
                public override string get()
                {
                    String openTag = "<title>";
                    String closeTag = "</title>";
                    String content = File.ReadAllText(file);
                    int start = content.IndexOf(openTag) + openTag.Length;
                    start = content.IndexOf(openTag, start) + openTag.Length;
                    int length = content.IndexOf(closeTag, start) - start;
                    return content.Substring(start, length);
                }
            }
        }
    }
}
