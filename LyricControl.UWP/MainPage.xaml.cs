using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.ServiceModel.Channels;
using System.Reflection;
using Windows.UI.Text;
using System.Drawing;
using Color = Windows.UI.Color;
using System.Text;
using Windows.ApplicationModel.AppExtensions;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.UserDataTasks;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LyricControl.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            CanvasControl.Draw += CanvasControl_Draw;
            this.Unloaded += (s, e) => this.CanvasControl.RemoveFromVisualTree();
        }




        public CanvasTextFormat TextFormat { get; set; } = new CanvasTextFormat
        {
            FontSize = 56,
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment = CanvasVerticalAlignment.Center,
            Direction = CanvasTextDirection.LeftToRightThenTopToBottom,
            FontWeight = FontWeights.SemiBold
        };

        public string Text { get; set; } = "";
         
        public int TotalTime { get; set; }
        public double NormalizedTime { get; set; }
        public int CurrentCharacter { get; set; }



        //public EasingFunctions.EaseFunctionBase EasingFunction { get; set; } = new EasingFunctions.CircleEase() { EasingMode = EasingMode.EaseOut };

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            float width = (float)(1 + Math.Sin(args.Timing.TotalTime.TotalSeconds)) * 0.5F * (float)sender.Size.Width;

            //var width = EasingFunction.Ease(NormalizedTime) * sender.Size.Width;

            DrawPrimaryText(sender , args , width);
            DrawSecondaryText(sender, args, width);
            DrawShadow(sender,args, width);

        }



        private void DrawSecondaryText(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, double width)
        {
            CanvasCommandList cl = new CanvasCommandList(sender);
            using (CanvasDrawingSession clds = cl.CreateDrawingSession())
            {
                var layout = new CanvasTextLayout(clds, Text, TextFormat, (float)sender.Size.Width,
                    (float)sender.Size.Height);
                clds.DrawTextLayout(layout,0,0, Color.FromArgb(50, 200, 200, 200));

                ///clds.DrawText(Text, 16, 0,Color.FromArgb(50, 200, 200, 200), TextFormat);
            }
            var primaryText = new CropEffect
            {
                Source = cl,
                SourceRectangle = new Rect(width, 0, sender.Size.Width - width, 256)
            };
            args.DrawingSession.DrawImage(primaryText);

        }

        private void DrawPrimaryText(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args,double width)
        {
            CanvasCommandList cl = new CanvasCommandList(sender);
            double width_;
            using (CanvasDrawingSession clds = cl.CreateDrawingSession())
            {
                var layout = new CanvasTextLayout(clds, Text, TextFormat, (float)sender.Size.Width,
                    (float)sender.Size.Height);
                width_ = layout.GetCharacterRegions(0, CurrentCharacter).Sum(p => p.LayoutBounds.Width);

                clds.DrawTextLayout(layout, 0, -3, Colors.White);
                //clds.DrawText(Text, 16, -3, Colors.White, TextFormat);
            }

            var primaryText = new CropEffect
            {
                Source = cl,
                SourceRectangle = new Rect(0, 0, width_, 256)
            };
            args.DrawingSession.DrawImage(primaryText);

        }


        private void DrawShadow(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, double width)
        {
            CanvasCommandList cl = new CanvasCommandList(sender);
            using (CanvasDrawingSession clds = cl.CreateDrawingSession())
            {
                var layout = new CanvasTextLayout(clds, Text, TextFormat, (float)sender.Size.Width,
                    (float)sender.Size.Height);
                clds.DrawTextLayout(layout, 0, 4, Color.FromArgb(200, 0, 0, 0));
                
                //clds.DrawText(Text, 16, 4, Color.FromArgb(200, 0, 0, 0), TextFormat);
            }

            var shadowBase = new CropEffect
            {
                Source = cl,
                SourceRectangle = new Rect(0, 4, width, 256)
            };

            var shadow = new GaussianBlurEffect
            {
                BlurAmount = 16,
                Source = shadowBase,
                BorderMode = EffectBorderMode.Soft
            };
            args.DrawingSession.DrawImage(shadow);

        }

        private void CurrentTimeBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            LyricControl.CurrentTime = TimeSpan.FromSeconds(args.NewValue);
        }

        private void DurationBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            LyricControl.Lyric.Duration = TimeSpan.FromSeconds(args.NewValue);
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            LyricControl.CurrentTime = TimeSpan.Zero;
            while (LyricControl.CurrentTime <= LyricControl.Lyric.Duration)
            {
                await Task.Delay(10);
                LyricControl.CurrentTime += TimeSpan.FromMilliseconds(10);
            }
        }
    }

}
