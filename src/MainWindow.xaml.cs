using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;

namespace Gopro360App
{
    public partial class MainWindow : Window
    {
        private GoProWifiService gopro;
        private string publishDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UPEvents360_Publish");

        public MainWindow()
        {
            InitializeComponent();
            gopro = new GoProWifiService();
            Log("App started.");
            Directory.CreateDirectory(publishDir);
        }

        private void Log(string text)
        {
            Dispatcher.Invoke(() =>
            {
                LogBox.Text += ">> " + DateTime.Now.ToString("HH:mm:ss") + " - " + text + "\n";
            });
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Log("Attempting to ping GoPro...");
            var ok = await gopro.PingAsync();
            if (!ok) { Log("GoPro not reachable. Connect to GoPro Wi-Fi first."); return; }
            await gopro.SetModeVideoAsync();
            var started = await gopro.StartRecordingAsync();
            if (started) { Log("Recording started on GoPro."); BtnStart.IsEnabled = false; BtnStop.IsEnabled = true; }
            else Log("Failed to start recording.");
        }

        private async void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            var stopped = await gopro.StopRecordingAsync();
            if (stopped) { Log("Recording stopped on GoPro."); BtnStart.IsEnabled = true; BtnStop.IsEnabled = false; }
            else Log("Failed to stop recording.");
        }

        private async void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            Log("Listing files on GoPro...");
            var files = await gopro.ListFilesAsync();
            if (files == null || files.Count==0) { Log("No files found on GoPro."); return; }
            var latest = files[files.Count-1];
            Log("Downloading: " + latest);
            var dest = System.IO.Path.Combine(publishDir, System.IO.Path.GetFileName(new Uri(latest).LocalPath));
            MainProgress.Value = 0;
            var ok = await gopro.DownloadFileAsync(latest, dest);
            if (ok) { Log("Downloaded to: " + dest); MainProgress.Value = 100; }
            else Log("Download failed.");
        }

        private async void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            Log("Triggering build script (PowerShell)");
            // The actual build script will be run by GitHub Actions or locally via PowerShell.
            // Here we simulate progress for the user.
            MainProgress.Value = 0;
            for (int i=0;i<=100;i+=10)
            {
                MainProgress.Value = i;
                await Task.Delay(180);
            }
            Log("Build simulation complete. Use GitHub Actions to produce installer.");
        }
    }
}
