using MonoUtils.Ui;

namespace NoNameButtonGame.Extensions;

public static class CameraExtension
{
    public static void DisplayOffset(this Camera sender)
        => sender.Move(Display.Size / 2);
}