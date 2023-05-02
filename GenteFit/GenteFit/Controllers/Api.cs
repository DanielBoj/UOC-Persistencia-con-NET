using GenteFit.Controllers.ControllersMongoDB;
using GenteFit.Models;
using GenteFit.Models.Collections;
using GenteFit.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
    [Route("api")]
    [ApiController]
    public class Api : ControllerBase
    {
        // Cremos la lógica interna de la aplicación
        /* Loggin */
        // GET api
        // Necesitamos una lista con todos los usuarios para poder logearnos
        [HttpGet("users")]
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
                    // Seteamos el tipo de usuario
                    usuarios.ForEach(usr => usr.Tipo = "cliente");
                }
                if (!await dbEmpleado.IsEmpty())
                {
                    // Cargamos una lista temporal con los empleados
                    List<Empleado> empleados = (await dbEmpleado.GetAllEmpleados());

                    // Seteamos el tipo de usuario
                    empleados.ForEach(usr => usr.Tipo = "empleado");

                    // Añadimos los empleados a la lista de usuarios
                    usuarios.AddRange(empleados);

                }

                if (!await dbAdmin.IsEmpty())
                {
                    // Cargamos una lista temporal con los administradores
                    List<Administrador> admins = (await dbAdmin.GetAllAdministradores());
                    
                    // Seteamos el tipo de usuario
                    admins.ForEach(usr => usr.Tipo = "admin");

                    // Añadimos los administradores a la lista de usuarios
                    usuarios.AddRange(admins);
                }

                // Devolvemos la lista de usuarios
                return usuarios.Count > 0 ? Ok(usuarios) : NotFound("No se han encontrado usuarios");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Comprobamos si el usuario existe y si la contraseña es correcta
        // POST api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                // Obtenemos la lista de usuarios -- Tenemos que realizar la conversión de tipos
                OkObjectResult? res = await ListAllUsuarios() as OkObjectResult;

                if (res?.Value is null || res?.Value is not List<User> usuarios)
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
                }
                else if (usuario is Empleado)
                {
                    return Ok(new { id = usuario.Id, tipo = "empleado" });
                }
                else if (usuario is Cliente)
                {
                    return Ok(new { id = usuario.Id, tipo = "cliente" });
                }
                else return NotFound("El email o el pass son incorrectos");
            }
            catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Centros */
        // Mostramos la información del centro
        // GET: api/Centro
        [HttpGet("centro")]
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
        [HttpGet("centro/{id}")]
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
        [HttpPut("centro/{id}")]
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
                return await db.Edit(centro)? StatusCode(200) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        /* Empleados */
        // Mostamos la lista de empleados
        // GET: api/empleado
        [HttpGet("empleado")]
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
        [HttpGet("empleado/{id}")]
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
        [HttpGet("administrador")]
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
        [HttpGet("administrador/{id}")]
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
        [HttpGet("cliente")]
        public async Task<IActionResult> ListAllClientes()
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();

            try
            {
                // Obtenemos la lista de clientes
                List<Cliente> clientes = await db.GetAllClientes();

                // Asignamos el tipo de usuario
                clientes.ForEach(cliente => cliente.Tipo = "cliente");

                return clientes.Count > 0? Ok(await db.GetAllClientes()) : NotFound("No existen clientes");
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de un cliente
        // GET: api/cliente/id
        [HttpGet("cliente/{id}")]
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
        [HttpPost("cliente")]
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
        [HttpPut("cliente/{id}")]
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

                // Actualizamos el cliente
                return await db.Edit(cliente)? StatusCode(200) : StatusCode(400);

            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos un cliente
        // DELETE: api/cliente/id
        [HttpDelete("cliente/{id}")]
        public async Task<IActionResult> DeleteCliente([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB
            ClienteController db = new();
            ReservaController dbReserva = new();
            EsperaController dbEspera = new();

            // Comprobamos que los datos sean correctos
            if(id is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Capturamos el cliente para realizar las operaciones secundarias.
                Cliente cliente = await db.Details(id);

                // Obtenemos la lista de reservas del cliente
                List<Reserva> reservas = await dbReserva.GetAllReservas();
                reservas?.FindAll(res => res.Cliente.Id.Equals(cliente.Id)).ToList();

                // Eliminamos las reservas del cliente de la bd
                reservas?.ForEach(async (res) => await dbReserva.Delete(res.Id.ToString()));

                // Obtenemos la lista de esperas del cliente
                List<Espera> esperas = await dbEspera.GetAllEsperas();
                esperas?.FindAll(es => es.Cliente.Id.Equals(cliente.Id)).ToList();

                // Eliminamos las esperas del cliente de la bd
                esperas?.ForEach(async (es) => await db.Delete(es.Id.ToString()));
               
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
        [HttpGet("clase")]
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
        [HttpGet("clase/{id}")]
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
        [HttpPost("clase")]
        public async Task<IActionResult> CreateClase([FromBody] Clase clase)
        {
            // Creamos el controlador de MongoDB
            ClaseController db = new();

            Clase toCreate = new Clase
            {
                Id = clase.Id,
                Nombre = clase.Nombre,
                Descripcion = clase.Descripcion,
                Profesor = clase.Profesor,
                Duracion = clase.Duracion,
                Plazas = clase.Plazas
            };

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
        [HttpPut("clase/{id}")]
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
                return await db.Edit(clase)? StatusCode(200) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos una clase
        // DELETE: api/clase/id
        [HttpDelete("clase/{id}")]
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
        [HttpGet("horario")]
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
        [HttpGet("filter:{clase}")]
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
        [HttpGet("horario/{idCliente};{idHorario}")]
        public async Task<IActionResult> FilterHorariosByCliente([FromRoute] string idCliente,
            [FromRoute] string idHorario)
        {
            // Creamos el controlador de MongoDB
            HorarioController dbHorario = new();
            ReservaController dbReserva = new();

            // Comprobamos que los datos sean correctos
            if (idCliente is null || idHorario is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos la lista de horarios y de reservas
                List<Reserva> reservas = await dbReserva.GetAllReservas();

                // Filtramos la lista de reservas para quedarnos con las que tengan el cliente y el horario que buscamos.
                reservas = reservas.Where(res => res.Cliente.Id.Equals(idCliente) && res.Horario.Id.Equals(idHorario)).ToList();
                // Devolvemos la lista de horarios.
                return reservas.Count > 0? Ok(reservas) : NotFound("No existen horarios para el cliente " + idCliente);
            } catch (Exception err)
            {
                return StatusCode(404, err.Message);
            }
        }

        // Obtenemos la información de un horario
        // GET: api/horario/id
        [HttpGet("horario/{id}")]
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
        [HttpPost("horario/{id}")]
        public async Task<IActionResult> CreateHorario([FromRoute] string id, [FromBody] Horario horario)
        {
            // Creamos el controlador de MongoDB
            HorarioController dbHorario = new();
            // Creamos un controlador auxiliar para manjear la Clase
            ClaseController dbClase = new();

            // Comprobamos que los datos sean correctos.
            if (id is null)
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

                // Generamos un nuevo objeto para asegurar que lo enviamos correctamente
                Horario toSave = new()
                {
                    Id = horario.Id,
                    Clase = clase,
                    Dia = horario.Dia,
                    Hora = horario.Hora,
                };

                // Añadimos el horario a la BD.
                return await dbHorario.Create(toSave)? CreatedAtAction(nameof(GetHorarioById), new { id = horario.Id }, horario) 
                    : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Modificamos la información de un horario
        // PUT: api/horario/id
        [HttpPut("horario/{id}")]
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

                // Buscamos las reservas que tengan el horario que vamos a modificar.
                List<Reserva> reservas = await dbReserva.GetAllReservas();

                // Obtenemos las reservas que tengan el horario que vamos a modificar.
                reservas = reservas.Where(res => res.Horario.Id.Equals(horario.Id)).ToList();

                // Comprobamos que existen reservas con el horario que vamos a modificar.
                if (reservas.Count > 0)
                {
                    // Obtenemos la lista de reservas y modificamos el horario de cada una en la BD.
                    reservas.ForEach(async (res) =>
                    {
                        // Modificamos el horario de la reserva.
                        res.Horario = horario;
                        // Actualizamos la reserva en la BD.
                        await dbReserva.Edit(res);
                    });
                }

                // Buscamos las esperas que tengan el horario que vamos a modificar.
                List<Espera> esperas = await dbEspera.GetAllEsperas();
                // Obtenemos las esperas que tengan el horario que vamos a modificar.
                esperas = esperas.Where(esp => esp.Horario.Id.Equals(horario.Id)).ToList();

                // Comprobamos que existen esperas con el horario que vamos a modificar.
                if (esperas.Count > 0)
                {
                    // Obtenemos la lista de esperas y modificamos el horario de cada una en la BD.
                    esperas.ForEach(async (esp) =>
                    {
                        // Modificamos el horario de la espera.
                        esp.Horario = horario;
                        // Actualizamos la espera en la BD.
                        await dbEspera.Edit(esp);
                    });
                }

                // Actualizamos el horario
                return await db.Edit(horario)? StatusCode(200) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // Eliminamos un horario
        // DELETE: api/horario/id
        [HttpDelete("horario/{id}")]
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
                if (horario.Id != null)
                {
                    // Obtetenemos la lista de reservas
                    List<Reserva> reservas = await dbReserva.GetAllReservas();
                    // Obtenemos las reservas que tengan el horario que vamos a eliminar.
                    reservas = reservas.Where(res => res.Horario.Id.Equals(horario.Id)).ToList();

                    // Comprobamos que existan elementos
                    if (reservas.Count > 0)
                    {
                        //Eliminamos los horarios de las reservas
                        reservas.ForEach(async (res) =>
                        {
                            // Eliminamos la reserva de la BD.
                            await dbReserva.Delete(res.Id.ToString());
                        });
                    }

                    // Obtetenemos la lista de esperas
                    List<Espera> esperas = await dbEspera.GetAllEsperas();
                    // Obtenemos las esperas que tengan el horario que vamos a eliminar.
                    esperas = esperas.Where(esp => esp.Horario.Id.Equals(horario.Id)).ToList();

                    // Comprobamos que existan elementos
                    if (esperas.Count > 0)
                    {
                        //Eliminamos los horarios de las esperas
                        esperas.ForEach(async (esp) =>
                        {
                            await dbEspera.Delete(esp.Id.ToString());
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
        [HttpGet("reserva")]
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
        [HttpGet("reserva/{id}")]
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
        [HttpPost("reserva/{idCliente};{idHorario}")]
        public async Task<IActionResult> CrearReserva([FromRoute] string idCliente, 
            [FromRoute] string idHorario,
            [FromBody] Reserva reserva)
        {
            // Creamos el controlador de MongoDB y los controladores auxiliares
            ReservaController dbReserva = new();
            HorarioController dbHorario = new();
            ClienteController dbCliente = new();

            // Usaremos un flag para comprobar que todos los pasos se realizan correctamente.
            bool flag = false;

            // Comprobamos que los datos sean correctos
            if (idCliente is null || idHorario is null /*|| reserva is null*/)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Obtenemos los datos de los objetos asociados a través de sus IDs.
                Cliente cliente = await dbCliente.Details(idCliente);
                Horario horario = await dbHorario.Details(idHorario);

                if (cliente != null || horario != null)
                {
                    // A través del horario obtenemos la clase y de allí las plazas máximas.
                    // Comprobamos las plazas ocupadas en la lista de reservas y las restamos al total de plazas de la clase.
                    // También debemos comprobar que el cliente no tiene ya una reserva en ese horario y que el máximo
                    // de reservas por cliente no se ha superado.

                    // Obtenemos las reservas ya creadas para ese horario.
                    List<Reserva> reservas = await dbReserva.GetAllReservas();

                    // Contamos los elementos de la lista de reservas que coinciden con el horario.
                    int plazasOcupadas = reservas.Count(res => res.Horario.Id.Equals(horario.Id));

                    // Obtenemos el número de plazas de la clase, si se superan, retornamos un error.
                    if (plazasOcupadas >= horario.Clase.Plazas)
                    {
                        return StatusCode(400, "No hay plazas disponibles para ese horario.");
                    }

                    // Comprobamos que el cliente no tiene ya una reserva en ese horario.
                    if (reservas.Any((res) =>
                    {
                        if (res.Cliente.Id.Equals(cliente?.Id) && res.Horario.Id.Equals(horario.Id))
                        {
                            return true;
                        }

                        return false;
                    }))
                    {
                        return StatusCode(400, "El cliente ya tiene una reserva para ese horario.");
                    }

                    // Comprobamos que el cliente no ha superado el máximo de reservas.
                    // Obtenemos el número de reservas del cliente.
                    if (reservas.Count(res => res.Cliente.Id.Equals(cliente?.Id)) >= Cliente.MaxReservas) {
                        return StatusCode(400, "El cliente ha superado el máximo de reservas.");
                    }
                   /* if (cliente?.Reservas != null && cliente.Reservas.Count >= Cliente.MaxReservas)
                    {
                        return StatusCode(400, "El cliente ha superado el máximo de reservas.");
                    }*/

                    // Creamos la reserva que insertaremos en la BD.
                    if (cliente is null || horario is null)
                    {
                        return NotFound("No existe el horario o el cliente.");
                    }

                    Reserva toSave = new(horario, cliente)
                    {
                        Id = reserva.Id
                    };


                    // Actualizamos la reserva en la base de datos.
                    flag = await dbReserva.Create(toSave);

                    // Ahora ya podemos devolver el resultado.
                    return flag ? Ok(true) :
                        StatusCode(400);
                }
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }

            return NotFound("No existe el horario o el cliente.");
        }

        // Eliminamos una reserva: Tendremos que eliminar la reserva de la lista de reservas del cliente.
        // Además, activará el trigger para modificar el estado de la espera asociada.
        // DELETE: api/reserva/id
        [HttpDelete("reserva/{id}")]
        public async Task<IActionResult> DeleteReserva([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB y los controladores auxiliares
            ReservaController dbReserva = new();
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

                    // Obtenemos el horario asociado a la reserva.
                    Horario horario = reserva.Horario;

                    // Usaremos un flag para comprobar que todos los pasos se realizan correctamente.
                    bool flag = false;

                    // Eliminamos la reserva de la base de datos.
                    flag = await dbReserva.Delete(id);

                    // Ejecutamos la función para comprobar la lista de esperas.
                    flag = await CheckEsperas(horario);

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
        [HttpGet("espera")]
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
        [HttpGet("espera/{id}")]
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
        [HttpPost("espera/{idCliente};{idHorario}")]
        public async Task<IActionResult> CreateEspera([FromRoute] string idCliente,
            [FromRoute] string idHorario,
            [FromBody] Espera espera)
        {
            // Creamos el controlador principal y los auxiliares de MongoDB
            EsperaController dbEspera = new();
            HorarioController dbHorario = new();
            ClienteController dbCliente = new();

            // Comprobamos que los datos sean correctos
            if (idCliente is null || idHorario is null || espera is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Ontenemos el cliente y el horario a través de sus IDs.
                Cliente cliente = await dbCliente.Details(idCliente);
                Horario horario = await dbHorario.Details(idHorario);

                // Comprobamos que el cliente y el horario existan.
                if (cliente is null)
                {
                    return NotFound("No existe el cliente.");
                }

                if (horario is null)
                {
                    return NotFound("No existe el horario.");
                }

                // Creamos la espera que añadiremos a la base de datos.
                Espera toSave = new(cliente, horario)
                {
                    Id = espera.Id
                };

                // Comprobamos que la espera no exista ya en la base de datos.
                // Obtebenemos la lista de esperas de la BD.
                List<Espera> esperas = await dbEspera.GetAllEsperas();


                // Tenemos  que comprobar si alguna espera de la lista coincide con el id del
                // cliente y el id del horario.
                if (esperas.Any(res => res.Cliente.Id == idCliente && res.Horario.Id == idHorario)) {
                    return BadRequest("La espera ya existe.");
                }

                // Añadimos la espera a la base de datos.
                bool flag = await dbEspera.Create(toSave);

                // Devolvemos el resultado.
                return flag? Ok(true) :
                    StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }

        // TODO: Modificar la espera

        // Eliminamos una espera
        // DELETE: api/espera/id
        [HttpDelete("espera/{id}")]
        public async Task<IActionResult> DeleteEspera([FromRoute] string id)
        {
            // Creamos el controlador de MongoDB.
            EsperaController db = new();
            ClienteController dbCliente = new();

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

                // Eliminamos la espera de la base de datos.
                bool flag = await db.Delete(id);

                // Devolvemos el resultado.
                return flag? StatusCode(200) : StatusCode(400);
            } catch (Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }


        /* Métodos auxiliares */
        // Comprobamos si queda alguna plazo libre y movemos la lista de esperas.
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
                // Obtenemos la lista de esperas
                List<Espera> esperasSrc = await dbEspera.GetAllEsperas();
                // Filtramos la lista de esperas para quedarnos con las que corresponden al horario.
                List<Espera> esperas = esperasSrc.Where(es => es.Horario.Id == horario.Id).ToList();


                // Comprobamos que la lista de esperas no esté vacía y obtenemos el elemento inicial
                // para cumplir con la política FIFO, pero hay que aplicar la lógica de negocio de que
                // el cliente no puede tener más de 2 reservas. Así que recorreremos la lista de esperas
                // hasta encontrar un cliente que cumpla con la condición y devolveremos el primero que
                // cumpla con la condición.
                if (esperas.Count > 0)
                {
                    // Iteramos la lista de esperas desde el inicio, para cumplir con la política FIFO y
                    // también con la restricción de máximo 2 reservas por cliente.
                    esperas.ForEach(async (es) =>
                    {
                        // Si no hemos asignado ningún valor a espera, seguimos con la iteración.
                        if (espera is null)
                        {
                            // Obtenemos el cliente asociado a la espera.
                            cliente = es.Cliente;

                            // Comprobamos cuántas reservas tiene el cliente en la lista de esperasSrc.
                            int reservas = esperasSrc.Where(e => e.Cliente.Id == cliente.Id).Count();

                            // Comprobamos que el cliente tenga menos de 2 reservas.
                            if (esperasSrc.Where(e => e.Cliente.Id == cliente.Id).Count() < Cliente.MaxReservas)
                            {
                                // Creamos la reserva.
                                reserva = new(horario, cliente)
                                {
                                    Id = ""
                                };

                                // Guardamos la espera en la variable auxiliar para borrarla de la base de datos.
                                espera = es;
                                // Espera funciona como un flag para indicar que hemos encontrado un 
                            }

                            // Guardamos los cambios en la base de datos.
                            if (espera is not null)
                            {

                                // Añadimos la nueva reserva a la base de datos.
                                if (reserva != null)
                                {
                                    flag = await dbReserva.Create(reserva);
                                }

                                // Eliminamos la espera de la base de datos.
                                flag = await dbEspera.Delete(espera.Id.ToString());
                            }

                        }
                    });

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
