using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SzolgProg_vizsga
{
    public static class Database
    {
        private const string host = "localhost";
        private const string db_user = "root";
        private const string db_psw = "";
        private const string db_name = "book_store";

        /// <summary>
        /// Beilleszt egy új könyvet. Ha nincs ilyen író, vagy kiadó, vagy műfaj akkor létrehoz újat (SQL injection ellen a paraméterezés véd, pl: /"; DELETE FROM `books` WHERE `id` = 3; --/ )
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="ISBN"></param>
        /// <param name="genre"></param>
        /// <param name="description"></param>
        /// <param name="publish_year"></param>
        /// <param name="price"></param>
        /// <param name="on_storage"></param>
        public static async void InsertNewBook(string title, string author, string publisher, string ISBN, string genre, string description, string publish_year, int price, int on_storage = 0)
        {
            var author_id = -1;
            using (var dbCon2 = ConnectionToDatabase)
            {
                
                dbCon2.Open();

                #region Író ID-je
                var cmd2 = new MySqlCommand("SELECT * FROM `author` WHERE `name` LIKE @author", dbCon2);
                _ = cmd2.Parameters.Add(new MySqlParameter("author", author));
                author_id = -1;
                using (var reader = await cmd2.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        author_id = int.Parse(reader.GetString(0)); //UNSAFE
                    }
                    await reader.CloseAsync();
                }
                await dbCon2.CloseAsync(); 
            }
            if (author_id == -1)
                author_id = await InsertNewAuthor(author);
            #endregion
            var dbCon = ConnectionToDatabase;
            #region Kiadó ID-je
            dbCon.Open();
            var cmd = new MySqlCommand("SELECT * FROM `publisher` WHERE `name` LIKE @publisher", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@publisher", publisher));
            var publisher_id = -1;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    publisher_id = int.Parse(reader.GetString(0));
                }
                reader.Close();
            }
            dbCon.Close();
            if (publisher_id == -1)
                publisher_id = await InsertNewPublisher(publisher);
            #endregion

            #region Műfaj ID-je
            dbCon.Open();
            cmd = new MySqlCommand("SELECT * FROM `genre` WHERE `name` LIKE @genre", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@genre", genre));
            var genre_id = -1;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    genre_id = int.Parse(reader.GetString(0));
                }
                reader.Close();
            }
            dbCon.Close();
            if (genre_id == -1)
                genre_id = await InsertNewGenre(genre);
            #endregion

            #region Insert
            dbCon.Open();
            cmd = new MySqlCommand("INSERT INTO `books` (`id`, `title`, `author`, `publisher`, `ISBN`, `genre`, `description`, `publish_year`, `price`, `on_storage`, `del`) VALUES (NULL, @title, @author, @publisher, @ISBN, @genre, @description, @publish_year, @price, @on_storage, 0)", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@title", title));
            _ = cmd.Parameters.Add(new MySqlParameter("@author", author_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@publisher", publisher_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@ISBN", ISBN));
            _ = cmd.Parameters.Add(new MySqlParameter("@genre", genre_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@description", description));
            _ = cmd.Parameters.Add(new MySqlParameter("@publish_year", publish_year));
            _ = cmd.Parameters.Add(new MySqlParameter("@price", price));
            _ = cmd.Parameters.Add(new MySqlParameter("@on_storage", on_storage));
            _ = await cmd.ExecuteNonQueryAsync();
            #endregion

            dbCon.Close();
        }

        internal static string GetUserName(string token)
        {
            var usr = "";
            try
            {
                var query = "SELECT `usr` FROM `user` WHERE `token`=@token";
                var dbCon = ConnectionToDatabase;
                dbCon.Open();
                var cmd = new MySqlCommand(query, dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@token", token));
                usr = cmd.ExecuteScalar().ToString();
                dbCon.Close();
            }
            catch (Exception)
            {
            }
            return usr;
        }

        internal static void InsertNewBook(BookModel request) => InsertNewBook(request.Title, request.Author, request.Publisher, request.Isbn, request.Genre, request.Description, request.PublishYear, request.Price, request.OnStorage);

        internal static AnswerModel BuyBook(int id, int num = 1)
        {
            try
            {
                var query = "SELECT `on_storage` FROM `books` WHERE `id` = @id";
                var dbCon = ConnectionToDatabase;
                dbCon.Open();
                var cmd = new MySqlCommand(query, dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@id", id));
                var storage = (int)cmd.ExecuteScalar();
                if(storage - num < 0)
                {
                    dbCon.Close();
                    return new AnswerModel() { Message = "Nem áll rendelkezésre ennyi könyv", MessageType = AnswerModel.Types.MessageType.Error };
                }
                query = "UPDATE `books` SET `on_storage` = `on_storage` - @num WHERE `id` = @id";
                cmd = new MySqlCommand(query, dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@id", id));
                _ = cmd.Parameters.Add(new MySqlParameter("@num", num));
                _ = cmd.ExecuteNonQuery();
                dbCon.Close();
                return new AnswerModel() { Message = "Vásárlás sikeres", MessageType = AnswerModel.Types.MessageType.Ok };
            }
            catch (Exception e)
            {
                return new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error };
            }
        }

        internal static async void EditBook(BookModel request)
        {
            var dbCon = ConnectionToDatabase;
            dbCon.Open();//UNSAFE
            #region Író ID-je
            var cmd = new MySqlCommand("SELECT * FROM `author` WHERE `name` LIKE @author", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("author", request.Author));
            var reader = await cmd.ExecuteReaderAsync();
            var author_id = -1;
            while (await reader.ReadAsync())
            {
                author_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            dbCon.Close();
            if (author_id == -1)
                author_id = await InsertNewAuthor(request.Author);
            #endregion

            #region Kiadó ID-je
            dbCon.Open();
            cmd = new MySqlCommand("SELECT * FROM `publisher` WHERE `name` LIKE @publisher", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@publisher", request.Publisher));
            reader = await cmd.ExecuteReaderAsync();
            var publisher_id = -1;
            while (await reader.ReadAsync())
            {
                publisher_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            dbCon.Close();
            if (publisher_id == -1)
                publisher_id = await InsertNewPublisher(request.Publisher);
            #endregion

            #region Műfaj ID-je
            dbCon.Open();
            cmd = new MySqlCommand("SELECT * FROM `genre` WHERE `name` LIKE @genre", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@genre", request.Genre));
            reader = await cmd.ExecuteReaderAsync();
            var genre_id = -1;
            while (await reader.ReadAsync())
            {
                genre_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            dbCon.Close();
            if (genre_id == -1)
                genre_id = await InsertNewGenre(request.Genre);
            #endregion

            #region Update
            dbCon.Open();
            cmd = new MySqlCommand("UPDATE `books` SET `title` = @title,`author` = @author,`publisher` = @publisher,`ISBN` = @ISBN,`genre` = @genre,`description` = @description,`publish_year` = @publish_year,`price` = @price,`on_storage` = @on_storage,`del` = @not_available WHERE `id` = @id", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@title", request.Title));
            _ = cmd.Parameters.Add(new MySqlParameter("@author", author_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@publisher", publisher_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@ISBN", request.Isbn));
            _ = cmd.Parameters.Add(new MySqlParameter("@genre", genre_id));
            _ = cmd.Parameters.Add(new MySqlParameter("@description", request.Description));
            _ = cmd.Parameters.Add(new MySqlParameter("@publish_year", request.PublishYear));
            _ = cmd.Parameters.Add(new MySqlParameter("@price", request.Price));
            _ = cmd.Parameters.Add(new MySqlParameter("@on_storage", request.OnStorage));
            _ = cmd.Parameters.Add(new MySqlParameter("@id", request.Id));
            _ = cmd.Parameters.Add(new MySqlParameter("@not_available", request.NotAvailable ? 1 : 0));
            _ = await cmd.ExecuteNonQueryAsync();
            #endregion

            dbCon.Close();
        }

        internal static void DeleteBook(BookLookupModel request)
        {
            try
            {
                var query = "DELETE FROM `books` WHERE `id` = @id";
                var dbCon = ConnectionToDatabase;
                dbCon.Open();
                var cmd = new MySqlCommand(query, dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@id", request.Id));
                _ = cmd.ExecuteNonQuery();
                dbCon.Close();
            }
            catch (Exception){}
        }

        public static BookModel GetBookAsync(int id)
        {
            var query = "SELECT `books`.`id`, `books`.`title`, `author`.`name`, `publisher`.`name`, `books`.`ISBN`, `genre`.`name`, `books`.`description`, `books`.`publish_year`, `books`.`price`, `books`.`on_storage`, `books`.`del` FROM `books`, `author`, `genre`, `publisher` WHERE `books`.`author`= `author`.`id` AND `books`.`publisher` = `publisher`.`id` AND `books`.`genre` = `genre`.`id` AND `books`.`id` = @id";

            var dbCon = ConnectionToDatabase;
            dbCon.Open(); //UNSAFE
            var cmd = new MySqlCommand(query, dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@id", id));
            var reader = cmd.ExecuteReader();
            var resultList = new List<BookModel>();
            while (reader.Read())
            {
                resultList.Add(new BookModel()
                {
                    Id = int.Parse(reader.GetString(0)),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Publisher = reader.GetString(3),
                    Isbn = reader.GetString(4),
                    Genre = reader.GetString(5),
                    Description = reader.GetString(6),
                    PublishYear = reader.GetString(7),
                    Price = int.Parse(reader.GetString(8)),
                    OnStorage = int.Parse(reader.GetString(9)),
                    NotAvailable = reader.GetString(10) == "1"
                });
            }
            dbCon.Close();
            return resultList.First();
        }

        public static async Task<int> InsertNewGenre(string genre_name)
        {
            var dbCon = ConnectionToDatabase;
            dbCon.Open();
            var cmd = new MySqlCommand("SELECT * FROM `genre` WHERE `name` LIKE @genre", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@genre", genre_name));
            var reader = await cmd.ExecuteReaderAsync();
            var genre_id = -1;
            while (await reader.ReadAsync())
            {
                genre_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            if(genre_id == -1)
            {
                cmd = new MySqlCommand("INSERT INTO `genre`(`id`, `name`) VALUES (NULL, @genre)", dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@genre", genre_name));
                _ = cmd.ExecuteNonQuery();
                genre_id = (int)cmd.LastInsertedId; //UNSAFE
            }
            dbCon.Close();
            return genre_id;
        }

        public static async Task<int> InsertNewPublisher(string publisher_name)
        {
            var dbCon = ConnectionToDatabase;
            dbCon.Open();
            var cmd = new MySqlCommand("SELECT * FROM `publisher` WHERE `name` LIKE @publisher", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@publisher", publisher_name));
            var reader = await cmd.ExecuteReaderAsync();
            var publisher_id = -1;
            while (await reader.ReadAsync())
            {
                publisher_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            if (publisher_id == -1)
            {
                cmd = new MySqlCommand("INSERT INTO `publisher`(`id`, `name`) VALUES (NULL, @publisher)", dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@publisher", publisher_name));
                _ = cmd.ExecuteNonQuery();
                publisher_id = (int)cmd.LastInsertedId; //UNSAFE
            }
            dbCon.Close();
            return publisher_id;
        }

        public static async Task<int> InsertNewAuthor(string author_name)
        {
            var dbCon = new MySqlConnection($"Server={host}; database={db_name}; UID={db_user}; password={db_psw}");
            dbCon.Open();
            var cmd = new MySqlCommand("SELECT * FROM `author` WHERE `name` LIKE @author", dbCon);
            _ = cmd.Parameters.Add(new MySqlParameter("@author", author_name));
            var reader = await cmd.ExecuteReaderAsync();
            var author_id = -1;
            while (await reader.ReadAsync())
            {
                author_id = int.Parse(reader.GetString(0)); //UNSAFE
            }
            reader.Close();
            if (author_id == -1)
            {
                cmd = new MySqlCommand("INSERT INTO `author`(`id`, `name`) VALUES (NULL, @author)", dbCon);
                _ = cmd.Parameters.Add(new MySqlParameter("@author", author_name));
                _ = cmd.ExecuteNonQuery();
                author_id = (int)cmd.LastInsertedId; //UNSAFE
            }
            dbCon.Close();
            return author_id;
        }

        public static async Task<List<BookModel>> GetBooks (string book_title = null)
        {
            var query = "SELECT `books`.`id`, `books`.`title`, `author`.`name`, `publisher`.`name`, `books`.`ISBN`, `genre`.`name`, `books`.`description`, `books`.`publish_year`, `books`.`price`, `books`.`on_storage`, `books`.`del` FROM `books`, `author`, `genre`, `publisher` WHERE `books`.`author`= `author`.`id` AND `books`.`publisher` = `publisher`.`id` AND `books`.`genre` = `genre`.`id`";

            if (!string.IsNullOrWhiteSpace(book_title)) 
                query += " AND `books`.`title` LIKE @title";

            var dbCon = ConnectionToDatabase;
            await dbCon.OpenAsync(); //UNSAFE
            var cmd = new MySqlCommand(query, dbCon);
            if (!string.IsNullOrWhiteSpace(book_title))
                _ = cmd.Parameters.Add(new MySqlParameter("@title", $"{book_title}%"));
            var reader = cmd.ExecuteReader();
            var resultList = new List<BookModel>();
            while (reader.Read())
            {
                resultList.Add(new BookModel() { 
                    Id = int.Parse(reader.GetString(0)), 
                    Title = reader.GetString(1), 
                    Author = reader.GetString(2), 
                    Publisher = reader.GetString(3), 
                    Isbn = reader.GetString(4), 
                    Genre = reader.GetString(5), 
                    Description = reader.GetString(6), 
                    PublishYear = reader.GetString(7), 
                    Price = int.Parse(reader.GetString(8)), 
                    OnStorage = int.Parse(reader.GetString(9)), 
                    NotAvailable = reader.GetString(10) == "1" 
                });
            }
            return resultList;
        }

        private static MySqlConnection ConnectionToDatabase => new MySqlConnection($"Server={host}; database={db_name}; UID={db_user}; password={db_psw}");

        public static bool OK { get {
                try
                {
                    var conDb = ConnectionToDatabase;
                    conDb.Open();
                    var test = conDb.Ping();
                    conDb.Close();
                    return test;
                }
                catch (Exception)
                {
                    return false;
                }
            } }
    }
}
