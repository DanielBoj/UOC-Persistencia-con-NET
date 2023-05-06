import xml.etree.ElementTree as ET
import xmlrpc.client

url = 'http...'
DB='mongo'
USER='admin@gentefit.com'
PASS='claptrap'

commo_proxy = xmlrpc.client.ServerProxy(url+'common')
objecet = xmlrpc.clien.ServerProxy(url+'object')
uid = common_proxy.login(DB,USER,PASS)

if uid:

  archivo_xml = ET.parse('Vendedores.xml')
  raiz = archivo_xml.getroot()
  for subraiz in raiz:
    do_write = object.execute(DB,uid,PASS,'x_vendedor','create',[{
      'x_idVendedor':subraiz[0].text,
      'x_nombre':subraiz[1].text,
      'x_apelido':subraiz[2].text,
      'x_producto':subraiz[3].text
      }])
    print('Vendedores cargados correctamente')
else:
  print('Error al conectar a odoo')

