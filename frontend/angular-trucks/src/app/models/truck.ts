export interface Truck {
  id?: number;
  model: TruckModel | null;
  manufactureYear: number;
  chassisId: string;
  color: string;
  plant: Plant | null;
}

export enum TruckModel {
  
  FH,
  FM,
  VM 
}

export enum Plant {
  
  Brasil,
  Suécia,
  USA,
  França
}