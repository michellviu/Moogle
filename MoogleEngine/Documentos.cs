using System;

public class Documentos
{
    string nombreDocumento ;
    float score = 0;
    int cantidadDePalabras;
    string[] contenido;
    string snippet;
    Dictionary<string, int> palabrasTF;
    public Documentos(string nombreDoc, int longitud, string[] contenido)
    {
        nombreDocumento = nombreDoc;
        palabrasTF = new Dictionary<string, int>();
        cantidadDePalabras = longitud;
        snippet = "";
        this.contenido = contenido;
    }
    public string[] getContenido()
    {
        return this.contenido;
    }
    public void setSnippet(string snippet)
    {
        this.snippet = snippet;
    }
    public string getSnippet()
    {
        return this.snippet;
    }
    public void setScore(float score)
    {
        this.score = score;
    }
    public int getCantidadDePalabras()
    {
        return this.cantidadDePalabras;
    }
    public Dictionary<string, int> getDictionay()
    {
        return this.palabrasTF;
    }

    public float getScore()
    {
        return this.score;
    }

    public string getNombre()
    {
        return this.nombreDocumento;
    }
    public void AddSnippet(string snippet)
    {
        if (this.snippet == "")
            this.snippet = snippet;
        else
            this.snippet += "..." + snippet;
    }
}
