using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
public class Datos
{
    private static Datos datos;
    public string[] archivos { get; private set; }
    public Documentos[] resultadoDoc { get; private set; }
    public List<string> terminosunicos { get; private set; }
    public double[,] matrizTfIdf { get; private set; }
    public string relativePath { get; private set; }
    public string fullPath { get; private set; }
    private Datos()
    {   
        this.relativePath = "../Content";

        this.fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        this.archivos = Directory.GetFiles(fullPath, "*.txt");//Cargar los documentos

        this.resultadoDoc = new Documentos[this.archivos.Length];

        this.terminosunicos = new List<string>();

        this.matrizTfIdf = new double[terminosunicos.Count(), archivos.Length];
        Console.WriteLine("Cargando base de datos... "+DateTime.Now);
        this.Procesamiento();
        Console.WriteLine("Se terminó de cargar la base de datos "+DateTime.Now);
    }
    public static Datos getInstance()
    {
         if(datos != null)
        {
            return datos;
        }
        datos = new Datos();
        return datos;
    } 
    public void Procesamiento()
    {
        //Lista de Diccionarios que mientras se itera para buscar terminosunicos el guardara para cada documento
        //un diccionario que contiene como clave las palabras del documento y cuantas veces aparece
        List<Dictionary<string, int>> ListDicc = new List<Dictionary<string, int>>();
        for (int i = 0; i < archivos.Length; i++)
        {   //Variable Dicc que es el diccionario destinado a cada documento
            //y toma la misma posicion en la lista de dicho documento
            var Dicc = new Dictionary<string, int>();

            //Leer cada documento
            string contenido = File.ReadAllText(archivos[i]);

            //Array para el Snippet
            // string[] contenidosnippet = contenido.Replace("\t"," ").Replace("\r", " ").Replace("\n", " ").Split(" ");

            string[] alltext = MetodosAdicionales.Normaliza(contenido);

            var doc = new Documentos(archivos[i].Replace("\\", "/").Split("/").Last(), alltext);

            for (int j = 0; j < alltext.Length; j++)
            {
                int cantveces = 0;
                if (Dicc.TryGetValue(alltext[j], out cantveces))
                {
                    cantveces++;
                    Dicc[alltext[j]] = cantveces;

                }
                else
                {
                    Dicc.Add(alltext[j], 1);
                    /////////////
                    int snippetStart = Math.Max(0, j - 4);
                    int snippetEnd = Math.Min(alltext.Length - 1, j + 4);
                    string snippet = MetodosAdicionales.SubString(alltext, snippetStart, snippetEnd, alltext[j]);
                    doc.getDictionay().Add(alltext[j], snippet);
                    ////////////
                }

                if (!terminosunicos.Contains(alltext[j]))
                {
                    terminosunicos.Add(alltext[j]);
                }
            }

            resultadoDoc[i] = doc;
            //Se agrega el Diccionario del documento a la lista
            ListDicc.Add(Dicc);
        }

        //Crear Matriz de Terminos por Documentos(TF) y contar en cuantos documentos aparece cada terminounico
        //para posteriormente calcular el IDF
        double[,] matrizTF = new double[terminosunicos.Count(), archivos.Length];
        double[] idf = new double[terminosunicos.Count()];
        int[] cantdocumentos = new int[terminosunicos.Count()];

        for (int i = 0; i < archivos.Length; i++)
        {
            string contenido = File.ReadAllText(archivos[i]);
            string[] palabras = MetodosAdicionales.Normaliza(contenido);
            var DiccArchivo = ListDicc[i];
            for (int j = 0; j < terminosunicos.Count(); j++)
            {
                var terminounico = terminosunicos[j];
                int cont = 0;

                if (DiccArchivo.TryGetValue(terminounico, out cont))
                    cantdocumentos[j]++;
                matrizTF[j, i] = (double)cont / palabras.Length;

            }
        }
        //multiplicar la frecuencia de termino por documentos por la frecuencia inversa
        this.matrizTfIdf = new double[terminosunicos.Count(), archivos.Length];
        for (int i = 0; i < terminosunicos.Count(); i++)
        {
            idf[i] = Math.Log2((double)archivos.Length + 1 / cantdocumentos[i]);
            for (int j = 0; j < archivos.Length; j++)
            {
                matrizTfIdf[i, j] = matrizTF[i, j] * idf[i];
            }
        }
    }
}
