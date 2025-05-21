namespace GameLauncher;

public static class EasingFunctions
{
    public static double Linear(double t) => t;

    public static double EaseIn(double t) => t * t;

    public static double EaseOut(double t) => t * (2 - t);

    public static double EaseInOut(double t) => t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
}
