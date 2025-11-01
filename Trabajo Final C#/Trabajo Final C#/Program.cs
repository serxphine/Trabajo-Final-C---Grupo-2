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
            Console.WriteLine();
            Console.WriteLine("1. Agregar cuenta");
            Console.WriteLine("2. Eliminar cuenta");
            Console.WriteLine("3. Listar clientes con más de una cuenta");
            Console.WriteLine("4. Realizar extracción");
            Console.WriteLine("5. Depositar dinero");
            Console.WriteLine("6. Transferir dinero");
            Console.WriteLine("7. Listar cuentas");
            Console.WriteLine("8. Listar clientes");
            Console.WriteLine();
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
                    foreach (Cliente c in banco.ListaClientes)
                    {
                        if (c.Dni == dni)
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

                    Console.WriteLine();
                    Console.Write("Saldo inicial: ");
                    double saldo = double.Parse(Console.ReadLine());

                    // Genera numero de cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    int nroCuenta = banco.CantidadCuentas() > 0 ?
                        banco.TodasLasCuentas().Max(c => c.Numero) + 1 : 1;

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
                        if (c.Numero == numero)
                        {
                            cuenta = c;
                            break;
                        }
                    }

                    if (cuenta != null)
                    {
                        banco.EliminarCuenta(cuenta);

                        // Tiene mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                        bool tieneMasCuentas = banco.TodasLasCuentas().Count(c => c.Titular.Dni == cuenta.Titular.Dni) > 0;

                        if (!tieneMasCuentas)
                        {
                            banco.EliminarCliente(cuenta.Titular);
                            Console.WriteLine();
                            Console.WriteLine("Cliente eliminado (ya no tenía más cuentas).");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Cuenta eliminada correctamente.");
                    }
                    else 
                    {
                        Console.WriteLine("Cuenta no encontrada.");
                    }
                
                    break;

                case 3:

                    // Obtiene clientes con mas de una cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    List<Cliente> clientesRepetidos = new List<Cliente>();

                    foreach (Cliente cli in banco.TodosLosClientes())
                    {
                        int cantidadCuentas = 0;

                        foreach (Cuenta cta in banco.TodasLasCuentas())
                        {
                            if (cta.Titular.Dni == cli.Dni) 
                            {
                                cantidadCuentas++;
                            }
                 
                        }

                        if (cantidadCuentas > 1) 
                        {
                            clientesRepetidos.Add(cli);
                        }
   
                    }

                    if (clientesRepetidos.Count == 0)
                    {
                        Console.WriteLine("No hay clientes con más de una cuenta.");
                    }

                    else
                    {
                        // Recorre los clientes con mas de una cuenta

                        foreach (Cliente cli in clientesRepetidos)
                        {
                            Console.WriteLine("Cliente: " + cli.Nombre + " " + cli.Apellido + " - DNI: " + cli.Dni);
                            Console.WriteLine();

                            // Muestra las cuentas de ese cliente

                            foreach (Cuenta cta in banco.TodasLasCuentas())
                            {
                                if (cta.Titular.Dni == cli.Dni)
                                    Console.WriteLine("  Cuenta Nº " + cta.Numero + " - Saldo: " + cta.Saldo);
                            }

                        }
                    }
                    break;

                case 4:

                    Console.Write("Número de cuenta: ");
                    int nroExtraccion = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cuenta extraerDe = null;
                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.Numero == nroExtraccion)
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
                    Console.WriteLine();

                    try //Codigo que puede generar una excepcion
                    {
                        extraerDe.Extraer(montoExt);
                        Console.WriteLine("Nuevo saldo: " + extraerDe.Saldo);
                    }
                    catch (MontoinsuficienteExcepcion)      //Sirve para atrapar y manejar los errores que pueden ocurrir
                    {
                        Console.WriteLine();
                        Console.WriteLine("Saldo insuficiente.");
                    }
                    break;

                case 5:

                    Console.Write("Número de cuenta: ");
                    int nroDep = int.Parse(Console.ReadLine());
                    Console.WriteLine();

                    // Busca Cuenta (cambiamos esto, antes lo teniamos como metodo de banco)

                    Cuenta depositarEn = null;
                    foreach (Cuenta c in banco.TodasLasCuentas())
                    {
                        if (c.Numero == nroDep)
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
                    Console.WriteLine();
                    depositarEn.Depositar(montoDep);

                    Console.WriteLine("Nuevo saldo:" + depositarEn.Saldo);
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
                        if (c.Numero == nroOrigen) 
                        {
                            origen = c;
                        }

                        if (c.Numero == nroDestino) 
                        {
                            destino = c;
                        }
     
                    }

                    if (origen == null || destino == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: una de las cuentas no existe.");
                        break;
                    }

                    try
                    {
                        origen.Extraer(monto);
                        destino.Depositar(monto);
                        Console.WriteLine();
                        Console.WriteLine("Transferencia exitosa.");
                    }
                    catch (MontoinsuficienteExcepcion)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Saldo insuficiente en la cuenta origen.");
                    }
                    break;

                case 7:

                    foreach (var cta in banco.ListaCuentas) 
                    {
                        Console.WriteLine("Cuenta Nº " + cta.Numero + " | " + cta.Titular.Apellido + " " + cta.Titular.Nombre + " | DNI: " + cta.Titular.Dni, " | Saldo: " + cta.Saldo);
                    }
     
                    break;

                case 8:

                    foreach (var cli in banco.TodosLosClientes()) 
                    {
                        Console.WriteLine("Cliente: " + cli.Nombre + cli.Apellido + " - DNI: " + cli.Dni + " - Tel: " + cli.Telefono + " - Mail: " + cli.Mail);
                    }
                        
                    break;

                default:

                    Console.WriteLine("Opcion no valida");
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

    // Propiedades

    public string Nombre 
    {
        get { return nombre; } set { nombre = value; }
    }
    public List<Cliente> ListaClientes => clientes;
    public List<Cuenta> ListaCuentas => cuentas;


    // Constructor 

    public Banco(string nombre)
    {
        this.nombre = nombre;
        clientes = new List<Cliente>();
        cuentas = new List<Cuenta>();
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

    // Propiedades

    public int Numero
    {
        get { return numero; } set { numero = value; }
    }

    public Cliente Titular
    {
        get { return titular; } set { titular = value; }
    }

    public double Saldo
    {
        get { return saldo; } set { saldo = value; }
    }

    // Constructor

    public Cuenta(int numero, Cliente titular, double saldo)
    {
        this.numero = numero;
        this.titular = titular;
        this.saldo = saldo;
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

    public override string ToString() 
    {
        return $"Numero: {numero} - Titular: {titular} - Saldo: {saldo}";
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

    // Propiedades

    public string Nombre 
    {
        get { return nombre; } set { nombre = value; }
    }

    public string Apellido 
    {
        get { return apellido; } set { apellido = value; }
    }

    public int Dni 
    {
        get { return dni; } set { dni = value; }
    }

    public string Direccion 
    {
        get { return direccion; } set { direccion = value; }
    }

    public int Telefono 
    {
        get { return telefono; } set { telefono = value; }
    }

    public string Mail 
    {
        get { return mail; } set { mail = value; }
    }

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

    public override string ToString()       //El override sirve para cambiar el comportamieno de un metodo heredado
    {
        return "Nombre y Apellido:" + nombre + "," + apellido + "DNI:" + dni;
    }
}
public class MontoinsuficienteExcepcion : Exception
{
    //Cuando creamos una excepcion, estamos utilizando herencia, ya que definimos una nueva clase que herada de la superclase excepcion
}
