#!/bin/bash
# Funcion: Ejecutar el proyecto 
run() {
    echo "Ejecutando el proyecto ..."
    dotnet watch run --project MoogleServer
}
#Funcion: Compilar y generar el PDF del informe
report(){
    echo "Compilando el informe ..."
    pdflatex informe.tex
}
#Funcion: Compilar y generar el PDF de la presentacion
slides(){
    echo "Compilando la presentacion ..."
    pdflatex presentacion.tex
}
# Función: Ejecutar el visor de informe
show_report() {
  echo "Abriendo el visor de informe..."
  if [ -f informe.pdf ]; then
    start informe.pdf
  else
    echo "Generando el PDF del informe..."
    pdflatex informe.tex
    start informe.pdf
  fi
}
# Función: Ejecutar el visor de presentación
show_slides() {
  echo "Abriendo el visor de presentación..."
  if [ -f presentacion.pdf ]; then
    start presentacion.pdf
  else
    echo "Generando el PDF de la presentación..."
    pdflatex presentacion.tex
    start presentacion.pdf
  fi
}

# Función: Limpiar archivos auxiliares
clean() {
  echo "Limpiando archivos auxiliares..."
  rm -f *.aux *.log *.out *.toc
}
# Menú principal
menu() {
  PS3="Seleccione una opción: "
  options=("Run" "Report" "Slides" "Show_report" "Show_slides" "Clean" "Salir")
  select opt in "${options[@]}"; do
    case $opt in
      "Run")
        run
        ;;
      "Report")
        report
        ;;
      "Slides")
        slides
        ;;
      "Show_report")
        show_report
        ;;
      "Show_slides")
        show_slides
        ;;
      "Clean")
        clean
        ;;
      "Salir")
        break
        ;;
      *) echo "Opción inválida";;
    esac
  done
}

# Ejecutar el menú principal
menu