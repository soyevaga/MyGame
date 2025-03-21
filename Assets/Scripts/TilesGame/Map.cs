
public class Map
{
    private int[,] matrix;
    private int[] arrows;
    public Map(int[,] matrix, int[] arrows)
    {
        this.matrix = matrix;
        this.arrows = arrows;
    }

    public int[,] GetMatrix()
    {
        return matrix;
    }

    public int[] GetArrows()
    {
        return arrows;
    }
}
