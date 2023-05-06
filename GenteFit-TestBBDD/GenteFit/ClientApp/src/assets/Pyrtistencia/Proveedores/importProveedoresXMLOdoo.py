import os
import xml.etree.ElementTree as ET

def importProveedoresXMLOdoo():
    archivo_xml = ET.parse('Proveedores.xml')
    raiz = archivo_xml.getroot()
    for subraiz in raiz:
        proveedor = {
            'id_Proveedor' : subraiz[0].text,
            'empresa' : subraiz[1].text,
            'cif' : subraiz[2].text,
            'producto' : subraiz[3].text
        }
        print(proveedor)

importProveedoresXMLOdoo()
print('ProveedorExportado')
