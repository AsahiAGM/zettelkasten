using System.Diagnostics;

namespace Zettelkasten
{
    public partial class App : Application
    {
        public static string DatabasePath { get; private set; } = null!;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();


            // データベースファイルのパスを設定
            string dbName = "zettelkasten.db";
#if ANDROID
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#elif WINDOWS
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#elif IOS
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        folderPath = Path.Combine(folderPath, "..", "Library");
#elif MACCATALYST
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#else
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#endif
            DatabasePath = Path.Combine(folderPath, dbName);
        }
    }
}
