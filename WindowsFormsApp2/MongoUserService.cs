using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public class MongoUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Movie> _Movie;
    public MongoUserService()
    {
        // 連線到本地MongoDB URI
        var client = new MongoClient("mongodb://localhost:27017"); 
        //連線到MovieTheater的資料庫
        var database = client.GetDatabase("MovieTheater");
        //連線到user的Collection
        _users = database.GetCollection<User>("user");
        //連線到Movie的Collectio
        _Movie = database.GetCollection<Movie>("Movie");
    }

    // 新增使用者文件
    public void InsertUser(User user)
    {
        _users.InsertOne(user);
    }
    // 新增電影文件
    public void InsertMovie(Movie movie)
    {
        _Movie.InsertOne(movie);
    }
    // 查詢使用者的帳號（可用來查是否存在）
    public int FindUsersByUsername(string username)
    {
        //如果有找到使用者的資料就回傳1找不到回傳0
        return _users.Find(u => u.Username == username).ToList().Count;
    }
    // 查詢電影的位置（可用來查是否存在）
    public bool FindMovieSeat(string movie,int seat,DateTime time)
    {
        //定義尋找電影名稱及座位資料的判斷式
        var filter = Builders<Movie>.Filter.Eq("Title",movie)&
                     Builders<Movie>.Filter.Eq("Seat",seat)&
                     Builders<Movie>.Filter.Eq("Time",time);
        //如果有找到符合判斷式的回傳true找不到回傳false
        var getmovie = _Movie.Find(filter).FirstOrDefault();
        if (getmovie != null) { 
            return true;
        }
        return false;
    }
    //利用非同步回傳MongoDB包含使用者帳號的所有資料
    public async Task<List<Movie>> FindMovieTiclet(string username)
    {
        var filter = Builders<Movie>.Filter.Eq("username", username);
        var list = await _Movie.Find(filter).ToListAsync();
        return list;
        
    }
    //利用非同步刪除MongoDB上的資料
    public async Task<bool> DeleteByIdAsync(ObjectId id)
    {
        var filter = Builders<Movie>.Filter.Eq(u => u.Id, id);
        var result = await _Movie.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
    // 驗證帳號密碼
    public ObjectId? ValidateLogin(string username, string password)
    {
        var filter = Builders<User>.Filter.Eq("username", username) &
                     Builders<User>.Filter.Eq("password", password);
        var user= _users.Find(filter).FirstOrDefault();
        if(user != null)
        {
            return user.Id;
        }
        return null;
    }
}