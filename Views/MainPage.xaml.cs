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
using System.Runtime.CompilerServices;

using SQLite;
using Zettelkasten.Models;
using Zettelkasten.ViewModels;

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
        /// View Model instance
        /// </summary>
        public static MainPageViewModel ViewModel = new MainPageViewModel();


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

            var card = new TestCard();
            card.Check();
        }     
        
        /// <summary>
        /// MainPageが初期化された際に一度だけ呼ばれるメソッド
        /// </summary>
        /// <param name="args"></param>
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            Debug.WriteLine($"◆ MainPage.OnNavigatedTo()");

            base.OnNavigatedTo(args);

            var mainPageContent = this.Content as Grid;

            if (mainPageContent is null)
            {
                Debug.WriteLine($"◆[ERROR] MainPage.OnNavigatedTo() : MainPage content is null or not Grid.");
                return;
            }
            
            var newTable = new Table(mainPageContent);
            CurrentTable.table = newTable;
            CurrentTable.TableDictionary.Add(1, newTable);
        }
        
        // ドラッグ更新時に呼ばれるメソッド
        internal void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            Debug.WriteLine($"◆ MainPage.OnPanUpdated");
            //Debug.WriteLine($"◆--- sender :{sender.GetType().Name}");
            //Debug.WriteLine($"◆--- anchor({_dragAnchor.StartX},{_dragAnchor.StartY})");

            // null or type check
            if (sender is not GraphicsView rectangle) return;

            double FIX_LIMIT = 5f;
            double LimitHeight = ((double)Height / 2) - ((double)rectangle.Height / 2) - FIX_LIMIT;
            double LimitWidth = ((double)Width / 2) - ((double)rectangle.Width / 2) - FIX_LIMIT;

            if (e.StatusType == GestureStatus.Started)
            {
                //Debug.WriteLine($"◆--- MainPage Size({Width},{Height})");
                //Debug.WriteLine($"◆--- anchor({_dragAnchor.StartX},{_dragAnchor.StartY})");

                ViewModel.StartDrag(rectangle.TranslationX,rectangle.TranslationY);
                return;
            }

            // GestureStatus が Running の場合に位置を更新
            if (e.StatusType == GestureStatus.Running)
            {
                var (nextX, nextY) = ViewModel.GetDraggedPosition(
                                        rectangle.TranslationX,
                                        rectangle.TranslationY,
                                        e.TotalX,
                                        e.TotalY,
                                        Width, Height,
                                        rectangle.Width, rectangle.Height
                                    );

                if (ViewModel.TryUpdateCardPosition(rectangle, nextX, nextY))
                {
                    MainThread.BeginInvokeOnMainThread(() => rectangle.Invalidate());
                }

                // ドラッグスタート位置からドラッグ後の位置を出力
                //Debug.WriteLine($"◆--- rectangle total moving [before] ({rectangle.TranslationX},{rectangle.TranslationY})");
                //Debug.WriteLine($"◆--- rectangle size ({rectangle.Width},{rectangle.Height})");

                // 矩形がアプリケーションの外に出るようなドラッグは受け付けない
                if (nextY > LimitHeight) nextY = LimitHeight;
                if (nextY < -1 * LimitHeight) nextY = -1 * LimitHeight;
                if (nextX > LimitWidth) nextX = LimitWidth;
                if (nextX < -1 * LimitWidth) nextX = -1 * LimitWidth;

                // 移動後の座標をセット
                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(rectangle,out var key);
                if ( key is not null && CurrentTable.FlashCardDictionary.ContainsKey(key))
                {
                    CurrentTable.FlashCardDictionary[key].Translation(nextX, nextY);
                }

                //Debug.WriteLine($"◆--- rectangle total moving [after] ({rectangle.TranslationX},{rectangle.TranslationY})");

                // 再描画を要求
                MainThread.InvokeOnMainThreadAsync(() => rectangle.Invalidate());
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
    }

    /// <summary>
    /// 現在アクティブであるテーブル
    /// </summary>
    public static class CurrentTable
    {
        /// <summary>
        /// 表示中のテーブル
        /// </summary>
        public static Table table { get; set; } = null!;
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
        /// 生成したFlashCardの辞書<br/><br/>
        /// key: GraphicsViewのGuid<br/>
        /// Count: 生成されているカードの数に等しい
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public static Dictionary<FlashCardGuid, FlashCard> FlashCardDictionary => table.FlashCardDictionary;             

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
            
            var _drawable = new FlashCardGraphics(property, _graphicsView);

            _graphicsView.Drawable = _drawable;           
            _graphicsView.WidthRequest = _drawable.property.CardWidth;
            _graphicsView.HeightRequest = _drawable.property.CardHeight;

            // application windows resize event hundler
            _graphicsView.SizeChanged += (s, e) =>
            {
                /*
                double TextboxSize_FIX_X = 20;
                double TextboxSize_FIX_Y = 20;

                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(_graphicsView, out var key);
                if (!CurrentTable.FlashCardDictionary.ContainsKey(key))
                {
                    Debug.WriteLine($"◆[ERROR][FlashCardFactory] _graphicsView.StartInteraction : {_graphicsView} is no registrated");
                    return;
                }

                // span Textbox               
                var card = CurrentTable.FlashCardDictionary[key];
                card.CardObject.Textbox.WidthRequest = _graphicsView.WidthRequest - TextboxSize_FIX_X;
                card.CardObject.Textbox.HeightRequest = _graphicsView.HeightRequest - TextboxSize_FIX_Y;               
                */


                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());
            };

            // application windows touch event hundler
            _graphicsView.StartInteraction += (s, e) =>
            {
                //Debug.WriteLine($"◆ Touch Card obj");
                var graph = s as GraphicsView;
                if (graph is null) return;
                var draggable = graph.Drawable as FlashCardGraphics;
                if (draggable is null) return;

                // get key
                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(_graphicsView, out var key);
                if (key is null ||  !CurrentTable.FlashCardDictionary.ContainsKey(key))
                {
                    Debug.WriteLine($"◆[ERROR][FlashCardFactory] _graphicsView.StartInteraction : {_graphicsView} is no registrated");
                    return;
                }

                Debug.WriteLine($"◆[CardObject .StartInteraction] Touch {CurrentTable.FlashCardDictionary[key].CardObject.Id}");

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
                    var flashCard = CurrentTable.FlashCardDictionary[key];

                    foreach (var viewModel in flashCard.GetViewModel())
                    {
                        grid.Children.Remove(viewModel);
                    }

                    CurrentTable.FlashCardDictionary.Remove(key);// FlashCardなくなる->CobjとTagもなくなる->CWTableからも消える

                    graph.Drawable = null;
                    graph.GestureRecognizers.Clear();
                    graph = null;

                    // id regenerate
                    CurrentTable.table.RootIdRegenerate();

                    return;
                }

                // Invalidate
                MainPageViewModel.ZIndexMove(graph);

            };

            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += MainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ                     

            // generate textbox
            var txbox = GenerateTextBox(_graphicsView, property);

            txbox.FontSize = property.TextFontSize;
            txbox.HeightRequest = property.TextBoxHeight;
            txbox.WidthRequest = property.TextBoxWidth;

            txbox.SizeChanged += (s, e) =>
            {
                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(_graphicsView, out var key);
                if (key is null || !CurrentTable.FlashCardDictionary.ContainsKey(key))
                {
                    Debug.WriteLine($"◆[ERROR][Tag] Tag Handle  _graphicsVie.SizeChanged : {_graphicsView} is no registrated");
                    return;
                }
                var card = CurrentTable.FlashCardDictionary[key];

                Debug.WriteLine($"◆[CardObject] .SizeChanged: Editor size changed [{_graphicsView.Id}]");
                Debug.WriteLine($"◆[CardObject]--- GraphicsView request  : ({_graphicsView.WidthRequest},{_graphicsView.HeightRequest})");
                Debug.WriteLine($"◆[CardObject]--- GraphicsView size     : ({_graphicsView.Width},{_graphicsView.Height})");
                Debug.WriteLine($"◆[CardObject]--- GraphicsView position : ({_graphicsView.X},{_graphicsView.Y})");
                Debug.WriteLine($"◆[CardObject]--- Editor request        : ({card.CardObject.Textbox.WidthRequest},{card.CardObject.Textbox.HeightRequest})");
                Debug.WriteLine($"◆[CardObject]--- Editor size           : ({card.CardObject.Textbox.Width},{card.CardObject.Textbox.Height})");
                Debug.WriteLine($"◆[CardObject]--- Editor position       : ({card.CardObject.TranslationX},{card.CardObject.Textbox.TranslationY})");

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());
            };

            // generate FlashCard           
            var cardObject = new CardObject(_graphicsView, txbox, property);
            var tag = new Tag(cardObject, property);
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
            const double TEXTBOX_Y_FIX = 5d;

            // Editor init
            var textbox = new Editor()
            {
                TranslationX = graph.TranslationX,
                TranslationY = graph.TranslationY + TEXTBOX_Y_FIX,
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
                MainPageViewModel.ZIndexMove(graph);
            };

            return textbox;
        }
    }

    /// <summary>
    /// FlashCardの描画および書式の設定
    /// </summary>
    public class FlashCardProperty
    {
        public float CardHeight { get; set; } = 150f;
        public float CardWidth { get; set; } = 350f;
        [PlantUmlIgnoreAssociation]
        public Color CardBackgroundColor { get; set; } = Colors.White;

        public float TextBoxHeight { get; set; } = 110f;
        public float TextBoxWidth { get; set; } = 300f;
        public int TextFontSize { get; set; } = 16;
        [PlantUmlIgnoreAssociation]
        public Color TextFontColor { get; set; } = Colors.Black;
        [PlantUmlIgnoreAssociation]
        public Color TextBoxBackGround { get; set; } = Colors.Transparent;

        public float TagHeight { get; set; } = 44f;
        public float TagWidth { get; set; } = 80f;
        public int TagFontSize { get; set; } = 10;
        [PlantUmlIgnoreAssociation]
        public Color TagFontColor { get; set; } = Colors.Black;
        [PlantUmlIgnoreAssociation]
        public Color TagBackGround { get; set; } = Colors.Transparent;

        
        public FlashCardProperty() { }
    }
    /// <summary>
    /// GraphicsViewからFlashCardを取得するためのKey
    /// </summary>
    public class FlashCardGuid
    {
        /// <summary>
        /// FlashCardの正規化キー
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Guid id;

        /// <summary>
        /// FlashCardの正規化キーの辞書<br/>
        /// key: GraphicsView
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public static ConditionalWeakTable<GraphicsView, FlashCardGuid> FlashCardGuidKeyDict = new();
        public FlashCardGuid(Guid _id) { id = _id; }
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
        /// Cardの固有Id (CardObjectの固有Id)
        /// </summary>
        [PlantUmlIgnoreAssociation]
        public Guid guid => CardObject.Id;
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

            GenerateKey();
            MoveTop();
        }
        /// <summary>
        /// 要素を最上段に移動
        /// </summary>
        public void MoveTop()
        {
            ZIndex = new List<ViewIndexPair>()
            {
                new ViewIndexPair(0, CardObject.Textbox),
                new ViewIndexPair(1, Tag.Label),
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
                Debug.WriteLine($"◆[FlashCard] {pair.ViewModel}:{pair.ViewModel.Id} has ZIndex {pair.ViewModel.ZIndex}");
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
                Tag.Label
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
            Tag.Translation(CardObject);

            Debug.WriteLine($"[FlashCard] Translation");
            Debug.WriteLine($"--- CardObject ({CardObject.TranslationX},{CardObject.TranslationY})");
            Debug.WriteLine($"--- Tag        ({Tag.TranslationX},{Tag.TranslationY})");
        }
        /// <summary>
        /// 辞書に登録するためのKeyを発行する
        /// </summary>
        /// <returns></returns>

        void GenerateKey()
        {
            var key = new FlashCardGuid( new Guid() );
            FlashCardGuid.FlashCardGuidKeyDict.Add(CardObject.Background, (key) );
            FlashCardGuid.FlashCardGuidKeyDict.Add(Tag.Background, key );
        }

        /// <summary>
        /// View and Index Container
        /// </summary>
        public class ViewIndexPair
        {
            public int Index { get; internal set; }
            [PlantUmlIgnoreAssociation]
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
        public int RootId { get; set; }
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
        public Label Label { get; } = null!;
        /// <summary>
        /// このタグの書式設定
        /// </summary>
        public FlashCardProperty Property { get; } = null!;
        /// <summary>
        /// 表示する文字
        /// </summary>
        string tagString => GetTagId();

        /// <summary>
        /// TagのY座標のオフセット<br/>
        /// </summary>
        const double TAG_PADDING_Y = 5d;
        /// <summary>
        /// Tag.LabelのY座標のオフセット<br/>
        /// </summary>
        const double LABEL_PADDING_Y = -5d;

        /// <summary>
        /// X座標
        /// </summary>
        public double TranslationX 
        {
            get => Background.TranslationX;
            set => Background.TranslationX = value;
        }
        /// <summary>
        /// Y座標
        /// </summary>
        public double TranslationY
        {
            get => Background.TranslationY;
            set => Background.TranslationY = value;
        }
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

        public Tag(CardObject cobj, FlashCardProperty property)
        {
            // init
            Id = Guid.NewGuid();
            CardObject = cobj;
            RootId = CurrentTable.table.RootCardCount + 1;
            Property = property;

            // set ViewModel
            var _graphicsView = new GraphicsView();
            var _drawable = new FlashCardGraphics(property, _graphicsView, false);
            _graphicsView.Drawable = _drawable;
            
            _graphicsView.WidthRequest = property.TagWidth;
            _graphicsView.HeightRequest = property.TagHeight;
            _graphicsView.TranslationX = (cobj.TranslationX - cobj.Background.WidthRequest / 2) + Property.TagWidth / 2;
            _graphicsView.TranslationY = (cobj.TranslationY - cobj.Background.HeightRequest / 2) + TAG_PADDING_Y;

            var _label = GenerateLabel(cobj);
            _label.FontSize = property.TagFontSize;

            // application windows resize event hundler
            _graphicsView.SizeChanged += (s, e) =>
            {
                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(_graphicsView, out var key);
                if (key is null || !CurrentTable.FlashCardDictionary.ContainsKey( key ) )
                {
                    Debug.WriteLine($"◆[ERROR][Tag] Tag Handle  _graphicsVie.SizeChanged : {_graphicsView} is no registrated");
                    return;
                }

                // span Textbox               
                var card = CurrentTable.FlashCardDictionary[key]; // KeyNotFoundExceptionが発生する場合がある 0429
                card.Tag.Label.WidthRequest = _graphicsView.WidthRequest - 10f;
                card.Tag.Label.HeightRequest = _graphicsView.HeightRequest - 20f;                            

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());               
            };           

            _label.SizeChanged += (s, e) =>
            {
                FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(_graphicsView, out var key);
                if (key is null || !CurrentTable.FlashCardDictionary.ContainsKey(key))
                {
                    Debug.WriteLine($"◆[ERROR][Tag] Tag Handle  _label.SizeChanged : {_graphicsView} is no registrated");
                    return;
                }
                var card = CurrentTable.FlashCardDictionary[key];

                Debug.WriteLine($"◆[Tag] .SizeChanged: Editor size changes [{_graphicsView.Id}]");
                Debug.WriteLine($"◆[Tag]--- GraphicsView request  : ({_graphicsView.WidthRequest},{_graphicsView.HeightRequest})");
                Debug.WriteLine($"◆[Tag]--- GraphicsView size     : ({_graphicsView.Width},{_graphicsView.Height})");
                Debug.WriteLine($"◆[Tag]--- GraphicsView position : ({_graphicsView.X},{_graphicsView.Y})");
                Debug.WriteLine($"◆[Tag]--- Label request         : ({card.Tag.Label.WidthRequest},{card.Tag.Label.HeightRequest})");
                Debug.WriteLine($"◆[Tag]--- Label size            : ({card.Tag.Label.Width},{card.Tag.Label.Height})");
                Debug.WriteLine($"◆[Tag]--- Label position        : ({card.Tag.Label.TranslationX},{card.Tag.Label.TranslationY})");

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());
            };

            var gesture = new TapGestureRecognizer();
            gesture.Tapped += (s, e) =>
            {
                // プルダウンリストを表示
                Debug.WriteLine("label tapped");
            };

            _label.GestureRecognizers.Add(gesture);

            // initialize
            Background = _graphicsView;
            Label = _label;
        }
        /// <summary>
        /// Labelを生成
        /// </summary>
        /// <returns></returns>
        public Label GenerateLabel(CardObject card)
        {
            // Label init
            var label = new Label()
            {
                Text = tagString,
                TranslationX = (card.TranslationX - card.Background.WidthRequest / 2) + Property.TagWidth / 2,
                TranslationY = (card.TranslationY - card.Background.HeightRequest / 2) + LABEL_PADDING_Y,
                BackgroundColor = Property.TagBackGround,
                TextColor = Property.TagFontColor,
                FontSize = Property.TagFontSize,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                IsVisible = true,
                IsEnabled = true,
            };

            return label;
        }
        /// <summary>
        /// タグ情報の更新
        /// </summary>
        public void Reload()
        {
            Label.Text = tagString;
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
        /// <param name="Cobj"></param>
        public void Translation(CardObject Cobj)
        {
            TranslationX = (Cobj.TranslationX - Cobj.Background.WidthRequest / 2) + Property.TagWidth / 2;
            TranslationY = (Cobj.TranslationY - Cobj.Background.HeightRequest / 2) + TAG_PADDING_Y;

            Label.TranslationX = (Cobj.TranslationX - Cobj.Background.WidthRequest / 2) + Property.TagWidth / 2;
            Label.TranslationY = (Cobj.TranslationY - Cobj.Background.HeightRequest / 2) + LABEL_PADDING_Y;
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
        /// 親となるタグを変更<br/><br/>
        /// ・指定しない場合はこのタグが新たなルートタグとなる
        /// </summary>
        /// <param name="newParent"></param>
        /// <returns></returns>
        public Tag? ChangeRoot(Tag? newParent = null,int newId = 0)
        {
            if (newParent is null)
            {
                RootId = CurrentTable.table.RootCardCount + 1;
                Parent = null;

                return null;
            }

            Parent = newParent;

            // 0 または配列外を指定した場合は最後尾に追加
            if ( newId == 0 || newId >= Parent.Children.Count() )
            {
                Parent.Children.Add(this);               
            }
            else
            {
                Parent.Children.Insert(newId-1,this);
            }

            RootId = newId;

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
    public class FlashCardGraphics : IDrawable
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
        float _cornerTag = 7f;
        /// <summary>
        /// CardObjectか否か<br/><br/>
        /// ・false: Tag用
        /// </summary>
        bool IsCard = true;

        public FlashCardGraphics(FlashCardProperty _property, GraphicsView parent, bool isCard = true)
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
                canvas.StrokeColor = Colors.Transparent;
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

    
}
