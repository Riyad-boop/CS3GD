using System;
using System.Collections.Generic;

public class User {
    public string username;
    public float score;

    public User(string username, float score) {
        this.username = username;
        this.score = score;
    }
}

public class UserScoreComparer : IComparer<User>
{
    public int Compare(User x, User y)
    {
        return y.score.CompareTo(x.score);
    }
}
