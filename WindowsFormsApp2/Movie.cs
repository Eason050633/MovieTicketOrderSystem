using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Windows.Forms;
public class Movie
{
    
    public ObjectId Id { get; set; }
    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("Seat")]
    public int Seat { get; set; }

    [BsonElement("Time")]
    public DateTime Time { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; }


}