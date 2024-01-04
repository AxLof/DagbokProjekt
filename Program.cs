using Projekt_2._2;


int userSelect = 0;
Diary.importDiaries();
while (userSelect != 7)
{
    printMenu();

    try
    {
        userSelect = Convert.ToInt16(Console.ReadLine()); 
        switch (userSelect)                               
        {                                                 
            case 1:                                       
                Diary.AddDiary();                         
                break;                                    
            case 2:                                       
                Diary.PrintDiaries();                     
                break;                                    
            case 3:                                       
                Diary.SaveDiary();                        
                break;                                    
            case 4:                                       
                Diary.RemoveDiary();                           
                break;       
            case 5:
                Diary.Search();
                break;
            case 6:
                Diary.OpenDiary();
                break;
            case 7:
                Diary.ExitSave();
                userSelect = 7;
                break;
            default:
                Console.WriteLine("\nSkriv in en giltig siffra!");
                break;
                                                  
        }                                                 
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}








void printMenu()
{
    Console.WriteLine("\nVälj ett alternativ:");
    Console.WriteLine("--------------------");
    Console.WriteLine("1. Skapa en ny dagbok");
    Console.WriteLine("2. Visa alla dagböcker");
    Console.WriteLine("3. Spara dagbok");
    Console.WriteLine("4. Ta bort dagbok");
    Console.WriteLine("5. Sök efter dagbok"); 
    Console.WriteLine("6. Öppna en dagbok");
    Console.WriteLine("7. Avsluta programmet");
           
}