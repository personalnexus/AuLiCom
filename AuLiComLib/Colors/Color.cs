namespace AuLiComLib.Colors
{
    internal class Color: IColor
    {
        public Color(string name, byte red, byte green, byte blue)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public string Name { get; }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
    }
}