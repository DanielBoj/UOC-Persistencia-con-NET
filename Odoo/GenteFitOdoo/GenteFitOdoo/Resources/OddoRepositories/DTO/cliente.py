''' Clase para implementar el objeto de transición entro Odoo y JSON'''


class ClienteData:

    # Constructor
    def __init__(self, id, name, email, phone, street, _zip, city, state_id, country_id, vat):
        self.id = id
        self.name = name
        self.email = email
        self.phone = phone
        self.street = street
        self.zip = _zip
        self.city = city
        self.vat = vat
        # Inicializamos provincia y país a None para obtener luego los datos desde Odoo
        self.state = None
        self.country = None

        # Odoo devuelve una tupla con el id y el nombre del país pero con un objeto del tipo bool, así
        # que primero hemos de comprobar que no sea un objeto bool y luego asignar el nombre de la
        # provincia y del país
        if state_id:
            self.state = state_id[1] if state_id[1] else None

        if country_id:
            self.country = country_id[1] if country_id[1] else None
