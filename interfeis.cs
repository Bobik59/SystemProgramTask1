using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
        public interface IMenuItem
        {
            string Name { get; }
            void Execute();
        }

        public class Menu : IMenuItem
        {
            public string Name { get; }
            private readonly List<IMenuItem> _menuItems;

            public Menu(string name, List<IMenuItem> menuItems)
            {
                Name = name;
                _menuItems = menuItems;
            }

            public void Execute()
            {
                ShowMenu(_menuItems);
            }

            private static void ShowMenu(List<IMenuItem> menuItems)
            {
                int selectedIndex = 0;
                int maxLength = menuItems.Max(item => item.Name.Length) + 4;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("+" + new string('-', maxLength) + "+");
                    for (int i = 0; i < menuItems.Count; i++)
                    {
                        string prefix = (i == selectedIndex) ? "* " : "  ";
                        Console.WriteLine($"| {prefix}{menuItems[i].Name.PadRight(maxLength - 4)} |");
                    }
                    Console.WriteLine("+" + new string('-', maxLength) + "+");
                    Console.WriteLine("Используйте стрелки для навигации, Enter для выбора. Esc для возврата.");

                    var key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : menuItems.Count - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex < menuItems.Count - 1) ? selectedIndex + 1 : 0;
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        menuItems[selectedIndex].Execute();
                        Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                        Console.ReadKey();
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            }
        }
}