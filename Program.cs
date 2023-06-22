using System;

class Program
{
    static void Main(string[] args)
    {
        // declare variables
        string filename = "files/new-testament.json";
        int bookSelection, chapterSelection, verseSelectionLow, verseSelectionHigh;


//        List<Book> books = ReadBookfromJsonFormat(filename); - Issue with auto Parsing content to structure
        // Read in New Testiment Json File to a string
        string FileContent = File.ReadAllText(filename);
        // Parser text to structure
        List<Book> books = MyOwnParser(FileContent);


        // Find out what book they want to use
        bookSelection = GetABookSelection(books);

        Book selectedBook = books[bookSelection];
        Console.WriteLine($"The Book selected was number {bookSelection+1} {selectedBook.GetName()}");
        
        // Find out which chapter of the book they want to use
        chapterSelection = GetAChapterSelection(selectedBook);

        Chapter selectedChapter = selectedBook.GetChapters()[chapterSelection];
        Console.WriteLine($"The Chapter {chapterSelection+1} was selected out of {selectedBook.GetChapters().Count}");

        // Find out which verses in the chapter they want to use
        (verseSelectionLow, verseSelectionHigh) = GetVerseRangeSelection(selectedChapter);

        for (int i = verseSelectionLow; i <= verseSelectionHigh; i++)
        {
            Verse selectedVerse = selectedChapter.GetVerses()[i];
            Console.WriteLine($"Verse {selectedVerse.GetVerseNumber()}: {selectedVerse.GetVerseText()}");
        }
    }

    
/*    static List<Book> ReadBookfromJsonFormat(string nameoffile)
    {
        string entirescripture = File.ReadAllText(nameoffile);
        return JsonSerializer.Deserialize<List<Book>>(entirescripture);
    }*/

    static List<Book> MyOwnParser(string contentinahugestring)
    {
        // create a list of all of the books compled together throughout this parser
        List<Book> books = new List<Book>();

        // get an array of all of the lines
        string[] lines = contentinahugestring.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // set variables to be used 
        Book currentBook = null;
        Chapter currentChapter = null;
        bool startverses = false;

        // search through each line to parse out the array of lines to build the structure of books, chapters, verses
        foreach (string line in lines)
        {
            // remove , at the end of each line
            string trimmedLine = line.Trim().TrimEnd(',');

            // for testing to identify parsing points
            //Console.Write($"{trimmedLine}");
            //Console.ReadLine();
            
            // first check if we are on a verse since this will be the most comon type of parse
            if (startverses)
            {
                // if at the end of verses for a chapter turn off the verse parsing
                if (trimmedLine == "]")
                {
                    startverses = false;
                }
                else if (trimmedLine.StartsWith("\"text\":"))
                {
                    // get the verse text by taking everything after the : and removing the "
                    string verseText = trimmedLine.Split(':')[1].Trim().Trim('\"');
                    // create a verse instance and add it to the current chapter's verses
                    Verse verse = new Verse();
                    verse.SetVerseText(verseText);
                    // write the verse to the chapter
                    currentChapter.GetVerses().Add(verse);
                }
                else if(trimmedLine.StartsWith("\"verse\":"))
                {
                    int verseNumber = int.Parse(trimmedLine.Split(':')[1]);
                    // find the last verse which was added to the current chapter and set the verse number
                    Verse lastVerse = currentChapter.GetVerses().LastOrDefault();
                    if (lastVerse != null)
                    {
                        lastVerse.SetVerseNumber(verseNumber);
                    }
                    else
                    {
                        Console.WriteLine("*****Error could not find and write verse number*****");
                    }
                }

            }
            else if (trimmedLine.StartsWith("\"book\":"))
            {
                string bookName = trimmedLine.Split(':')[1].Trim().Trim('\"');
                currentBook = new Book();
                currentBook.SetName(bookName);
                books.Add(currentBook);
            }
            else if (trimmedLine.StartsWith("\"chapter\":"))
            {
                int chapterNumber = int.Parse(trimmedLine.Split(':')[1]);
                currentChapter = new Chapter();
                currentChapter.SetChapterNumber(chapterNumber);
                currentBook.GetChapters().Add(currentChapter);
            }
            else if (trimmedLine.StartsWith("\"reference\":") || trimmedLine.StartsWith("\"full_title\":")||
            trimmedLine.StartsWith("\"lds_slug\":")||trimmedLine.StartsWith("\"note\":")||trimmedLine.StartsWith("\"last_modified\":")
            ||trimmedLine.StartsWith("\"title\":")|| trimmedLine.StartsWith("\"title_page\":")|| trimmedLine.StartsWith("\"subtitle\":") || 
            trimmedLine.StartsWith("\"text\":")|| trimmedLine.StartsWith("\"version\":"))
            {
                // Ignore these lines, as they are not needed in the parsing process
            }
            else if (trimmedLine.StartsWith("\"verses\":"))
            {
                startverses = true;
            }
        }

        return books;
    }
    
    static int GetABookSelection(List<Book> listofbooks)
    {
        string choice = "-1";
        int selection = -1;

        do{
            Console.WriteLine("Select the Number next to the book you would like to get a scripture from");
            for(int b = 0; b < listofbooks.Count; b++)
            {
                
                Console.Write($"{b+1}. {listofbooks[b].GetName()}   ");
            }
            Console.WriteLine("");
            Console.Write("> ");
            choice = Console.ReadLine();

        }while (!int.TryParse(choice, out selection) || selection < 1 || selection > listofbooks.Count +1);
        return selection -1;
    }

    static int GetAChapterSelection(Book book)
    {
     string choice = "-1";
    int selection = -1;
    int chapterCount = book.GetChapters().Count;

    do
    {
        Console.WriteLine($"Select the chapter number for the book: {book.GetName()}");
        Console.WriteLine($"Enter a number from 1 to {chapterCount}");
        Console.Write("> ");
        choice = Console.ReadLine();

    } while (!int.TryParse(choice, out selection) || selection < 1 || selection > chapterCount);

    return selection - 1;
    }

    static (int, int) GetVerseRangeSelection(Chapter chapter)
    {
        string startChoice = "-1";
        string endChoice = "-1";
        int startSelection = -1;
        int endSelection = -1;
        int verseCount = chapter.GetVerses().Count;

        do
        {
            Console.WriteLine($"Select the starting and ending verse numbers for the chapter: {chapter.GetChapterNumber()}");
            Console.WriteLine($"Enter starting verse number (1 - {verseCount}):");
            Console.Write("> ");
            startChoice = Console.ReadLine();

            Console.WriteLine($"Enter ending verse number ({startChoice} - {verseCount}):");
            Console.Write("> ");
            endChoice = Console.ReadLine();

            if (endChoice == "")
            {
                endChoice = startChoice;
            }

        } while (!int.TryParse(startChoice, out startSelection) || !int.TryParse(endChoice, out endSelection) ||
                startSelection < 1 || endSelection < 1 || startSelection > verseCount || endSelection > verseCount ||
                startSelection > endSelection);

        return (startSelection - 1, endSelection - 1);
    }
}