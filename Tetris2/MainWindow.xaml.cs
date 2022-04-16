using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Border[,] gameBoard = new Border[20, 10];
        internal double cellSize { get; set; }

        internal List<Pièce> pièces = new List<Pièce>(); // liste des pièces de la partie
        private int ActualPiecePlaced = 0; // la piece dans la liste de piece actuelle
        private Random random = new Random();

        private Pièce ActualPiece; // la pièce actuellement joué
        private int ActualRotation = 0; // la rotation que la pièce à actuellement
        private int[] ActualRotationPos; // position du centre de rotation de la pièce actualle
        private List<int[]> ActualPiecePos; // toutes les coordonnées de la pièce actuelle
       
        private bool isGameOver = false;

        private Timer gameTimer;

        private Brush COLOR_PIECE_I = Brushes.LightBlue;
        private Brush COLOR_PIECE_O = Brushes.Yellow;
        private Brush COLOR_PIECE_T = Brushes.Purple;
        private Brush COLOR_PIECE_L = Brushes.Orange;
        private Brush COLOR_PIECE_J = Brushes.Blue;
        private Brush COLOR_PIECE_Z = Brushes.Red;
        private Brush COLOR_PIECE_S = Brushes.Green;

        public MainWindow()
        {
            InitializeComponent();

            button_play.MouseEnter += (sender, e) =>
            {
                buttonPlay_img.Visibility = Visibility.Hidden;
                buttonPlay_img_mouseover.Visibility = Visibility.Visible;
            };
            button_play.MouseLeave += (sender, e) =>
            {
                buttonPlay_img.Visibility = Visibility.Visible;
                buttonPlay_img_mouseover.Visibility = Visibility.Hidden;
            };
        }

        private void AddGameBoard()
        {
            // 10 x 20
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Border r = new Border();
                    r.BorderBrush = Brushes.AliceBlue;
                    r.DataContext = cellSize;
                    r.BorderThickness = new Thickness(0.2, 0.2, 0.2, 0.2);
                    GameBoard.Children.Add(r);
                    Grid.SetColumn(r, y);
                    Grid.SetRow(r, x);

                    gameBoard[x, y] = r; // Ligne = [0], Colonne = [1]

                    if (x == 0 && y == 0) // Si premier tour alors on set la width des columns pour qu'elle soit = à la height des rows
                    {
                        UpdateLayout();
                        cellSize = gameBoard[0, 0].ActualHeight;
                    }

                    r.Width = cellSize;
                }
            }
        }

        private void StartGame()
        {
            isGameOver = false;

            // ui
            Border_GameBoard.BorderThickness = new Thickness(0, 0, 0, 0);
            GameBoard.Children.Clear();
            AddGameBoard();

            // Génération de la liste des pièces qui vont arrivé dans l'ordre
            pièces.Clear();
            pièces = AddRandomPieces();
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);
            pièces.Insert(0, Pièce.I);

            // timer init
            gameTimer = new Timer(1000);
            gameTimer.Elapsed += new ElapsedEventHandler(MovePieceDownOneBlock);
            gameTimer.Start();

            // la premiere piece
            ActualPiecePlaced = -1; // car ça fait ActualPiecePlaced++ en début de méthode après
            AfficherProchainePiece();
        }

        private void AfficherProchainePiece()
        {
            ActualPiecePlaced++;
            ActualRotation = 0;

            switch (pièces[ActualPiecePlaced])
            {
                case Pièce.I:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,3},
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{0,6}
                    }, new int[] { 0, 3 }, COLOR_PIECE_I, Pièce.I);

                    break;
                case Pièce.J:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,3},
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{1,5}
                    }, new int[] { 0, 3 }, COLOR_PIECE_J, Pièce.J);

                    break;
                case Pièce.L:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,3},
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{1,3}
                    }, new int[] { 0, 3 }, COLOR_PIECE_L, Pièce.L);

                    break;
                case Pièce.O:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{1,4},
                        new int[]{1,5}
                    }, new int[] { 0, 4 }, COLOR_PIECE_O, Pièce.O);

                    break;
                case Pièce.T:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,3},
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{1,4}
                    }, new int[] { 0, 3 }, COLOR_PIECE_T, Pièce.T);

                    break;
                case Pièce.S:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{1,3},
                        new int[]{0,4},
                        new int[]{0,5},
                        new int[]{1,4}
                    }, new int[] { 0, 3 }, COLOR_PIECE_S, Pièce.S);

                    break;
                case Pièce.Z:

                    MakePieceAppear(new List<int[]>()
                    {
                        new int[]{0,3},
                        new int[]{0,4},
                        new int[]{1,4},
                        new int[]{1,5}
                    }, new int[] { 0, 3 }, COLOR_PIECE_Z, Pièce.Z);

                    break;

            }

            // Affiche la prochaine pièece
            foreach (Image img in Grid_NextPieces.Children)
            {
                img.Visibility = Visibility.Collapsed;
                if (img.Name.Contains(pièces[ActualPiecePlaced + 1].ToString()))
                    img.Visibility = Visibility.Visible;
            }

        }

        private void MovePieceDownOneBlock(object? sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    foreach(var piecePartPos in ActualPiecePos)
                    {
                        if ((string)gameBoard[piecePartPos[0] + 1, piecePartPos[1]].Tag == "STILL") // Si la descente tappera dans un autre truc
                        {
                            throw new NotImplementedException();
                        }
                    }

                    // descend la pièce
                    Brush pcColor = gameBoard[ActualPiecePos[0][0], ActualPiecePos[0][1]].Background;

                    foreach (var piecePartPos in ActualPiecePos)
                    { 
                        // remove l'ancienne piece                      
                        gameBoard[piecePartPos[0], piecePartPos[1]].Tag = string.Empty;
                        gameBoard[piecePartPos[0], piecePartPos[1]].Background = null;
                    }

                    foreach (var piecePartPos in ActualPiecePos)
                     // put new piece avec une distance de + 1
                    {
                        gameBoard[piecePartPos[0] + 1, piecePartPos[1]].Tag = "MOVING";
                        gameBoard[piecePartPos[0] + 1, piecePartPos[1]].Background = pcColor;
                    }

                    for (int i = 0; i < ActualPiecePos.Count; i++) // set new coo 
                    {
                        ActualPiecePos[i][0] = ActualPiecePos[i][0] + 1;
                    }
                    // et ne pas oublier rotation coo qui descend aussi
                    ActualRotationPos[0] = ActualRotationPos[0] + 1;

                }
                catch
                {
                    // = atteind le fond, on les met donc en still

                    foreach (var piecePartPos in ActualPiecePos)
                    {
                        
                        gameBoard[piecePartPos[0], piecePartPos[1]].Tag = "STILL";
                    }

                    // check for line completation
                    List<int> completedLine = new List<int>();

                    for (int x = 0; x < 20; x++)
                    {
                        bool isLineComplet = true;
                        for (int y = 0; y < 10; y++)
                        {
                            if((string)gameBoard[x,y].Tag != "STILL")
                            {
                                // ligne pas complete donc teste la suivante
                                isLineComplet = false;
                                break;
                            }
                        }

                        if (isLineComplet)
                        {
                            GameBoard.Children.Clear();

                            // ex : line 8 complet
                            for (int line = x - 1; line >= 0; line--) // 7, 6, 5, 4, 3, 2, 1, 0
                            {
                                // 7 have to become 8
                                for (int y = 0; y < 10; y++)
                                {
                                    // 8             now equal to = 7
                                    gameBoard[line + 1, y] = gameBoard[line, y];
                                }

                                
                            }

                            // 0 now equal to 1
                            // 0 is now empty.
                            for (int x2 = 0; x2 < 20; x2++)
                            {
                                for (int y = 0; y < 10; y++)
                                {                                   
                                    if (x2 == 0) // première ligne
                                    {
                                        Border r = new Border();
                                        r.BorderBrush = Brushes.AliceBlue;
                                        r.DataContext = cellSize;
                                        r.BorderThickness = new Thickness(0.2, 0.2, 0.2, 0.2);
                                        GameBoard.Children.Add(r);
                                        Grid.SetColumn(r, y);
                                        Grid.SetRow(r, x2);
                                        gameBoard[x2, y] = r;
                                        r.Width = cellSize;
                                    }
                                    else // première line toujours void
                                    {
                                        GameBoard.Children.Add(gameBoard[x2, y]);
                                        Grid.SetColumn(gameBoard[x2, y], y);
                                        Grid.SetRow(gameBoard[x2, y], x2);
                                    }
                                }
                            }
                        }
                    }

                    //lance une nouvelle pièce
                    AfficherProchainePiece();
                }
            });

        }

        private void MovePieceLeftOrRight(bool isLeft)
        {
            try
            {

                foreach (var piecePartPos in ActualPiecePos)
                {
                    if (isLeft)
                    {
                        if ((string)gameBoard[piecePartPos[0], piecePartPos[1] - 1].Tag == "STILL") // Si la descente tappera dans un autre truc
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                        if ((string)gameBoard[piecePartPos[0], piecePartPos[1] + 1].Tag == "STILL") // Si la descente tappera dans un autre truc
                    {
                        throw new NotImplementedException();
                    }
                }

                // bouge la piece
                Brush pcColor = gameBoard[ActualPiecePos[0][0], ActualPiecePos[0][1]].Background;

                foreach (var piecePartPos in ActualPiecePos) // remove l'ancienne piece
                {
                    gameBoard[piecePartPos[0], piecePartPos[1]].Tag = string.Empty;
                    gameBoard[piecePartPos[0], piecePartPos[1]].Background = null;
                }

                foreach (var piecePartPos in ActualPiecePos)
                // put new piece avec une distance de + 1 ou - 1
                {
                    if (isLeft)
                    {
                        gameBoard[piecePartPos[0], piecePartPos[1] - 1].Tag = "MOVING";
                        gameBoard[piecePartPos[0], piecePartPos[1] - 1].Background = pcColor;
                    }
                    else
                    {
                        gameBoard[piecePartPos[0], piecePartPos[1] + 1].Tag = "MOVING";
                        gameBoard[piecePartPos[0], piecePartPos[1] + 1].Background = pcColor;
                    }
                }

                for (int i = 0; i < ActualPiecePos.Count; i++) // set new coo 
                {
                    if (isLeft)
                        ActualPiecePos[i][1] = ActualPiecePos[i][1] - 1;
                    else
                        ActualPiecePos[i][1] = ActualPiecePos[i][1] + 1;

                }
                // et ne pas oublier rotation coo qui descend aussi
                if (isLeft)

                    ActualRotationPos[1] = ActualRotationPos[1] - 1;
                else
                    ActualRotationPos[1] = ActualRotationPos[1] + 1;


            }
            catch
            {
            }
        }

        private void MakePieceAppear(List<int[]> positions, int[] rotation, Brush color, Pièce piece)
        {
            foreach (var position in positions)
            {
                if ((string)gameBoard[position[0], position[1]].Tag == "STILL")
                {
                    GameOver();
                }
                else
                {
                    gameBoard[position[0], position[1]].Tag = "MOVING";
                    gameBoard[position[0], position[1]].Background = color;
                }

            }

            gameBoard[rotation[0], rotation[1]].Tag = "ROTATION";
            ActualRotationPos = rotation;
            ActualPiecePos = positions;

            ActualPiece = piece;
        }

        private void GameOver()
        {
            isGameOver = true;

            gameTimer.Stop();
            Border_GameBoard.BorderThickness = new Thickness(5, 5, 5, 0);
        }

        private List<Pièce> AddRandomPieces()
        {
            Array values = Enum.GetValues(typeof(Pièce));
            List<Pièce> p = new List<Pièce>();

            for (int i = 0; i < 500; i++)
            {
                p.Add((Pièce)values.GetValue(random.Next(values.Length)));
                System.Threading.Thread.Sleep(random.Next(0, 1));
            }

            return p;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            AddGameBoard();

            this.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged); // Pour pas trigger l'event au démarrage et que ça fasse une ereur gameBoard = null
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            cellSize = gameBoard[0, 0].ActualHeight;

            // reset la width pour qu'elle soit égal à la hauteur de la row.
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    gameBoard[x, y].Width = cellSize;
                }
            }
        }

        private void ButtonStart_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                StartGame();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameOver)
            {
                if (e.Key == Key.Space)
                {
                    ActualRotation++;
                    if (ActualRotation > 3)
                        ActualRotation = 0;
                    RotatePiece();
                }
                else if (e.Key == Key.Left)
                {
                    MovePieceLeftOrRight(true);
                }
                else if (e.Key == Key.Right)
                {
                    MovePieceLeftOrRight(false);
                }
                else if (e.Key == Key.Down)
                {
                    MovePieceDownOneBlock(this, null);
                }
            }
        }

        private void RotatePiece()
        {
            // Rotate actual pieces
            ActualPiecePos.ForEach(x =>
            {
                gameBoard[x[0], x[1]].Background = null;
                gameBoard[x[0], x[1]].Tag = string.Empty;
            });

            switch (ActualRotation) // ROTATION ALGHO
            {
                case 0:
                    switch (ActualPiece)
                    {
                        case Pièce.I:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 3 },
                            }, COLOR_PIECE_I);
                            break;

                        case Pièce.O:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_O);
                            break;

                        case Pièce.T:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_T);
                            break;

                        case Pièce.L:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0], ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                            }, COLOR_PIECE_L);
                            break;


                        case Pièce.J:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0], ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 2},
                            }, COLOR_PIECE_J);
                            break;

                        case Pièce.Z:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] +1 },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 2},
                            }, COLOR_PIECE_Z);
                            break;

                        case Pièce.S:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1},
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +2},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1},
                            }, COLOR_PIECE_S);
                            break;
                    }

                    break;
                case 1:
                    switch (ActualPiece)
                    {
                        case Pièce.I:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] +2},
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01]+2 },
                                new int[2]{ ActualRotationPos[0] + 3, ActualRotationPos[01] +2},
                            }, COLOR_PIECE_I);
                            break;
                        case Pièce.O:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_O);
                            break;

                        case Pièce.T:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01]  },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_T);
                            break;

                        case Pièce.L:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1},
                                new int[2]{ ActualRotationPos[0] + 2 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_L);
                            break;

                        case Pièce.J:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01] + 1},
                                new int[2]{ ActualRotationPos[0] + 2 , ActualRotationPos[01]  },
                            }, COLOR_PIECE_J);
                            break;

                        case Pièce.Z:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] +1 },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01]},
                            }, COLOR_PIECE_Z);
                            break;

                        case Pièce.S:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01] + 1},
                            }, COLOR_PIECE_S);
                            break;
                    }
                    break;
                case 2:
                    switch (ActualPiece)
                    {
                        case Pièce.I:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0]  , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 3 },
                            }, COLOR_PIECE_I);
                            break;
                        case Pièce.O:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 1 },
                            }, COLOR_PIECE_O);
                            break;

                        case Pièce.T:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01]  },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 2 },
                            }, COLOR_PIECE_T);
                            break;
                        case Pièce.L:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 2 },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01]},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 2 },
                            }, COLOR_PIECE_L);
                            break;

                        case Pièce.J:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01]},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] + 2 },
                            }, COLOR_PIECE_J);
                            break;

                        case Pièce.Z:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] +1 },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 2},
                            }, COLOR_PIECE_Z);
                            break;

                        case Pièce.S:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1},
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +2},
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1},
                            }, COLOR_PIECE_S);
                            break;
                    }
                    break;

                case 3:
                    switch (ActualPiece)
                    {
                        case Pièce.I:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] +2},
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01]+2 },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01] +2},
                                new int[2]{ ActualRotationPos[0] + 3, ActualRotationPos[01]+2 },
                            }, COLOR_PIECE_I);
                            break;
                        case Pièce.O:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1] + 1 },
                            }, COLOR_PIECE_O);
                            break;



                        case Pièce.T:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1]  },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[1]  },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1] + 1 },
                            }, COLOR_PIECE_T);
                            break;

                        case Pièce.L:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1]},
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0] + 2 , ActualRotationPos[1] +1 },
                            }, COLOR_PIECE_L);
                            break;

                        case Pièce.J:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[1]},
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[1] },
                                new int[2]{ ActualRotationPos[0], ActualRotationPos[1] +1 },
                            }, COLOR_PIECE_J);
                            break;


                        case Pièce.Z:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01]  +1 },
                                new int[2]{ ActualRotationPos[0] + 1 , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] +1 },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01]},
                            }, COLOR_PIECE_Z);
                            break;

                        case Pièce.S:
                            CheckIfCanAndRotate(new List<int[]>()
                            {
                                new int[2]{ ActualRotationPos[0] , ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] },
                                new int[2]{ ActualRotationPos[0] + 1, ActualRotationPos[01] + 1 },
                                new int[2]{ ActualRotationPos[0] + 2, ActualRotationPos[01] + 1},
                            }, COLOR_PIECE_S);
                            break;
                    }
                    break;
            }
        }

        private void CheckIfCanAndRotate(List<int[]> posOfNewRotation, Brush color)
        {
            try
            {
                posOfNewRotation.ForEach(piecePos =>
                {
                    if ((string)gameBoard[piecePos[0], piecePos[1]].Tag == "STILL") // Si la rotation tappera dans le truc
                    {
                        new Exception();
                    }
                });

                // La rotation est possible car pas d'erreur 
                posOfNewRotation.ForEach(piecePos =>
                {
                    ColorAndTagAt(piecePos[0], piecePos[1], "MOVING", color);

                });
                // on peut donc aussi mettre les nouvelles pos
                ActualPiecePos = posOfNewRotation;
            }
            catch
            {
                // piece ne peut rotate = remet les couleurs anciennes
                ActualPiecePos.ForEach(x => // remet les couleurs car impossible à rotate
                {
                    gameBoard[x[0], x[1]].Background = color;
                    gameBoard[x[0], x[1]].Tag = "MOVING";
                });
            }
        }

        private void ColorAndTagAt(int x, int y, string tag, Brush color)
        {
            gameBoard[x, y].Background = COLOR_PIECE_I;
            gameBoard[x, y].Background = color;
            gameBoard[x, y].Tag = tag;
        }
    }

    internal enum Pièce
    {
        /*
         * O O
         * O O
        */
        O,
        /*
         * O
         * O
         * O
         * O
        */
        I,
        /*
         *  OO
         * OO
        */
        S,
        /*
         * OO
         *  OO
        */
        Z,
        /*
         * O
         * O
         * OO
        */
        L,
        /*
         *  O
         *  O
         * OO
        */
        J,
        /*
         * OOO
         *  O
        */
        T
    }
}
