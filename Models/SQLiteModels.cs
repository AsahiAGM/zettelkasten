using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettelkasten.Models
{
    /// <summary>
    /// FlashCardのデータモデル
    /// </summary>
    public class Card
    {
        [PrimaryKey, AutoIncrement]
        public int CardId { get; set; }

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } // ISO形式の文字列

        public int TreeTagId { get; set; }
    }
    /// <summary>
    /// ツリー構造のための整数タグ
    /// </summary>
    public class TreeTag
    {
        [PrimaryKey, AutoIncrement]
        public int TreeTagId { get; set; }

        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }
    }
    /// <summary>
    /// 意味タグをカテゴリ分けするためのクラス
    /// </summary>
    public class TagCategory
    {
        [PrimaryKey, AutoIncrement]
        public int TagCategoryId { get; set; }

        public string Name { get; set; } = null!;
    }
    /// <summary>
    /// 意味タグ
    /// </summary>
    public class MeaningTag
    {
        [PrimaryKey, AutoIncrement]
        public int TagId { get; set; }

        public string Name { get; set; } = null!;

        public int TagCategoryId { get; set; }
    }
    /// <summary>
    /// 複合主キーのための中間テーブル<br/><br/>
    /// CardId + TagId を複合キーにするのが理想ですが、sqlite-net-pcl は複合主キー非対応のため、Id を追加しています。
    /// </summary>
    public class CardTag
    {
        [PrimaryKey]
        public int Id { get; set; } // SQLiteに複合主キーは直接使いづらいため1列の主キーに

        public int CardId { get; set; }

        public int TagId { get; set; }

        public double Weight { get; set; }
    }










    /// <summary>
    /// MainPageからアクセスできるかどうかテストするためのクラス
    /// </summary>
    public class TestCard
    {
        public TestCard() { }

        public void Check()
        {
            Debug.WriteLine("Check method called");
        }
    }
}
