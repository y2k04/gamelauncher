using System.IO;

namespace GameLauncher
{
    public class Game
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Arguments { get; set; }
        public string ArtworkPath { get; set; }

        public Game(string name, string location, string arguments, string artworkPath)
        {
            Name = name;
            Location = location;
            Arguments = arguments;
            ArtworkPath = artworkPath;
        }
    }
}
