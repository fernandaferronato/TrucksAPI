import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { Plant, Truck, TruckModel } from './models/truck';

@Component({
  selector: 'app-truck-edit-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatDialogModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  templateUrl: './truckEdit.component.html',
  styles: [`
    .w-full { width: 100%; margin-top: 10px; }
  `]
})
export class TruckEditComponent {

  truckModels = Object.keys(TruckModel).filter(key => isNaN(Number(key)));
  truckPlant = Object.keys(Plant).filter(key => isNaN(Number(key)));
  
  constructor(
    public dialogRef: MatDialogRef<TruckEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Truck
  ) {}

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onSave(): void {
    this.dialogRef.close(this.data);
  }
}
