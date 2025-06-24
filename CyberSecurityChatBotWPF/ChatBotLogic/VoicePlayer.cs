using System;
using System.Windows;
using NAudio.Wave;


namespace CybersecurityBot.Bot
{
    public static class VoicePlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                using (AudioFileReader audioFile = new AudioFileReader("ChatBot.wav"))
                using (WaveOutEvent outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                // Show a message box in WPF instead of using Console
                MessageBox.Show("Error playing voice greeting:\n" + ex.Message, "Audio Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
