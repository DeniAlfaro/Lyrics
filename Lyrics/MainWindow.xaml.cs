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

        int indexActual;

        bool dragging = false;
        public MainWindow()
        {
            InitializeComponent();
            indexActual = 0;
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

            var segundosActuales = reader.CurrentTime.TotalSeconds;
            var segundosTotales = reader.TotalTime.TotalSeconds;
            var listaCambiosSegundos = new List<int>() {
                7,
                13,
                17,
                28,
                49,
                64,
                78,
                95,
                103,
                110,
                120,
                131,
                152,
                163,
                181,
                197,
                205,
                212,
                260
            };
            var listaCambiosTextos = new List<String>() {
                "When your legs don't work like they used to before",
                "And I can't sweep you off of your feet",
                "Will your mouth still remember the taste of my love? \n Will your eyes still smile from your cheeks",
                "And darling I will be loving you 'til we're 70 \n And baby my heart could still fall as hard at 23",
                "And I'm thinking 'bout how people fall in love in mysterious ways \n Maybe just the touch of a hand",
                "Well me I fall in love with you every single day \n And I just wanna tell you I am",
                "So honey now \nTake me into your loving arms \n Kiss me under the light of a thousand stars",
                "Place your head on my beating heart \n I'm thinking out loud",
                "Maybe we found love right where we are",
                "When my hair's all but gone and my memory fades \nAnd the crowds don't remember my name",
                "When my hands don't play the strings the same way, \nI know you will still love me the same",
                "Cause honey your soul can never grow old, it's evergreen \nBaby your smile's forever in my mind and memory",
                "I'm thinking 'bout how people \nfall in love in mysterious ways",
                "Maybe it's all part of a plan \nI'll just keep on making the same mistakes \nHoping that you'll understand",
                "But baby now \nTake me into your loving arms \nKiss me under the light of a thousand stars",
                "Place your head on my beating heart \n I'm thinking out loud",
                "That maybe we found love right where we are, oh",
                "So baby now \nTake me into your loving arms \nKiss me under the light of a thousand stars",
                "Oh darling, place your head on my beating heart \nI'm thinking out loud",
                "That maybe we found love right where we are \nOh maybe we found love right where we are \nAnd we found love right where we are",
            };

            var segundosCambioSiguiente = listaCambiosSegundos[indexActual];

            if (segundosActuales >= segundosCambioSiguiente)
            {
                var textoCambioSiguiente = listaCambiosTextos[indexActual];
                txtLetra.Text = textoCambioSiguiente;

                if (listaCambiosSegundos.Count > indexActual + 1)
                {
                    indexActual++;
                }
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
            txtLetra.Visibility = Visibility.Visible;

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

/*
 * When your legs don't work like they used to before
And I can't sweep you off of your feet
Will your mouth still remember the taste of my love
Will your eyes still smile from your cheeks
And darling I will be loving you 'til we're 70
And baby my heart could still fall as hard at 23
And I'm thinking 'bout how people fall in love in mysterious ways
Maybe just the touch of a hand
Oh me I fall in love with you every single day
And I just wanna tell you I am
So honey now
Take me into your loving arms
Kiss me under the light of a thousand stars
Place your head on my beating heart
I'm thinking out loud
Maybe we found love right where we are
When my hair's all but gone and my memory fades
And the crowds don't remember my name
When my hands don't play the strings the same way, mm
I know you will still love me the same
'Cause honey your soul can never grow old, it's evergreen
Baby your smile's forever in my mind and memory
I'm thinking 'bout how people fall in love in mysterious ways
Maybe it's all part of a plan
I'll just keep on making the same mistakes
Hoping that you'll understand
But baby now
Take me into your loving arms
Kiss me under the light of a thousand stars
Place your head on my beating heart
I'm thinking out loud
That maybe we found love right where we are, oh
So baby now
Take me into your loving arms
Kiss me under the light of a thousand stars
Oh darling, place your head on my beating heart
I'm thinking out loud
That maybe we found love right where we are
Oh maybe we found love right where we are
And we found love right where we are
*/
