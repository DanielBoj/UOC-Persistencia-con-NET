''' Clase para implementar el objeto de transición entro Odoo y JSON '''


class EmpleadoData:

    # Constructor
    def __init__(self, id, name, email, job_id, department_id):
        self.id = id
        self.name = name
        self.email = email
        self.department = None

        if department_id:
            self.department = department_id[1] if department_id[1] else None
