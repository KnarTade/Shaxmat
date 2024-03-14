using CheckmateLibrary;
using CheckmateLibrary.Pieces;
using CheckmateLibrary.Structs;

namespace Checkmate;

public class BoardPrint
{
    /// <summary>
    /// Empty chessboard
    /// </summary>
    /// <returns></returns>
    public static char[,] InitializeChessboard()
    {
        char[,] chessboard = new char[8, 8];
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                chessboard[i, j] = ' ';
            }
        }
        return chessboard;
    }

    /// <summary>
    /// Chessboard with numbers and letters
    /// </summary>
    /// <param name="chessboard"></param>
    public static void PrintChessBoard(char[,] chessboard)
    {
        Console.Write("  A B C D E F G H");
        Console.WriteLine();
        ConsoleColor originalConsoleColor = Console.BackgroundColor;
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            Console.Write(i + 1);
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                Console.BackgroundColor = (i + j) % 2 == 0 ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.Write(chessboard[i, j] + " ");
            }
            Console.BackgroundColor = originalConsoleColor;
            Console.WriteLine();
        }
    }

    public static void PrintChessBoardWithValidMoves(char[,] chessboard, List<Coordinates> validMoves)
    {
        Console.Write("  A B C D E F G H");
        Console.WriteLine();
        ConsoleColor originalConsoleColor = Console.BackgroundColor;
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            Console.Write(i + 1);
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                Console.BackgroundColor = (i + j) % 2 == 0 ? ConsoleColor.DarkGray : ConsoleColor.White;
                Coordinates currentPosition = new Coordinates(i, j);
                if (validMoves.Contains(currentPosition))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(chessboard[i, j] + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(chessboard[i, j] + " ");
                }
            }
            Console.BackgroundColor = originalConsoleColor;
            Console.WriteLine();
        }
        Console.ResetColor();
    }



    /// <summary>
    /// Is piece  chessboardi sahmanum
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static bool IsValidInput(string input)
    {
        if (input.Length == 2)
        {
            char firstChar = input[0];
            char secondChar = input[1];
            bool isFirstCharValid = char.IsLetter(firstChar) && (firstChar >= 'A' && firstChar <= 'H');
            bool isSecondCharValid = char.IsDigit(secondChar) && (secondChar >= '1' && secondChar <= '8');
            return isFirstCharValid && isSecondCharValid;
        }
        return false;
    }

    /// <summary>
    /// add all pieces on chessboard
    /// </summary>
    /// <param name="chessboard"></param>
    public static bool SetPieces(char[,] chessboard)
    {
        PlacePiece(Figure.King, FigureColor.Black, chessboard);
        PlacePiece(Figure.King, FigureColor.White, chessboard);
        PlacePiece(Figure.Queen, FigureColor.White, chessboard);
        PlacePiece(Figure.Rook, FigureColor.White, chessboard);
        PlacePiece(Figure.Rook, FigureColor.White, chessboard);

        if (AreFiguresOverlapping(chessboard))
        {
            Console.WriteLine("Warning: Overlapping positions detected! Please reposition the pieces.");
            // return SetPieces(InitializeChessboard()); // Restart piece placement
            return false;
        }

        //PrintChessBoard(chessboard);
        // return chessboard;
        return true;
    }

    /// <summary>
    /// check if there is 2 pieces in the same coordinate
    /// </summary>
    /// <param name="chessboard"></param>
    /// <returns></returns>
    private static bool AreFiguresOverlapping(char[,] chessboard)
    {
        HashSet<char> occupiedPositions = new HashSet<char>();
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                char currentPiece = chessboard[i, j];
                // Skip empty positions
                if (currentPiece == ' ')
                    continue;
                // Check if the position is already occupied by another piece
                if (occupiedPositions.Contains(currentPiece))
                {
                    //Coordinates position = new Coordinates(i, j);
                    //Coordinates.PrintError($"Error: Position {position} is occupied by more than one piece.");
                    return false;
                }
                occupiedPositions.Add(currentPiece);
            }
        }
        return true;
    }

    /// <summary>
    /// add piece on empty chessboard
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="color"></param>
    /// <param name="chessboard"></param>
    private static void PlacePiece(Figure piece, FigureColor color, char[,] chessboard)
    {
        {
            while (true)
            {
                Console.Write($"Enter the position for the {color} {piece} (e.g., A8): ");
                string position = Console.ReadLine();
                Coordinates coordinates = new Coordinates(position);

                // Check if the entered position is valid
                if (!Coordinates.IsValidCoordinates(coordinates))
                {
                    Coordinates.PrintError("Invalid position. Please enter a valid position.");
                    continue;
                }

                // Check if the entered position is already occupied
                if (chessboard[coordinates.Row, coordinates.Column] != ' ')
                {
                    Coordinates.PrintError("Position is already occupied. Please choose a different position.");
                    continue;
                }

                // Place the figure on the chessboard
                chessboard[coordinates.Row, coordinates.Column] = GetSymbol.GetSymbolChar(piece, color);
                break; // Exit the loop if the figure is placed successfully
            }
        }

    }



    private static bool PlaceSymbolOnBoard(string position, Figure figure, FigureColor color, char[,] chessboard)
    {
        Coordinates coordinates = new Coordinates(position);
        if (!Coordinates.IsValidCoordinates(coordinates))
        {
            Coordinates.PrintError($"Invalid input. Please enter a valid position for {color}{figure}.");
            return false;
        }
        else
        {
            chessboard[coordinates.Row, coordinates.Column] = GetSymbol.GetSymbolChar(figure, color);
            return true;
        }
    }
    public static bool IsNearbyPositionsEmpty(char[,] chessboard, Coordinates kingPosition)
    {
        int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };
        for (int i = 0; i < rowOffsets.Length; i++)
        {
            int newRow = kingPosition.Row + rowOffsets[i];
            int newCol = kingPosition.Column + colOffsets[i];
            Coordinates newPosition = new Coordinates(newRow, newCol);
            if (Coordinates.IsValidCoordinates(newPosition))
            {
                char pieceAtPosition = chessboard[newPosition.Row, newPosition.Column];
                if (pieceAtPosition != ' ')
                {
                    return false;
                }
            }
        }
        return true;
    }



    /// <summary>
    /// Find 1 piece coordinate
    /// </summary>
    /// <param name="chessboard"></param>
    /// <param name="piece"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private static Coordinates FindPiecePosition(char[,] chessboard, Figure piece, FigureColor color)
    {
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                if (chessboard[i, j] == GetSymbol.GetSymbolChar(piece, color))
                {
                    return new Coordinates(i, j);
                }
            }
        }

        // If the piece is not found, return an invalid position
        return new Coordinates(-1, -1);
    }


    /// <summary>
    /// Find all pieces coordinates
    /// </summary>
    /// <param name="chessboard"></param>
    /// <param name="piece"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private static List<Coordinates> FindAllPiecePositions(char[,] chessboard, Figure piece, FigureColor color)
    {
        List<Coordinates> positions = new List<Coordinates>();

        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                if (chessboard[i, j] == GetSymbol.GetSymbolChar(piece, color))
                {
                    positions.Add(new Coordinates(i, j));
                }
            }
        }

        return positions;
    }

    public static List<Coordinates> GetValidMovesForTeam(char[,] chessboard, FigureColor color)
    {
        List<Coordinates> validMoves = new List<Coordinates>();
        Piece[] pieces = new Piece[] { new King(), new Queen(), new Rook(), new Rook() };
        foreach (var piece in pieces)
        {
            List<Coordinates> pieceValidMoves = piece.GetEmptyChessboardValidMoves(chessboard, new Coordinates(-1, -1), color);
            validMoves.AddRange(pieceValidMoves);
        }
        return validMoves;
    }

    public static void PrintChessBoardWithColors(char[,] chessboard)
    {
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                if ((i + j) % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(chessboard[i, j] + " ");
            }
            Console.WriteLine();
        }

        // Reset console colors after printing the chessboard
        Console.ResetColor();
    }

}
