public class Parameter
{
    public string Type { get; set; }
    public string Text { get; set; }

    public Parameter(string type, string text)
    {
        Type = type;
        Text = text;
    }
}