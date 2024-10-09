using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusdriverBreaksConsoleUI
{
    public class Menu
    {
        private readonly EMenuLevel _menuLevel;

        private readonly List<MenuItem> _menuItems = new List<MenuItem>();
        private readonly MenuItem _menuItemExit = new MenuItem("E", "Exit", null);

        private HashSet<string> _menuShortCuts = new HashSet<string>();
        private HashSet<string> _menuSpecialShortCuts = new HashSet<string>();

        public string Input;
        private readonly string _title;


        public Menu(string title, EMenuLevel menuLevel)
        {
            _title = title;
            _menuLevel = menuLevel;

            switch (_menuLevel)
            {
                case EMenuLevel.Root:
                    _menuSpecialShortCuts.Add(_menuItemExit.ShortCut.ToUpper());
                    break;
            }
        }

        public void AddMenuItem(MenuItem item, int position = -1)
        {
            if (_menuSpecialShortCuts.Add(item.ShortCut.ToUpper()) == false)
            {
                throw new ApplicationException($"Conflicting menu shortcut {item.ShortCut.ToUpper()}");
            }

            if (_menuShortCuts.Add(item.ShortCut.ToUpper()) == false)
            {
                throw new ApplicationException($"Conflicting menu shortcut {item.ShortCut.ToUpper()}");
            }


            if (position == -1)
            {
                _menuItems.Add(item);
            }
            else
            {
                _menuItems.Insert(position, item);
            }
        }

        public void DeleteMenuItem(int position = 0)
        {
            _menuItems.RemoveAt(position);
        }

        public void EmptyMenu()
        {
            while (_menuItems.Count != 0)
            {
                DeleteMenuItem();
            }

            _menuShortCuts = new HashSet<string>();
            _menuSpecialShortCuts = new HashSet<string>();
            _menuSpecialShortCuts.Add(_menuItemExit.ShortCut.ToUpper());
        }

        public void AddMenuItems(List<MenuItem> items)
        {
            foreach (var menuItem in items)
            {
                if (_menuItems.Contains(menuItem))
                {
                    continue;
                }
                AddMenuItem(menuItem);
            }
        }

        public HashSet<string> GetMenuItems()
        {
            return _menuSpecialShortCuts;
        }

        public string Run()
        {
            var runDone = false;
            do
            {
                OutputMenu();
                Console.Write("Your choice:");
                Input = Console.ReadLine()?.Trim().ToUpper();
                var isInputValid = false;
                if (Input != null)
                {
                    isInputValid = _menuShortCuts.Contains(Input);
                }
                if (isInputValid)
                {
                    var item = _menuItems.FirstOrDefault(t => t.ShortCut.ToUpper() == Input);
                    Input = item?.RunMethod == null ? Input : item.RunMethod();
                }

                if (Input != null)
                {
                    runDone = _menuSpecialShortCuts.Contains(Input);
                }

                if (!runDone && !isInputValid)
                {
                    Console.WriteLine($"Unknown shortcut '{Input}'!");
                }
            } while (!runDone);


            return Input ?? "";
        }

        private void OutputMenu()
        {
            Console.WriteLine("====> " + _title + " <====");

            Console.WriteLine("-------------------");

            foreach (var t in _menuItems)
            {
                Console.WriteLine(t);
            }

            Console.WriteLine("-------------------");

            switch (_menuLevel)
            {
                case EMenuLevel.Root:
                    Console.WriteLine(_menuItemExit);
                    break;
            }

            Console.WriteLine("=====================");
        }
    }
}
