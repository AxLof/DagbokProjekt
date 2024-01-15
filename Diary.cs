using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dagbok_slutv_;

public class Diary
{
    private static string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); 
    private static string macDirr = defaultPath + "/Dagbok";
    private static string winDirr = defaultPath + "\\Dagbok";
    private static List<Diary> listOfDiaries = new List<Diary>(); //Innehåller alla instanser av Diary. 
    private static bool hasCreated = false;
    private static bool hasSaved = false;
    public string Title { get; private set; }
    public DateTime DateTime { get; private set; }
    public string Content { get; private set; }
    public int ID { get; private set; }

    public Diary(string title, DateTime dateTime, string content, int id)
    {
        Title = title;
        DateTime = dateTime;
        Content = content;
        ID = id;
    }

    public static void AddPost()
    {
        string title;
        DateTime dateTime = DateTime.Now;
        string content;
        int id;

        do
        {
            Console.WriteLine("\nTitel: ");
            title = Console.ReadLine();
            if (string.IsNullOrEmpty(title)) { Console.WriteLine("Du måste skriva in en titel!"); }
        } while (string.IsNullOrEmpty(title));

        Console.WriteLine("\nBrödtext: ");
        content = Console.ReadLine();
        id = 1;
        Diary d = new Diary(title, dateTime, content, id);
        listOfDiaries.Insert(0, d);
        foreach (Diary diary in listOfDiaries)
        {
            diary.ID = listOfDiaries.IndexOf(diary)+1;
        }
        hasCreated = true;

    }

    public static void SavePost() //Skapar en ny mapp "Dagbok" och sparar filerna där. 
    {

        string userDirr = "";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            userDirr = winDirr;
        }

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            userDirr = macDirr;
        }

        else
        {
            Console.WriteLine("Ditt operativsystem stöds inte av programmet");
        }

        if (!Directory.Exists(userDirr))
        {
            Directory.CreateDirectory(userDirr);
        }

        if (listOfDiaries.Count > 0)
        {
            foreach (Diary d in listOfDiaries)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(userDirr, d.Title)))
                {
                    outputFile.WriteLine(d.Title);
                    outputFile.WriteLine(d.DateTime);
                    outputFile.WriteLine("");
                    outputFile.WriteLine(d.Content);
                }
            }

            Console.WriteLine($"\nInlägg sparade i \"{userDirr}\"");
            hasSaved = true;
        }

        else
        {
            Console.WriteLine("\nDet finns inga inlägg att spara!");
        }

    }

    public static void ViewPost() //Skriver ut Titel och Datum för samtliga inlägg och låter användaren välja vilket inlägg som ska öppnas.
    {
        int open=0;

        if (listOfDiaries.Count > 0)
        {
            open=Selection("\nSkriv in nummret på inlägget som du vill öppna!");
           
        }

        else
        {
            Console.WriteLine("\nDet finns inga inlägg att visa.");
        }

        foreach (Diary d in listOfDiaries )
        {
            if (d.ID == open)
            {
                Console.WriteLine($"\n{d.Title}\n{d.DateTime}\n\n{d.Content}");
            }
        }

    }

    public static void RemovePost() //Tar bort ett inlägg.
    {
        string removedDiary = "";
        string userDirr = "";
        int remove = 0;

        if (listOfDiaries.Count > 0)
        {
            
            remove = Selection("\nSkriv in nummret på inlägget som du vill ta bort!");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { userDirr = winDirr + "\\"; }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) { userDirr = macDirr + "/"; }
            else { Console.WriteLine("\nDitt operativsystem stöds inte av programmet!"); }

            for (int i = 0; i < listOfDiaries.Count; i++)
            {
                if (listOfDiaries[i].ID == remove)
                {
                    removedDiary = listOfDiaries[i].Title;
                    File.Delete(userDirr + removedDiary);
                    listOfDiaries.RemoveAt(i);
                }
            }

            if(remove > 0)
            {
                Console.WriteLine($"\nInlägg \"{removedDiary}\" borttagen!");
                foreach (Diary d in listOfDiaries ) 
                {
                    d.ID = listOfDiaries.IndexOf(d)+1;
                }
            }
        }

        else
        {
            Console.WriteLine("\nDet finns inga inlägg att ta bort.");
        }
    }

    public static void Search() //Söker efter inlägg baserat på titel eller del av titel. 
    {
        if (listOfDiaries.Count > 0)
        {
            Console.WriteLine("\nSök efter inlägg:");
            string search = Console.ReadLine();
            List<Diary> searchHits = new List<Diary>();

            for (int i = 0; i < listOfDiaries.Count; i++)
            {

                if (listOfDiaries[i].Title.Contains(search))
                {
                    searchHits.Add(listOfDiaries[i]);
                }


            }

            if (searchHits.Count == 0)
            {
                Console.WriteLine($"\nInga träffar hittades för söksträngen \"{search}\", tänk på att programmet är skriftlägeskänsligt!");
            }
            else
            {
                Console.WriteLine($"\nTotalt {searchHits.Count} träffar hittades för söksträngen \"{search}\"");
                foreach (Diary d in searchHits)
                {
                    Console.WriteLine($"\n{d.Title} {d.DateTime}");
                }
            }

        }
    }

    public static void ImportPosts() //Importerar alla filer i mappen "Dagbok", skapar objekt och lägger till dom i listan "listOfDiaries"
    {
        string importDirr = "";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { importDirr = winDirr; }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) { importDirr = winDirr; }
        else { Console.WriteLine("Error: Kunde inte importera inlägg."); }

        if (Directory.Exists(importDirr))
        {
            string[] fileTitles = Directory.GetFiles(importDirr);
            List<string> titleImport = new List<string>();
            List<string> contentImport = new List<string>();
            List<DateTime> dateImport = new List<DateTime>();

            foreach (string filePath in fileTitles)
            {
                string fileName = Path.GetFileName(filePath);
                titleImport.Add(fileName);
            }

            foreach (string filePath in fileTitles)
            {
                string fileContent = File.ReadLines(filePath).Skip(3).FirstOrDefault();
                contentImport.Add(fileContent);

            }

            foreach (string filePath in fileTitles)
            {
                DateTime date = Convert.ToDateTime(File.ReadLines(filePath).Skip(1).Take(1).FirstOrDefault());
                dateImport.Add(date);
            }

            for (int i = 0; i < titleImport.Count; i++)
            {
                
                Diary d = new Diary(titleImport[i], dateImport[i], contentImport[i], i+1 );
                listOfDiaries.Add(d);
            }

            listOfDiaries.Sort((x, y) => y.DateTime.CompareTo(x.DateTime));
            foreach (Diary d in listOfDiaries)
            {
                d.ID = listOfDiaries.IndexOf(d)+1;
            }

        }

    }

    public static void ExitSave() //Förhindrar att användaren oavsiktligt avslutar programmet utan att spara. 
    {
        if (hasSaved == false && hasCreated)
        {
            Console.WriteLine("\nDet finns osparade ändringar, är du säker på att du vill avsluta programmet?");
            Console.WriteLine("[Enter]: Spara och avsluta.        [Backspace]: Avsluta utan att spara ");
            bool validKey = false;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);


                if (key.Key == ConsoleKey.Enter)
                {
                    SavePost();
                    validKey = true;

                }

                else if (key.Key == ConsoleKey.Backspace)
                {
                    Environment.Exit(0);
                    validKey = true;
                }
            } while (validKey == false);
        }
    }

    private static int Selection(string input) //Kallas när man vill göra ett användarval via ID så att man slipper skriva samma kod flera gånger. 
    {
        int userInputSelection=0;

        foreach (Diary d in listOfDiaries)
        {
            Console.WriteLine($"\n[{d.ID}]: Titel: {d.Title}, Skapad: {d.DateTime}");
        }
        Console.WriteLine(input);
        Console.WriteLine("Eller skriv in \'0\' för att avbryta!");
        do
        {
            try
            {
                userInputSelection = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("\nError: Försök igen!");
            }

            if(userInputSelection > listOfDiaries.Count)
            {
                Console.WriteLine($"\nSkriv in en siffra mellan 0 och {listOfDiaries.Count}!");
            }
            if( userInputSelection == 0)
            {
                return 0;
            }

        }
        while (userInputSelection <= 0 || userInputSelection > listOfDiaries.Count);

        return userInputSelection;

    }
}


    

