import { Injectable } from '@angular/core';
import axios from 'axios';
import { Truck } from '../models/truck';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TruckService {
  private apiUrl = 'http://localhost:5249/api/trucks';
  private trucksSubject = new BehaviorSubject<Truck[]>([]);
  public trucks$ = this.trucksSubject.asObservable();
  
  constructor(private http: HttpClient) { }


  async getAll(): Promise<Truck[]> {
    const res = await axios.get<Truck[]>(this.apiUrl);
    return res.data;
  }

    async loadTrucks() {
    const trucks = await this.getAll();
    this.trucksSubject.next(trucks);
  }

  getByChassis(chassisId: string): Observable<Truck[]> {
   return this.http.get<Truck[]>(`${this.apiUrl}/by-chassis/${chassisId}`);
  }

  async create(truck: Truck): Promise<Truck> {
    const res = await axios.post<Truck>(this.apiUrl, truck);
    return res.data;
  }

  async update(truck: Truck): Promise<Truck> {
    const res = await axios.put<Truck>(`${this.apiUrl}/${truck.id}`, truck);
    return res.data;
}

  async delete(id: number): Promise<void> {
    await axios.delete(`${this.apiUrl}/${id}`);
  }
}
