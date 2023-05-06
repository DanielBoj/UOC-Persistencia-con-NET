import os
import xml.etree.ElementTree as ET

def importVendedorXMLOdoo():
    archivo_xml = ET.parse('Vendedores.xml')
    raiz = archivo_xml.getroot()
    for subraiz in raiz:
        vendedor = {
            'id_Vendedor' : subraiz[0].text,
            'nombre' : subraiz[1].text,
            'apellido' : subraiz[2].text,
            'producto' : subraiz[3].text
        }
        print(vendedor)

importVendedorXMLOdoo()
print('VendedorExportado')
