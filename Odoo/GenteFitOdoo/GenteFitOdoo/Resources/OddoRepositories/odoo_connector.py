''' Este programa conecta con Odoo para obtener los datos de los clientes '''
# Importamos la librería para trabajar con Odoo
import odoorpc

# Creamos una clase que se encargará de conectarse con Odoo
class OdooConnector:
    # Definimos los valores de conexión
    db_name = 'gentefit'
    username = 'admin'
    password = 'claptrap'
    host = '20.126.4.248'
    port = 8069

    # Constructor por defecto
    def __init__(self):
        self.common = None;
        self.uid = None;

    # Método para conectarse con Odoo
    def connect(self):
        # Nos conectamos a la url de Odoo
        self.odoo = odoorpc.ODOO(self.host, port=self.port)
        # Nos identificamos
        self.odoo.login(self.db_name, self.username, self.password)
        self.uid = self.odoo.env.uid

    # Método de lectura de datos general, tendreos que pasar el modelo por parámetro
    def search_read(self, model_name, domain=[], fields=None, offset=0, limit=None, order=None):
        model = self.odoo.env[model_name]
        return model.search_read(domain, fields or [], offset=offset, limit=limit or 0, order=order)
