// Esta interfaz modeliza los datos de la superclase Usuario.

export interface User {
  id?: string,
  email: string,
  pass: string,
  tipo: string
}

export interface UserResponse {
  id?: string,
  tipo: string,
}

export interface UserBody {
  email: string,
  password: string,
}
