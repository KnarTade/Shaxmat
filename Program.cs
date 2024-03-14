using CheckmateLibrary;
using CheckmateLibrary.Pieces;
using CheckmateLibrary.Structs;

namespace Checkmate;

internal class Program
{
    static void Main()
    {
        char[,] chessboard;
        bool isBoardValid;

        do
        {
            chessboard = BoardPrint.InitializeChessboard();
            BoardPrint.PrintChessBoard(chessboard);
            isBoardValid = BoardPrint.SetPieces(chessboard);
        } while (!isBoardValid);

        BoardPrint.PrintChessBoard(chessboard);

        var blackHitCoords = GetHitCoords(chessboard, FigureColor.Black);
        var whiteHitCoords = GetHitCoords(chessboard, FigureColor.White);
    }

    private static List<Coordinates> GetHitCoords(char[,] chessboard, FigureColor color)
    {
        IEnumerable<Coordinates>  allHitCoords = new List<Coordinates>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece figure = GetFigure(chessboard[i, j]);
                if (true )//figure.Color == color)
                {
                    var hitCoords = figure.GetEmptyChessboardValidMoves(chessboard, new Coordinates(i,j),color);
                    allHitCoords = allHitCoords.Union(hitCoords);
                }
            }
        }

        return allHitCoords.ToList();
    }

    private static Piece GetFigure(char v)
    {
        throw new NotImplementedException();
    }
}


