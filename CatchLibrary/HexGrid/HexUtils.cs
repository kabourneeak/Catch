namespace CatchLibrary.HexGrid
{
    public static class HexUtils
    {
        public const float SIN60 = 0.8660254f;
        public const float COS60 = 0.5f;
        public const float SQRT3OVER3 = 0.5773502f;

        public static float GetRadiusHeight(float radius)
        {
            return radius * SIN60;
        }
    }
}
