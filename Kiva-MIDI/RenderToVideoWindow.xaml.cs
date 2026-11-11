using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Kiva
{
    public partial class RenderToVideoWindow : Window
    {
        readonly MainWindow mainWindow;
        bool isRendering;

        public RenderToVideoWindow(MainWindow owner, string currentMidiPath)
        {
            InitializeComponent();
            mainWindow = owner;
            midiPathText.Text = currentMidiPath ?? string.Empty;

            string bundledFfmpeg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            if (File.Exists(bundledFfmpeg))
                ffmpegPathText.Text = bundledFfmpeg;

            string currentMidi = string.IsNullOrWhiteSpace(currentMidiPath) ? null : Path.GetFileNameWithoutExtension(currentMidiPath);
            outputPathText.Text = currentMidi == null ? string.Empty : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), currentMidi + ".mp4");
        }

        private void BrowseMidi_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose a MIDI file",
                Filter = "MIDI files (*.mid;*.midi)|*.mid;*.midi|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog(this) == true)
                midiPathText.Text = dialog.FileName;
        }

        private void BrowseFfmpeg_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose ffmpeg.exe",
                Filter = "FFmpeg executable (ffmpeg.exe)|ffmpeg.exe|Executable files (*.exe)|*.exe"
            };
            if (dialog.ShowDialog(this) == true)
                ffmpegPathText.Text = dialog.FileName;
        }

        private void BrowseAudio_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose rendered MIDI audio",
                Filter = "Audio files|*.wav;*.ogg;*.flac;*.mp3;*.m4a;*.aac;*.opus;*.wma|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog(this) == true)
                audioPathText.Text = dialog.FileName;
        }

        private void BrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            ChooseOutputPath();
        }

        private bool ChooseOutputPath()
        {
            string suggested = outputPathText.Text;
            if (string.IsNullOrWhiteSpace(suggested))
                suggested = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Tree Render.mp4");

            var dialog = new SaveFileDialog
            {
                Title = "Save rendered video",
                Filter = "MP4 video (*.mp4)|*.mp4",
                DefaultExt = ".mp4",
                AddExtension = true,
                FileName = Path.GetFileName(suggested)
            };
            string directory = Path.GetDirectoryName(suggested);
            if (Directory.Exists(directory))
                dialog.InitialDirectory = directory;

            if (dialog.ShowDialog(this) != true)
                return false;

            outputPathText.Text = dialog.FileName;
            return true;
        }

        private async void Render_Click(object sender, RoutedEventArgs e)
        {
            string midiPath = string.IsNullOrWhiteSpace(midiPathText.Text) ? mainWindow.CurrentMidiPath : midiPathText.Text.Trim();
            string ffmpegPath = ffmpegPathText.Text.Trim();
            string audioPath = audioPathText.Text.Trim();
            string outputPath = outputPathText.Text.Trim();

            if (string.IsNullOrWhiteSpace(midiPath) || !File.Exists(midiPath))
            {
                statusText.Text = "Choose an existing MIDI file.";
                return;
            }
            if (string.IsNullOrWhiteSpace(ffmpegPath) || !File.Exists(ffmpegPath))
            {
                statusText.Text = "Choose the ffmpeg.exe file.";
                return;
            }
            if (string.IsNullOrWhiteSpace(audioPath) || !File.Exists(audioPath))
            {
                statusText.Text = "Choose audio rendered from the MIDI file.";
                return;
            }
            if (string.IsNullOrWhiteSpace(outputPath) && !ChooseOutputPath())
                return;
            if (!outputPathText.Text.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                outputPathText.Text += ".mp4";

            isRendering = true;
            renderButton.IsEnabled = false;
            closeButton.IsEnabled = false;
            renderProgress.Value = 0;
            statusText.Text = "Starting FFmpeg...";

            try
            {
                await mainWindow.RenderVideoAsync(midiPath, ffmpegPath, audioPath, outputPathText.Text,
                    (current, total) =>
                    {
                        renderProgress.Value = total == 0 ? 0 : current * 100.0 / total;
                        statusText.Text = "Rendering frame " + current.ToString("N0") + " of " + total.ToString("N0");
                    });
                renderProgress.Value = 100;
                statusText.Text = "Video saved: " + outputPathText.Text;
            }
            catch (Exception ex)
            {
                statusText.Text = "Rendering failed.";
                MessageBox.Show(this, ex.Message, "Render to Video", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isRendering = false;
                renderButton.IsEnabled = true;
                closeButton.IsEnabled = true;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (!isRendering)
                Close();
        }
    }
}