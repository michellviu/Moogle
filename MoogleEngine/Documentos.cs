using System;

public class Documentos
{
    string nombreDocumento;
    double score = 0;
    string[] contenido;
    string snippet;
    Dictionary<string, string> DiccSnippet;
    public Documentos(string nombreDoc, string[] contenido)
    {
        nombreDocumento = nombreDoc;
        snippet = "";
        this.contenido = contenido;
        this.DiccSnippet = new Dictionary<string, string>();
    }
    public void setSnippet(string snippet)
    {
        this.snippet = snippet;
    }
    public string getSnippet()
    {
        return this.snippet;
    }
    public void setScore(double score)
    {
        this.score = score;
    }
    public double getScore()
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
    public bool AddScore(double score)
    {
        double scoreAnterior = this.score;
        this.score += score;
        return this.score > scoreAnterior;
    }
    public void ResetScore()
    {
        this.score = 0;
    }

    public void ResetSnippet()
    {
        this.snippet = "";
    }

    public Dictionary<string, string> getDictionay()
    {
        return this.DiccSnippet;
    }

}
