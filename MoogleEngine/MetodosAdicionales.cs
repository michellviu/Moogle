using System;
using System.Text.RegularExpressions;

public static class MetodosAdicionales
{
    public static string[] ArrayQuery(string query) //metodo para convertir la busqueda en un array y normalizarla
    {
        string[] words = new string[query.Split(' ').Length];
        for (int i = 0; i < words.Length; i++)
        {
            query = query.ToLower();
            query = query.Replace("\r", " ");
            query = Regex.Replace(query, @"[^\w\s]", "");
            words = query.Split(' ');
            words = words.Distinct().ToArray();
            string[] items = new string[] { "en","la", "lo", "las", "los", "el", "a", "con", "de", "y", "o", "u", "e", "del", "ella", "the", "in", "you", "he", "she", "they","your" };
            for (int j = 0; j < items.Length; j++)
            {
                string item = items[j];
                words = words.Where(e => e != item).ToArray();
            }
        }
        return words;
    }
    public static string SubString(string[] array, int inicio, int fin, string termino)
    {
        string[] sub = new string[fin - inicio + 1];
        for (int i = 0; i < sub.Length; i++)
        {
            if (array[inicio + i] == termino)
                sub[i] = "\"" + array[inicio + i] + "\"";
            else
                sub[i] = array[inicio + i];
        }
        string snippet = string.Join(" ", sub);
        return snippet;
    }
}



