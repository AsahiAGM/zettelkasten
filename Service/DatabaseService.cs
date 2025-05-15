using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettelkasten.Models;

namespace Zettelkasten.Service
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database = null!;

        private static async Task InitAsync()
        {
            if (_database != null)
                return;

            // ファイルパスの取得（デバイスに応じて安全な場所に保存）※　FileSystem.AppDataDirectory　は　Microsoft.Maui.Storage　に含まれる
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "appdata.db");

            _database = new SQLiteAsyncConnection(dbPath);

            // テーブルを作成（なければ作成）
            await _database.CreateTableAsync<Card>();
            await _database.CreateTableAsync<TreeTag>();
            await _database.CreateTableAsync<TagCategory>();
            await _database.CreateTableAsync<MeaningTag>();
            await _database.CreateTableAsync<CardTag>();
        }

        public static async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            await InitAsync();
            return _database;
        }

        /// <summary>
        /// カードを保存するメソッド
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static async Task<int> SaveCardAsync(Card card)
        {
            if (card.CardId != 0)
            {
                // 既存カードの場合、更新
                return await _database.UpdateAsync(card);
            }
            else
            {
                // 新しいカードの場合、挿入
                return await _database.InsertAsync(card);
            }
        }
        /// <summary>
        /// カードをIDで取得するメソッド
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static async Task<Card> GetCardAsync(int cardId)
        {
            return await _database.Table<Card>().Where(c => c.CardId == cardId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// すべてのカードを取得するメソッド
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Card>> GetAllCardsAsync()
        {
            return await _database.Table<Card>().ToListAsync();
        }

        /// <summary>
        /// カードにタグを追加するメソッド
        /// </summary>
        /// <param name="cardTag"></param>
        /// <returns></returns>
        public static async Task<int> AddCardTagAsync(CardTag cardTag)
        {
            return await _database.InsertAsync(cardTag);
        }

        /// <summary>
        /// 特定のタグを持つカードを取得するメソッド
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static async Task<List<Card>> GetCardsByTagAsync(int tagId)
        {
            var cardTags = await _database.Table<CardTag>().Where(ct => ct.TagId == tagId).ToListAsync();
            var cardIds = cardTags.Select(ct => ct.CardId).ToList();
            return await _database.Table<Card>().Where(c => cardIds.Contains(c.CardId)).ToListAsync();
        }
        /// <summary>
        /// MeaningAndTagを保存するメソッド
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static async Task<int> SaveMeaningTagAsync(MeaningTag tag)
        {
            var connection = new SQLiteAsyncConnection(App.DatabasePath);
            return await connection.InsertAsync(tag);            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static async Task<Card?> GetCardByIdAsync(int cardId)
        {
            var connection = new SQLiteAsyncConnection(App.DatabasePath);
            return await connection.Table<Card>().Where(c => c.CardId == cardId).FirstOrDefaultAsync();
        }
    }
}
