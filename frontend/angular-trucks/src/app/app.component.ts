import { map, Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

import { TruckService } from './services/truck.service';
import { Plant, Truck, TruckModel } from './models/truck';
import { TruckListComponent } from './truckList.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule, CommonModule, TruckListComponent,MatFormFieldModule,
  MatInputModule,
  MatSelectModule,
  MatTableModule,
  MatButtonModule],

  templateUrl: './app.component.html',
  styles: [`
    .container { max-width: 600px; margin: auto; padding: 20px; }
    .form input { margin: 5px; }
    ul { list-style: none; padding: 0; }
    li { margin: 5px 0; }
  `]
})
export class AppComponent implements OnInit {
  
  // List of trucks, provided by TruckService
  trucks$: Observable<Truck[]>;

  newTruck: Truck = { chassisId: '', model: null, manufactureYear: new Date().getFullYear(), color: '', plant: null };
  
  // Stores the chassis value used for searching
  searchChassis: string = '';
  // Filtered list of trucks (search results)
  filteredTruck$: Observable<Truck[]> | null;

  // Arrays of truck models and plants converted from enums to strings
  truckModels = Object.keys(TruckModel).filter(key => isNaN(Number(key)));
  truckPlant = Object.keys(Plant).filter(key => isNaN(Number(key)));

  constructor(private service: TruckService, private toastr: ToastrService) {
    this.trucks$ = this.service.trucks$;
    this.filteredTruck$ = null;
  }

  displayedColumns: string[] = ['chassisId', 'model', 'manufactureYear', 'color', 'plant', 'actions'];

  async ngOnInit() {
    await this.service.loadTrucks();
    this.filteredTruck$ = null;
  }

  async addTruck(form: NgForm) {
    if (form.invalid) {
      console.log('Por favor, preencha todos os dados obrigatórios.', 'Erro de Validação');
      this.toastr.error('Por favor, preencha todos os dados obrigatórios', 'Erro de Validação');
      return;
    }

    // Checks if a truck with the same chassis already exists
    this.trucks$.pipe(
      map(trucksList => trucksList.some(t => t.chassisId === this.newTruck.chassisId))
    ).subscribe(exists => {
      if (exists) {
        this.toastr.error('Já existe um caminhão com este Chassi.', 'Erro de Validação');
        return;
      }
    });

    await this.service.create(this.newTruck);
    this.toastr.success('Caminhão cadastrado com sucesso!', '');
    // Resets the form
    this.newTruck = { chassisId: '', model: null, manufactureYear: new Date().getFullYear(), color: '', plant: null };
    this.searchChassis = '';
    this.filteredTruck$ = null;
    await this.service.loadTrucks();
  }

  searchTruckByChassis() {
    if (!this.searchChassis) {
      this.searchChassis = '';
      this.filteredTruck$ = null;
      return;
  }

  this.filteredTruck$ = this.service.getByChassis(this.searchChassis);
  this.filteredTruck$.subscribe(trucks => {
    if (!trucks || trucks.length === 0) {
      this.toastr.info('Nenhum caminhão foi encontrado.', '');
    }
  });
}

  // Clears the search input and removes the filter
  clearSearch() {
  this.searchChassis = '';
  this.filteredTruck$ = null;
  }

  async deleteTruck(id: number) {
    try {
      await this.service.delete(id); 
      this.toastr.success('Caminhão deletado com sucesso!', '');
    }
    catch (error) {
      this.toastr.error('Erro ao deletar caminhão', 'Erro');
    }
    await this.service.loadTrucks();
  }
}

