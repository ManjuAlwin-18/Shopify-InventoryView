import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InventoryArticle } from '../inventoryArticle.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {


  constructor(private http: HttpClient) { }

  getInventory(): Observable<InventoryArticle[]> {
    return this.http.get<InventoryArticle[]>(environment.apiurl);
  }

}
