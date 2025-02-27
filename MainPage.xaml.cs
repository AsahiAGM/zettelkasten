using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;

namespace Zettelkasten
{
    public partial class MainPage : ContentPage
    {
        private DraggableGraphics _drawable;
        private GraphicsView _graphicsView;
        private Anchor? _dragAnchor;

        public MainPage()
        {
            InitializeComponent();

            _drawable = new DraggableGraphics(this);

            _graphicsView = new GraphicsView
            {
                Drawable = _drawable
            };           

            // PanGestureRecognizer を追加してドラッグを有効にする
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            _graphicsView.GestureRecognizers.Add(panGesture);

            Content = _graphicsView;

            // ページのサイズが確定したら初期位置を設定
            this.SizeChanged += OnPageSizeChanged;
        }

        // ページのサイズが確定したら初期位置を設定
        private void OnPageSizeChanged(object? sender, EventArgs e)
        {
            // ページの中央に配置
            _drawable.X = (float)(this.Width / 2 - (_drawable.Width/2) ); // 矩形の幅が200なので調整
            _drawable.Y = (float)(this.Height / 2 - (_drawable.Height/2) );  // 矩形の高さが100なので調整

            // init
            _dragAnchor = new Anchor(_drawable.X, _drawable.Y);

            _graphicsView.Invalidate(); // 再描画
        }


        // ドラッグ更新時に呼ばれるメソッド
        private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            float Limit = 20f;

            var rectangle = sender as GraphicsView;
           
            if (rectangle == null) return;

            // GestureStatusがRunningの場合に位置を更新
            if (e.StatusType == GestureStatus.Running)
            {
                var nextX = (float)_dragAnchor.StartX + (float)e.TotalX;
                var nextY = (float)_dragAnchor.StartY + (float)e.TotalY;

                // 矩形がアプリケーションの外に出るようなドラッグは受け付けない
                if (nextY < Limit ) nextY = Limit;
                if (nextY > Height - (Limit+_drawable.Height) ) nextY = (float)(Height - (Limit + _drawable.Height));
                if (nextX < Limit) nextX = Limit;
                if (nextX > Width - (Limit+_drawable.Width) ) nextX = (float)(Width - (Limit + _drawable.Width));

                // ドラッグスタート位置からドラッグ後の位置を出力
                _drawable.X = nextX;
                _drawable.Y = nextY;

                // 再描画を要求
                _graphicsView.Invalidate();               
            }

            if(e.StatusType == GestureStatus.Completed)
            {
                // update
                _dragAnchor = new Anchor(_drawable.X, _drawable.Y);
            }
        }        
    }
    public class DraggableGraphics : IDrawable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public readonly float Width = 200;
        public readonly float Height = 100;

        private float _corner = 10f;
        

        public DraggableGraphics(MainPage mainPage)
        {
            X = (float)mainPage.Width / 2;
            Y = (float)mainPage.Height / 2;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.FillRoundedRectangle(X, Y, Width, Height, _corner);
        }
    }

    class Anchor
    {
        public double StartX;
        public double StartY;
        public Anchor(double _x, double _y) { StartX = _x; StartY = _y; }
    }
}
