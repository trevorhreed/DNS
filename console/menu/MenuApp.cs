using MyApp.menu.Example;
using MyApp.menu.Face;
using MyApp.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.menu
{
    class MenuApp : IApp
    {
        public void run()
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
            }.Show();
        }

        private static void ListModels(object param)
        {
            Menu menu = new Menu()
            {
                Title = "List of all models",
                MenuItems = new List<MenuItem>(),
                BuildMenuItemsAction = delegate(Menu m)
                {
                    m.MenuItems = new List<MenuItem>();
                    IEnumerable<ViewPerson> models = new ExampleController().List();
                    if (models.Count() > 0)
                    {
                        m.MenuItems.Add(new MenuItem()
                        {
                            Label = "Back"
                        });
                        foreach (ViewPerson viewPerson in new ExampleController().List())
                        {
                            string key = viewPerson.Id.ToString();
                            m.MenuItems.Add(new MenuItem()
                            {
                                Label = viewPerson.Name + " (" + key.Substring(0, 5) + "..." + key.Substring(key.Length - 5) + ")",
                                TaskAction = ShowModel,
                                ActionParameter = viewPerson.Id
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
            ViewPerson model = new ExampleController().Get((Guid)param);
            Menu menu = new Menu()
            {
                Title = "Model (" + model.Id + ")",
                BuildMenuItemsAction = delegate(Menu m)
                {
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
                        ActionParameter = model.Id
                    }
                }
            };
            menu.Show();
        }

        private static void AddModel(object param)
        {
            ViewPerson model = new ViewPerson();
            Dictionary<string, string> input = Menu.GetInput("Please enter the following information", "Name", "Age");
            model.Name = input["Name"];
            model.Age = input["Age"];
            new ExampleController().SavePerson(model);
        }

        private static void UpdateModel(object param)
        {
            ViewPerson model = (ViewPerson)param;
            string name = "Name (" + model.Name + ")";
            string age = "Age (" + model.Age + ")";
            Dictionary<string, string> input = Menu.GetInput("Please enter the following information", name, age);
            model.Name = input[name];
            model.Age = input[age];
            new ExampleController().SavePerson(model);
        }

        private static void DeleteModel(object param)
        {
            new ExampleController().DeletePerson((Guid)param);
            throw new ReturnToPreviousMenuException();
        }

        private static void Exit(object param)
        {
            System.Environment.Exit((int)param);
        }
    }
}
