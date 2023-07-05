using System;
using System.Collections.Generic;
using System.Linq;

namespace LyricControl.UWP.Models
{
    public class Lyric
    {
        public string Text { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsKaraokeLyric => !(KaraokeLyrics is null);

        public List<KaraokeLyric> KaraokeLyrics { get; set; }

        public static Lyric CreateSample()
        {
            var lyric = new Lyric
            {
                StartTime = TimeSpan.Zero,
                KaraokeLyrics = new List<KaraokeLyric>
                {
                    new KaraokeLyric
                    {
                        Text = "Lorem ipsum",
                        Duration = TimeSpan.FromSeconds(0.5),
                    },
                    new KaraokeLyricSpace(),
                    new KaraokeLyric
                    {
                        Text = "dolor",
                        Duration = TimeSpan.FromSeconds(2),
                    },
                    new KaraokeLyricSpace(),
                    new KaraokeLyric
                    {
                        Text = "sit amet",
                        Duration = TimeSpan.FromSeconds(1),
                    },
                    new KaraokeLyricSpace(),
                    new KaraokeLyric
                    {
                        Text = "consectetur",
                        Duration = TimeSpan.FromSeconds(0.5),
                    },
                    new KaraokeLyricSpace(),
                    new KaraokeLyric
                    {
                        Text = "adipisicing",
                        Duration = TimeSpan.FromSeconds(2),
                    },
                    new KaraokeLyricSpace(),
                    new KaraokeLyric
                    {
                        Text = "elit",
                        Duration = TimeSpan.FromSeconds(1),
                    },
                },
            };
            var sb = new System.Text.StringBuilder();
            foreach (var item in lyric.KaraokeLyrics)
            {
                sb.Append(item.Text);
            }
            lyric.Text = sb.ToString();
            lyric.Duration = TimeSpan.FromTicks(lyric.KaraokeLyrics.Sum(p => p.Duration.Ticks));
            return lyric;
        }
    }
    public class KaraokeLyric
    {
        public string Text { get; set; }
        public TimeSpan Duration { get; set; }
        //public int Position{ get; set; }//首个字母在所有歌词中的位置
        public int Length => Text.Length;
    }
    public class KaraokeLyricSpace : KaraokeLyric
    {
        public KaraokeLyricSpace()
        {
            this.Text = " ";
            this.Duration = TimeSpan.Zero;
        }
    }


}