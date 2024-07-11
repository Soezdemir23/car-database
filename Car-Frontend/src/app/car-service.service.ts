import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CarInterface } from './car-interface';

@Injectable({
  providedIn: 'root'
})
export class CarServiceService {

  constructor(private http: HttpClient) {}
  
  getCars(){
    return this.http.get<CarInterface[]>("http://localhost:5100/cars");
  }

  getCarById(id:number){
    return this.http.get<CarInterface>(`http://localhost:5100/cars/${id}`);
  }

  updateCar(newCar: CarInterface, id: number){
    return this.http.put<CarInterface>(`http://localhost:5100/cars/${id}`, newCar);
  }

  createCar(newCar:CarInterface) {
    return this.http.post<CarInterface>("http://localhost:5100/cars", newCar);
  }

  removeCar(id: number){
    return this.http.delete(`http://localhost:5100/cars/${id}`);
  }


}