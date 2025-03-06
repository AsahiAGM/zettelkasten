using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using System.Linq;
using Microsoft.Maui;

namespace Zettelkasten
{
    public static class TableComponent
    {
        public static int GenerateCardCount { get; private set; }
        public static MainPage? MainPage { get; set; }// Initiated MainPage Constructor.

        public static Dictionary<GraphicsView, int> CardDictionary = new();
        public static GraphicsView GenerateCard(this MainPage mainPage)
        {
            Debug.WriteLine($"◆ TableComponent.GenerateCard()");
            GenerateCardCount++;

            var _drawable = new DraggableGraphics(GenerateCardCount);
            var _graphicsView = new GraphicsView
            {
                Drawable = _drawable,
                HeightRequest = _drawable.Height,
                WidthRequest = _drawable.Width
            };

            _graphicsView.SizeChanged += (s,e) =>
            {
                Debug.WriteLine($"◆--- GraphicsView size : ({_graphicsView.Height},{_graphicsView.Width})");               
                Debug.WriteLine($"◆--- GraphicsView position : ({_graphicsView.Y},{_graphicsView.X})");

                // Invalidate
                _graphicsView.IsVisible = true;
                MainThread.InvokeOnMainThreadAsync(() => _graphicsView.Invalidate());
            };

            _graphicsView.StartInteraction += (s,e) =>
            {
                var graph = s as GraphicsView;
                if (graph is null) return;
                var draggable = graph.Drawable as DraggableGraphics;
                if (draggable is null) return;

                ZIndexMove(graph);
                Debug.WriteLine($"◆--- GraphicsView touched : No.({draggable.Number}) value[{CardDictionary[graph]}]");
            };
            
            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();// Card1枚につき1つインスタンスしているので個別に反応してほしいんだが？
            panGesture.PanUpdated += mainPage.OnPanUpdated;// メインページのドラッグ処理を拾う
            _graphicsView.GestureRecognizers.Add(panGesture);// GraphicsView.GestureRecognizersは個別のプロパティ          

            // add children
            mainPage.grid.Children.Add(_graphicsView);

            // Dictionaryに登録
            if (CardDictionary.Count > 0)
            {
                CardDictionary.Add(_graphicsView, CardDictionary.Last().Value + 1);
            }
            else
            {
                CardDictionary.Add(_graphicsView, 1);
            } 

            Debug.WriteLine($"◆--- generate card : value[{CardDictionary.Last().Value}], MainPage[{mainPage is not null}], GraView[{_graphicsView is not null}]");           

            return _graphicsView;
        }

        static void ZIndexMove(GraphicsView touchCard)
        {
            // タッチしたカード以外をのvalueを+1し、タッチしたカードは1(最小値)にする
            foreach(var key in CardDictionary.Keys.ToList())
            {                             
                if( key == touchCard )
                {
                    CardDictionary[key] = 1;                   
                }
                else
                {
                    CardDictionary[key]++;
                }

                // 大きいほど一番上にカードが来る ので、ZIndex = int.MaxValue - dictionary.value
                key.ZIndex = int.MaxValue - CardDictionary[key];
            }
        }
    }
    public partial class MainPage : ContentPage
    {
        internal Grid grid;
        internal Anchor _dragAnchor;

        public MainPage()
        {
            Debug.WriteLine($"◆ MainPage Constructor");

            InitializeComponent();// initialize and read XAML. This generate when initializing and belong "obj".

            // init
            TableComponent.MainPage = this;
            _dragAnchor = new Anchor(0, 0);

            grid = new Grid();
            Content = grid;
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
        public readonly int Number;
        public readonly float Width = 200f;
        public readonly float Height = 100f;

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
