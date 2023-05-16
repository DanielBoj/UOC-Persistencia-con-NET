using GenteFit.Models.Prototypes;
using GenteFit.Models.Usuarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GenteFit.Models.Enums;
using System.Text;
using GenteFitBackup.Models;
using System.Globalization;
using System.Text.Json.Nodes;

namespace GenteFitOdoo.Models.Repositories
{
    /* Esta clase contiene toda la lógica para conectarse a la API de Python y realizar las peticiones
     *      necesarias para obtener los datos de la BBDD de Odoo. */
    public class PythonApiConnection
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        // Constructor
        public PythonApiConnection(HttpClient client)
        {
            _httpClient = client;
            _baseUrl = "http://localhost:5005/";
            client.BaseAddress = new Uri(_baseUrl);
        }

        /* Métodos para obtener los datos de la BBDD de Odoo */
        /* Para consumir la api vamos a conectarnos con el endpoint que necesitemos a través de la url
         *         *      http://localhost:5005/endpoint. */

        /* Clientes */

        // Método para obtener todos los clientes de la BBDD de Odoo
        public async Task<List<Cliente>> GetAllClientes()
        {
            try
            {
                // Consumimos la API de Python
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}clientes");

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserializamos el objeto JSON
                if (responseBody != null)
                {
                    // Convertimos los datos en un array de objetos JSON
                    JArray data = JArray.Parse(responseBody);

                    // Creamos una lista de clientes
                    List<Cliente> clientes = new();

                    // Recorremos el array de objetos JSON
                    foreach (JObject item in data.Cast<JObject>())
                    {
                        // Creamos un nuevo cliente
                        Cliente cliente = new Cliente
                        {
                            // Asignamos los valores
                            Nombre = item["name"]?.ToObject<string>(),
                            Nif = item["vat"]?.ToObject<string>(),
                            Email = item["email"]?.ToObject<string>(),
                            // Asignamos un password por defecto
                            Pass = "claptrap",
                            Telefono = item["phone"]?.ToObject<string>(),
                            Direccion = new Direccion
                            {
                                Domicilio = item["street"]?.ToObject<string>(),
                                Poblacion = item["city"]?.ToObject<string>(),
                                Pais = item["country"]?.ToObject<string>(),
                                Cp = (int)item["zip"]?.ToObject<int>()
                            },
                            Genero = Genero.No_Definido,
                            Iban = "ES3600112233445566778899",
                            Tipo = "cliente"

                        };
                        // Añadimos el cliente a la lista
                        clientes.Add(cliente);
                    }

                    // Devolvemos la lista de clientes
                    return clientes;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Si no, devolvemos una lista vacía
            return new List<Cliente>();
        }

        // Creamos un cliente, tendremos que serialziar los atributos para generar un JSON
        public async Task<int> CreateCliente(Cliente cliente)
        {
            // Por parámetro recibimos un cliente, vamos a convertirlo en un objeto JSON
            JObject data = new()
            {
                { "name", cliente.Nombre },
                { "vat", cliente.Nif },
                { "email", cliente.Email },
                { "phone", cliente.Telefono },
                { "street", cliente.Direccion.Domicilio },
                { "city", cliente.Direccion.Poblacion },
                { "country", cliente.Direccion.Pais },
                { "zip", cliente.Direccion.Cp }
            };

            // Consumimos la API de Python
            try
            {
                // Tenemos que enviar un POST con un objeto JSON a la API de Python. El objeto JSON tiene que tener el mismo formato que el que espera la API.
                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}clientes", new StringContent(data.ToString(), Encoding.UTF8, "application/json"));

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();

                // Convertimos los datos en un objeto JSON
                JObject dataResponse = JObject.Parse(responseBody);

                // Devolvemos el id del cliente creado o -1 si ha habido algún problema
                return dataResponse["cliente_id"]?.ToObject<int>() ?? -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Si ha habido un error, devolvemos -1
            return -1;
        }

        /* Productos */

        // Método para obtener todos los productos de la BBDD de Odoo
        public async Task<List<Producto>> GetAllProductos()
        {
            try
            {
                // Consumimos la API de Python
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}productos");

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserializamos el objeto JSON
                if (responseBody != null)
                {
                    // COnvertimos los datos en un array de objetos JSON
                    JArray data = JArray.Parse(responseBody);

                    // Creamos una lista de productos
                    List<Producto> productos = new();

                    // Recorremos el array de objetos 
                    foreach (JObject item in data.Cast<JObject>())
                    {
                        // Creamos un nuevo producto
                        Producto producto = new Producto
                        {
                            // Asignamos los valores
                            Id = item["id"]?.ToObject<int>(),
                            DefaultCode = item["default_code"]?.ToObject<string>(),
                            Name = item["name"]?.ToObject<string>(),
                            Categ = item["categ"]?.ToObject<string>(),
                            ListPrice = item["list_price"]?.ToObject<decimal>(),
                            StandardPrice = item["standard_price"]?.ToObject<decimal>(),
                        };

                        // Añadimos el producto a la lista
                        productos.Add(producto);
                    }

                    // Devolvemos la lista de productos
                    return productos.Count > 0 ? productos : new List<Producto>();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            // Si no, devolvemos una lista vacía
            return new List<Producto>();
        }

        // Método para crear un producto en la BBDD de Odoo
        public async Task<int> CreateProducto(Producto producto)
        {
            // Por parámetro recibimos un producto, vamos a convertirlo en un objeto JSON
            JObject data = new()
            {
                { "default_code", producto.DefaultCode },
                { "name", producto.Name },
                { "categ", producto.Categ },
                { "list_price", producto.ListPrice },
                { "standard_price", producto.StandardPrice },
            };

            // Consumimos la API de Python
            try
            {
                // Tenemos que enviar un POST con un objeto JSON a la API de Python. El objeto JSON tiene que tener el mismo formato que el que espera la API.
                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}productos", new StringContent(data.ToString(), Encoding.UTF8, "application/json"));

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();

                // Convertimos los datos en un objeto JSON
                JObject dataResponse = JObject.Parse(responseBody);

                // Devolvemos el id del producto creado o -1 si ha habido algún problema
                return dataResponse["producto_id"]?.ToObject<int>() ?? -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Si ha habido un error, devolvemos -1
            return -1;
        }

        /* Proveedores */

        // Método para obtener todos los proveedores de la BBDD de Odoo
        public async Task<List<Proveedor>> GetAllProveedores()
        {
            try
            {
                // Consumimos los datos de la API de Python
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}proveedores");

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserializamos el objeto JSON
                if (responseBody != null)
                {
                    // Convertimos los datos en un Array de objetos JSON
                    JArray data = JArray.Parse(responseBody);

                    // Creamos una lista de proveedores
                    List<Proveedor> proveedores = new();

                    // Recorremos el array de objetos
                    foreach (JObject item in data.Cast<JObject>())
                    {
                        // Creamos un nuevo proveedor
                        Proveedor proveedor = new Proveedor
                        {
                            // Asignamos los valores
                            Id = item["id"]?.ToObject<int>(),
                            Name = item["name"]?.ToObject<string>(),
                            Nif = item["vat"]?.ToObject<string>(),
                            Direccion = new()
                            {
                                // Asignamos los valores
                                Domicilio = item["street"].ToObject<string>(),
                                Poblacion = item["city"].ToObject<string>(),
                                Pais = item["country"].ToObject<string>(),
                                Cp = (int)item["zip"]?.ToObject<int>()
                            },
                            Phone = item["phone"]?.ToObject<string>(),
                            Email = item["email"]?.ToObject<string>(),
                            Website = item["website"]?.ToObject<string>()
                        };

                        // Añadimos el proveedor a la lista
                        proveedores.Add(proveedor);
                    }

                    // Devolvemos la lista de proveedores
                    return proveedores.Count > 0 ? proveedores : new List<Proveedor>();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            // Si no, devolvemos una lista vacía
            return new List<Proveedor>();
        }

        // Método para crear un proveedor en la BBDD de Odoo
        public async Task<int> CreateProveedor(Proveedor proveedor)
        {
            // Por parámetro recibimos un proveedor que hay que convertir en un objeto JSON
            JObject data = new()
            {
                { "name", proveedor.Name },
                { "vat", proveedor.Nif },
                { "street", proveedor.Direccion?.Domicilio },
                { "city", proveedor.Direccion?.Poblacion },
                { "country", proveedor.Direccion?.Pais },
                { "zip", proveedor.Direccion?.Cp },
                { "phone", proveedor.Phone },
                { "email", proveedor.Email },
                { "website", proveedor.Website }
            };

            // Consumimos la API de Python
            try
            {
                // Tenemos que enviar un POST con un objeto JSON a la API de Python. El objeto JSON tiene que tener el mismo formato que el que espera la API.
                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}proveedores", new StringContent(data.ToString(), Encoding.UTF8, "application/json"));

                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();

                // Leemos el cuerpo del retorno y deserializamos los datos
                string responseBody = await response.Content.ReadAsStringAsync();

                // Convertimos los datos en un objeto JSON
                JObject dataResponse = JObject.Parse(responseBody);

                return dataResponse["proveedor_id"]?.ToObject<int>() ?? -1;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            // Si ha habido un error, devolvemos -1
            return -1;
        }

        /* Empleados */
        // Obtenemos una lista de empleados
        public async Task<List<EmpleadoOdoo>> GetAllEmpleados()
        {
            try
            {
                // Consumimos los datos de la API de Python
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}empleados");
                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();
                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserializamos el objeto JSON
                if (responseBody != null)
                {
                    // Convertimos los datos en un Array de objetos JSON
                    JArray data = JArray.Parse(responseBody);
                    // Creamos una lista de empleados
                    List<EmpleadoOdoo> empleados = new();
                    // Recorremos el array de objetos
                    foreach (JObject item in data.Cast<JObject>())
                    {
                        // Creamos un nuevo empleado
                        EmpleadoOdoo empleado = new EmpleadoOdoo
                        {
                            // Asignamos los valores
                            Id = item["id"]?.ToObject<int>(),
                            Name = item["name"]?.ToObject<string>(),
                            Email = item["email"]?.ToObject<string>(),
                            Department = item["department"]?.ToObject<string>(),
                        };
                        // Añadimos el empleado a la lista
                        empleados.Add(empleado);
                    }
                    // Devolvemos la lista de empleados
                    return empleados.Count > 0 ? empleados : new List<EmpleadoOdoo>();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            // Si no, devolvemos una lista vacía
            return new List<EmpleadoOdoo>();    
        }

        /* Pedidos */
        // Obtenemos una lista de pedidos
        public async Task<List<Pedido>> GetAllPedidos()
        {
            try
            {
                // Consumimos los datos de la API de Python
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}pedidos");
                // Comprobamos que el retorno sea correcto, si el response no es correcto, devuelve una excepción
                response.EnsureSuccessStatusCode();
                // Leemos el cuerpo
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserializamos el objeto JSON
                if (responseBody != null)
                {
                    // Convertimos los datos en un Array de objetos JSON
                    JArray data = JArray.Parse(responseBody);
                    // Creamos una lista de pedidos
                    List<Pedido> pedidos = new();
                    // Recorremos el array de objetos
                    foreach (JObject item in data.Cast<JObject>())
                    {
                        // Intentamos parsear la fecha, si no, devolvemos una fecha vacía
                        DateTime fecha;
                        try
                        {
                            fecha = DateTime.TryParseExact(item["date_order"]?.ToObject<string>(), "yyyy-MM-dd HH:mm:ss", 
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha) ? fecha : new DateTime();
                        }
                        catch (Exception)
                        {
                            fecha = new DateTime();
                        }


                        // Creamos un nuevo pedido
                        Pedido pedido = new()
                        {
                            // Asignamos los valores
                            Id = item["id"]?.ToObject<int>(),
                            DisplayName = item["display_name"]?.ToObject<string>(),
                            DateOrder = fecha,
                            Company = item["company"]?.ToObject<string>(),
                            Partner = item["partner"]?.ToObject<string>(),
                            AmountUntaxed = item["amount_untaxed"]?.ToObject<double>(),
                            AmountTotal = item["amount_total"]?.ToObject<double>(),
                            TaxGroupName = item["tax_group_name"]?.ToObject<string>(),
                            DeliveryStatus = item["delivery_status"]?.ToObject<string>() ?? "false"
                        };
                        // Añadimos el pedido a la lista
                        pedidos.Add(pedido);
                    }
                    // Devolvemos la lista de pedidos
                    return pedidos.Count > 0 ? pedidos : new List<Pedido>();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            // Si no, devolvemos una lista vacía
            return new List<Pedido>();
        }
    }
}
