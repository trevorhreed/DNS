using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Face
{
    public delegate void MenuBuildAction(Menu menu);
    public delegate void MenuItemAction(object param);

    public class MenuItem
    {
        private static string itemMargin = "\t";

        public string Label { get; set; }
        public Boolean Selectable { get; set; }
        public Boolean Selected { get; set; }
        public Int32? LineNumber { get; set; }
        public Menu SubMenu;
        public MenuItemAction TaskAction;
        public object ActionParameter;

        public MenuItem()
        {
            this.Selectable = true;
        }
        public void Print()
        {
            if (LineNumber == null)
            {
                LineNumber = Console.CursorTop;
            }
            Console.SetCursorPosition(0, LineNumber.Value);
            string check = this.Selectable ? this.Selected ? "[*] " : "[ ] " : "    ";
            Console.WriteLine(MenuItem.itemMargin + check + this.Label);
        }
        public MenuItem SetSelected(bool selected)
        {
            this.Selected = selected;
            return this;
        }
        public void Execute()
        {
            if (SubMenu != null)
            {
                SubMenu.Show();
            }
            else if (TaskAction != null)
            {
                TaskAction(this.ActionParameter);
            }
            else
            {
                throw new ReturnToPreviousMenuException();
            }
        }
    }
    public class Menu
    {
        private static string headerMargin = "  ";
        private static string descMargin = "     ";

        private static void PrintHeader(string text)
        {
            Console.WriteLine("\n" + Menu.headerMargin + text + "\n" + Menu.headerMargin + new String('=', text.Length) + "\n");
        }
        private static void PrintDescription(List<string> lines)
        {
            foreach (string line in lines)
            {
                Console.WriteLine(descMargin + line);
            }
            Console.WriteLine();
        }
        public static List<string> MakeAlignedList(params string[] items)
        {
            List<string> lines = new List<string>();
            if (items.Length == 0 || items.Length % 2 != 0)
            {
                return lines;
            }
            int maxSize = 0;
            for (int i = 0; i < items.Length; i += 2)
            {
                if (maxSize < items[i].Length)
                {
                    maxSize = items[i].Length;
                }
            }
            for (int i = 0; i < items.Length; i += 2)
            {
                lines.Add(new String(' ', maxSize - items[i].Length) + items[i] + ": " + items[i+1]);
            }
            return lines;
        }
        public static Dictionary<string, string> GetInput(string desc, params string[] prompts)
        {
            Console.Clear();
            Menu.PrintHeader(desc);
            Dictionary<string, string> results = new Dictionary<string, string>();
            foreach (string prompt in prompts)
            {
                Console.Write("\t" + prompt + ": ");
                results.Add(prompt, Console.ReadLine());
            }
            return results;
        }
        
        public Boolean HardMenu { get; set; }
        public string Title { get; set; }
        public List<string> Description { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public int CurrentIndex { get; set; }
        public MenuBuildAction BuildMenuItemsAction;

        public Menu()
        {
            this.HardMenu = false;
            this.MenuItems = new List<MenuItem>();
        }

        public void Show()
        {
            try
            {
                this.Print();
                do
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        this.MoveUp();
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        this.MoveDown();
                    }
                    else if (keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        this.ExecuteCurrent();
                        this.Print();
                    }
                    else if ((keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.LeftArrow) && !this.HardMenu)
                    {
                        break;
                    }
                } while (true);
            }
            catch (ReturnToPreviousMenuException) { /* return to previous menu */ }
        }
        private void Print()
        {
            Console.Clear();
            if (BuildMenuItemsAction != null)
            {
                BuildMenuItemsAction(this);
            }
            Menu.PrintHeader(this.Title);
            if (this.Description != null)
            {
                Menu.PrintDescription(this.Description);
            }
            if (MenuItems.Count > 0)
            {
                foreach (MenuItem menuItem in MenuItems)
                {
                    menuItem.SetSelected(false);
                }
                CurrentIndex = 0;
                MenuItems.First().SetSelected(true);
            }
            foreach (MenuItem menuItem in this.MenuItems)
            {
                menuItem.Print();
            }
            Console.WriteLine();
        }

        public void Move(int amount)
        {
            this.GetCurrent().SetSelected(false).Print();
            CurrentIndex += amount;
            if (CurrentIndex < 0)
            {
                CurrentIndex = MenuItems.Count - 1;
            }
            if (CurrentIndex >= MenuItems.Count)
            {
                CurrentIndex = 0;
            }
            this.GetCurrent().SetSelected(true).Print();
        }
        public void MoveUp()
        {
            Move(-1);
        }
        public void MoveDown()
        {
            Move(1);
        }
        public MenuItem GetCurrent()
        {
            if (MenuItems.Count > 0)
            {
                if (CurrentIndex >= 0 && CurrentIndex < MenuItems.Count)
                {
                    return MenuItems[CurrentIndex];
                }
                else
                {
                    return MenuItems.First();
                }
            }
            else
            {
                throw new ReturnToPreviousMenuException();
            }
        }
        public void ExecuteCurrent()
        {
            this.GetCurrent().Execute();
        }
    }
}
