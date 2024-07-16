public class Component
{
    public string Type { get; set; }
    public string Sub_type { get; set; }
    public int Index { get; set; }
    public object [] Parameters { get; set; }

    public Component(string type, string sub_type, int index, object [] parameters)
    {
        Type = type;
        Sub_type = sub_type;
        Index = index;
        Parameters = parameters;
    }
}