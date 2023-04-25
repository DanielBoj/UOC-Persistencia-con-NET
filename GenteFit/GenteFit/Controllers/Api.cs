using GenteFit.Controllers.ControllersMongoDB;
using GenteFit.Models;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/* API RESTful para la aplicación GenteFit.
 * En esta clase vamos a crear toda la lógica del backend, hemos simplificado todas las acciones
 en los controladores DTO de cada entidad y nos hemos asegurado de que todas las operaciones
 y métodos sean asíncronos en'lso repositorios. Para ello, los métodos de interacción con la BD
 deben devolver Task<>, dentro del operador diamante especificaremos los tipos de retorno que queremos
 por último, en la API, retornaremos los responses mediante el tipo Task<IActionResult>, de este
 forma podremos devolver toda la información que queramos que, posteriormente, capturaremos en la app
 de frontend.
 Es muy importante también el uso de los decoradores:
 [Route("")] indica el enrutado de la API; [ApiController] indica que se trata de un controlador API
 que manejará endpoints y, finalmente, hemos d eespecificar los verbos de cada acción antes de cada uno
 de los métodos y las rutas concretas para estos, por ejemplo, [HttpGet("/centro/{id}")], mediante {id}
 indicamos que recibiremos la id del objeto por parámetro en la URL. */
namespace GenteFit.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Api : ControllerBase
    {
        // Cremos la lógica interna de la aplicación
        /* Loggin */
        // GET api
        // Necesitamos una lista con todos los usuarios para poder logearnos
        [HttpGet("[controller]/users")]
        public async Task<IActionResult> ListAllUsuarios()
        {
            // Declaramos una lista para almacenar los usuarios
            List<User> usuarios = new();

            // Creamos los controladores de MongoDB
            ClienteController dbCliente = new();
            EmpleadoController dbEmpleado = new();
            AdministradorController dbAdmin = new();

            try
            {
                // Primero comprobamos si existen elementos y luego los añadimos a la lista.
                if (!await dbCliente.IsEmpty()) 
                {
                    usuarios.AddRange(await dbCliente.GetAllClientes());
                }
                if (!await dbEmpleado.IsEmpty())
                {
                    usuarios.AddRange(await dbEmpleado.GetAllEmpleados());
                }

                if (!await dbAdmin.IsEmpty())
                {
                    usuarios.AddRange(await dbAdmin.GetAllAdministradores());
                }

                // Devolvemos la lista de usuarios
                return usuarios.Count > 0? Ok(usuarios) : NotFound("No se han encontrado usuarios");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Comprobamos si el usuario existe y si la contraseña es correcta
        // POST api/login
        [HttpPost("[controller]/login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            // Comprobamos que el usuario no sea nulo
            if (user is null)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Obtenemos la lista de usuarios -- Tenemos que realizar la conversión de tipos
                OkObjectResult? res = await ListAllUsuarios() as OkObjectResult;

                if (res.Value is null || res.Value is not List<User> usuarios)
                {
                    return NotFound("List error");
                }

                // Comprobamos si el usuario existe en la BD y si las credenciales son correctas.
                User? usuario = usuarios.Find(usr => usr.Email == user.Email &&
                        usr.Pass == user.Pass);

                // Si no existe, devolvemos un NotFound
                // Discriminamos el tipo de usuario
                if (usuario is Administrador)
                {
                    return Ok(new { id = usuario.Id, tipo = "admin" });
                } else if (usuario is Empleado)
                {
                    return Ok(new { id = usuario.Id, tipo = "empleado" });
                } else if (usuario is Cliente)
                {
                    return Ok(new { id = usuario.Id, tipo = "cliente" });
                } else return NotFound("El email o el pass son incorrectos");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Centros */
        // Mostramos la información del centro
        // GET: api/Centro
        [HttpGet("[controller]/centro")]
        public async Task<IActionResult> ListCentros()
        {
            try
            {
                CentroController db = new();
                List<Centro> centros = await db.GetAllCentros();

                // Queremos únicamente el primer elemento de la lista aunque no sepamos su ID.
                Centro centro = centros.ElementAt(0);

                return centro is not null? Ok(centro) : NotFound("No existen centros");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }          
        }

        // Obtenemos un centro por su ID
        // GET: api/Centro/id
        [HttpGet("[controller]/centro/{id}")]
        public async Task<IActionResult> GetCentroById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB.
            CentroController db = new();

            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Centro centro = await db.Details(id);

                // Comprobamos que el centro existe.
                // Devolvemos el centro.
                return centro is not null? Ok(centro) : NotFound("El centro no existe");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Modificamos la información del centro
        // PUT: api/Centro
        [HttpPut("[controller]/centro/{id}")]
        public async Task<IActionResult> UpdateCentro([FromRoute]string id, [FromBody] Centro centro)
        {
            // Instanciamos el controlador de MongoDB
            CentroController db = new();

            // Nos aseguramos de que los datos sean correctos
            if (centro is null || id is null)
            {
                return BadRequest(ModelState);
            }

            // Cargamos el centro original para comprobar que existe y obtener su id.
            Centro srcCentro = await db.Details(id);

            if (srcCentro is null)
            {
                return NotFound("El centro no existe");
            }

            try
            {
                // Preparamos el objeto:
                // Nos aseguramos de que el ID del objeto que estamos editando es el mismo que el que hemos pasado por parámetro
                centro.Id = srcCentro.Id;

                // Actualizamos el centro
                return await db.Edit(centro)? Created("Id", centro.Id) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Empleados */
        // Mostamos la lista de empleados
        // GET: api/empleado
        [HttpGet("[controller]/empleado")]
        public async Task<IActionResult> ListEmpleados()
        {
            // Creamos el controlador de MongoDB
            EmpleadoController db = new();

            try
            {
                // Obtenemos la lista de empleados
                List<Empleado> empleados = await db.GetAllEmpleados();
                
                // Devolvemos la lista de empleados
                return empleados.Count > 0? Ok(empleados) : NotFound("No existen empleados");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Mostramos la información de un empleado
        // GET: api/empleado/id
        [HttpGet("[controller]/empleado/{id}")]
        public async Task<IActionResult> GetEmpleadoById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            EmpleadoController db = new();

            // Comprobamos que los datos sean correctos
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Empleado empleado = await db.Details(id);

                return empleado is not null? Ok(empleado) : NotFound("El empleado no existe");
            }
            catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        /* Administrador */
        // Obtenemos una lista de administradores
        // GET: api/administrador
        [HttpGet("[controller]/administrador")]
        public async Task<IActionResult> ListAllAdministradores()
        {
            // Creamos el controlador de MongoDB
            AdministradorController db = new();

            try
            {
                // Obtenemos la lista de administradores
                List<Administrador> administradores = await db.GetAllAdministradores();
                return administradores.Count > 0? Ok(administradores) : NotFound("No existen administradores");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de un administrador
        // GET: api/administrador/id
        [HttpGet("[controller]/administrador/{id}")]
        public async Task<IActionResult> GetAdministradorById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            AdministradorController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Administrador administrador = await db.Details(id);

                // Devolvemos el administrador
                return administrador is not null? Ok(administrador) : NotFound("El administrador no existe.");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Clientes */
        // Obtenemos una lista de clientes
        // GET: api/cliente
        [HttpGet("[controller]/cliente")]
        public async Task<IActionResult> ListAllClientes()
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();

            try
            {
                // Obtenemos la lista de clientes
                List<Cliente> clientes = await db.GetAllClientes();

                return clientes.Count > 0? Ok(await db.GetAllClientes()) : NotFound("No existen clientes");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de un cliente
        // GET: api/cliente/id
        [HttpGet("[controller]/cliente/{id}")]
        public async Task<IActionResult> GetClienteById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Cliente cliente = await db.Details(id);

                // Comprobamos que el cliente existe y lo devolvemos.
                return cliente is not null? Ok(cliente) : NotFound("El cliente no existe");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Creamos un nuevo cliente
        // POST: api/cliente
        [HttpPost("[controller]/cliente")]
        public async Task<IActionResult> CreateCliente([FromBody] Cliente cliente)
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();

            // Comprobamos que los datos sean correctos
            if (cliente is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Comprobamos que el cliente no existe
                if (db.GetAllClientes().Result.Any(c => c.Nif.Equals(cliente.Nif)))
                {
                    return BadRequest("El cliente ya existe");
                }

                // Inicializamos las colecciones
                cliente.Reservas = new();
                cliente.Esperas = new();

                // Creamos el cliente
                return await db.Create(cliente)? CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id.ToString() }, cliente) 
                    : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Modificamos la información de un cliente
        // PUT: api/cliente/id
        [HttpPut("[controller]/cliente/{id}")]
        public async Task<IActionResult> UpdateCliente([FromRoute] string id, [FromBody] Cliente cliente)
        {
            // Creamos los controladores de MongoDB
            ClienteController db = new();
            ReservaController dbReserva = new();
            EsperaController dbEspera = new();

            // Comprobamos que los datos sean correctos
            if (id is null || cliente is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Cargamos el cliente original para comprobar que existe y obtener su ID.
                Cliente srcCliente = await db.Details(id);

                if (srcCliente is null)
                {
                    return NotFound();
                }

                // Nos aseguramos de que la id es correcta.
                cliente.Id = srcCliente.Id;

                // Modificamos los objetos de las colecciones relacionados con el cliente.
                if (cliente.Reservas.Count > 0)
                {
                    cliente.Reservas.ForEach(async (res) =>
                    {
                        // Modificamos el cliente en la reserva.
                        res.Cliente = cliente;

                        // Actualizamos la reserva en la BD.
                        await dbReserva.Edit(res);
                    });
                }

                if (cliente.Esperas.Count > 0)
                {
                    cliente.Esperas.ForEach(async (esp) =>
                    {
                        // Modificamos el cliente en la espera.
                        esp.Cliente = cliente;
                        // Actualizamos la espera en la BD.
                        await dbEspera.Edit(esp);
                    });
                }

                // Actualizamos el cliente
                return await db.Edit(cliente)? Created("Id", cliente.Id) : StatusCode(400);

            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos un cliente
        // DELETE: api/cliente/id
        [HttpDelete("[controller]/cliente/{id}")]
        public async Task<IActionResult> DeleteCliente([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();

            // Comprobamos que los datos sean correctos
            if(id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Capturamos el cliente para realizar las operaciones secundarias.
                Cliente cliente = await db.Details(id);

                // Primero tenemos que eliminar las reservas y esperas del cliente si existen.
                if (cliente.Reservas.Count > 0)
                {
                    // Eliminamos las reservas de la BD..
                    cliente.Reservas.ForEach(async (res) => await DeleteReserva(res.Id.ToString()));
                }

                if (cliente.Esperas.Count > 0)
                {
                    //Eliminamos las esperas de la BD.
                    cliente.Esperas.ForEach(async (es) => await DeleteEspera(es.Id.ToString()));
                }
               
                // Eliminamos el cliente
                return await db.Delete(id) ? StatusCode(200) : NotFound("El cliente no existe.");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }

        }

        /* Clases */
        // Obtenemos una lista de clases
        // GET: api/clase
        [HttpGet("[controller]/clase")]
        public async Task<IActionResult> ListAllClases()
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();

            try
            {
                // Obtenemos la lista de clases
                List<Clase> clases = await db.GetAllClases();

                // Devolvemos la lista de clases.
                return clases.Count > 0? Ok(await db.GetAllClases()) : NotFound("No existen clases");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }

        }

        // Obtenemos la información de una clase
        // GET: api/clase/id
        [HttpGet("[controller]/clase/{id}")]
        public async Task<IActionResult> GetClaseById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Clase clase = await db.Details(id);

                return clase is not null? Ok(clase) : NotFound("La clase no existe");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Creamos una nueva clase
        // POST: api/clase
        [HttpPost("[controller]/clase")]
        public async Task<IActionResult> CreateClase([FromBody] Clase clase)
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();

            // Comprobamos que los datos sean correctos
            if (clase is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Comprobamos que la clase no existe
                if (db.GetAllClases().Result.Any(cls => cls.Nombre.Equals(clase.Nombre)))
                {
                    return BadRequest("La clase ya existe");
                }

                // Creamos la clase
                return await db.Create(clase)? CreatedAtAction(nameof(GetClaseById), new { id = clase.Id }, clase) 
                    : StatusCode(400);        
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Modificamos la información de una clase
        // PUT: api/clase/id
        [HttpPut("[controller]/clase/{id}")]
        public async Task<IActionResult> UpdateClase([FromRoute] string id, [FromBody] Clase clase)
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();
            HorarioController dbHorario = new();

            // Comprobamos que los datos sean correctos
            if (id is null || clase is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Cargamos la clase original para comprobar que existe y obtener su ID.
                Clase srcClase = await db.Details(id);
                
                // Comprobamos que la clase existe.
                if (srcClase is null)
                {
                    return NotFound("La clase no existe.");
                }

                // Nos aseguramos de que la id es correcta.
                clase.Id = srcClase.Id;

                // Modificamos los horarios de la clase
                List<Horario> horarios = await dbHorario.GetAllHorarios();
                // Filtramos el horario
                horarios.Where(hor => hor.Clase.Id.Equals(clase.Id)).ToList()
                    // Actualizamos el horario
                    .ForEach(async (hor) =>
                {
                    hor.Clase = clase;

                    // Actualizamos el horario en la BD.
                    await dbHorario.Edit(hor);
                });

                // Actualizamos la clase
                return await db.Edit(clase)? Created("Id", clase.Id) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos una clase
        // DELETE: api/clase/id
        [HttpDelete("[controller]/clase/{id}")]
        public async Task<IActionResult> DeleteClase([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();
            HorarioController dbHorario = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Eliminamos los horarios de la clase
                List<Horario> horarios = await dbHorario.GetAllHorarios();
                // Filtramos la lista de horarios para quedarnos con los que tengan la clase que queremos eliminar.
                horarios.Where(hor => hor.Clase.Id.Equals(id)).ToList()
                    // Eliminamos los horarios relacionados con la clase.
                    .ForEach(async (hor) =>
                    {
                        // Usamos la función de la API que automatiza la eliminación de horarios.
                        await DeleteHorario(hor.Id.ToString());
                    });

                // Eliminamos la clase
                return await db.Delete(id)? StatusCode(200) : NotFound("La clase no existe.");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Horarios */
        // Obtenemos una lista de horarios
        // GET: api/horario
        [HttpGet("[controller]/horario")]
        public async Task<IActionResult> ListAllHorarios()
        {
            // Creamos el controlador de MongoDB
            HorarioController db = new();

            try
            {
                // Obtenemos la lista de horarios
                List<Horario> horarios = await db.GetAllHorarios();

                // Devolvemos la lista de horarios.
                return horarios.Count > 0? Ok(horarios) : NotFound("No existen horarios");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemor una lista con todos los horarios filtrados por clase
        // GET: api/horario/filter:clase
        [HttpGet("[controller]/filter:{clase}")]
        public async Task<IActionResult> ListHorariosByClase([FromRoute] string clase)
        {
            // Creamos el controlador de MongoDB
            HorarioController dbHorario = new();

            // Comprobamos que los datos sean correctos
            if (clase is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos la lista de horarios
                List<Horario> horarios = await dbHorario.GetAllHorarios();

                // Filtramos la lista de horarios para quedarnos con los que tengan la clase que buscamos.
                horarios = horarios.Where(hor => hor.Clase.Nombre.ToLower().Equals(clase.ToLower())).ToList();
                
                // Devolvemos la lista de horarios.
                return horarios.Count > 0? Ok(horarios) : NotFound("No existen horarios para la clase " + clase);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Filtramos los horarios por cliente
        // GET: api/horario/idCliente
        [HttpGet("[controller]/horario/{idCliente}")]
        public async Task<IActionResult> FilterHorariosByCliente([FromRoute] string idCliente)
        {
            // Creamos el controlador de MongoDB
            HorarioController db = new();

            // Comprobamos que los datos sean correctos
            if (idCliente is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos la lista de horarios
                List<Horario> horarios = await db.GetAllHorarios();

                // Filtramos la lista de horarios para quedarnos con los que tengan el cliente que buscamos.
                horarios = horarios.Where(hor => hor.Reservas.Any(res => res.Cliente.Id.Equals(idCliente))).ToList();

                // Devolvemos la lista de horarios.
                return horarios.Count > 0? Ok(horarios) : NotFound("No existen horarios para el cliente " + idCliente);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Filtramos los horarios por cliente y clase
        // GET: api/horario/idCliente?clase:clase
        [HttpGet("[controller]/horario/{idCliente}clase:{clase}")]
        public async Task<IActionResult> ListHorariosByClienteyClase([FromRoute] string idCliente,
            [FromRoute] string clase)
        {
            // Creamos el controlador de MongoDB
            HorarioController db = new();

            // Comprobamos que los datos sean correctos
            if (idCliente is null || clase is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos la lista de horarios
                List<Horario> horarios = await db.GetAllHorarios();
                
                // Filtramos los horarios por cliente
                horarios = horarios.Where(hor => hor.Reservas.Any(res => res.Cliente.Id.Equals(idCliente))).ToList();

                // Filtramos la lista de horarios para quedarnos con los que tengan la clase que buscamos.
                horarios = horarios.Where(hor => hor.Clase.Nombre.ToLower().Equals(clase.ToLower())).ToList();

                // Devolvemos la lista de horarios.
                return horarios.Count > 0? Ok(horarios) : NotFound("No existen horarios para el cliente " + idCliente + " y la clase " + clase);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de un horario
        // GET: api/horario/id
        [HttpGet("[controller]/horario/{id}")]
        public async Task<IActionResult> GetHorarioById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            HorarioController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Horario horario = await db.Details(id);

                return horario is not null? Ok(horario) : NotFound("El horario no existe");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Creamos un nuevo horario
        // POST: api/horario
        [HttpPost("[controller]/horario/{id}")]
        public async Task<IActionResult> CreateHorario([FromRoute] string id, [FromBody] Horario horario)
        {
            // Creamos el controlador de MongoDB
            HorarioController dbHorario = new();
            // Creamos un controlador auxiliar para manjear la Clase
            ClaseController dbClase = new();

            // Comprobamos que los datos sean correctos.
            if (id is null || horario is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Si existe, capturamos la clase y realizamos la comprobación.
                Clase clase = await dbClase.Details(id);

                if (clase is null)
                {
                    return NotFound("La clase no existe.");
                }

                // Comprobamos que el horario está libre
                if (dbHorario.GetAllHorarios().Result.Any(hor => hor.Dia.Equals(horario.Dia) &&
                    hor.Hora.Equals(horario.Hora)))
                {
                    return BadRequest("El horario ya está ocupado.");
                }

                // Añadimos el horario a la BD.
                return await dbHorario.Create(horario)? CreatedAtAction(nameof(GetHorarioById), new { id = horario.Id }, horario) 
                    : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Modificamos la información de un horario
        // PUT: api/horario/id
        [HttpPut("[controller]/horario/{id}")]
        public async Task<IActionResult> UpdateHorario([FromRoute] string id, [FromBody] Horario horario)
        {
            // Creamos el controlador de MongoDB
            HorarioController db = new();
            ReservaController dbReserva = new();
            EsperaController dbEspera = new();
            
            // Comprobamos que los datos sean correctos
            if (id is null || 
                horario is null || 
                horario.Clase is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Cargamos el horario original para comprobar que existe y obtener su ID.
                Horario srcHorario = await db.Details(id);

                // Comprobamos que el horario existe.
                if (horario is null)
                {
                    return NotFound("No existe el horario.");
                }

                // Nos aseguramos de que la id es correcta.
                horario.Id = srcHorario.Id;

                // Modificamos las reservas y esperas relacionadas con el horario.
                if (horario.Reservas.Count > 0)
                {
                    // Obtenemos la lista de reservas y modificamos el horario de cada una en la BD.
                    horario.Reservas.ForEach(async (res) =>
                    {
                        // Modificamos el horario de la reserva.
                        res.Horario = horario;

                        // Actualizamos la reserva en la BD.
                        await dbReserva.Edit(res);
                    });
                }

                if (horario.Esperas.Count > 0)
                {
                    // Obtenemos la lista de esperas y modificamos el horario de cada una en la BD.
                    horario.Esperas.ForEach(async (esp) =>
                    {
                        // Modificamos el horario de la espera.
                        esp.Horario = horario;

                        // Actualizamos la espera en la BD.
                        await dbEspera.Edit(esp);
                    });
                }

                // Actualizamos el horario
                return await db.Edit(horario)? Created("Id", horario.Id) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos un horario
        // DELETE: api/horario/id
        [HttpDelete("[controller]/horario/{id}")]
        public async Task<IActionResult> DeleteHorario([FromRoute] string id)
        {
            // Creamos el controlador principal y los auxiliares de MongoDB.
            HorarioController dbHorario = new();
            ReservaController dbReserva = new();
            EsperaController dbEspera = new();
            ClienteController dbCliente = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos el horario de la DB para poder realizar las acciones secundarias.
                Horario horario = await dbHorario.Details(id);

                // Obtenemos la lista de reservas y de esperas para el horario y eliminamos todos los
                // elementos de la BD.
                if (horario.Id is not null)
                {
                    if (horario.Reservas.Count > 0)
                    {
                        horario.Reservas.ForEach(async (res) =>
                        {
                            // Obtenemos el cliente
                            Cliente cliente = await dbCliente.Details(res.Cliente.Id.ToString());

                            // Modificamos la lista de reservas del cliente
                            cliente.Reservas.Remove(res);

                            // Modificamos el cliente en la db
                            await dbCliente.Edit(cliente);

                            // Eliminamos la reserva de la BD.
                            await dbReserva.Delete(res.Id.ToString());
                        });
                    }
                    
                    if (horario.Esperas.Count > 0)
                    {
                        horario.Esperas.ForEach(async (es) =>
                        {
                            // Obtenemos el cliente
                            Cliente cliente = await dbCliente.Details(es.Cliente.Id.ToString());

                            // Modificamos la lista de esperas del cliente.
                            cliente.Esperas.Remove(es);

                            // Modificamos el cliente en la BD.
                            await dbCliente.Edit(cliente);

                            // Eliminalos la espera de la BD.
                            await dbEspera.Delete(es.Id.ToString());
                        });
                    }
                }

                // Eliminamos el horario de la BD.
                return await dbHorario.Delete(id)? StatusCode(200) : NotFound("El horario no existe.");
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Reservas */
        // Obtenemos una lista de reservas
        // GET: api/reserva
        [HttpGet("[controller]/reserva")]
        public async Task<IActionResult> ListAllReservas()
        {
            // Creamos el controlador de MongoDB
            ReservaController db = new();

            try
            {
                // Obtenemos la lista de reservas
                List<Reserva> reservas = await db.GetAllReservas();

                // Devolvemos la lista de reservas.
                return reservas.Count > 0? Ok(reservas) : NotFound("No existen reservas");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de una reserva
        // GET: api/reserva/id
        [HttpGet("[controller]/reserva/{id}")]
        public async Task<IActionResult> GetReservaById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ReservaController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Reserva reserva = await db.Details(id);

                return reserva is not null? Ok(reserva) : NotFound("La reserva no existe");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Creamos una nueva reserva: Por parámetros de URL hemos de recibir el ID del usuario y el ID del horario. 
        // POST: api/reserva
        [HttpPost("[controller]/reserva/{id}")]
        public async Task<IActionResult> CrearReserva([FromRoute] string id, 
            [FromRoute] string horarioId,
            [FromBody] Reserva reserva)
        {
            // Creamos el controlador de MongoDB y los controladores auxiliares
            ReservaController dbReserva = new();
            HorarioController dbHorario = new();
            ClienteController dbCliente = new();

            // Usaremos un flag para comprobar que todos los pasos se realizan correctamente.
            bool flag = false;

            // Comprobamos que los datos sean correctos
            if (id is null || horarioId is null || reserva is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos de los objetos asociados a través de sus IDs.
                Cliente cliente = await dbCliente.Details(id);
                Horario horario = await dbHorario.Details(horarioId);

                if (cliente is not null || horario is not null)
                {
                    // A través del horario obtenemos la clase y de allí las plazas máximas.
                    // Comprobamos las plazas ocupadas en la lista de reservas y las restamos al total de plazas de la clase.
                    // También debemos comprobar que el cliente no tiene ya una reserva en ese horario y que el máximo
                    // de reservas por cliente no se ha superado.
                    if (horario.Reservas.Count < horario.Clase.Plazas &&
                        !cliente.Reservas.Any(res => res.Horario.Id.Equals(horario.Id)) &&
                        cliente.Reservas.Count < Cliente.MaxReservas
                        )
                    {
                        // Asignamos los objetos asociados a la reserva.
                        reserva.Cliente = cliente;
                        reserva.Horario = horario;

                        // Ahora tenemos que seguir la lógica de las asociaciones de nuestra lógica de negocio.
                        // Primero creamos la reserva en la base de datos.
                        flag = await dbReserva.Create(reserva);

                        // Ahora tenemos que actualizar el horario con la nueva reserva.
                        if (flag)
                        {
                            // Añadimos la reserva al horario.
                            horario.Reservas.Add(reserva);

                            // Actualizamos el horario en la base de datos.
                            flag = await dbHorario.Edit(horario);
                        }

                        // Por último, tenemos que actualizar el cliente con la nueva reserva.
                        if (flag)
                        {
                            // Añadimos la reserva al cliente.
                            cliente.Reservas.Add(reserva);

                            // Actualizamos el cliente en la base de datos.
                            flag = await dbCliente.Edit(cliente);
                        }
                    }

                    // Ahora ya podemos devolver el resultado.
                    return flag ? CreatedAtAction("GetReservaById", new { id = reserva.Id }, reserva) :
                        StatusCode(400);
                }
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }

            return NotFound("No existe el horario o el cliente.");
        }

        // Eliminamos una reserva: Tendremos que eliminar la reserva de la lista de reservas del cliente y del horario.
        // Además, activará el trigger para modificar el estado de la espera asociada.
        // DELETE: api/reserva/id
        [HttpDelete("[controller]/reserva/{id}")]
        public async Task<IActionResult> DeleteReserva([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB y los controladores auxiliares
            ReservaController dbReserva = new();
            HorarioController dbHorario = new();
            ClienteController dbCliente = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos de la reserva a través de su ID.
                Reserva reserva = await dbReserva.Details(id);

                // Comprobamos que la reserva exista.
                if (reserva is not null)
                {
                    // Obtenemos los datos de los objetos asociados a través de sus IDs.
                    Cliente cliente = await dbCliente.Details(reserva.Cliente.Id.ToString());
                    Horario horario = await dbHorario.Details(reserva.Horario.Id.ToString());

                    // Eliminamos la reserva de la lista de reservas del cliente y del horario.
                    cliente.Reservas.Remove(reserva);

                    // Ejecutamos la función para comprobar la lista de esperas.
                    await CheckEsperas(horario);

                    // Usaremos un flag para comprobar que todos los pasos se realizan correctamente.
                    bool flag = false;

                    // Guardamos los cambios en el cliente.
                    flag = await dbCliente.Edit(cliente);

                    // Eliminamos la reserva de la lista de reservas del horario.
                    horario.Reservas.Remove(reserva);

                    // Guardamos los cambios en el horario.
                    flag = await dbHorario.Edit(horario);

                    // Eliminamos la reserva de la base de datos.
                    flag = await dbReserva.Delete(id);

                    // Ahora ya podemos devolver el resultado.
                    return flag? StatusCode(200) : StatusCode(400);
                }
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }

            return NotFound("No existe la reserva.");
        }

        /* Esperas */
        // Obtenemos una lista de esperas
        // GET: api/espera
        [HttpGet("[controller]/espera")]
        public async Task<IActionResult> ListAllEsperas()
        {
            // Creamos el controlador de MongoDB
            EsperaController db = new();

            try
            {
                // Obtenemos la lista de esperas
                List<Espera> esperas = await db.GetAllEsperas();

                // Devolvemos la lista de esperas.
                return esperas.Count > 0? Ok(esperas) : NotFound("No existen esperas");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de una espera
        // GET: api/espera/id
        [HttpGet("[controller]/espera/{id}")]
        public async Task<IActionResult> GetEsperaById([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            EsperaController db = new();

            // Comprobamos que los datos sean correctos
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos desde MongoDB a través del ID del elemento.
                Espera espera = await db.Details(id);

                return espera is not null? Ok(espera) : NotFound("La espera no existe");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Creamos una nueva espera: Por argumento vamos a recibir el ID del cliente y el ID del horario.
        // Como cuerpo recibiremos la espera.
        // POST: api/espera
        [HttpPost("[controller]/espera/{id}")]
        public async Task<IActionResult> CreateEspera([FromRoute] string id,
            [FromRoute] string idHorario,
            [FromBody] Espera espera)
        {
            // Creamos el controlador principal y los auxiliares de MongoDB
            EsperaController dbEspera = new();
            HorarioController dbHorario = new();
            ClienteController dbCliente = new();

            // Comprobamos que los datos sean correctos
            if (id is null || idHorario is null || espera is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Añadimos el cliente y el horario a la espera.
                espera.Cliente = await dbCliente.Details(id);
                espera.Horario = await dbHorario.Details(idHorario);

                // Añadimos la espera a la lista de esperas del cliente.
                espera.Cliente.Esperas.Add(espera);
                // Modificamos el cliente en la base de datos.
                bool flag = await dbCliente.Edit(espera.Cliente);

                // Añadimos la espera a la lista de esperas del horario.
                espera.Horario.Esperas.Add(espera);
                // Modificamos el horario en la base de datos.
                flag = await dbHorario.Edit(espera.Horario);

                // Añadimos la espera a la base de datos.
                flag = await dbEspera.Create(espera);

                // Devolvemos el resultado.
                return flag? CreatedAtAction(nameof(GetEsperaById), new { id = espera.Id }, espera) :
                    StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // TODO: Modificar la espera

        // Eliminamos una espera
        // DELETE: api/espera/id
        [HttpDelete("[controller]/espera/{id}")]
        public async Task<IActionResult> DeleteEspera([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB.
            EsperaController db = new();
            ClienteController dbCliente = new();
            HorarioController dbHorario = new();

            // Comprobamos que los datos sean correctos.
            if (id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos la espera para asegurar el ID y realizar las acciones secundarias.
                Espera espera = await db.Details(id);

                // COmprobamos que la espera exista.
                if (espera is null)
                {
                    return NotFound("La espera no existe");
                }

                // Eliminamos los datos relacionados con la espera.
                espera.Cliente.Esperas.Remove(espera);
                bool flag = await dbCliente.Edit(espera.Cliente);

                espera.Horario.Esperas.Remove(espera);
                flag = await dbHorario.Edit(espera.Horario);

                // Eliminamos la espera de la base de datos.
                flag = await db.Delete(id);

                // Devolvemos el resultado.
                return flag? StatusCode(200) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }


        /* Métodos auxiliares */
        // Comprobamos la si queda alguna plazo libre y movemos la lista de esperas.
        
        public static async Task<bool> CheckEsperas(Horario horario) {
            
            //Creamos los controladores de MongoDB
            HorarioController dbHorario = new();
            EsperaController dbEspera = new();
            ReservaController dbReserva = new();
            ClienteController dbCliente = new();

            // Creamos el flag para comprobar que todo se realiza correctamente.
            bool flag = false;

            // Creamos el cliente y la espera fuera del marco de los métodos para poder usarlo en el bucle y acceder
            // fuera del ámbito de los métodos.
            Cliente? cliente = null;
            Espera? espera = null;
            Reserva? reserva = null;

            try
            { 
                // Obtenemos la lista de esperas relacionada con el horario.
                List<Espera> esperas = horario.Esperas;

                // Comprobamos que la lista de esperas no esté vacía y obtenemos el elemento inicial
                // para cumplir con la política FIFO, pero hay que aplicar la lógica de negocio de que
                // el cliente no puede tener más de 2 reservas. Así que recorreremos la lista de esperas
                // hasta encontrar un cliente que cumpla con la condición y devolveremos el primero que
                // cumpla con la condición.
                if (esperas.Count > 0)
                {
                    // Iteramos la lista de esperas desde el inicio, para cumplir con la política FIFO y
                    // también con la restricción de máximo 2 reservas por cliente.
                    esperas.ForEach(es =>
                    {
                        // Si no hemos asignado ningún valor a espera, seguimos con la iteración.
                        if (espera is null)
                        {
                            // Obtenemos el cliente asociado a la espera.
                            cliente = es.Cliente;

                            // Comprobamos que el cliente tenga reservas libres.
                            if (cliente.Reservas.Count > Cliente.MaxReservas)
                            {
                                // Eliminamos la espera de la lista de esperas del cliente y del horario.
                                cliente.Esperas.Remove(es);
                                horario.Esperas.Remove(es);

                                // Creamos la reserva.
                                reserva = new(horario, cliente);

                                // Añadimos la reserva a la lista de reservas del cliente y del horario.
                                cliente.Reservas.Add(reserva);
                                horario.Reservas.Add(reserva);

                                // Guardamos la espera en la variable auxiliar para borrarla de la base de datos.
                                espera = es;
                                // Espera funciona como un flag para indicar que hemos encontrado un cliente
                            }
                        }
                    });

                    // Guardamos los cambios en la base de datos.
                    if (espera is not null)
                    {
                        flag = await dbCliente.Edit(cliente);
                        flag = await dbHorario.Edit(horario);

                        // Añadimos la nueva reserva a la base de datos.
                        flag = await dbReserva.Create(reserva);

                        // Eliminamos la espera de la base de datos.
                        flag = await dbEspera.Delete(espera.Id.ToString());
                    }

                    // Devolvemos el resultado.
                    return flag;
                }
                
                // Si no hay esperas, no devolvemos un error, simplemente devolvemos un código 200.
                return false;

            } catch (Exception err)
            {
                Console.WriteLine(err.Message.ToString());
                return false;
            }
        }
    }
}
