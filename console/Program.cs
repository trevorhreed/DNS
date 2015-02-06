using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Db;
using MyApp.Face;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Menu()
            {
                HardMenu = true,
                Title = "Welcome to Model Manipulator",
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem() 
                    {
                        Label = "List all models",
                        TaskAction = ListModels
                    },
                    new MenuItem()
                    {
                        Label = "Add new model",
                        TaskAction = AddModel
                    },
                    new MenuItem()
                    {
                        Label = "Exit",
                        TaskAction = Exit,
                        ActionParameter = 0
                    }
                }
            };//.Show();
        }

        private static void ListModels(object param)
        {
            Menu menu = new Menu()
            {
                Title = "List of all models",
                MenuItems = new List<MenuItem>(),
                BuildMenuItemsAction = delegate(Menu m){
                    m.MenuItems = new List<MenuItem>();
                    Dictionary<Guid, Person> models = Person.All();
                    if (models.Count > 0)
                    {
                        m.MenuItems.Add(new MenuItem()
                        {
                            Label = "Back"
                        });
                        foreach (KeyValuePair<Guid, Person> pair in Person.All())
                        {
                            string key = pair.Key.ToString();
                            m.MenuItems.Add(new MenuItem()
                            {
                                Label = pair.Value.Name + " (" + key.Substring(0, 5) + "..." + key.Substring(key.Length-5) + ")",
                                TaskAction = ShowModel,
                                ActionParameter = pair.Value
                            });
                        }
                    }
                    else
                    {
                        m.MenuItems.Add(new MenuItem()
                        {
                            Label = "No models.",
                            Selectable = false
                        });
                    }
                }
            };
            menu.Show();
        }

        private static void ShowModel(object param)
        {
            Person model = (Person)param;
            Menu menu = new Menu()
            {
                Title = "Model (" + model.getId() + ")",
                BuildMenuItemsAction = delegate(Menu m){
                    m.Description = Menu.MakeAlignedList(
                        "Name", model.Name,
                        "Age", model.Age
                    );
                },
                MenuItems = new List<MenuItem>()
                {
                    new MenuItem(){
                        Label = "Back"
                    },
                    new MenuItem(){
                        Label = "Update",
                        TaskAction = UpdateModel,
                        ActionParameter = model
                    },
                    new MenuItem(){
                        Label = "Delete",
                        TaskAction = DeleteModel,
                        ActionParameter = model.getId()
                    }
                }
            };
            menu.Show();
        }

        private static void AddModel(object param)
        {
            Person model = new Person();
            Dictionary<string, string> input = Menu.GetInput("Please enter the following information", "Name", "Age");
            model.Name = input["Name"];
            model.Age = input["Age"];
            Person.Put(model);
        }

        private static void UpdateModel(object param)
        {
            Person model = (Person)param;
            string name = "Name (" + model.Name + ")";
            string age = "Age (" + model.Age + ")";
            Dictionary<string, string> input = Menu.GetInput("Please enter the following information", name, age);
            model.Name = input[name];
            model.Age = input[age];
        }

        private static void DeleteModel(object param)
        {
            Person.Del((Guid)param);
            throw new ReturnToPreviousMenuException();
        }

        private static void Exit(object param)
        {
            System.Environment.Exit((int)param);
        }
    }
}
