public class Chapter
{
    private int _chapterNumber;
    private  List<Verse> _verses;

    public Chapter()
    {
        _verses = new List<Verse>();
    }

    public int GetChapterNumber()
    {
        return _chapterNumber;
    }

    public void SetChapterNumber (int chapterNumber)
    {
        _chapterNumber = chapterNumber;
    }


    public List<Verse> GetVerses()
    {
         return _verses; 
        
    }

    public void SetVerses (List<Verse> verses)
    {
        _verses = verses; 
    }
}