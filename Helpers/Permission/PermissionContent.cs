namespace Monolithic.Helpers;

public class PermissionContent
{
    public string Key { get; set; }

    public string Description { get; set; }

    public PermissionContent() { }

    public PermissionContent(string Key, string Description)
    {
        this.Key = Key;
        this.Description = Description;
    }
}