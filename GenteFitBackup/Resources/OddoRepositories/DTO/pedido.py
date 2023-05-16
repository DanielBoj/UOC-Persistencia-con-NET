''' Clase para implementar el objeto de transición entro Odoo y JSON  ''' 

class PedidoData:
     
     def __init__(self, id, write_date, amount_total, amount_untaxed, company_id, date_order, delivery_status, display_name, partner_id, tax_totals):
        self.id = id
        self.write_date = write_date
        self.amount_total = amount_total
        self.amount_untaxed = amount_untaxed
        self.company = None
        self.date_order = date_order
        self.delivery_status = delivery_status
        self.display_name = display_name
        self.partner = None
        self.tax_group_name = 'IVA 21%'

        if company_id:
            self.company = company_id[1] if company_id[1] else None

        if partner_id:
            self.partner = partner_id[1] if partner_id[1] else None