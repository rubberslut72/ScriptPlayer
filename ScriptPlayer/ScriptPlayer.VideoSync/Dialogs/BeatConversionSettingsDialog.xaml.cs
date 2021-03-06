﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScriptPlayer.Shared;
using ScriptPlayer.Shared.Scripts;
using ScriptPlayer.VideoSync.Controls;

namespace ScriptPlayer.VideoSync.Dialogs
{
    /// <summary>
    /// Interaction logic for BeatConversionSettingsDialog.xaml
    /// </summary>
    public partial class BeatConversionSettingsDialog : Window
    {
        public static readonly DependencyProperty ConversionModesProperty = DependencyProperty.Register(
            "ConversionModes", typeof(List<ConversionMode>), typeof(BeatConversionSettingsDialog), new PropertyMetadata(default(List<ConversionMode>)));

        public List<ConversionMode> ConversionModes
        {
            get { return (List<ConversionMode>) GetValue(ConversionModesProperty); }
            set { SetValue(ConversionModesProperty, value); }
        }

        public static readonly DependencyProperty ConversionModeProperty = DependencyProperty.Register(
            "ConversionMode", typeof(ConversionMode), typeof(BeatConversionSettingsDialog), new PropertyMetadata(default(ConversionMode)));

        public ConversionMode ConversionMode
        {
            get { return (ConversionMode) GetValue(ConversionModeProperty); }
            set { SetValue(ConversionModeProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(Byte), typeof(BeatConversionSettingsDialog), new PropertyMetadata(default(Byte)));

        public Byte MinValue
        {
            get { return (Byte) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(Byte), typeof(BeatConversionSettingsDialog), new PropertyMetadata(default(Byte)));

        public Byte MaxValue
        {
            get { return (Byte) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register(
            "Result", typeof(ConversionSettings), typeof(BeatConversionSettingsDialog), new PropertyMetadata(default(ConversionSettings)));
        
        private static RelativePositionCollection _previousPattern;
        private static bool[] _previousBeats;

        public static void SetPattern(RelativePositionCollection collection)
        {
            _previousPattern = collection;
        }

        public ConversionSettings Result
        {
            get { return (ConversionSettings) GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public BeatConversionSettingsDialog(ConversionSettings initialValues = null)
        {
            ConversionModes = new List<ConversionMode>(Enum.GetValues(typeof(ConversionMode)).Cast<ConversionMode>());

            if (initialValues == null)
            {
                ConversionMode = ConversionMode.UpOrDown;
                MinValue = 0;
                MaxValue = 99;
            }
            else
            {
                ConversionMode = initialValues.Mode;
                MinValue = initialValues.Min;
                MaxValue = initialValues.Max;
            }

            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            ((Button) sender).Focus();

            Result = new ConversionSettings
            {
                Min = MinValue,
                Max = MaxValue,
                Mode = ConversionMode
            };

            if (ConversionMode == ConversionMode.Custom)
            {
                CustomConversionDialog dialog = new CustomConversionDialog(
                    _previousPattern ?? new RelativePositionCollection(),
                    _previousBeats ?? new []{true,true}){Owner = this};

                if (dialog.ShowDialog() != true)
                    return;

                _previousPattern = dialog.Positions;
                _previousBeats = dialog.BeatPattern;

                Result.CustomPositions = dialog.Positions;
                Result.BeatPattern = dialog.BeatPattern;
            }

            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MinValue = 0;
            MaxValue = 99;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MinValue = 50;
            MaxValue = 99;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MinValue = 0;
            MaxValue = 50;
        }
    }
}
