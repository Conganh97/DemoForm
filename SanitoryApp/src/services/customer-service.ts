import http from '../https-common'
import ICustomer from '../models/customer'
import IFormRegister from "@/models/FormRegister.ts";
import IFormLogin from "@/models/FormLogin.ts";
import IUser from "@/models/user.ts";

class CustomerService {
  getAll() {
    return http.get<Array<ICustomer>>('/customers')
  }

  get(id: string) {
    return http.get<ICustomer>(`/customers/${id}`)
  }

  create(data: IFormRegister) {
    return http.post<IFormRegister>('/customers', data)
  }

  update(data: ICustomer, id: any) {
    return http.put<any>(`/customers/${id}`, data)
  }

  delete(id: any) {
    return http.delete<any>(`/customers/${id}`)
  }

  deleteAll() {
    return http.delete<any>(`/customers`)
  }

  findByTitle(title: string) {
    return http.get<Array<ICustomer>>(`/customers?title=${title}`)
  }

  login(data: IFormLogin) {
    return http.post<IUser>('/customers/login', data)
  }
}

export default new CustomerService()
