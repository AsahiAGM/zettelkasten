using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;

using PlantUmlClassDiagramGenerator.SourceGenerator;
using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;

namespace Zettelkasten
{
    /// <summary>
    /// UIとイベント
    /// </summary>
    public partial class MainPage : ContentPage
    {
        internal Anchor _dragAnchor;

        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 作成したテーブルと生成カードリストの辞書
        /// </summary>
        internal Dictionary<Table, Grid> DrawObjectDictionary = new();// SQLiteのDBにもこれ格納するかも

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
        /// MainPageが初期化された際に一度だけ呼ばれるメソッド
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

            CurrentTable.table = new Table( mainPageContent );
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

            double FIX_LIMIT = 5f;
            double LimitHeight = ((double)Height / 2) - ((double)rectangle.Height / 2) - FIX_LIMIT;
            double LimitWidth = ((double)Width / 2) - ((double)rectangle.Width / 2) - FIX_LIMIT;

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
                var nextX = (double)_dragAnchor.StartX + (double)e.TotalX;
                var nextY = (double)_dragAnchor.StartY + (double)e.TotalY;

                // 矩形がアプリケーションの外に出るようなドラッグは受け付けない
                if (nextY > LimitHeight) nextY = LimitHeight;
                if (nextY < -1 * LimitHeight) nextY = -1 * LimitHeight;
                if (nextX > LimitWidth) nextX = LimitWidth;
                if (nextX < -1 * LimitWidth) nextX = -1 * LimitWidth;

                // 移動後の座標をセット
                rectangle.TranslationX = nextX;
                rectangle.TranslationY = nextY;
                if (CurrentTable.FlashCardDictionary.ContainsKey(rectangle))
                {
                    CurrentTable.FlashCardDictionary[rectangle].Translation(nextX, nextY);
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
            CurrentTable.table.FlashCardDictionary.Add(
                newCard.Cardobject,
                newCard
                );
        }
    }

    /// <summary>
    /// ユーザーがインスタンスしたデータ(SQLite実装後はそちらに委譲)
    /// </summary>
    public static class CurrentTable
    {
        /// <summary>
        /// 表示中のテーブル
        /// </summary>
        public static Table table { get; set; } = new( new Grid() );
        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 作成したテーブルの辞書
        /// </summary>
        public static Dictionary<int, Table> TableDictionary = new();


        // property
        /// <summary>
        /// 現在のTableが所属するGrid<br/>
        /// </summary>
        public static Grid MainPageContent => table.ParentGrid;
        /// <summary>
        /// 生成したカードの数
        /// </summary>
        public static int GenerateCardCount => table.GenerateCardCount;

        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 生成したFlashCardの辞書<br/>
        /// key: GraphicsView
        /// </summary>
        public static Dictionary<GraphicsView, FlashCard> FlashCardDictionary => table.FlashCardDictionary;
        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 生成したFlashCardのZIndex
        /// </summary>
        public static Dictionary<View, int> VisualComponentZIndexDictionary => table.VisualComponentZIndexDictionary;
        
        

        /// <summary>
        /// 編集するテーブルを変更する<br/>
        /// </summary>
        /// <param name="tableId"></param>
        public static void ChangeTable(int tableId)
        {
            table = TableDictionary[tableId];

            if (table.FlashCardDictionary.Count() > 0)
            {
                MainPageContent.Clear();

                var cards = table.FlashCardDictionary.Values.ToList();
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
        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 固有ID 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 生成したFlashCardの累計数
        /// </summary>
        public int GenerateCardCount { get; set; } = 0;
        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 生成したFlashCardの辞書<br/>
        /// Key : GraphicsView<br/>
        /// </summary>
        public Dictionary<GraphicsView, FlashCard> FlashCardDictionary = new();
        [PlantUmlIgnoreAssociation]
        /// <summary>
        /// 生成したFlashCardのZIndex
        /// </summary>
        public Dictionary<View, int> VisualComponentZIndexDictionary = new();
        /// <summary>
        /// このテーブルの所属Grid
        /// </summary>
        public readonly Grid ParentGrid;

        public Table(Grid parent)
        {
            Id = Guid.NewGuid();
            ParentGrid = parent;
        }

        public override bool Equals(object obj)
        {
            if(obj is Table other)
            {
                return this.Id.Equals(other.Id);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
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
            // grid reference
            var grid = CurrentTable.MainPageContent;

            // init
            var property = new FlashCardProperty();

            var _graphicsView = new GraphicsView();
            var _drawable = new DraggableGraphics(CurrentTable.GenerateCardCount ,property ,_graphicsView);

            _graphicsView.Drawable = _drawable;
            _graphicsView.HeightRequest = _drawable.property.CardHeight;
            _graphicsView.WidthRequest = _drawable.property.CardWidth;


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
                var card = CurrentTable.FlashCardDictionary[_graphicsView];
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

                // deletebutton area setting
                float size = 8f;
                float padding = 5f;
                Rect buttonArea = new Rect(
                    graph.TranslationX + graph.Width - (padding + size) - 3f,
                    graph.TranslationY + padding,
                    size,
                    size
                );
                //Debug.WriteLine($"◆--- delete button position :({buttonArea.Left},{buttonArea.Right},{buttonArea.Top},{buttonArea.Bottom})");

                // Delete Card Button
                if (touchX >= buttonArea.Left && touchX <= buttonArea.Right && touchY >= buttonArea.Top && touchY <= buttonArea.Bottom)
                {
                    //Debug.WriteLine($"◆!!!--- touched delete button");

                    CurrentTable.VisualComponentZIndexDictionary.Remove(graph);
                    grid.Children.Remove(graph);

                    if (CurrentTable.FlashCardDictionary.ContainsKey(graph))
                    {
                        var card = CurrentTable.FlashCardDictionary[graph];
                        
                        CurrentTable.VisualComponentZIndexDictionary.Remove( card.Textbox);
                        grid.Children.Remove(card.Textbox);

                        CurrentTable.FlashCardDictionary.Remove(graph);
                    }

                    graph.Drawable = null;
                    graph.GestureRecognizers.Clear();
                    graph = null;

                    return;
                }

                // Invalidate
                ZIndexMove(MainPage, graph);

                // debug
                /*
                foreach (var view in grid)
                {
                    Debug.WriteLine($"◆--- Index[{view.ZIndex}],[{view}]");
                }
                */
            };

            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += MainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ                     

            // generate textbox
            var txbox = GenerateTextBox(MainPage, _graphicsView, property);

            // registered Dictionary(Card:GraphicsView & Textbox:Editor)
            if (CurrentTable.VisualComponentZIndexDictionary.Count > 0)
            {
                CurrentTable.VisualComponentZIndexDictionary.Add(_graphicsView, CurrentTable.VisualComponentZIndexDictionary.Last().Value + 2);
                CurrentTable.VisualComponentZIndexDictionary.Add(txbox, CurrentTable.VisualComponentZIndexDictionary.Last().Value + 1);
            }
            else
            {
                CurrentTable.VisualComponentZIndexDictionary.Add(_graphicsView, 1);
                CurrentTable.VisualComponentZIndexDictionary.Add(txbox, 0);
            }

            // debug
            /*
            Debug.WriteLine($"◆--- generate card : value[{TableComponent.VisualComponentDictionary.Last().Value}], MainPage[{mainPage is not null}], GraView[{_graphicsView is not null}]");
            if (TableComponent.MainPageContent is null || _graphicsView is null || txbox is null)
            {
                Debug.WriteLine($"TableComponent.MainPageContent: {TableComponent.MainPageContent is null}");
                Debug.WriteLine($"graphicsVie: {_graphicsView is null}");
                Debug.WriteLine($"txbox: {txbox is null}");
            }
            */

            return new FlashCard(grid ,_graphicsView, txbox, property);
        }
        /// <summary>
        /// テキスト入力フォームの生成<br/>
        /// </summary>
        /// <param name="mainPage"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        static Editor GenerateTextBox(this MainPage mainPage, GraphicsView graph, FlashCardProperty property)
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
                TextColor = property.TextFontColor,
                FontSize = property.TextFontSize,
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

            return textbox;
        }

        /// <summary>
        /// カードのイベントハンドラに委譲する階層移動<br/>
        /// </summary>
        /// <param name="mainPage"></param>
        /// <param name="touchCard"></param>
        static void ZIndexMove(this MainPage mainPage, GraphicsView touchCard)
        {
            // タッチしたカード以外をのvalueを+1し、タッチしたカードは1(最小値)にする
            foreach (var key in CurrentTable.VisualComponentZIndexDictionary.Keys.ToList())
            {
                // カードじゃない要素は飛ばす
                if (key as GraphicsView is null) continue;

                var gView = key as GraphicsView;

                if (gView.Id == touchCard.Id)
                {
                    // タッチしたカードのZIndexを最上層+1に
                    CurrentTable.VisualComponentZIndexDictionary[key] = 1;                   

                    // タッチしたカードのTextboxを最上層に
                    var card = CurrentTable.FlashCardDictionary[gView];
                    CurrentTable.VisualComponentZIndexDictionary[card.Textbox] = 0;

                    Debug.WriteLine($"touch_card :{card.Cardobject.Id},{card.Textbox.Id}");
                }
                else
                {
                    // タッチしたカード以外は下層に(2オブジェクト１組みなので、+2ずつ増える)
                    CurrentTable.VisualComponentZIndexDictionary[key] = CurrentTable.VisualComponentZIndexDictionary[key] + 2;

                    var card = CurrentTable.FlashCardDictionary[gView];
                    CurrentTable.VisualComponentZIndexDictionary[card.Textbox] = CurrentTable.VisualComponentZIndexDictionary[card.Textbox] + 2;

                    Debug.WriteLine($"other_card :{card.Cardobject.Id},{card.Textbox.Id}");
                }
            }

            // 再描画
            foreach (var key in CurrentTable.VisualComponentZIndexDictionary.Keys.ToList())
            {
                // 大きいほど一番上にカードが来る ので、ZIndex = int.MaxValue - dictionary.value
                if (mainPage is not null)
                {
                    var gridContent = mainPage.Content as Grid;
                    var view = gridContent?.Children.FirstOrDefault(x => x == key) as View;

                    if (view is not null)
                    {
                        view.ZIndex = int.MaxValue - CurrentTable.VisualComponentZIndexDictionary[key];
                        Debug.WriteLine($"◆---Index [{CurrentTable.VisualComponentZIndexDictionary[key]}]. Key is [{view.Id}]. ");
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
        public readonly Grid ParentGrid;
        /// <summary>
        /// 単語カードの参照
        /// </summary>
        public readonly GraphicsView Cardobject;
        /// <summary>
        /// テキストボックス
        /// </summary>
        public readonly Editor Textbox;
        /// <summary>
        /// 書式設定
        /// </summary>
        public FlashCardProperty Property;
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

        public void Translation(double nextX, double nextY)
        {
            const double TEXTBOX_Y_FIX = 5d;

            TranslationX = nextX;
            TranslationY = nextY;

            Textbox.TranslationX = nextX;
            Textbox.TranslationY = nextY + TEXTBOX_Y_FIX;
        }

        public FlashCard(Grid _grid, GraphicsView _view ,Editor _editor ,FlashCardProperty _property)
        {
            ParentGrid = _grid;
            Cardobject = _view;
            Textbox = _editor;
            Property = _property;

            ParentGrid.Add(_view);
            ParentGrid.Add(_editor);
        }
    }
    public class FlashCardProperty
    {
        public int TextFontSize { get; set; } = 12;
        [PlantUmlIgnoreAssociation]
        public Color TextFontColor { get; set; } = Colors.Black;
        public int TagFontSize { get; set; } = 8;
        [PlantUmlIgnoreAssociation]
        public Color TagFontColor { get; set; } = Colors.Black;
        public float CardHeight { get; set; } = 150f;
        public float CardWidth { get; set; } = 350f;
        [PlantUmlIgnoreAssociation]
        public Color CardBackgroundColor { get; set; } = Colors.White;
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
        /// <summary>
        /// 親のGraphicsView
        /// </summary>
        GraphicsView parentView;
        /// <summary>
        /// 書式設定
        /// </summary>
        public FlashCardProperty property { get; private set; }
        /// <summary>
        /// テキストの初期値
        /// </summary>
        public string Content = "...";
        /// <summary>
        /// 最初のカードが生成されてから何番目か
        /// </summary>
        public readonly int Number;
        /// <summary>
        /// 角の丸さ
        /// </summary>
        private float _corner = 7f;

        public DraggableGraphics(int num, FlashCardProperty _property, GraphicsView parent)
        {
            Number = num;
            parentView = parent;
            property = _property;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float OFFSET = 1f; 

            Debug.WriteLine($"◆ DraggableGraphics.Draw");

            // 影の色と透明度を設定
            //canvas.SetShadow(new SizeF(OFFSET, OFFSET), 5, Colors.RosyBrown.WithAlpha(0.5f));

            // 塗りつぶしの方法を設定
            canvas.FillColor = property.CardBackgroundColor;
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

            // 影を解除（次の描画に影をつけないため）
            canvas.SetShadow(SizeF.Zero, 0, Colors.Transparent);
        }
    }
    /// <summary>
    /// オブジェクトを移動させる際の基準点
    /// </summary>
    class Anchor
    {
        public double StartX;
        public double StartY;
        public Anchor(double _x, double _y) { StartX = _x; StartY = _y; }
    }
}
