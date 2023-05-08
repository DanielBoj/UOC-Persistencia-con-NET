''' Clase para implementar el objeto de transición entro Odoo y JSON'''


class ProveedorData:

    # Constructor
    def __init__(self, id, name, email, phone, website, street, city, _zip, state_id, country_id, company_id, vat):
        self.id = id
        self.name = name
        self.email = email
        self.phone = phone
        self.website = website
        self.street = street
        self.city = city
        self.zip = _zip
        self.state = None
        self.country = None
        self.company = None
        self.vat = vat

        # Odoo devuelve una tupla con el id y el nombre del país pero con un objeto del tipo bool, así
        # que primero hemos de comprobar que no sea un objeto bool y luego asignar el nombre de la
        # provincia y del país
        if state_id:
            self.state = state_id[1] if state_id[1] else None

        if country_id:
            self.country = country_id[1] if country_id[1] else None

        if company_id:
            self.company = company_id[1] if company_id[1] else None
