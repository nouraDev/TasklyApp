namespace TaskApp.Domain.Defaults
{
    public static class ListColors
    {
        private static readonly string[] Palette =
        {
        "#FF5733", "#33C1FF", "#75FF33", "#FFC300", "#9B33FF", "#FF33A8",
        "#00C2A8", "#FF8C33", "#3D5AFE", "#00BFA5"
        };

        private static readonly Random _random = new();

        public static string GetRandomColor() => Palette[_random.Next(Palette.Length)];
    }

}
