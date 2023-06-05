using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class SnakeGame
{
    private static int width = 30;
    private static int height = 20;
    private static int score = 0;
    private static int delay = 200;
    private static bool gameOver = false;
    private static Direction direction = Direction.Right;
    private static Random random = new Random();
    private static List<int> snakeX = new List<int>();
    private static List<int> snakeY = new List<int>();
    private static int fruitX;
    private static int fruitY;

    static void Main(string[] args)
    {
        Console.Title = "Snake Game";
        Console.CursorVisible = false;
        Console.SetWindowSize(width + 1, height + 2);
        Console.SetBufferSize(width + 1, height + 2);

        InitializeGame();

        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                HandleKeyPress(Console.ReadKey(true).Key);
            }

            MoveSnake();
            CheckCollision();
            DrawGame();

            Thread.Sleep(delay);
        }

        Console.SetCursorPosition(width / 2 - 4, height / 2);
        Console.WriteLine("Game Over!");
        Console.SetCursorPosition(width / 2 - 6, height / 2 + 1);
        Console.WriteLine("Score: " + score);
        Console.ReadKey();
    }

    static void InitializeGame()
    {
        snakeX.Clear();
        snakeY.Clear();
        snakeX.Add(width / 2);
        snakeY.Add(height / 2);
        score = 0;
        delay = 200;
        gameOver = false;

        PlaceFruit();
    }

    static void PlaceFruit()
    {
        fruitX = random.Next(0, width);
        fruitY = random.Next(0, height);
    }

    static void HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                if (direction != Direction.Right)
                    direction = Direction.Left;
                break;
            case ConsoleKey.RightArrow:
                if (direction != Direction.Left)
                    direction = Direction.Right;
                break;
            case ConsoleKey.UpArrow:
                if (direction != Direction.Down)
                    direction = Direction.Up;
                break;
            case ConsoleKey.DownArrow:
                if (direction != Direction.Up)
                    direction = Direction.Down;
                break;
        }
    }

    static void MoveSnake()
    {
        int snakeHeadX = snakeX.First();
        int snakeHeadY = snakeY.First();

        switch (direction)
        {
            case Direction.Left:
                snakeHeadX--;
                break;
            case Direction.Right:
                snakeHeadX++;
                break;
            case Direction.Up:
                snakeHeadY--;
                break;
            case Direction.Down:
                snakeHeadY++;
                break;
        }

        snakeX.Insert(0, snakeHeadX);
        snakeY.Insert(0, snakeHeadY);

        if (snakeHeadX == fruitX && snakeHeadY == fruitY)
        {
            score++;
            PlaceFruit();
            if (delay > 10)
                delay -= 10;
        }
        else
        {
            snakeX.RemoveAt(snakeX.Count - 1);
            snakeY.RemoveAt(snakeY.Count - 1);
        }
    }

    static void CheckCollision()
    {
        int snakeHeadX = snakeX.First();
        int snakeHeadY = snakeY.First();

        if (snakeHeadX < 0 || snakeHeadX >= width || snakeHeadY < 0 || snakeHeadY >= height)
        {
            gameOver = true;
        }

        if (snakeX.Skip(1).Contains(snakeHeadX) && snakeY.Skip(1).Contains(snakeHeadY))
        {
            gameOver = true;
        }
    }

    static void DrawGame()
    {
        Console.Clear();

        // Draw fruit
        Console.SetCursorPosition(fruitX, fruitY);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("O");

        // Draw snake
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < snakeX.Count; i++)
        {
            Console.SetCursorPosition(snakeX[i], snakeY[i]);
            Console.Write(i == 0 ? "@" : "*");
        }

        // Draw score
        Console.SetCursorPosition(0, height + 1);
        Console.WriteLine("Score: " + score);
    }

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
