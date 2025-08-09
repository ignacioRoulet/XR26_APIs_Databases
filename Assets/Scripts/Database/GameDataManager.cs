using UnityEngine;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System;

namespace Databases
{
    /// Game Data Manager for handling SQLite database operations
    public class GameDataManager : MonoBehaviour
    {
        [Header("Database Configuration")]
        [SerializeField] private string databaseName = "GameData.db";

        private SQLiteConnection _database;
        private string _databasePath;

        // Singleton pattern for easy access
        public static GameDataManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDatabase();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// DONE: Students will implement this method
        private void InitializeDatabase()
        {
            try
            {
                // DONE: Set up database path using Application.persistentDataPath
                _databasePath = Path.Combine(Application.persistentDataPath, databaseName);

                // DONE: Create SQLite connection
                _database = new SQLiteConnection(_databasePath);

                // DONE: Create tables for game data
                _database.CreateTable<HighScore>();

                Debug.Log($"Database initialized at: {_databasePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize database: {ex.Message}");
            }
        }

        #region High Score Operations

        /// DONE: Students will implement this method
        public void AddHighScore(string playerName, int score, string levelName = "Default")
        {
            try
            {
                // DONE: Create a new HighScore object
                var highScore = new HighScore(playerName, score, levelName);

                // DONE: Insert it into the database using _database.Insert()
                _database.Insert(highScore);

                Debug.Log($"High score added: {playerName} - {score} points");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add high score: {ex.Message}");
            }
        }

        /// DONE: Students will implement this method
        public List<HighScore> GetTopHighScores(int limit = 10)
        {
            try
            {
                // DONE: Query the database for top scores
                return _database.Table<HighScore>()
                    .OrderByDescending(hs => hs.Score)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }

        /// DONE: Students will implement this method
        public List<HighScore> GetHighScoresForLevel(string levelName, int limit = 10)
        {
            try
            {
                // DONE: Query the database for scores filtered by level
                return _database.Table<HighScore>()
                    .Where(hs => hs.LevelName == levelName)
                    .OrderByDescending(hs => hs.Score)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get level high scores: {ex.Message}");
                return new List<HighScore>();
            }
        }

        #endregion

        #region Database Utility Methods

        /// DONE: Students will implement this method
        public int GetHighScoreCount()
        {
            try
            {
                // DONE: Count the total number of high scores
                return _database.Table<HighScore>().Count();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get high score count: {ex.Message}");
                return 0;
            }
        }

        /// DONE: Students will implement this method
        public void ClearAllHighScores()
        {
            try
            {
                // DONE: Delete all high scores from the database
                _database.DeleteAll<HighScore>();

                Debug.Log("All high scores cleared");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to clear high scores: {ex.Message}");
            }
        }

        /// <summary>
        /// Close the database connection when the application quits
        /// </summary>
        private void OnApplicationQuit()
        {
            _database?.Close();
        }

        #endregion
    }
}
