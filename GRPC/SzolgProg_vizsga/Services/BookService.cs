using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SzolgProg_vizsga
{
    public class BookService : Book.BookBase
    {
        public BookService(ILogger<BookService> logger) => Logger = logger;

        private ILogger<BookService> Logger { get; }

        public static Dictionary<string, string> Sessions { get; private set; } = new Dictionary<string, string>();

        public override async Task<BookModel> GetBooksById(BookLookupModel request, ServerCallContext context)
        {
            try
            {
                return request is null
                                   ? await Task.FromResult(new BookModel())
                                   : await Task.FromResult(Database.GetBookAsync(request.Id));
            }
            catch (Exception)
            {
                return await Task.FromResult(new BookModel());
            }
        }

        public override async Task GetBooksByTitle(BookSearchModel request, IServerStreamWriter<BookModel> responseStream, ServerCallContext context)
        {
            try
            {
                var resultList = await Database.GetBooks(request.Name ?? "");
                foreach (var item in resultList)
                {
                    await responseStream.WriteAsync(item);
                }
            }
            catch (Exception)
            {
            }
        }

        public override async Task<AnswerModel> BuyBook(BookLookupModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                if (!Sessions.ContainsKey(request.UserToken))
                    throw new Exception("Felhasználó nincs bejelentkezve");
                if (Database.GetBookAsync(request.Id) is null)
                    throw new Exception("Nem létezik ilyen könyv");
                return await Task.FromResult(Database.BuyBook(request.Id, request.Number));
            }
            catch (Exception e)
            {
                return await Task.FromResult(new AnswerModel() { Message = $"{e.Message}", MessageType=AnswerModel.Types.MessageType.Error });
            }
        }

        public override async Task<AnswerModel> Login(UserModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                var usr = Database.GetUserName(request.Token);
                if (string.IsNullOrWhiteSpace(usr))
                    throw new Exception("Ismeretlen Token");
                lock (Sessions)
                {
                    if(!Sessions.ContainsKey(request.Token))
                        Sessions.Add(request.Token, usr); 
                }
                return await Task.FromResult(new AnswerModel() { Message = $"Bejelentkezve mint {usr}", MessageType = AnswerModel.Types.MessageType.Ok });
            }
            catch (Exception e)
            {
                return await Task.FromResult(new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error });
            }
        }

        public override async Task<AnswerModel> Logout(UserModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                var loggedIn = false;
                lock (Sessions)
                {
                    loggedIn = Sessions.ContainsKey(request.Token); 
                }
                if (!loggedIn)
                    throw new Exception("Felhasználó nincs bejelentkezve");
                lock (Sessions)
                {
                    _ = Sessions.Remove(request.Token); 
                }
                return await Task.FromResult(new AnswerModel() { Message = $"Felhasználó kijelentkeztetve", MessageType = AnswerModel.Types.MessageType.Ok });
            }
            catch (Exception e) { return await Task.FromResult(new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error }); }
        }

        public override async Task<AnswerModel> NewBook(BookModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                var loggedIn = false;
                lock (Sessions)
                {
                    loggedIn = Sessions.ContainsKey(request.UserToken); 
                }
                if (!loggedIn)
                    throw new Exception("Felhasználó nincs bejelentkezve");
                Database.InsertNewBook(request);
                return await Task.FromResult(new AnswerModel() { Message = "Új könyv felvéve", MessageType = AnswerModel.Types.MessageType.Ok });
            }
            catch (Exception e) { return await Task.FromResult(new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error }); }
        }

        public override async Task<AnswerModel> EditBook(BookModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                var loggedIn = false;
                lock (Sessions)
                {
                    loggedIn = Sessions.ContainsKey(request.UserToken);
                }
                if (!loggedIn)
                    throw new Exception("Felhasználó nincs bejelentkezve");
                Database.EditBook(request);
                return await Task.FromResult(new AnswerModel() { Message = "Könyv módosítva", MessageType = AnswerModel.Types.MessageType.Ok });
            }
            catch (Exception e) { return await Task.FromResult(new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error }); }
        }

        public override async Task<AnswerModel> DeleteBook(BookLookupModel request, ServerCallContext context)
        {
            try
            {
                if (request is null)
                    throw new Exception("Request null érték");
                var loggedIn = false;
                lock (Sessions)
                {
                    loggedIn = Sessions.ContainsKey(request.UserToken);
                }
                if (!loggedIn)
                    throw new Exception("Felhasználó nincs bejelentkezve");
                Database.DeleteBook(request);
                return await Task.FromResult(new AnswerModel() { Message = "Könyv törölve", MessageType = AnswerModel.Types.MessageType.Ok });
            }
            catch (Exception e) { return await Task.FromResult(new AnswerModel() { Message = e.Message, MessageType = AnswerModel.Types.MessageType.Error }); }
        }
    }
}
