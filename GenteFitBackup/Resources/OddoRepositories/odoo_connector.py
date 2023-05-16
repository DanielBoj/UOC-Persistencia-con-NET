''' Este programa conecta con Odoo para obtener los datos de los clientes '''
# Importamos la librería para trabajar con Odoo
import odoorpc

# Creamos una clase que se encargará de conectarse con Odoo


class OdooConnector:
    # Definimos los valores de conexión
    db_name = 'gentefit'
    username = 'dboj@uoc.edu'
    password = 'claptrap'
    host = 'localhost'
    port = 8069

    # Constructor por defecto
    def __init__(self):
        self.common = None
        self.uid = None

    # Método para conectarse con Odoo
    def connect(self):
        # Nos conectamos a la url de Odoo
        self.odoo = odoorpc.ODOO(self.host, port=self.port)

        # Nos identificamos
        self.odoo.login(self.db_name, self.username, self.password)
        self.uid = self.odoo.env.uid

        # # Agregar la API Key al objeto `self.odoo`
        # api_key = '0c6889477b2a37e8f0732151414bd49eabe6b456'
        # self.odoo.session_id = api_key

    # Método de lectura de datos general, tendreos que pasar el modelo por parámetro
    def search_read(self, model_name, domain=[], fields=None, offset=0, limit=None, order=None, all_fields=True):
        model = self.odoo.env[model_name]
        return model.search_read(domain, fields or [], offset=offset, limit=limit or 0, order=order)

    # Método para crear objetos en Odoo
    def create(self, model_name, data):
        model = self.odoo.env[model_name]
        record_id = model.create(data)
        return record_id
