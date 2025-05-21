using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xunit;

using Zettelkasten.Models;

namespace Zettelkasten.Service
{
    public class DatabaseServiceTests
    {
        private SQLiteAsyncConnection _database;

        // テストの前にデータベースを初期化
        public DatabaseServiceTests()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "test.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Card>().Wait();  // Cardテーブルを作成
        }

        /// <summary>
        /// 保存と取得のテスト
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SaveCardAsync_ShouldInsertCardCorrectly()
        {
            // Arrange: カードを作成
            var newCard = new Card
            {
                Content = "Test card content",
                CreatedAt = DateTime.UtcNow
            };

            // Act: カードを保存
            var result = await DatabaseService.SaveCardAsync(newCard);

            // Assert: ちゃんと保存されたか
            Xunit.Assert.True(result > 0, "カードの保存に失敗しました");

            // 確認のため再取得
            var loadedCard = await DatabaseService.GetCardByIdAsync(newCard.CardId);

            Xunit.Assert.NotNull(loadedCard);
            Xunit.Assert.Equal(newCard.Content, loadedCard.Content);
        }










        // カードの保存テスト
        [Fact]
        public async Task SaveCardAsync_ShouldSaveCard_WhenCardIsNew()
        {
            // Arrange: 新しいカードを作成
            var newCard = new Card { Content = "Test Card" };

            // Act: カードを保存
            var result = await DatabaseService.SaveCardAsync(newCard);

            // Assert: 保存したカードがDBに存在するか確認
            var savedCard = await _database.Table<Card>().Where(c => c.Content == "Test Card").FirstOrDefaultAsync();
            Xunit.Assert.NotNull(savedCard);
            Xunit.Assert.Equal("Test Card", savedCard.Content);
        }

        // カードの取得テスト
        [Fact]
        public async Task GetCardAsync_ShouldReturnCard_WhenCardIdIsValid()
        {
            // Arrange: 新しいカードを作成して保存
            var newCard = new Card { Content = "Another Test Card" };
            await DatabaseService.SaveCardAsync(newCard);

            // Act: 保存したカードをIDで取得
            var savedCard = await DatabaseService.GetCardAsync(newCard.CardId);

            // Assert: 取得したカードが正しいか確認
            Xunit.Assert.NotNull(savedCard);
            Xunit.Assert.Equal("Another Test Card", savedCard.Content);
        }

        // 複数のカードを取得するテスト
        [Fact]
        public async Task GetAllCardsAsync_ShouldReturnAllCards()
        {
            // Arrange: 複数のカードを保存
            await DatabaseService.SaveCardAsync(new Card { Content = "Card 1" });
            await DatabaseService.SaveCardAsync(new Card { Content = "Card 2" });

            // Act: すべてのカードを取得
            var cards = await DatabaseService.GetAllCardsAsync();

            // Assert: カードが2つ取得できているか確認
            Xunit.Assert.Equal(2, cards.Count);
        }

        // タグとカードの関係をテスト
        [Fact]
        public async Task AddCardTagAsync_ShouldAddCardTagRelationship()
        {
            // Arrange: 新しいカードとタグを作成
            var newCard = new Card { Content = "Card with Tag" };
            await DatabaseService.SaveCardAsync(newCard);

            // MeaningTagのインスタンスを作成
            var tag = new MeaningTag { Name = "TestTag" };
            await DatabaseService.SaveMeaningTagAsync(tag);  // SaveMeaningTagAsyncで保存

            // CardTagのインスタンスを作成
            var cardTag = new CardTag { CardId = newCard.CardId, TagId = tag.TagId };

            // CardとTagの関連付けを保存
            var result = await DatabaseService.AddCardTagAsync(cardTag);

            // Assert: 関連付けが正しく保存されているか確認
            var savedCardTag = await _database.Table<CardTag>().Where(ct => ct.CardId == newCard.CardId && ct.TagId == tag.TagId).FirstOrDefaultAsync();
            Xunit.Assert.NotNull(savedCardTag);
        }
    }

}
