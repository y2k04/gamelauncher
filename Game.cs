﻿namespace GameLauncher
{
    public class Game
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Arguments { get; set; }
        public string ArtworkPath { get; set; }
        public int PlayTime { get; set; }

        public Game(string name, string location, string arguments, string artworkPath, int playTime = 0)
        {
            Name = name;
            Location = location;
            Arguments = arguments;
            ArtworkPath = artworkPath;
            PlayTime = playTime;
        }
    }
}
