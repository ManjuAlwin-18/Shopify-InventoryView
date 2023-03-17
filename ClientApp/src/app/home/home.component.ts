import { Component } from '@angular/core';
import { InventoryService } from './inventory.service';
import { InventoryArticle } from '../inventoryArticle.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  cron = null;
  inventory: InventoryArticle[];

  /**
   * Constructor
   * @param appService
   *
   * injects appService
   */
  constructor(private inventoryService: InventoryService) { }

  /**
   * ngOnInit
   *
   * starts interval that calles method getInventory() each minute,
   * when component is initialized
   *
   * */
  ngOnInit() {
    this.cron = setInterval(() => {
      this.getInventory();
    }, 1000);
  }

  /**
   *
   * ngOnDestroy
   *
   * stops interval, when component is no longer existing
   *
   * */
  ngOnDestroy() {
    if (this.cron) clearInterval(this.cron);
  }

  /**
   *
   * getInventory()
   *
   * calls getInventory() from AppService. Subscribes to said method. When data is retrieved
   * subscription overwrites this.inventory with its data.
   *
   * */
  getInventory() {
    this.inventoryService.getInventory()
      .subscribe(inventory => this.inventory = inventory);
  }
}
