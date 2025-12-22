import { Component, ChangeDetectionStrategy, input, output, signal } from '@angular/core';

@Component({
  selector: 'app-payment-modal',
  templateUrl: './payment-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaymentModalComponent {
  isVisible = input.required<boolean>();
  totalAmount = input.required<number>();
  closeModal = output<void>();
  paymentSuccess = output<void>();

  isProcessing = signal(false);

  onClose() {
    if (this.isProcessing()) return;
    this.closeModal.emit();
  }

  async processPayment(event: Event) {
    event.preventDefault();
    if (this.isProcessing()) return;

    this.isProcessing.set(true);
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 2500));
    this.isProcessing.set(false);
    this.paymentSuccess.emit();
  }
}
