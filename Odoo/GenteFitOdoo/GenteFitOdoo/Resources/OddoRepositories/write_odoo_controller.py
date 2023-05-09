''' Este script se encarga de escribir los datos de las entidades de Odoo desde la app de GenteFit '''
# Importamos la librería para pasar los resultados a JSON
import json
# Importamos la clase de conexión que hemos creado
from odoo_connector import OdooConnector
# Importamos las clases DTO que hemos creado
from DTO.cliente import ClienteData
from DTO.producto import ProductoData
from DTO.proveedor import ProveedorData
# Importamos una librería para facilitar la conversión de los NIF
import vatnumber

# Creamos una instacia de la clase OdooConnector
connector = OdooConnector()
# Nos conectamos a Odoo
connector.connect()

# Función para crear un cliente


def createCliente(cliente):
    # Configuramos el NIF para que Odoo lo acepte
    # Obtenemos el número del NIF
    cliente['vat'] = cliente['vat'][:8]
    # Añadimos el prefijo del país
    cliente['vat'] = f'ESA{cliente["vat"]}'

    # Buscamos el id del país
    country_id = connector.search_read("res.country", [
        ("name", "=", cliente['country'])])[0]['id'] if connector.search_read("res.country", [("name", "=", cliente['country'])]) else 1

    # Guardamos el cliente que hemos creado en Odoo
    cliente_id = connector.create('res.partner', {
        'customer_rank': 1,
        'name': cliente['name'],
        'email': cliente['email'],
        'phone': cliente['phone'],
        'street': cliente['street'],
        'zip': cliente['zip'],
        'city': cliente['city'],
        'country_id': country_id,
        # 'vat': cliente['vat'],
    })

    # Devolvemos el id del cliente creado
    return cliente_id

# Función para crear un producto


def createProducto(producto):
    # Buscamos el id de la categoría
    categ_ids = connector.search_read("product.category", [("name", "=", producto['categ'])])[0]['id'] if connector.search_read(
        "product.category", [("name", "=", producto['categ'])]) else 1

    # Guardamos el producto que hemos creado en Odoo
    producto_id = connector.create('product.product', {
        'name': producto['name'],
        'default_code': producto['default_code'],
        'list_price': producto['list_price'],
        'standard_price': producto['standard_price'],
        'categ_id': categ_ids,
        'type': 'consu',
    })

    # Devolvemos el id del producto creado
    return producto_id

# Función para crear un proveedor


def createProveedor(proveedor):
    # Buscamos el id del país
    country_id = connector.search_read("res.country", [
        ("name", "=", proveedor['country'])])[0]['id'] if connector.search_read("res.country", [("name", "=", proveedor['country'])]) else 1

    # Guardamos el proveedor en Odoo
    proveedor_id = connector.create('res.partner', {
        'supplier_rank': 1,
        'name': proveedor['name'],
        'email': proveedor['email'],
        'website': proveedor['website'],
        'phone': proveedor['phone'],
        'street': proveedor['street'],
        'zip': proveedor['zip'],
        'city': proveedor['city'],
        'country_id': country_id,
        'vat': proveedor['vat'],
    })

    return proveedor_id
