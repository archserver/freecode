public class Book
{
    private string _bookName;
    private List<Chapter> _chapters;

    public Book()
    {
        _chapters = new List<Chapter>();
    }

    public string GetName()
    {
        return _bookName;
    }

    public void SetName( string bookName)
    {
      _bookName = bookName;
    }

    public List<Chapter> GetChapters()
    {
        return _chapters;
    }

    public void SetChapters (List<Chapter> chapters)
    {
        _chapters = chapters;
    }
}