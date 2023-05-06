''' Clase para implementar el objeto de transición entro Odoo y JSON'''
class ProductoData:

    # Constructor
    def __init__(self, id, name, default_code, list_price, standard_price, seller_ids, categ_id):
        self.id = id
        self.name = name
        self.default_code = default_code
        self.list_price = list_price
        self.standard_price = standard_price
        self.seller = None
        self.categ = None

        # Odoo devuelve una tupla con el id y el nombre de la categoría o del país, pero parte de un objeto bool
        if seller_ids:
            self.seller = seller_ids[1] if seller_ids[1] else None

        if categ_id:
            self.categ = categ_id[1] if categ_id[1] else None
