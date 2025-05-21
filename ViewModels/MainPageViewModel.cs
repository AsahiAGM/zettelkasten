using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zettelkasten.Models;

namespace Zettelkasten.ViewModels
{
    public class MainPageViewModel
    {
        /// <summary>
        /// ドラッグ移動を開始した際の基準点
        /// </summary>
        internal Anchor _dragAnchor { get; set; } = new();

        /// <summary>
        /// 現在の編集対象テーブル
        /// </summary>
        public Table CurrentTable { get; set; } = new();




        /// <summary>
        /// AppShellから呼ばれるカード生成メソッド<br/>
        /// </summary>
        public static void GenerateCard()
        {
            var newCard = FlashCardFactory.GererateCard();

            // 先に生成されているカードのZIndexを調整
            if (CurrentTable.FlashCardDictionary.Count > 0)
            {
                foreach (var card in CurrentTable.FlashCardDictionary.Values)
                {
                    card.MoveLower();
                }
            }

            FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(newCard.CardObject.Background, out var key);

            // registrate dictionary
            if (key is null)
            {
                Debug.WriteLine($"[ERROR][MainPage.GenerateKey()] Key not registered.");
                return;
            }
            CurrentTable.FlashCardDictionary.Add(key, newCard);
        }

        /// <summary>
        /// カードのイベントハンドラに委譲する階層移動<br/>
        /// </summary>
        /// <param name="touchCard"></param>
        public static void ZIndexMove(GraphicsView touchCard)
        {
            // 全てのカードを１つ下段に移す
            foreach (var card in CurrentTable.FlashCardDictionary.Values.ToList())
            {
                card.MoveLower();
            }

            FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(touchCard, out var key);
            if (key is not null && CurrentTable.FlashCardDictionary.ContainsKey(key))
            {
                // 触れたGraphicsが所属するカードを最上段へ
                CurrentTable.FlashCardDictionary[key].MoveTop();
            }
        }

        /// <summary>
        /// Drag開始時の基準点を設定するメソッド<br/>
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void StartDrag(double startX, double startY)
        {
            _dragAnchor = new Anchor(startX, startY);
        }

        /// <summary>
        /// Drag中の移動位置を取得するメソッド<br/>
        /// </summary>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="totalX"></param>
        /// <param name="totalY"></param>
        /// <param name="pageWidth"></param>
        /// <param name="pageHeight"></param>
        /// <param name="viewWidth"></param>
        /// <param name="viewHeight"></param>
        /// <returns></returns>
        public (double nextX, double nextY) GetDraggedPosition(
        double currentX, double currentY,
        double totalX, double totalY,
        double pageWidth, double pageHeight,
        double viewWidth, double viewHeight)
        {
            double FIX_LIMIT = 5f;
            double limitHeight = (pageHeight / 2) - (viewHeight / 2) - FIX_LIMIT;
            double limitWidth = (pageWidth / 2) - (viewWidth / 2) - FIX_LIMIT;

            double nextX = _dragAnchor.StartX + totalX;
            double nextY = _dragAnchor.StartY + totalY;

            // 範囲制限
            nextX = Math.Clamp(nextX, -limitWidth, limitWidth);
            nextY = Math.Clamp(nextY, -limitHeight, limitHeight);

            return (nextX, nextY);
        }

        public bool TryUpdateCardPosition(GraphicsView view, double x, double y)
        {
            if (FlashCardGuid.FlashCardGuidKeyDict.TryGetValue(view, out var key) &&
                CurrentTable.FlashCardDictionary.TryGetValue(key, out var card))
            {
                card.Translation(x, y);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// オブジェクトを移動させる際の基準点
    /// </summary>
    class Anchor
    {
        public double StartX;
        public double StartY;
        public Anchor(double _x = 0, double _y = 0) { StartX = _x; StartY = _y; }
    }
}
