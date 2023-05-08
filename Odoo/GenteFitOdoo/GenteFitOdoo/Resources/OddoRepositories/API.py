''' API para crear los endpoints que llamaremos desde ASP.NET '''
# Importamos la librería para crear la API y enviar objetos JSON
from flask import Flask, request
import json
# Importamos nuestras clases de conexión y controladores
from odoo_connector import OdooConnector
from read_odoo_controller import getClientes, getProductos, getProveedores
from write_odoo_controller import createCliente, createProducto, createProveedor


app = Flask(__name__)

# Obtenemos los clientes mediante nuestro controlador
# Clientes


@app.route('/clientes')
def get_clientes():
    # Llamamos a nuesto controlador
    clientes_list = getClientes()

    return clientes_list

# Creamos un cliente en Odoo
# Usamos un decorador para indicar que vamos a realizar la acción POST


@app.route('/clientes', methods=['POST'])
def create_cliente():
    # Obtenemos los datos del cliente desde el body del request
    cliente = request.json

    # Llamamos a la función de nuestro controlador
    cliente_id = createCliente(cliente)

    # Devolvemos el id del cliente creado en un response
    return json.dumps({'cliente_id': cliente_id})

# Productos
# Obtenemos los productos mediante nuestro controlador


@app.route('/productos')
def get_productos():
    # Llamamos a neustro controlador
    productos_list = getProductos()

    return productos_list

# Creamos un producto en Odoo
# Usamos un decorador para indicar que vamos a realizar la acción POST


@app.route('/productos', methods=['POST'])
def create_producto():
    # Obtenemos los datos del cliente desde el body del request
    producto = request.json

    # Llamamos a la función de nuestro controlador
    producto_id = createProducto(producto)

    # Devolvemos el id del producto creado en un response
    return json.dumps({'producto_id': producto_id})

# Proveedores
# Obtenemos los proveedores mediante nuestro controlador


@app.route('/proveedores')
def get_proveedores():
    # Llamamos a nuestro controlador
    proveedores_list = getProveedores()

    return proveedores_list

# Creamos un proveedor en Odoo
# Usamos un decorador para indicar que vamos a realizar la acción POST


@app.route('/proveedores', methods=['POST'])
def create_proveedor():
    # Obtenemos los datos del proveedor desde el body del request
    proveedor = request.json

    # Llamamos a la función de nuestro controlador
    proveedor_id = createProveedor(proveedor)

    # Devolvemos el id del proveedor creado en un response
    return json.dumps({'proveedor_id': proveedor_id})


# Ejecutamos la aplicación
if __name__ == '__main__':
    app.run(port=5005, debug=True)

    # Abrimos la conexión con Odoo
    connector = OdooConnector()
    connector.connect()

    # Cerramos la conexión con Odoo
    connector.disconnect()
