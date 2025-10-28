class Program
{
    public static void Main(string[] args)
    {
        Banco banco = new Banco("Banco");
        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("------ MENÚ BANCO ------");
            Console.WriteLine("1. Agregar cuenta");
            Console.WriteLine("2. Eliminar cuenta");
            Console.WriteLine("3. Listar clientes con más de una cuenta");
            Console.WriteLine("4. Realizar extracción");
            Console.WriteLine("5. Depositar dinero");
            Console.WriteLine("6. Transferir dinero");
            Console.WriteLine("7. Listar cuentas");
            Console.WriteLine("8. Listar clientes");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            Console.Write("Opción: ");
            opcion = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (opcion)         // utilizamos switch ya que nos permite facilitar el menu de opciones
            {
                case 1:
                    Console.Write("DNI del cliente: ");
                    int dni = int.Parse(Console.ReadLine());

                    // Busca al cliente (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cliente cliente = null;
                    foreach (Cliente c in banco.TodosLosClientes())
                    {
                        if (c.GetDni() == dni)
                        {
                            cliente = c;
                            break;
                        }
                    }

                    if (cliente == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usted es un cliente nuevo, debe ingresar los datos:");
                        Console.WriteLine();
                        Console.Write("Nombre: ");
                        string nombre = Console.ReadLine();
                        Console.Write("Apellido: ");
                        string apellido = Console.ReadLine();
                        Console.Write("Dirección: ");
                        string direccion = Console.ReadLine();
                        Console.Write("Teléfono: ");
                        int telefono = int.Parse(Console.ReadLine());
                        Console.Write("Mail: ");
                        string mail = Console.ReadLine();

                        cliente = new Cliente(nombre, apellido, dni, direccion, telefono, mail);
                        banco.AgregarCliente(cliente);

                        //Esto agrega al nuevo cliente en la lista
                    }

                    Console.Write("Saldo inicial: ");
                    double saldo = double.Parse(Console.ReadLine());

                    // Genera numero de cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    int nroCuenta = banco.CantidadCuentas() > 0 ?
                        banco.TodasLasCuentas().Max(c => c.GetNumero()) + 1 : 1;

                    Cuenta nueva = new Cuenta(nroCuenta, cliente, saldo);
                    banco.AgregarCuenta(nueva);

                    Console.WriteLine();
                    Console.WriteLine("Cuenta creada exitosamente. Nº de cuenta:" + nroCuenta);
                    break;

                case 2:
                    Console.Write("Número de cuenta: ");
                    int numero = int.Parse(Console.ReadLine());

                    // Busca la cuenta (cambiamos esto, antes lo teniamos en metodo como banco)

                    Cuenta cuenta = null;
                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.GetNumero() == numero)
                        {
                            cuenta = c;
                            break;
                        }
                    }

                    if (cuenta != null)
                    {
                        banco.EliminarCuenta(cuenta);

                        // Tiene mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                        bool tieneMasCuentas = banco.TodasLasCuentas()
                            .Count(c => c.GetTitular().GetDni() == cuenta.GetTitular().GetDni()) > 0;

                        if (!tieneMasCuentas)
                        {
                            banco.EliminarCliente(cuenta.GetTitular());
                            Console.WriteLine("Cliente eliminado (ya no tenía más cuentas).");
                        }

                        Console.WriteLine("Cuenta eliminada correctamente.");
                    }
                    else
                        Console.WriteLine("Cuenta no encontrada.");
                    break;

                case 3:

                    // Obtiene clientes con mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    var clientesRepetidos = banco.TodosLosClientes()
                        .Where(c => banco.TodasLasCuentas()
                        .Count(cta => cta.GetTitular().GetDni() == c.GetDni()) > 1)
                        .ToList();

                    if (clientesRepetidos.Count == 0)
                        Console.WriteLine("No hay clientes con más de una cuenta.");
                    else
                    {
                        foreach (var cli in clientesRepetidos)  //Hace un reccorido en la lista 
                        {
                            Console.WriteLine("Cliente:" + cli.GetApellido() + "," + cli.GetNombre() + "- DNI:" + cli.GetDni());
                            foreach (var cta in banco.TodasLasCuentas()
                                .Where(c => c.GetTitular().GetDni() == cli.GetDni()))
                            {
                                Console.WriteLine("Cuenta Nº" + cta.GetNumero() + "-" + "Saldo:" + cta.GetSaldo());
                            }
                        }
                    }
                    break;

                case 4:
                    Console.Write("Número de cuenta: ");
                    int nroExtraccion = int.Parse(Console.ReadLine());

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cuenta extraerDe = null;
                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.GetNumero() == nroExtraccion)
                        {
                            extraerDe = c;
                            break;
                        }
                    }

                    if (extraerDe == null)
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.Write("Monto a extraer: ");
                    double montoExt = double.Parse(Console.ReadLine());

                    try//Codigo que puede generar una excepcion
                    {
                        extraerDe.Extraer(montoExt);
                        Console.WriteLine("Nuevo saldo: " + extraerDe.GetSaldo());
                    }
                    catch (MontoinsuficienteExcepcion)      //Sirve para atrapar y manejar los errores que pueden ocurrir
                    {
                        Console.WriteLine("Saldo insuficiente.");
                    }
                    break;

                case 5:
                    Console.Write("Número de cuenta: ");
                    int nroDep = int.Parse(Console.ReadLine());

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cuenta depositarEn = null;
                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.GetNumero() == nroDep)
                        {
                            depositarEn = c;
                            break;
                        }
                    }

                    if (depositarEn == null)
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                        break;
                    }

                    Console.Write("Monto a depositar: ");
                    double montoDep = double.Parse(Console.ReadLine());
                    depositarEn.Depositar(montoDep);

                    Console.WriteLine("Nuevo saldo:" + depositarEn.GetNumero());
                    break;

                case 6:
                    Console.Write("Número de cuenta origen: ");
                    int nroOrigen = int.Parse(Console.ReadLine());
                    Console.Write("Número de cuenta destino: ");
                    int nroDestino = int.Parse(Console.ReadLine());
                    Console.Write("Monto: ");
                    double monto = double.Parse(Console.ReadLine());

                    // Transferir ahora se hace en el main (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cuenta origen = null;
                    Cuenta destino = null;

                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.GetNumero() == nroOrigen)
                            origen = c;
                        if (c.GetNumero() == nroDestino)
                            destino = c;
                    }

                    if (origen == null || destino == null)
                    {
                        Console.WriteLine("Error: una de las cuentas no existe.");
                        break;
                    }

                    try
                    {
                        origen.Extraer(monto);
                        destino.Depositar(monto);
                        Console.WriteLine("Transferencia exitosa.");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine("Saldo insuficiente en la cuenta origen.");
                    }
                    break;
                case 7:
                    foreach (var cta in banco.TodasLasCuentas())
                        Console.WriteLine("Cuenta Nº " + cta.GetNumero() + " |" + cta.GetTitular().GetApellido() + ", " + cta.GetTitular().GetNombre(), " | DNI:" + cta.GetTitular().GetDni(), "| Saldo:" + cta.GetSaldo());
                    break;
                case 8:
                    foreach (var cli in banco.TodosLosClientes())
                        Console.WriteLine(cli.GetNombre() + cli.GetApellido(), " - DNI:" + cli.GetDni(), " - Tel:" + cli.GetTelefono(), " - Mail:" + cli.GetMail());
                    break;

                default:
                    Console.WriteLine("opcion no valida");
                    break;


            }

        } while (opcion != 0);
    }
}
	

public class Banco
{
    private string nombre;
    private List<Cliente> clientes;
    private List<Cuenta> cuentas;

    public Banco(string nombre)
    {
        this.nombre = nombre;
        clientes = new List<Cliente>();
        cuentas = new List<Cuenta>();
    }

    // Metodos de acceso

    public string GetNombre() 
    {
        return nombre;
    }

    // Metodos simples para la clase cliente

    public void AgregarCliente(Cliente c)
    {
        clientes.Add(c);            // Add agrega un elemento a la lista
    }
    public void EliminarCliente(Cliente c)
    {
        clientes.Remove(c);         // Remove permite eleminar un elemento de la lista segun la posicion
    }

    public int CantidadClientes()
    {
        return clientes.Count;      // Propiedad que dice cuantos elementos hay en la lista
    }
    public Cliente VerCliente(int i)
    {
        return clientes[i];
    }
    public List<Cliente> TodosLosClientes()
    {
        return clientes;
    }

    // Metodos simples para la clase cuenta

    public void AgregarCuenta(Cuenta c)
    {
        cuentas.Add(c);
    }
    public void EliminarCuenta(Cuenta c)
    {
        cuentas.Remove(c);
    }
    public int CantidadCuentas()
    {
        return cuentas.Count;
    }
    public Cuenta VerCuenta(int i)
    {
        return cuentas[i];
    }
    public List<Cuenta> TodasLasCuentas()
    {
        return cuentas;
    }

}
public class Cuenta
{
    private int numero;
    private Cliente titular;
    private double saldo;

    public Cuenta(int numero, Cliente titular, double saldo)
    {
        this.numero = numero;
        this.titular = titular;
        this.saldo = saldo;
    }

    // Metodos de acceso

    public int GetNumero() 
    {
        return numero;
    }

    public Cliente GetTitular() 
    {
        return titular;
    }

    public double GetSaldo() 
    {
        return saldo;
    }

    // Metodos simples (depositar y extraer)

    public void Depositar(double monto)
    {
        if (monto <= 0) 
        {
            throw new Exception("El monto debe ser mayor que 0");
        }

        //El throw sirve para lanzar la excepcion

        saldo += monto;
    }

    public void Extraer(double monto)
    {
        if (monto > saldo)
        {
            throw new MontoinsuficienteExcepcion();
        }

        saldo -= monto;
    }
}
public class Cliente
{

    // Atributos definidos

    private string nombre;
    private string apellido;
    private int dni;
    private string direccion;
    private int telefono;
    private string mail;

    // Constructor
    public Cliente(string nombre, string apellido, int dni, string direccion, int telefono, string mail)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        this.direccion = direccion;
        this.telefono = telefono;
        this.mail = mail;
    }

    // Metodos de acceso

    public string GetNombre() 
    {
        return nombre;
    }

    public string GetApellido() 
    {
        return apellido;
    }

    public int GetDni() 
    {
        return dni;
    }

    public string GetDireccion() 
    {
        return direccion;
    }

    public int GetTelefono() 
    {
        return telefono;
    }

    public string GetMail() 
    {
        return mail;
    }

    public override string ToString()       //El override sirve para cambiar el comportamieno de un metodo heredado
    {
        return "Nombre y Apellido:" + nombre + "," + apellido + "DNI:" + dni;
    }
}
public class MontoinsuficienteExcepcion : Exception
{
    //Cuando creamos una excepcion, estamos utilizando herencia, ya que definimos una nueva clase que herada de la superclase excepcion
}
