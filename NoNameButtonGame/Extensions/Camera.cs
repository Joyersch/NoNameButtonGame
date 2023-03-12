namespace NoNameButtonGame.Extensions;

public static class CameraExtension
{
    public static void DisplayOffset(this Camera sender)
        => sender.Move(Display.Display.Size / 2);
}