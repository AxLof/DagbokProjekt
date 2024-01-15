namespace Dagbok_slutv_
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int userSelect = 0;
            Diary.ImportPosts();
            while (userSelect != 6)
            {
                printMenu();

                try
                {
                    userSelect = Convert.ToInt16(Console.ReadLine());
                    switch (userSelect)
                    {
                        case 1:
                            Diary.AddPost();
                            break;
                        case 2:
                            Diary.ViewPost();
                            break;
                        case 3:
                            Diary.SavePost();
                            break;
                        case 4:
                            Diary.RemovePost();
                            break;
                        case 5:
                            Diary.Search();
                            break;
                        case 6:
                            Diary.ExitSave();
                            userSelect = 6;
                            break;
                        default:
                            Console.WriteLine("\nSkriv in en giltig siffra!");
                            break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nSkriv in ett giltigt tecken!\n");
                }
            }

            void printMenu()
            {
                Console.WriteLine("\nVälj ett alternativ:");
                Console.WriteLine("--------------------");
                Console.WriteLine("1. Skapa ett nytt inlägg");
                Console.WriteLine("2. Öppna inlägg");
                Console.WriteLine("3. Spara inlägg");
                Console.WriteLine("4. Ta bort ett inlägg");
                Console.WriteLine("5. Sök efter ett inlägg");
                Console.WriteLine("6. Avsluta programmet");

            }
        }
    }
}