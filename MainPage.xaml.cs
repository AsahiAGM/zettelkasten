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
        /// <summary>
        /// singleton instance
        /// </summary>
        public static MainPage Instance { get; private set; } = null!;
        /// <summary>
        /// ドラッグ移動を開始した際の基準点
        /// </summary>
        internal Anchor _dragAnchor;

        /// <summary>
        /// 作成したテーブルと生成カードリストの辞書
        /// </summary>
        [PlantUmlIgnoreAssociation]
        //internal Dictionary<Table, Grid> DrawObjectDictionary = new();// SQLiteのDBにもこれ格納するかも

        public MainPage()
        {
            Debug.WriteLine($"◆ MainPage Constructor");

            InitializeComponent();// initialize and read XAML. This generate when initializing and belong "obj".

            
            if (Instance is null)
            {
                Debug.WriteLine($"◆[MainPage Constructor] MainPage Instance Initiation");

                // init
                Instance = this;
            }

            _dragAnchor = new Anchor(0, 0);
            
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
                Debug.WriteLine($"◆[ERROR] MainPage.OnNavigatedTo() : MainPage content is null or not Grid.");
                return;
            }
            
            CurrentTable.table = new Table(mainPageContent);
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
                newCard.CardObject.Background,
                newCard
                );
        }

        /// <summary>
        /// カードのイベントハンドラに委譲する階層移動<br/>
        /// </summary>
        /// <param name="touchCard"></param>
        public static void ZIndexMove(GraphicsView touchCard)
        {
            // タッチしたカード以外をのvalueを+1し、タッチしたカードは1(最小値)にする
            foreach (var key in CurrentTable.FlashCardDictionary.Keys.ToList())
            {
                if (key.Id == touchCard.Id)
                {
                    // タッチしたカードを最上層に
                    CurrentTable.FlashCardDictionary[touchCard].MoveTop();
                }
                else
                {
                    // タッチしたカード以外は下層に
                    CurrentTable.FlashCardDictionary[key].MoveLower();
                }
            }
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
        public static Table table { get; set; } = new(new Grid());
        /// <summary>
        /// 作成したテーブルの辞書
        /// </summary>
        [PlantUmlIgnoreAssociation]
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

        /// <summary>
        /// 生成したFlashCardの辞書<br/>
        /// key: GraphicsView
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public static Dictionary<GraphicsView, FlashCard> FlashCardDictionary => table.FlashCardDictionary;

        /// <summary>
        /// 編集するテーブルを変更する<br/>
        /// </summary>
        /// <param name="tableId"></param>
        public static void ChangeTable(int tableId)
        {
            table = TableDictionary[tableId];
        }

        // テーブルの内容物を全てGridに反映させるメソッドを追加する必要がある
        // まだ新規テーブル作成を実装する段階じゃないので保留。
    }

    /// <summary>
    /// カードとテキストのコンテナクラス
    /// </summary>
    public class Table : IEquatable<Table>
    {
        /// <summary>
        /// 固有ID 
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Guid Id { get; }
        /// <summary>
        /// 生成したFlashCardの累計数
        /// </summary>
        public int GenerateCardCount { get; private set; } = 0;
        
        /// <summary>
        /// このテーブルのルートの数
        /// </summary>
        public int RootCardCount
        {
            get
            {
                return RootList.Count();
            }
        }
        
        /// <summary>
        /// このテーブルのルートカードのリスト
        /// </summary>
        public List<FlashCard> RootList
        {
            get
            {
                return FlashCardDictionary.Values.Where(x => x.IsRoot).ToList();
            }
        }       

        /// <summary>
        /// 生成したFlashCardの辞書<br/>
        /// Key : GraphicsView<br/>
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Dictionary<GraphicsView, FlashCard> FlashCardDictionary = new();
        

        /// <summary>
        /// このテーブルの所属Grid
        /// </summary>
        public readonly Grid ParentGrid;

        public Table(Grid parent)
        {
            Id = Guid.NewGuid();
            ParentGrid = parent;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(Table? other)
        {
            return this.Id == (other?.Id ?? Guid.Empty);
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
        public static MainPage MainPage => MainPage.Instance;

        public static FlashCard GererateCard()
        {
            // grid reference
            var grid = CurrentTable.MainPageContent;

            // init
            var property = new FlashCardProperty();
            var _graphicsView = new GraphicsView();
            
            var _drawable = new DraggableGraphics(property, _graphicsView);

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
                card.CardObject.Textbox.WidthRequest = _graphicsView.Width - 10f;
                card.CardObject.Textbox.HeightRequest = _graphicsView.Height - 20f;
                
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
                    var flashCard = CurrentTable.FlashCardDictionary[graph];
                    var list = grid.Children.ToList();

                    list.RemoveAll(x => flashCard.GetViewModel().Contains(x));
                    CurrentTable.FlashCardDictionary.Remove(graph);

                    graph.Drawable = null;
                    graph.GestureRecognizers.Clear();
                    graph = null;                   
                    return;
                }

                // Invalidate
                MainPage.ZIndexMove(graph);

            };

            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += MainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ                     

            // generate textbox
            var txbox = GenerateTextBox(_graphicsView, property);

            // generate FlashCard           
            var cardObject = new CardObject(_graphicsView, txbox, property);
            var tag = new Tag(cardObject, property, GenerateTextBox);
            var flashcard = new FlashCard(cardObject, tag);
            

            // set grid           
            foreach (var viewModel in flashcard.GetViewModel())
            {
                CurrentTable.MainPageContent.Add(viewModel);
            }         

            return flashcard;
        }
        /// <summary>
        /// テキスト入力フォームの生成<br/>
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        static Editor GenerateTextBox(GraphicsView graph, FlashCardProperty property)
        {
            //Debug.WriteLine($"◆ generate textbox");

            // form const
            float padding = 5f;

            // Editor init
            var textbox = new Editor()
            {
                TranslationX = graph.TranslationX,
                TranslationY = graph.TranslationY + padding,
                BackgroundColor = property.TextBoxBackGround,
                TextColor = property.TextFontColor,
                FontSize = property.TextFontSize,
                VerticalTextAlignment = TextAlignment.Start,
                IsVisible = true,
                IsEnabled = true,
                Visual = default,
                // textboxの影についてはプラットフォームごとのカスタムハンドラーが必要とのこと。
            };

            textbox.Focused += (s, e) =>
            {
                MainPage.ZIndexMove(graph);
            };

            return textbox;
        }
    }

    /// <summary>
    /// FlashCardの描画および書式の設定
    /// </summary>
    public class FlashCardProperty
    {
        public int TextFontSize { get; set; } = 16;
        [PlantUmlIgnoreAssociation]
        public Color TextFontColor { get; set; } = Colors.Black;
        [PlantUmlIgnoreAssociation]
        public Color TextBoxBackGround { get; set; } = Colors.Transparent;

        public float TagHeight { get; set; } = 20f;
        public float TagWidth { get; set; } = 80f;
        public int TagFontSize { get; set; } = 10;
        [PlantUmlIgnoreAssociation]
        public Color TagFontColor { get; set; } = Colors.Black;

        public float CardHeight { get; set; } = 150f;
        public float CardWidth { get; set; } = 350f;
        [PlantUmlIgnoreAssociation]
        public Color CardBackgroundColor { get; set; } = Colors.White;
        public FlashCardProperty() { }
    }

    /// <summary>
    /// CardObjectとTagのコンテナ
    /// </summary>
    public class FlashCard
    {
        /// <summary>
        /// 単語カードとテキストボックス
        /// </summary>
        public CardObject CardObject { get; } = null!;
        /// <summary>
        /// タグと表示窓
        /// </summary>
        public Tag Tag { get; } = null!;
        /// <summary>
        /// このカードのRootId
        /// </summary>
        public int RootId => Tag.RootId;
        /// <summary>
        /// このカードがルートか否か
        /// </summary>
        public bool IsRoot => Tag.IsRoot;
        /// <summary>
        /// X座標(CardObject由来)
        /// </summary>
        public double TranslationX => CardObject.TranslationX;
        /// <summary>
        /// Y座標(CardObject由来)
        /// </summary>
        public double TranslationY => CardObject.TranslationY;
        /// <summary>
        /// ViewModelとZIndexのペアリスト
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public List<ViewIndexPair> ZIndex { get; private set; } = new();
        public FlashCard(CardObject cardObject, Tag tag)
        {
            CardObject = cardObject;
            Tag = tag;

            ZIndex = new List<ViewIndexPair>()
            {
                new ViewIndexPair(0, CardObject.Textbox),
                new ViewIndexPair(1, Tag.Textbox),
                new ViewIndexPair(2, CardObject.Background),
                new ViewIndexPair(3, Tag.Background)
            };
        }
        /// <summary>
        /// 要素を最上段に移動
        /// </summary>
        public void MoveTop()
        {
            ZIndex = new List<ViewIndexPair>()
            {
                new ViewIndexPair(0, CardObject.Textbox),
                new ViewIndexPair(1, Tag.Textbox),
                new ViewIndexPair(2, CardObject.Background),
                new ViewIndexPair(3, Tag.Background)
            };

            CalcZIndex();
        }
        /// <summary>
        /// 要素を下段に移動
        /// </summary>
        public void MoveLower()
        {
            foreach (var view in ZIndex)
            {
                view.Index += ZIndex.Count();
            }

            CalcZIndex();
        }
        /// <summary>
        /// IViewのZIndexを再計算
        /// </summary>
        void CalcZIndex()
        {
            foreach (var pair in ZIndex)
            {
                pair.ViewModel.ZIndex = int.MaxValue - pair.Index;
            }
        }
        /// <summary>
        /// カードを構成する描画要素(View)を取得<br/><br/>
        /// ・Gridに参照させる際に使用(AddRanage,RemoveAllの適用)
        /// </summary>
        /// <returns></returns>
        public List<IView> GetViewModel()
        {
            return new List<IView>()
            {
                CardObject.Background,
                CardObject.Textbox,
                Tag.Background,
                Tag.Textbox
            };
        }
        /// <summary>
        /// ViewModelの座標移動
        /// </summary>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        public void Translation(double nextX, double nextY)
        {
            CardObject.Translation(nextX, nextY);
            Tag.Translation(nextX, nextY);
        }

        /// <summary>
        /// View and Index Container
        /// </summary>
        public class ViewIndexPair
        {
            public int Index { get; internal set; }
            public View ViewModel { get; } = null!;
            public ViewIndexPair(int index, View viewModel)
            {
                Index = index;
                ViewModel = viewModel;
            }
        }
    }

    public class CardObject : IEquatable<CardObject>
    {
        /// <summary>
        /// 固有Id
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Guid Id { get; }
        /// <summary>
        /// 単語カードの参照
        /// </summary>
        public readonly GraphicsView Background;
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
            get => Background.TranslationX;
            set => Background.TranslationX = value;
        }
        public double TranslationY
        {
            get => Background.TranslationY;
            set => Background.TranslationY = value;
        }

        public CardObject(GraphicsView _view, Editor _editor, FlashCardProperty _property)
        {
            Id = Guid.NewGuid();

            Background = _view;
            Textbox = _editor;
            Property = _property;
        }
        /// <summary>
        /// ViewModelの座標移動
        /// </summary>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        public void Translation(double nextX, double nextY)
        {
            const double TEXTBOX_Y_FIX = 5d;

            TranslationX = nextX;
            TranslationY = nextY;

            Textbox.TranslationX = nextX;
            Textbox.TranslationY = nextY + TEXTBOX_Y_FIX;
        }

        /// <summary>
        /// IEquatable.Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CardObject? other)
        {
            return this.Id == (other?.Id ?? Guid.Empty);
        }
        /// <summary>
        /// GetHash from Guid
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    /// <summary>
    /// FlashCardをツリー構造で管理するためのタグ
    /// </summary>
    public class Tag : IEquatable<Tag>
    {
        /// <summary>
        /// オブジェクトの固有Id
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Guid Id { get; }
        /// <summary>
        /// ChildrenにおけるId<br/><br/>
        /// ・-1：未所属の浮いた状態。ロジックエラーである可能性あり
        /// </summary>
        public int RootId { get; private set; }
        /// <summary>
        /// このタグがルートタグか否か
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return Parent is null;
            }
        }
        /// <summary>
        /// このタグが管理するFlashCard
        /// </summary>
        public CardObject CardObject { get; } = null!;
        /// <summary>
        /// このタグの図形描画用オブジェクト
        /// </summary>
        public GraphicsView Background { get; } = null!;
        /// <summary>
        /// このタグの表示用オブジェクト
        /// </summary>
        public Editor Textbox { get; } = null!;
        /// <summary>
        /// 表示する文字
        /// </summary>
        string tagString => GetTagId();

        /// <summary>
        /// X座標
        /// </summary>
        public double TranslationX { get; private set; }
        /// <summary>
        /// Y座標
        /// </summary>
        public double TranslationY { get; private set; }
        /// <summary>
        /// 親のTag<br/>
        /// ・null: このタグがルートタグである時
        /// </summary>
        public Tag? Parent { get; private set; }
        /// <summary>
        /// private children list
        /// </summary>
        [PlantUmlIgnoreAssociation]
        List<Tag> _children = null!;
        /// <summary>
        /// 子のタグのリスト<br/>
        /// ・Listは遅延初期化
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public List<Tag> Children
        {
            get
            {
                if (_children is null) _children = new();
                return _children;
            }
        }

        public Tag(CardObject cobj, FlashCardProperty property, Func<GraphicsView, FlashCardProperty, Editor> GenerateTextBox)
        {
            // init
            Id = Guid.NewGuid();
            CardObject = cobj;
            RootId = CurrentTable.table.RootCardCount + 1;

            // fix const
            const double OFFSET = 5;

            // set ViewModel
            var _graphicsView = new GraphicsView();
            var _drawable = new DraggableGraphics(property, _graphicsView, false);
            _graphicsView.Drawable = _drawable;

            _graphicsView.HeightRequest = property.TagHeight;
            _graphicsView.WidthRequest = property.TagWidth;
            _graphicsView.TranslationX = (cobj.TranslationX - cobj.Background.Width / 2) + OFFSET;
            _graphicsView.TranslationY = (cobj.TranslationY - cobj.Background.Height / 2) - property.TagHeight;

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
                var card = CurrentTable.FlashCardDictionary[_graphicsView]; // KeyNotFoundExceptionが発生する場合がある 0429
                card.CardObject.Textbox.WidthRequest = _graphicsView.Width - 10f;
                card.CardObject.Textbox.HeightRequest = _graphicsView.Height - 20f;
            };

            var _txbox = GenerateTextBox(_graphicsView, property);

            Background = _graphicsView;
            Textbox = _txbox;
        }

        /// <summary>
        /// タグの再帰取得(深さ優先)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tag> Traverse()
        {
            yield return this;
            foreach (var child in Children)
            {
                yield return child;
            }
        }
        /// <summary>
        /// ViewModelの座標移動
        /// </summary>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        public void Translation(double nextX, double nextY)
        {
            const double TEXTBOX_Y_FIX = 5d;

            TranslationX = nextX;
            TranslationY = nextY;

            Textbox.TranslationX = nextX;
            Textbox.TranslationY = nextY + TEXTBOX_Y_FIX;
        }
        /// <summary>
        /// 表示用のIdを作成
        /// </summary>
        /// <returns></returns>
        public string GetTagId()
        {
            var li = new List<int>();
            li.Add(RootId);

            Tag? next = Parent;

            while (next is not null)
            {
                li.Add(next.RootId);
                next = next.Parent;
            }

            li.Reverse();
            return string.Join("-", li);
        }
        /// <summary>
        /// 親となるタグを変更<br/>
        /// ・指定しない場合はこのタグが新たなルートタグとなる
        /// </summary>
        /// <param name="newParent"></param>
        /// <returns></returns>
        public Tag? ChangeParent(Tag? newParent = null)
        {
            if (newParent is null)
            {
                RootId = CurrentTable.table.RootCardCount + 1;
                Parent = null;

                return null;
            }

            Parent = newParent;
            Parent.Children.Add(this);
            RootId = Parent.Children.Count();

            return newParent;
        }
        /// <summary>
        /// 現在の階層における順番を変更する
        /// </summary>
        /// <param name="newId"></param>
        public void MoveIdInChildren(int newId)
        {
            if (Parent is null)
            {
                Debug.WriteLine($"[Tag.MoveIdInChildren] this tag don't have parent");
                return;
            }

            Parent.Children.Insert(newId - 1, this); // 先頭にしたい場合は1を指定するので、Listの仕様に合わせてここで-1する
            Parent.ResetRootId();

        }
        /// <summary>
        /// RootIdを振り直す
        /// </summary>
        public void ResetRootId()
        {
            for (int i = 0; i < Children.Count(); i++)
            {
                Children[0].RootId = i + 1;
            }
        }
        /// <summary>
        /// 直下の任意の子タグを削除する
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool RemoveChild(Tag target)
        {
            bool result = Children.Remove(target);

            if (result)
            {
                target.Parent = null;
                target.RootId = -1;

                ResetRootId();
            }

            return result;
        }
        /// <summary>
        /// IEquatable.Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Tag? other)
        {
            return this.Id == (other?.Id ?? Guid.Empty);
        }
        /// <summary>
        /// GetHash from Guid
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }


    }
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
        /// 角の丸さ(Card)
        /// </summary>
        float _corner = 7f;
        /// <summary>
        /// 角の丸さ(Tag)
        /// </summary>
        float _cornerTag = 20f;
        /// <summary>
        /// CardObjectか否か<br/><br/>
        /// ・false: Tag用
        /// </summary>
        bool IsCard = true;

        public DraggableGraphics(FlashCardProperty _property, GraphicsView parent, bool isCard = true)
        {
            parentView = parent;
            property = _property;
            IsCard = isCard;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float OFFSET = 1f;

            Debug.WriteLine($"◆ DraggableGraphics.Draw");

            if (IsCard) // Card用の設定
            {
                // 影の色と透明度を設定
                //canvas.SetShadow(new SizeF(OFFSET, OFFSET), 5, Colors.RosyBrown.WithAlpha(0.5f));

                // 塗りつぶしの方法を設定
                canvas.FillColor = property.CardBackgroundColor;
                canvas.FillRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width - OFFSET, dirtyRect.Height - OFFSET, _corner);
                Debug.WriteLine($"◆--- Card Draw at Position({dirtyRect.X},{dirtyRect.Y}),Size({dirtyRect.Width},{dirtyRect.Height})");

                // 枠線の色と線幅を設定
                canvas.StrokeColor = Colors.RosyBrown;
                canvas.StrokeSize = 0.2f; // 線の太さ
                canvas.DrawRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width - OFFSET, dirtyRect.Height - OFFSET, _corner - 1f);

                // 削除ボタン
                float size = 10f;// カードの大きさに応じた比率の方が良いが、プラットフォームによって大きさは異なる為調整は必要
                float padding = 5f;
                float deleteButtonX = dirtyRect.Right - size - padding - 3f;
                float deleteButtonY = dirtyRect.Top;
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(deleteButtonX, deleteButtonY, size, size);
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 14;
                canvas.DrawString(
                    "×",
                    deleteButtonX,
                    deleteButtonY,
                    size + padding,
                    size + padding,
                    HorizontalAlignment.Left,
                    VerticalAlignment.Top
                );
            }
            else // Tag用の設定
            {
                // 影の色と透明度を設定
                //canvas.SetShadow(new SizeF(OFFSET, OFFSET), 5, Colors.RosyBrown.WithAlpha(0.5f));

                // 塗りつぶしの方法を設定
                canvas.FillColor = property.CardBackgroundColor;
                canvas.FillRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width - OFFSET, dirtyRect.Height - OFFSET, _cornerTag);
                Debug.WriteLine($"◆--- Tag Draw at Position({dirtyRect.X},{dirtyRect.Y}),Size({dirtyRect.Width},{dirtyRect.Height})");

                // 枠線の色と線幅を設定
                canvas.StrokeColor = Colors.RosyBrown;
                canvas.StrokeSize = 0.2f; // 線の太さ
                canvas.DrawRoundedRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width - OFFSET, dirtyRect.Height - OFFSET, _corner - 1f);
            }

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
