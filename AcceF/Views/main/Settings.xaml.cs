using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Popups;
using Windows.Storage;
using AcceF.ModelViews;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using System.IO.Compression;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private string clientId = "c769467f-cf86-4f84-bad7-b0bbaaec8b94";
        private string returnUrl = "https://login.live.com/oauth20_desktop.srf";
        private OneDriveClient client = null;
        private OnlineIdAuthenticationProvider msaProvider = null;
        private readonly string[] scopes =
        {
            "onedrive.readwrite",
            "onedrive.appfolder",
            "wl.signin",
        };

        private bool isLoggedIn = false;
        public Settings()
        {

            this.InitializeComponent();
            

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await loggin();
        }
        private async void Down_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Une synchronisation descendante remplacera les données de votre application AcceF par celles contenues dans OneDrive. \n Voulez vous procédez?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                progressRing.Visibility = Visibility.Visible;
                progressRing.IsActive = true;
                if (!this.msaProvider.IsAuthenticated || this.client == null)
                {
                    await loggin();
                }

                var builder = this.client.Drive.Root.ItemWithPath("AcceF/acceF.db");
                var contentStream = await builder.Content.Request().GetAsync();
                StorageFile db = await ApplicationData.Current.LocalFolder.CreateFileAsync("acceF.db", CreationCollisionOption.ReplaceExisting);
                downloader(contentStream, db);

                var builderZip = this.client.Drive.Root.ItemWithPath("AcceF/file.zip");
                var contentStreamZip = await builderZip.Content.Request().GetAsync();
                StorageFile zip = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("file.zip", CreationCollisionOption.ReplaceExisting);
                downloader(contentStreamZip, zip);
                StorageFolder files = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Files", CreationCollisionOption.OpenIfExists);
                await files.DeleteAsync();
                files = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Files", CreationCollisionOption.OpenIfExists);
                ZipFile.ExtractToDirectory(zip.Path, files.Path);
                await zip.DeleteAsync();
                progressRing.IsActive = false;
                progressRing.Visibility = Visibility.Collapsed;
            }

        }
        private async void Up_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Une synchronisation montante remplacera les données de OneDrive par celles de l'application AcceF. \n Voulez vous procédez?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                progressRing.Visibility = Visibility.Visible;
                progressRing.IsActive = true;
                if (!this.msaProvider.IsAuthenticated || this.client == null)
                {
                    await loggin();
                }

                StorageFile db = await ApplicationData.Current.LocalFolder.GetFileAsync("acceF.db");
                BasicProperties pro = await db.GetBasicPropertiesAsync();
                Debug.WriteLine(pro.Size);

                using (var contentStream = await db.OpenStreamForReadAsync())
                {
                    var uploadedItem = await this.client
                                                 .Drive
                                                 .Root
                                                 .ItemWithPath("AcceF/" + db.Name)
                                                 .Content
                                                 .Request()
                                                 .PutAsync<Item>(contentStream);
                }
                StorageFolder fileFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Files", CreationCollisionOption.OpenIfExists);
                ZipFile.CreateFromDirectory(fileFolder.Path, ApplicationData.Current.TemporaryFolder.Path + "\\file.zip", CompressionLevel.Optimal, false);

                StorageFile zip = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.TemporaryFolder.Path + "\\file.zip");
                using (var contentStream = await zip.OpenStreamForReadAsync())
                {
                    var uploadedItem = await this.client
                                                 .Drive
                                                 .Root
                                                 .ItemWithPath("AcceF/" + zip.Name)
                                                 .Content
                                                 .Request()
                                                 .PutAsync<Item>(contentStream);
                }
                await zip.DeleteAsync();
                progressRing.IsActive = false;
                progressRing.Visibility = Visibility.Collapsed;
                
            }

        }

        private async Task loggin()
        {
            msaProvider = new OnlineIdAuthenticationProvider(scopes);
            await msaProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                this.client = new OneDriveClient(msaProvider);
        }
        private async Task loggout()
        {
            await msaProvider.SignOutAsync();
        }

        private async void downloader(Stream stream, StorageFile fileOutput)
        {
            using (stream)
            {
                using (var downloadMemoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(downloadMemoryStream);
                    var fileBytes = downloadMemoryStream.ToArray();
                    await FileIO.WriteBytesAsync(fileOutput, fileBytes);
                }
            }
        }


    }
}