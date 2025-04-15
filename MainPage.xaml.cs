using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;

namespace Zettelkasten
{
    /// <summary>
    /// UIとイベント
    /// </summary>
    public partial class MainPage : ContentPage
    {
        internal Anchor _dragAnchor;

        public MainPage()
        {
            Debug.WriteLine($"◆ MainPage Constructor");

            InitializeComponent();// initialize and read XAML. This generate when initializing and belong "obj".

            // init
            FlashCardFactory.MainPage = this;           

            _dragAnchor = new Anchor(0, 0);
            Content = new Grid();
        }
        /// <summary>
        /// MainPageが初期化されたのちに一度だけ呼ばれるメソッド
        /// </summary>
        /// <param name="args"></param>
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            var mainPageContent = this.Content as Grid;

            if (mainPageContent is null)
            {
                Debug.WriteLine($"◆[ERROR] MainPage.OnNavigatedTo() : MainPage content is null.");
                return;
            }

            TableComponent.MainPageContent = mainPageContent.Children as List<IView>;
            Debug.WriteLine($"content is null [{TableComponent.MainPageContent is null}]");
        }

        // ドラッグ更新時に呼ばれるメソッド
        internal void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            Debug.WriteLine($"◆ MainPage.OnPanUpdated");
            //Debug.WriteLine($"◆--- sender :{sender.GetType().Name}");
            //Debug.WriteLine($"◆--- anchor({_dragAnchor.StartX},{_dragAnchor.StartY})");

            // cast
            GraphicsView? rectangle = sender as GraphicsView;
            if (rectangle == null) return;

            float FIX_LIMIT = 5f;
            float LimitHeight = ((float)Height / 2) - ((float)rectangle.Height / 2) - FIX_LIMIT;
            float LimitWidth = ((float)Width / 2) - ((float)rectangle.Width / 2) - FIX_LIMIT;

            if (e.StatusType == GestureStatus.Started)
            {
                // start position init
                _dragAnchor = new Anchor(rectangle.TranslationX, rectangle.TranslationY);
                //Debug.WriteLine($"◆--- MainPage Size({Width},{Height})");
                //Debug.WriteLine($"◆--- anchor({_dragAnchor.StartX},{_dragAnchor.StartY})");
                return;
            }

            // GestureStatus が Running の場合に位置を更新
            if (e.StatusType == GestureStatus.Running)
            {
                // ドラッグスタート位置からドラッグ後の位置を出力
                //Debug.WriteLine($"◆--- rectangle total moving [before] ({rectangle.TranslationX},{rectangle.TranslationY})");
                //Debug.WriteLine($"◆--- rectangle size ({rectangle.Width},{rectangle.Height})");
                var nextX = (float)_dragAnchor.StartX + (float)e.TotalX;
                var nextY = (float)_dragAnchor.StartY + (float)e.TotalY;

                // 矩形がアプリケーションの外に出るようなドラッグは受け付けない
                if (nextY > LimitHeight) nextY = LimitHeight;
                if (nextY < -1 * LimitHeight) nextY = -1 * LimitHeight;
                if (nextX > LimitWidth) nextX = LimitWidth;
                if (nextX < -1 * LimitWidth) nextX = -1 * LimitWidth;

                // 移動後の座標をセット
                rectangle.TranslationX = nextX;
                rectangle.TranslationY = nextY;
                if (TableComponent.FlashCardDictionary.ContainsKey(rectangle))
                {
                    TableComponent.FlashCardDictionary[rectangle].TranslationX = nextX;
                    TableComponent.FlashCardDictionary[rectangle].TranslationY = nextY + 5f;
                }

                //Debug.WriteLine($"◆--- rectangle total moving [after] ({rectangle.TranslationX},{rectangle.TranslationY})");

                // 再描画を要求
                rectangle.Invalidate();
            }
        }
        private void MainPage_SizeChanged(object? sender, EventArgs e)
        {
            //Debug.WriteLine($"◆ MainPage.MainPage_SizeChanged");

            // この時点で Height および Width は有効な値になります
            double height = this.Height;
            double width = this.Width;

            //Debug.WriteLine($"◆--- MainPage Height: {height}, Width: {width}");
            
        }

        /// <summary>
        /// AppShellから呼ばれるカード生成メソッド<br/>
        /// </summary>
        public static void GenerateCard()
        {
            var newCard = FlashCardFactory.GererateCard();
            TableComponent.CurrentTable.FlashCardDictionary.Add(
                newCard.Cardobject,
                newCard
                );
        }
    }

    /// <summary>
    /// ユーザーがインスタンスしたデータ(SQLite実装後はそちらに委譲)
    /// </summary>
    public static class TableComponent
    {
        /// <summary>
        /// 表示中のテーブル
        /// </summary>
        public static Table CurrentTable { get; private set; } = new();
        /// <summary>
        /// 作成したテーブルの辞書
        /// </summary>
        public static Dictionary<int, Table> TableDictionary = new();
        /// <summary>
        /// MainPageのContentを保持するリスト<br/>
        /// </summary>
        public static List<IView> MainPageContent = new();

        // property
        public static Dictionary<View, int> VisualComponentDictionary => CurrentTable.VisualComponentDictionary;
        public static Dictionary<GraphicsView,FlashCard> FlashCardDictionary => CurrentTable.FlashCardDictionary;
        public static int GenerateCardCount => CurrentTable.GenerateCardCount;

        /// <summary>
        /// 編集するテーブルを変更する<br/>
        /// </summary>
        /// <param name="tableId"></param>
        public static void ChangeTable(int tableId)
        {
            CurrentTable = TableDictionary[tableId];

            if (CurrentTable.FlashCardDictionary.Count() > 0)
            {
                MainPageContent.Clear();

                var cards = CurrentTable.FlashCardDictionary.Values.ToList();
                foreach (var card in cards)
                {
                    MainPageContent.Add(card.Cardobject);
                    MainPageContent.Add(card.Textbox);
                }
            }
            else
            {
                MainPageContent.Clear();
                Debug.WriteLine($"◆[ERROR] TableComponent.ChangeTable() : No table data. TableId[{tableId}]");
            }
        }
    }

    /// <summary>
    /// カードとテキストのコンテナクラス
    /// </summary>
    public class Table
    {
        /// <summary>
        /// 生成したFlashCardの累計数
        /// </summary>
        public int GenerateCardCount { get; set; } = 0;
        /// <summary>
        /// 生成したFlashCardの辞書<br/>
        /// Key : GraphicsView<br/>
        /// </summary>
        public Dictionary<GraphicsView, FlashCard> FlashCardDictionary = new();
        /// <summary>
        /// 生成したFlashCardのZIndex
        /// </summary>
        public Dictionary<View, int> VisualComponentDictionary = new();
    }

    /// <summary>
    /// 単語カード生成クラス<br/>
    ///  ・MainPage.Component は Grid であり、IViewインスタンスを都度.Addする必要がある。<br/>
    ///  ・カードとタグの GraphicsView および Entry を１個ずつ順に出力する<br/>
    ///  ・出力したカードとタグの構造体を Dictionary で管理。ZIndex を振り分けるためのレイヤーレベルを辞書化する
    /// </summary>
    public static class FlashCardFactory
    {
        public static MainPage MainPage { get; set; }

        public static FlashCard GererateCard()
        {
            //Debug.WriteLine($"◆ TableComponent.GenerateCard()");
            //TableComponent.GenerateCardCount++;

            // grid reference
            var grid = MainPage.Content as Grid;
            if (grid is null)
            {
                //Debug.WriteLine($"◆[ERROR] reference missing. MainPage content don't have 'Grid'.");
                return null;
            }

            // init
            var _drawable = new DraggableGraphics(TableComponent.GenerateCardCount);
            var _graphicsView = new GraphicsView
            {
                Drawable = _drawable,
                HeightRequest = _drawable.Height,
                WidthRequest = _drawable.Width
            };

            // application windows resize event hundler
            _graphicsView.SizeChanged += (s, e) =>
            {
                //Debug.WriteLine($"◆ GraphicsView size changes");
                //Debug.WriteLine($"◆--- GraphicsView size : ({_graphicsView.Height},{_graphicsView.Width})");
                //Debug.WriteLine($"◆--- GraphicsView position : ({_graphicsView.Y},{_graphicsView.X})");

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());

                // span Textbox                
                var card = TableComponent.FlashCardDictionary[_graphicsView];
                card.Textbox.WidthRequest = _graphicsView.Width - 10f;
                card.Textbox.HeightRequest = _graphicsView.Height - 20f;
            };

            // application windows touch event hundler
            _graphicsView.StartInteraction += (s, e) =>
            {
                //Debug.WriteLine($"◆ Touch Card obj");
                var graph = s as GraphicsView;
                if (graph is null) return;
                var draggable = graph.Drawable as DraggableGraphics;
                if (draggable is null) return;

                // debug position
                var touchX = e.Touches[0].X + graph.TranslationX; // タッチされたX座標
                var touchY = e.Touches[0].Y + graph.TranslationY; // タッチされたY座標
                                                                  //Debug.WriteLine($"◆--- touched position : ({touchX},{touchY})");

                // ZIndex realignment
                ZIndexMove(MainPage, graph);
                //Debug.WriteLine($"◆--- GraphicsView touched : No.({draggable.Number}) value[{TableComponent.VisualComponentDictionary[graph]}]");

                // deletebutton
                float size = 8f;
                float padding = 5f;
                Rect buttonArea = new Rect(
                    graph.TranslationX + graph.Width - (padding + size) - 3f,
                    graph.TranslationY + padding,
                    size,
                    size
                );
                //Debug.WriteLine($"◆--- delete button position :({buttonArea.Left},{buttonArea.Right},{buttonArea.Top},{buttonArea.Bottom})");

                // textbox generate
                size = 9f;
                padding = 5f;
                Rect textboxButton = new Rect(
                    graph.TranslationX + padding,
                    graph.TranslationY + padding,
                    size,
                    size
                );
                //Debug.WriteLine($"◆--- textbox button position :({textboxButton.Left},{textboxButton.Right},{textboxButton.Top},{textboxButton.Bottom})");

                // Delete Card Button
                if (touchX >= buttonArea.Left && touchX <= buttonArea.Right && touchY >= buttonArea.Top && touchY <= buttonArea.Bottom)
                {
                    //Debug.WriteLine($"◆!!!--- touched delete button");

                    TableComponent.VisualComponentDictionary.Remove(graph);

                    grid.Children.Remove(graph);
                    TableComponent.VisualComponentDictionary.Remove(graph);

                    if (TableComponent.FlashCardDictionary.ContainsKey(graph))
                    {
                        var card = TableComponent.FlashCardDictionary[graph];

                        grid.Children.Remove( card.Cardobject );
                        TableComponent.VisualComponentDictionary.Remove( card.Cardobject );
                        TableComponent.FlashCardDictionary.Remove(graph);
                    }

                    graph.Drawable = null;
                    graph.GestureRecognizers.Clear();
                    graph = null;

                    return;
                }

                // Invalidate
                ZIndexMove(MainPage, graph);

                // debug
                foreach (var view in grid)
                {
                    //Debug.WriteLine($"◆--- Index[{view.ZIndex}],[{view}]");
                }
            };

            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();// Card1枚につき1つインスタンスしているので個別に反応してほしいんだが？
            panGesture.PanUpdated += MainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ          

            // registered Dictionary
            if (TableComponent.VisualComponentDictionary.Count > 0)
            {
                TableComponent.VisualComponentDictionary.Add(_graphicsView, TableComponent.VisualComponentDictionary.Last().Value + 1);
            }
            else
            {
                TableComponent.VisualComponentDictionary.Add(_graphicsView, 1);
            }

            // generate textbox
            var txbox = GenerateTextBox(MainPage, _graphicsView);

            // debug
            //Debug.WriteLine($"◆--- generate card : value[{TableComponent.VisualComponentDictionary.Last().Value}], MainPage[{mainPage is not null}], GraView[{_graphicsView is not null}]");

            return new FlashCard(TableComponent.MainPageContent,_graphicsView,txbox);
        }
        /// <summary>
        /// テキスト入力フォームの生成
        /// </summary>
        /// <param name="mainPage"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        static Editor GenerateTextBox(this MainPage mainPage, GraphicsView graph)
        {
            //Debug.WriteLine($"◆ generate textbox");

            // form const
            float padding = 5f;

            // Editor init
            var textbox = new Editor()
            {
                Placeholder = "input some text...",
                TranslationX = graph.TranslationX,
                TranslationY = graph.TranslationY + padding,
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black,
                VerticalTextAlignment = TextAlignment.Start,
                IsVisible = true,
                IsEnabled = true,
                Visual = default,
                ZIndex = 0,

            };// textboxの影についてはプラットフォームごとのカスタムハンドラーが必要とのこと。
            textbox.Focused += (s, e) =>
            {
                ZIndexMove(mainPage, graph);
            };
            //Debug.WriteLine($"◆--- textbox Rect({textbox.TranslationX},{textbox.TranslationY},{textbox.WidthRequest},{textbox.HeightRequest})");

            // add
            /*
            var grid = mainPage?.Content as Grid;
            if (grid is null)
            {
                Debug.WriteLine($"◆[ERROR] reference missing. MainPage content don't have 'Grid'.");
                return textbox;
            }
            */

            return textbox;
        }

        /// <summary>
        /// カードのイベントハンドラに委譲する階層移動<br/>
        /// </summary>
        /// <param name="mainPage"></param>
        /// <param name="touchCard"></param>
        static void ZIndexMove(this MainPage mainPage, GraphicsView touchCard)
        {
            Editor? pairTb = null;

            // タッチしたカード以外をのvalueを+1し、タッチしたカードは1(最小値)にする
            foreach (var key in TableComponent.VisualComponentDictionary.Keys.ToList())
            {
                Debug.WriteLine($"pairTb is null :{pairTb is null}");

                if (key == touchCard)
                {
                    TableComponent.VisualComponentDictionary[key] = 1;
                    //Debug.WriteLine($"EditorDictionary.ContainsKey(touchCard) :{TableComponent.FlashCardDictionary.ContainsKey(touchCard)}");
                    if (TableComponent.FlashCardDictionary.ContainsKey(touchCard))
                    {
                        var card = TableComponent.FlashCardDictionary[touchCard];

                        //Debug.WriteLine($"{ EditorDictionary[touchCard] } = {VisualComponentDictionary[EditorDictionary[touchCard]]}");
                        TableComponent.VisualComponentDictionary[card.Cardobject] = 0;
                        pairTb = TableComponent.FlashCardDictionary[touchCard].Textbox;
                    }
                }
                else
                {
                    if (key != pairTb)
                    {
                        TableComponent.VisualComponentDictionary[key] = TableComponent.VisualComponentDictionary[key] + 2;
                        Debug.WriteLine($" plus ");
                    }
                }
            }

            // 再描画
            foreach (var key in TableComponent.VisualComponentDictionary.Keys.ToList())
            {
                // 大きいほど一番上にカードが来る ので、ZIndex = int.MaxValue - dictionary.value
                if (mainPage is not null)
                {
                    var gridContent = mainPage.Content as Grid;
                    var view = gridContent?.Children.FirstOrDefault(x => x == key) as View;

                    if (view is not null)
                    {
                        view.ZIndex = int.MaxValue - TableComponent.VisualComponentDictionary[key];
                        //Debug.WriteLine($"◆---Index [{int.MaxValue - VisualComponentDictionary[key]}]. Key is [{view}]. ");
                    }
                }
            }
        }
    }

    public class FlashCard
    {
        /// <summary>
        /// 所属しているGrid
        /// </summary>
        public readonly List<IView> ParentGrid;
        /// <summary>
        /// 単語カードの参照
        /// </summary>
        public readonly GraphicsView Cardobject;
        /// <summary>
        /// テキストボックス
        /// </summary>
        public readonly Editor Textbox;
        public double TranslationX
        {
            get => Cardobject.TranslationX;
            set => Cardobject.TranslationX = value;
        }
        public double TranslationY
        {
            get => Cardobject.TranslationY;
            set => Cardobject.TranslationY = value;
        }

        public FlashCard(List<IView> _gridChildren, GraphicsView _view ,Editor _editor)
        {
            ParentGrid = _gridChildren;
            Cardobject = _view;
            Textbox = _editor;

            ParentGrid.Add(_view);
            ParentGrid.Add(_editor);
        }
    }
    public class FlashCardProperty
    {
        /* 
         * 
         * 
         */
        public int TextFontSize { get; set; } = 12;
        public Color TextFontColor { get; set; } = Colors.Black;
        public int TagFontSize { get; set; } = 8;
        public Color TagFontColor { get; set; } = Colors.Black;
        public float CardHeight { get; set; } = 350f;
        public float CardWidth { get; set; } = 150f;
        public FlashCardProperty() { }
    }
    /*
    public class TestObj : IDrawable
    {
        public TestObj() 
        {
            Debug.WriteLine($"◆--- TestObj Constructor");
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Debug.WriteLine($"◆--- TestObj Draw");
        }
    }
    */
    public class DraggableGraphics : IDrawable
    {
        public string Content = "...";

        public readonly int Number;
        public readonly float Width = 350f;
        public readonly float Height = 150f;

        private float _corner = 7f;
        
        public DraggableGraphics(int num)
        {
            Number = num;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float OFFSET = 1f; 

            Debug.WriteLine($"◆ DraggableGraphics.Draw");

            // 影の色と透明度を設定
            //canvas.SetShadow(new SizeF(OFFSET, OFFSET), 5, Colors.RosyBrown.WithAlpha(0.5f));

            // 塗りつぶしの方法を設定
            canvas.FillColor = Colors.White;
            canvas.FillRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width-OFFSET, dirtyRect.Height- OFFSET, _corner);
            Debug.WriteLine($"◆--- Card Draw at Position({dirtyRect.X},{dirtyRect.Y}),Size({dirtyRect.Width},{dirtyRect.Height})");

            // 枠線の色と線幅を設定
            canvas.StrokeColor = Colors.RosyBrown;
            canvas.StrokeSize = 0.2f; // 線の太さ
            canvas.DrawRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width-OFFSET, dirtyRect.Height-OFFSET, _corner-1f);
            
            // 削除ボタン
                float size = 10f;// カードの大きさに応じた比率の方が良いが、プラットフォームによって大きさは異なる為調整は必要
                float padding = 5f;
                float deleteButtonX = dirtyRect.Right - size - padding - 3f;
                float deleteButtonY = dirtyRect.Top;
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(deleteButtonX,deleteButtonY, size, size);
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 14;
                canvas.DrawString(
                    "×",
                    deleteButtonX,
                    deleteButtonY,
                    size+padding,
                    size+padding,
                    HorizontalAlignment.Left,
                    VerticalAlignment.Top
                );
            /*
            // テキストボックス展開ボタン
                size = 10f;// カードの大きさに応じた比率の方が良いが、プラットフォームによって大きさは異なる為調整は必要
                padding = 1f;
                float textButtonX = dirtyRect.Left + padding + 3f;
                float textButtonY = dirtyRect.Top + padding;
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(textButtonX, textButtonY, size, size);
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 12;
                canvas.DrawString(
                    "＋",
                    textButtonX,
                    textButtonY,
                    size + padding,
                    size + padding + 3f,
                    HorizontalAlignment.Left,
                    VerticalAlignment.Top
                );           

            // テキスト
                padding = 20f;
                float textX = dirtyRect.Left + 7f;
                float textY = textButtonY + padding;
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 12;
                canvas.DrawString(
                    Content,
                    textX,
                    textY,
                    dirtyRect.Width - 40f,
                    dirtyRect.Height - 40f,
                    HorizontalAlignment.Left,
                    VerticalAlignment.Top
                );
            */

            // 影を解除（次の描画に影をつけないため）
            canvas.SetShadow(SizeF.Zero, 0, Colors.Transparent);
        }
    }
    class Anchor
    {
        public double StartX;
        public double StartY;
        public Anchor(double _x, double _y) { StartX = _x; StartY = _y; }
    }
}
