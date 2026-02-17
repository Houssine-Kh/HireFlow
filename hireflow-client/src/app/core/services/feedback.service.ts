import { Injectable, inject } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';

@Injectable({ providedIn: 'root' })
export class FeedbackService {
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  // --- TOASTERS ---

  success(detail: string, summary: string = 'Success') {
    this.messageService.add({ severity: 'success', summary, detail });
  }

  error(detail: string, summary: string = 'Error') {
    this.messageService.add({ severity: 'error', summary, detail });
  }

info(detail: string, summary: string = 'Info') {
    this.messageService.add({ severity: 'info', summary, detail });
  }

  // --- CONFIRMATION DIALOG ---
  
  // 
confirm(
    message: string, 
    onAccept: () => void, 
    header: string = 'Confirm Action',
    icon: string = 'pi pi-exclamation-triangle' 
  ) {
    this.confirmationService.confirm({
      message: message,
      header: header,
      icon: icon,
      acceptButtonStyleClass: 'p-button-danger p-button-text',
      rejectButtonStyleClass: 'p-button-text p-button-plain',
      accept: onAccept
    });
  }
}