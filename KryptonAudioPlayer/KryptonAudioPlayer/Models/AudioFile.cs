using Avalonia.Media.Imaging;

namespace KryptonAudioPlayer.Models
{
    public class AudioFile
    {
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? FilePath { get; set; }
        public Bitmap? CoverImage { get; set; }
        public string? Duration { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AudioFile))
                return false;

            var other = (AudioFile)obj;
            return Artist == other.Artist && Title == other.Title && FilePath == other.FilePath;
        }

        public override int GetHashCode()
        {
            return Artist.GetHashCode() ^ Title.GetHashCode() ^ FilePath.GetHashCode();
        }
    }
}

