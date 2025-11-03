
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { PizzaCardComponent } from '../../components/pizza-card/pizza-card.component';
import { Pizza } from '../../models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, PizzaCardComponent, RouterLink]
})
export class HomeComponent {
  private apiService = inject(ApiService);
  topPizzas = toSignal(this.apiService.getTopPizzas(), { initialValue: [] as Pizza[] });
}
