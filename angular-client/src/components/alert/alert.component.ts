import { Component, ChangeDetectionStrategy, input, output, effect, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertComponent implements OnDestroy {
  message = input.required<string>();
  type = input.required<'success' | 'error'>();
  close = output<void>();

  private timeoutId: any;

  constructor() {
    effect((onCleanup) => {
      this.timeoutId = setTimeout(() => {
        this.close.emit();
      }, 5000);

      onCleanup(() => {
        clearTimeout(this.timeoutId);
      });
    });
  }

  ngOnDestroy() {
    clearTimeout(this.timeoutId);
  }
}
