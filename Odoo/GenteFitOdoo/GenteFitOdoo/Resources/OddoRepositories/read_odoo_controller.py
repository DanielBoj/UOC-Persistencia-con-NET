''' Este script se encarga de importar los datos de las entidades de Odoo a la app de GenteFit '''
# Importamos la librería para pasar los resultados a JSON
import json
# Importamos la clase de conexión que hemos creado
from odoo_connector import OdooConnector
# Importamos las clases DTO que hemos creado
from DTO.cliente import ClienteData
from DTO.producto import ProductoData
from DTO.proveedor import ProveedorData

# Creamos una instancia de la clase OdooConnector
connector = OdooConnector()
# Nos conectamos a Odoo
connector.connect()

# Función para retornar los clientes


def getClientes():
    # Obtenemos los datos de los clientes
    clientes = connector.search_read('res.partner')

    # Creamos una lista vacía para almacenar los clientes
    clientes_list = []

    # Recorremos los clientes obtenidos y filtramos sus datos
    # necesitamos comprobar que el partner recuperado sea un cliente
    # para ello, usamos el campo customer, que es un booleano que debe de
    # estar en True.
    for cliente in clientes:
        if cliente['customer_rank']:
            # Creamos un cliente filtrado
            cliente = ClienteData(cliente['id'], cliente['name'], cliente['email'], cliente['phone'],
                                  cliente['street'], cliente['zip'], cliente['city'], cliente['state_id'], cliente['country_id'], cliente['vat'])
            # Añadimos el cliente a la lista
            clientes_list.append(cliente)

    # Convertimos la lista de clientes a JSON
    clientes_json = json.dumps(
        clientes_list, default=lambda o: o.__dict__, sort_keys=True, indent=4)
    # Devolvemos el JSON
    return clientes_json

# Función para retornar los productos


def getProductos():
    # Obtenemos los datos de los productos
    productos = connector.search_read('product.product')

    # Creamos una lista vacía para almacenar los productos
    productos_list = []

    # Recorremos los productos obtenidos y filtramos sus datos
    for producto in productos:
        # Creamos un producto filtrado
        producto = ProductoData(producto['id'], producto['name'], producto['default_code'],
                                producto['list_price'], producto['standard_price'], producto['categ_id'])
        # Añadimos el producto a la lista
        productos_list.append(producto)

    # Convertimos la lista de productos a JSON
    productos_json = json.dumps(
        productos_list, default=lambda o: o.__dict__, sort_keys=True, indent=4)

    # Devolvemos el JSON
    return productos_json

# Función para retornar los proveedores


def getProveedores():
    # Obtenemos los datos de los proveedores
    proveedores = connector.search_read('res.partner')

    # Creamos una lista vacía para almacenar los proveedores
    proveedores_list = []

    # Recorremos los proveedores obtenidos y filtramos sus datos
    # Tenemos que comprobar que el partner recuperado sea un proveedor
    # para ello, usamos el campo supplier, que es un booleano que debe de
    # estar en True.
    for proveedor in proveedores:
        # Comprobamos que el partner sea un proveedor
        if not proveedor['customer_rank'] and proveedor['supplier_rank']:
            # Creamos un proveedor filtrado
            proveedor = ProveedorData(proveedor['id'], proveedor['name'], proveedor['email'], proveedor['phone'], proveedor['website'],
                                      proveedor['street'], proveedor['city'], proveedor['zip'], proveedor['state_id'], proveedor['country_id'], proveedor['company_id'], proveedor['vat'])

            # Añadimos el proveedor a la lista
            proveedores_list.append(proveedor)

    # Convertimos la lista de proveedores a JSON
    proveedores_json = json.dumps(
        proveedores_list, default=lambda o: o.__dict__, sort_keys=True, indent=4)

    # Devolvemos el JSON
    return proveedores_json
