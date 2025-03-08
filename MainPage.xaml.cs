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
    public static class TableComponent
    {
        public static int GenerateCardCount { get; private set; }
        public static MainPage? MainPage { get; set; }// Initiated MainPage Constructor.

        public static Dictionary<View, int> VisualComponentDictionary = new();
        public static Dictionary<GraphicsView, Editor> EditorDictionary = new();
        public static GraphicsView? GenerateCard(this MainPage mainPage)
        {
            Debug.WriteLine($"◆ TableComponent.GenerateCard()");
            GenerateCardCount++;

            // grid reference
            var grid = mainPage.Content as Grid;
            if (grid is null)
            {
                Debug.WriteLine($"◆[ERROR] reference missing. MainPage content don't have 'Grid'.");
                return null;
            }

            // init
            var _drawable = new DraggableGraphics(GenerateCardCount);
            var _graphicsView = new GraphicsView
            {
                Drawable = _drawable,
                HeightRequest = _drawable.Height,
                WidthRequest = _drawable.Width
            };

            // application windows resize event hundler
            _graphicsView.SizeChanged += (s, e) =>
            {
                Debug.WriteLine($"◆ GraphicsView size changes");
                Debug.WriteLine($"◆--- GraphicsView size : ({_graphicsView.Height},{_graphicsView.Width})");
                Debug.WriteLine($"◆--- GraphicsView position : ({_graphicsView.Y},{_graphicsView.X})");

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());

                // span Textbox                
                var textbox = EditorDictionary[_graphicsView];
                textbox.WidthRequest = _graphicsView.Width - 10f;
                textbox.HeightRequest = _graphicsView.Height - 20f;
            };

            // application windows touch event hundler
            _graphicsView.StartInteraction += (s, e) =>
            {
                Debug.WriteLine($"◆ Touch Card obj");
                var graph = s as GraphicsView;
                if (graph is null) return;
                var draggable = graph.Drawable as DraggableGraphics;
                if (draggable is null) return;

                // debug position
                var touchX = e.Touches[0].X + graph.TranslationX; // タッチされたX座標
                var touchY = e.Touches[0].Y + graph.TranslationY; // タッチされたY座標
                Debug.WriteLine($"◆--- touched position : ({touchX},{touchY})");

                // ZIndex realignment
                ZIndexMove(graph);
                Debug.WriteLine($"◆--- GraphicsView touched : No.({draggable.Number}) value[{VisualComponentDictionary[graph]}]");

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
                    Debug.WriteLine($"◆!!!--- touched delete button");

                    VisualComponentDictionary.Remove(graph);

                    grid.Children.Remove(graph);
                    VisualComponentDictionary.Remove(graph);

                    if (EditorDictionary.ContainsKey(graph))
                    {
                        grid.Children.Remove(EditorDictionary[graph]);
                        VisualComponentDictionary.Remove(EditorDictionary[graph]);
                        EditorDictionary.Remove(graph);
                    }

                    graph.Drawable = null;
                    graph.GestureRecognizers.Clear();
                    graph = null;

                    return;
                }

                /*
                // Add Textbox Button
                if (touchX >= textboxButton.Left && touchX <= textboxButton.Right && touchY >= textboxButton.Top && touchY <= textboxButton.Bottom)
                {
                    Debug.WriteLine($"◆!!!--- generate textbox");
                    var textbox = new Editor()
                    {
                        Placeholder = "initialize textbox",
                        TranslationX = graph.TranslationX,
                        TranslationY = graph.TranslationY + padding,
                        WidthRequest = graph.Width - 10f,
                        HeightRequest = graph.Height - 20f,
                        BackgroundColor = Colors.White,
                        TextColor = Colors.Black,
                        VerticalTextAlignment = TextAlignment.Start,
                        IsVisible = true,
                        IsEnabled = true,
                        ZIndex = 0
                    };
                    textbox.Focused += (s, e) =>
                    {
                        ZIndexMove(graph);
                    };
                    Debug.WriteLine($"◆!!!--- textbox Rect({textbox.TranslationX},{textbox.TranslationY},{textbox.WidthRequest},{textbox.HeightRequest})");

                    // add                   
                    /
                     　EditorDictionary       ... Cardの参照とセットになっているtextboxオブジェクトの参照
                     　CardDictionary         ... Cardの参照とZINdexの辞書
                     　mainPage.grid.Children ... MainPageに描画されているViewオブジェクトの参照
                    /

                    if (EditorDictionary.ContainsKey(_graphicsView))
                    {
                        if (VisualComponentDictionary.ContainsKey(EditorDictionary[_graphicsView]))
                        {
                            VisualComponentDictionary.Remove(EditorDictionary[_graphicsView]);
                        }

                        grid.Children.Remove(EditorDictionary[_graphicsView]);
                        EditorDictionary.Remove(_graphicsView);
                    }

                    MainThread.BeginInvokeOnMainThread ( () =>
                    {
                        grid.Children.Add(textbox);

                        EditorDictionary.Add(_graphicsView, textbox);
                        VisualComponentDictionary.Add(textbox, 1);
                    });
                  */           

                // Invalidate
                ZIndexMove(graph);

                // debug
                foreach (var view in grid)
                {
                    Debug.WriteLine($"◆--- Index[{view.ZIndex}],[{view}]");
                }
            };            
            
            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();// Card1枚につき1つインスタンスしているので個別に反応してほしいんだが？
            panGesture.PanUpdated += mainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ          

            // add children                               
            grid.Children.Add(_graphicsView);           

            // registered Dictionary
            if (VisualComponentDictionary.Count > 0)
            {
                VisualComponentDictionary.Add(_graphicsView, VisualComponentDictionary.Last().Value + 1);
            }
            else
            {
                VisualComponentDictionary.Add(_graphicsView, 1);
            }

            // generate textbox
            GenerateTextBox(_graphicsView);

            // debug
            Debug.WriteLine($"◆--- generate card : value[{VisualComponentDictionary.Last().Value}], MainPage[{mainPage is not null}], GraView[{_graphicsView is not null}]");           

            return _graphicsView;
        }

        static void GenerateTextBox(GraphicsView graph)
        {
            Debug.WriteLine($"◆ generate textbox");

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
                ZIndexMove(graph);
            };
            //Debug.WriteLine($"◆--- textbox Rect({textbox.TranslationX},{textbox.TranslationY},{textbox.WidthRequest},{textbox.HeightRequest})");

            // add                   
            var grid = MainPage?.Content as Grid;
            if (grid is null)
            {
                Debug.WriteLine($"◆[ERROR] reference missing. MainPage content don't have 'Grid'.");
                return;
            }

            if (EditorDictionary.ContainsKey(graph))
            {
                if (VisualComponentDictionary.ContainsKey(EditorDictionary[graph]))
                {
                    VisualComponentDictionary.Remove(EditorDictionary[graph]);
                }

                grid.Children.Remove(EditorDictionary[graph]);
                EditorDictionary.Remove(graph);
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                grid.Children.Add(textbox);

                EditorDictionary.Add(graph, textbox);
                VisualComponentDictionary.Add(textbox,0);
            });
        }

        static void ZIndexMove(GraphicsView touchCard)
        {
            Editor? pairTb = null;

            // タッチしたカード以外をのvalueを+1し、タッチしたカードは1(最小値)にする
            foreach(var key in VisualComponentDictionary.Keys.ToList())
            {
                if( key == touchCard )
                {
                    VisualComponentDictionary[key] = 1;
                    if(EditorDictionary.ContainsKey(touchCard))
                    {
                        VisualComponentDictionary[EditorDictionary[touchCard]] = 0;
                        pairTb = EditorDictionary[touchCard];
                    }                   
                }
                else
                {
                    if (key != pairTb)
                    {
                        VisualComponentDictionary[key] = VisualComponentDictionary[key] + 2;
                        Debug.WriteLine($" plus ");
                    }                   
                }

                // 大きいほど一番上にカードが来る ので、ZIndex = int.MaxValue - dictionary.value
                if( MainPage is not null )
                {
                    var gridContent = MainPage.Content as Grid;
                    var view = gridContent?.Children.FirstOrDefault(x => x == key) as View;

                    if( view is not null )
                    {
                        view.ZIndex = int.MaxValue - VisualComponentDictionary[key];
                        //Debug.WriteLine($"◆---Index [{int.MaxValue - VisualComponentDictionary[key]}]. Key is [{view}]. ");
                    }                   
                }               
            }
        }
    }
    public partial class MainPage : ContentPage
    {
        internal Anchor _dragAnchor;

        public MainPage()
        {
            Debug.WriteLine($"◆ MainPage Constructor");

            InitializeComponent();// initialize and read XAML. This generate when initializing and belong "obj".

            // init
            TableComponent.MainPage = this;
            _dragAnchor = new Anchor(0, 0);
            Content = new Grid();
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
                Debug.WriteLine($"◆--- MainPage Size({Width},{Height})");
                Debug.WriteLine($"◆--- anchor({_dragAnchor.StartX},{_dragAnchor.StartY})");
                return;
            }

            // GestureStatus が Running の場合に位置を更新
            if (e.StatusType == GestureStatus.Running)
            {
                // ドラッグスタート位置からドラッグ後の位置を出力
                Debug.WriteLine($"◆--- rectangle total moving [before] ({rectangle.TranslationX},{rectangle.TranslationY})");
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
                if( TableComponent.EditorDictionary.ContainsKey(rectangle) )
                {
                    TableComponent.EditorDictionary[rectangle].TranslationX = nextX;
                    TableComponent.EditorDictionary[rectangle].TranslationY = nextY + 5f;
                }
                
                Debug.WriteLine($"◆--- rectangle total moving [after] ({rectangle.TranslationX},{rectangle.TranslationY})");

                // 再描画を要求
                rectangle.Invalidate();
            }
        }
        private void MainPage_SizeChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine($"◆ MainPage.MainPage_SizeChanged");

            // この時点で Height および Width は有効な値になります
            double height = this.Height;
            double width = this.Width;

            Debug.WriteLine($"◆--- MainPage Height: {height}, Width: {width}");
        }
    }
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
