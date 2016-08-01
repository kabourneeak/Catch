namespace CatchLibrary.HexGrid
{
    public class HexUtils
    {
        private HexUtils()
        {
            
        }

        public const float SIN60 = 0.8660254f;
        public const float COS60 = 0.5f;

        public static float GetRadiusHeight(float radius)
        {
            return radius * SIN60;
        }
    }
}
