using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;

public class MongoUserService
{
    private readonly IMongoCollection<User> _users;

    public MongoUserService()
    {
        var client = new MongoClient("mongodb://localhost:27017"); // 改成你自己的 MongoDB URI
        var database = client.GetDatabase("MovieTheater");
        _users = database.GetCollection<User>("user");
    }

    // 新增使用者文件
    public void InsertUser(User user)
    {
        _users.InsertOne(user);
    }

    // 查詢使用者（可用來查是否存在）
    public int FindUsersByUsername(string username)
    {
        return _users.Find(u => u.Username == username).ToList().Count;
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