# Auram

Auram is a database! It's easy to use and it is open source!

## How to use

1. Create a C# program
2. Right Click on Dependencies in the Solution Explorer
3. Click Add Project Reference
4. Click Browse at the bottom right, then in the open file dialog, select Auram.dll
5. In your program, add this:

```cs
using Auram;
```

## Commands

```cs
Database database = new Database();
database.Add("KeyGoesHere", "ValueGoesHere"); # Add a value to database
database.Save(path); # Save the database in your computer
database.Clear(); # Clear the database
database.Load(path); # Load the database from the computer
string value = database.Get<string>("KeyGoesHere"); # Return the value of the key given
```

## About Auram
- Compresses the database file
- The database goes into a single file
- Open Source
- Easy to use in C#

## My website (empty)
[NZX](https://nzx.hu/)