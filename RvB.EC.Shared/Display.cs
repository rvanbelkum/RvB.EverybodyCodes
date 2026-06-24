namespace RvB.EC.Shared;

public static class Display {
    public static void Write(char ch, ConsoleColor foreGroundColor) {
        Console.ForegroundColor = foreGroundColor;
        Console.Write(ch);
        Console.ResetColor();
    }

    public static void Write(string text, ConsoleColor foreGroundColor) {
        Console.ForegroundColor = foreGroundColor;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void WriteLine(char ch, ConsoleColor foreGroundColor) {
        Write(ch, foreGroundColor);
        Console.WriteLine();
    }

    public static void WriteLine(string text, ConsoleColor foreGroundColor) {
        Write(text, foreGroundColor);
        Console.WriteLine();
    }
}
