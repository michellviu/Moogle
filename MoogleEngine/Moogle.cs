using System;
using System.IO;
using System.Text.RegularExpressions;
namespace MoogleEngine;
public static class Moogle
{
    public static SearchResult Query(string query)
    {
        /////////////////////////////////////
        string relativePath = "../Content";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(),relativePath);
        string[] archivos = Directory.GetFiles(fullPath,"*.txt"); //Cargar los documentos

        var terminosBUscados = MetodosAdicionales.ArrayQuery(query);

        //Lista que contendra el listado de documentos con sus palabras y cada una con su TF
        var resultadoDoc = new List<Documentos>();
        //Variable que contendra los terminos buscados y en cuantos documentos aparece
        var palabrasIDF = new Dictionary<string, int>();

        for (int i = 0; i < archivos.Length; i++)
        {   //Leer cada documento
            string contenido = File.ReadAllText(archivos[i]);
            // Normalizar convirtiendo todo el texto a minúsculas, eliminar saltos de linea, caracteres especiales
            contenido = contenido.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            contenido = Regex.Replace(contenido, @"[^\w\s]", "");
            contenido = contenido.Replace("\n", " ");

            // documentos que se almacenara en la lista de resultadoDoc

            var alltext = contenido.Split(" ");
            var doc = new Documentos(archivos[i], alltext.Length, alltext);
            bool flag = false;
            //Guardar el idf de cada palabra

            for (int j = 0; j < terminosBUscados.Length; j++)
            {
                var terminoBuscado = terminosBUscados[j];
                int cont = 0;
                // si encuentra el termino marca que lo encontro y cuenta
                int aux = -1;
                for (int k = 0; k < alltext.Length; k++)
                {

                    if (alltext[k].Equals(terminoBuscado))
                    {
                        cont++;
                        if (aux == -1)
                        {
                            aux = k;
                            int snippetStart = Math.Max(0, k - 4);
                            int snippetEnd = Math.Min(doc.getContenido().Length - 1, k + 4);
                            string snippet = MetodosAdicionales.SubString(alltext, snippetStart, snippetEnd, terminoBuscado);
                            doc.AddSnippet(snippet);
                        }
                    }
                }
                if (cont > 0)
                {
                    flag = true;
                    //Actualizar diccionario del dodumento para el termino buscado TF
                    doc.getDictionay().Add(terminoBuscado, cont);
                    //ACTUALIZAR IDF
                    int cantDocumentos;
                    if (palabrasIDF.TryGetValue(terminoBuscado, out cantDocumentos))
                    {
                        cantDocumentos++;//aumentando en uno
                        palabrasIDF[terminoBuscado] = cantDocumentos;
                    }
                    else
                    {
                        palabrasIDF.Add(terminoBuscado, 1);
                    }
                }
            }
            if (flag)
                resultadoDoc.Add(doc);
        }
        //Recorrer lista de resultadoDoc y por cada elemento hacer el calculo del score de su Query
        int idf;
        int tf;
        resultadoDoc.ForEach(d =>
        {
            float score = 0;
            for (int i = 0; i < terminosBUscados.Length; i++)
            {
                d.getDictionay().TryGetValue(terminosBUscados[i], out tf);
                palabrasIDF.TryGetValue(terminosBUscados[i], out idf);
                if (tf > 0)
                    score += ((float)tf / d.getCantidadDePalabras()) * (float)Math.Log2((float)archivos.Length / idf);
                else score += 0;
            }
            d.setScore(score);
        });

        resultadoDoc.Sort((d1, d2) =>
        {
            return d1.getScore().CompareTo(d2.getScore());
        });
        resultadoDoc.Reverse();
        SearchItem[] items = new SearchItem[7];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SearchItem(resultadoDoc[i].getNombre().Replace("\\", "/").Split("/").Last(), resultadoDoc[i].getSnippet(), resultadoDoc[i].getScore());
        }
        ///////////////////////////////////////
        return new SearchResult(items, query);
    }
}
