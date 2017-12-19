using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using static System.Console;
namespace Campo_Minado
{
    class Program
    {
        public static Cell[,] Celulas;
        static int QuantBombas;
        static int MaxCelulas;
        static Random Aleatorio;
        static int posX;
        static int posY;
        static bool GameOver = false;
        static void Main(string[] args)
        {
            SetWindowSize(79, 30);
            Setup();
            new Thread(tJogo).Start();
          

        }
        static void Setup()
        {
            QuantBombas = 10;
            MaxCelulas = 20;
            Celulas = new Cell[MaxCelulas, MaxCelulas];
            Aleatorio = new Random();
            Title = "Campo Minado by gusdnide";
            posX = 0;
            posY = 0;
            List<int> Bombas = new List<int>();

            for (int i = 0; i < MaxCelulas; i++)
            {
                //Gerar Bombas
                int id = Aleatorio.Next(0, MaxCelulas * MaxCelulas);
                if (!Bombas.Contains(id))
                    Bombas.Add(id);
                //Inicializar construtor
                for (int j = 0; j < MaxCelulas; j++)
                {
                    Celulas[i, j] = new Cell(i, j);
                }
            }

            int count = 0;
            for (int i = 0; i < MaxCelulas; i++)
            {
                for (int j = 0; j < MaxCelulas; j++)
                {
                    if (Bombas.Contains(count))
                        Celulas[i, j].eBomba = true;
                    count++;
                }

            }
            VerificarRedor();
        }
        static void VerificarRedor()
        {

            for (int i = 0; i < MaxCelulas; i++)
            {
                for (int j = 0; j < MaxCelulas; j++)
                {
                    int count = 0;

                    if (!Celulas[i, j].eBomba)
                        for (int x = -1; x <= 1; x++)
                        {
                            if ((i + x) >= 0 && (i + x) < MaxCelulas)
                            {
                                for (int y = -1; y <= 1; y++)
                                {
                                    if ((j + y) >= 0 && (j + y) < MaxCelulas)
                                    {
                                        if (Celulas[i + x, j + y].eBomba)
                                        {
                                            count++;
                                        }
                                    }
                                }
                            }
                        }
                    Celulas[i, j].cRedor = count;
                }
            }
        }
        static void Revelar(int i, int j)
        {
            Celulas[i , j ].Revelar();
            for (int x = -1; x <= 1; x++)
            {
                if ((i + x) >= 0 && (i + x) < MaxCelulas)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if ((j + y) >= 0 && (j + y) < MaxCelulas)
                        {
                            if (!Celulas[i + x, j + y].Revelado && !Celulas[i + x, j + y].eBomba && Celulas[i+x, j+y].cRedor <= 1)
                            {

                                Revelar(i + x, j + y);
                            }else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }
        static void tJogo()
        {
            while (!GameOver)
            {
                
                for (int i = 0; i < MaxCelulas; i++)
                {
                    for (int j = 0; j < MaxCelulas; j++)
                    {
                        if (posY == j && posX == i)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.Gray;
                        Celulas[i, j].Desenhar();
                    }
                }
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (posX > 0)
                            posX--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (posX < MaxCelulas)
                            posX++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (posY > 0)
                            posY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (posY < MaxCelulas)
                            posY++;
                        break;
                    case ConsoleKey.Enter:
                        Revelar(posX, posY); 
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
    class Vec2
    {
        public int x { get; set; }
        public int y { get; set; }
        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x + v2.x, v2.y + v1.y);
        }
        public static bool operator !=(Vec2 v1, Vec2 v2)
        {
            return !(v1.x == v2.x && v1.y == v2.y);
        }

        public static bool operator ==(Vec2 v1, Vec2 v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }
        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v2.y - v1.y);
        }
    }
    class Cell
    {
        public Vec2 Posicao { get; set; }
        public bool eBomba { get; set; }
        public bool Revelado { get; set; }
        public int cRedor { get; set; }
        public Cell(int x, int y)
        {
            Posicao = new Vec2(x, y);
            eBomba = false;
            Revelado = false;
            cRedor = 0;
        }
        public void Revelar()
        {
            this.Revelado = true;
        }
        public void Desenhar()
        {
            SetCursorPosition(Posicao.x, Posicao.y);
            if (Revelado)
            {
                if (eBomba)
                {
                    Console.Write("@");
                }
                else
                {
                    if (cRedor > 0)
                    {
                        Console.Write(cRedor);
                    }
                    else
                    {
                        Console.Write("░");
                    }
                }
            }
            else
            {
                Console.Write("█");
            }
        }


    }
}
