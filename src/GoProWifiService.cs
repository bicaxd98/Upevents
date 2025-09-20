using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Gopro360App
{
    public class GoProWifiService
    {
        private readonly HttpClient http;
        public string CameraIp { get; private set; } = "10.5.5.9";
        public Uri BaseUri => new Uri($"http://{CameraIp}/");

        public GoProWifiService()
        {
            http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(12);
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                var resp = await http.GetAsync(new Uri(BaseUri, "gp/gpControl/ping"));
                return resp.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> SetModeVideoAsync()
        {
            try { var resp = await http.GetAsync(new Uri(BaseUri, "gp/gpControl/command/mode?p=0")); return resp.IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> StartRecordingAsync()
        {
            try { var resp = await http.GetAsync(new Uri(BaseUri, "gp/gpControl/command/shutter?p=1")); return resp.IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> StopRecordingAsync()
        {
            try { var resp = await http.GetAsync(new Uri(BaseUri, "gp/gpControl/command/shutter?p=0")); return resp.IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var results = new List<string>();
            try
            {
                var candidatePaths = new[] { "videos", "videos/DCIM", "videos/DCIM/100GOPRO", "videos/100GOPRO" };
                foreach (var p in candidatePaths)
                {
                    try
                    {
                        var uri = new Uri(BaseUri, p + "/");
                        var resp = await http.GetAsync(uri);
                        if (!resp.IsSuccessStatusCode) continue;
                        var html = await resp.Content.ReadAsStringAsync();
                        var links = ParseLinksFromHtml(html);
                        foreach (var l in links)
                        {
                            var fileUri = new Uri(uri, l);
                            results.Add(fileUri.ToString());
                        }
                        if (results.Count>0) break;
                    }
                    catch { continue; }
                }
            }
            catch { }
            return results;
        }

        private List<string> ParseLinksFromHtml(string html)
        {
            var res = new List<string>();
            int idx=0;
            while(true)
            {
                var h = html.IndexOf("href=\"", idx, StringComparison.OrdinalIgnoreCase);
                if (h<0) break;
                var start = h+6;
                var end = html.IndexOf('"', start);
                if (end<0) break;
                var link = html.Substring(start, end-start);
                if (link.EndsWith("/")) { idx = end+1; continue; }
                if (link.Contains(".MP4", StringComparison.OrdinalIgnoreCase) || link.Contains(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    res.Add(link);
                }
                idx = end+1;
            }
            return res;
        }

        public async Task<bool> DownloadFileAsync(string fileUrl, string destinationPath)
        {
            try
            {
                var resp = await http.GetAsync(fileUrl);
                if (!resp.IsSuccessStatusCode) return false;
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                using var fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await resp.Content.CopyToAsync(fs);
                return true;
            }
            catch { return false; }
        }
    }
}
