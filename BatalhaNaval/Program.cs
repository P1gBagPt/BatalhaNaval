using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BatalhaNaval
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             (20)
             -------------------------------
             -----Tabuleiro do jogo de dimensão de 10 * 10
             -----Barcos a ocupar apenas uma posição (apenas de 1 dimensão)
             -----Posicionamento dos barcos (8 por jogador)de cada um dos jogadores
             -----Versão “Jogador Vs Jogador”
             -----Verificação se acertou no barco ou se foi “água”
             -----Verificação se alguém ganhou o jogo
             -----Apenas  permitir  os  jogadores  jogarem  dentro  do  tabuleiro  e  numa  posição  que  ainda não tenha sido jogada
             -----Gravação  e  apresentação  dos highscoresgravados  num  ficheiro
             (a  ordenação  dos resultados é efetuado pelo números mínimo de jogadas para vencer)
            ----------------------------------
            Modo de jogo(2 valores):
            -----Fácil (tabuleiro 10 * 10)
            -----Médio (tabuleiro 15 * 15)
            -----Difícil (tabuleiro 20 * 20)
            ----------------------------------
            Barcos de várias dimensões
            (4valores)
            -----Submarino (1 posição) -+20 | 15 cada posição
            -----Corvetas (2 posições) -+30 | (1 barco) | 15 cada posição
            -----Fragatas(3 posições) -+45 | 15 cada posição
            -----Porta-aviões (4 posições) -+80 | 0.25 * 4  (1 barco)  20 cada posição
            ----------------------------------
            (4valores)
            -----Versão “Jogador vs Computador"
            ----------------------------------*/
            /*
             Perguntas
            -----Mostrar os tabuleiros, lado a lado ou viz do jogador 1 aparece o tabuleiro e depois quando for o jogador 2 aparece só o dele ou usar clears
            -----Gravação dos highscoresgravados, como é, se os barcos tem pontos ou nao.
            -----Porta avioes tem 4 posições ou seja 1 barco, se acertar na posição 2 do barco conta como -1barco ou não?
            -----Verificação de quem ganhou é por pontos ou por total de barcos?
            
             */

            //Selecionar o Modo de Jogo FACIL/MEDIO/DIFICIL

            //Menu para as instruções do jogo
            int instrucoes = 0;
            bool parseSuccess = false;
            do
            {
                mostrarMenuInstrucoes();
                try
                {
                    instrucoes = int.Parse(Console.ReadLine());
                    parseSuccess = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Por favor, insira um número válido.");
                    parseSuccess = false;
                }
            } while (instrucoes < 1 || instrucoes > 2 || !parseSuccess);

            if (instrucoes == 1)
            {
                //Função para mostrar as instruções
                mostrarInstrucoes();
            }

            //Selecionar o modo de Jogo (Fácil|Médio|Difícil)
            int modoJogo = 0;
            bool inputValido = false;
            while (!inputValido)
            {
                try
                {
                    mostrarModosTabuleiros();
                    modoJogo = int.Parse(Console.ReadLine());
                    if (modoJogo < 1 || modoJogo > 3)
                    {
                        throw new Exception("Insira um numero entre 1 e 3.");
                    }
                    inputValido = true;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }

            //Numero de posições na matriz (10*10|15*15|20*20)
            int celulas = 0;
            //Criar os tipos de barcos
            int portaAvioes = 0, fragatas = 0, corvetas = 0, submarinos = 0;
            //Inicializar os barcos dos jogadores
            int barcosJogador1 = 0;
            int barcosJogador2 = 0;
            //Inicializar a dificuldade
            string dificuldade = "";
            //Dependendo de cada tipo de jogo os barcos sao diferentes
            switch (modoJogo)
            {
                //10*10
                case 1:
                    celulas = 10;
                    //4 * 2 = 8
                    //4 * 1 = 4 + 8 = 12
                    //Porta avioes = 0
                    //Fragatas = 0
                    corvetas = 4;
                    submarinos = 4;
                    barcosJogador1 = 12;
                    barcosJogador2 = 12;
                    dificuldade = "Fácil";
                    //12
                    break;
                case 2:
                    celulas = 15;
                    //Porta avioes = 0
                    //3 * 3 = 9
                    //4 * 2 = 8
                    //1 + 9 + 8 = 18
                    fragatas = 3;
                    corvetas = 4;
                    submarinos = 1;
                    barcosJogador1 = 18;
                    barcosJogador2 = 18;
                    dificuldade = "Média";
                    //18
                    break;
                case 3:
                    celulas = 20;
                    //3 * 4 = 12
                    //2 * 3 = 6
                    //2 * 2 = 4
                    //1 + 12 + 6 + 4 = 23
                    portaAvioes = 3;
                    fragatas = 2;
                    corvetas = 2;
                    submarinos = 1;
                    barcosJogador1 = 23;
                    barcosJogador2 = 23;
                    dificuldade = "Difícil";
                    //23
                    break;
            }

            //Selecionar o Modo de Jogo (Jogador vs Jogador) ou (Jogador vs Computador)
            int modoJogar;
            do
            {
                Console.Clear();
                if (modoJogo == 1)
                {
                    Console.Write("Modo - ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Fácil ");
                    Console.ResetColor();
                    Console.WriteLine("(tabuleiro 10 * 10)\n");
                }
                else if (modoJogo == 2)
                {
                    Console.Write("Modo - ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Médio ");
                    Console.ResetColor();
                    Console.WriteLine("(tabuleiro 15 * 15)\n");
                }
                else if (modoJogo == 3)
                {
                    Console.Write("Modo - ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Difícil ");
                    Console.ResetColor();
                    Console.WriteLine("(tabuleiro 20 * 20)\n");
                }
                mostrarModosJogos();

                modoJogar = int.Parse(Console.ReadLine());
            } while (modoJogar < 1 || modoJogar > 2);
            Console.Clear();

            //Pedir os nome(s) ao(s) jogadore(s)
            string nomeJogador2 = "";

            Console.WriteLine("Nome do Jogador 1: ");
            string nomeJogador1 = Console.ReadLine();

            if (modoJogar == 1)
            {
                Console.Clear();
                Console.WriteLine("Nome do Jogador 2: ");
                nomeJogador2 = Console.ReadLine();
            }

            //Ficheiro Scores
            FileInfo ficheiroScores = new FileInfo(@"c:\ficheiros\BatalhaNaval\Scores.txt");
            StreamWriter escritaScores;
            //Se não existir cria um ficheiro Scores.txt senão apenas anexa o vencedor um ficheiro já existente
            if (!ficheiroScores.Exists)
            {
                escritaScores = ficheiroScores.CreateText();
            }
            else
            {
                escritaScores = ficheiroScores.AppendText();
            }

            //Criar ou substituir o ficheiro do histórico de Jogadas
            FileInfo ficheiroJogadas = new FileInfo(@"c:\ficheiros\BatalhaNaval\Jogadas.txt");
            StreamWriter escritaJogadas = ficheiroJogadas.CreateText();
            //Historico de Jogadas

            // Tabuleiros dos 2 Jogadores
            char[,] tabuleiroJogador1 = new char[celulas, celulas];
            char[,] tabuleiroJogador2 = new char[celulas, celulas];

            // Colocar os barcos aleatoriamente nos 2 tabuleiros
            Random rnd = new Random();
            //Contar as jogadas de cada jogador
            int jogadas1 = 0, jogadas2 = 0;
            //Pontos de cada jogador
            int pontos1 = 0, pontos2 = 0;

            int colocarBarcos1 = 0, colocarBarcos2 = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("Jogador 1 deseja introduzir os barcos ou introduzir aleatoriamente?");
                Console.WriteLine("1- Introduzir barcos");
                Console.WriteLine("2- Aleatorio");
                Console.Write("Opção: ");
                colocarBarcos1 = int.Parse(Console.ReadLine());
            } while (colocarBarcos1 < 1 || colocarBarcos1 > 2);

            if (modoJogar == 1)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Jogador 2 deseja introduzir os barcos ou introduzir aleatoriamente?");
                    Console.WriteLine("1- Introduzir barcos");
                    Console.WriteLine("2- Aleatorio");
                    Console.Write("Opção: ");
                    colocarBarcos2 = int.Parse(Console.ReadLine());
                } while (colocarBarcos2 < 1 || colocarBarcos2 > 2);
            }


            if (colocarBarcos1 == 2)
            {
                //-----Posicionar BARCOS TABULEIRO 1-----

                for (int i = 0; i < portaAvioes; i++)
                {
                    //Gerar uma linha e uma coluna
                    int row = rnd.Next(0, celulas - 4);
                    int col = rnd.Next(0, celulas - 1);
                    bool emptySpot = false;

                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador1[row, col] != '\0' || tabuleiroJogador1[row + 1, col] != '\0' || tabuleiroJogador1[row + 2, col] != '\0' || tabuleiroJogador1[row + 3, col] != '\0')
                        {
                            emptySpot = false;
                            //Linha
                            row = rnd.Next(0, celulas - 4);
                            col = rnd.Next(0, celulas - 1);
                        }
                    }

                    tabuleiroJogador1[row, col] = 'P';
                    tabuleiroJogador1[row + 1, col] = 'P';
                    tabuleiroJogador1[row + 2, col] = 'P';
                    tabuleiroJogador1[row + 3, col] = 'P';
                }

                //-Fragatas(3 posições)
                for (int i = 0; i < fragatas; i++)
                {
                    int row = rnd.Next(0, celulas - 1);
                    int col = rnd.Next(0, celulas - 3);

                    bool emptySpot = false;
                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador1[row, col] != '\0' || tabuleiroJogador1[row, col + 1] != '\0' || tabuleiroJogador1[row, col + 2] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas - 1);
                            col = rnd.Next(0, celulas - 3);
                        }
                    }

                    tabuleiroJogador1[row, col] = 'F';
                    tabuleiroJogador1[row, col + 1] = 'F';
                    tabuleiroJogador1[row, col + 2] = 'F';

                }

                //-Corvetas(2 posições)
                for (int i = 0; i < corvetas; i++)
                {
                    int row = rnd.Next(0, celulas - 2);
                    int col = rnd.Next(0, celulas - 1);

                    bool emptySpot = false;
                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador1[row, col] != '\0' || tabuleiroJogador1[row + 1, col] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas - 2);
                            col = rnd.Next(0, celulas - 1);
                        }
                    }

                    tabuleiroJogador1[row, col] = 'C';
                    tabuleiroJogador1[row + 1, col] = 'C';
                }

                //-Submarino (1 posição)
                for (int i = 0; i < submarinos; i++)
                {
                    int row = rnd.Next(0, celulas);
                    int col = rnd.Next(0, celulas);
                    bool emptySpot = false;

                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador1[row, col] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas);
                            col = rnd.Next(0, celulas);
                        }
                    }

                    tabuleiroJogador1[row, col] = 'S';
                }


                //-----Posicionar BARCOS TABULEIRO 1-----
            }
            else if (colocarBarcos1 == 1)
            {
                int Linha = 0, Col = 0;
                string letter;

                for (int i = 0; i < portaAvioes; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {

                        MostrarTabuleiro1(tabuleiroJogador1, celulas);
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 1 introduza o porta aviões numero {i + 1} |");
                                Console.WriteLine($"Linha do porta aviões: (1 e {celulas})");

                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do porta aviões: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }

                                Col = (int)letter[0] - 65;
                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha + 3 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador1[Linha, Col] != '\0' || tabuleiroJogador1[Linha + 1, Col] != '\0' || tabuleiroJogador1[Linha + 2, Col] != '\0' || tabuleiroJogador1[Linha + 3, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro1(tabuleiroJogador1, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();

                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 1 introduza o porta aviões numero {i + 1}|");
                                        Console.WriteLine($"Linha do porta aviões: (1 e {celulas})");
                                        Linha = int.Parse(Console.ReadLine()) - 1;

                                        Console.WriteLine($"Coluna do porta aviões: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }


                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador1[Linha, Col] = 'P';
                        tabuleiroJogador1[Linha + 1, Col] = 'P';
                        tabuleiroJogador1[Linha + 2, Col] = 'P';
                        tabuleiroJogador1[Linha + 3, Col] = 'P';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < fragatas; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro1(tabuleiroJogador1, celulas);
              
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 1 introduza a fragata numero {i + 1}|");
                                Console.WriteLine($"Linha do fragata: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do fragata: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha + 2 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador1[Linha, Col] != '\0' || tabuleiroJogador1[Linha + 1, Col] != '\0' || tabuleiroJogador1[Linha + 2, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro1(tabuleiroJogador1, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();
                                
                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 1 introduza a fragata numero {i + 1}|");
                                        Console.WriteLine($"Linha do fragatas: (1 e {celulas})");

                                        Linha = int.Parse(Console.ReadLine()) - 1;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }
                                Console.WriteLine($"Coluna do fragatas: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador1[Linha, Col] = 'F';
                        tabuleiroJogador1[Linha + 1, Col] = 'F';
                        tabuleiroJogador1[Linha + 2, Col] = 'F';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < corvetas; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro1(tabuleiroJogador1, celulas);
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 1 introduza a corveta numero {i + 1}|");
                                Console.WriteLine($"Linha da corveta: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do corvetas: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha + 1 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador1[Linha, Col] != '\0' || tabuleiroJogador1[Linha + 1, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro1(tabuleiroJogador1, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();
                               
                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 1 introduza a corveta numero {i + 1}|");
                                        Console.WriteLine($"Linha da corveta: (1 e {celulas})");

                                        Linha = int.Parse(Console.ReadLine()) - 1;

                                        Console.WriteLine($"Coluna do corveta: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }


                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador1[Linha, Col] = 'C';
                        tabuleiroJogador1[Linha + 1, Col] = 'C';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < submarinos; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro1(tabuleiroJogador1, celulas);
                       
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 1 introduza o submarino numero {i + 1}|");
                                Console.WriteLine($"Linha do submarino: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do submarino: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador1[Linha, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro1(tabuleiroJogador1, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();

                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 1 introduza o submarino numero {i + 1}|");
                                        Console.WriteLine($"Linha do submarino: (1 e {celulas})");
                                        Linha = int.Parse(Console.ReadLine()) - 1;
                                        Console.WriteLine($"Coluna do submarino: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }
                                

                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador1[Linha, Col] = 'S';
                        inputValid = true;

                    }
                }

            }



            if (modoJogar == 2)
            {
                PreencherRandom(tabuleiroJogador2, celulas, portaAvioes, fragatas, corvetas, submarinos);
            }
            if (colocarBarcos2 == 2)
            {
                //-----Posicionar BARCOS TABULEIRO 2-----
                //-Porta - aviões(4 posições)
                for (int i = 0; i < portaAvioes; i++)
                {
                    int row = rnd.Next(0, celulas - 4);
                    int col = rnd.Next(0, celulas - 1);
                    bool emptySpot = false;

                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row + 1, col] != '\0' || tabuleiroJogador2[row + 2, col] != '\0' || tabuleiroJogador2[row + 3, col] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas - 4);
                            col = rnd.Next(0, celulas - 1);
                        }
                    }

                    tabuleiroJogador2[row, col] = 'P';
                    tabuleiroJogador2[row + 1, col] = 'P';
                    tabuleiroJogador2[row + 2, col] = 'P';
                    tabuleiroJogador2[row + 3, col] = 'P';
                }

                //-Fragatas(3 posições)
                for (int i = 0; i < fragatas; i++)
                {
                    int row = rnd.Next(0, celulas - 1);
                    int col = rnd.Next(0, celulas - 3);

                    bool emptySpot = false;
                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row, col + 1] != '\0' || tabuleiroJogador2[row, col + 2] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas - 1);
                            col = rnd.Next(0, celulas - 3);
                        }
                    }

                    tabuleiroJogador2[row, col] = 'F';
                    tabuleiroJogador2[row, col + 1] = 'F';
                    tabuleiroJogador2[row, col + 2] = 'F';

                }

                //-Corvetas(2 posições)
                for (int i = 0; i < corvetas; i++)
                {
                    int row = rnd.Next(0, celulas - 2);
                    int col = rnd.Next(0, celulas - 1);

                    bool emptySpot = false;
                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row + 1, col] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas - 2);
                            col = rnd.Next(0, celulas - 1);
                        }
                    }

                    tabuleiroJogador2[row, col] = 'C';
                    tabuleiroJogador2[row + 1, col] = 'C';
                }

                //-Submarino (1 posição)
                for (int i = 0; i < submarinos; i++)
                {
                    int row = rnd.Next(0, celulas);
                    int col = rnd.Next(0, celulas);
                    bool emptySpot = false;

                    while (!emptySpot)
                    {
                        emptySpot = true;

                        if (tabuleiroJogador2[row, col] != '\0')
                        {
                            emptySpot = false;
                            row = rnd.Next(0, celulas);
                            col = rnd.Next(0, celulas);
                        }
                    }

                    tabuleiroJogador2[row, col] = 'S';
                }
                //-----Posicionar BARCOS TABULEIRO 2-----
            }
            else if (colocarBarcos2 == 1)
            {
                int Linha = 0, Col = 0;
                string letter;

                for (int i = 0; i < portaAvioes; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro2(tabuleiroJogador2, celulas);

                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 2 introduza o porta aviões numero {i + 1}|");
                                Console.WriteLine($"Linha do porta aviões: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do porta aviões: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }
                        
                        if (Linha + 3 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador2[Linha, Col] != '\0' || tabuleiroJogador2[Linha + 1, Col] != '\0' || tabuleiroJogador2[Linha + 2, Col] != '\0' || tabuleiroJogador2[Linha + 3, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro2(tabuleiroJogador2, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();
                                
                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 2 introduza o porta aviões numero {i + 1}|");
                                        Console.WriteLine($"Linha do porta aviões: (1 e {celulas})");
                                        Linha = int.Parse(Console.ReadLine()) - 1;
                                        Console.WriteLine($"Coluna do porta aviões: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;
                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }

                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador2[Linha, Col] = 'P';
                        tabuleiroJogador2[Linha + 1, Col] = 'P';
                        tabuleiroJogador2[Linha + 2, Col] = 'P';
                        tabuleiroJogador2[Linha + 3, Col] = 'P';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < fragatas; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro2(tabuleiroJogador2, celulas);

                       
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 2 introduza a fragata numero {i + 1}|");
                                Console.WriteLine($"Linha da fragata: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna da fragata: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha + 2 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador2[Linha, Col] != '\0' || tabuleiroJogador2[Linha + 1, Col] != '\0' || tabuleiroJogador2[Linha + 2, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro2(tabuleiroJogador2, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();

                                try
                                {

                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 2 introduza a fragata numero {i + 1}|");
                                        Console.WriteLine($"Linha da fragata: (1 e {celulas})");
                                        Linha = int.Parse(Console.ReadLine()) - 1;
                                        Console.WriteLine($"Coluna da fragata: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }


                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador2[Linha, Col] = 'F';
                        tabuleiroJogador2[Linha + 1, Col] = 'F';
                        tabuleiroJogador2[Linha + 2, Col] = 'F';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < corvetas; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro2(tabuleiroJogador2, celulas);
                      
                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 2 introduza a corveta numero {i + 1}|");
                                Console.WriteLine($"Linha da corveta: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do corvetas: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }
                       

                        if (Linha + 1 >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador2[Linha, Col] != '\0' || tabuleiroJogador2[Linha + 1, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro2(tabuleiroJogador2, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();
                                
                                try
                                {
                                    Console.WriteLine($"|Jogador 2 introduza a corveta numero {i + 1}|");
                                    Console.WriteLine($"Linha da corveta: (1 e {celulas})");
                                    Linha = int.Parse(Console.ReadLine()) - 1;

                                    Console.WriteLine($"Coluna da corveta: (A e {(char)('A' + celulas - 1)})");
                                    letter = Console.ReadLine().ToUpper();
                                    if (string.IsNullOrEmpty(letter))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                        Console.ResetColor();
                                        continue;
                                    }
                                    Col = (int)letter[0] - 65;

                                    if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }


                            }
                        }
                        //Adiciona ao tabuleiro do jogador 2
                        tabuleiroJogador2[Linha, Col] = 'C';
                        tabuleiroJogador2[Linha + 1, Col] = 'C';
                        inputValid = true;

                    }
                }

                for (int i = 0; i < submarinos; i++)
                {
                    Console.Clear();
                    bool inputValid = false;
                    while (!inputValid)
                    {
                        MostrarTabuleiro2(tabuleiroJogador2, celulas);


                        try
                        {
                            while (true)
                            {
                                Console.WriteLine($"|Jogador 1 introduza o submarino numero {i + 1}|");
                                Console.WriteLine($"Linha do submarino: (1 e {celulas})");
                                Linha = int.Parse(Console.ReadLine()) - 1;

                                Console.WriteLine($"Coluna do submarino: (A e {(char)('A' + celulas - 1)})");
                                letter = Console.ReadLine().ToUpper();
                                if (string.IsNullOrEmpty(letter))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                    continue;
                                }
                                Col = (int)letter[0] - 65;

                                if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                            Console.ResetColor();
                            continue;
                        }


                        if (Linha >= celulas)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("A posição inserida não é válida. Insira uma posição válida.");
                            Console.WriteLine("Carrega em qualquer tecla para introduzir as coordenadas novamente...");
                            Console.ReadKey();
                            Console.ResetColor();
                            Console.Clear();
                            continue;
                        }
                        bool emptySpot = false;

                        while (!emptySpot)
                        {
                            emptySpot = true;

                            if (tabuleiroJogador2[Linha, Col] != '\0')
                            {
                                emptySpot = false;
                                Console.Clear();
                                //Linha
                                MostrarTabuleiro2(tabuleiroJogador2, celulas);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Posição já ocupada\n");
                                Console.ResetColor();

                                try
                                {
                                    while (true)
                                    {
                                        Console.WriteLine($"|Jogador 1 introduza o submarino numero {i + 1}|");
                                        Console.WriteLine($"Linha do submarino: (1 e {celulas})");
                                        Linha = int.Parse(Console.ReadLine()) - 1;
                                        Console.WriteLine($"Coluna do submarino: (A e {(char)('A' + celulas - 1)})");
                                        letter = Console.ReadLine().ToUpper();
                                        if (string.IsNullOrEmpty(letter))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                            continue;
                                        }
                                        Col = (int)letter[0] - 65;

                                        if (Linha < 0 || Linha > celulas - 1 || Col < 0 || Col > celulas - 1)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Você não inseriu um valor numérico válido. Insira um número entre 1 e " + celulas);
                                    Console.ResetColor();
                                    continue;
                                }
                                

                            }
                        }
                        //Adiciona ao tabuleiro do jogador 1
                        tabuleiroJogador2[Linha, Col] = 'S';
                        inputValid = true;

                    }
                }
            }
            //Total de barcos dos jogadores , HIT = playerShips --;           
            //Verificar Jogadas anteriores
            char[,] tabuJogadas1 = new char[celulas, celulas];
            char[,] tabuJogadas2 = new char[celulas, celulas];
            Console.Clear();
            // Loop do jogo até alguem ganhar playerShips > 0
            while (barcosJogador1 > 0 || barcosJogador2 > 0)
            {
                Console.Clear();
                // Mostrar o tabuleiro do Jogador 1
                if (modoJogo == 1)
                {
                    //Facil
                    Console.WriteLine("   Tabuleiro Jogador 1   Tabuleiro de Jogadas");
                }
                else if (modoJogo == 2)
                {
                    Console.WriteLine("        Tabuleiro Jogador 1              Tabuleiro de Jogadas");
                }
                else if (modoJogo == 3)
                {
                    Console.WriteLine("             Tabuleiro Jogador 1                         Tabuleiro de Jogadas");
                }
                Console.Write("  ");

                //Mostrar os indicadores de A a uma letra
                for (char i = 'A'; i < 'A' + celulas; i++)
                {
                    Console.Write(i.ToString().PadLeft(2));
                }
                Console.Write("   ");
                //Mostrar os indicadores de letras
                for (char i = 'A'; i < 'A' + celulas; i++)
                {
                    Console.Write(i.ToString().PadLeft(2));
                }
                Console.WriteLine();

                //Percorrer o tabuleiro
                for (int i = 0; i < celulas; i++)
                {
                    //Mostraros indicadores de colunas com os numeros
                    Console.Write((i + 1).ToString().PadLeft(2) + " ");
                    for (int j = 0; j < celulas; j++)
                    {
                        //Espaço vazio(água) é um (-)
                        if (tabuleiroJogador1[i, j] == '\0')
                            Console.Write("- ");
                        //Se já tiver acertado e for H mostra H  em verde
                        else if (tabuleiroJogador1[i, j] == 'H')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                        }
                        //Se já tiver acertado e for H mostra A  em vermelho
                        else if (tabuleiroJogador1[i, j] == 'A')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                            //COR DO SUBMARINO
                        }
                        else if (tabuleiroJogador1[i, j] == 'S')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                            //COR DA CARAVELA
                        }
                        else if (tabuleiroJogador1[i, j] == 'C')
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                            //COR DA FRAGATA
                        }
                        else if (tabuleiroJogador1[i, j] == 'F')
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                            //COR DO PORTA AVIÕES
                        }
                        else if (tabuleiroJogador1[i, j] == 'P')
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(tabuleiroJogador1[i, j] + " ");
                            Console.ResetColor();
                        }
                    }
                    //Indicadores de numeros para o tabuleiro de Jogadas
                    Console.Write((i + 1).ToString().PadLeft(2) + " ");

                    //TABULEIRO DAS JOGADAS
                    for (int j = 0; j < celulas; j++)
                    {
                        if (tabuJogadas1[i, j] == '\0')
                            Console.Write("- ");
                        else if (tabuJogadas1[i, j] == 'H')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(tabuJogadas1[i, j] + " ");
                            Console.ResetColor();
                        }
                        else if (tabuJogadas1[i, j] == 'A')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(tabuJogadas1[i, j] + " ");
                            Console.ResetColor();
                        }
                        else
                            Console.Write(tabuJogadas1[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                int adivinharLinha, adivinharColuna;

                //Vez do Jogador 1
                while (true)
                {
                    //Pedir as coordenadas X(linhas) e Y(colunas)
                    try
                    {
                        Console.WriteLine($"Jogador 1, linha entre 1 e {celulas}: ");
                        adivinharLinha = int.Parse(Console.ReadLine()) - 1;
                        Console.WriteLine($"Jogador 1, coluna entre A e {(char)('A' + celulas - 1)}:");
                        string letter = Console.ReadLine().ToUpper();
                        if (string.IsNullOrEmpty(letter))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                            Console.ResetColor();
                            continue;
                        }
                        adivinharColuna = (int)letter[0] - 65;
                        //Verificações para apenas poder jogar dentro do tabuleiro
                        //Validações
                        if (adivinharLinha < 0 || adivinharLinha > celulas - 1 || adivinharColuna < 0 || adivinharColuna > celulas - 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Coordenadas inválidas, insira coordenadas(1 a {celulas}) e letras (A e {(char)('A' + celulas - 1)})");
                            Console.ResetColor();
                        }
                        //Verificar se a jogada já tinha sido jogada anteriormente
                        else if (tabuJogadas1[adivinharLinha, adivinharColuna] != '\0')
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Esta posição já foi jogada (x:{adivinharLinha + 1}, y:{letter})tenta outra posição");
                            Console.ResetColor();
                        }
                        else
                        {
                            //Estando tudo OK adiciona uma jogada e escreve no ficheiro de Jogadas
                            jogadas1++;
                            escritaJogadas.WriteLine($"Jogador 1 x: {adivinharLinha + 1} | y: {adivinharColuna + 1}\n");
                            break;
                        }
                    }
                    catch (FormatException)
                    {
                        //Alguns Erros de input validar
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Insira apenas números (1 a {celulas}) e letras (A e {(char)('A' + celulas - 1)})");
                        Console.ResetColor();
                    }
                }

                // Verificar se acertou o Jogador 1
                if (tabuleiroJogador2[adivinharLinha, adivinharColuna] == 'S' || tabuleiroJogador2[adivinharLinha, adivinharColuna] == 'C' || tabuleiroJogador2[adivinharLinha, adivinharColuna] == 'F' || tabuleiroJogador2[adivinharLinha, adivinharColuna] == 'P')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ACERTOU");
                    barcosJogador2--;
                    tabuJogadas1[adivinharLinha, adivinharColuna] = 'H';
                    Console.ResetColor();
                    switch (tabuleiroJogador2[adivinharLinha, adivinharColuna])
                    {
                        case 'S':
                            pontos1 += 20;
                            break;
                        case 'C':
                            pontos1 += 15;
                            break;
                        case 'F':
                            pontos1 += 15;
                            break;
                        case 'P':
                            pontos1 += 20;
                            break;
                    }
                    tabuleiroJogador2[adivinharLinha, adivinharColuna] = 'H';
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("AGUA");
                    tabuJogadas1[adivinharLinha, adivinharColuna] = 'A';
                    Console.ResetColor();
                    tabuleiroJogador2[adivinharLinha, adivinharColuna] = 'A';
                }

                //Verificar se alguem ganhou playerShips == 0
                if (barcosJogador2 == 0)
                {
                    if (modoJogar == 2)
                    {
                        escritaScores.WriteLine($"{nomeJogador1} | jogadas: {jogadas1} | Dificuldade: {dificuldade} | Pontos: {pontos1}");
                        escritaScores.Close();
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (i % 2 != 0)
                        {
                            int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                            Console.ForegroundColor = (ConsoleColor)colorIndex;
                            Console.WriteLine("                  Jogador 1 Ganhou!");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                            Console.ForegroundColor = (ConsoleColor)colorIndex; Console.WriteLine("Jogador 1 Ganhou!");
                            Thread.Sleep(1000);
                        }
                    }
                    Console.WriteLine();
                    break;
                }

                //Maquina ou Jogador
                // Mostrar a tabela do Jogador 2
                Console.Clear();
                if (modoJogo == 1)
                {
                    //Facil
                    Console.WriteLine("   Tabuleiro Jogador 2   Tabuleiro de Jogadas");
                }
                else if (modoJogo == 2)
                {
                    //Medio
                    Console.WriteLine("        Tabuleiro Jogador 2              Tabuleiro de Jogadas");
                }
                else if (modoJogo == 3)
                {
                    //Dificil
                    Console.WriteLine("             Tabuleiro Jogador 2                         Tabuleiro de Jogadas");
                }
                Console.Write("  ");
                for (char i = 'A'; i < 'A' + celulas; i++)
                {
                    Console.Write(i.ToString().PadLeft(2));
                }
                Console.Write("   ");
                for (char i = 'A'; i < 'A' + celulas; i++)
                {
                    Console.Write(i.ToString().PadLeft(2));
                }
                Console.WriteLine();

                for (int i = 0; i < celulas; i++)
                {
                    Console.Write((i + 1).ToString().PadLeft(2) + " ");
                    for (int j = 0; j < celulas; j++)
                    {
                        if (tabuleiroJogador2[i, j] == '\0')
                            Console.Write("- ");
                        else if (tabuleiroJogador2[i, j] == 'H')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                        }
                        else if (tabuleiroJogador2[i, j] == 'A')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                        }
                        else if (tabuleiroJogador2[i, j] == 'S')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                            //COR DA CARAVELA
                        }
                        else if (tabuleiroJogador2[i, j] == 'C')
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                            //COR DA FRAGATA
                        }
                        else if (tabuleiroJogador2[i, j] == 'F')
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                            //COR DO PORTA AVIÕES
                        }
                        else if (tabuleiroJogador2[i, j] == 'P')
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(tabuleiroJogador2[i, j] + " ");
                            Console.ResetColor();
                        }
                    }
                    Console.Write((i + 1).ToString().PadLeft(2) + " ");

                    for (int j = 0; j < celulas; j++)
                    {
                        if (tabuJogadas2[i, j] == '\0')
                            Console.Write("- ");
                        else if (tabuJogadas2[i, j] == 'H')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(tabuJogadas2[i, j] + " ");
                            Console.ResetColor();
                        }
                        else if (tabuJogadas2[i, j] == 'A')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(tabuJogadas2[i, j] + " ");
                            Console.ResetColor();
                        }
                        else
                            Console.Write(tabuJogadas2[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                adivinharLinha = 0;
                adivinharColuna = 0;
                //Vez do Jogador 2
                //Console.Write("1 - Jogador vs Jogador");
                //Console.Write("2 - Jogador vs Computador");
                if (modoJogar == 1)
                {
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine($"Jogador 2, linha entre 1 e {celulas}:");
                            adivinharLinha = int.Parse(Console.ReadLine()) - 1;
                            Console.WriteLine($"Jogador 2, coluna entre A e {(char)('A' + celulas - 1)}:");
                            string letter = Console.ReadLine().ToUpper();
                            if (string.IsNullOrEmpty(letter))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Você não inseriu nenhum valor. Insira uma letra entre A e " + (char)('A' + celulas - 1));
                                Console.ResetColor();
                                continue;
                            }
                            adivinharColuna = (int)letter[0] - 65;
                            if (adivinharLinha < 0 || adivinharLinha > celulas - 1 || adivinharColuna < 0 || adivinharColuna > celulas - 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Coordenadas inválidas, insira coordenadas(1 a {celulas})");
                                Console.ResetColor();
                            }
                            else if (tabuJogadas2[adivinharLinha, adivinharColuna] != '\0')
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Esta posição já foi jogada (x:{adivinharLinha + 1}, y:{letter})tenta outra posição");
                                Console.ResetColor();
                            }
                            else
                            {
                                escritaJogadas.WriteLine($"Jogador 2 x: {adivinharLinha + 1} | y: {letter}\n");
                                jogadas2++;
                                break;
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Insira apenas números (1 a {celulas}) e letras (A e {(char)('A' + celulas - 1)})");
                            Console.ResetColor();
                        }
                    }
                }
                else if (modoJogar == 2)
                {
                    while (true)
                    {
                        adivinharLinha = rnd.Next(0, celulas);
                        adivinharColuna = rnd.Next(0, celulas);

                        while (tabuJogadas2[adivinharLinha, adivinharColuna] != '\0')
                        {
                            adivinharLinha = rnd.Next(0, celulas);
                            adivinharColuna = rnd.Next(0, celulas);
                        }

                        jogadas2++;

                        escritaJogadas.WriteLine($"Jogador 2 x: {adivinharLinha + 1} | y: {adivinharColuna + 1}\n");
                        break;
                    }
                }

                // Verificar se acertou o Jogador 2
                if (tabuleiroJogador1[adivinharLinha, adivinharColuna] == 'S' || tabuleiroJogador1[adivinharLinha, adivinharColuna] == 'C' || tabuleiroJogador1[adivinharLinha, adivinharColuna] == 'F' || tabuleiroJogador1[adivinharLinha, adivinharColuna] == 'P')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ACERTOU");
                    barcosJogador1--;
                    tabuJogadas2[adivinharLinha, adivinharColuna] = 'H';
                    Console.ResetColor();
                    switch (tabuleiroJogador1[adivinharLinha, adivinharColuna])
                    {
                        case 'S':
                            pontos2 += 20;
                            break;
                        case 'C':
                            pontos2 += 15;
                            break;
                        case 'F':
                            pontos2 += 15;
                            break;
                        case 'P':
                            pontos2 += 20;
                            break;
                    }
                    tabuleiroJogador1[adivinharLinha, adivinharColuna] = 'H';
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("AGUA");
                    tabuJogadas2[adivinharLinha, adivinharColuna] = 'A';
                    Console.ResetColor();
                    tabuleiroJogador1[adivinharLinha, adivinharColuna] = 'A';
                }

                //Verificar se alguem ganhou playerShips == 0              
                if (barcosJogador1 == 0)
                {
                    if (modoJogar == 2)
                    {
                        escritaScores.WriteLine($"Computador | jogadas: {jogadas2} | Dificuldade: {dificuldade} | Pontos: {pontos2}");
                        escritaScores.Close();

                    }
                    else if (modoJogar == 1)
                    {
                        escritaScores.WriteLine($"{nomeJogador2} | jogadas: {jogadas2} | Dificuldade: {dificuldade} | Pontos: {pontos2}");
                        escritaScores.Close();
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (modoJogar == 2)
                        {
                            if (i % 2 != 0)
                            {
                                int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                                Console.ForegroundColor = (ConsoleColor)colorIndex; Console.WriteLine("                  O Computador Ganhou!");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                                Console.ForegroundColor = (ConsoleColor)colorIndex; Console.WriteLine("O Computador Ganhou!");
                                Thread.Sleep(1000);
                            }
                        }
                        else if (modoJogar == 1)
                        {
                            if (i % 2 != 0)
                            {
                                int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                                Console.ForegroundColor = (ConsoleColor)colorIndex; Console.WriteLine("                  Jogador 2 Ganhou!");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                int colorIndex = rnd.Next((int)ConsoleColor.DarkGray, (int)ConsoleColor.Yellow);
                                Console.ForegroundColor = (ConsoleColor)colorIndex; Console.WriteLine("Jogador 2 Ganhou!");
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    Console.WriteLine();
                    break;
                }
            }
            Console.ResetColor();
            escritaJogadas.Close();

            StreamReader ler = new StreamReader(@"c:\ficheiros\BatalhaNaval\Scores.txt", Encoding.GetEncoding("utf-8"));
            string linha;
            List<Score> lst_scores = new List<Score>();
            while ((linha = ler.ReadLine()) != null)
            {
                Score score = new Score();
                string nome = linha.Substring(0, linha.IndexOf(" | jogadas: "));
                string jogadas = linha.Substring(linha.IndexOf(" | jogadas: ") + " | jogadas: ".Length, linha.IndexOf(" | Dificuldade: ") - (linha.IndexOf(" | jogadas: ") + " | jogadas: ".Length));
                string dificuldades = linha.Substring(linha.IndexOf(" | Dificuldade: ") + " | Dificuldade: ".Length, linha.IndexOf(" | Pontos: ") - (linha.IndexOf(" | Dificuldade: ") + " | Dificuldade: ".Length));
                string pontos = linha.Substring(linha.IndexOf(" | Pontos: ") + " | Pontos: ".Length);

                score.add_Score(nome, int.Parse(jogadas), dificuldades, int.Parse(pontos));
                lst_scores.Add(score);
            }
            ler.Close();

            FileInfo ficheiroScore = new FileInfo(@"c:\ficheiros\BatalhaNaval\Scores.txt");
            StreamWriter escritaScore = ficheiroScores.CreateText();
            //Ordenadas por numero de Jogadas
            foreach (Score s in lst_scores.OrderBy(x => x.NumJogadas).Take(20))
            {
                escritaScore.WriteLine($"{s.Nome} | jogadas: {s.NumJogadas} | Dificuldade: {s.Dificuldade} | Pontos: {s.Pontos}");
            }
            escritaScore.Close();

            mostrarCopy();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Carrega em qualquer tecla para sair do jogo...");
            Console.ResetColor();
            Console.ReadKey();
        }



        static void mostrarMenuInstrucoes()
        {
            Console.Clear();
            Console.WriteLine("  +--------------------------------+");
            Console.WriteLine("  |      Manual de Instruções      |");
            Console.WriteLine("  +--------------------------------+");
            Console.WriteLine("  | 1 - Ler manual de instruções   |");
            Console.WriteLine("  | 2 - Não preciso                |");
            Console.WriteLine("  +--------------------------------+\n");
            Console.Write("Opção: ");
        }
        static void mostrarInstrucoes()
        {
            Console.Clear();
            Console.WriteLine("  +--------------------------------+");
            Console.WriteLine("  |      Manual de Instruções      |");
            Console.WriteLine("  +--------------------------------+\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Como jogar: \n");
            Console.ResetColor();
            Console.Write("Existem 3 modos de jogo o ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("fácil, ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("médio");
            Console.ResetColor();
            Console.Write(" e ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("difícil");
            Console.ResetColor();
            Console.Write(", cada um deles contem diferenças porém todos eles tem 8 barcos\n" +
                "   Fácil - Tabuleiro de Jogo 10 * 10 contendo 4 Submarinos e 4 Corvetas,\n" +
                "   Médio - Tabuleiro de Jogo 15 * 15 contendo 3 Fragatas, 4 Corvetas e 1 Submarino,\n" +
                "   Difícil - Tabuleiro de Jogo 20 * 20 contendo 3 Porta Aviões, 2 Fragatas, 2 Corvetas e 1 Submarino;\n"); ;
            Console.Write("Depois de escolher o modo de jogo desejado o utilizador tem de escolher se quer jogar contra outro jogador\nou contra o computador.\n" +
                "Dependendo contra quem está a jogar é mostrado ao utilizador o tabuleiro de jogo (Vez do jogador 1: tabuleiro 1)\n" +
                "e o tabuleiro com as suas jogadas anteriores depois de aparecer o tabuleiro, o utilizador pode \n" +
                "então escolher a Linha e a Coluna em que quer atingir\n" +
                "Exemplo: Linha 5 | Coluna B\n" +
                "Dependendo onde estão colocados os barcos do adversário pode ou não ter acertado, designados por ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ÁGUA(A)");
            Console.ResetColor();
            Console.Write(" ou ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("ACERTOU(H)\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Características: ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("   (S)");
            Console.ResetColor();
            Console.Write("Submarinos = ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Azul");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("   (C)");
            Console.ResetColor();
            Console.Write("Corvetas = ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Azul Turquesa");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("   (F)");
            Console.ResetColor();
            Console.Write("Fragatas = ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Amarelo");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("   (P)");
            Console.ResetColor();
            Console.Write("Porta Aviões = ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Roxo\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Regras: ");
            Console.ResetColor();
            Console.Write("Cada jogador só pode jogar uma vez numa coordenada (Jogador 1: Linha 1 | Coluna H) \n" +
                "se o jogador 1 mais tarde no jogo tentar colocar as mesmas coordenadas irá aparecer um aviso para\ncolocar outras coordenadas.\n" +
                "Só é permitido jogar");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" DENTRO");
            Console.ResetColor();
            Console.Write(" do tabuleiro.\n\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Carrega em alguma tecla para avançar...");
            Console.ResetColor();

            Console.ReadKey();
        }

        static void mostrarModosTabuleiros()
        {
            Console.Clear();
            Console.WriteLine("  +---------------------------------+");
            Console.WriteLine("  |        Tabuleiro de Jogo        |");
            Console.WriteLine("  +---------------------------------+");
            Console.WriteLine("  | 1 - Fácil (tabuleiro 10 * 10)   |");
            Console.WriteLine("  | 2 - Médio (tabuleiro 15 * 15)   |");
            Console.WriteLine("  | 3 - Difícil (tabuleiro 20 * 20) |");
            Console.WriteLine("  +---------------------------------+\n");
            Console.Write("Opção: ");
        }

        static void mostrarModosJogos()
        {
            Console.WriteLine("  +----------------------------+");
            Console.WriteLine("  |        Tipo de Jogo        |");
            Console.WriteLine("  +----------------------------+");
            Console.WriteLine("  | 1 - Jogador vs Jogador     |");
            Console.WriteLine("  | 2 - Jogador vs Computador  |");
            Console.WriteLine("  +----------------------------+\n");
            Console.Write("Opção: ");
        }

        private static void MostrarTabuleiro1(char[,] tabuleiro1, int celulas)
        {
            Console.Write("  ");
            for (char i = 'A'; i < 'A' + celulas; i++)
            {
                Console.Write(i.ToString().PadLeft(2));
            }
            Console.WriteLine();
            for (int b = 0; b < celulas; b++)
            {
                //Mostraros indicadores de colunas com os numeros
                Console.Write((b + 1).ToString().PadLeft(2) + " ");
                for (int j = 0; j < celulas; j++)
                {
                    //Espaço vazio(água) é um (-)
                    if (tabuleiro1[b, j] == '\0')
                        Console.Write("- ");
                    //Se já tiver acertado e for H mostra H  em verde
                    else if (tabuleiro1[b, j] == 'H')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                    }
                    //Se já tiver acertado e for H mostra A  em vermelho
                    else if (tabuleiro1[b, j] == 'A')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                        //COR DO SUBMARINO
                    }
                    else if (tabuleiro1[b, j] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                        //COR DA CARAVELA
                    }
                    else if (tabuleiro1[b, j] == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                        //COR DA FRAGATA
                    }
                    else if (tabuleiro1[b, j] == 'F')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                        //COR DO PORTA AVIÕES
                    }
                    else if (tabuleiro1[b, j] == 'P')
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(tabuleiro1[b, j] + " ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }


        private static void MostrarTabuleiro2(char[,] tabuleiro2, int celulas)
        {
            Console.Write("  ");
            for (char i = 'A'; i < 'A' + celulas; i++)
            {
                Console.Write(i.ToString().PadLeft(2));
            }
            Console.WriteLine();
            for (int b = 0; b < celulas; b++)
            {
                //Mostraros indicadores de colunas com os numeros
                Console.Write((b + 1).ToString().PadLeft(2) + " ");
                for (int j = 0; j < celulas; j++)
                {
                    //Espaço vazio(água) é um (-)
                    if (tabuleiro2[b, j] == '\0')
                        Console.Write("- ");
                    //Se já tiver acertado e for H mostra H  em verde
                    else if (tabuleiro2[b, j] == 'H')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                    }
                    //Se já tiver acertado e for H mostra A  em vermelho
                    else if (tabuleiro2[b, j] == 'A')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                        //COR DO SUBMARINO
                    }
                    else if (tabuleiro2[b, j] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                        //COR DA CARAVELA
                    }
                    else if (tabuleiro2[b, j] == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                        //COR DA FRAGATA
                    }
                    else if (tabuleiro2[b, j] == 'F')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                        //COR DO PORTA AVIÕES
                    }
                    else if (tabuleiro2[b, j] == 'P')
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(tabuleiro2[b, j] + " ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }

        private static void PreencherRandom(char[,] tabuleiroJogador2, int celulas, int portaAvioes, int fragatas, int corvetas, int submarinos) {
            Random rnd = new Random();
            for (int i = 0; i < portaAvioes; i++)
            {
                //Gerar uma linha e uma coluna
                int row = rnd.Next(0, celulas - 4);
                int col = rnd.Next(0, celulas - 1);
                bool emptySpot = false;

                while (!emptySpot)
                {
                    emptySpot = true;

                    if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row + 1, col] != '\0' || tabuleiroJogador2[row + 2, col] != '\0' || tabuleiroJogador2[row + 3, col] != '\0')
                    {
                        emptySpot = false;
                        //Linha
                        row = rnd.Next(0, celulas - 4);
                        col = rnd.Next(0, celulas - 1);
                    }
                }

                tabuleiroJogador2[row, col] = 'P';
                tabuleiroJogador2[row + 1, col] = 'P';
                tabuleiroJogador2[row + 2, col] = 'P';
                tabuleiroJogador2[row + 3, col] = 'P';
            }

            //-Fragatas(3 posições)
            for (int i = 0; i < fragatas; i++)
            {
                int row = rnd.Next(0, celulas - 1);
                int col = rnd.Next(0, celulas - 3);

                bool emptySpot = false;
                while (!emptySpot)
                {
                    emptySpot = true;

                    if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row, col + 1] != '\0' || tabuleiroJogador2[row, col + 2] != '\0')
                    {
                        emptySpot = false;
                        row = rnd.Next(0, celulas - 1);
                        col = rnd.Next(0, celulas - 3);
                    }
                }

                tabuleiroJogador2[row, col] = 'F';
                tabuleiroJogador2[row, col + 1] = 'F';
                tabuleiroJogador2[row, col + 2] = 'F';

            }

            //-Corvetas(2 posições)
            for (int i = 0; i < corvetas; i++)
            {
                int row = rnd.Next(0, celulas - 2);
                int col = rnd.Next(0, celulas - 1);

                bool emptySpot = false;
                while (!emptySpot)
                {
                    emptySpot = true;

                    if (tabuleiroJogador2[row, col] != '\0' || tabuleiroJogador2[row + 1, col] != '\0')
                    {
                        emptySpot = false;
                        row = rnd.Next(0, celulas - 2);
                        col = rnd.Next(0, celulas - 1);
                    }
                }

                tabuleiroJogador2[row, col] = 'C';
                tabuleiroJogador2[row + 1, col] = 'C';
            }

            //-Submarino (1 posição)
            for (int i = 0; i < submarinos; i++)
            {
                int row = rnd.Next(0, celulas);
                int col = rnd.Next(0, celulas);
                bool emptySpot = false;

                while (!emptySpot)
                {
                    emptySpot = true;

                    if (tabuleiroJogador2[row, col] != '\0')
                    {
                        emptySpot = false;
                        row = rnd.Next(0, celulas);
                        col = rnd.Next(0, celulas);
                    }
                }

                tabuleiroJogador2[row, col] = 'S';
            }
        }

        static void mostrarCopy()
        {
            Console.Write("Feito por ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Daniel Cunha\n\n");
            Console.ResetColor();
        }
    }
    class Score
    {
        public void add_Score(string nome, int numJogadas, string dificuldade, int pontos)
        {
            Nome = nome;
            NumJogadas = numJogadas;
            Dificuldade = dificuldade;
            Pontos = pontos;
        }
        public string Nome { get; set; }
        public int NumJogadas { get; set; }
        public string Dificuldade { get; set; }
        public int Pontos { get; set; }
    }
}