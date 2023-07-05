using LyricControl.UWP.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.Mime.MediaTypeNames;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace LyricControl.UWP.Controls
{
    public sealed partial class LyricControl : UserControl
    {
        public LyricControl()
        {
            this.InitializeComponent();
            this.Unloaded += LyricControl_Unloaded;
            this.Loaded += LyricControl_Loaded;
        }

        private void LyricControl_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasControl.Update += CanvasControl_Update; ;
            CanvasControl.Draw += CanvasControl_Draw; ;
            CanvasControl.Paused = false;
        }

        private void LyricControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CanvasControl.Update -= CanvasControl_Update;
            CanvasControl.Draw -= CanvasControl_Draw;
            CanvasControl.Paused = true;
        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            using (var textFormat = new CanvasTextFormat
            {
                FontSize = _fontSize,
                HorizontalAlignment = _horizontalTextAlignment,
                VerticalAlignment = _verticalTextAlignment,
                Options = CanvasDrawTextOptions.EnableColorFont,
                WordWrapping = _wordWrapping,
                Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
                FontStyle = _fontStyle,
                FontWeight = _fontWeight,
                FontFamily = _textFontFamily
            })

            using (var textLayout = new CanvasTextLayout(args.DrawingSession,_lyric.Text,textFormat,(float)sender.Size.Width,(float)sender.Size.Height))
            {
                args.DrawingSession.DrawTextLayout(textLayout,0,0,_lyricColor);

                var cl = new CanvasCommandList(sender);
                using (CanvasDrawingSession clds = cl.CreateDrawingSession())
                {
                    clds.DrawTextLayout(textLayout, 0, 0, _accentLyricColor);
                }

                var accentLyric = new CropEffect
                {
                    Source = cl,
                    SourceRectangle = new Rect(textLayout.LayoutBounds.Left, textLayout.LayoutBounds.Top, GetCropWidth(_currentTime,_lyric,textLayout),textLayout.LayoutBounds.Height),
                };
                var shadow = new ColorMatrixEffect
                {
                    Source = new GaussianBlurEffect
                    {
                        BlurAmount = _blurAmount,
                        Source = accentLyric,
                        BorderMode = EffectBorderMode.Soft
                    },
                    ColorMatrix = GetColorMatrix(_shadowColor)
                };
                args.DrawingSession.DrawImage(shadow);
                args.DrawingSession.DrawImage(accentLyric);
            }
        }

        private Matrix5x4 GetColorMatrix(Color color)
        {
            var matrix = new Matrix5x4();

            var R = ((float)color.R - 128) / 128;
            var G = ((float)color.G - 128) / 128;
            var B = ((float)color.B - 128) / 128;

            matrix.M11 = R;
            matrix.M12 = G;
            matrix.M13 = B;

            matrix.M21 = R;
            matrix.M22 = G;
            matrix.M23 = B;

            matrix.M31 = R;
            matrix.M32 = G;
            matrix.M33 = B;

            matrix.M44 = 1;

            return matrix;
        }

        private double GetCropWidth(TimeSpan currentTime,Lyric lyric,CanvasTextLayout textLayout)
        {
            if(lyric.IsKaraokeLyric)
            {
                var lyrics = lyric.KaraokeLyrics;
                var time = TimeSpan.Zero;
                var currentLyric = lyrics.Last();
                //获取播放中单词在歌词的位置
                foreach(var item in lyric.KaraokeLyrics)
                {
                    if(item.Duration + time > currentTime)
                    {
                        currentLyric = item;
                        break;
                    }
                    time += item.Duration;
                }

                var index = lyrics.IndexOf(currentLyric);
                var position = lyrics.GetRange(0,index).Sum(p => p.Length);
                var startTime = TimeSpan.FromMilliseconds(lyrics.GetRange(0, index).Sum(p => p.Duration.TotalMilliseconds));
                //获取已经播放的长度
                var playedWidth = textLayout.GetCharacterRegions(0,position).Sum(p => p.LayoutBounds.Width);
                //获取正在播放单词的长度
                var currentWidth = textLayout.GetCharacterRegions(position, currentLyric.Length).Sum(p => p.LayoutBounds.Width);
                //计算占比
                var playingWidth = _easeFunction.Ease((currentTime - startTime) / currentLyric.Duration) * currentWidth;
                //求和
                var width = playedWidth + playingWidth;
                return width;  
            }
            else//非卡拉OK歌词根据已播放时长占总时长的比例计算
            {
                return _easeFunction.Ease((currentTime / lyric.Duration) * textLayout.LayoutBounds.Width);
            }
        }

        private void CanvasControl_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
             
        }
    }
}
