// Clase que representa un círculo y permite calcular su área y perímetro
public class Circulo
{
    private double radio;

    public Circulo(double r)
    {
        radio = r;
    }

    // CalcularArea es una función que devuelve un valor double,
    // se utiliza para calcular el área de un círculo, requiere como argumento el radio del círculo
    public double CalcularArea()
    {
        return Math.PI * radio * radio;
    }

    public double CalcularPerimetro()
    {
        return 2 * Math.PI * radio;
    }
}
