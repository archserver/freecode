public class Verse
{
    private string _verseText;
    private int _verseNumber;

    public Verse()
    {

    }

    public int GetVerseNumber()
    {
        return _verseNumber;
    }
    public string GetVerseText()
    {
        return _verseText;
    }

    public void SetVerseNumber(int verseNumber)
    {
        _verseNumber = verseNumber;
    }

    public void SetVerseText(string verseText)
    {
         _verseText = verseText;
    }
}
