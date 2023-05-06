''' API para crear los endpoints que llamaremos desde ASP.NET '''
# Importamos la librería para crear la API y enviar objetos JSON
from flask import Flask
import json
# Importamos nuestro módulos read_odoo_controller
from read_odoo_controller import getClientes, getProductos, getProveedores


app = Flask(__name__)

# Obtenemos los clientes mediante nuestro controlador


@app.route('/clientes')
def get_clientes():
    # Llamamos a nuesto controlador
    clientes_list = getClientes()

    return clientes_list


# Obtenemos los productos mediante nuestro controlador
@app.route('/productos')
def get_productos():
    # LLamamos a neustro controlador
    productos_list = getProductos()

    return productos_list

# Obtenemos los proveedores mediante nuestro controlador


@app.route('/proveedores')
def get_proveedores():
    # Llamamos a nuestro controlador
    proveedores_list = getProveedores()

    return proveedores_list


# Ejecutamos la aplicación
if __name__ == '__main__':
    app.run(port=5005, debug=True)
