import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CarInterface } from './car-interface';

@Injectable({
  providedIn: 'root'
  
})


export class CarServiceService {

  constructor(private http: HttpClient) {}
  
  getCars(){
    return this.http.get<CarInterface[]>("https://cars-database.tryasp.net/cars");
  }

  getCarById(id:number){
    return this.http.get<CarInterface>(`https://cars-database.tryasp.net/cars/${id}`);
  }

  updateCar(newCar: CarInterface, id: number){
    return this.http.put<CarInterface>(`https://cars-database.tryasp.net/cars/${id}`, newCar);
  }

  createCar(newCar:CarInterface) {
    return this.http.post<CarInterface>("https://cars-database.tryasp.net/cars", newCar);
  }

  removeCar(id: number){
    return this.http.delete<CarInterface>(`https://cars-database.tryasp.net/cars/${id}`);
  }


}