using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlantUmlClassDiagramGenerator.SourceGenerator;
using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using System.Runtime.CompilerServices;

namespace Zettelkasten.Models
{
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
        [PlantUmlIgnoreAssociation]
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
        public Dictionary<FlashCardGuid, FlashCard> FlashCardDictionary = new();

        public Table()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// 現在のTagのツリー構造をIdに反映
        /// </summary>
        public void RootIdRegenerate()
        {
            int rootId = 1;
            foreach (var card in FlashCardDictionary.Values)
            {
                int id = 1;
                card.Tag.RootId = rootId;

                foreach (var child in card.Tag.Children)
                {
                    child.RootId = id;
                    id++;
                }

                rootId++;

                card.Tag.Reload();
                Debug.WriteLine($"{card.CardObject.Id} have rootId [{card.RootId}]");
            }
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
}
