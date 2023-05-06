using Amazon.Runtime.Internal;
using GenteFit.Models.Prototypes;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GenteFit.Models.Enums;

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
                            Iban = "",
                            Tipo = "cliente"

                        };
                        // Añadimos el cliente a la lista
                        clientes.Add(cliente);
                    }

                    // Devolvemos la lista de clientes
                    return clientes;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Si no, devolvemos una lista vacía
            return new List<Cliente>();
        }

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
                            Seller = item["seller"]?.ToObject<string>()
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
                            Direccion = new()
                            {
                                // Asignamos los valores
                                Domicilio = item["street"]?.ToObject<string>(),
                                Poblacion = item["city"]?.ToObject<string>(),
                                Pais = item["country"]?.ToObject<string>(),
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
            } catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            // Si no, devolvemos una lista vacía
            return new List<Proveedor>();
        }
    }
}
