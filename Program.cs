using System;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace ConsoleApp2
{
    internal class Program
    {

        internal class One : IMenuItem
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

            public string Name => "One";
            public void Execute()
            {
                MessageBox(IntPtr.Zero, "Hello, World!", "", 0);
            }
        }
        internal class Two : IMenuItem
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

            public string Name => "Two";
            public void Execute()
            {
                do
                {
                    PlayGame();
                } while (AskToPlayAgain());
            }
            static void PlayGame()
            {
                int min = 0;
                int max = 100;
                int guess;
                bool guessedCorrectly = false;

                MessageBox(IntPtr.Zero, "Загадайте число от 0 до 100 и нажмите OK.", "Игра: Угадай число", 0);

                while (!guessedCorrectly)
                {
                    guess = (min + max) / 2;
                    int result = MessageBox(IntPtr.Zero, $"Ваше число: {guess}. Это ваше число?", "Игра: Угадай число", 1 | 32 | 0x40000);

                    if (result == 1)
                    {
                        MessageBox(IntPtr.Zero, $"Ура! Я угадал ваше число: {guess}!", "Игра: Угадай число", 0);
                        guessedCorrectly = true;
                    }
                    else if (result == 2) 
                    {
                        int hintResult = MessageBox(IntPtr.Zero, $"Ваше число больше {guess}?", "Игра: Угадай число", 1 | 32 | 0x40000);
                        if (hintResult == 1)
                        {
                            min = guess + 1; 
                        }
                        else
                        {
                            max = guess - 1;
                        }
                    }

                    if (min > max)
                    {
                        MessageBox(IntPtr.Zero, "Похоже, вы ошиблись! Попробуйте снова.", "Игра: Угадай число", 0);
                        return;
                    }
                }
            }

            static bool AskToPlayAgain()
            {
                    int result = MessageBox(IntPtr.Zero, "Хотите сыграть снова?", "Игра: Угадай число", 1 | 32);
                    return result == 1;
            }
        }

        internal class Three : IMenuItem
        {
            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll")]
            private static extern bool IsWindowVisible(IntPtr hWnd);

            private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            private const uint WM_CLOSE = 0x0010;

            public string Name => "Three";
            public void Execute()
            {
                EnumWindows(new EnumWindowsProc(EnumWindowCallback), IntPtr.Zero);
            }

            private static bool EnumWindowCallback(IntPtr hWnd, IntPtr lParam)
            {
                if (IsWindowVisible(hWnd))
                {
                    StringBuilder windowText = new StringBuilder(256);
                    GetWindowText(hWnd, windowText, windowText.Capacity);

                    if (windowText.ToString().Contains("Блокнот") || windowText.ToString().Contains("Безымянный - Блокнот"))
                    {
                        SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        Console.WriteLine($"Закрыто окно: {windowText}");
                    }
                }
                return true;
            }
        }





        static void Main()
        {
            var Menu = new Menu("Menu", new List<IMenuItem>
            {
                new One(),
                new Two(),
                new Three(),

            });

            Menu.Execute();
        }
    }
}
