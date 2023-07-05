﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI;
using LyricControl.UWP.Models;
using System.ComponentModel;
using System.Drawing;
using Color = Windows.UI.Color;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Animation;

namespace LyricControl.UWP.Controls
{
    public partial class LyricControl
    {
        /// <summary>
        /// 当前播放的时间
        /// </summary>
        public int BlurAmount
        {
            get => (int)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

        public static readonly DependencyProperty BlurAmountProperty =
            DependencyProperty.Register(nameof(BlurAmount), typeof(int), typeof(LyricControl), new PropertyMetadata(16, OnBlurAmountChanged));

        private int _blurAmount = 16;

        private static void OnBlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._blurAmount = (int)e.NewValue;
        }

        /// <summary>
        /// 当前播放的时间
        /// </summary>
        public TimeSpan CurrentTime
        {
            get => (TimeSpan)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(TimeSpan), typeof(LyricControl), new PropertyMetadata(TimeSpan.Zero, OnCurrentTimeChanged));

        private TimeSpan _currentTime = TimeSpan.Zero;

        private static void OnCurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._currentTime = (TimeSpan)e.NewValue;
        }

        /// <summary>
        /// 文字样式(斜体等)
        /// </summary>
        public new FontStyle FontStyle
        {
            get => (FontStyle)GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }

        public new static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register(nameof(FontStyle), typeof(FontStyle), typeof(LyricControl), new PropertyMetadata(FontStyle.Normal, OnFontStyleChanged));

        private FontStyle _fontStyle = FontStyle.Normal;

        private static void OnFontStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._fontStyle = (FontStyle)e.NewValue;
        }

        /// <summary>
        /// 歌词播放的缓动曲线
        /// </summary>
        public EaseFunctionBase EaseFunction
        {
            get => (EaseFunctionBase)GetValue(EaseFunctionProperty);
            set => SetValue(EaseFunctionProperty, value);
        }

        public static readonly DependencyProperty EaseFunctionProperty =
            DependencyProperty.Register(nameof(EaseFunction), typeof(EaseFunctionBase), typeof(LyricControl), new PropertyMetadata(new SineEase { EasingMode = EasingMode.EaseInOut }, OnEaseFunctionChanged));

        private EaseFunctionBase _easeFunction = new SineEase { EasingMode = EasingMode.EaseInOut };

        private static void OnEaseFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._easeFunction = (EaseFunctionBase)e.NewValue;
        }

        /// <summary>
        /// 字重(粗体等)
        /// </summary>
        public new FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        public new static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register(nameof(FontWeight), typeof(FontWeight), typeof(LyricControl), new PropertyMetadata(FontWeights.SemiBold, OnFontWeightChanged));

        private FontWeight _fontWeight = FontWeights.SemiBold;

        private static void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._fontWeight = (FontWeight)e.NewValue;
        }

        /// <summary>
        /// 歌词颜色(未激活)
        /// </summary>
        public new int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public new static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(int), typeof(LyricControl), new PropertyMetadata(56, OnFontSizeChanged));

        private int _fontSize = 54;

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._fontSize = (int)e.NewValue;
        }

        /// <summary>
        /// 歌词颜色(未激活)
        /// </summary>
        public Color LyricColor
        {
            get => (Color)GetValue(LyricColorProperty);
            set => SetValue(LyricColorProperty, value);
        }

        public static readonly DependencyProperty LyricColorProperty =
            DependencyProperty.Register(nameof(LyricColor), typeof(Color), typeof(LyricControl), new PropertyMetadata(Color.FromArgb(50, 200, 200, 200),OnLyricColorChanged));

        private Color _lyricColor = Color.FromArgb(50, 200, 200, 200);

        private static void OnLyricColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._lyricColor = (Color)e.NewValue;
        }

        /// <summary>
        /// 歌词颜色(激活)
        /// </summary>

        public Color AccentLyricColor
        {
            get => (Color)GetValue(AccentLyricColorProperty);
            set => SetValue(AccentLyricColorProperty, value);
        }

        public static readonly DependencyProperty AccentLyricColorProperty =
            DependencyProperty.Register(nameof(AccentLyricColor), typeof(Color), typeof(LyricControl), new PropertyMetadata(Colors.White,OnAccentLyricColorChanged));

        private Color _accentLyricColor = Colors.White;

        private static void OnAccentLyricColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._accentLyricColor = (Color)e.NewValue;
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(LyricControl), new PropertyMetadata(Color.FromArgb(200, 0, 0, 0),OnShadowColorChanged));

        private Color _shadowColor = Color.FromArgb(200, 0, 0, 0);

        private static void OnShadowColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._shadowColor = (Color)e.NewValue;
        }
        
        /// <summary>
        /// 歌词
        /// </summary>
        public Lyric Lyric
        {
            get => (Lyric)GetValue(LyricProperty);
            set => SetValue(LyricProperty, value);
        }

        public static readonly DependencyProperty LyricProperty =
            DependencyProperty.Register(nameof(Lyric), typeof(Lyric), typeof(LyricControl), new PropertyMetadata(Lyric.CreateSample(),OnLyricChanged));

        private Lyric _lyric = Lyric.CreateSample();

        private static void OnLyricChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricControl)d)._lyric = (Lyric)e.NewValue;
        }
    }
}
