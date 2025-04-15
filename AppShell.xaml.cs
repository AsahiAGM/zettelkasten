using System.Diagnostics;

namespace Zettelkasten
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        // 新規カードを生成するボタンがクリックされたときの処理
        private void OnAddCardClicked(object sender, EventArgs e)
        {
            MainPage.GenerateCard();
        }

        // メニューボタンがクリックされたときの処理
        private void OnMenuClicked(object sender, EventArgs e)
        {
            DisplayAlert("メニュー", "メニューが表示されました。", "OK");
        }
    }
}
