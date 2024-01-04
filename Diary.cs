using System.Runtime.InteropServices;


namespace Projekt_2._2;

public class Diary
{
    
    public static string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static string macDirr = defaultPath + "/Dagbok";
    public static string winDirr = defaultPath + "\\Dagbok";
    public static List<Diary> listOfdiaries = new List<Diary>();
    public static bool hasCreated = false;
    public static bool hasSaved = false;
    public string Title { get; private set; }
    public DateTime DateTime { get; private set; }
    public string Content { get; private set; }

    public Diary(string title, DateTime dateTime, string content)
    {
        Title = title;
        DateTime = dateTime;
        Content = content;
    }

    public static void AddDiary()
    {
        string title;
        DateTime dateTime = DateTime.Now;
        string content;
        
        do
        {
            Console.WriteLine("\nTitel: ");
            title = Console.ReadLine();
            if(string.IsNullOrEmpty(title)){Console.WriteLine("Du måste skriva in en titel!");}
        } while (string.IsNullOrEmpty(title));
        
        Console.WriteLine("\nBrödtext: ");
        content = Console.ReadLine();

        Diary d = new Diary(title, dateTime, content);
        listOfdiaries.Add(d);
        hasCreated = true;

    }

    public static void SaveDiary() //Skapar en ny mapp "Dagbok" och sparar filerna där. 
    {

        string userDirr="";

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

        if (listOfdiaries.Count > 0)
        {
            foreach (Diary d in listOfdiaries)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(userDirr, d.Title)))
                {
                    outputFile.WriteLine(d.Title);
                    outputFile.WriteLine(d.DateTime);
                    outputFile.WriteLine("");
                    outputFile.WriteLine(d.Content);
                }
            }

            Console.WriteLine($"\nDagböcker sparade i \"{userDirr}\"");
            hasSaved = true;
        }
        
        else
        {
            Console.WriteLine("\nDet finns inga dagböcker att spara!");
        }
        
    }

    public static void PrintDiaries() //Skriver ut samtliga dagböcker. 
    {
        if (listOfdiaries.Count > 0)
        {
            Console.WriteLine("\nDessa dagböcker hittades:");
            foreach (Diary d in listOfdiaries)
            {
                Console.WriteLine($"\nTitel: {d.Title}, Skapad: {d.DateTime}");
            }
        }

        else
        {
            Console.WriteLine("\nDet finns inga dagböcker att visa.");
        }
        
    }

    public static void RemoveDiary() //Tar bort en dagbok, tar ännu inte bort en dagbok från filsystemet. 
    {
        
        if (listOfdiaries.Count > 0)
        {
            PrintDiaries();
            Console.WriteLine("\nSkriv in titeln på dagboken som du vill ta bort");
            string remove = Console.ReadLine();
            string removedDiary="";
            bool isRemoved = false;

            for (int i = 0; i < listOfdiaries.Count; i++)
            {
                if (listOfdiaries[i].Title == remove)
                {
                    removedDiary = listOfdiaries[i].Title;
                    listOfdiaries.RemoveAt(i);
                    isRemoved = true;
                }
            }

            if (isRemoved)
            {
                Console.WriteLine($"\nDagbok \"{removedDiary}\" borttagen!");
            }
            else
            {
                Console.WriteLine($"\nIngen dagbok med titeln \"{remove}\" hittades. Tänkt på att programmet är skriftlägeskänsligt!");
            }
        }

        else
        {
            Console.WriteLine("\nDet finns inga dagböcker att ta bort.");
        }
    }

    public static void Search() //Söker efter en dagbok baserat på titel eller del av titel. 
    {
        if (listOfdiaries.Count > 0)
        {
            Console.WriteLine("\nSök efter dagbok:");
            string search = Console.ReadLine();
            List<Diary> searchHits = new List<Diary>();

            for(int i = 0; i<listOfdiaries.Count; i++)
            {

                if (listOfdiaries[i].Title.Contains(search))
                {
                    searchHits.Add(listOfdiaries[i]);
                }
                
                
            }

            if (searchHits.Count==0)
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
    
    public static void OpenDiary() //Skriver ut innehållet av en specifik dagbok
    {
        PrintDiaries();
        Console.WriteLine("\nVilken dagbok vill du öppna?");
        string open = Console.ReadLine();
        bool hasOpened = false;
       
        foreach (Diary d in listOfdiaries)
        {
          if (d.Title == open)
          {
              Console.WriteLine($"\n{d.Title}\n{d.DateTime}\n\n{d.Content}");
                hasOpened = true;
          }
        }

        if (hasOpened==false)
        {
            Console.WriteLine($"Kunde inte öppna "{open}", dagboken finns inte.");
        }
    }

    //Detta verkar vara onödigt komplicerat, skrotar den idén tillsvidare!!!!
    /*public static void editDiary()
    {
        Console.WriteLine("\nVad vill du redigera?\n[1]: Titel\n[2]: Brödtext");
        int editChoice = Convert.ToInt32(Console.ReadLine());

        if (editChoice == 1)
        {
            Console.WriteLine("\nTitel:");
            

        }
    }*/

    public static void importDiaries() //Importerar alla filer i mappen "Dagbok" och lägger till dom i listan med dagböcker.
    {

        if (Directory.Exists(macDirr))
        {
            string[] fileTitles = Directory.GetFiles(macDirr);
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
                string fileContent = File.ReadAllText(filePath);
                contentImport.Add(fileContent);
            
            }
            
            foreach (string filePath in fileTitles)
            {
                DateTime date = Convert.ToDateTime(File.ReadLines(filePath).Skip(1).Take(1).First());
                dateImport.Add(date);
            
            }
            
            for (int i = 0; i < titleImport.Count; i++)
            {
                Diary d = new Diary(titleImport[i], dateImport[i], contentImport[i]);
                listOfdiaries.Add(d);
            }
        }
    }

    public static void ExitSave()
    {
        if (hasSaved == false && hasCreated)
        {
            Console.WriteLine("\nDet finns osparade ändringar, är du säker på att du vill avsluta programmet?");
            Console.WriteLine("[Enter]: Spara och avsluta.        [Backspace]: Avsluta utan att spara ");
            bool validKey=false;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                
                if (key.Key == ConsoleKey.Enter)
                {
                    SaveDiary();
                    validKey = true;

                }
            
                else if (key.Key == ConsoleKey.Backspace)
                {
                    Environment.Exit(0);
                    validKey = true;
                }
            } while (validKey==false);
        }
    }
}