using MyApp.utils;
using System;
using System.Collections.Generic;

namespace MyApp.board
{
    class BoardApp : IApp
    {
        private const bool WINNER = true;

        Board _board;
        Cursor _cursor;
        Turn _turn;

        private void init()
        {
            _board = new Board();
            _cursor = new Cursor();
            _turn = new Turn();

            _board.paint();
            _cursor.paint();
        }

        public void run()
        {
            Console.CursorVisible = false;
            init();

            ConsoleKeyInfo input;
            do{
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.UpArrow)
                {
                    _cursor.moveUp();
                }
                else if (input.Key == ConsoleKey.RightArrow)
                {
                    _cursor.moveRight();
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    _cursor.moveDown();
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    _cursor.moveLeft();
                }
                else if (input.Key == ConsoleKey.Enter)
                {
                    if (_board.set(_cursor.Location, _turn) == WINNER)
                    {
                        Screen.pause();
                    }                    
                }
                else if (input.Key == ConsoleKey.R)
                {
                    init();
                }
                _board.paint();
            } while (input.Key != ConsoleKey.Escape);
        }
    }

    class Screen
    {
        public static void put(int x, int y, char c)
        {
            if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
            {
                return;
            }
            Console.SetCursorPosition(x, y);
            Console.Write(c);
        }
        public static void put(Point p, char c)
        {
            put(p.X, p.Y, c);
        }

        public static int translate(int i)
        {
            return (i * 2) + 3;
        }
        public static Point translate(int x, int y)
        {
            return new Point((x * 2) + 3, (y * 2) + 3);
        }
        public static Point translate(Point p)
        {
            return translate(p.X, p.Y);
        }

        internal static void pause()
        {
            Console.ReadKey(true);
        }
    }

    class Board
    {
        private Dictionary<int, Dictionary<int, char>> _grid;

        public Board(int GRID_SIZE)
        {
            init(GRID_SIZE);
        }
        public Board()
        {
            init(3);
        }

        private void init(int GRID_SIZE)
        {
            _grid = new Dictionary<int, Dictionary<int, char>>();
            for (var y = 0; y < GRID_SIZE; y++)
            {
                _grid.Add(y, new Dictionary<int, char>());
                for (var x = 0; x < GRID_SIZE; x++)
                {
                    _grid[y].Add(x, '-');
                }
            }
        }

        public char get(int x, int y)
        {
            return _grid[x][y];
        }
        public char get(Point p)
        {
            return get(p.X, p.Y);
        }
        public bool set(int x, int y, Turn turn)
        {
            if (_grid[x][y] == '-')
            {
                _grid[x][y] = turn.Player;
            }
            return playerHasWon();
        }
        public bool set(Point p, Turn turn)
        {
            return set(p.X, p.Y, turn);
        }

        public bool playerHasWon()
        {
            var possibleSolutions = new List<List<Point>>() {
                new List<Point>(){ new Point(0, 0), new Point(0, 1), new Point(0, 2) },
                new List<Point>(){ new Point(1, 0), new Point(1, 1), new Point(1, 2) },
                new List<Point>(){ new Point(2, 0), new Point(2, 1), new Point(2, 2) },
                new List<Point>(){ new Point(0, 0), new Point(1, 0), new Point(2, 0) },
                new List<Point>(){ new Point(0, 1), new Point(1, 1), new Point(2, 1) },
                new List<Point>(){ new Point(0, 2), new Point(1, 2), new Point(2, 2) },
                new List<Point>(){ new Point(0, 0), new Point(1, 1), new Point(2, 2) },
                new List<Point>(){ new Point(0, 2), new Point(1, 1), new Point(2, 0) }
            };
            foreach (List<Point> points in possibleSolutions)
            {
                string marks = "";
                foreach (Point p in points)
                {
                    marks += this.get(p);
                }
                if (marks.Length == 3 && marks[0] != '-' && marks[0] == marks[1] && marks[1] == marks[2])
                {
                    markWinner(marks[0]);
                    return true;
                }
            }
            return false;
        }
        public void markWinner(char c)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (x == 1 && y == 1)
                    {
                        Screen.put(Screen.translate(x, y), c);
                    }
                    else
                    {
                        Screen.put(Screen.translate(x, y), ' ');
                    }
                }
            }
        }

        public void paint()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Screen.put(Screen.translate(x, y), this.get(x, y));
                }
            }
        }
    }

    class Turn
    {
        private char _player;
        public char Player
        {
            get
            {
                if (_player == 'X')
                {
                    _player = 'O';
                }
                else
                {
                    _player = 'X';
                }
                return _player;
            }
        }
    }

    class Point
    {
        private int _x;
        public int X { get { return _x; } set { _x = value; } }
        private int _y;
        public int Y { get { return _y; } set { _y = value; } }
        public Point()
        {
            _x = 0;
            _y = 0;
        }
        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public bool isAt(int x, int y)
        {
            return _x == x && _y == y;
        }
    }

    class Cursor
    {
        private int MAX;
        private Point _location;
        public Point Location
        {
            get
            {
                if (_location.Y > MAX)
                {
                    _location.Y = 0;
                }
                else if (_location.Y < 0)
                {
                    _location.Y = MAX - 1;
                }
                if (_location.X > MAX)
                {
                    _location.X = 0;
                }
                else if (_location.X < 0)
                {
                    _location.X = MAX - 1;
                }
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public Cursor() : base()
        {
            this.MAX = 3;
            this.Location = new Point();
        }
        public Cursor(int max) : base()
        {
            this.MAX = max;
            this.Location = new Point();
        }

        public void moveUp()
        {
            erase();
            this.Location.Y--;
            paint();
        }
        public void moveDown()
        {
            erase();
            this.Location.Y++;
            paint();
        }
        public void moveLeft()
        {
            erase();
            this.Location.X--;
            paint();
        }
        public void moveRight()
        {
            erase();
            this.Location.X++;
            paint();
        }

        public void paint()
        {
            Point p = Screen.translate(this.Location);
            Screen.put(p.X - 1, p.Y, '[');
            Screen.put(p.X + 1, p.Y, ']');
        }
        public void erase()
        {
            Point p = Screen.translate(this.Location);
            Screen.put(p.X - 1, p.Y, ' ');
            Screen.put(p.X + 1, p.Y, ' ');
        }
    }
}
