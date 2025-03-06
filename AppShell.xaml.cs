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
            // 新しいカードを生成する処理をここに記述
            //DisplayAlert("新規カード", "新しいカードが生成されました。", "OK");
            TableComponent.MainPage.GenerateCard();
        }

        // メニューボタンがクリックされたときの処理
        private void OnMenuClicked(object sender, EventArgs e)
        {
            // メニューを表示する処理をここに記述
            DisplayAlert("メニュー", "メニューが表示されました。", "OK");
        }
    }
}
