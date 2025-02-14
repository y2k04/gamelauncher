namespace GameLauncher
{
    public class Game(string name = "", string location = "", string arguments = "", string artworkPath = "", double playTime = 0)
    {
        public string Name { get; set; } = name;
        public string Location { get; set; } = location;
        public string Arguments { get; set; } = arguments;
        public string ArtworkPath { get; set; } = artworkPath;
        public double PlayTime { get; set; } = playTime;
    }
}
