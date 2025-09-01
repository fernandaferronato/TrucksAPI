import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

import { Truck } from './models/truck';
import { TruckService } from './services/truck.service';
import {TruckEditComponent } from './truckEdit.component';

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule
  ],
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './truckList.component.html',
    styles: [`
    .truck-table { width: 100%; margin-top: 1rem; }
    .truck-table th, .truck-table td { text-align: center; padding: 8px; }
    `]
})
export class TruckListComponent {
  @Input() trucks: Truck[] = [];
  @Output() onDelete = new EventEmitter<number>();
  @Output() onChangeColor = new EventEmitter<Truck>();

  constructor(private service: TruckService, private toastr: ToastrService, private dialog: MatDialog) {}

  displayedColumns: string[] = [
    'id',
    'model',
    'manufactureYear',
    'chassisId',
    'color',
    'plant',
    'actions'
  ];

  async deleteTruck(id: number) {
    this.onDelete.emit(id);
  } 
  
  editTruck(truck: Truck) {
    const dialogRef = this.dialog.open(TruckEditComponent, {
      width: '400px',
      data: { ...truck }
    });
  
    dialogRef.afterClosed().subscribe((result: Truck | null) => {
      console.log(result);

      if (result) {
        this.service.update(result)
        .then((updated) => {
          const index = this.trucks.findIndex(t => t.id === updated.id);
          if (index > -1) this.trucks[index] = updated;
          this.service.loadTrucks();
          this.toastr.success('Caminhão atualizado com sucesso!', '');
        })
        .catch(() => {
          this.toastr.error('Erro ao atualizar caminhão', 'Erro');
        });
    }
  });
}
}

