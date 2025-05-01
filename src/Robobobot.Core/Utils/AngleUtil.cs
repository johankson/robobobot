namespace Robobobot.Core.Utils;

public static class AngleUtil
{
    public static int WrapAngle(int degrees) => (degrees % 360 + 360) % 360;
}

public static class VectorUtil
{
    public static float DegreeToRadian(float degree) =>
        (float)(Math.PI / 180) * degree;
}