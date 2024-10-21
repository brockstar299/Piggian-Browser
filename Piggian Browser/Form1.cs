using CefSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Piggian_Browser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Subscribe to events
            chromiumWebBrowser1.TitleChanged += ChromiumWebBrowser1_TitleChanged;
            chromiumWebBrowser1.AddressChanged += ChromiumWebBrowser1_AddressChanged;

            // Check if application was launched with command line arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string fileOrUrl = args[1];
                LoadFileOrUrl(fileOrUrl);
            }
            else
            {
                // Set initial URL for chromiumWebBrowser1 (default)
                chromiumWebBrowser1.Load("https://www.google.com");
            }
        }

        private void LoadFileOrUrl(string input)
        {
            // Check internet connectivity
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                // No internet connection, load custom offline page
                LoadOfflinePage();
                return;
            }

            // Check if input is a URL
            if (Uri.TryCreate(input, UriKind.Absolute, out Uri result))
            {
                // Load URL in embedded browser
                chromiumWebBrowser1.Load(input);
            }
            else
            {
                // Check if input is a file path
                if (File.Exists(input))
                {
                    // Open directory containing the file
                    string directoryPath = Path.GetDirectoryName(input);
                    Process.Start("explorer.exe", directoryPath);
                }
                else
                {
                    // Load a 404 page if neither URL nor file path is valid
                    Load404Page();
                }
            }
        }

        private void LoadOfflinePage()
        {
            // Load custom HTML for offline page
            string offlineHtml = @"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>No Internet Connection</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            text-align: center;
                            background-color: #f0f0f0;
                            padding: 50px;
                        }
                        h1 {
                            font-size: 3em;
                            color: #333;
                        }
                        p {
                            font-size: 1.2em;
                            color: #666;
                        }
                        a {
                            color: #007bff;
                            text-decoration: none;
                        }
                        a:hover {
                            text-decoration: underline;
                        }
                    </style>
                </head>
                <body>
                    <h1>No Internet Connection</h1>
                    <p>Your computer is not connected to the internet.</p>
                    <p>Here are some things you can try:</p>
                    <ul>
                        <li>Check your network cables, modem, and router.</li>
                        <li>Restart your modem or router.</li>
                        <li>Connect to a different Wi-Fi network.</li>
                        <li>Contact your network administrator or internet service provider.</li>
                    </ul>
                </body>
                </html>";

            // Load the custom HTML content in the embedded browser
            chromiumWebBrowser1.LoadHtml(offlineHtml);
        }

        private void Load404Page()
        {
            // Load custom HTML for 404 page
            string html404 = @"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>404 - Page Not Found</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            text-align: center;
                            background-color: #f0f0f0;
                            padding: 50px;
                        }
                        h1 {
                            font-size: 3em;
                            color: #333;
                        }
                        p {
                            font-size: 1.2em;
                            color: #666;
                        }
                        a {
                            color: #007bff;
                            text-decoration: none;
                        }
                        a:hover {
                            text-decoration: underline;
                        }
                    </style>
                </head>
                <body>
                    <h1>404 - Page Not Found</h1>
                    <p>The requested page could not be found.</p>
                    <p>Please check the URL or try again later.</p>
                </body>
                </html>";

            // Load the custom HTML content in the embedded browser
            chromiumWebBrowser1.LoadHtml(html404);
        }

        private void ChromiumWebBrowser1_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            textBox1.Text = e.Address.ToString();
        }

        private void ChromiumWebBrowser1_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                this.Text = e.Title;
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();
            LoadFileOrUrl(input);
        }

        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 newForm = new Form1();
            newForm.Show();
        }

        private void toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textBox1.Text = e.ClickedItem.ToString();
            button1_Click(sender, e); // Handle the click event
        }

        private void developerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.ShowDevTools();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "https://search.yahoo.com/search?fr=mcafee&type=E210US550G0&p=" + textBox1.Text;
            button1_Click(sender, e); // Handle the click event
        }

        private void openInChromeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(textBox1.Text);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.Back();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.Forward();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            chromiumWebBrowser1.Refresh();
        }
    }
}