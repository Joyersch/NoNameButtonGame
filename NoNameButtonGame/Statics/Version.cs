using System.Reflection;

namespace NoNameButtonGame.Statics;


public static class Version
{
    public new static string ToString()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        return $"v{assemblyVersion!.Major}.{assemblyVersion!.Minor}.{assemblyVersion!.Build}";
    }
}