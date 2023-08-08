using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace MoogleEngine;
public static class Moogle
{
    public static SearchResult Query(string query, Datos p)
    {
        string[] busqueda = MetodosAdicionales.Normaliza(query);
        //Crear vector consulta
        double[] vectorcosulta = new double[p.terminosunicos.Count()];
        for (int i = 0; i < p.terminosunicos.Count(); i++)
        {
            var terminounico = p.terminosunicos[i];
            int cont = 0;
            for (int j = 0; j < busqueda.Length; j++)
            {
                if (busqueda[j].Equals(terminounico))
                {
                    cont++;
                }
            }

            vectorcosulta[i] = (double)cont / busqueda.Length;
        }
        //Calcular similitud entre la consulta y cada uno de los documentos
        //multiplicando la matrizTfIdf por vector consulta

        List<Documentos> a = new List<Documentos>();

        for (int i = 0; i < p.archivos.Length; i++)
        {
            p.resultadoDoc[i].ResetScore();
            p.resultadoDoc[i].ResetSnippet();
            double productoEscalar = 0.0;
            double longitudDocumento = 0.0;
            double longitudQuery = 0.0;
            for (int j = 0; j < p.terminosunicos.Count(); j++)
            {
                productoEscalar += p.matrizTfIdf[j, i] * vectorcosulta[j];
                longitudDocumento += Math.Pow(p.matrizTfIdf[j, i], 2);
                longitudQuery += Math.Pow(vectorcosulta[j], 2);
                if (p.resultadoDoc[i].AddScore((p.matrizTfIdf[j, i] * vectorcosulta[j])))
                {
                    p.resultadoDoc[i].AddSnippet(p.resultadoDoc[i].getDictionay()[p.terminosunicos[j]]);
                }
            }
            longitudDocumento = Math.Sqrt(longitudDocumento);
            longitudQuery = Math.Sqrt(longitudQuery);
            p.resultadoDoc[i].setScore(productoEscalar / (longitudDocumento * longitudQuery));
            if (p.resultadoDoc[i].getScore() > 0)
            {
                a.Add(p.resultadoDoc[i]);
            }
        }
        //Ordenar los documentos por similitud
        a.Sort((d1, d2) =>
            {
                return d2.getScore().CompareTo(d1.getScore());
            });
        SearchItem[] items = new SearchItem[a.Count()];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SearchItem(a[i].getNombre(), a[i].getSnippet(), (float)a[i].getScore());
        }
        ///////////////////////////////////////
        return new SearchResult(items, query);
    }
}
