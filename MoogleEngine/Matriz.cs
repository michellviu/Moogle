using System;
public class Matriz
{
    private double[,] data;
    public Matriz(double[,] data)
    {
        this.data = data;
    }
    public static Matriz SumaMatriz(Matriz a, Matriz b)
    {
        if (a.data.GetLength(0) != b.data.GetLength(0) || a.data.GetLength(1) != b.data.GetLength(1))
        {
            throw new ArgumentException("Matrices must have the same dimensions");
        }

        double[,] result = new double[a.data.GetLength(0), a.data.GetLength(1)];

        for (int i = 0; i < a.data.GetLength(0); i++)
        {
            for (int j = 0; j < a.data.GetLength(1); j++)
            {
                result[i, j] = a.data[i, j] + b.data[i, j];
            }
        }
        return new Matriz(result);
    }
    public static Matriz restaMatriz(Matriz a, Matriz b)
    {
        if (a.data.GetLength(0) != b.data.GetLength(0) || a.data.GetLength(1) != b.data.GetLength(1))
        {
            throw new ArgumentException("Matrices must have the same dimensions.");
        }

        double[,] result = new double[a.data.GetLength(0), a.data.GetLength(1)];

        for (int i = 0; i < a.data.GetLength(0); i++)
        {
            for (int j = 0; j < a.data.GetLength(1); j++)
            {
                result[i, j] = a.data[i, j] - b.data[i, j];
            }
        }

        return new Matriz(result);
    }
    public static Matriz MultiplicaMatriz(Matriz a, Matriz b)
    {
        if (a.data.GetLength(1) != b.data.GetLength(0))
        {
            throw new ArgumentException("Number of columns in matrix A must match number of rows in matrix B.");
        }

        double[,] result = new double[a.data.GetLength(0), b.data.GetLength(1)];

        for (int i = 0; i < a.data.GetLength(0); i++)
        {
            for (int j = 0; j < b.data.GetLength(1); j++)
            {
                double sum = 0;

                for (int k = 0; k < a.data.GetLength(1); k++)
                {
                    sum += a.data[i, k] * b.data[k, j];
                }

                result[i, j] = sum;
            }
        }

        return new Matriz(result);
    }

    public static Matriz EscalarMatriz(double scalar, Matriz matrix)
    {
        double[,] result = new double[matrix.data.GetLength(0), matrix.data.GetLength(1)];

        for (int i = 0; i < matrix.data.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.data.GetLength(1); j++)
            {
                result[i, j] = scalar * matrix.data[i, j];
            }
        }
        return new Matriz(result);
    }

    public static double[] VectorMatriz(double[] vector, double[,] matrix)
    {
        int vectorLength = vector.Length;
        int matrixCols = matrix.GetLength(1);
        double[] result = new double[matrixCols];

        for (int i = 0; i < matrixCols; i++)
        {
            double sum = 0;
            for (int j = 0; j < vectorLength; j++)
            {
                sum += vector[j] * matrix[j, i];
            }
            result[i] = sum;
        }

        return result;
    }
    public static double MultiplicaVectores(double[] vector1, double[] vector2)
    {
        int vectorLength = vector1.Length;
        double result = 0;

        for (int i = 0; i < vectorLength; i++)
        {
            result += vector1[i] * vector2[i];
        }
        return result;
    }
    public static double[] SumaVectores(double[] vector1, double[] vector2)
    {
        int vectorLength = vector1.Length;
        double[] result = new double[vectorLength];

        for (int i = 0; i < vectorLength; i++)
        {
            result[i] = vector1[i] + vector2[i];
        }

        return result;
    }
    public static double[] RestaVectores(double[] vector1, double[] vector2)
    {
        int vectorLength = vector1.Length;
        double[] result = new double[vectorLength];

        for (int i = 0; i < vectorLength; i++)
        {
            result[i] = vector1[i] - vector2[i];
        }
        return result;
    }

    public static double[] MultiplicaEscalarVector(double scalar, double[] vector)
    {
        int vectorLength = vector.Length;
        double[] result = new double[vectorLength];

        for (int i = 0; i < vectorLength; i++)
        {
            result[i] = scalar * vector[i];
        }

        return result;
    }
}



