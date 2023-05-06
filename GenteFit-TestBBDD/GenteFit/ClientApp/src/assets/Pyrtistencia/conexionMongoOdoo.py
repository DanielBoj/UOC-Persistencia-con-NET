import xmlrpc.client
from pymongo import MongoClient

# Datos Odoo
url = 'http://20.126.4.248:086'
db = 'gentefit'
username = 'admin'
password = 'claptrap'

# Datos Mongodb
mongo_url = 'mongodb+srv://gentefit:WlNUIgyIrJUCccUo@persistencia-gentefit.ehftwrr.mongodb.net/admin'
mongo_db = 'gentefit'
mongo_collection = 'Cliente'

# Conexiónservidor Odoo
common = xmlrpc.client.ServerProxy('{}/xmlrpc/2/common'.format(url))
uid = common.authenticate(db, username, password, {})

models = xmlrpc.client.ServerProxy('{}/xmlrpc/2/object'.format(url))

# Conexión a MongoDB
mongo_client = MongoClient(mongo_url)
mongo_db = mongo_client[mongo_db]
mongo_collection = mongo_db[mongo_collection]

#Actualizando Odoo a partir de Mongo
for doc in mongo_collection.find():
    partner_data = {
        'id_Cliente': doc['id_Cliente'],
        'nombre': doc['nombre'],
        'apellido': doc['apellido'],
        'direccion': doc['direccion'],
        'edad': doc['edad']
    }
    partner_id = models.execute_kw(
        db, uid, password,
        'res.partner', 'search',
        [['id_Cliente', '=', doc['id_Cliente']]]
    )
    if partner_id:
        models.execute_kw(
            db, uid, password,
            'res.partner', 'write',
            [partner_id, partner_data]
        )
        print("Se ha actualizado correctamente el cliente de Odoo:", partner_id)
    else:
        partner_id = models.execute_kw(
            db, uid, password,
            'res.partner', 'create',
            [partner_data]
        )
        print("Se ha creado correctamente el cliente de Odoo:", partner_id)

# Actualizando Mongo a traves de Odoo
for doc in models.execute_kw(db, uid, password, 'res.partner', 'search_read', [[]]):
    customer_data = {
        'id_Cliente': doc['id_Cliente'],
        'nombre': doc['nombre'],
        'apellido': doc['apellido'],
        'direccion': doc['direccion'],
        'edad': doc['edad']
    }
    mongo_collection.update_one(
        {'id_Cliente': doc['id_Cliente']},
        {'$set': customer_data},
        upsert=True
    )
    print("Se ha actualizado correctamente el cliente de MongoDB:", doc['id_Cliente'])
