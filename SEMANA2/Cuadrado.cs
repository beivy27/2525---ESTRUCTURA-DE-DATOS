// Clase que representa un cuadrado y permite calcular su área y perímetro
public class Cuadrado
{
    private double lado;

    public Cuadrado(double l)
    {
        lado = l;
    }

    public double CalcularArea()
    {
        return lado * lado;
    }

    public double CalcularPerimetro()
    {
        return 4 * lado;
    }
}
