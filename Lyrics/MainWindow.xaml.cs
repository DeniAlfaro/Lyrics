using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using System.Windows.Threading;

namespace Lyrics
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        //Lector de archivos
        AudioFileReader reader;
        //Comunicacion con la tarjeta de audio exclusiva
        WaveOut output;

        bool dragging = false;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //para que recorra el slider
            if (!dragging)
            {
                ProgressBar.Value = reader.CurrentTime.TotalSeconds;
            }
        }

        private void BtnReproducir_Click(object sender, RoutedEventArgs e)
        {
            string file = @"thinkingOutLoud.mp3";
            reader = new AudioFileReader(file);
            output = new WaveOut();

            output.PlaybackStopped += Output_PlaybackStopped;
            output.Init(reader);
            output.Play();

            ProgressBar.Maximum = reader.TotalTime.TotalSeconds;
            ProgressBar.Value = reader.CurrentTime.TotalSeconds;

            timer.Start();
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            timer.Stop();

            reader.Dispose();
            output.Dispose();
        }

        private void ProgressBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragging = true;
        }

        private void ProgressBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragging = false;
            if (reader != null && output != null && output.PlaybackState != PlaybackState.Stopped)
            {
                reader.CurrentTime = TimeSpan.FromSeconds(ProgressBar.Value);
            }
        }
    }
}
